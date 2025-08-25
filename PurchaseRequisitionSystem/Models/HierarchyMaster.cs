using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class HierarchyMaster
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int FacilityId { get; set; }
        
        [Required]
        public string Department { get; set; } = string.Empty;
        
        [Required]
        public ExpenseType ExpenseType { get; set; }
        
        public int Level { get; set; }
        
        [Required]
        public string ApproverRole { get; set; } = string.Empty;
        
        public string? ApproverUserId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public string CreatedBy { get; set; } = string.Empty;
        
        // Foreign keys
        [ForeignKey("FacilityId")]
        public virtual FacilityMaster Facility { get; set; } = null!;
        
        [ForeignKey("ApproverUserId")]
        public virtual ApplicationUser? ApproverUser { get; set; }
    }
}