using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class PRAttachment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PurchaseRequisitionId { get; set; }
        
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public string FileType { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        public string UploadedBy { get; set; } = string.Empty;
        
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
        
        public string? Description { get; set; }
        
        // Foreign keys
        [ForeignKey("PurchaseRequisitionId")]
        public virtual PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        
        [ForeignKey("UploadedBy")]
        public virtual ApplicationUser UploadedByUser { get; set; } = null!;
    }
}