using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان سجل الحضور والانصراف
/// </summary>
public class AttendanceLog : BaseEntity
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// الموظف المرتبط
    /// </summary>
    public Employee? Employee { get; set; }

    /// <summary>
    /// المعرف البصري
    /// </summary>
    public string BiometricId { get; set; } = string.Empty;

    /// <summary>
    /// وقت الحدث
    /// </summary>
    public DateTime EventAt { get; set; }

    /// <summary>
    /// الاتجاه (IN, OUT, غير مصنف)
    /// </summary>
    public string Direction { get; set; } = "غير مصنف";

    /// <summary>
    /// المصدر (يدوي، جهاز بصمة، Excel)
    /// </summary>
    public string Source { get; set; } = "يدوي";

    /// <summary>
    /// اسم الجهاز
    /// </summary>
    public string? DeviceName { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المدرسة المرتبطة
    /// </summary>
    public School School { get; set; } = null!;
}