using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// جلسة المناداة الإلكترونية - لتسجيل الحضور الصفي
/// </summary>
public class RollCallSession : BaseEntity
{
    /// <summary>
    /// معرف الفصل الدراسي
    /// </summary>
    public Guid ClassRoomId { get; set; }

    /// <summary>
    /// الفصل الدراسي المرتبط
    /// </summary>
    public ClassRoom ClassRoom { get; set; } = null!;

    /// <summary>
    /// معرف المعلم/المدرس
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    /// المعلم/المدرس المرتبط
    /// </summary>
    public Employee Teacher { get; set; } = null!;

    /// <summary>
    /// تاريخ الجلسة
    /// </summary>
    public DateTime SessionDate { get; set; }

    /// <summary>
    /// الحصة/الفترة (First, Second, Third, etc.)
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// الحصة/الفترة بالعربية
    /// </summary>
    public string PeriodArabic { get; set; } = string.Empty;

    /// <summary>
    /// المادة الدراسية
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// المادة الدراسية بالعربية
    /// </summary>
    public string? SubjectArabic { get; set; }

    /// <summary>
    /// وقت بدء الجلسة
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// وقت انتهاء الجلسة
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// هل الجلسة مكتملة
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// تاريخ إكمال الجلسة
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// ملاحظات المعلم على الجلسة
    /// </summary>
    public string? TeacherNotes { get; set; }

    /// <summary>
    /// إجمالي عدد الطلاب المتوقعين
    /// </summary>
    public int ExpectedStudentsCount { get; set; }

    /// <summary>
    /// عدد الحاضرين
    /// </summary>
    public int PresentCount { get; set; }

    /// <summary>
    /// عدد الغائبين
    /// </summary>
    public int AbsentCount { get; set; }

    /// <summary>
    /// عدد المتأخرين
    /// </summary>
    public int LateCount { get; set; }

    /// <summary>
    /// نوع المناداة (Regular, Substitute, Exam, etc.)
    /// </summary>
    public string SessionType { get; set; } = "Regular";

    /// <summary>
    /// نوع المناداة بالعربية
    /// </summary>
    public string SessionTypeArabic { get; set; } = "عادية";

    /// <summary>
    /// هل تم إرسال إشعارات WhatsApp لأولياء الأمور
    /// </summary>
    public bool WhatsAppNotificationsSent { get; set; }

    /// <summary>
    /// تاريخ إرسال الإشعارات
    /// </summary>
    public DateTime? WhatsAppNotificationsSentAt { get; set; }

    /// <summary>
    /// سجلات المناداة للطلاب
    /// </summary>
    public ICollection<RollCallRecord> Records { get; set; } = new List<RollCallRecord>();
}