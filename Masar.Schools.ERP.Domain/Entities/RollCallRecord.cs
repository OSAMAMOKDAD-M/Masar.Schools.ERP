using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// سجل مناداة طالب فردي
/// </summary>
public class RollCallRecord : BaseEntity
{
    /// <summary>
    /// معرف جلسة المناداة
    /// </summary>
    public Guid SessionId { get; set; }

    /// <summary>
    /// جلسة المناداة المرتبطة
    /// </summary>
    public RollCallSession Session { get; set; } = null!;

    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// الطالب المرتبط
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// حالة الحضور (Present, Absent, Late, Excused)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = string.Empty;

    /// <summary>
    /// وقت التسجيل
    /// </summary>
    public DateTime? RecordedAt { get; set; }

    /// <summary>
    /// ملاحظات على الحضور
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// سبب الغياب (إن وجد)
    /// </summary>
    public string? AbsenceReason { get; set; }

    /// <summary>
    /// سبب الغياب بالعربية
    /// </summary>
    public string? AbsenceReasonArabic { get; set; }

    /// <summary>
    /// مدة التأخير بالدقائق (إن وجد)
    /// </summary>
    public int? LateMinutes { get; set; }

    /// <summary>
    /// هل تم إرسال إشعار لولي الأمر
    /// </summary>
    public bool GuardianNotificationSent { get; set; }

    /// <summary>
    /// تاريخ إرسال الإشعار
    /// </summary>
    public DateTime? GuardianNotificationSentAt { get; set; }

    /// <summary>
    /// نوع الإشعار المرسل (WhatsApp, SMS, Email)
    /// </summary>
    public string? NotificationType { get; set; }

    /// <summary>
    /// هل تم تحديث السجل لاحقاً
    /// </summary>
    public bool IsModified { get; set; }

    /// <summary>
    /// تاريخ التعديل
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// من قام بالتعديل
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// الحالة الأصلية قبل التعديل
    /// </summary>
    public string? OriginalStatus { get; set; }

    /// <summary>
    /// سبب التعديل
    /// </summary>
    public string? ModificationReason { get; set; }

    /// <summary>
    /// طريقة التسجيل (Manual, Automated, Biometric)
    /// </summary>
    public string RecordingMethod { get; set; } = "Manual";

    /// <summary>
    /// هل الطالب حضر الحصة بالكامل
    /// </summary>
    public bool AttendedFullPeriod { get; set; } = true;

    /// <summary>
    /// وقت المغادرة المبكرة (إن وجد)
    /// </summary>
    public DateTime? EarlyDepartureTime { get; set; }

    /// <summary>
    /// سبب المغادرة المبكرة
    /// </summary>
    public string? EarlyDepartureReason { get; set; }

    /// <summary>
    /// هل الغياب مبرر
    /// </summary>
    public bool IsExcused { get; set; }

    /// <summary>
    /// من وافق على العذر
    /// </summary>
    public string? ExcuseApprovedBy { get; set; }

    /// <summary>
    /// تاريخ الموافقة على العذر
    /// </summary>
    public DateTime? ExcuseApprovedAt { get; set; }
}