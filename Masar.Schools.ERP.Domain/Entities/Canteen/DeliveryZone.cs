using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// مناطق التوصيل في الكانتين
/// </summary>
public class DeliveryZone : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// اسم المنطقة بالعربية
    /// </summary>
    public string NameAr { get; set; } = string.Empty;

    /// <summary>
    /// اسم المنطقة بالإنجليزية (اختياري)
    /// </summary>
    public string? NameEn { get; set; }

    /// <summary>
    /// رسوم التوصيل للمنطقة
    /// </summary>
    public decimal DeliveryFee { get; set; } = 0;

    /// <summary>
    /// وصف المنطقة (اختياري)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// هل المنطقة متاحة للتوصيل
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// الحد الأدنى للطلب للتوصيل
    /// </summary>
    public decimal MinimumOrderAmount { get; set; } = 0;

    /// <summary>
    /// وقت التوصيل المتوقع (بالدقائق)
    /// </summary>
    public int EstimatedDeliveryTime { get; set; } = 30;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// نقاط الاستلام في هذه المنطقة
    /// </summary>
    public ICollection<PickUpPoint> PickUpPoints { get; set; } = new List<PickUpPoint>();
}