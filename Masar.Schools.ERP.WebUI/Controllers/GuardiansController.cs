using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class GuardiansController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<GuardiansController> _logger;

    public GuardiansController(MasarDbContext context, ILogger<GuardiansController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Guardians/Index
    public async Task<IActionResult> Index(GuardianFilterDto filter)
    {
        var query = _context.Guardians
            .Include(g => g.Tenant)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.ClassRoom)
            .Where(g => !g.IsDeleted);

        // Apply search filter
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            query = query.Where(g =>
                g.FullName.Contains(filter.SearchTerm) ||
                g.FullNameArabic.Contains(filter.SearchTerm) ||
                g.NationalId.Contains(filter.SearchTerm) ||
                g.PhoneNumber.Contains(filter.SearchTerm) ||
                g.Email.Contains(filter.SearchTerm));
        }

        // Apply tenant filter
        if (filter.TenantId.HasValue)
        {
            query = query.Where(g => g.TenantId == filter.TenantId.Value);
        }

        // Apply school filter (via students)
        if (filter.SchoolId.HasValue)
        {
            query = query.Where(g => g.Students.Any(s => s.SchoolId == filter.SchoolId.Value));
        }

        // Apply class filter (via students)
        if (filter.ClassRoomId.HasValue)
        {
            query = query.Where(g => g.Students.Any(s => s.ClassRoomId == filter.ClassRoomId.Value));
        }

        // Apply children count filter
        if (filter.MinChildren.HasValue)
        {
            query = query.Where(g => g.Students.Count >= filter.MinChildren.Value);
        }

        var guardians = await query
            .OrderBy(g => g.FullNameArabic)
            .ToListAsync();

        // Populate filter dropdowns
        ViewBag.Tenants = new SelectList(await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync(), "Id", "Name", filter.TenantId);
        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic", filter.SchoolId);
        ViewBag.ClassRooms = new SelectList(await _context.ClassRooms.Where(c => !c.IsDeleted).ToListAsync(), "Id", "NameArabic", filter.ClassRoomId);

        return View(guardians);
    }

    // GET: /Guardians/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Tenants = new SelectList(await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync(), "Id", "Name");
        return View();
    }

    // POST: /Guardians/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GuardianCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Tenants = new SelectList(await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync(), "Id", "Name");
            return View(model);
        }

        var guardian = new Guardian
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            FirstNameArabic = model.FirstNameArabic,
            LastName = model.LastName,
            LastNameArabic = model.LastNameArabic,
            FullName = $"{model.FirstName} {model.LastName}",
            FullNameArabic = $"{model.FirstNameArabic} {model.LastNameArabic}",
            NationalId = model.NationalId,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Address = model.Address,
            Occupation = model.Occupation,
            OccupationArabic = model.OccupationArabic,
            Relationship = model.Relationship,
            RelationshipArabic = model.RelationshipArabic,
            WhatsAppNumber = model.WhatsAppNumber,
            TenantId = model.TenantId,
            IsActive = true,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.Guardians.Add(guardian);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إضافة ولي الأمر بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Guardians/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var guardian = await _context.Guardians
            .Include(g => g.Tenant)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.School)
            .Include(g => g.Students.Where(s => !s.IsDeleted))
                .ThenInclude(s => s.ClassRoom)
            .FirstOrDefaultAsync(g => g.Id == id.Value && !g.IsDeleted);

        if (guardian == null)
        {
            return NotFound();
        }

        return View(guardian);
    }

    // GET: /Guardians/LinkStudent
    public async Task<IActionResult> LinkStudent(Guid? guardianId)
    {
        if (guardianId == null)
        {
            // If no ID provided, get first available guardian
            var firstGuardian = await _context.Guardians
                .Where(g => !g.IsDeleted && g.IsActive)
                .FirstOrDefaultAsync();
            
            if (firstGuardian != null)
            {
                return RedirectToAction(nameof(LinkStudent), new { guardianId = firstGuardian.Id });
            }
            
            return RedirectToAction(nameof(Index));
        }

        var guardian = await _context.Guardians.FindAsync(guardianId.Value);
        if (guardian == null)
        {
            return NotFound();
        }

        var availableStudents = await _context.Students
            .Where(s => !s.IsDeleted && s.GuardianId == null)
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .ToListAsync();

        ViewBag.Guardian = guardian;
        return View(availableStudents);
    }

    // POST: /Guardians/LinkStudent
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkStudent(Guid? guardianId, Guid? studentId)
    {
        if (guardianId == null || studentId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students.FindAsync(studentId.Value);
        if (student == null)
        {
            return NotFound();
        }

        student.GuardianId = guardianId.Value;
        student.UpdatedAt = DateTime.Now;
        student.UpdatedBy = User.Identity?.Name;

        _context.Update(student);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم ربط الطالب بولي الأمر بنجاح";
        return RedirectToAction(nameof(Details), new { id = guardianId.Value });
    }

    // POST: /Guardians/UnlinkStudent
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnlinkStudent(Guid? guardianId, Guid? studentId)
    {
        if (guardianId == null || studentId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students.FindAsync(studentId.Value);
        if (student == null)
        {
            return NotFound();
        }

        student.GuardianId = null;
        student.UpdatedAt = DateTime.Now;
        student.UpdatedBy = User.Identity?.Name;

        _context.Update(student);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم فك ربط الطالب بنجاح";
        return RedirectToAction(nameof(Details), new { id = guardianId.Value });
    }
}

// DTOs
public class GuardianCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Occupation { get; set; }
    public string? OccupationArabic { get; set; }
    public string? Relationship { get; set; }
    public string? RelationshipArabic { get; set; }
    public string? WhatsAppNumber { get; set; }
    public Guid TenantId { get; set; }
}

public class GuardianFilterDto
{
    public string? SearchTerm { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? SchoolId { get; set; }
    public Guid? ClassRoomId { get; set; }
    public int? MinChildren { get; set; }
}
