using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة الحضور والانصراف
/// </summary>
public class HRAttendanceService : IHRAttendanceService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HRAttendanceService> _logger;

    public HRAttendanceService(MasarDbContext context, ILogger<HRAttendanceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<AttendanceLog>> GetAttendanceLogsAsync(DateTime? from, DateTime? to)
    {
        var query = _context.AttendanceLogs
            .Include(a => a.Employee)
            .Include(a => a.School)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(a => a.EventAt >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.EventAt <= to.Value);

        return await query.OrderByDescending(a => a.EventAt).ToListAsync();
    }

    public async Task<AttendanceLog?> GetAttendanceLogByIdAsync(Guid id)
    {
        return await _context.AttendanceLogs
            .Include(a => a.Employee)
            .Include(a => a.School)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AttendanceLog> CreateAttendanceLogAsync(AttendanceCreateDto dto)
    {
        var log = new AttendanceLog
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            BiometricId = dto.BiometricId,
            EventAt = dto.EventAt,
            Direction = dto.Direction,
            Source = dto.Source,
            DeviceName = dto.DeviceName,
            Notes = dto.Notes,
            SchoolId = dto.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.AttendanceLogs.Add(log);
        await _context.SaveChangesAsync();

        return log;
    }

    public async Task<bool> DeleteAttendanceLogAsync(Guid id)
    {
        var log = await _context.AttendanceLogs.FindAsync(id);
        if (log == null) return false;

        log.IsDeleted = true;
        log.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task ImportAttendanceFromExcelAsync(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        if (worksheet == null)
            throw new Exception("Excel file is empty or has no worksheets");

        var rows = new List<AttendanceCreateDto>();

        var lastRow = worksheet.LastRowUsed();
        if (lastRow == null)
            throw new Exception("Excel file has no data rows");

        var rowCount = lastRow.RowNumber();
        for (int row = 2; row <= rowCount; row++)
        {
            var biometricId = worksheet.Cell(row, 1).GetString();
            var eventDateText = worksheet.Cell(row, 2).GetString();
            var eventTimeText = worksheet.Cell(row, 3).GetString();
            var direction = worksheet.Cell(row, 4).GetString();

            if (string.IsNullOrWhiteSpace(biometricId) || 
                string.IsNullOrWhiteSpace(eventDateText) || 
                string.IsNullOrWhiteSpace(eventTimeText))
                continue;

            DateTime eventDate;
            DateTime eventTime;

            if (!DateTime.TryParse(eventDateText, out eventDate))
                continue;

            if (!DateTime.TryParse(eventTimeText, out eventTime))
                continue;

            var eventAt = eventDate.Date + eventTime.TimeOfDay;

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.BiometricId == biometricId);

            rows.Add(new AttendanceCreateDto
            {
                EmployeeId = employee?.Id,
                BiometricId = biometricId,
                EventAt = eventAt,
                Direction = direction.ToUpper() switch
                {
                    "IN" => "IN",
                    "OUT" => "OUT",
                    _ => "غير مصنف"
                },
                Source = "Excel",
                SchoolId = employee?.SchoolId ?? Guid.Empty
            });
        }

        // Import data
        foreach (var row in rows)
        {
            var log = new AttendanceLog
            {
                Id = Guid.NewGuid(),
                EmployeeId = row.EmployeeId,
                BiometricId = row.BiometricId,
                EventAt = row.EventAt,
                Direction = row.Direction,
                Source = row.Source,
                SchoolId = row.SchoolId,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            _context.AttendanceLogs.Add(log);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<AttendancePermit>> GetPermitsAsync(DateTime? from, DateTime? to)
    {
        var query = _context.AttendancePermits
            .Include(p => p.Employee)
            .Include(p => p.School)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(p => p.PermitDate >= from.Value);

        if (to.HasValue)
            query = query.Where(p => p.PermitDate <= to.Value);

        return await query.OrderByDescending(p => p.PermitDate).ToListAsync();
    }

    public async Task<AttendancePermit> CreatePermitAsync(PermitCreateDto dto)
    {
        var permit = new AttendancePermit
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            PermitDate = dto.PermitDate,
            DurationHours = dto.DurationHours,
            Note = dto.Note,
            SchoolId = dto.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.AttendancePermits.Add(permit);
        await _context.SaveChangesAsync();

        return permit;
    }

    public async Task<bool> DeletePermitAsync(Guid id)
    {
        var permit = await _context.AttendancePermits.FindAsync(id);
        if (permit == null) return false;

        permit.IsDeleted = true;
        permit.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<AttendanceSummary> CalculateAttendanceSummaryAsync(Guid employeeId, DateTime from, DateTime to)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var totalDays = (to - from).Days + 1;
        var workDays = 0;

        // Calculate work days (excluding weekends)
        for (var date = from; date <= to; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Friday && date.DayOfWeek != DayOfWeek.Saturday)
                workDays++;
        }

        var attendanceLogs = await _context.AttendanceLogs
            .Where(a => a.EmployeeId == employeeId && 
                       a.EventAt >= from && 
                       a.EventAt <= to)
            .ToListAsync();

        var permits = await _context.AttendancePermits
            .Where(p => p.EmployeeId == employeeId && 
                       p.PermitDate >= from && 
                       p.PermitDate <= to)
            .ToListAsync();

        // Group by date to determine presence
        var attendanceByDate = attendanceLogs
            .GroupBy(a => a.EventAt.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        var presentDays = 0;
        var lateDays = 0;
        var totalHours = 0m;
        var workedHours = 0m;
        var permitHours = permits.Sum(p => p.DurationHours);

        foreach (var date in attendanceByDate.Keys)
        {
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                continue;

            var dayLogs = attendanceByDate[date];
            var inLog = dayLogs.FirstOrDefault(l => l.Direction == "IN");
            var outLog = dayLogs.FirstOrDefault(l => l.Direction == "OUT");

            if (inLog != null && outLog != null)
            {
                presentDays++;
                var hours = (decimal)(outLog.EventAt - inLog.EventAt).TotalHours;
                workedHours += hours;
                totalHours += 8; // Standard work day

                // Check for late arrival (after 8:00 AM)
                if (inLog.EventAt.TimeOfDay > TimeSpan.FromHours(8))
                {
                    lateDays++;
                }
            }
        }

        var absentDays = workDays - presentDays;

        return new AttendanceSummary
        {
            TotalDays = totalDays,
            PresentDays = presentDays,
            AbsentDays = absentDays,
            LateDays = lateDays,
            TotalHours = totalHours,
            WorkedHours = workedHours,
            PermitHours = permitHours
        };
    }
}