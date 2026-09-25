using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان رصيد الإجازات للموظف
/// </summary>
public class LeaveBalance : BaseEntity
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// الموظف المرتبط
    /// </summary>
    public Employee Employee { get; set; } = null!;

    /// <summary>
    /// نوع الإجازة (Annual, Sick, Maternity, etc.)
    /// </summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// نوع الإجازة بالعربية
    /// </summary>
    public string LeaveTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// السنة المالية
    /// </summary>
    public int FiscalYear { get; set; }

    /// <summary>
    /// الرصيد الافتراضي السنوي
    /// </summary>
    public decimal AnnualAllowance { get; set; }

    /// <summary>
    /// الرصيد المستخدم
    /// </summary>
    public decimal UsedDays { get; set; }

    /// <summary>
    /// الرصيد المتبقي
    /// </summary>
    public decimal RemainingDays { get; set; }

    /// <summary>
    /// أيام إضافية من سنوات سابقة
    /// </summary>
    public decimal CarriedOverDays { get; set; }

    /// <summary>
    /// أيام منح خاصة
    /// </summary>
    public decimal SpecialAllowance { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// تاريخ آخر تحديث
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// هل النشاط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;
}