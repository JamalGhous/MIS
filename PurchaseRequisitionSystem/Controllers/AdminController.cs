using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseRequisitionSystem.Models;

namespace PurchaseRequisitionSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Facility Master Management
        public async Task<IActionResult> FacilityMaster()
        {
            var facilities = await _context.FacilityMasters.ToListAsync();
            return View(facilities);
        }

        [HttpGet]
        public IActionResult CreateFacility()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFacility(FacilityMaster facility)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                facility.CreatedBy = currentUser?.EmployeeId ?? "System";
                
                _context.FacilityMasters.Add(facility);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Facility created successfully!";
                return RedirectToAction(nameof(FacilityMaster));
            }
            return View(facility);
        }

        [HttpGet]
        public async Task<IActionResult> EditFacility(int id)
        {
            var facility = await _context.FacilityMasters.FindAsync(id);
            if (facility == null) return NotFound();
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFacility(FacilityMaster facility)
        {
            if (ModelState.IsValid)
            {
                _context.Update(facility);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Facility updated successfully!";
                return RedirectToAction(nameof(FacilityMaster));
            }
            return View(facility);
        }

        // Hierarchy Master Management
        public async Task<IActionResult> HierarchyMaster()
        {
            var hierarchies = await _context.HierarchyMasters
                .Include(h => h.Facility)
                .Include(h => h.ApproverUser)
                .OrderBy(h => h.FacilityId)
                .ThenBy(h => h.Department)
                .ThenBy(h => h.Level)
                .ToListAsync();
            
            return View(hierarchies);
        }

        [HttpGet]
        public async Task<IActionResult> CreateHierarchy()
        {
            ViewBag.Facilities = await _context.FacilityMasters.Where(f => f.IsActive).ToListAsync();
            ViewBag.Users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHierarchy(HierarchyMaster hierarchy)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                hierarchy.CreatedBy = currentUser?.EmployeeId ?? "System";
                
                _context.HierarchyMasters.Add(hierarchy);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Hierarchy created successfully!";
                return RedirectToAction(nameof(HierarchyMaster));
            }
            
            ViewBag.Facilities = await _context.FacilityMasters.Where(f => f.IsActive).ToListAsync();
            ViewBag.Users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();
            return View(hierarchy);
        }

        // User Management
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    TempData["Success"] = "User created successfully!";
                    return RedirectToAction(nameof(Users));
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        // Reports
        public async Task<IActionResult> Reports()
        {
            var totalPRs = await _context.PurchaseRequisitions.CountAsync();
            var approvedPRs = await _context.PurchaseRequisitions.CountAsync(pr => pr.CurrentStatus == ApprovalStatus.Approved);
            var rejectedPRs = await _context.PurchaseRequisitions.CountAsync(pr => pr.CurrentStatus == ApprovalStatus.Rejected);
            var pendingPRs = await _context.PurchaseRequisitions.CountAsync(pr => pr.CurrentStatus != ApprovalStatus.Approved && pr.CurrentStatus != ApprovalStatus.Rejected);

            ViewBag.TotalPRs = totalPRs;
            ViewBag.ApprovedPRs = approvedPRs;
            ViewBag.RejectedPRs = rejectedPRs;
            ViewBag.PendingPRs = pendingPRs;

            var recentPRs = await _context.PurchaseRequisitions
                .Include(pr => pr.Initiator)
                .Include(pr => pr.Facility)
                .OrderByDescending(pr => pr.CreatedDate)
                .Take(10)
                .ToListAsync();

            return View(recentPRs);
        }
    }
}