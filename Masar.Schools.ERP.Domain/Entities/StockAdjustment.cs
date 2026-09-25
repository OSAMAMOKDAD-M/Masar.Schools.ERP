using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان تعديل المخزون
/// </summary>
public class StockAdjustment : BaseEntity
{
    /// <summary>
    /// رقم التعديل
    /// </summary>
    public string AdjustmentNumber { get; set; } = string.Empty;

    /// <summary>
    /// نوع التعديل (Add, Remove, Damaged, Expired)
    /// </summary>
    public string AdjustmentType { get; set; } = string.Empty;

    /// <summary>
    /// نوع التعديل بالعربية
    /// </summary>
    public string AdjustmentTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// معرف المخزن
    /// </summary>
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// المخزن المرتبط
    /// </summary>
    public Warehouse Warehouse { get; set; } = null!;

    /// <summary>
    /// تاريخ التعديل
    /// </summary>
    public DateTime AdjustmentDate { get; set; }

    /// <summary>
    /// السبب
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// السبب بالعربية
    /// </summary>
    public string ReasonArabic { get; set; } = string.Empty;

    /// <summary>
    /// الوصف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// القيمة الإجمالية للتعديل
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// الحالة (Pending, Approved, Rejected)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "قيد الانتظار";

    /// <summary>
    /// معرف المعتمد
    /// </summary>
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// تاريخ الاعتماد
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// ملاحظات الاعتماد
    /// </summary>
    public string? ApprovalNotes { get; set; }

    /// <summary>
    /// جمع بنود التعديل
    /// </summary>
    public ICollection<StockAdjustmentItem> AdjustmentItems { get; set; } = new List<StockAdjustmentItem>();
}

/// <summary>
/// كيان بند تعديل المخزون
/// </summary>
public class StockAdjustmentItem : BaseEntity
{
    /// <summary>
    /// معرف تعديل المخزون
    /// </summary>
    public Guid StockAdjustmentId { get; set; }

    /// <summary>
    /// تعديل المخزون المرتبط
    /// </summary>
    public StockAdjustment StockAdjustment { get; set; } = null!;

    /// <summary>
    /// معرف الصنف
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// الصنف المرتبط
    /// </summary>
    public InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// الرصيد الحالي
    /// </summary>
    public decimal CurrentBalance { get; set; }

    /// <summary>
    /// الكمية المعدلة
    /// </summary>
    public decimal AdjustedQuantity { get; set; }

    /// <summary>
    /// الرصيد بعد التعديل
    /// </summary>
    public decimal NewBalance { get; set; }

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
