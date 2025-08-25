using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class DelegationHistory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PurchaseRequisitionId { get; set; }
        
        [Required]
        public string FromUserId { get; set; } = string.Empty;
        
        [Required]
        public string ToUserId { get; set; } = string.Empty;
        
        public string? Reason { get; set; }
        
        public DateTime DelegatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? ExpiryDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Foreign keys
        [ForeignKey("PurchaseRequisitionId")]
        public virtual PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        
        [ForeignKey("FromUserId")]
        public virtual ApplicationUser FromUser { get; set; } = null!;
        
        [ForeignKey("ToUserId")]
        public virtual ApplicationUser ToUser { get; set; } = null!;
    }
}