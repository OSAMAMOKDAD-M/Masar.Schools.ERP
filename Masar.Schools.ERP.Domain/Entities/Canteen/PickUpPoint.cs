using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// نقاط الاستلام في مناطق التوصيل
/// </summary>
public class PickUpPoint : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid DeliveryZoneId { get; set; }
    public DeliveryZone DeliveryZone { get; set; } = null!;

    /// <summary>
    /// اسم نقطة الاستلام بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم نقطة الاستلام بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// موقع نقطة الاستلام
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// وصف نقطة الاستلام (اختياري)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// هل نقطة الاستلام متاحة
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// ملاحظات (اختياري)
    /// </summary>
    public string? Notes { get; set; }
}