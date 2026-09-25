using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان نقل المخزون بين المخازن
/// </summary>
public class InventoryTransfer : BaseEntity
{
    /// <summary>
    /// رقم النقل
    /// </summary>
    public string TransferNumber { get; set; } = string.Empty;

    /// <summary>
    /// معرف المخزن المصدر
    /// </summary>
    public Guid FromWarehouseId { get; set; }

    /// <summary>
    /// المخزن المصدر
    /// </summary>
    public Warehouse FromWarehouse { get; set; } = null!;

    /// <summary>
    /// معرف المخزن المستقبل
    /// </summary>
    public Guid ToWarehouseId { get; set; }

    /// <summary>
    /// المخزن المستقبل
    /// </summary>
    public Warehouse ToWarehouse { get; set; } = null!;

    /// <summary>
    /// تاريخ النقل
    /// </summary>
    public DateTime TransferDate { get; set; }

    /// <summary>
    /// الحالة (Draft, Pending, InTransit, Completed, Cancelled)
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "مسودة";

    /// <summary>
    /// القيمة الإجمالية
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// السبب
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// السبب بالعربية
    /// </summary>
    public string ReasonArabic { get; set; } = string.Empty;

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// معتمد النقل
    /// </summary>
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// تاريخ الاعتماد
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// من قام بالنقل
    /// </summary>
    public string? PerformedBy { get; set; }

    /// <summary>
    /// جمع بنود النقل
    /// </summary>
    public ICollection<InventoryTransferItem> TransferItems { get; set; } = new List<InventoryTransferItem>();
}

/// <summary>
/// كيان بند نقل المخزون
/// </summary>
public class InventoryTransferItem : BaseEntity
{
    /// <summary>
    /// معرف نقل المخزون
    /// </summary>
    public Guid InventoryTransferId { get; set; }

    /// <summary>
    /// نقل المخزون المرتبط
    /// </summary>
    public InventoryTransfer InventoryTransfer { get; set; } = null!;

    /// <summary>
    /// معرف الصنف
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// الصنف المرتبط
    /// </summary>
    public InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// الكمية المنقولة
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// التكلفة الوحدة
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// القيمة الإجمالية
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// ملاحظات البند
    /// </summary>
    public string? Notes { get; set; }
}
