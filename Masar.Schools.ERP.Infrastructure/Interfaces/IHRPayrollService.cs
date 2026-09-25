using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة الرواتب
/// </summary>
public interface IHRPayrollService
{
    Task<List<PayrollProfile>> GetPayrollProfilesAsync(DateTime? periodStart, DateTime? periodEnd);
    Task<PayrollProfile?> GetPayrollProfileByIdAsync(Guid id);
    Task<PayrollProfile> CreatePayrollProfileAsync(PayrollCreateDto dto);
    Task<PayrollProfile?> UpdatePayrollProfileAsync(Guid id, PayrollCreateDto dto);
    Task<bool> DeletePayrollProfileAsync(Guid id);
    Task<PayrollCalculation> CalculatePayrollAsync(Guid employeeId, PayrollCreateDto dto);
    Task<bool> ArchivePayrollAsync(List<Guid> profileUids);
    Task<List<PayrollArchive>> GetPayrollArchivesAsync(DateTime? from, DateTime? to);
    Task<GosiCalculation> CalculateGosiAsync(PayrollProfile profile, Employee employee);
}

/// <summary>
/// DTO لإنشاء الرواتب
/// </summary>
public class PayrollCreateDto
{
    public Guid EmployeeId { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public decimal Basic { get; set; }
    public decimal Housing { get; set; }
    public decimal Transport { get; set; }
    public decimal OtherEarnings { get; set; }
    public decimal VariableEarnings { get; set; }
    public decimal Deductions { get; set; }
    public decimal AbsenceDays { get; set; }
    public decimal LateDays { get; set; }
    public decimal LateDeduction { get; set; }
    public bool GosiEnabled { get; set; } = true;
    public string GosiScheme { get; set; } = "auto";
    public decimal GosiCommission { get; set; }
    public decimal GosiInKindHousing { get; set; }
    public decimal GosiWage { get; set; }
    public int GosiServiceDays { get; set; } = 30;
    public decimal GosiEmployeePensionRate { get; set; }
    public decimal GosiEmployerPensionRate { get; set; }
    public decimal GosiEmployeeSanedRate { get; set; }
    public decimal GosiEmployerSanedRate { get; set; }
    public decimal GosiOccupationalHazardRate { get; set; }
    public string? Notes { get; set; }
    public Guid SchoolId { get; set; }
}

/// <summary>
/// حساب الرواتب
/// </summary>
public class PayrollCalculation
{
    public decimal Gross { get; set; }
    public decimal AbsenceDeduction { get; set; }
    public decimal LateDeduction { get; set; }
    public GosiCalculation Gosi { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal Net { get; set; }
    public decimal EmployerCost { get; set; }
}

/// <summary>
/// حساب GOSI
/// </summary>
public class GosiCalculation
{
    public bool Enabled { get; set; }
    public string Scheme { get; set; }
    public decimal Wage { get; set; }
    public int ServiceDays { get; set; }
    public decimal EmployeePensionRate { get; set; }
    public decimal EmployerPensionRate { get; set; }
    public decimal EmployeeSanedRate { get; set; }
    public decimal EmployerSanedRate { get; set; }
    public decimal HazardRate { get; set; }
    public decimal EmployeeContribution { get; set; }
    public decimal EmployerContribution { get; set; }
}