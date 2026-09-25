using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// سجل أكاديمي للطالب - يتتبع الأداء الأكاديمي عبر السنوات
/// </summary>
public class AcademicRecord : BaseEntity
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
    /// السنة الأكاديمية
    /// </summary>
    public int AcademicYear { get; set; }

    /// <summary>
    /// المستوى الدراسي
    /// </summary>
    public string? GradeLevel { get; set; }

    /// <summary>
    /// المستوى الدراسي بالعربية
    /// </summary>
    public string? GradeLevelArabic { get; set; }

    /// <summary>
    /// الشعبة/القسم
    /// </summary>
    public string? Section { get; set; }

    /// <summary>
    /// الشعبة/القسم بالعربية
    /// </summary>
    public string? SectionArabic { get; set; }

    /// <summary>
    /// المعدل التراكمي (GPA)
    /// </summary>
    public decimal? GPA { get; set; }

    /// <summary>
    /// النسبة المئوية العامة
    /// </summary>
    public decimal? OverallPercentage { get; set; }

    /// <summary>
    /// الوضع الأكاديمي (Good, Probation, Suspended, Honors)
    /// </summary>
    public string? AcademicStanding { get; set; }

    /// <summary>
    /// الوضع الأكاديمي بالعربية
    /// </summary>
    public string? AcademicStandingArabic { get; set; }

    /// <summary>
    /// المرتبة في الفصل
    /// </summary>
    public int? ClassRank { get; set; }

    /// <summary>
    /// إجمالي عدد الطلاب في الفصل
    /// </summary>
    public int? TotalStudentsInClass { get; set; }

    /// <summary>
    /// عدد أيام الغياب
    /// </summary>
    public int? AbsenceDays { get; set; }

    /// <summary>
    /// عدد أيام التأخير
    /// </summary>
    public int? LateDays { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// هل السجل نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// تاريخ بدء السنة الأكاديمية
    /// </summary>
    public DateTime? AcademicYearStart { get; set; }

    /// <summary>
    /// تاريخ انتهاء السنة الأكاديمية
    /// </summary>
    public DateTime? AcademicYearEnd { get; set; }

    /// <summary>
    /// الفصل الدراسي الحالي
    /// </summary>
    public string? CurrentTerm { get; set; }

    /// <summary>
    /// الفصل الدراسي الحالي بالعربية
    /// </summary>
    public string? CurrentTermArabic { get; set; }
}