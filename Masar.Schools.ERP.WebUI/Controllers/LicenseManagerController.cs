using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Licensing.Models;
using Masar.Schools.ERP.Licensing.Services;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller إدارة التراخيص (للسوبر أدمن فقط)
/// </summary>
[Authorize]
public class LicenseManagerController : Controller
{
    private readonly LicenseEngine _licenseEngine;
    private readonly ILogger<LicenseManagerController> _logger;

    public LicenseManagerController(
        LicenseEngine licenseEngine,
        ILogger<LicenseManagerController> logger)
    {
        _licenseEngine = licenseEngine;
        _logger = logger;
    }

    // GET: /Admin/LicenseManager
    public IActionResult Index()
    {
        // عرض قائمة جميع التراخيص الصادرة
        // يجب الاتصال بقاعدة بيانات السوبر أدمن هنا
        var model = new LicenseIndexViewModel
        {
            Licenses = new List<LicenseSummaryViewModel>() // تجريبي - يجب استبداله ببيانات حقيقية
        };
        return View(model);
    }

    // GET: /Admin/LicenseManager/Generate
    public IActionResult Generate()
    {
        var model = new LicenseGenerateViewModel
        {
            ValidFrom = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddYears(1),
            MaxStudents = 100,
            MaxUsers = 10,
            LicenseType = LicenseType.Standard,
            AvailableModules = Enum.GetValues<LicenseModules>().Cast<LicenseModules>().ToList()
        };
        return View(model);
    }

    // POST: /Admin/LicenseManager/Generate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Generate(LicenseGenerateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableModules = Enum.GetValues<LicenseModules>().Cast<LicenseModules>().ToList();
            return View(model);
        }

        try
        {
            // جمع الموديولات المفعلة
            var selectedModules = LicenseModules.None;
            if (model.SelectedModules != null)
            {
                foreach (var module in model.SelectedModules)
                {
                    selectedModules |= (LicenseModules)Enum.Parse(typeof(LicenseModules), module);
                }
            }

            // إنشاء نموذج الترخيص
            var license = new LicenseModel
            {
                LicenseId = Guid.NewGuid(),
                ClientName = model.ClientName,
                MachineId = model.MachineId,
                ValidFrom = model.ValidFrom,
                ValidUntil = model.ValidUntil,
                MaxStudents = model.MaxStudents,
                MaxUsers = model.MaxUsers,
                Modules = selectedModules,
                LicenseType = model.LicenseType,
                CreatedAt = DateTime.UtcNow,
                IssuedBy = User.Identity?.Name ?? "SuperAdmin",
                Notes = model.Notes
            };

            // توليد مفتاح الترخيص (باستخدام المفتاح الخاص المخزن في إعدادات السوبر أدمن)
            string privateKey = GetSuperAdminPrivateKey(); // يجب استرجاعه من مكان آمن
            string licenseKey = _licenseEngine.GenerateLicenseKey(license, privateKey);

            // حفظ الترخيص في قاعدة البيانات المركزية
            SaveLicenseToDatabase(license);

            // عرض المفتاح للسوبر أدمن
            ViewBag.LicenseKey = licenseKey;
            ViewBag.LicenseId = license.LicenseId;
            
            _logger.LogInformation("تم توليد ترخيص جديد للعميل: {ClientName}, LicenseId: {LicenseId}", 
                model.ClientName, license.LicenseId);

            return View("GenerateSuccess", license);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في توليد الترخيص");
            ModelState.AddModelError("", "حدث خطأ في توليد الترخيص");
            model.AvailableModules = Enum.GetValues<LicenseModules>().Cast<LicenseModules>().ToList();
            return View(model);
        }
    }

    // POST: /Admin/LicenseManager/Revoke/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Revoke(Guid? licenseId)
    {
        if (licenseId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // إلغاء الترخيص في قاعدة البيانات المركزية
            RevokeLicenseInDatabase(licenseId.Value);

            _logger.LogInformation("تم إلغاء الترخيص: {LicenseId}", licenseId.Value);

            TempData["Success"] = "تم إلغاء الترخيص بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في إلغاء الترخيص");
            TempData["Error"] = "حدث خطأ في إلغاء الترخيص";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Admin/LicenseManager/Details/5
    public IActionResult Details(Guid? licenseId)
    {
        if (licenseId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // عرض تفاصيل الترخيص
        var license = GetLicenseFromDatabase(licenseId.Value);
        if (license == null)
        {
            return NotFound();
        }
        return View(license);
    }

    // GET: /Admin/LicenseManager/GenerateKeys
    public IActionResult GenerateKeys()
    {
        // توليد زوج مفاتيح RSA جديد
        var (privateKey, publicKey) = _licenseEngine.GenerateRsaKeyPair();
        
        var model = new KeyPairViewModel
        {
            PrivateKey = privateKey,
            PublicKey = publicKey
        };
        
        return View(model);
    }

    #region Helper Methods (تجريبية - يجب استبدالها بتنفيذ حقيقي)

    private string GetSuperAdminPrivateKey()
    {
        // في الإنتاج، يجب استرجاع المفتاح الخاص من مكان آمن
        // مثل Azure Key Vault أو ملف مشفر بكلمة مرور قوية
        // حالياً سنستخدم مفتاح تجريبي
        return "MIIEpAIBAAKCAQEA..."; // تجريبي
    }

    private void SaveLicenseToDatabase(LicenseModel license)
    {
        // في الإنتاج، يجب حفظ الترخيص في قاعدة بيانات السوبر أدمن
        _logger.LogInformation("حفظ الترخيص في قاعدة البيانات: {LicenseId}", license.LicenseId);
    }

    private void RevokeLicenseInDatabase(Guid licenseId)
    {
        // في الإنتاج، يجب إلغاء الترخيص في قاعدة البيانات
        _logger.LogInformation("إلغاء الترخيص في قاعدة البيانات: {LicenseId}", licenseId);
    }

    private LicenseModel? GetLicenseFromDatabase(Guid licenseId)
    {
        // في الإنتاج، يجب استرجاع الترخيص من قاعدة البيانات
        return null;
    }

    #endregion
}

// ViewModels
public class LicenseIndexViewModel
{
    public List<LicenseSummaryViewModel> Licenses { get; set; } = new();
}

public class LicenseSummaryViewModel
{
    public Guid LicenseId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string MachineId { get; set; } = string.Empty;
    public DateTime ValidUntil { get; set; }
    public LicenseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LicenseGenerateViewModel
{
    public string ClientName { get; set; } = string.Empty;
    public string MachineId { get; set; } = string.Empty;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public int MaxStudents { get; set; }
    public int MaxUsers { get; set; }
    public LicenseType LicenseType { get; set; }
    public List<string>? SelectedModules { get; set; }
    public List<LicenseModules> AvailableModules { get; set; } = new();
    public string? Notes { get; set; }
}

public class KeyPairViewModel
{
    public string PrivateKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
}