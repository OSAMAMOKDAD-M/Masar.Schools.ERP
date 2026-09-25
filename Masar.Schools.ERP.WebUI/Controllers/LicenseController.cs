using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Licensing.Models;
using Masar.Schools.ERP.Licensing.Services;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller واجهة تفعيل الترخيص (للعميل)
/// </summary>
public class LicenseController : Controller
{
    private readonly ILicenseService _licenseService;
    private readonly HardwareFingerprintService _fingerprintService;
    private readonly ILogger<LicenseController> _logger;

    public LicenseController(
        ILicenseService licenseService,
        HardwareFingerprintService fingerprintService,
        ILogger<LicenseController> logger)
    {
        _licenseService = licenseService;
        _fingerprintService = fingerprintService;
        _logger = logger;
    }

    // GET: /License/Activation
    public IActionResult Activation()
    {
        var model = new LicenseActivationViewModel
        {
            MachineId = _fingerprintService.GenerateMachineId(),
            HardwareInfo = _fingerprintService.GetHardwareInfo()
        };
        return View(model);
    }

    // POST: /License/Activation
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activation(LicenseActivationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.MachineId = _fingerprintService.GenerateMachineId();
            model.HardwareInfo = _fingerprintService.GetHardwareInfo();
            return View(model);
        }

        try
        {
            bool activated = await _licenseService.ActivateLicenseAsync(model.LicenseKey);
            
            if (activated)
            {
                TempData["Success"] = "تم تفعيل الترخيص بنجاح";
                return RedirectToAction(nameof(Status));
            }
            else
            {
                ModelState.AddModelError("", "مفتاح الترخيص غير صحيح أو لا يطابق هذا الجهاز");
                model.MachineId = _fingerprintService.GenerateMachineId();
                model.HardwareInfo = _fingerprintService.GetHardwareInfo();
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في تفعيل الترخيص");
            ModelState.AddModelError("", "حدث خطأ في تفعيل الترخيص");
            model.MachineId = _fingerprintService.GenerateMachineId();
            model.HardwareInfo = _fingerprintService.GetHardwareInfo();
            return View(model);
        }
    }

    // GET: /License/Status
    public async Task<IActionResult> Status()
    {
        var validationResult = await _licenseService.ValidateCurrentLicenseAsync();
        var license = await _licenseService.GetCurrentLicenseAsync();

        var model = new LicenseStatusViewModel
        {
            IsValid = validationResult.IsValid,
            Status = validationResult.Status,
            Message = validationResult.Message,
            License = license,
            MachineId = _fingerprintService.GenerateMachineId(),
            HardwareInfo = _fingerprintService.GetHardwareInfo()
        };

        return View(model);
    }

    // POST: /License/Deactivate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate()
    {
        try
        {
            bool deactivated = await _licenseService.DeactivateLicenseAsync();
            
            if (deactivated)
            {
                TempData["Success"] = "تم إلغاء تفعيل الترخيص";
                return RedirectToAction(nameof(Activation));
            }
            else
            {
                TempData["Error"] = "لا يوجد ترخيص مفعل";
                return RedirectToAction(nameof(Status));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في إلغاء تفعيل الترخيص");
            TempData["Error"] = "حدث خطأ في إلغاء تفعيل الترخيص";
            return RedirectToAction(nameof(Status));
        }
    }

    // GET: /License/DownloadTemplate
    public IActionResult DownloadTemplate()
    {
        // تنزيل نموذج طلب ترخيص
        var template = GenerateLicenseRequestTemplate();
        return File(template, "text/plain", "LicenseRequestTemplate.txt");
    }

    private byte[] GenerateLicenseRequestTemplate()
    {
        var template = $@"نموذج طلب ترخيص لنظام مَسَار للمدارس
==================================================

تاريخ الطلب: {DateTime.Now:yyyy-MM-dd}

معلومات المدرسة:
----------------
اسم المدرسة: ___________________________________
رقم السجل التجاري: _______________________________
العنوان: _________________________________________
رقم الهاتف: _____________________________________
البريد الإلكتروني: _______________________________

معلومات الجهاز:
----------------
كود الجهاز (Machine ID): {_fingerprintService.GenerateMachineId()}
نظام التشغيل: {Environment.OSVersion.VersionString}
معالج: {Environment.ProcessorCount} أنوية
الذاكرة: {Environment.WorkingSet / (1024 * 1024)} MB

الترخيص المطلوب:
----------------
نوع الترخيص: [ ] تجريبي [ ] قياسي [ ] احترافي [ ] مؤسسي
عدد الطلاب المطلوب: _________
عدد المستخدمين المطلوب: _________
تاريخ البدء المطلوب: _________
تاريخ الانتهاء المطلوب: _________

الموديولات المطلوبة:
----------------------
[ ] نظام ZATCA للفواتير
[ ] تكامل واتساب
[ ] نظام النقل والمواصلات
[ ] العيادة المدرسية
[ ] المقصف المدرسي
[ ] الجدول المدرسي
[ ] السلوك والمواظبة
[ ] التقارير المتقدمة

ملاحظات إضافية:
__________________
__________________
__________________

التوقيع: _____________________
الختم: _____________________
";
        return System.Text.Encoding.UTF8.GetBytes(template);
    }
}

// ViewModels
public class LicenseActivationViewModel
{
    public string MachineId { get; set; } = string.Empty;
    public HardwareInfo HardwareInfo { get; set; } = new();
    public string LicenseKey { get; set; } = string.Empty;
}

public class LicenseStatusViewModel
{
    public bool IsValid { get; set; }
    public LicenseStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public LicenseModel? License { get; set; }
    public string MachineId { get; set; } = string.Empty;
    public HardwareInfo HardwareInfo { get; set; } = new();
}