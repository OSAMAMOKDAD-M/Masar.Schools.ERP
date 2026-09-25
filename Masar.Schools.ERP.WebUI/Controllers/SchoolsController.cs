using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class SchoolsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<SchoolsController> _logger;

    public SchoolsController(MasarDbContext context, ILogger<SchoolsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Schools
    public async Task<IActionResult> Index()
    {
        var schools = await _context.Schools
            .Include(s => s.Branches)
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.NameArabic)
            .ToListAsync();

        return View(schools);
    }

    // GET: /Schools/Create
    public async Task<IActionResult> Create()
    {
        var tenants = await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();
        
        if (tenants.Count == 0)
        {
            TempData["Error"] = "لا توجد مؤسسات في النظام. يجب إضافة مؤسسة أولاً.";
            return RedirectToAction("Create", "Tenants");
        }

        ViewBag.Tenants = new SelectList(tenants, "Id", "NameArabic");
        
        // If there's only one tenant, pre-select it and return a model with that tenant
        if (tenants.Count == 1)
        {
            var model = new School
            {
                TenantId = tenants.First().Id
            };
            return View(model);
        }

        return View(new School());
    }

    // POST: /Schools/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(School model)
    {
        // Log incoming data for debugging
        _logger.LogInformation($"Create School POST - Name: {model.Name}, NameArabic: {model.NameArabic}, TenantId: {model.TenantId}");
        _logger.LogInformation($"Form data count: {Request.Form.Count}");
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation($"Form key: {key}, value: {Request.Form[key]}");
        }

        // Check if tenants exist
        var tenants = await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();
        _logger.LogInformation($"Available tenants count: {tenants.Count}");
        
        if (tenants.Count == 0)
        {
            TempData["Error"] = "لا توجد مؤسسات في النظام. يجب إضافة مؤسسة أولاً.";
            return RedirectToAction("Create", "Tenants");
        }

        // Clear ALL validation errors and start fresh
        ModelState.Clear();

        // Manual validation for required fields
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError("Name", "اسم المدرسة (إنجليزي) مطلوب");
        }
        
        if (string.IsNullOrWhiteSpace(model.NameArabic))
        {
            ModelState.AddModelError("NameArabic", "اسم المدرسة (عربي) مطلوب");
        }

        // Check if TenantId is provided
        if (model.TenantId == null || model.TenantId == Guid.Empty)
        {
            _logger.LogError("TenantId is null or empty in POST request");
            ModelState.AddModelError("TenantId", "يجب اختيار المؤسسة");
        }
        else
        {
            _logger.LogInformation($"TenantId received: {model.TenantId}");
        }

        // Log ModelState errors for debugging
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError($"Validation Error: {error.ErrorMessage}");
            }

            ViewBag.Tenants = new SelectList(tenants, "Id", "NameArabic");
            return View(model);
        }

        try
        {
            var school = new School
            {
                Id = Guid.NewGuid(),
                Name = model.Name ?? string.Empty,
                NameArabic = model.NameArabic ?? string.Empty,
                Code = model.Code,
                NoorSchoolId = model.NoorSchoolId,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                IsActive = true,
                TenantId = model.TenantId ?? Guid.Empty,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name
            };

            _context.Schools.Add(school);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إضافة المدرسة بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating school");
            ModelState.AddModelError("", "حدث خطأ أثناء حفظ المدرسة: " + ex.Message);
            
            ViewBag.Tenants = new SelectList(tenants, "Id", "NameArabic");
            return View(model);
        }
    }

    // GET: /Schools/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var school = await _context.Schools.FindAsync(id);
        if (school == null || school.IsDeleted)
        {
            return NotFound();
        }

        ViewBag.Tenants = new SelectList(
            await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync(),
            "Id", "NameArabic", school.TenantId);

        return View(school);
    }

    // POST: /Schools/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, School model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Tenants = new SelectList(
                await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync(),
                "Id", "NameArabic", model.TenantId);
            return View(model);
        }

        var school = await _context.Schools.FindAsync(id);
        if (school == null || school.IsDeleted)
        {
            return NotFound();
        }

        school.Name = model.Name;
        school.NameArabic = model.NameArabic;
        school.Code = model.Code;
        school.NoorSchoolId = model.NoorSchoolId;
        school.Email = model.Email;
        school.Phone = model.Phone;
        school.Address = model.Address;
        school.City = model.City;
        school.IsActive = model.IsActive;
        school.TenantId = model.TenantId ?? school.TenantId;
        school.UpdatedAt = DateTime.Now;
        school.UpdatedBy = User.Identity?.Name;

        _context.Update(school);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم تحديث بيانات المدرسة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Schools/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var school = await _context.Schools
            .Include(s => s.Students)
            .Include(s => s.Employees)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (school == null)
        {
            return NotFound();
        }

        return View(school);
    }

    // POST: /Schools/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var school = await _context.Schools.FindAsync(id);
        if (school == null)
        {
            return NotFound();
        }

        // Check if school has students or employees
        var hasStudents = await _context.Students.AnyAsync(s => s.SchoolId == id && !s.IsDeleted);
        var hasEmployees = await _context.Employees.AnyAsync(e => e.SchoolId == id && !e.IsDeleted);

        if (hasStudents || hasEmployees)
        {
            TempData["Error"] = "لا يمكن حذف المدرسة لأنها تحتوي على طلاب أو موظفين";
            return RedirectToAction(nameof(Index));
        }

        school.IsDeleted = true;
        school.DeletedAt = DateTime.Now;

        _context.Update(school);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم حذف المدرسة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Schools/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var school = await _context.Schools
            .Include(s => s.Branches)
            .Include(s => s.Students.Where(s => !s.IsDeleted))
            .Include(s => s.Employees.Where(e => !e.IsDeleted))
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (school == null)
        {
            return NotFound();
        }

        return View(school);
    }
}