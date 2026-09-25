using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class TermClosureController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ITermClosingService _termClosingService;
    private readonly ILogger<TermClosureController> _logger;

    public TermClosureController(
        MasarDbContext context,
        ITermClosingService termClosingService,
        ILogger<TermClosureController> logger)
    {
        _context = context;
        _termClosingService = termClosingService;
        _logger = logger;
    }

    // GET: /TermClosure/Index
    public async Task<IActionResult> Index()
    {
        var activeTerms = await _context.AcademicTerms
            .Where(t => t.IsActive)
            .Include(t => t.School)
            .ToListAsync();

        var archives = await _termClosingService.GetAllArchivesAsync();

        var viewModel = new TermClosureIndexViewModel
        {
            ActiveTerms = activeTerms,
            Archives = archives
        };

        return View(viewModel);
    }

    // GET: /TermClosure/Close/{id}
    public async Task<IActionResult> Close(Guid id)
    {
        var term = await _context.AcademicTerms
            .Include(t => t.School)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (term == null)
        {
            return NotFound();
        }

        return View(term);
    }

    // POST: /TermClosure/Validate
    [HttpPost]
    public async Task<IActionResult> Validate(Guid termId)
    {
        try
        {
            var validation = await _termClosingService.ValidateBeforeClosingAsync(termId);
            return Json(new { success = true, data = validation });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating term closing");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /TermClosure/CloseTerm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseTerm(CloseAcademicTermRequest request)
    {
        try
        {
            var result = await _termClosingService.CloseAcademicTermAsync(request);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Close), new { id = request.TermId });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing academic term");
            TempData["Error"] = $"حدث خطأ: {ex.Message}";
            return RedirectToAction(nameof(Close), new { id = request.TermId });
        }
    }

    // GET: /TermClosure/Archives
    public async Task<IActionResult> Archives()
    {
        var archives = await _termClosingService.GetAllArchivesAsync();
        return View(archives);
    }

    // GET: /TermClosure/ArchiveDetails/{id}
    public async Task<IActionResult> ArchiveDetails(Guid id)
    {
        var archive = await _termClosingService.GetArchiveByIdAsync(id);
        if (archive == null)
        {
            return NotFound();
        }

        return View(archive);
    }

    // POST: /TermClosure/Restore/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            var result = await _termClosingService.RestoreArchiveAsync(id, User.Identity?.Name ?? "System");
            if (result)
            {
                TempData["Success"] = "تم استعادة الأرشيف بنجاح";
            }
            else
            {
                TempData["Error"] = "فشل استعادة الأرشيف";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring archive");
            TempData["Error"] = $"حدث خطأ: {ex.Message}";
        }

        return RedirectToAction(nameof(Archives));
    }
}

/// <summary>
/// ViewModel لصفحة الفهرس
/// </summary>
public class TermClosureIndexViewModel
{
    public List<AcademicTerm> ActiveTerms { get; set; } = new();
    public List<TermArchiveDto> Archives { get; set; } = new();
}
