using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class ApprovalHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PurchaseRequisitionId { get; set; }
        
        [Required]
        public string ApproverId { get; set; } = string.Empty;
        
        [Required]
        public ApprovalStatus Status { get; set; }
        
        public string? Comments { get; set; }
        
        public string? AttachmentPath { get; set; }
        
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        
        public int Level { get; set; }
        
        // Foreign keys
        [ForeignKey("PurchaseRequisitionId")]
        public virtual PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        
        [ForeignKey("ApproverId")]
        public virtual ApplicationUser Approver { get; set; } = null!;
    }
}