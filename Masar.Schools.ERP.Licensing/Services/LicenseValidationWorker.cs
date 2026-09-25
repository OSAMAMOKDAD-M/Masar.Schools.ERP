using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Masar.Schools.ERP.Licensing.Models;

namespace Masar.Schools.ERP.Licensing.Services;

/// <summary>
/// خدمة خلفية للتحقق المستمر من صحة الترخيص
/// تعمل كل 6 ساعات للتأكد من سريان الرخصة والتحقق من التلاعب بساعة النظام
/// </summary>
public class LicenseValidationWorker : BackgroundService
{
    private readonly ILogger<LicenseValidationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _validationInterval = TimeSpan.FromHours(6);

    public LicenseValidationWorker(
        ILogger<LicenseValidationWorker> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("بدء خدمة التحقق من الترخيص في الخلفية");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ValidateLicensePeriodically(stoppingToken);
                await Task.Delay(_validationInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // التوقف العادي
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في خدمة التحقق من الترخيص");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // الانتظار 5 دقائق قبل المحاولة مرة أخرى
            }
        }

        _logger.LogInformation("توقف خدمة التحقق من الترخيص");
    }

    private async Task ValidateLicensePeriodically(CancellationToken cancellationToken)
    {
        using var scope = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.CreateScope(_serviceProvider);
        
        // الحصول على خدمة الترخيص
        var licenseService = scope.ServiceProvider.GetService<ILicenseService>();
        if (licenseService == null)
        {
            _logger.LogWarning("خدمة الترخيص غير متوفرة");
            return;
        }

        // التحقق من الترخيص
        var validationResult = await licenseService.ValidateCurrentLicenseAsync();
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("الترخيص غير صالح: {Status} - {Message}", 
                validationResult.Status, validationResult.Message);
            
            // هنا يمكن إضافة منطق إضافي مثل:
            // - إرسال إشعار للسوبر أدمن
            // - تقييد بعض الوظائف
            // - تسجيل محاولة التلاعب
        }
        else
        {
            _logger.LogInformation("الترخيص صحيح وساري: ينتهي في {ExpiryDate}", 
                validationResult.LicenseData?.ValidUntil);
        }

        // التحقق من التلاعب بساعة النظام
        await DetectSystemClockTampering(cancellationToken);
    }

    private async Task DetectSystemClockTampering(CancellationToken cancellationToken)
    {
        try
        {
            // الحصول على الوقت من خادم NTP موثوق
            // هذا مثال بسيط - في الإنتاج يجب استخدام خدمة NTP حقيقية
            var ntpTime = await GetNtpTimeAsync(cancellationToken);
            var localTime = DateTime.UtcNow;

            // السماح بفرق 5 دقائق كحد أقصى
            var timeDifference = Math.Abs((ntpTime - localTime).TotalMinutes);

            if (timeDifference > 5)
            {
                _logger.LogWarning("اكتشاف تلاعب محتمل بساعة النظام! الفرق: {Minutes} دقيقة", timeDifference);
                
                // تسجيل محاولة التلاعب
                await LogClockTamperingAttempt(timeDifference, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل التحقق من ساعة النظام");
        }
    }

    private async Task<DateTime> GetNtpTimeAsync(CancellationToken cancellationToken)
    {
        // في الإنتاج، يجب استخدام خادم NTP حقيقي
        // هذا مجرد مثال توضيحي
        await Task.Delay(100, cancellationToken);
        return DateTime.UtcNow; // مؤقتاً - يجب استبداله بـ NTP request حقيقي
    }

    private async Task LogClockTamperingAttempt(double timeDifference, CancellationToken cancellationToken)
    {
        // تسجيل محاولة التلاعب في قاعدة البيانات أو ملف log
        _logger.LogError("تم تسجيل محاولة تلاعب بساعة النظام: الفرق {Minutes} دقيقة", timeDifference);
        
        // هنا يمكن إضافة:
        // - إرسال تنبيه للسوبر أدمن
        // - إضافة إلى القائمة السوداء
        // - إبطال الترخيص تلقائياً
        await Task.CompletedTask;
    }
}

/// <summary>
/// واجهة خدمة الترخيص
/// </summary>
public interface ILicenseService
{
    Task<LicenseValidationResult> ValidateCurrentLicenseAsync();
    Task<bool> IsModuleEnabledAsync(LicenseModules module);
    Task<LicenseModel?> GetCurrentLicenseAsync();
    Task<bool> ActivateLicenseAsync(string licenseKey);
    Task<bool> DeactivateLicenseAsync();
}