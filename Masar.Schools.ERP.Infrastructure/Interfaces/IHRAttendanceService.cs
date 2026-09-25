using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة الحضور والانصراف
/// </summary>
public interface IHRAttendanceService
{
    Task<List<AttendanceLog>> GetAttendanceLogsAsync(DateTime? from, DateTime? to);
    Task<AttendanceLog?> GetAttendanceLogByIdAsync(Guid id);
    Task<AttendanceLog> CreateAttendanceLogAsync(AttendanceCreateDto dto);
    Task<bool> DeleteAttendanceLogAsync(Guid id);
    Task ImportAttendanceFromExcelAsync(IFormFile file);
    Task<List<AttendancePermit>> GetPermitsAsync(DateTime? from, DateTime? to);
    Task<AttendancePermit> CreatePermitAsync(PermitCreateDto dto);
    Task<bool> DeletePermitAsync(Guid id);
    Task<AttendanceSummary> CalculateAttendanceSummaryAsync(Guid employeeId, DateTime from, DateTime to);
}

/// <summary>
/// DTO لإنشاء سجل الحضور
/// </summary>
public class AttendanceCreateDto
{
    public Guid? EmployeeId { get; set; }
    public string BiometricId { get; set; } = string.Empty;
    public DateTime EventAt { get; set; }
    public string Direction { get; set; } = "غير مصنف";
    public string Source { get; set; } = "يدوي";
    public string? DeviceName { get; set; }
    public string? Notes { get; set; }
    public Guid SchoolId { get; set; }
}

/// <summary>
/// DTO لإنشاء تصريح
/// </summary>
public class PermitCreateDto
{
    public Guid EmployeeId { get; set; }
    public DateTime PermitDate { get; set; }
    public decimal DurationHours { get; set; }
    public string? Note { get; set; }
    public Guid SchoolId { get; set; }
}

/// <summary>
/// ملخص الحضور
/// </summary>
public class AttendanceSummary
{
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public decimal TotalHours { get; set; }
    public decimal WorkedHours { get; set; }
    public decimal PermitHours { get; set; }
}