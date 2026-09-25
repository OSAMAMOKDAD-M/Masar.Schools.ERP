using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان الفصل الدراسي
/// </summary>
public class AcademicTerm : BaseEntity
{
    /// <summary>
    /// اسم الفصل الدراسي (Term 1, Term 2, Term 3)
    /// </summary>
    public string TermName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الفصل بالعربية
    /// </summary>
    public string TermNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ البدء
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ الانتهاء
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// حالة الفصل (Pending, Active, Completed, Cancelled)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "قيد الانتظار";

    /// <summary>
    /// هل الفصل نشط
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// هل تم تفعيل نظام الحضور بالبصمة
    /// </summary>
    public bool BiometricAttendanceEnabled { get; set; }

    /// <summary>
    /// هل تم تفعيل محرك التنبؤ بالمخاطر
    /// </summary>
    public bool RiskPredictionEnabled { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// عدد الطلاب المسجلين
    /// </summary>
    public int EnrolledStudentsCount { get; set; }

    /// <summary>
    /// عدد الشعب النشطة
    /// </summary>
    public int ActiveSectionsCount { get; set; }

    /// <summary>
    /// جمع الشعب المرتبطة بالفصل
    /// </summary>
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
