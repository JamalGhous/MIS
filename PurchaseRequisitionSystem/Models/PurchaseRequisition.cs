using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class PurchaseRequisition
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string PRNumber { get; set; } = string.Empty;
        
        [Required]
        public string InitiatorId { get; set; } = string.Empty;
        
        [Required]
        public int FacilityId { get; set; }
        
        [Required]
        public RequestType RequestType { get; set; }
        
        [Required]
        public ExpenseType ExpenseType { get; set; }
        
        public OpexCategory? OpexCategory { get; set; }
        
        public bool IsBudgeted { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostEstimation { get; set; }
        
        public string ItemCode { get; set; } = string.Empty;
        
        [Required]
        public string ItemDescription { get; set; } = string.Empty;
        
        [Required]
        public string Specification { get; set; } = string.Empty;
        
        public string Justification { get; set; } = string.Empty;
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public UnitOfMeasure UnitOfMeasure { get; set; }
        
        [Required]
        public string Department { get; set; } = string.Empty;
        
        public ApprovalStatus CurrentStatus { get; set; } = ApprovalStatus.Initiated;
        
        public string? CurrentApproverId { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastModifiedDate { get; set; }
        
        public string? RejectionReason { get; set; }
        
        public string? ClarificationNotes { get; set; }
        
        // Foreign keys
        [ForeignKey("InitiatorId")]
        public virtual ApplicationUser Initiator { get; set; } = null!;
        
        [ForeignKey("FacilityId")]
        public virtual FacilityMaster Facility { get; set; } = null!;
        
        [ForeignKey("CurrentApproverId")]
        public virtual ApplicationUser? CurrentApprover { get; set; }
        
        // Navigation properties
        public virtual ICollection<ApprovalHistory> ApprovalHistory { get; set; } = new List<ApprovalHistory>();
        public virtual ICollection<PRAttachment> Attachments { get; set; } = new List<PRAttachment>();
        public virtual ICollection<DelegationHistory> DelegationHistory { get; set; } = new List<DelegationHistory>();
    }
}