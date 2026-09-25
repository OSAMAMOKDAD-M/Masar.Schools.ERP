using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان أرشيف الفصل الدراسي
/// </summary>
public class TermArchive : BaseEntity
{
    /// <summary>
    /// معرف الفصل الدراسي المؤرشف
    /// </summary>
    public Guid TermId { get; set; }

    /// <summary>
    /// الفصل الدراسي المرتبط
    /// </summary>
    public AcademicTerm Term { get; set; } = null!;

    /// <summary>
    /// اسم الفصل الدراسي
    /// </summary>
    public string TermName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الفصل بالعربية
    /// </summary>
    public string TermNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ الإغلاق
    /// </summary>
    public DateTime ClosingDate { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// عدد الطلاب المؤرشفين
    /// </summary>
    public int ArchivedStudentsCount { get; set; }

    /// <summary>
    /// عدد الدرجات المؤرشفة
    /// </summary>
    public int ArchivedGradesCount { get; set; }

    /// <summary>
    /// عدد سجلات الحضور المؤرشفة
    /// </summary>
    public int ArchivedAttendanceRecords { get; set; }

    /// <summary>
    /// إجمالي الفواتير المؤرشفة
    /// </summary>
    public decimal ArchivedInvoicesTotal { get; set; }

    /// <summary>
    /// إجمالي المدفوعات المؤرشفة
    /// </summary>
    public decimal ArchivedPaymentsTotal { get; set; }

    /// <summary>
    /// ملاحظات الإغلاق
    /// </summary>
    public string? ClosingNotes { get; set; }

    /// <summary>
    /// من قام بالإغلاق
    /// </summary>
    public string? ClosedBy { get; set; }

    /// <summary>
    /// بيانات الأرشيف بتنسيق JSON
    /// </summary>
    public string? ArchiveData { get; set; }

    /// <summary>
    /// مسار ملف الأرشيف
    /// </summary>
    public string? ArchiveFilePath { get; set; }

    /// <summary>
    /// حجم الأرشيف بالبايت
    /// </summary>
    public long ArchiveSizeBytes { get; set; }

    /// <summary>
    /// حالة الأرشيف
    /// </summary>
    public string Status { get; set; } = "Archived";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "مؤرشف";

    /// <summary>
    /// هل الأرشيف مكتمل
    /// </summary>
    public bool IsArchiveComplete { get; set; }

    /// <summary>
    /// تاريخ استعادة الأرشيف (إن وجد)
    /// </summary>
    public DateTime? RestoredAt { get; set; }

    /// <summary>
    /// من قام بالاستعادة
    /// </summary>
    public string? RestoredBy { get; set; }
}
