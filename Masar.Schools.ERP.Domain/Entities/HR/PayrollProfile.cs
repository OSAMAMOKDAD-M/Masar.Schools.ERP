using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.HR;

/// <summary>
/// كيان ملف الرواتب
/// </summary>
public class PayrollProfile : BaseEntity
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
    /// تاريخ السريان من
    /// </summary>
    public DateTime? EffectiveFrom { get; set; }

    /// <summary>
    /// بداية الفترة
    /// </summary>
    public DateTime? PeriodStart { get; set; }

    /// <summary>
    /// نهاية الفترة
    /// </summary>
    public DateTime? PeriodEnd { get; set; }

    /// <summary>
    /// الراتب الأساسي
    /// </summary>
    public decimal Basic { get; set; }

    /// <summary>
    /// بدل السكن
    /// </summary>
    public decimal Housing { get; set; }

    /// <summary>
    /// بدل النقل
    /// </summary>
    public decimal Transport { get; set; }

    /// <summary>
    /// بدلات أخرى
    /// </summary>
    public decimal OtherEarnings { get; set; }

    /// <summary>
    /// بدلات متغيرة
    /// </summary>
    public decimal VariableEarnings { get; set; }

    /// <summary>
    /// الخصومات
    /// </summary>
    public decimal Deductions { get; set; }

    /// <summary>
    /// أيام الغياب
    /// </summary>
    public decimal AbsenceDays { get; set; }

    /// <summary>
    /// أيام التأخير
    /// </summary>
    public decimal LateDays { get; set; }

    /// <summary>
    /// خصم التأخير
    /// </summary>
    public decimal LateDeduction { get; set; }

    /// <summary>
    /// تفعيل GOSI
    /// </summary>
    public bool GosiEnabled { get; set; } = true;

    /// <summary>
    /// نظام GOSI (auto, manual)
    /// </summary>
    public string GosiScheme { get; set; } = "auto";

    /// <summary>
    /// عمولة GOSI
    /// </summary>
    public decimal GosiCommission { get; set; }

    /// <summary>
    /// سكن عيني GOSI
    /// </summary>
    public decimal GosiInKindHousing { get; set; }

    /// <summary>
    /// أجر GOSI
    /// </summary>
    public decimal GosiWage { get; set; }

    /// <summary>
    /// أيام الخدمة GOSI
    /// </summary>
    public int GosiServiceDays { get; set; } = 30;

    /// <summary>
    /// معدل تقاعد الموظف GOSI
    /// </summary>
    public decimal GosiEmployeePensionRate { get; set; }

    /// <summary>
    /// معدل تقاعد صاحب العمل GOSI
    /// </summary>
    public decimal GosiEmployerPensionRate { get; set; }

    /// <summary>
    /// معدل ساند الموظف GOSI
    /// </summary>
    public decimal GosiEmployeeSanedRate { get; set; }

    /// <summary>
    /// معدل ساند صاحب العمل GOSI
    /// </summary>
    public decimal GosiEmployerSanedRate { get; set; }

    /// <summary>
    /// معدل المخاطر المهنية GOSI
    /// </summary>
    public decimal GosiOccupationalHazardRate { get; set; }

    /// <summary>
    /// مساهمة الموظف GOSI
    /// </summary>
    public decimal GosiEmployeeContribution { get; set; }

    /// <summary>
    /// مساهمة صاحب العمل GOSI
    /// </summary>
    public decimal GosiEmployerContribution { get; set; }

    /// <summary>
    /// تمت الموافقة
    /// </summary>
    public bool Approved { get; set; }

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