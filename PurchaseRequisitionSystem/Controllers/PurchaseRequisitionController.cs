using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseRequisitionSystem.Models;
using PurchaseRequisitionSystem.Services;
using PurchaseRequisitionSystem.ViewModels;

namespace PurchaseRequisitionSystem.Controllers
{
    [Authorize]
    public class PurchaseRequisitionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPRService _prService;
        private readonly IApprovalService _approvalService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseRequisitionController(
            ApplicationDbContext context,
            IPRService prService,
            IApprovalService approvalService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _prService = prService;
            _approvalService = approvalService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var userRequests = await _prService.GetUserRequestsAsync(currentUser.Id);
            return View(userRequests);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreatePRViewModel
            {
                Facilities = await _context.FacilityMasters.Where(f => f.IsActive).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePRViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                var pr = new PurchaseRequisition
                {
                    InitiatorId = currentUser.Id,
                    FacilityId = model.FacilityId,
                    RequestType = model.RequestType,
                    ExpenseType = model.ExpenseType,
                    OpexCategory = model.OpexCategory,
                    IsBudgeted = model.IsBudgeted,
                    CostEstimation = model.CostEstimation,
                    ItemCode = model.ItemCode,
                    ItemDescription = model.ItemDescription,
                    Specification = model.Specification,
                    Justification = model.Justification,
                    Quantity = model.Quantity,
                    UnitOfMeasure = model.UnitOfMeasure,
                    Department = model.Department
                };

                await _prService.CreatePRAsync(pr, model.Attachments);
                
                TempData["Success"] = "Purchase Requisition created successfully!";
                return RedirectToAction(nameof(Details), new { id = pr.Id });
            }

            model.Facilities = await _context.FacilityMasters.Where(f => f.IsActive).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pr = await _prService.GetPRByIdAsync(id, true);
            if (pr == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // Check if user can view this PR
            if (pr.InitiatorId != currentUser.Id && 
                pr.CurrentApproverId != currentUser.Id && 
                !await CanUserViewPR(currentUser, pr))
            {
                return Forbid();
            }

            var timelineModel = new PRTimelineViewModel
            {
                PurchaseRequisition = pr,
                ApprovalHistory = pr.ApprovalHistory.OrderBy(ah => ah.ActionDate).ToList(),
                DelegationHistory = pr.DelegationHistory.OrderBy(dh => dh.DelegatedDate).ToList(),
                Attachments = pr.Attachments.ToList()
            };

            return View(timelineModel);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var pr = await _prService.GetPRByIdAsync(id);
            if (pr == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (!await _approvalService.CanUserApproveAsync(currentUser.Id, id))
            {
                return Forbid();
            }

            var model = new ApprovalActionViewModel
            {
                PurchaseRequisitionId = id,
                PurchaseRequisition = pr,
                DelegateUsers = await _approvalService.GetEligibleDelegateUsersAsync(currentUser.Id, currentUser.Role)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessApproval(ApprovalActionViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (!await _approvalService.CanUserApproveAsync(currentUser.Id, model.PurchaseRequisitionId))
            {
                return Forbid();
            }

            var success = await _approvalService.ProcessApprovalAsync(
                model.PurchaseRequisitionId,
                currentUser.Id,
                model.Action,
                model.Comments,
                model.DelegateToUserId,
                model.RejectionReason);

            if (success)
            {
                TempData["Success"] = $"Purchase Requisition {model.Action}d successfully!";
                return RedirectToAction("Dashboard", "Home");
            }

            TempData["Error"] = "Failed to process approval action.";
            return RedirectToAction(nameof(Approve), new { id = model.PurchaseRequisitionId });
        }

        private async Task<bool> CanUserViewPR(ApplicationUser user, PurchaseRequisition pr)
        {
            // Users can view PRs if they are in the approval chain or have admin role
            if (user.Role == UserRole.Admin) return true;

            var hasApprovalHistory = await _context.ApprovalHistories
                .AnyAsync(ah => ah.PurchaseRequisitionId == pr.Id && ah.ApproverId == user.Id);

            return hasApprovalHistory;
        }
    }
}