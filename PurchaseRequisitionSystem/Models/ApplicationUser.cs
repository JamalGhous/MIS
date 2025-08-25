using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PurchaseRequisitionSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        public string Department { get; set; } = string.Empty;
        
        public FacilityCode? Facility { get; set; }
        
        public UserRole Role { get; set; }
        
        public string Manager { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<PurchaseRequisition> InitiatedRequests { get; set; } = new List<PurchaseRequisition>();
        public virtual ICollection<ApprovalHistory> ApprovalHistory { get; set; } = new List<ApprovalHistory>();
        public virtual ICollection<DelegationHistory> DelegationsFrom { get; set; } = new List<DelegationHistory>();
        public virtual ICollection<DelegationHistory> DelegationsTo { get; set; } = new List<DelegationHistory>();
    }
}