using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة مكافأة نهاية الخدمة
/// </summary>
public interface IHRGratuityService
{
    Task<GratuityCalculation> CalculateGratuityAsync(Guid employeeId, DateTime endDate);
    Task<ResignedEmployee> CreateResignedEmployeeAsync(ResignedEmployeeCreateDto dto);
    Task<List<ResignedEmployee>> GetResignedEmployeesAsync();
    Task<ResignedEmployee?> GetResignedEmployeeByIdAsync(Guid id);
    Task<byte[]> GenerateGratuityLetterAsync(Guid employeeId, DateTime endDate, LetterType type);
}

/// <summary>
/// DTO لإنشاء موظف مستقيل
/// </summary>
public class ResignedEmployeeCreateDto
{
    public Guid EmployeeId { get; set; }
    public string? Reason { get; set; }
    public string? ReasonArabic { get; set; }
    public DateTime ResignDate { get; set; }
    public string? Notes { get; set; }
    public Guid SchoolId { get; set; }
}

/// <summary>
/// حساب مكافأة نهاية الخدمة
/// </summary>
public class GratuityCalculation
{
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeNameArabic { get; set; } = string.Empty;
    public string? EmployeeCode { get; set; }
    public decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ServiceYears { get; set; }
    public int ServiceMonths { get; set; }
    public int ServiceDays { get; set; }
    public decimal GratuityAmount { get; set; }
    public string CalculationDetails { get; set; } = string.Empty;
}

/// <summary>
/// نوع الخطاب
/// </summary>
public enum LetterType
{
    ExperienceCertificate,
    ServiceCertificate,
    GratuityCalculation
}