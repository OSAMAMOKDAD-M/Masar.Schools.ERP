using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان أمر الشراء
/// </summary>
public class PurchaseOrder : BaseEntity
{
    /// <summary>
    /// رقم أمر الشراء
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// معرف المورد
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// المورد المرتبط
    /// </summary>
    public Supplier Supplier { get; set; } = null!;

    /// <summary>
    /// معرف المخزن
    /// </summary>
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// المخزن المرتبط
    /// </summary>
    public Warehouse Warehouse { get; set; } = null!;

    /// <summary>
    /// تاريخ الطلب
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// تاريخ التسليم المتوقع
    /// </summary>
    public DateTime ExpectedDeliveryDate { get; set; }

    /// <summary>
    /// تاريخ التسليم الفعلي
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// الحالة (Draft, Pending, Approved, Received, Cancelled)
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "مسودة";

    /// <summary>
    /// إجمالي المبلغ
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// المبلغ المدفوع
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// المبلغ المتبقي
    /// </summary>
    public decimal RemainingAmount { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// شروط الدفع
    /// </summary>
    public string PaymentTerms { get; set; } = "Net 30";

    /// <summary>
    /// شروط التسليم
    /// </summary>
    public string? DeliveryTerms { get; set; }

    /// <summary>
    /// الملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ملاحظات بالعربية
    /// </summary>
    public string? NotesArabic { get; set; }

    /// <summary>
    /// معرف معتمد الطلب
    /// </summary>
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// تاريخ الاعتماد
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// اسم معتمد الطلب
    /// </summary>
    public string? ApprovedByName { get; set; }

    /// <summary>
    /// جمع بنود أمر الشراء
    /// </summary>
    public ICollection<PurchaseOrderItem> OrderItems { get; set; } = new List<PurchaseOrderItem>();

    /// <summary>
    /// جمع فواتير الشراء
    /// </summary>
    public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
}

/// <summary>
/// كيان بند أمر الشراء
/// </summary>
public class PurchaseOrderItem : BaseEntity
{
    /// <summary>
    /// معرف أمر الشراء
    /// </summary>
    public Guid PurchaseOrderId { get; set; }

    /// <summary>
    /// أمر الشراء المرتبط
    /// </summary>
    public PurchaseOrder PurchaseOrder { get; set; } = null!;

    /// <summary>
    /// معرف الصنف
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// الصنف المرتبط
    /// </summary>
    public InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// الكمية المطلوبة
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// الكمية المستلمة
    /// </summary>
    public decimal ReceivedQuantity { get; set; }

    /// <summary>
    /// الكمية المتبقية
    /// </summary>
    public decimal RemainingQuantity { get; set; }

    /// <summary>
    /// سعر الوحدة
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// إجمالي المبلغ
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// الخصم
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// الضريبة
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// المبلغ النهائي
    /// </summary>
    public decimal FinalAmount { get; set; }

    /// <summary>
    /// ملاحظات البند
    /// </summary>
    public string? Notes { get; set; }
}
