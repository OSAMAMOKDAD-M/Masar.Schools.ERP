using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using System.IO.Compression;

namespace Masar.Schools.ERP.Infrastructure.Services;

public interface IFileBackupService
{
    Task<BackupRecord> CreateFilesBackupAsync(string? description = null, bool isAutomated = false);
    Task<bool> RestoreFilesBackupAsync(Guid backupRecordId);
    Task<List<BackupRecord>> GetFileBackupHistoryAsync(int count = 50);
}

public class FileBackupService : IFileBackupService
{
    private readonly ILogger<FileBackupService> _logger;
    private readonly IConfiguration _configuration;
    private readonly MasarDbContext _context;

    public FileBackupService(
        ILogger<FileBackupService> logger,
        IConfiguration configuration,
        MasarDbContext context)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
    }

    private async Task<string> GetBackupPathAsync()
    {
        var settings = await _context.BackupSettings.FirstOrDefaultAsync();
        var configuredPath = settings?.FilesBackupPath ?? _configuration["Backup:FilesPath"] ?? "Backups/Files";

        _logger.LogInformation("Files backup path from settings: {FilesBackupPath}, from config: {ConfigPath}, final: {FinalPath}",
            settings?.FilesBackupPath,
            _configuration["Backup:FilesPath"],
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

    private string GetFilesPath()
    {
        var configuredPath = _configuration["Files:Path"] ?? Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads");
        
        // Convert relative path to absolute
        if (!Path.IsPathRooted(configuredPath))
        {
            configuredPath = Path.Combine(Environment.CurrentDirectory, configuredPath);
        }
        
        return configuredPath;
    }

    public async Task<BackupRecord> CreateFilesBackupAsync(string? description = null, bool isAutomated = false)
    {
        var backupRecord = new BackupRecord
        {
            Id = Guid.NewGuid(),
            Type = BackupType.Files,
            Status = BackupStatus.InProgress,
            Description = description,
            IsAutomated = isAutomated,
            CreatedAt = DateTime.UtcNow,
            ScheduledTime = DateTime.UtcNow
        };

        try
        {
            var backupPath = await GetBackupPathAsync();
            var filesPath = GetFilesPath();
            var fileName = $"Files_{DateTime.UtcNow:yyyyMMdd_HHmmss}.zip";
            var filePath = Path.Combine(backupPath, fileName);

            if (Directory.Exists(filesPath))
            {
                ZipFile.CreateFromDirectory(filesPath, filePath, CompressionLevel.Optimal, false);
                backupRecord.FileSizeBytes = new FileInfo(filePath).Length;
            }
            else
            {
                _logger.LogWarning("Files directory does not exist: {FilesPath}", filesPath);
                backupRecord.FileSizeBytes = 0;
            }

            backupRecord.Status = BackupStatus.Completed;
            backupRecord.FilePath = filePath;
            backupRecord.StorageLocation = "Local";
            backupRecord.CompletedTime = DateTime.UtcNow;

            // حفظ السجل في قاعدة البيانات
            _context.BackupRecords.Add(backupRecord);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Files backup created successfully: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            backupRecord.Status = BackupStatus.Failed;
            backupRecord.ErrorMessage = ex.Message;
            backupRecord.CompletedTime = DateTime.UtcNow;

            // حفظ السجل الفاشل في قاعدة البيانات
            _context.BackupRecords.Add(backupRecord);
            await _context.SaveChangesAsync();

            _logger.LogError(ex, "Failed to create files backup");
        }

        return backupRecord;
    }

    public async Task<bool> RestoreFilesBackupAsync(Guid backupRecordId)
    {
        var backupRecord = await GetBackupRecordAsync(backupRecordId);
        if (backupRecord == null || backupRecord.FilePath == null)
        {
            _logger.LogError("Backup record not found or file path is null: {BackupRecordId}", backupRecordId);
            return false;
        }

        try
        {
            var filesPath = GetFilesPath();
            var backupPath = await GetBackupPathAsync();

            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            var tempPath = Path.Combine(backupPath, "temp_" + Guid.NewGuid());
            ZipFile.ExtractToDirectory(backupRecord.FilePath, tempPath);

            foreach (var file in Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories))
            {
                var relativePath = file.Substring(tempPath.Length + 1);
                var destinationPath = Path.Combine(filesPath, relativePath);
                var destinationDir = Path.GetDirectoryName(destinationPath);

                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                File.Copy(file, destinationPath, true);
            }

            Directory.Delete(tempPath, true);

            _logger.LogInformation("Files restored successfully from: {FilePath}", backupRecord.FilePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore files from backup: {FilePath}", backupRecord.FilePath);
            return false;
        }
    }

    public async Task<List<BackupRecord>> GetFileBackupHistoryAsync(int count = 50)
    {
        var backupPath = await GetBackupPathAsync();
        _logger.LogInformation("Getting files backup history from: {BackupPath}", backupPath);

        var backupRecords = new List<BackupRecord>();

        if (Directory.Exists(backupPath))
        {
            var files = Directory.GetFiles(backupPath, "Files_*.zip")
                .OrderByDescending(f => File.GetCreationTime(f))
                .Take(count);

            _logger.LogInformation("Found {FileCount} files backup files", files.Count());

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                backupRecords.Add(new BackupRecord
                {
                    Id = Guid.NewGuid(),
                    Type = BackupType.Files,
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

    private async Task<BackupRecord?> GetBackupRecordAsync(Guid id)
    {
        return await _context.BackupRecords.FindAsync(id);
    }
}
