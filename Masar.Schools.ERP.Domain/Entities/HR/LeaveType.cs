using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان نوع الإجازة
/// </summary>
public class LeaveType : BaseEntity
{
    /// <summary>
    /// اسم نوع الإجازة
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// اسم نوع الإجازة بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

    /// <summary>
    /// الأيام المسموحة سنوياً
    /// </summary>
    public decimal DaysPerYear { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// هل تلقائي (نظام 21/30)
    /// </summary>
    public bool IsAutomatic { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// هل نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int SortOrder { get; set; }
}