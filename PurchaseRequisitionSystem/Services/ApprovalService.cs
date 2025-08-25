using Microsoft.EntityFrameworkCore;
using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly ApplicationDbContext _context;

        public ApprovalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetNextApproverAsync(PurchaseRequisition pr)
        {
            // Check if hierarchy master exists for this facility/department
            var hierarchyMaster = await _context.HierarchyMasters
                .Where(h => h.FacilityId == pr.FacilityId 
                           && h.Department == pr.Department 
                           && h.ExpenseType == pr.ExpenseType 
                           && h.IsActive)
                .OrderBy(h => h.Level)
                .FirstOrDefaultAsync();

            if (hierarchyMaster != null)
            {
                return await GetNextApproverFromHierarchy(pr, hierarchyMaster);
            }
            else
            {
                return await GetNextApproverFromDefaultRouting(pr);
            }
        }

        private async Task<string?> GetNextApproverFromHierarchy(PurchaseRequisition pr, HierarchyMaster hierarchyMaster)
        {
            // Find current level in approval process
            var currentLevel = GetCurrentApprovalLevel(pr.CurrentStatus);
            
            // Get next level hierarchy
            var nextHierarchy = await _context.HierarchyMasters
                .Where(h => h.FacilityId == pr.FacilityId 
                           && h.Department == pr.Department 
                           && h.ExpenseType == pr.ExpenseType 
                           && h.Level > currentLevel 
                           && h.IsActive)
                .OrderBy(h => h.Level)
                .FirstOrDefaultAsync();

            if (nextHierarchy?.ApproverUserId != null)
            {
                // Check for active delegations
                var delegation = await _context.DelegationHistories
                    .Where(d => d.FromUserId == nextHierarchy.ApproverUserId 
                               && d.IsActive 
                               && (d.ExpiryDate == null || d.ExpiryDate > DateTime.UtcNow))
                    .FirstOrDefaultAsync();

                return delegation?.ToUserId ?? nextHierarchy.ApproverUserId;
            }

            return null;
        }

        private async Task<string?> GetNextApproverFromDefaultRouting(PurchaseRequisition pr)
        {
            // Default approval flow based on current status
            switch (pr.CurrentStatus)
            {
                case ApprovalStatus.Initiated:
                    // Find HOD of initiator
                    var initiator = await _context.Users.FindAsync(pr.InitiatorId);
                    return await FindUserByRole(UserRole.HOD, initiator?.Department);

                case ApprovalStatus.PendingHODInitiator:
                    // Find HOD of selected department
                    return await FindUserByRole(UserRole.HOD, pr.Department);

                case ApprovalStatus.PendingHODDepartment:
                    return await FindUserByRole(UserRole.Finance);

                case ApprovalStatus.PendingFinance:
                    return await FindUserByRole(UserRole.Warehouse);

                case ApprovalStatus.PendingWarehouse:
                    return await FindUserByRole(UserRole.SCM);

                case ApprovalStatus.PendingSCM:
                    return await FindUserByRole(UserRole.Director);

                case ApprovalStatus.PendingDirector:
                    return await FindUserByRole(UserRole.CFO);

                case ApprovalStatus.PendingCFO:
                    return await FindUserByRole(UserRole.CEO);

                default:
                    return null;
            }
        }

        private async Task<string?> FindUserByRole(UserRole role, string? department = null)
        {
            var query = _context.Users.Where(u => u.Role == role && u.IsActive);
            
            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(u => u.Department == department);
            }

            var user = await query.FirstOrDefaultAsync();
            
            if (user != null)
            {
                // Check for delegations
                var delegation = await _context.DelegationHistories
                    .Where(d => d.FromUserId == user.Id 
                               && d.IsActive 
                               && (d.ExpiryDate == null || d.ExpiryDate > DateTime.UtcNow))
                    .FirstOrDefaultAsync();

                return delegation?.ToUserId ?? user.Id;
            }

            return null;
        }

        public async Task<bool> ProcessApprovalAsync(int prId, string approverId, string action, 
            string? comments = null, string? delegateToUserId = null, string? rejectionReason = null)
        {
            var pr = await _context.PurchaseRequisitions.FindAsync(prId);
            if (pr == null) return false;

            var approvalHistory = new ApprovalHistory
            {
                PurchaseRequisitionId = prId,
                ApproverId = approverId,
                Comments = comments,
                Level = GetCurrentApprovalLevel(pr.CurrentStatus)
            };

            switch (action.ToLower())
            {
                case "approve":
                    approvalHistory.Status = ApprovalStatus.Approved;
                    var nextApprover = await GetNextApproverAsync(pr);
                    
                    if (nextApprover != null)
                    {
                        pr.CurrentApproverId = nextApprover;
                        pr.CurrentStatus = GetNextApprovalStatus(pr.CurrentStatus);
                    }
                    else
                    {
                        pr.CurrentStatus = ApprovalStatus.Approved;
                        pr.CurrentApproverId = null;
                    }
                    break;

                case "reject":
                    approvalHistory.Status = ApprovalStatus.Rejected;
                    pr.CurrentStatus = ApprovalStatus.Rejected;
                    pr.RejectionReason = rejectionReason;
                    pr.CurrentApproverId = null;
                    break;

                case "delegate":
                    if (string.IsNullOrEmpty(delegateToUserId)) return false;
                    
                    approvalHistory.Status = ApprovalStatus.Delegated;
                    
                    var delegation = new DelegationHistory
                    {
                        PurchaseRequisitionId = prId,
                        FromUserId = approverId,
                        ToUserId = delegateToUserId,
                        Reason = comments
                    };
                    
                    pr.CurrentApproverId = delegateToUserId;
                    _context.DelegationHistories.Add(delegation);
                    break;

                case "clarification":
                    approvalHistory.Status = ApprovalStatus.Clarification;
                    pr.CurrentStatus = ApprovalStatus.Clarification;
                    pr.ClarificationNotes = comments;
                    pr.CurrentApproverId = pr.InitiatorId;
                    break;

                default:
                    return false;
            }

            pr.LastModifiedDate = DateTime.UtcNow;
            _context.ApprovalHistories.Add(approvalHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ApplicationUser>> GetEligibleDelegateUsersAsync(string currentUserId, UserRole currentRole)
        {
            return await _context.Users
                .Where(u => u.Id != currentUserId 
                           && u.Role == currentRole 
                           && u.IsActive)
                .ToListAsync();
        }

        public async Task<bool> CanUserApproveAsync(string userId, int prId)
        {
            var pr = await _context.PurchaseRequisitions.FindAsync(prId);
            if (pr == null) return false;

            return pr.CurrentApproverId == userId;
        }

        private int GetCurrentApprovalLevel(ApprovalStatus status)
        {
            return status switch
            {
                ApprovalStatus.Initiated => 0,
                ApprovalStatus.PendingHODInitiator => 1,
                ApprovalStatus.PendingHODDepartment => 2,
                ApprovalStatus.PendingFinance => 3,
                ApprovalStatus.PendingWarehouse => 4,
                ApprovalStatus.PendingSCM => 5,
                ApprovalStatus.PendingDirector => 6,
                ApprovalStatus.PendingCFO => 7,
                ApprovalStatus.PendingCEO => 8,
                _ => 0
            };
        }

        private ApprovalStatus GetNextApprovalStatus(ApprovalStatus currentStatus)
        {
            return currentStatus switch
            {
                ApprovalStatus.Initiated => ApprovalStatus.PendingHODInitiator,
                ApprovalStatus.PendingHODInitiator => ApprovalStatus.PendingHODDepartment,
                ApprovalStatus.PendingHODDepartment => ApprovalStatus.PendingFinance,
                ApprovalStatus.PendingFinance => ApprovalStatus.PendingWarehouse,
                ApprovalStatus.PendingWarehouse => ApprovalStatus.PendingSCM,
                ApprovalStatus.PendingSCM => ApprovalStatus.PendingDirector,
                ApprovalStatus.PendingDirector => ApprovalStatus.PendingCFO,
                ApprovalStatus.PendingCFO => ApprovalStatus.PendingCEO,
                _ => ApprovalStatus.Approved
            };
        }
    }
}