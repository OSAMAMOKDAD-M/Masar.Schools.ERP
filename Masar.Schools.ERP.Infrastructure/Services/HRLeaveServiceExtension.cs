using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// امتداد خدمة الإجازات مع نظام 21/30 المتدرج
/// </summary>
public class HRLeaveServiceExtension
{
    private readonly MasarDbContext _context;

    public HRLeaveServiceExtension(MasarDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// حساب المستحق السنوي حسب نظام 21/30 المتدرج
    /// </summary>
    public async Task<decimal> CalculateAnnualEntitlementAsync(Guid employeeId, int yearNumber)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null) return 0;

        var completedYears = yearNumber - 1;

        // السنوات 1-5: 21 يوم
        if (completedYears < 5)
            return 21;

        // السنة 6+: 30 يوم
        return 30;
    }

    /// <summary>
    /// حساب رصيد الإجازة الكامل
    /// </summary>
    public async Task<LeaveBalanceCalculation> CalculateLeaveBalanceAsync(Guid employeeId, string calculationMode)
    {
        var employee = await _context.Employees
            .Include(e => e.Contracts)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            return new LeaveBalanceCalculation();

        var currentContract = employee.Contracts.FirstOrDefault(c => c.IsCurrent);
        if (currentContract == null)
            return new LeaveBalanceCalculation();

        var startDate = currentContract.StartDate;
        var endDate = currentContract.EndDate;
        var totalYears = (int)((endDate - startDate).TotalDays / 365);

        decimal thisYearEntitlement = 0;
        decimal carryover = 0;
        decimal used = 0;

        if (calculationMode == "annual")
        {
            // الرصيد السنوي الكامل
            thisYearEntitlement = await CalculateAnnualEntitlementAsync(employeeId, totalYears + 1);
        }
        else
        {
            // المستحق الشهري حتى اليوم
            var monthsWorked = (int)((DateTime.Now - startDate).TotalDays / 30);
            var monthlyRate = await CalculateAnnualEntitlementAsync(employeeId, totalYears + 1) / 12;
            thisYearEntitlement = monthlyRate * monthsWorked;
        }

        // الرصيد المنقول
        carryover = await CalculateCarryoverAsync(employeeId);

        // المستخدم
        used = await _context.LeaveRequests
            .Where(l => l.EmployeeId == employeeId && 
                       l.StartDate.Year == DateTime.Now.Year &&
                       l.Status == "Approved" &&
                       !l.IsDeleted)
            .SumAsync(l => (decimal)l.DaysCount);

        return new LeaveBalanceCalculation
        {
            ThisYearEntitlement = thisYearEntitlement,
            CarryoverRemaining = carryover,
            Used = used,
            TotalRemaining = thisYearEntitlement + carryover - used,
            ServiceYears = totalYears
        };
    }

    /// <summary>
    /// حساب الرصيد المنقول من السنة السابقة
    /// </summary>
    public async Task<decimal> CalculateCarryoverAsync(Guid employeeId)
    {
        var lastYear = DateTime.Now.Year - 1;

        var lastYearBalance = await _context.LeaveBalances
            .Where(l => l.EmployeeId == employeeId && 
                       l.FiscalYear == lastYear &&
                       l.IsActive)
            .FirstOrDefaultAsync();

        if (lastYearBalance == null) return 0;

        // الحد الأقصى للرصيد المنقول هو 10 أيام
        var remaining = lastYearBalance.AnnualAllowance - lastYearBalance.UsedDays;
        return Math.Min(remaining, 10);
    }
}