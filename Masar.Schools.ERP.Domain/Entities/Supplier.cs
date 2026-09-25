using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان المورد
/// </summary>
public class Supplier : BaseEntity
{
    /// <summary>
    /// اسم المورد
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// اسم المورد بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

    /// <summary>
    /// كود المورد
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// رقم السجل التجاري
    /// </summary>
    public string? CommercialRegistration { get; set; }

    /// <summary>
    /// الرقم الضريبي
    /// </summary>
    public string? TaxNumber { get; set; }

    /// <summary>
    /// العنوان
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// المدينة
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// رقم الهاتف
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// رقم البريد الإلكتروني
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// اسم جهة الاتصال
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// رقم هاتف جهة الاتصال
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// فئة المورد (Standard, Preferred, Key)
    /// </summary>
    public string SupplierCategory { get; set; } = "Standard";

    /// <summary>
    /// فئة المورد بالعربية
    /// </summary>
    public string SupplierCategoryArabic { get; set; } = "عادي";

    /// <summary>
    /// شروط الدفع (Net 30, Net 60, etc.)
    /// </summary>
    public string PaymentTerms { get; set; } = "Net 30";

    /// <summary>
    /// الحد الائتماني
    /// </summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// هل المورد نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// تقييم المورد (1-5)
    /// </summary>
    public int Rating { get; set; } = 3;

    /// <summary>
    /// جمع أوامر الشراء
    /// </summary>
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    /// <summary>
    /// جمع الأصناف المرتبطة
    /// </summary>
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}
