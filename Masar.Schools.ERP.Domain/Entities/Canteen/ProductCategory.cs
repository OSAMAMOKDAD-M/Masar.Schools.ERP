using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// تصنيفات منتجات الكانتين
/// </summary>
public class ProductCategory : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// اسم التصنيف بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم التصنيف بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// هل التصنيف نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// وصف التصنيف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// المنتجات في هذا التصنيف
    /// </summary>
    public ICollection<CanteenProduct> Products { get; set; } = new List<CanteenProduct>();
}