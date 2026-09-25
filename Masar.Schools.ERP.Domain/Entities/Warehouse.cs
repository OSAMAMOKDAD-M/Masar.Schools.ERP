using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان المخزن
/// </summary>
public class Warehouse : BaseEntity
{
    /// <summary>
    /// اسم المخزن
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// اسم المخزن بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

    /// <summary>
    /// رمز المخزن
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;

    /// <summary>
    /// معرف الفرع (إن وجد)
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// الفرع المرتبط
    /// </summary>
    public Branch? Branch { get; set; }

    /// <summary>
    /// نوع المخزن (Main, Branch, Canteen, Transport, etc.)
    /// </summary>
    public string WarehouseType { get; set; } = "Main";

    /// <summary>
    /// نوع المخزن بالعربية
    /// </summary>
    public string WarehouseTypeArabic { get; set; } = "رئيسي";

    /// <summary>
    /// الموقع
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// المسؤول عن المخزن
    /// </summary>
    public string? Manager { get; set; }

    /// <summary>
    /// رقم هاتف المسؤول
    /// </summary>
    public string? ManagerPhone { get; set; }

    /// <summary>
    /// السعة الإجمالية
    /// </summary>
    public decimal? Capacity { get; set; }

    /// <summary>
    /// وحدة القياس
    /// </summary>
    public string? CapacityUnit { get; set; }

    /// <summary>
    /// هل المخزن نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// جمع حركات المخزون
    /// </summary>
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    /// <summary>
    /// جمع تعديلات المخزون
    /// </summary>
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
}
