using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان أرشيف العقود
/// </summary>
public class ContractArchive : BaseEntity
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
    /// رقم العقد الأصلي
    /// </summary>
    public int OriginalContractNumber { get; set; }

    /// <summary>
    /// تاريخ بدء العقد
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ انتهاء العقد
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// الراتب
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
    /// سبب الأرشفة
    /// </summary>
    public string? ArchiveReason { get; set; }

    /// <summary>
    /// سبب الأرشفة بالعربية
    /// </summary>
    public string? ArchiveReasonArabic { get; set; }

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