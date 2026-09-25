using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class GradesController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<GradesController> _logger;

    public GradesController(MasarDbContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Grades/Index
    public async Task<IActionResult> Index()
    {
        var classRooms = await _context.ClassRooms
            .Include(c => c.Branch)
            .Include(c => c.Branch.School)
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.GradeLevelLegacy)
            .ThenBy(c => c.SectionLegacy)
            .ToListAsync();

        ViewBag.CurrentTerm = GetCurrentTerm();
        return View(classRooms);
    }

    // GET: /Grades/ControlSheet
    public async Task<IActionResult> ControlSheet(Guid? classId, int termId)
    {
        if (classId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var classRoom = await _context.ClassRooms
            .Include(c => c.Branch)
            .Include(c => c.Branch.School)
            .FirstOrDefaultAsync(c => c.Id == classId.Value && !c.IsDeleted);

        if (classRoom == null)
        {
            return NotFound();
        }

        var students = await _context.Students
            .Where(s => s.ClassRoomId == classId.Value && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.FullNameArabic)
            .ToListAsync();

        var grades = await _context.Grades
            .Where(g => g.Student != null && g.Student.ClassRoomId == classId.Value && !g.IsDeleted)
            .Include(g => g.Student)
            .Include(g => g.Teacher)
            .ToListAsync();

        ViewBag.ClassRoom = classRoom;
        ViewBag.TermId = termId;
        ViewBag.Students = students;
        ViewBag.Grades = grades.ToDictionary(g => g.StudentId, g => g);

        return View();
    }

    // POST: /Grades/ControlSheet
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ControlSheet(GradeSheetDto model)
    {
        if (!ModelState.IsValid)
        {
            return await ControlSheet(model.ClassRoomId, model.TermId);
        }

        foreach (var grade in model.Grades)
        {
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == grade.StudentId && 
                                       g.Term == model.TermId.ToString() && 
                                       !g.IsDeleted);

            if (existingGrade != null)
            {
                existingGrade.MidtermScore = grade.MidtermScore;
                existingGrade.FinalScore = grade.FinalScore;
                existingGrade.TotalScore = grade.TotalScore;
                existingGrade.MaxScore = grade.MaxScore;
                existingGrade.Notes = grade.Notes;
                existingGrade.UpdatedAt = DateTime.Now;
                existingGrade.UpdatedBy = User.Identity?.Name;
                _context.Update(existingGrade);
            }
            else
            {
                var newGrade = new Grade
                {
                    Id = Guid.NewGuid(),
                    StudentId = grade.StudentId,
                    Term = model.TermId.ToString(),
                    Subject = model.Subject,
                    SubjectArabic = model.SubjectArabic,
                    TeacherId = model.TeacherId,
                    MidtermScore = grade.MidtermScore,
                    FinalScore = grade.FinalScore,
                    TotalScore = grade.TotalScore,
                    MaxScore = grade.MaxScore,
                    Notes = grade.Notes,
                    CreatedAt = DateTime.Now,
                    CreatedBy = User.Identity?.Name
                };

                _context.Grades.Add(newGrade);
            }
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = "تم حفظ الدرجات بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Grades/GenerateSeatingNumbers
    public async Task<IActionResult> GenerateSeatingNumbers(Guid? classId)
    {
        if (classId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var classRoom = await _context.ClassRooms.FindAsync(classId.Value);
        if (classRoom == null)
        {
            return NotFound();
        }

        var students = await _context.Students
            .Where(s => s.ClassRoomId == classId.Value && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.FullNameArabic)
            .ToListAsync();

        var seatingNumbers = new List<SeatingNumberDto>();
        var random = new Random();

        for (int i = 0; i < students.Count; i++)
        {
            seatingNumbers.Add(new SeatingNumberDto
            {
                StudentId = students[i].Id,
                StudentName = students[i].FullNameArabic,
                SeatingNumber = random.Next(1000, 9999),
                SecretNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            });
        }

        ViewBag.ClassRoom = classRoom;
        return View("SeatingNumbers", seatingNumbers);
    }

    // GET: /Grades/Certificates
    public async Task<IActionResult> Certificates(Guid? classId)
    {
        if (classId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var classRoom = await _context.ClassRooms
            .Include(c => c.Branch)
            .Include(c => c.Branch.School)
            .FirstOrDefaultAsync(c => c.Id == classId.Value && !c.IsDeleted);

        if (classRoom == null)
        {
            return NotFound();
        }

        var students = await _context.Students
            .Include(s => s.Grades.Where(g => !g.IsDeleted))
            .Where(s => s.ClassRoomId == classId.Value && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.FullNameArabic)
            .ToListAsync();

        ViewBag.ClassRoom = classRoom;
        return View(students);
    }

    private int GetCurrentTerm()
    {
        var month = DateTime.Now.Month;
        return month >= 9 ? 1 : (month >= 1 && month <= 6 ? 2 : 3);
    }
}

// DTOs
public class GradeSheetDto
{
    public Guid ClassRoomId { get; set; }
    public int TermId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string SubjectArabic { get; set; } = string.Empty;
    public Guid? TeacherId { get; set; }
    public List<StudentGradeDto> Grades { get; set; } = new();
}

public class StudentGradeDto
{
    public Guid StudentId { get; set; }
    public decimal? MidtermScore { get; set; }
    public decimal? FinalScore { get; set; }
    public decimal? TotalScore { get; set; }
    public decimal? MaxScore { get; set; }
    public string? Notes { get; set; }
}

public class SeatingNumberDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int SeatingNumber { get; set; }
    public string SecretNumber { get; set; } = string.Empty;
}
