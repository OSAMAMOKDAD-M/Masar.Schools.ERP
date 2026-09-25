using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان فاتورة الشراء
/// </summary>
public class PurchaseInvoice : BaseEntity
{
    /// <summary>
    /// رقم الفاتورة
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// رقم الفاتورة الضريبية
    /// </summary>
    public string? TaxInvoiceNumber { get; set; }

    /// <summary>
    /// معرف أمر الشراء
    /// </summary>
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// أمر الشراء المرتبط
    /// </summary>
    public PurchaseOrder? PurchaseOrder { get; set; }

    /// <summary>
    /// معرف المورد
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// المورد المرتبط
    /// </summary>
    public Supplier Supplier { get; set; } = null!;

    /// <summary>
    /// تاريخ الفاتورة
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// تاريخ الاستحقاق
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// الإجمالي قبل الضريبة
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    /// الضريبة
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// الخصم
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// الإجمالي
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
    /// الحالة (Draft, Pending, Paid, Partially Paid, Overdue, Cancelled)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "قيد الانتظار";

    /// <summary>
    /// هل معتمدة مع ZATCA
    /// </summary>
    public bool IsZatcaApproved { get; set; }

    /// <summary>
    /// معرف ZATCA
    /// </summary>
    public string? ZatcaId { get; set; }

    /// <summary>
    /// تاريخ الاعتماد مع ZATCA
    /// </summary>
    public DateTime? ZatcaApprovedAt { get; set; }

    /// <summary>
    /// الملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ملاحظات بالعربية
    /// </summary>
    public string? NotesArabic { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// جمع بنود الفاتورة
    /// </summary>
    public ICollection<PurchaseInvoiceItem> InvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
}

/// <summary>
/// كيان بند فاتورة الشراء
/// </summary>
public class PurchaseInvoiceItem : BaseEntity
{
    /// <summary>
    /// معرف فاتورة الشراء
    /// </summary>
    public Guid PurchaseInvoiceId { get; set; }

    /// <summary>
    /// فاتورة الشراء المرتبطة
    /// </summary>
    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;

    /// <summary>
    /// معرف الصنف
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// الصنف المرتبط
    /// </summary>
    public InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// الكمية
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// سعر الوحدة
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// الإجمالي
    /// </summary>
    public decimal TotalAmount { get; set; }

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
