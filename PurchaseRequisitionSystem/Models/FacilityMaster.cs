using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseRequisitionSystem.Models
{
    public class FacilityMaster
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public FacilityCode Code { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public string CreatedBy { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<PurchaseRequisition> PurchaseRequisitions { get; set; } = new List<PurchaseRequisition>();
        public virtual ICollection<HierarchyMaster> HierarchyMasters { get; set; } = new List<HierarchyMaster>();
    }
}