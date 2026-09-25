using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// طاولات الكانتين (لطلبات الصالة)
/// </summary>
public class CanteenTable : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    /// <summary>
    /// رقم الطاولة
    /// </summary>
    public int TableNumber { get; set; }

    /// <summary>
    /// اسم الطاولة (اختياري)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// عدد المقاعد
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// موقع الطاولة (اختياري)
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// هل الطاولة متاحة
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// حالة الطاولة
    /// </summary>
    public TableStatus Status { get; set; } = TableStatus.Available;

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// ملاحظات (اختياري)
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// حالة الطاولة
/// </summary>
public enum TableStatus
{
    /// <summary>
    /// متاحة
    /// </summary>
    Available = 1,

    /// <summary>
    /// محجوزة
    /// </summary>
    Occupied = 2,

    /// <summary>
    /// تحت التنظيف
    /// </summary>
    Cleaning = 3,

    /// <summary>
    /// خارج الخدمة
    /// </summary>
    OutOfService = 4
}