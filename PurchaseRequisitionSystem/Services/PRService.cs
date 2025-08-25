using Microsoft.EntityFrameworkCore;
using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.Services
{
    public class PRService : IPRService
    {
        private readonly ApplicationDbContext _context;
        private readonly IApprovalService _approvalService;
        private readonly IWebHostEnvironment _environment;

        public PRService(ApplicationDbContext context, IApprovalService approvalService, IWebHostEnvironment environment)
        {
            _context = context;
            _approvalService = approvalService;
            _environment = environment;
        }

        public async Task<string> GeneratePRNumberAsync()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            
            var lastPR = await _context.PurchaseRequisitions
                .Where(pr => pr.CreatedDate.Year == currentYear && pr.CreatedDate.Month == currentMonth)
                .OrderByDescending(pr => pr.Id)
                .FirstOrDefaultAsync();

            int sequenceNumber = 1;
            if (lastPR != null)
            {
                var lastNumber = lastPR.PRNumber.Split('-').LastOrDefault();
                if (int.TryParse(lastNumber, out int lastSeq))
                {
                    sequenceNumber = lastSeq + 1;
                }
            }

            return $"PR-{currentYear:yyyy}-{currentMonth:D2}-{sequenceNumber:D4}";
        }

        public async Task<PurchaseRequisition> CreatePRAsync(PurchaseRequisition pr, List<IFormFile>? attachments)
        {
            pr.PRNumber = await GeneratePRNumberAsync();
            pr.CreatedDate = DateTime.UtcNow;
            pr.CurrentStatus = ApprovalStatus.Initiated;

            // Get first approver
            var nextApprover = await _approvalService.GetNextApproverAsync(pr);
            if (nextApprover != null)
            {
                pr.CurrentApproverId = nextApprover;
                pr.CurrentStatus = ApprovalStatus.PendingHODInitiator;
            }

            _context.PurchaseRequisitions.Add(pr);
            await _context.SaveChangesAsync();

            // Save attachments if any
            if (attachments != null && attachments.Count > 0)
            {
                await SaveAttachmentsAsync(pr.Id, attachments, pr.InitiatorId);
            }

            // Create initial approval history entry
            var initialHistory = new ApprovalHistory
            {
                PurchaseRequisitionId = pr.Id,
                ApproverId = pr.InitiatorId,
                Status = ApprovalStatus.Initiated,
                Comments = "Purchase Requisition initiated",
                Level = 0
            };

            _context.ApprovalHistories.Add(initialHistory);
            await _context.SaveChangesAsync();

            return pr;
        }

        public async Task<List<PurchaseRequisition>> GetUserRequestsAsync(string userId)
        {
            return await _context.PurchaseRequisitions
                .Include(pr => pr.Facility)
                .Include(pr => pr.CurrentApprover)
                .Where(pr => pr.InitiatorId == userId)
                .OrderByDescending(pr => pr.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<PurchaseRequisition>> GetPendingApprovalsAsync(string userId)
        {
            return await _context.PurchaseRequisitions
                .Include(pr => pr.Initiator)
                .Include(pr => pr.Facility)
                .Where(pr => pr.CurrentApproverId == userId 
                            && pr.CurrentStatus != ApprovalStatus.Approved 
                            && pr.CurrentStatus != ApprovalStatus.Rejected)
                .OrderBy(pr => pr.CreatedDate)
                .ToListAsync();
        }

        public async Task<PurchaseRequisition?> GetPRByIdAsync(int id, bool includeAll = false)
        {
            var query = _context.PurchaseRequisitions
                .Include(pr => pr.Initiator)
                .Include(pr => pr.Facility)
                .Include(pr => pr.CurrentApprover);

            if (includeAll)
            {
                query = query
                    .Include(pr => pr.ApprovalHistory)
                        .ThenInclude(ah => ah.Approver)
                    .Include(pr => pr.Attachments)
                        .ThenInclude(att => att.UploadedByUser)
                    .Include(pr => pr.DelegationHistory)
                        .ThenInclude(dh => dh.FromUser)
                    .Include(pr => pr.DelegationHistory)
                        .ThenInclude(dh => dh.ToUser);
            }

            return await query.FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task SaveAttachmentsAsync(int prId, List<IFormFile> attachments, string uploadedBy)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "pr", prId.ToString());
            Directory.CreateDirectory(uploadPath);

            foreach (var file in attachments)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadPath, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var attachment = new PRAttachment
                    {
                        PurchaseRequisitionId = prId,
                        FileName = file.FileName,
                        FilePath = $"/uploads/pr/{prId}/{fileName}",
                        FileType = file.ContentType,
                        FileSize = file.Length,
                        UploadedBy = uploadedBy
                    };

                    _context.PRAttachments.Add(attachment);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}