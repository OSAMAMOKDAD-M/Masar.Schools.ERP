using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// سجل تاريخي للتنبيهات المرسلة للطلاب ذوي مستوى خطر مرتفع
/// </summary>
public class RiskAlertHistory : BaseEntity
{
    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// الطالب المرتبط
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// مستوى الخطر السابق
    /// </summary>
    public string PreviousRiskLevel { get; set; } = string.Empty;

    /// <summary>
    /// درجة الخطر السابقة
    /// </summary>
    public decimal PreviousRiskScore { get; set; }

    /// <summary>
    /// مستوى الخطر الجديد
    /// </summary>
    public string NewRiskLevel { get; set; } = string.Empty;

    /// <summary>
    /// درجة الخطر الجديدة
    /// </summary>
    public decimal NewRiskScore { get; set; }

    /// <summary>
    /// نوع التنبيه
    /// </summary>
    public string AlertType { get; set; } = string.Empty; // WarningTransition, CriticalTransition, Improvement, Periodic

    /// <summary>
    /// نوع الانتقال (Safe→Warning, Warning→Critical, إلخ)
    /// </summary>
    public string? TransitionType { get; set; }

    /// <summary>
    /// رسالة التنبيه المرسلة
    /// </summary>
    public string? AlertMessage { get; set; }

    /// <summary>
    /// المستلم (Admin, Supervisor, Parent)
    /// </summary>
    public string? Recipient { get; set; }

    /// <summary>
    /// وسالة الإرسال (WhatsApp, Email, In-App)
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    /// حالة الإرسال
    /// </summary>
    public string Status { get; set; } = string.Empty; // Pending, Sent, Failed, Cancelled

    /// <summary>
    /// وقت الإرسال
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// رسالة الخطأ إذا فشل الإرسال
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// معرف الإشعار المرتبط
    /// </summary>
    public string? NotificationId { get; set; }
}
