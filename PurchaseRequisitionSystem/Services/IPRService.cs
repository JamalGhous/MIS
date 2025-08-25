using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.Services
{
    public interface IPRService
    {
        Task<string> GeneratePRNumberAsync();
        Task<PurchaseRequisition> CreatePRAsync(PurchaseRequisition pr, List<IFormFile>? attachments);
        Task<List<PurchaseRequisition>> GetUserRequestsAsync(string userId);
        Task<List<PurchaseRequisition>> GetPendingApprovalsAsync(string userId);
        Task<PurchaseRequisition?> GetPRByIdAsync(int id, bool includeAll = false);
        Task SaveAttachmentsAsync(int prId, List<IFormFile> attachments, string uploadedBy);
    }
}