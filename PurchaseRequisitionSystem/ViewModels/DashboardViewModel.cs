using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.ViewModels
{
    public class DashboardViewModel
    {
        public List<PurchaseRequisition> MyRequests { get; set; } = new List<PurchaseRequisition>();
        public List<PurchaseRequisition> PendingApprovals { get; set; } = new List<PurchaseRequisition>();
        public List<PurchaseRequisition> RecentActivity { get; set; } = new List<PurchaseRequisition>();
        
        // Statistics
        public int TotalInProcess { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public int PendingMyApproval { get; set; }
        
        public UserRole CurrentUserRole { get; set; }
        public string CurrentUserName { get; set; } = string.Empty;
    }
    
    public class ApprovalActionViewModel
    {
        public int PurchaseRequisitionId { get; set; }
        public string Action { get; set; } = string.Empty; // Approve, Reject, Delegate
        public string? Comments { get; set; }
        public string? DelegateToUserId { get; set; }
        public string? RejectionReason { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        
        // For displaying PR details
        public PurchaseRequisition? PurchaseRequisition { get; set; }
        public List<ApplicationUser> DelegateUsers { get; set; } = new List<ApplicationUser>();
    }
    
    public class PRTimelineViewModel
    {
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        public List<ApprovalHistory> ApprovalHistory { get; set; } = new List<ApprovalHistory>();
        public List<DelegationHistory> DelegationHistory { get; set; } = new List<DelegationHistory>();
        public List<PRAttachment> Attachments { get; set; } = new List<PRAttachment>();
    }
}