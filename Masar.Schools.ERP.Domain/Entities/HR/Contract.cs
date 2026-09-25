using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان عقد الموظف
/// </summary>
public class Contract : BaseEntity
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
    /// رقم العقد
    /// </summary>
    public int ContractNumber { get; set; }

    /// <summary>
    /// تاريخ بدء العقد
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ انتهاء العقد
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// الراتب الأساسي
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// المسمى الوظيفي
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// المسمى الوظيفي بالعربية
    /// </summary>
    public string? JobTitleArabic { get; set; }

    /// <summary>
    /// القسم
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// القسم بالعربية
    /// </summary>
    public string? DepartmentArabic { get; set; }

    /// <summary>
    /// مدة العقد بالسنوات
    /// </summary>
    public int DurationYears { get; set; }

    /// <summary>
    /// مدة العقد بالأشهر
    /// </summary>
    public int DurationMonths { get; set; }

    /// <summary>
    /// مدة العقد بالأيام
    /// </summary>
    public int DurationDays { get; set; }

    /// <summary>
    /// حالة العقد (ساري، منتهي، ملغي)
    /// </summary>
    public string Status { get; set; } = "ساري";

    /// <summary>
    /// حالة العقد بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "ساري";

    /// <summary>
    /// هل هو العقد الحالي
    /// </summary>
    public bool IsCurrent { get; set; } = true;

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