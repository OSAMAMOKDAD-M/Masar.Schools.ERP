using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(MasarDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Home/Index
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        // Get current user's tenant
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        var stats = new DashboardStatsViewModel
        {
            TotalStudents = await _context.Students.CountAsync(s => s.IsActive && !s.IsDeleted),
            TotalEmployees = await _context.Employees.CountAsync(e => e.IsActive && !e.IsDeleted),
            TotalSchools = await _context.Schools.CountAsync(s => !s.IsDeleted),
            TodayAttendanceRate = await GetTodayAttendanceRate(),
            TodayCollections = await GetTodayCollections(),
            ActiveCallNotifications = await GetActiveCallNotifications(),
            TotalGuardians = await _context.Guardians.CountAsync(g => g.IsActive && !g.IsDeleted)
        };

        return View(stats);
    }

    // GET: /Home/GetLiveStats
    [HttpGet]
    public async Task<IActionResult> GetLiveStats()
    {
        var stats = new
        {
            TotalStudents = await _context.Students.CountAsync(s => s.IsActive && !s.IsDeleted),
            TotalEmployees = await _context.Employees.CountAsync(e => e.IsActive && !e.IsDeleted),
            TodayAttendanceRate = await GetTodayAttendanceRate(),
            TodayCollections = await GetTodayCollections(),
            ActiveCallNotifications = await GetActiveCallNotifications(),
            LastUpdated = DateTime.Now
        };

        return Json(stats);
    }

    private async Task<double> GetTodayAttendanceRate()
    {
        var today = DateTime.Today;
        var totalStudents = await _context.Students.CountAsync(s => s.IsActive && !s.IsDeleted);
        
        if (totalStudents == 0) return 0;

        var presentStudents = await _context.AttendanceRecords
            .CountAsync(a => a.Date == today && 
                           a.Status == "Present" && 
                           !a.IsDeleted);

        return (double)presentStudents / totalStudents * 100;
    }

    private async Task<decimal> GetTodayCollections()
    {
        var today = DateTime.Today;
        return await _context.InvoicePayments
            .Where(p => p.PaymentDate.HasValue && 
                       p.PaymentDate.Value.Date == today && 
                       !p.IsDeleted)
            .SumAsync(p => p.Amount);
    }

    private async Task<int> GetActiveCallNotifications()
    {
        var today = DateTime.Today;
        return await _context.AttendanceRecords
            .CountAsync(a => a.Date == today && 
                           a.Status == "Absent" && 
                           !a.IsDeleted &&
                           !a.WhatsAppNotificationSent);
    }
}

public class DashboardStatsViewModel
{
    public int TotalStudents { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalSchools { get; set; }
    public double TodayAttendanceRate { get; set; }
    public decimal TodayCollections { get; set; }
    public int ActiveCallNotifications { get; set; }
    public int TotalGuardians { get; set; }
}
