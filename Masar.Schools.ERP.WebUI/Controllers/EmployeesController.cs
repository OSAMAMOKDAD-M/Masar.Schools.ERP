using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Infrastructure.Interfaces;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<EmployeesController> _logger;
    private readonly ILeaveService _leaveService;

    public EmployeesController(MasarDbContext context, ILogger<EmployeesController> logger, ILeaveService leaveService)
    {
        _context = context;
        _logger = logger;
        _leaveService = leaveService;
    }

    // GET: /Employees/Index
    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees
            .Include(e => e.School)
            .Include(e => e.Branch)
            .Where(e => !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync();

        return View(employees);
    }

    // GET: /Employees/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
        return View();
    }

    // POST: /Employees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
            ViewBag.Branches = new SelectList(await _context.Branches.Where(b => !b.IsDeleted).ToListAsync(), "Id", "NameArabic");
            return View(model);
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            FirstNameArabic = model.FirstNameArabic,
            LastName = model.LastName,
            LastNameArabic = model.LastNameArabic,
            FullName = $"{model.FirstName} {model.LastName}",
            FullNameArabic = $"{model.FirstNameArabic} {model.LastNameArabic}",
            NationalId = model.NationalId,
            EmployeeNumber = await GenerateEmployeeNumber(),
            JobTitle = model.JobTitle,
            JobTitleArabic = model.JobTitleArabic,
            Department = model.Department,
            DepartmentArabic = model.DepartmentArabic,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            HireDate = model.HireDate,
            Salary = model.Salary,
            BankAccountNumber = model.BankAccountNumber,
            BankName = model.BankName,
            Address = model.Address,
            ProfileImagePath = model.ProfileImagePath,
            SchoolId = model.SchoolId,
            BranchId = model.BranchId,
            IsActive = true,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إضافة الموظف بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Employees/TeacherSchedule/5
    [HttpGet("TeacherSchedule/{id}")]
    public async Task<IActionResult> TeacherSchedule(Guid id)
    {
        var employee = await _context.Employees
            .Include(e => e.School)
            .Include(e => e.Branch)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            return NotFound();
        }

        // TODO: Implement schedule logic when schedule entities are added
        ViewBag.Schedules = new List<object>(); // Placeholder

        return View(employee);
    }

    // GET: /Employees/TeacherScheduleIndex (صفحة رئيسية تعرض جميع المعلمين)
    public async Task<IActionResult> TeacherScheduleIndex()
    {
        var teachers = await _context.Employees
            .Include(e => e.School)
            .Include(e => e.Branch)
            .Where(e => !e.IsDeleted && e.JobTitleArabic != null && (e.JobTitleArabic.Contains("معلم") || e.JobTitleArabic.Contains("مدرس")))
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync();

        return View("TeacherScheduleIndex", teachers);
    }

    // GET: /Employees/Payroll
    public async Task<IActionResult> Payroll()
    {
        var employees = await _context.Employees
            .Include(e => e.School)
            .Where(e => e.IsActive && !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync();

        var currentMonth = DateTime.Now;
        var firstDay = new DateTime(currentMonth.Year, currentMonth.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        ViewBag.PayrollPeriod = $"{firstDay:yyyy-MM-dd} إلى {lastDay:yyyy-MM-dd}";
        return View(employees);
    }

    // GET: /Employees/Leaves
    public async Task<IActionResult> Leaves()
    {
        var leaveRequests = await _leaveService.GetAllLeaveRequestsAsync();

        ViewBag.Employees = await _context.Employees
            .Where(e => e.IsActive && !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync();

        return View(leaveRequests);
    }

    // GET: /Employees/Leaves/Create
    public async Task<IActionResult> CreateLeave()
    {
        ViewBag.Employees = new SelectList(await _context.Employees
            .Where(e => e.IsActive && !e.IsDeleted)
            .OrderBy(e => e.FullNameArabic)
            .ToListAsync(), "Id", "FullNameArabic");

        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");

        return View();
    }

    // POST: /Employees/Leaves/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLeave(LeaveRequestCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Employees = new SelectList(await _context.Employees
                .Where(e => e.IsActive && !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync(), "Id", "FullNameArabic");

            ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");

            return View(model);
        }

        try
        {
            await _leaveService.CreateLeaveRequestAsync(model);
            TempData["Success"] = "تم إنشاء طلب الإجازة بنجاح";
            return RedirectToAction(nameof(Leaves));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.Employees = new SelectList(await _context.Employees
                .Where(e => e.IsActive && !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync(), "Id", "FullNameArabic");

            ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");

            return View(model);
        }
    }

    // POST: /Employees/Leaves/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveLeave(Guid id)
    {
        try
        {
            var actionDto = new LeaveRequestActionDto
            {
                Status = "Approved",
                StatusArabic = "موافق عليه"
            };

            await _leaveService.ApproveLeaveRequestAsync(id, actionDto);
            TempData["Success"] = "تم الموافقة على طلب الإجازة بنجاح";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Leaves));
    }

    // POST: /Employees/Leaves/Reject/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectLeave(Guid id, string rejectionReason)
    {
        try
        {
            var actionDto = new LeaveRequestActionDto
            {
                Status = "Rejected",
                StatusArabic = "مرفوض",
                RejectionReason = rejectionReason,
                RejectionReasonArabic = rejectionReason
            };

            await _leaveService.RejectLeaveRequestAsync(id, actionDto);
            TempData["Success"] = "تم رفض طلب الإجازة بنجاح";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Leaves));
    }

    // GET: /Employees/Leaves/Balance/5
    public async Task<IActionResult> LeaveBalance(Guid? employeeId)
    {
        if (employeeId == null)
        {
            return RedirectToAction(nameof(Leaves));
        }

        var balances = await _leaveService.GetLeaveBalancesByEmployeeIdAsync(employeeId.Value);

        var employee = await _context.Employees.FindAsync(employeeId.Value);
        ViewBag.Employee = employee;

        return View(balances);
    }

    // GET: /Employees/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var employee = await _context.Employees
            .Include(e => e.School)
            .Include(e => e.Branch)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    private async Task<string> GenerateEmployeeNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.Employees
            .Where(e => e.EmployeeNumber != null && e.EmployeeNumber.StartsWith("EMP" + year))
            .OrderByDescending(e => e.EmployeeNumber)
            .Select(e => e.EmployeeNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"EMP{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(7));
        return $"EMP{year}{(lastNum + 1).ToString("D4")}";
    }
}

// DTOs
public class EmployeeCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Department { get; set; }
    public string? DepartmentArabic { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? Address { get; set; }
    public string? ProfileImagePath { get; set; }
    public Guid SchoolId { get; set; }
    public Guid? BranchId { get; set; }
}
