using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان طلب الشراء (Internal Request)
/// </summary>
public class PurchaseRequest : BaseEntity
{
    /// <summary>
    /// رقم الطلب
    /// </summary>
    public string RequestNumber { get; set; } = string.Empty;

    /// <summary>
    /// معرف القسم/الإدارة
    /// </summary>
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// اسم القسم
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// تاريخ الطلب
    /// </summary>
    public DateTime RequestDate { get; set; }

    /// <summary>
    /// تاريخ الطلب المطلوب
    /// </summary>
    public DateTime RequiredBy { get; set; }

    /// <summary>
    /// طالب الطلب
    /// </summary>
    public string RequestedBy { get; set; } = string.Empty;

    /// <summary>
    /// الغرض من الطلب
    /// </summary>
    public string Purpose { get; set; } = string.Empty;

    /// <summary>
    /// الغرض بالعربية
    /// </summary>
    public string PurposeArabic { get; set; } = string.Empty;

    /// <summary>
    /// الميزانية المتاحة
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// العملة
    /// </summary>
    public string Currency { get; set; } = "SAR";

    /// <summary>
    /// الحالة (Draft, Pending, Approved, Rejected, Completed)
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "مسودة";

    /// <summary>
    /// معتمد الطلب
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
    /// ملاحظات الطلب
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// جمع بنود الطلب
    /// </summary>
    public ICollection<PurchaseRequestItem> RequestItems { get; set; } = new List<PurchaseRequestItem>();
}

/// <summary>
/// كيان بند طلب الشراء
/// </summary>
public class PurchaseRequestItem : BaseEntity
{
    /// <summary>
    /// معرف طلب الشراء
    /// </summary>
    public Guid PurchaseRequestId { get; set; }

    /// <summary>
    /// طلب الشراء المرتبط
    /// </summary>
    public PurchaseRequest PurchaseRequest { get; set; } = null!;

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
    /// السعر التقديري
    /// </summary>
    public decimal EstimatedPrice { get; set; }

    /// <summary>
    /// الإجمالي التقديري
    /// </summary>
    public decimal EstimatedTotal { get; set; }

    /// <summary>
    /// المفضلة المورد المقترح
    /// </summary>
    public string? PreferredSupplier { get; set; }

    /// <summary>
    /// ملاحظات البند
    /// </summary>
    public string? Notes { get; set; }
}
