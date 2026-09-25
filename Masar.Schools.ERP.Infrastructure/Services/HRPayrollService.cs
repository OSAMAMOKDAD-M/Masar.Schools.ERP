using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة الرواتب
/// </summary>
public class HRPayrollService : IHRPayrollService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HRPayrollService> _logger;

    public HRPayrollService(MasarDbContext context, ILogger<HRPayrollService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PayrollProfile>> GetPayrollProfilesAsync(DateTime? periodStart, DateTime? periodEnd)
    {
        var query = _context.PayrollProfiles
            .Include(p => p.Employee)
            .Include(p => p.School)
            .AsQueryable();

        if (periodStart.HasValue)
            query = query.Where(p => p.PeriodStart >= periodStart.Value);

        if (periodEnd.HasValue)
            query = query.Where(p => p.PeriodEnd <= periodEnd.Value);

        return await query.OrderByDescending(p => p.EffectiveFrom).ToListAsync();
    }

    public async Task<PayrollProfile?> GetPayrollProfileByIdAsync(Guid id)
    {
        return await _context.PayrollProfiles
            .Include(p => p.Employee)
            .Include(p => p.School)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PayrollProfile> CreatePayrollProfileAsync(PayrollCreateDto dto)
    {
        var profile = new PayrollProfile
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            EffectiveFrom = dto.EffectiveFrom,
            PeriodStart = dto.PeriodStart,
            PeriodEnd = dto.PeriodEnd,
            Basic = dto.Basic,
            Housing = dto.Housing,
            Transport = dto.Transport,
            OtherEarnings = dto.OtherEarnings,
            VariableEarnings = dto.VariableEarnings,
            Deductions = dto.Deductions,
            AbsenceDays = dto.AbsenceDays,
            LateDays = dto.LateDays,
            LateDeduction = dto.LateDeduction,
            GosiEnabled = dto.GosiEnabled,
            GosiScheme = dto.GosiScheme,
            GosiCommission = dto.GosiCommission,
            GosiInKindHousing = dto.GosiInKindHousing,
            GosiWage = dto.GosiWage,
            GosiServiceDays = dto.GosiServiceDays,
            GosiEmployeePensionRate = dto.GosiEmployeePensionRate,
            GosiEmployerPensionRate = dto.GosiEmployerPensionRate,
            GosiEmployeeSanedRate = dto.GosiEmployeeSanedRate,
            GosiEmployerSanedRate = dto.GosiEmployerSanedRate,
            GosiOccupationalHazardRate = dto.GosiOccupationalHazardRate,
            Notes = dto.Notes,
            SchoolId = dto.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        // Calculate GOSI
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee != null)
        {
            var gosiCalculation = await CalculateGosiAsync(profile, employee);
            profile.GosiEmployeeContribution = gosiCalculation.EmployeeContribution;
            profile.GosiEmployerContribution = gosiCalculation.EmployerContribution;
        }

        _context.PayrollProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return profile;
    }

    public async Task<PayrollProfile?> UpdatePayrollProfileAsync(Guid id, PayrollCreateDto dto)
    {
        var profile = await _context.PayrollProfiles.FindAsync(id);
        if (profile == null) return null;

        profile.EffectiveFrom = dto.EffectiveFrom;
        profile.PeriodStart = dto.PeriodStart;
        profile.PeriodEnd = dto.PeriodEnd;
        profile.Basic = dto.Basic;
        profile.Housing = dto.Housing;
        profile.Transport = dto.Transport;
        profile.OtherEarnings = dto.OtherEarnings;
        profile.VariableEarnings = dto.VariableEarnings;
        profile.Deductions = dto.Deductions;
        profile.AbsenceDays = dto.AbsenceDays;
        profile.LateDays = dto.LateDays;
        profile.LateDeduction = dto.LateDeduction;
        profile.GosiEnabled = dto.GosiEnabled;
        profile.GosiScheme = dto.GosiScheme;
        profile.GosiCommission = dto.GosiCommission;
        profile.GosiInKindHousing = dto.GosiInKindHousing;
        profile.GosiWage = dto.GosiWage;
        profile.GosiServiceDays = dto.GosiServiceDays;
        profile.GosiEmployeePensionRate = dto.GosiEmployeePensionRate;
        profile.GosiEmployerPensionRate = dto.GosiEmployerPensionRate;
        profile.GosiEmployeeSanedRate = dto.GosiEmployeeSanedRate;
        profile.GosiEmployerSanedRate = dto.GosiEmployerSanedRate;
        profile.GosiOccupationalHazardRate = dto.GosiOccupationalHazardRate;
        profile.Notes = dto.Notes;
        profile.UpdatedAt = DateTime.Now;
        profile.UpdatedBy = "System";

        // Recalculate GOSI
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee != null)
        {
            var gosiCalculation = await CalculateGosiAsync(profile, employee);
            profile.GosiEmployeeContribution = gosiCalculation.EmployeeContribution;
            profile.GosiEmployerContribution = gosiCalculation.EmployerContribution;
        }

        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<bool> DeletePayrollProfileAsync(Guid id)
    {
        var profile = await _context.PayrollProfiles.FindAsync(id);
        if (profile == null) return false;

        profile.IsDeleted = true;
        profile.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PayrollCalculation> CalculatePayrollAsync(Guid employeeId, PayrollCreateDto dto)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var gross = dto.Basic + dto.Housing + dto.Transport + 
                     dto.OtherEarnings + dto.VariableEarnings;

        var absenceDeduction = (dto.Basic / 30) * dto.AbsenceDays;
        var lateDeduction = dto.LateDeduction;

        var profile = new PayrollProfile
        {
            GosiEnabled = dto.GosiEnabled,
            GosiScheme = dto.GosiScheme,
            GosiWage = dto.GosiWage,
            GosiServiceDays = dto.GosiServiceDays,
            Basic = dto.Basic
        };

        var gosi = await CalculateGosiAsync(profile, employee);

        var totalDeductions = dto.Deductions + absenceDeduction + 
                              lateDeduction + gosi.EmployeeContribution;

        var net = gross - totalDeductions;
        var employerCost = gross + gosi.EmployerContribution;

        return new PayrollCalculation
        {
            Gross = gross,
            AbsenceDeduction = absenceDeduction,
            LateDeduction = lateDeduction,
            Gosi = gosi,
            TotalDeductions = totalDeductions,
            Net = net,
            EmployerCost = employerCost
        };
    }

    public async Task<bool> ArchivePayrollAsync(List<Guid> profileUids)
    {
        foreach (var profileId in profileUids)
        {
            var profile = await _context.PayrollProfiles
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == profileId);

            if (profile == null) continue;

            var archive = new PayrollArchive
            {
                Id = Guid.NewGuid(),
                EmployeeId = profile.EmployeeId,
                EmployeeName = profile.Employee.FullName,
                EmployeeNameArabic = profile.Employee.FullNameArabic,
                EmployeeCode = profile.Employee.EmployeeNumber,
                PeriodStart = profile.PeriodStart,
                PeriodEnd = profile.PeriodEnd,
                Basic = profile.Basic,
                Housing = profile.Housing,
                Transport = profile.Transport,
                OtherEarnings = profile.OtherEarnings,
                VariableEarnings = profile.VariableEarnings,
                Deductions = profile.Deductions,
                AbsenceDays = profile.AbsenceDays,
                AbsenceDeduction = (profile.Basic / 30) * profile.AbsenceDays,
                LateDays = profile.LateDays,
                LateDeduction = profile.LateDeduction,
                GosiEnabled = profile.GosiEnabled,
                GosiScheme = profile.GosiScheme,
                GosiCommission = profile.GosiCommission,
                GosiInKindHousing = profile.GosiInKindHousing,
                GosiWage = profile.GosiWage,
                GosiServiceDays = profile.GosiServiceDays,
                GosiEmployeePensionRate = profile.GosiEmployeePensionRate,
                GosiEmployerPensionRate = profile.GosiEmployerPensionRate,
                GosiEmployeeSanedRate = profile.GosiEmployeeSanedRate,
                GosiEmployerSanedRate = profile.GosiEmployerSanedRate,
                GosiOccupationalHazardRate = profile.GosiOccupationalHazardRate,
                GosiEmployeeContribution = profile.GosiEmployeeContribution,
                GosiEmployerContribution = profile.GosiEmployerContribution,
                NetSalary = profile.Basic + profile.Housing + profile.Transport + 
                           profile.OtherEarnings + profile.VariableEarnings - 
                           profile.Deductions - ((profile.Basic / 30) * profile.AbsenceDays) - 
                           profile.LateDeduction - profile.GosiEmployeeContribution,
                Notes = profile.Notes,
                SchoolId = profile.SchoolId,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            _context.PayrollArchives.Add(archive);

            profile.IsDeleted = true;
            profile.DeletedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PayrollArchive>> GetPayrollArchivesAsync(DateTime? from, DateTime? to)
    {
        var query = _context.PayrollArchives
            .Include(p => p.Employee)
            .Include(p => p.School)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(p => p.PeriodStart >= from.Value);

        if (to.HasValue)
            query = query.Where(p => p.PeriodEnd <= to.Value);

        return await query.OrderByDescending(p => p.PeriodEnd).ToListAsync();
    }

    public async Task<GosiCalculation> CalculateGosiAsync(PayrollProfile profile, Employee employee)
    {
        if (!profile.GosiEnabled)
            return new GosiCalculation { Enabled = false };

        var gosiWage = profile.GosiWage > 0 ? profile.GosiWage : profile.Basic;
        var serviceDays = profile.GosiServiceDays > 0 ? profile.GosiServiceDays : 30;

        // Default GOSI rates (can be customized)
        var employeePensionRate = 0.09m; // 9%
        var employerPensionRate = 0.09m; // 9%
        var employeeSanedRate = 0.01m; // 1%
        var employerSanedRate = 0.02m; // 2%
        var hazardRate = 0.01m; // 1%

        // Use custom rates if provided
        if (profile.GosiEmployeePensionRate > 0) employeePensionRate = profile.GosiEmployeePensionRate;
        if (profile.GosiEmployerPensionRate > 0) employerPensionRate = profile.GosiEmployerPensionRate;
        if (profile.GosiEmployeeSanedRate > 0) employeeSanedRate = profile.GosiEmployeeSanedRate;
        if (profile.GosiEmployerSanedRate > 0) employerSanedRate = profile.GosiEmployerSanedRate;
        if (profile.GosiOccupationalHazardRate > 0) hazardRate = profile.GosiOccupationalHazardRate;

        var adjustedWage = (gosiWage * serviceDays / 30);

        var employeeContribution = adjustedWage * (employeePensionRate + employeeSanedRate);
        var employerContribution = adjustedWage * (employerPensionRate + employerSanedRate + hazardRate);

        return new GosiCalculation
        {
            Enabled = true,
            Scheme = profile.GosiScheme,
            Wage = gosiWage,
            ServiceDays = serviceDays,
            EmployeePensionRate = employeePensionRate,
            EmployerPensionRate = employerPensionRate,
            EmployeeSanedRate = employeeSanedRate,
            EmployerSanedRate = employerSanedRate,
            HazardRate = hazardRate,
            EmployeeContribution = employeeContribution,
            EmployerContribution = employerContribution
        };
    }
}