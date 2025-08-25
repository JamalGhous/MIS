using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PurchaseRequisitionSystem.Models;
using PurchaseRequisitionSystem.Services;
using PurchaseRequisitionSystem.ViewModels;
using System.Diagnostics;

namespace PurchaseRequisitionSystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPRService _prService;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IPRService prService)
    {
        _logger = logger;
        _userManager = userManager;
        _prService = prService;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        return RedirectToAction(nameof(Dashboard));
    }

    public async Task<IActionResult> Dashboard()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        var myRequests = await _prService.GetUserRequestsAsync(currentUser.Id);
        var pendingApprovals = await _prService.GetPendingApprovalsAsync(currentUser.Id);

        var model = new DashboardViewModel
        {
            MyRequests = myRequests,
            PendingApprovals = pendingApprovals,
            CurrentUserRole = currentUser.Role,
            CurrentUserName = currentUser.FullName,
            TotalInProcess = myRequests.Count(r => r.CurrentStatus != ApprovalStatus.Approved && r.CurrentStatus != ApprovalStatus.Rejected),
            TotalApproved = myRequests.Count(r => r.CurrentStatus == ApprovalStatus.Approved),
            TotalRejected = myRequests.Count(r => r.CurrentStatus == ApprovalStatus.Rejected),
            PendingMyApproval = pendingApprovals.Count
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
