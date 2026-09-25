using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Licensing.Models;
using Masar.Schools.ERP.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class HRController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HRController> _logger;
    private readonly IHRContractService _contractService;
    private readonly IHRPayrollService _payrollService;
    private readonly IHRAttendanceService _attendanceService;
    private readonly IHRGratuityService _gratuityService;
    private readonly ILeaveService _leaveService;
    private readonly UserManager<MasarUser> _userManager;

    public HRController(
        MasarDbContext context,
        ILogger<HRController> logger,
        IHRContractService contractService,
        IHRPayrollService payrollService,
        IHRAttendanceService attendanceService,
        IHRGratuityService gratuityService,
        ILeaveService leaveService,
        UserManager<MasarUser> userManager)
    {
        _context = context;
        _logger = logger;
        _contractService = contractService;
        _payrollService = payrollService;
        _attendanceService = attendanceService;
        _gratuityService = gratuityService;
        _leaveService = leaveService;
        _userManager = userManager;
    }

    // Helper method to check permissions
    private async Task<bool> HasPermissionAsync(string permissionCode)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return false;

        // Check if user has the specific permission
        var hasPermission = await _context.UserPermissions
            .AnyAsync(up => up.UserId == user.Id && 
                       up.Permission.Code == permissionCode && 
                       up.Permission.IsActive);

        // Also check role permissions
        if (!hasPermission)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleIds = await _context.Roles
                .Where(r => userRoles.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            hasPermission = await _context.RolePermissions
                .AnyAsync(rp => roleIds.Contains(rp.RoleId) && 
                           rp.Permission.Code == permissionCode && 
                           rp.Permission.IsActive);
        }

        return hasPermission;
    }

    // GET: /HR/Dashboard
    public async Task<IActionResult> Dashboard()
    {
        // Check permissions
        if (!await HasPermissionAsync(PermissionConstants.HR.Dashboard))
        {
            return Forbid();
        }

        // Check HR module license
        if (!HasHRModuleAccess())
        {
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });
        }

        var currentSchoolId = GetCurrentSchoolId();
        
        var viewModel = new HRDashboardViewModel
        {
            TotalEmployees = await _context.Employees
                .Where(e => e.SchoolId == currentSchoolId && !e.IsResigned && !e.IsDeleted)
                .CountAsync(),
            
            ActiveContracts = await _context.Contracts
                .Where(c => c.SchoolId == currentSchoolId && c.IsCurrent && !c.IsDeleted)
                .CountAsync(),
            
            PendingLeaveRequests = await _context.LeaveRequests
                .Where(l => l.SchoolId == currentSchoolId && l.Status == "Pending" && !l.IsDeleted)
                .CountAsync(),
            
            ThisMonthPayroll = await _context.PayrollProfiles
                .Where(p => p.SchoolId == currentSchoolId && 
                           p.PeriodStart.HasValue && 
                           p.PeriodStart.Value.Month == DateTime.Now.Month &&
                           p.PeriodStart.Value.Year == DateTime.Now.Year &&
                           !p.IsDeleted)
                .SumAsync(p => p.Basic + p.Housing + p.Transport),
            
            RecentAttendance = await _context.AttendanceLogs
                .Where(a => a.SchoolId == currentSchoolId && 
                           a.EventAt >= DateTime.Today && 
                           !a.IsDeleted)
                .CountAsync(),
            
            ResignedThisYear = await _context.ResignedEmployees
                .Where(r => r.SchoolId == currentSchoolId && 
                           r.ResignDate.Year == DateTime.Now.Year &&
                           !r.IsDeleted)
                .CountAsync()
        };

        return View(viewModel);
    }

    // GET: /HR/Contracts
    public async Task<IActionResult> Contracts()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.ContractsView))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var currentSchoolId = GetCurrentSchoolId();
        var contracts = await _context.Contracts
            .Include(c => c.Employee)
            .Where(c => c.SchoolId == currentSchoolId && !c.IsDeleted)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();

        return View(contracts);
    }

    // GET: /HR/Contracts/Create
    public async Task<IActionResult> CreateContract()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.ContractsCreate))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        ViewBag.Employees = new SelectList(await _context.Employees
            .Where(e => e.SchoolId == GetCurrentSchoolId() && !e.IsResigned && !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync(), "Id", "FullNameArabic");

        return View();
    }

    // POST: /HR/Contracts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateContract(ContractCreateDto dto)
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.ContractsCreate))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        if (!ModelState.IsValid)
        {
            ViewBag.Employees = new SelectList(await _context.Employees
                .Where(e => e.SchoolId == GetCurrentSchoolId() && !e.IsResigned && !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync(), "Id", "FullNameArabic");
            return View(dto);
        }

        dto.SchoolId = GetCurrentSchoolId();
        await _contractService.CreateContractAsync(dto);

        TempData["Success"] = "تم إنشاء العقد بنجاح";
        return RedirectToAction(nameof(Contracts));
    }

    // GET: /HR/Payroll
    public async Task<IActionResult> Payroll()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.PayrollView))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var currentSchoolId = GetCurrentSchoolId();
        var payrollProfiles = await _context.PayrollProfiles
            .Include(p => p.Employee)
            .Where(p => p.SchoolId == currentSchoolId && !p.IsDeleted)
            .OrderByDescending(p => p.EffectiveFrom)
            .ToListAsync();

        return View(payrollProfiles);
    }

    // GET: /HR/Payroll/Create
    public async Task<IActionResult> CreatePayroll()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.PayrollCreate))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        ViewBag.Employees = new SelectList(await _context.Employees
            .Where(e => e.SchoolId == GetCurrentSchoolId() && !e.IsResigned && !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync(), "Id", "FullNameArabic");

        return View();
    }

    // POST: /HR/Payroll/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePayroll(PayrollCreateDto dto)
    {
        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        if (!ModelState.IsValid)
        {
            ViewBag.Employees = new SelectList(await _context.Employees
                .Where(e => e.SchoolId == GetCurrentSchoolId() && !e.IsResigned && !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync(), "Id", "FullNameArabic");
            return View(dto);
        }

        dto.SchoolId = GetCurrentSchoolId();
        await _payrollService.CreatePayrollProfileAsync(dto);

        TempData["Success"] = "تم إنشاء ملف الرواتب بنجاح";
        return RedirectToAction(nameof(Payroll));
    }

    // GET: /HR/Attendance
    public async Task<IActionResult> Attendance()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.AttendanceView))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var currentSchoolId = GetCurrentSchoolId();
        var attendanceLogs = await _context.AttendanceLogs
            .Include(a => a.Employee)
            .Where(a => a.SchoolId == currentSchoolId && !a.IsDeleted)
            .OrderByDescending(a => a.EventAt)
            .Take(100)
            .ToListAsync();

        return View(attendanceLogs);
    }

    // GET: /HR/Attendance/Import
    public async Task<IActionResult> ImportAttendance()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.AttendanceImport))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        return View();
    }

    // POST: /HR/Attendance/Import
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportAttendance(IFormFile file)
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.AttendanceImport))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "يرجى اختيار ملف Excel");
            return View();
        }

        try
        {
            await _attendanceService.ImportAttendanceFromExcelAsync(file);
            TempData["Success"] = "تم استيراد بيانات الحضور بنجاح";
            return RedirectToAction(nameof(Attendance));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    // GET: /HR/Gratuity
    public async Task<IActionResult> Gratuity()
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.GratuityView))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var currentSchoolId = GetCurrentSchoolId();
        var resignedEmployees = await _context.ResignedEmployees
            .Where(r => r.SchoolId == currentSchoolId && !r.IsDeleted)
            .OrderByDescending(r => r.ResignDate)
            .ToListAsync();

        return View(resignedEmployees);
    }

    // GET: /HR/Gratuity/Calculate/{id}
    public async Task<IActionResult> CalculateGratuity(Guid id)
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.GratuityCalculate))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var calculation = await _gratuityService.CalculateGratuityAsync(id, DateTime.Now);
        return View(calculation);
    }

    // GET: /HR/LeaveBalance/{id}
    public async Task<IActionResult> LeaveBalance(Guid id)
    {
        if (!await HasPermissionAsync(PermissionConstants.HR.LeaveBalanceView))
            return Forbid();

        if (!HasHRModuleAccess())
            return RedirectToAction("LicenseRequired", "Home", new { module = "الموارد البشرية" });

        var balance = await _leaveService.CalculateLeaveBalanceAsync(id, "annual");
        return View(balance);
    }

    // Helper methods
    private bool HasHRModuleAccess()
    {
        // Check if HR module is enabled in license
        // This would integrate with the existing licensing system
        return true; // Placeholder - implement actual license check
    }

    private Guid GetCurrentSchoolId()
    {
        // Get current school ID from user context or session
        // This would integrate with the existing multi-tenancy system
        return Guid.Empty; // Placeholder - implement actual school ID retrieval
    }
}

// ViewModels
public class HRDashboardViewModel
{
    public int TotalEmployees { get; set; }
    public int ActiveContracts { get; set; }
    public int PendingLeaveRequests { get; set; }
    public decimal ThisMonthPayroll { get; set; }
    public int RecentAttendance { get; set; }
    public int ResignedThisYear { get; set; }
}