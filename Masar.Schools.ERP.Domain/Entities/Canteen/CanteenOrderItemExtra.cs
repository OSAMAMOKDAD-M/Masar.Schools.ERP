namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// إضافات بنود طلبات الكانتين
/// </summary>
public class CanteenOrderItemExtra
{
    public Guid CanteenOrderItemId { get; set; }
    public CanteenOrderItem OrderItem { get; set; } = null!;

    public Guid CanteenProductExtraId { get; set; }
    public CanteenProductExtra Extra { get; set; } = null!;

    /// <summary>
    /// السعر وقت الطلب (Snapshot) - لا يتأثر بتغيير سعر الإضافة لاحقاً
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// الكمية (للإضافات المتكررة)
    /// </summary>
    public int Quantity { get; set; } = 1;
}