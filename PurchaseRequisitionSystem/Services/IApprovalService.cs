using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.Services
{
    public interface IApprovalService
    {
        Task<string?> GetNextApproverAsync(PurchaseRequisition pr);
        Task<bool> ProcessApprovalAsync(int prId, string approverId, string action, string? comments = null, string? delegateToUserId = null, string? rejectionReason = null);
        Task<List<ApplicationUser>> GetEligibleDelegateUsersAsync(string currentUserId, UserRole currentRole);
        Task<bool> CanUserApproveAsync(string userId, int prId);
    }
}