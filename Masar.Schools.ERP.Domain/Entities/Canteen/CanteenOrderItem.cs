using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// بنود طلبات الكانتين
/// </summary>
public class CanteenOrderItem : BaseEntity
{
    public Guid CanteenOrderId { get; set; }
    public CanteenOrder Order { get; set; } = null!;

    public Guid CanteenProductId { get; set; }
    public CanteenProduct Product { get; set; } = null!;

    /// <summary>
    /// الحجم المختار (اختياري)
    /// </summary>
    public Guid? VariantId { get; set; }
    public CanteenProductVariant? Variant { get; set; }

    /// <summary>
    /// الكمية
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// سعر الوحدة وقت الطلب
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// الإجمالي للبند
    /// </summary>
    public decimal LineTotal { get; set; }

    /// <summary>
    /// ملاحظات خاصة للصنف
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// الإضافات للبند
    /// </summary>
    public ICollection<CanteenOrderItemExtra> Extras { get; set; } = new List<CanteenOrderItemExtra>();
}