using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان صنف المخزون
/// </summary>
public class InventoryItem : BaseEntity
{
    /// <summary>
    /// اسم الصنف
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// اسم الصنف بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

    /// <summary>
    /// كود الصنف (SKU)
    /// </summary>
    public string SKU { get; set; } = string.Empty;

    /// <summary>
    /// الباركود
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// تصنيف الصنف
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// التصنيف بالعربية
    /// </summary>
    public string CategoryArabic { get; set; } = string.Empty;

    /// <summary>
    /// وحدة القياس (Unit, Kg, Liter, etc.)
    /// </summary>
    public string UnitOfMeasure { get; set; } = "Unit";

    /// <summary>
    /// وحدة القياس بالعربية
    /// </summary>
    public string UnitOfMeasureArabic { get; set; } = "وحدة";

    /// <summary>
    /// الحد الأدنى لإعادة الطلب
    /// </summary>
    public decimal ReorderLevel { get; set; }

    /// <summary>
    /// الحد الأقصى للمخزون
    /// </summary>
    public decimal MaxStockLevel { get; set; }

    /// <summary>
    /// كمية الطلب المقترحة
    /// </summary>
    public decimal ReorderQuantity { get; set; }

    /// <summary>
    /// التكلفة المتوسطة
    /// </summary>
    public decimal AverageCost { get; set; }

    /// <summary>
    /// سعر البيع
    /// </summary>
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// هل الصنف نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// هل يصرف من المخزون
    /// </summary>
    public bool IsStockItem { get; set; } = true;

    /// <summary>
    /// الوصف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// الوصف بالعربية
    /// </summary>
    public string? DescriptionArabic { get; set; }

    /// <summary>
    /// معرف المورد المفضل
    /// </summary>
    public Guid? PreferredSupplierId { get; set; }

    /// <summary>
    /// المورد المفضل
    /// </summary>
    public Supplier? PreferredSupplier { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// جمع حركات المخزون
    /// </summary>
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    /// <summary>
    /// جمع عناصر أوامر الشراء
    /// </summary>
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
}
