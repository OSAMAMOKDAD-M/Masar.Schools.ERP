using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان الجنسية
/// </summary>
public class Nationality : BaseEntity
{
    /// <summary>
    /// اسم الجنسية
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// اسم الجنسية بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

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