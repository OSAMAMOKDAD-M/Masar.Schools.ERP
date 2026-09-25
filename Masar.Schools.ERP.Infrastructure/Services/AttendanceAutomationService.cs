using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

public class AttendanceAutomationService : BackgroundService, IAttendanceAutomationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AttendanceAutomationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _checkInterval;

    public AttendanceAutomationService(
        IServiceProvider serviceProvider,
        ILogger<AttendanceAutomationService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        _checkInterval = TimeSpan.FromMinutes(5); // Check every 5 minutes
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Attendance Automation Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAttendanceRecordsAsync(stoppingToken);
                await ProcessFingerprintDataAsync(stoppingToken);
                await CheckAbsentStudentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Attendance Automation Service");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Attendance Automation Service stopped.");
    }

    private async Task ProcessAttendanceRecordsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasarDbContext>();
        var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();

        try
        {
            // Get attendance records that haven't sent WhatsApp notifications
            var pendingNotifications = await dbContext.AttendanceRecords
                .Include(a => a.Student)
                .Where(a => !a.WhatsAppNotificationSent && a.CheckInTime.HasValue)
                .ToListAsync(stoppingToken);

            foreach (var record in pendingNotifications)
            {
                try
                {
                    // Get guardian through student relationship if it exists
                    if (record.Student.GuardianId.HasValue && !string.IsNullOrEmpty(record.Student.Guardian?.WhatsAppNumber))
                    {
                        var notificationSent = await whatsAppService.SendAttendanceNotificationAsync(
                            record.Student.Guardian.WhatsAppNumber,
                            record.Student.FullNameArabic,
                            record.Status,
                            record.Date
                        );

                        if (notificationSent)
                        {
                            record.WhatsAppNotificationSent = true;
                            record.WhatsAppNotificationSentAt = DateTime.UtcNow;
                            _logger.LogInformation("WhatsApp notification sent for attendance record {RecordId}", record.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending WhatsApp notification for attendance record {RecordId}", record.Id);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Processed {Count} attendance notifications", pendingNotifications.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing attendance records");
        }
    }

    private async Task ProcessFingerprintDataAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasarDbContext>();

        try
        {
            // In a real implementation, this would connect to fingerprint devices
            // For now, we'll simulate processing fingerprint data
            
            _logger.LogInformation("Processing fingerprint data from devices...");

            // Simulate checking for new fingerprint data
            // In production, this would:
            // 1. Connect to fingerprint device APIs
            // 2. Download new fingerprint records
            // 3. Match fingerprints to students/employees
            // 4. Create attendance records automatically
            
            var today = DateTime.UtcNow.Date;
            var studentsWithoutAttendance = await dbContext.Students
                .Where(s => s.IsActive && 
                           !dbContext.AttendanceRecords
                               .Any(a => a.StudentId == s.Id && a.Date == today))
                .Include(s => s.ClassRoom)
                .Include(s => s.Guardian)
                .ToListAsync(stoppingToken);

            _logger.LogInformation("Found {Count} students without attendance records for today", studentsWithoutAttendance.Count);

            // Note: In production, this would be triggered by actual fingerprint device events
            // This is just a placeholder for the automated processing logic
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing fingerprint data");
        }
    }

    private async Task CheckAbsentStudentsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasarDbContext>();
        var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();

        try
        {
            var today = DateTime.UtcNow.Date;
            var currentTime = DateTime.UtcNow.TimeOfDay;

            // Check for students who haven't checked in by a certain time (e.g., 8:30 AM)
            var checkInThreshold = TimeSpan.FromHours(8) + TimeSpan.FromMinutes(30);

            if (currentTime >= checkInThreshold)
            {
                var absentStudents = await dbContext.AttendanceRecords
                    .Include(a => a.Student)
                    .ThenInclude(s => s.Guardian)
                    .Where(a => a.Date == today && 
                               a.Status == "Absent" && 
                               !a.WhatsAppNotificationSent)
                    .ToListAsync(stoppingToken);

                foreach (var record in absentStudents)
                {
                    try
                    {
                        if (record.Student.Guardian != null && !string.IsNullOrEmpty(record.Student.Guardian.WhatsAppNumber))
                        {
                            var notificationSent = await whatsAppService.SendAttendanceNotificationAsync(
                                record.Student.Guardian.WhatsAppNumber,
                                record.Student.FullNameArabic,
                                record.Status,
                                record.Date
                            );

                            if (notificationSent)
                            {
                                record.WhatsAppNotificationSent = true;
                                record.WhatsAppNotificationSentAt = DateTime.UtcNow;
                                _logger.LogInformation("WhatsApp notification sent for absent student {StudentName}", 
                                    record.Student.FullNameArabic);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending WhatsApp notification for absent student {StudentId}", record.StudentId);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Processed {Count} absent student notifications", absentStudents.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking absent students");
        }
    }

    public async Task<bool> ProcessFingerprintScanAsync(string deviceId, string fingerprintTemplate, DateTime scanTime)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasarDbContext>();

        try
        {
            // Find student/employee by fingerprint template
            var student = await dbContext.Students
                .Include(s => s.ClassRoom)
                .Include(s => s.Guardian)
                .FirstOrDefaultAsync(s => s.FingerprintTemplate == fingerprintTemplate);

            if (student != null)
            {
                return await CreateAttendanceRecordAsync(student.Id, scanTime, deviceId, "Student");
            }

            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(e => e.FingerprintTemplate == fingerprintTemplate);

            if (employee != null)
            {
                return await CreateAttendanceRecordAsync(employee.Id, scanTime, deviceId, "Employee");
            }

            _logger.LogWarning("No matching student or employee found for fingerprint template from device {DeviceId}", deviceId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing fingerprint scan from device {DeviceId}", deviceId);
            return false;
        }
    }

    private async Task<bool> CreateAttendanceRecordAsync(Guid personId, DateTime scanTime, string deviceId, string personType)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MasarDbContext>();

        try
        {
            var today = scanTime.Date;
            var existingRecord = await dbContext.AttendanceRecords
                .FirstOrDefaultAsync(a => a.Date == today && 
                                       (personType == "Student" ? a.StudentId == personId : a.EmployeeId == personId));

            if (existingRecord != null)
            {
                // Update check-out time if already checked in
                if (existingRecord.CheckInTime.HasValue && !existingRecord.CheckOutTime.HasValue)
                {
                    existingRecord.CheckOutTime = scanTime;
                    existingRecord.DeviceId = deviceId;
                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Updated check-out time for {PersonType} {PersonId}", personType, personId);
                    return true;
                }
                return false;
            }

            // Create new attendance record
            var record = new AttendanceRecord
            {
                Id = Guid.NewGuid(),
                Date = today,
                CheckInTime = scanTime,
                Status = DetermineAttendanceStatus(scanTime),
                DeviceId = deviceId,
                IsAutomated = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "FingerprintDevice"
            };

            if (personType == "Student")
            {
                record.StudentId = personId;
                var student = await dbContext.Students.FindAsync(personId);
                if (student != null)
                {
                    record.ClassRoomId = student.ClassRoomId ?? Guid.Empty;
                }
            }
            else
            {
                record.EmployeeId = personId;
            }

            dbContext.AttendanceRecords.Add(record);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Created attendance record for {PersonType} {PersonId} at {ScanTime}", 
                personType, personId, scanTime);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating attendance record for {PersonType} {PersonId}", personType, personId);
            return false;
        }
    }

    private string DetermineAttendanceStatus(DateTime checkInTime)
    {
        var schoolStartTime = new TimeSpan(8, 0, 0); // 8:00 AM
        var lateThreshold = new TimeSpan(8, 30, 0); // 8:30 AM

        if (checkInTime.TimeOfDay <= schoolStartTime)
        {
            return "Present";
        }
        else if (checkInTime.TimeOfDay <= lateThreshold)
        {
            return "Late";
        }
        else
        {
            return "Absent";
        }
    }
}

public interface IAttendanceAutomationService
{
    Task<bool> ProcessFingerprintScanAsync(string deviceId, string fingerprintTemplate, DateTime scanTime);
}