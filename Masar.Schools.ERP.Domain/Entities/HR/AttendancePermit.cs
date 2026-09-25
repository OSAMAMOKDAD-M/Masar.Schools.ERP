using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان تصريح الحضور
/// </summary>
public class AttendancePermit : BaseEntity
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
    /// تاريخ التصريح
    /// </summary>
    public DateTime PermitDate { get; set; }

    /// <summary>
    /// مدة التصريح بالساعات
    /// </summary>
    public decimal DurationHours { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;
}