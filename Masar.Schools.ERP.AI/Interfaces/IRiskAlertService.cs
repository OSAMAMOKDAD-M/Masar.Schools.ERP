using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.AI.Interfaces;

/// <summary>
/// واجهة خدمة التنبيهات بالمخاطر
/// </summary>
public interface IRiskAlertService
{
    /// <summary>
    /// إنشاء تنبيه عند انتقال مستوى الخطر
    /// </summary>
    Task<RiskAlertHistory> CreateRiskTransitionAlertAsync(
        Guid studentId,
        string previousLevel,
        decimal previousScore,
        string newLevel,
        decimal newScore,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// إرسال تنبيه للمشرف
    /// </summary>
    Task SendSupervisorAlertAsync(
        RiskAlertHistory alert,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// إرسال تنبيه لولي الأمر عبر WhatsApp
    /// </summary>
    Task SendParentWhatsAppAlertAsync(
        RiskAlertHistory alert,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// التحقق من إمكانية إرسال تنبيه (Cooldown)
    /// </summary>
    Task<bool> CanSendAlertAsync(
        Guid studentId,
        string alertType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تحديث حالة التنبيه
    /// </summary>
    Task UpdateAlertStatusAsync(
        Guid alertId,
        string status,
        string? errorMessage = null,
        CancellationToken cancellationToken = default);
}
