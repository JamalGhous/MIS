using System.ComponentModel.DataAnnotations;
using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.ViewModels
{
    public class CreatePRViewModel
    {
        [Required]
        [Display(Name = "Facility")]
        public int FacilityId { get; set; }
        
        [Required]
        [Display(Name = "Request Type")]
        public RequestType RequestType { get; set; }
        
        [Required]
        [Display(Name = "Expense Type")]
        public ExpenseType ExpenseType { get; set; }
        
        [Display(Name = "OPEX Category")]
        public OpexCategory? OpexCategory { get; set; }
        
        [Display(Name = "Is Budgeted")]
        public bool IsBudgeted { get; set; }
        
        [Required]
        [Display(Name = "Cost Estimation")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0")]
        public decimal CostEstimation { get; set; }
        
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Item Description")]
        [StringLength(500)]
        public string ItemDescription { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Specification")]
        [StringLength(1000)]
        public string Specification { get; set; } = string.Empty;
        
        [Display(Name = "Justification")]
        [StringLength(1000)]
        public string Justification { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        [Required]
        [Display(Name = "Unit of Measure")]
        public UnitOfMeasure UnitOfMeasure { get; set; }
        
        [Required]
        [Display(Name = "Department")]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;
        
        [Display(Name = "Attachments")]
        public List<IFormFile>? Attachments { get; set; }
        
        // For dropdown lists
        public List<FacilityMaster> Facilities { get; set; } = new List<FacilityMaster>();
    }
}