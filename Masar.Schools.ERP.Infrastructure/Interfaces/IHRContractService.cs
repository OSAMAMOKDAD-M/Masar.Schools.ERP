using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة العقود
/// </summary>
public interface IHRContractService
{
    Task<List<Contract>> GetEmployeeContractsAsync(Guid employeeId);
    Task<Contract?> GetContractByIdAsync(Guid id);
    Task<Contract> CreateContractAsync(ContractCreateDto dto);
    Task<Contract?> UpdateContractAsync(Guid id, ContractCreateDto dto);
    Task<bool> DeleteContractAsync(Guid id);
    Task<Contract?> RenewContractAsync(Guid employeeId, ContractRenewDto dto);
    Task<bool> ArchiveContractAsync(Guid contractId, string reason);
    Task<List<ContractArchive>> GetContractArchivesAsync(Guid employeeId);
    Task<ContractDuration> CalculateDurationAsync(DateTime start, DateTime end);
}

/// <summary>
/// DTO لإنشاء العقد
/// </summary>
public class ContractCreateDto
{
    public Guid EmployeeId { get; set; }
    public int ContractNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Salary { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Department { get; set; }
    public string? DepartmentArabic { get; set; }
    public string? Notes { get; set; }
    public Guid SchoolId { get; set; }
}

/// <summary>
/// DTO لتجديد العقد
/// </summary>
public class ContractRenewDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Salary { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Department { get; set; }
    public string? DepartmentArabic { get; set; }
    public ContractDuration Duration { get; set; }
}

/// <summary>
/// مدة العقد
/// </summary>
public class ContractDuration
{
    public int Years { get; set; }
    public int Months { get; set; }
    public int Days { get; set; }
}