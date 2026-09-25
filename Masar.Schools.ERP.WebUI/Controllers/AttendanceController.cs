using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class AttendanceController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(MasarDbContext context, ILogger<AttendanceController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Attendance/Index
    public async Task<IActionResult> Index(DateTime? date)
    {
        var selectedDate = date ?? DateTime.Today;

        var attendanceRecords = await _context.AttendanceRecords
            .Include(a => a.Student)
                .ThenInclude(s => s.ClassRoom)
            .Include(a => a.Student)
                .ThenInclude(s => s.Guardian)
            .Include(a => a.Employee)
            .Include(a => a.ClassRoom)
            .Where(a => a.Date == selectedDate && !a.IsDeleted)
            .OrderBy(a => a.CheckInTime)
            .ToListAsync();

        ViewBag.SelectedDate = selectedDate;
        return View(attendanceRecords);
    }

    // GET: /Attendance/LiveLog
    public async Task<IActionResult> LiveLog()
    {
        var today = DateTime.Today;
        var recentAttendance = await _context.AttendanceRecords
            .Include(a => a.Student)
                .ThenInclude(s => s.ClassRoom)
            .Include(a => a.Employee)
            .Where(a => a.Date == today && !a.IsDeleted)
            .OrderByDescending(a => a.CheckInTime)
            .Take(50)
            .ToListAsync();

        return View(recentAttendance);
    }

    // GET: /Attendance/CallSystem
    public async Task<IActionResult> CallSystem()
    {
        var today = DateTime.Today;
        var absentStudents = await _context.AttendanceRecords
            .Include(a => a.Student)
                .ThenInclude(s => s.ClassRoom)
            .Include(a => a.Student)
                .ThenInclude(s => s.Guardian)
            .Where(a => a.Date == today &&
                       a.Status == "Absent" &&
                       !a.IsDeleted)
            .ToListAsync();

        // Order by client-side to avoid EF Core translation issues
        absentStudents = absentStudents
            .OrderBy(a => a.Student?.ClassRoom?.GradeLevelLegacy ?? string.Empty)
            .ThenBy(a => a.Student?.FullNameArabic ?? string.Empty)
            .ToList();

        return View(absentStudents);
    }

    // GET: /Attendance/ManualTake
    public async Task<IActionResult> ManualTake(Guid? classRoomId)
    {
        ViewBag.ClassRooms = new SelectList(
            await _context.ClassRooms
                .Include(c => c.Branch)
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.GradeLevelLegacy)
                .ThenBy(c => c.SectionLegacy)
                .ToListAsync(),
            "Id", "NameArabic");

        if (classRoomId.HasValue)
        {
            var students = await _context.Students
                .Include(s => s.ClassRoom)
                .Where(s => s.ClassRoomId == classRoomId.Value && s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.FullNameArabic)
                .ToListAsync();

            var today = DateTime.Today;
            var existingAttendance = await _context.AttendanceRecords
                .Where(a => a.Date == today &&
                           a.ClassRoomId == classRoomId.Value &&
                           !a.IsDeleted)
                .ToListAsync();

            ViewBag.SelectedClassRoomId = classRoomId;
            ViewBag.Students = students;
            ViewBag.ExistingAttendance = existingAttendance.ToDictionary(a => a.StudentId, a => a);
        }

        return View();
    }

    // POST: /Attendance/ManualTake
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManualTake(ManualAttendanceDto model)
    {
        if (!ModelState.IsValid)
        {
            return await ManualTake(model.ClassRoomId);
        }

        var today = DateTime.Today;
        var currentUserId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

        foreach (var attendance in model.AttendanceList)
        {
            var existingRecord = await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.Date == today && 
                                       a.StudentId == attendance.StudentId && 
                                       !a.IsDeleted);

            if (existingRecord != null)
            {
                existingRecord.Status = attendance.Status;
                existingRecord.Notes = attendance.Notes;
                existingRecord.UpdatedAt = DateTime.Now;
                existingRecord.UpdatedBy = User.Identity?.Name;
                _context.Update(existingRecord);
            }
            else
            {
                var newRecord = new AttendanceRecord
                {
                    Id = Guid.NewGuid(),
                    Date = today,
                    Status = attendance.Status,
                    Notes = attendance.Notes,
                    StudentId = attendance.StudentId,
                    ClassRoomId = model.ClassRoomId,
                    IsAutomated = false,
                    DeviceId = "MANUAL",
                    CreatedAt = DateTime.Now,
                    CreatedBy = User.Identity?.Name
                };

                _context.AttendanceRecords.Add(newRecord);
            }
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = "تم حفظ الحضور والغياب بنجاح";
        return RedirectToAction(nameof(Index), new { date = today });
    }
}

// DTOs
public class ManualAttendanceDto
{
    public Guid ClassRoomId { get; set; }
    public List<StudentAttendanceDto> AttendanceList { get; set; } = new();
}

public class StudentAttendanceDto
{
    public Guid StudentId { get; set; }
    public string Status { get; set; } = "Present";
    public string? Notes { get; set; }
}
