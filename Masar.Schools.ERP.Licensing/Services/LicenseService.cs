using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Masar.Schools.ERP.Licensing.Models;

namespace Masar.Schools.ERP.Licensing.Services;

/// <summary>
/// تنفيذ خدمة الترخيص
/// </summary>
public class LicenseService : ILicenseService
{
    private readonly ILogger<LicenseService> _logger;
    private readonly HardwareFingerprintService _fingerprintService;
    private readonly LicenseEngine _licenseEngine;
    private readonly LicenseOptions _options;
    private readonly string _licenseFilePath;

    public LicenseService(
        ILogger<LicenseService> logger,
        HardwareFingerprintService fingerprintService,
        LicenseEngine licenseEngine,
        IOptions<LicenseOptions> options)
    {
        _logger = logger;
        _fingerprintService = fingerprintService;
        _licenseEngine = licenseEngine;
        _options = options.Value;
        
        // مسار ملف الترخيص
        _licenseFilePath = Path.Combine(_options.LicenseDirectory, "license.masarlic");
    }

    /// <summary>
    /// التحقق من الترخيص الحالي
    /// </summary>
    public async Task<LicenseValidationResult> ValidateCurrentLicenseAsync()
    {
        try
        {
            // التحقق من وجود ملف الترخيص
            if (!File.Exists(_licenseFilePath))
            {
                return LicenseValidationResult.Failure(LicenseStatus.NotActivated, "لا يوجد ملف ترخيص");
            }

            // قراءة ملف الترخيص
            string licenseKey = await File.ReadAllTextAsync(_licenseFilePath);

            // الحصول على بصمة الجهاز الحالية
            string currentMachineId = _fingerprintService.GenerateMachineId();

            // التحقق من الترخيص
            var result = _licenseEngine.ValidateLicense(licenseKey, _options.PublicKey, currentMachineId);

            if (result.IsValid && result.LicenseData != null)
            {
                // التحقق من عدم إلغاء الترخيص
                if (await IsLicenseRevokedAsync(result.LicenseData.LicenseId))
                {
                    return LicenseValidationResult.Failure(LicenseStatus.Revoked, "الترخيص ملغى");
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في التحقق من الترخيص");
            return LicenseValidationResult.Failure(LicenseStatus.Corrupted, $"خطأ في قراءة الترخيص: {ex.Message}");
        }
    }

    /// <summary>
    /// التحقق من تفعيل موديول معين
    /// </summary>
    public async Task<bool> IsModuleEnabledAsync(LicenseModules module)
    {
        var validationResult = await ValidateCurrentLicenseAsync();
        
        if (!validationResult.IsValid || validationResult.LicenseData == null)
        {
            return false;
        }

        return (validationResult.LicenseData.Modules & module) == module;
    }

    /// <summary>
    /// الحصول على بيانات الترخيص الحالي
    /// </summary>
    public async Task<LicenseModel?> GetCurrentLicenseAsync()
    {
        var validationResult = await ValidateCurrentLicenseAsync();
        return validationResult.IsValid ? validationResult.LicenseData : null;
    }

    /// <summary>
    /// تفعيل الترخيص
    /// </summary>
    public async Task<bool> ActivateLicenseAsync(string licenseKey)
    {
        try
        {
            // التحقق من المفتاح
            string currentMachineId = _fingerprintService.GenerateMachineId();
            var validationResult = _licenseEngine.ValidateLicense(licenseKey, _options.PublicKey, currentMachineId);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("فشل تفعيل الترخيص: {Status} - {Message}", 
                    validationResult.Status, validationResult.Message);
                return false;
            }

            // التحقق من عدم إلغاء الترخيص
            if (validationResult.LicenseData != null && await IsLicenseRevokedAsync(validationResult.LicenseData.LicenseId))
            {
                _logger.LogWarning("محاولة تفعيل ترخيص ملغى: {LicenseId}", validationResult.LicenseData.LicenseId);
                return false;
            }

            // إنشاء مجلد الترخيص إذا لم يكن موجوداً
            Directory.CreateDirectory(_options.LicenseDirectory);

            // حفظ ملف الترخيص
            await File.WriteAllTextAsync(_licenseFilePath, licenseKey);

            _logger.LogInformation("تم تفعيل الترخيص بنجاح للعميل: {ClientName}", 
                validationResult.LicenseData?.ClientName);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في تفعيل الترخيص");
            return false;
        }
    }

    /// <summary>
    /// إلغاء الترخيص
    /// </summary>
    public async Task<bool> DeactivateLicenseAsync()
    {
        try
        {
            if (File.Exists(_licenseFilePath))
            {
                await Task.Run(() => File.Delete(_licenseFilePath));
                _logger.LogInformation("تم إلغاء تفعيل الترخيص");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في إلغاء تفعيل الترخيص");
            return false;
        }
    }

    /// <summary>
    /// التحقق من إلغاء الترخيص (من قاعدة البيانات المركزية للسوبر أدمن)
    /// </summary>
    private async Task<bool> IsLicenseRevokedAsync(Guid licenseId)
    {
        // في الإنتاج، هذا يجب الاتصال بقاعدة بيانات السوبر أدمن
        // حالياً سنفترض أن الترخيص غير ملغى
        await Task.CompletedTask;
        return false;
    }
}

/// <summary>
/// خيارات إعدادات الترخيص
/// </summary>
public class LicenseOptions
{
    /// <summary>
    /// مجلد حفظ ملف الترخيص
    /// </summary>
    public string LicenseDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MasarSchoolsERP");

    /// <summary>
    /// المفتاح العام RSA (مدمج في التطبيق)
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;
}