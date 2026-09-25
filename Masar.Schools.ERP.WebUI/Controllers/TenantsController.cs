using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class TenantsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<TenantsController> _logger;
    private readonly IWebHostEnvironment _environment;

    public TenantsController(MasarDbContext context, ILogger<TenantsController> logger, IWebHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    private async Task<string> UploadLogoFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        try
        {
            // إنشاء مجلد uploads/logos إذا لم يكن موجوداً
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "logos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // إنشاء اسم فريد للملف
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // نسخ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/logos/" + uniqueFileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading logo file");
            return null;
        }
    }

    // GET: /Tenants
    public async Task<IActionResult> Index()
    {
        var tenants = await _context.Tenants
            .Include(t => t.Schools.Where(s => !s.IsDeleted))
            .Include(t => t.MasarUsers)
            .Include(t => t.MasarRoles)
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.NameArabic)
            .ToListAsync();

        return View(tenants);
    }

    // GET: /Tenants/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tenants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tenant model, IFormFile? logoFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // رفع ملف الشعار إذا تم اختياره
            string logoPath = null;
            if (logoFile != null && logoFile.Length > 0)
            {
                logoPath = await UploadLogoFile(logoFile);
            }
            else if (!string.IsNullOrEmpty(Request.Form["LogoPath"]))
            {
                logoPath = Request.Form["LogoPath"];
            }

            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = model.Name ?? string.Empty,
                NameArabic = model.NameArabic ?? string.Empty,
                LicenseNumber = model.LicenseNumber ?? string.Empty,
                LogoPath = logoPath,
                PrimaryColor = model.PrimaryColor,
                SecondaryColor = model.SecondaryColor,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                IsActive = true,
                SubscriptionStartDate = model.SubscriptionStartDate,
                SubscriptionEndDate = model.SubscriptionEndDate,
                ZatcaCertificatePath = model.ZatcaCertificatePath,
                ZatcaSecretKey = model.ZatcaSecretKey,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إضافة المؤسسة بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant");
            ModelState.AddModelError("", "حدث خطأ أثناء حفظ المؤسسة: " + ex.Message);
            return View(model);
        }
    }

    // GET: /Tenants/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null || tenant.IsDeleted)
        {
            return NotFound();
        }

        return View(tenant);
    }

    // POST: /Tenants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Tenant model, IFormFile? logoFile)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null || tenant.IsDeleted)
            {
                return NotFound();
            }

            // رفع ملف الشعار الجديد إذا تم اختياره
            if (logoFile != null && logoFile.Length > 0)
            {
                var newLogoPath = await UploadLogoFile(logoFile);
                if (!string.IsNullOrEmpty(newLogoPath))
                {
                    // حذف الشعار القديم إذا كان موجوداً
                    if (!string.IsNullOrEmpty(tenant.LogoPath))
                    {
                        var oldLogoPath = Path.Combine(_environment.WebRootPath, tenant.LogoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldLogoPath))
                        {
                            System.IO.File.Delete(oldLogoPath);
                        }
                    }
                    tenant.LogoPath = newLogoPath;
                }
            }
            // إذا لم يتم اختيار ملف جديد، احتفظ بالشعار القديم
            // لا نعدل tenant.LogoPath إذا لم يتم رفع ملف جديد

            tenant.Name = model.Name ?? string.Empty;
            tenant.NameArabic = model.NameArabic ?? string.Empty;
            tenant.LicenseNumber = model.LicenseNumber ?? string.Empty;
            tenant.PrimaryColor = model.PrimaryColor;
            tenant.SecondaryColor = model.SecondaryColor;
            tenant.Email = model.Email;
            tenant.Phone = model.Phone;
            tenant.Address = model.Address;
            tenant.City = model.City;
            tenant.IsActive = model.IsActive;
            tenant.SubscriptionStartDate = model.SubscriptionStartDate;
            tenant.SubscriptionEndDate = model.SubscriptionEndDate;
            tenant.ZatcaCertificatePath = model.ZatcaCertificatePath;
            tenant.ZatcaSecretKey = model.ZatcaSecretKey;
            tenant.UpdatedAt = DateTime.Now;
            tenant.UpdatedBy = User.Identity?.Name;

            _context.Update(tenant);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تحديث بيانات المؤسسة بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث المؤسسة: " + ex.Message);
            return View(model);
        }
    }

    // GET: /Tenants/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants
            .Include(t => t.Schools.Where(s => !s.IsDeleted))
            .Include(t => t.MasarUsers)
            .Include(t => t.MasarRoles)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tenant == null)
        {
            return NotFound();
        }

        return View(tenant);
    }

    // POST: /Tenants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }

        // Check if tenant has schools or users
        var hasSchools = await _context.Schools.AnyAsync(s => s.TenantId == id && !s.IsDeleted);
        var hasUsers = await _context.MasarUsers.AnyAsync(u => u.TenantId == id);
        var hasRoles = await _context.MasarRoles.AnyAsync(r => r.TenantId == id);

        if (hasSchools || hasUsers || hasRoles)
        {
            TempData["Error"] = "لا يمكن حذف المؤسسة لأنها تحتوي على مدارس أو مستخدمين أو أدوار";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            tenant.IsDeleted = true;
            tenant.DeletedAt = DateTime.Now;

            _context.Update(tenant);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حذف المؤسسة بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tenant");
            TempData["Error"] = "حدث خطأ أثناء حذف المؤسسة: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Tenants/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tenant = await _context.Tenants
            .Include(t => t.Schools.Where(s => !s.IsDeleted))
            .Include(t => t.MasarUsers)
            .Include(t => t.MasarRoles)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (tenant == null)
        {
            return NotFound();
        }

        return View(tenant);
    }
}