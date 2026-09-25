using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class AcademicTermController : Controller
{
    private readonly MasarDbContext _context;
    private readonly IAcademicTermOpeningService _academicTermService;
    private readonly ILogger<AcademicTermController> _logger;

    public AcademicTermController(
        MasarDbContext context,
        IAcademicTermOpeningService academicTermService,
        ILogger<AcademicTermController> logger)
    {
        _context = context;
        _academicTermService = academicTermService;
        _logger = logger;
    }

    // GET: /AcademicTerm/Index
    public async Task<IActionResult> Index()
    {
        var terms = await _academicTermService.GetAllAcademicTermsAsync();
        return View(terms);
    }

    // GET: /AcademicTerm/Open
    public async Task<IActionResult> Open()
    {
        ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.GradeLevels = new SelectList(await _context.GradeLevels.Where(gl => !gl.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Grades = new SelectList(await _context.Grades.Where(g => !g.IsDeleted).ToListAsync(), "Id", "NameArabic");
        ViewBag.Sections = new SelectList(await _context.Sections.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");

        return View();
    }

    // POST: /AcademicTerm/GetSummary
    [HttpPost]
    public async Task<IActionResult> GetSummary(OpenAcademicTermRequest request)
    {
        try
        {
            var summary = await _academicTermService.GetOpeningSummaryAsync(request);
            return Json(new { success = true, data = summary });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting opening summary");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /AcademicTerm/OpenTerm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OpenTerm(OpenAcademicTermRequest request)
    {
        try
        {
            var result = await _academicTermService.OpenAcademicTermAsync(request);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.Message;
                ViewBag.Schools = new SelectList(await _context.Schools.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
                ViewBag.GradeLevels = new SelectList(await _context.GradeLevels.Where(gl => !gl.IsDeleted).ToListAsync(), "Id", "NameArabic");
                ViewBag.Grades = new SelectList(await _context.Grades.Where(g => !g.IsDeleted).ToListAsync(), "Id", "NameArabic");
                ViewBag.Sections = new SelectList(await _context.Sections.Where(s => !s.IsDeleted).ToListAsync(), "Id", "NameArabic");
                
                return View("Open", request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening academic term");
            TempData["Error"] = $"حدث خطأ: {ex.Message}";
            return RedirectToAction(nameof(Open));
        }
    }

    // GET: /AcademicTerm/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var term = await _academicTermService.GetAcademicTermByIdAsync(id);
        if (term == null)
        {
            return NotFound();
        }

        return View(term);
    }

    // POST: /AcademicTerm/Toggle/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(Guid id, bool isActive)
    {
        try
        {
            var result = await _academicTermService.ToggleAcademicTermStatusAsync(id, isActive);
            if (result)
            {
                TempData["Success"] = isActive ? "تم تفعيل الفصل الدراسي" : "تم تعطيل الفصل الدراسي";
            }
            else
            {
                TempData["Error"] = "فشل تغيير حالة الفصل الدراسي";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling academic term status");
            TempData["Error"] = $"حدث خطأ: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
