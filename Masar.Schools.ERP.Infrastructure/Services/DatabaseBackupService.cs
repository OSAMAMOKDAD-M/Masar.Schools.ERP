using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using System.Data.SqlClient;

namespace Masar.Schools.ERP.Infrastructure.Services;

public interface IDatabaseBackupService
{
    Task<BackupRecord> CreateBackupAsync(BackupType type, string? description = null, bool isAutomated = false);
    Task<bool> RestoreBackupAsync(Guid backupRecordId);
    Task<List<BackupRecord>> GetBackupHistoryAsync(int count = 50);
    Task<bool> DeleteBackupAsync(Guid backupRecordId);
}

public class DatabaseBackupService : IDatabaseBackupService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<DatabaseBackupService> _logger;
    private readonly IConfiguration _configuration;

    public DatabaseBackupService(
        MasarDbContext context,
        ILogger<DatabaseBackupService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    private async Task<string> GetBackupPathAsync()
    {
        var settings = await _context.BackupSettings.FirstOrDefaultAsync();
        var configuredPath = settings?.DatabaseBackupPath ?? _configuration["Backup:DatabasePath"] ?? "Backups/Database";

        _logger.LogInformation("Database backup path from settings: {DatabaseBackupPath}, from config: {ConfigPath}, final: {FinalPath}",
            settings?.DatabaseBackupPath,
            _configuration["Backup:DatabasePath"],
            configuredPath);

        // Convert relative path to absolute
        if (!Path.IsPathRooted(configuredPath))
        {
            configuredPath = Path.Combine(Environment.CurrentDirectory, configuredPath);
            _logger.LogInformation("Converted relative path to absolute: {AbsolutePath}", configuredPath);
        }

        if (!Directory.Exists(configuredPath))
        {
            Directory.CreateDirectory(configuredPath);
            _logger.LogInformation("Created backup directory: {BackupPath}", configuredPath);
        }

        return configuredPath;
    }

    public async Task<BackupRecord> CreateBackupAsync(BackupType type, string? description = null, bool isAutomated = false)
    {
        var backupRecord = new BackupRecord
        {
            Id = Guid.NewGuid(),
            Type = type,
            Status = BackupStatus.InProgress,
            Description = description,
            IsAutomated = isAutomated,
            CreatedAt = DateTime.UtcNow,
            ScheduledTime = DateTime.UtcNow
        };

        try
        {
            _context.BackupRecords.Add(backupRecord);
            await _context.SaveChangesAsync();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var serverName = builder.DataSource;
            var backupPath = await GetBackupPathAsync();

            _logger.LogInformation("Starting database backup. Database: {DatabaseName}, Server: {ServerName}, BackupPath: {BackupPath}",
                databaseName, serverName, backupPath);

            // Ensure backup directory exists
            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
                _logger.LogInformation("Created backup directory: {BackupPath}", backupPath);
            }

            var fileName = $"{databaseName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak";
            var filePath = Path.Combine(backupPath, fileName);

            // Use a temporary path that SQL Server can access
            var tempPath = Path.GetTempPath();
            var tempFilePath = Path.Combine(tempPath, fileName);

            _logger.LogInformation("Temporary backup path: {TempFilePath}", tempFilePath);

            var backupQuery = $@"
                BACKUP DATABASE [{databaseName}]
                TO DISK = '{tempFilePath}'
                WITH FORMAT,
                MEDIANAME = 'MasarSchoolsBackup',
                NAME = 'Full Backup of {databaseName}';";

            _logger.LogInformation("Executing SQL backup command...");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(backupQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            _logger.LogInformation("SQL backup command executed successfully");

            // Copy file from temp location to backup directory
            if (File.Exists(tempFilePath))
            {
                _logger.LogInformation("Backup file found at temp location. Copying to: {FilePath}", filePath);
                File.Copy(tempFilePath, filePath, true);
                File.Delete(tempFilePath);
                backupRecord.FileSizeBytes = new FileInfo(filePath).Length;
                _logger.LogInformation("Backup file copied successfully. Size: {Size} bytes", backupRecord.FileSizeBytes);
            }
            else
            {
                throw new Exception("Backup file was not created by SQL Server at temp location");
            }

            backupRecord.Status = BackupStatus.Completed;
            backupRecord.FilePath = filePath;
            backupRecord.StorageLocation = "Local";
            backupRecord.CompletedTime = DateTime.UtcNow;

            _logger.LogInformation("Database backup created successfully: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            backupRecord.Status = BackupStatus.Failed;
            backupRecord.ErrorMessage = ex.Message;
            backupRecord.CompletedTime = DateTime.UtcNow;
            _logger.LogError(ex, "Failed to create database backup. Error: {ErrorMessage}", ex.Message);
        }

        await _context.SaveChangesAsync();
        return backupRecord;
    }

    public async Task<bool> RestoreBackupAsync(Guid backupRecordId)
    {
        var backupRecord = await _context.BackupRecords.FindAsync(backupRecordId);
        if (backupRecord == null || backupRecord.FilePath == null)
        {
            _logger.LogError("Backup record not found or file path is null: {BackupRecordId}", backupRecordId);
            return false;
        }

        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;

            // Copy backup file to temp directory for SQL Server access
            var tempPath = Path.GetTempPath();
            var tempFileName = Path.GetFileName(backupRecord.FilePath);
            var tempFilePath = Path.Combine(tempPath, tempFileName);
            
            if (File.Exists(backupRecord.FilePath))
            {
                File.Copy(backupRecord.FilePath, tempFilePath, true);
            }
            else
            {
                throw new Exception("Backup file not found");
            }

            var restoreQuery = $@"
                ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{databaseName}] FROM DISK = '{tempFilePath}' WITH REPLACE;
                ALTER DATABASE [{databaseName}] SET MULTI_USER;";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(restoreQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            // Clean up temp file
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            _logger.LogInformation("Database restored successfully from: {FilePath}", backupRecord.FilePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore database from backup: {FilePath}", backupRecord.FilePath);
            return false;
        }
    }

    public async Task<List<BackupRecord>> GetBackupHistoryAsync(int count = 50)
    {
        var backupPath = await GetBackupPathAsync();
        _logger.LogInformation("Getting database backup history from: {BackupPath}", backupPath);

        var backupRecords = new List<BackupRecord>();

        if (Directory.Exists(backupPath))
        {
            var files = Directory.GetFiles(backupPath, "*.bak")
                .OrderByDescending(f => File.GetCreationTime(f))
                .Take(count);

            _logger.LogInformation("Found {FileCount} database backup files", files.Count());

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                backupRecords.Add(new BackupRecord
                {
                    Id = Guid.NewGuid(),
                    Type = BackupType.Database,
                    Status = BackupStatus.Completed,
                    FilePath = file,
                    FileSizeBytes = fileInfo.Length,
                    StorageLocation = "Local",
                    CreatedAt = fileInfo.CreationTimeUtc,
                    CompletedTime = fileInfo.CreationTimeUtc
                });
            }
        }
        else
        {
            _logger.LogWarning("Backup directory does not exist: {BackupPath}", backupPath);
        }

        return await Task.FromResult(backupRecords);
    }

    public async Task<bool> DeleteBackupAsync(Guid backupRecordId)
    {
        var backupRecord = await _context.BackupRecords.FindAsync(backupRecordId);
        if (backupRecord == null)
        {
            return false;
        }

        try
        {
            if (!string.IsNullOrEmpty(backupRecord.FilePath) && File.Exists(backupRecord.FilePath))
            {
                File.Delete(backupRecord.FilePath);
            }

            _context.BackupRecords.Remove(backupRecord);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Backup deleted successfully: {BackupRecordId}", backupRecordId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete backup: {BackupRecordId}", backupRecordId);
            return false;
        }
    }
}
