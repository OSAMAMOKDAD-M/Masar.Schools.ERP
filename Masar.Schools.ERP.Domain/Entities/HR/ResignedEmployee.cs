using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان الموظف المستقيل
/// </summary>
public class ResignedEmployee : BaseEntity
{
    /// <summary>
    /// معرف الموظف الأصلي
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// الموظف المرتبط
    /// </summary>
    public Employee Employee { get; set; } = null!;

    /// <summary>
    /// كود الموظف
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// الاسم الكامل
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الكامل بالعربية
    /// </summary>
    public string FullNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// الهوية الوطنية
    /// </summary>
    public string? NationalId { get; set; }

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
    /// الراتب
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// تاريخ بدء العقد
    /// </summary>
    public DateTime? ContractStart { get; set; }

    /// <summary>
    /// تاريخ الاستقالة
    /// </summary>
    public DateTime ResignDate { get; set; }

    /// <summary>
    /// سبب الاستقالة
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// سبب الاستقالة بالعربية
    /// </summary>
    public string? ReasonArabic { get; set; }

    /// <summary>
    /// سنوات الخدمة
    /// </summary>
    public int ServiceYears { get; set; }

    /// <summary>
    /// أشهر الخدمة
    /// </summary>
    public int ServiceMonths { get; set; }

    /// <summary>
    /// أيام الخدمة
    /// </summary>
    public int ServiceDays { get; set; }

    /// <summary>
    /// مكافأة نهاية الخدمة
    /// </summary>
    public decimal GratuityAmount { get; set; }

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