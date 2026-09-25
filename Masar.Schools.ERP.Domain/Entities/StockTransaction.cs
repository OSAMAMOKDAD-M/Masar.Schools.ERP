using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان حركة المخزون
/// </summary>
public class StockTransaction : BaseEntity
{
    /// <summary>
    /// رقم الحركة
    /// </summary>
    public string TransactionNumber { get; set; } = string.Empty;

    /// <summary>
    /// نوع الحركة (Receipt, Issue, Transfer, Adjustment, Damage)
    /// </summary>
    public string TransactionType { get; set; } = string.Empty;

    /// <summary>
    /// نوع الحركة بالعربية
    /// </summary>
    public string TransactionTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// معرف الصنف
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// الصنف المرتبط
    /// </summary>
    public InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// معرف المخزن
    /// </summary>
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// المخزن المرتبط
    /// </summary>
    public Warehouse Warehouse { get; set; } = null!;

    /// <summary>
    /// الكمية
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// الكمية السالبة (للتسويات)
    /// </summary>
    public decimal? NegativeQuantity { get; set; }

    /// <summary>
    /// التكلفة الوحدة
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// التكلفة الإجمالية
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// الرصيد قبل الحركة
    /// </summary>
    public decimal BalanceBefore { get; set; }

    /// <summary>
    /// الرصيد بعد الحركة
    /// </summary>
    public decimal BalanceAfter { get; set; }

    /// <summary>
    /// معرف المستلم (إن وجد)
    /// </summary>
    public Guid? ReceiverId { get; set; }

    /// <summary>
    /// اسم المستلم
    /// </summary>
    public string? ReceiverName { get; set; }

    /// <summary>
    /// تاريخ الحركة
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// المرجع (أمر شراء، فاتورة، إلخ)
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// الملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// من قام بالحركة
    /// </summary>
    public string? PerformedBy { get; set; }
}
