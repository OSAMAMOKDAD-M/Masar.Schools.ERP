using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// الإضافات/التوبينج للمنتجات (جبنة إضافية، صوص، إلخ)
/// </summary>
public class CanteenProductExtra : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// اسم الإضافة بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم الإضافة بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// سعر الإضافة
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// هل الإضافة متاحة
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// وصف الإضافة (اختياري)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// المنتجات التي تسمح بهذه الإضافة
    /// </summary>
    public ICollection<CanteenProduct> Products { get; set; } = new List<CanteenProduct>();
}