using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة العقود
/// </summary>
public class HRContractService : IHRContractService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HRContractService> _logger;

    public HRContractService(MasarDbContext context, ILogger<HRContractService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Contract>> GetEmployeeContractsAsync(Guid employeeId)
    {
        return await _context.Contracts
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();
    }

    public async Task<Contract?> GetContractByIdAsync(Guid id)
    {
        return await _context.Contracts
            .Include(c => c.Employee)
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contract> CreateContractAsync(ContractCreateDto dto)
    {
        var duration = await CalculateDurationAsync(dto.StartDate, dto.EndDate);

        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            ContractNumber = dto.ContractNumber,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Salary = dto.Salary,
            JobTitle = dto.JobTitle,
            JobTitleArabic = dto.JobTitleArabic,
            Department = dto.Department,
            DepartmentArabic = dto.DepartmentArabic,
            DurationYears = duration.Years,
            DurationMonths = duration.Months,
            DurationDays = duration.Days,
            Status = "ساري",
            StatusArabic = "ساري",
            IsCurrent = true,
            Notes = dto.Notes,
            SchoolId = dto.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        // Archive previous current contract
        var previousContract = await _context.Contracts
            .FirstOrDefaultAsync(c => c.EmployeeId == dto.EmployeeId && c.IsCurrent);

        if (previousContract != null)
        {
            previousContract.IsCurrent = false;
            previousContract.Status = "منتهٍ/مؤرشف";
            previousContract.StatusArabic = "منتهٍ/مؤرشف";
        }

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();

        // Update employee contract info
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee != null)
        {
            employee.ContractStart = dto.StartDate;
            employee.ContractEnd = dto.EndDate;
            employee.Salary = dto.Salary;
            employee.JobTitle = dto.JobTitle;
            employee.JobTitleArabic = dto.JobTitleArabic;
            employee.Department = dto.Department;
            employee.DepartmentArabic = dto.DepartmentArabic;
            await _context.SaveChangesAsync();
        }

        return contract;
    }

    public async Task<Contract?> UpdateContractAsync(Guid id, ContractCreateDto dto)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null) return null;

        var duration = await CalculateDurationAsync(dto.StartDate, dto.EndDate);

        contract.StartDate = dto.StartDate;
        contract.EndDate = dto.EndDate;
        contract.Salary = dto.Salary;
        contract.JobTitle = dto.JobTitle;
        contract.JobTitleArabic = dto.JobTitleArabic;
        contract.Department = dto.Department;
        contract.DepartmentArabic = dto.DepartmentArabic;
        contract.DurationYears = duration.Years;
        contract.DurationMonths = duration.Months;
        contract.DurationDays = duration.Days;
        contract.Notes = dto.Notes;
        contract.UpdatedAt = DateTime.Now;
        contract.UpdatedBy = "System";

        await _context.SaveChangesAsync();
        return contract;
    }

    public async Task<bool> DeleteContractAsync(Guid id)
    {
        var contract = await _context.Contracts.FindAsync(id);
        if (contract == null) return false;

        contract.IsDeleted = true;
        contract.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Contract?> RenewContractAsync(Guid employeeId, ContractRenewDto dto)
    {
        // Archive current contract
        var currentContract = await _context.Contracts
            .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.IsCurrent);

        if (currentContract != null)
        {
            currentContract.IsCurrent = false;
            currentContract.Status = "منتهٍ/مؤرشف";
            currentContract.StatusArabic = "منتهٍ/مؤرشف";
        }

        // Get next contract number
        var lastContractNumber = await _context.Contracts
            .Where(c => c.EmployeeId == employeeId)
            .MaxAsync(c => (int?)c.ContractNumber) ?? 0;

        var newContract = new Contract
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            ContractNumber = lastContractNumber + 1,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Salary = dto.Salary,
            JobTitle = dto.JobTitle,
            JobTitleArabic = dto.JobTitleArabic,
            Department = dto.Department,
            DepartmentArabic = dto.DepartmentArabic,
            DurationYears = dto.Duration.Years,
            DurationMonths = dto.Duration.Months,
            DurationDays = dto.Duration.Days,
            Status = "ساري",
            StatusArabic = "ساري",
            IsCurrent = true,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.Contracts.Add(newContract);

        // Update employee
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee != null)
        {
            employee.ContractStart = dto.StartDate;
            employee.ContractEnd = dto.EndDate;
            employee.Salary = dto.Salary;
            employee.JobTitle = dto.JobTitle;
            employee.JobTitleArabic = dto.JobTitleArabic;
            employee.Department = dto.Department;
            employee.DepartmentArabic = dto.DepartmentArabic;
        }

        await _context.SaveChangesAsync();
        return newContract;
    }

    public async Task<bool> ArchiveContractAsync(Guid contractId, string reason)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        if (contract == null) return false;

        var archive = new ContractArchive
        {
            Id = Guid.NewGuid(),
            EmployeeId = contract.EmployeeId,
            OriginalContractNumber = contract.ContractNumber,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Salary = contract.Salary,
            JobTitle = contract.JobTitle,
            JobTitleArabic = contract.JobTitleArabic,
            Department = contract.Department,
            DepartmentArabic = contract.DepartmentArabic,
            ArchiveReason = reason,
            ArchiveReasonArabic = reason,
            SchoolId = contract.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.ContractArchives.Add(archive);

        contract.IsDeleted = true;
        contract.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ContractArchive>> GetContractArchivesAsync(Guid employeeId)
    {
        return await _context.ContractArchives
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();
    }

    public async Task<ContractDuration> CalculateDurationAsync(DateTime start, DateTime end)
    {
        var totalDays = (end - start).Days;
        var years = totalDays / 365;
        var remainingDays = totalDays % 365;
        var months = remainingDays / 30;
        var days = remainingDays % 30;

        return new ContractDuration
        {
            Years = years,
            Months = months,
            Days = days
        };
    }
}