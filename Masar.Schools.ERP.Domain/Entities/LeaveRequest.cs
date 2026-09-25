using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان طلب الإجازة
/// </summary>
public class LeaveRequest : BaseEntity
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// الموظف المرتبط
    /// </summary>
    public Employee Employee { get; set; } = null!;

    /// <summary>
    /// نوع الإجازة (Annual, Sick, Maternity, Unpaid, Emergency, etc.)
    /// </summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// نوع الإجازة بالعربية
    /// </summary>
    public string LeaveTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ البدء
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ الانتهاء
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// عدد الأيام
    /// </summary>
    public int DaysCount { get; set; }

    /// <summary>
    /// السبب/السبب
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// السبب بالعربية
    /// </summary>
    public string? ReasonArabic { get; set; }

    /// <summary>
    /// حالة الطلب (Pending, Approved, Rejected, Cancelled)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "قيد المراجعة";

    /// <summary>
    /// ملاحظات المدير
    /// </summary>
    public string? ManagerNotes { get; set; }

    /// <summary>
    /// تاريخ الموافقة/الرفض
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// من قام بالموافقة/الرفض
    /// </summary>
    public string? ActionBy { get; set; }

    /// <summary>
    /// المستندات المرفقة (مسارات مفصولة بفاصلة)
    /// </summary>
    public string? AttachmentPaths { get; set; }

    /// <summary>
    /// هل تم الإخطار للموظف
    /// </summary>
    public bool NotificationSent { get; set; }

    /// <summary>
    /// تاريخ الإخطار
    /// </summary>
    public DateTime? NotificationSentAt { get; set; }

    /// <summary>
    /// رقم الإجازة (تلقائي)
    /// </summary>
    public string? LeaveNumber { get; set; }

    /// <summary>
    /// رصيد الإجازة قبل الطلب
    /// </summary>
    public decimal? PreviousBalance { get; set; }

    /// <summary>
    /// رصيد الإجازة بعد الطلب
    /// </summary>
    public decimal? RemainingBalance { get; set; }

    /// <summary>
    /// هل إجازة مدفوعة
    /// </summary>
    public bool IsPaid { get; set; } = true;

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;
}