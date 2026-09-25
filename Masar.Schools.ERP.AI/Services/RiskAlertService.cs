using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Masar.Schools.ERP.AI.Services;

/// <summary>
/// خدمة إدارة تنبيهات المخاطر
/// </summary>
public class RiskAlertService : IRiskAlertService
{
    private readonly MasarDbContext _context;
    private readonly IWhatsAppService _whatsAppService;
    private readonly MasarAIOptions _options;
    private readonly ILogger<RiskAlertService> _logger;

    public RiskAlertService(
        MasarDbContext context,
        IWhatsAppService whatsAppService,
        IOptions<MasarAIOptions> options,
        ILogger<RiskAlertService> logger)
    {
        _context = context;
        _whatsAppService = whatsAppService;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<RiskAlertHistory> CreateRiskTransitionAlertAsync(
        Guid studentId,
        string previousLevel,
        decimal previousScore,
        string newLevel,
        decimal newScore,
        CancellationToken cancellationToken = default)
    {
        var alert = new RiskAlertHistory
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            PreviousRiskLevel = previousLevel,
            PreviousRiskScore = previousScore,
            NewRiskLevel = newLevel,
            NewRiskScore = newScore,
            AlertType = DetermineAlertType(previousLevel, newLevel),
            TransitionType = $"{previousLevel} → {newLevel}",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "MasarAI"
        };

        _context.RiskAlertHistories.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Created risk transition alert for student {StudentId}: {PreviousLevel} ({PreviousScore}) → {NewLevel} ({NewScore})",
            studentId, previousLevel, previousScore, newLevel, newScore);

        return alert;
    }

    public async Task SendSupervisorAlertAsync(
        RiskAlertHistory alert,
        CancellationToken cancellationToken = default)
    {
        // في النسخة الحالية، سنستخدم In-App Notification
        // يمكن توسيعها لإرسال Email أو إشعار Dashboard
        
        _logger.LogInformation(
            "Supervisor alert sent for student {StudentId}: {RiskLevel}",
            alert.StudentId, alert.NewRiskLevel);

        alert.Channel = "In-App";
        alert.SentAt = DateTime.UtcNow;
        alert.Status = "Sent";

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SendParentWhatsAppAlertAsync(
        RiskAlertHistory alert,
        CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .Include(s => s.Guardian)
            .FirstOrDefaultAsync(s => s.Id == alert.StudentId, cancellationToken);

        if (student?.Guardian == null)
        {
            _logger.LogWarning("No guardian found for student {StudentId}", alert.StudentId);
            alert.Status = "Failed";
            alert.ErrorMessage = "No guardian found";
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        try
        {
            var message = BuildParentAlertMessage(student, alert);
            var phoneNumber = student.Guardian.PhoneNumber;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                alert.Status = "Failed";
                alert.ErrorMessage = "No phone number available";
                await _context.SaveChangesAsync(cancellationToken);
                return;
            }

            // استخدام IWhatsAppService الموجود
            await _whatsAppService.SendGeneralNotificationAsync(phoneNumber, "تنبيه من مُساعد مَسَار الذكي", message);

            alert.Channel = "WhatsApp";
            alert.SentAt = DateTime.UtcNow;
            alert.Status = "Sent";
            await _context.SaveChangesAsync(cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "WhatsApp alert sent to guardian {PhoneNumber} for student {StudentId}",
                phoneNumber, alert.StudentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send WhatsApp alert for student {StudentId}", alert.StudentId);
            alert.Status = "Failed";
            alert.ErrorMessage = ex.Message;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> CanSendAlertAsync(
        Guid studentId,
        string alertType,
        CancellationToken cancellationToken = default)
    {
        var cooldownPeriod = TimeSpan.FromHours(_options.NotificationSettings.CooldownPeriodHours);
        var cutoffTime = DateTime.UtcNow.Subtract(cooldownPeriod);

        var recentAlert = await _context.RiskAlertHistories
            .AnyAsync(a => a.StudentId == studentId &&
                        a.AlertType == alertType &&
                        a.Status == "Sent" &&
                        a.SentAt >= cutoffTime,
                        cancellationToken);

        return !recentAlert;
    }

    public async Task UpdateAlertStatusAsync(
        Guid alertId,
        string status,
        string? errorMessage = null,
        CancellationToken cancellationToken = default)
    {
        var alert = await _context.RiskAlertHistories
            .FirstOrDefaultAsync(a => a.Id == alertId, cancellationToken);

        if (alert != null)
        {
            alert.Status = status;
            alert.ErrorMessage = errorMessage;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private string DetermineAlertType(string previous, string current)
    {
        if (current == "Critical" && previous != "Critical")
            return "CriticalTransition";

        if (current == "Warning" && previous == "Safe")
            return "WarningTransition";

        if (current == "Safe" && previous != "Safe")
            return "Improvement";

        return "Periodic";
    }

    private string BuildParentAlertMessage(Domain.Entities.Student student, RiskAlertHistory alert)
    {
        // رسالة حماية للأخصوصية - لا تحتوي على تفاصيل مالية حساسة
        return $"نود تنبيهكم إلى وجود بعض المؤشرات التي تستدعي متابعة مستوى الطالب الدراسي والحضور للطالب {student.FullNameArabic}. يرجى التواصل مع المدرسة لمزيد من التفاصيل.\n\n© 2026 نظام مَسَار للمدارس - مُساعد مَسَار الذكي";
    }
}
