using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// بوابة ولي الأمر
/// Parent Portal Controller
/// </summary>
[AllowAnonymous]
public class ParentPortalController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<ParentPortalController> _logger;

    public ParentPortalController(MasarDbContext context, ILogger<ParentPortalController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// صفحة تسجيل دخول ولي الأمر
    /// Parent Login Page
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// معالجة تسجيل دخول ولي الأمر
    /// Handle Parent Login
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(ParentLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Find guardian by national ID and phone number
        var guardian = await _context.Guardians
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.School)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.ClassRoom)
            .FirstOrDefaultAsync(g => 
                g.NationalId == model.NationalId && 
                g.PhoneNumber == model.PhoneNumber &&
                !g.IsDeleted &&
                g.IsActive);

        if (guardian == null)
        {
            ModelState.AddModelError(string.Empty, "رقم الهوية أو رقم الهاتف غير صحيح");
            return View(model);
        }

        // Store guardian info in session
        HttpContext.Session.SetString("GuardianId", guardian.Id.ToString());
        HttpContext.Session.SetString("GuardianName", guardian.FullNameArabic);

        _logger.LogInformation("Parent {GuardianName} logged in successfully", guardian.FullNameArabic);

        return RedirectToAction(nameof(Dashboard));
    }

    /// <summary>
    /// لوحة تحكم ولي الأمر
    /// Parent Dashboard
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var guardianIdStr = HttpContext.Session.GetString("GuardianId");
        if (string.IsNullOrEmpty(guardianIdStr) || !Guid.TryParse(guardianIdStr, out var guardianId))
        {
            return RedirectToAction(nameof(Login));
        }

        var guardian = await _context.Guardians
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.School)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.ClassRoom)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.AttendanceRecords)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.Grades)
            .FirstOrDefaultAsync(g => g.Id == guardianId);

        if (guardian == null)
        {
            return RedirectToAction(nameof(Login));
        }

        ViewBag.GuardianName = guardian.FullNameArabic;
        return View(guardian);
    }

    /// <summary>
    /// تفاصيل الطالب من بوابة ولي الأمر
    /// Student Details from Parent Portal
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> StudentDetails(Guid studentId)
    {
        var guardianIdStr = HttpContext.Session.GetString("GuardianId");
        if (string.IsNullOrEmpty(guardianIdStr) || !Guid.TryParse(guardianIdStr, out var guardianId))
        {
            return RedirectToAction(nameof(Login));
        }

        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .Include(s => s.AttendanceRecords.OrderByDescending(a => a.Date).Take(30))
            .Include(s => s.Grades)
            .Include(s => s.Documents)
            .FirstOrDefaultAsync(s => s.Id == studentId && s.GuardianId == guardianId && !s.IsDeleted);

        if (student == null)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        ViewBag.GuardianName = HttpContext.Session.GetString("GuardianName");
        return View(student);
    }

    /// <summary>
    /// سجل الحضور للطالب
    /// Student Attendance Record
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> StudentAttendance(Guid studentId)
    {
        var guardianIdStr = HttpContext.Session.GetString("GuardianId");
        if (string.IsNullOrEmpty(guardianIdStr) || !Guid.TryParse(guardianIdStr, out var guardianId))
        {
            return RedirectToAction(nameof(Login));
        }

        var student = await _context.Students
            .Include(s => s.AttendanceRecords.OrderByDescending(a => a.Date).Take(90))
            .FirstOrDefaultAsync(s => s.Id == studentId && s.GuardianId == guardianId && !s.IsDeleted);

        if (student == null)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        ViewBag.GuardianName = HttpContext.Session.GetString("GuardianName");
        return View(student);
    }

    /// <summary>
    /// السجل الأكاديمي للطالب
    /// Student Academic Record
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> StudentGrades(Guid studentId)
    {
        var guardianIdStr = HttpContext.Session.GetString("GuardianId");
        if (string.IsNullOrEmpty(guardianIdStr) || !Guid.TryParse(guardianIdStr, out var guardianId))
        {
            return RedirectToAction(nameof(Login));
        }

        var student = await _context.Students
            .Include(s => s.Grades)
                .ThenInclude(g => g.Subject)
            .Include(s => s.AcademicRecords)
            .FirstOrDefaultAsync(s => s.Id == studentId && s.GuardianId == guardianId && !s.IsDeleted);

        if (student == null)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        ViewBag.GuardianName = HttpContext.Session.GetString("GuardianName");
        return View(student);
    }

    /// <summary>
    /// تسجيل الخروج
    /// Logout
    /// </summary>
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}

// DTOs
public class ParentLoginViewModel
{
    [Required(ErrorMessage = "رقم الهوية مطلوب")]
    [Display(Name = "رقم الهوية")]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Display(Name = "رقم الهاتف")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    public string PhoneNumber { get; set; } = string.Empty;
}
