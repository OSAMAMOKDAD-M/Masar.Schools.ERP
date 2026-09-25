using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// أحجام منتجات الكانتين (صغير/وسط/كبير)
/// </summary>
public class CanteenProductVariant : BaseEntity
{
    public Guid CanteenProductId { get; set; }
    public CanteenProduct Product { get; set; } = null!;

    /// <summary>
    /// اسم الحجم بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم الحجم بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// السعر الإضافي فوق السعر الأساسي
    /// </summary>
    public decimal ExtraPrice { get; set; }

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// هل هذا الحجم متوفر
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// وصف الحجم (اختياري)
    /// </summary>
    public string? Description { get; set; }
}