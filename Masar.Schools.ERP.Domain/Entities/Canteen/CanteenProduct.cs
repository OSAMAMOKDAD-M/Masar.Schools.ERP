using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// منتجات الكانتين
/// </summary>
public class CanteenProduct : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid ProductCategoryId { get; set; }
    public ProductCategory Category { get; set; } = null!;

    /// <summary>
    /// اسم المنتج بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم المنتج بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// وصف المنتج بالعربية
    /// </summary>
    public string? DescriptionAr { get; set; }

    /// <summary>
    /// السعر الأساسي الافتراضي
    /// </summary>
    public decimal BasePrice { get; set; }

    /// <summary>
    /// تكلفة المنتج (للحسابات الداخلية)
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// كمية المخزون المتاحة
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// الكود الشريطي (Barcode)
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// مسار صورة المنتج
    /// </summary>
    public string? ImagePath { get; set; }

    /// <summary>
    /// هل المنتج متاح للبيع
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// هل يسمح بالدفع من محفظة الطالب
    /// </summary>
    public bool AllowStudentBalance { get; set; } = true;

    /// <summary>
    /// هل يسمح بالدفع من محفظة الموظف
    /// </summary>
    public bool AllowEmployeeBalance { get; set; } = true;

    /// <summary>
    /// حد الطلب الأقصى للكمية
    /// </summary>
    public int MaxOrderQuantity { get; set; } = 10;

    /// <summary>
    /// حد الطلب الأدنى للكمية
    /// </summary>
    public int MinOrderQuantity { get; set; } = 1;

    /// <summary>
    /// هل المنتج مفضل
    /// </summary>
    public bool IsFavorite { get; set; } = false;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// صور المنتج
    /// </summary>
    public ICollection<CanteenProductImage> Images { get; set; } = new List<CanteenProductImage>();

    /// <summary>
    /// أحجام المنتج
    /// </summary>
    public ICollection<CanteenProductVariant> Variants { get; set; } = new List<CanteenProductVariant>();

    /// <summary>
    /// الإضافات المسموح بها للمنتج
    /// </summary>
    public ICollection<CanteenProductExtra> Extras { get; set; } = new List<CanteenProductExtra>();
}