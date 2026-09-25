using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// صور منتجات الكانتين
/// </summary>
public class CanteenProductImage : BaseEntity
{
    public Guid CanteenProductId { get; set; }
    public CanteenProduct Product { get; set; } = null!;

    /// <summary>
    /// مسار الصورة
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>
    /// هل هذه الصورة هي الصورة الأساسية
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// ترتيب الصورة
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// وصف الصورة (اختياري)
    /// </summary>
    public string? AltText { get; set; }
}