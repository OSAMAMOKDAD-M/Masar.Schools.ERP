using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.WebUI.Controllers;

public class BackupSettingsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<BackupSettingsController> _logger;

    public BackupSettingsController(MasarDbContext context, ILogger<BackupSettingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _context.BackupSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new BackupSettings
            {
                DatabaseBackupPath = "Backups/Database",
                FilesBackupPath = "Backups/Files",
                RetentionDays = 30,
                EnableAutoBackup = true,
                AutoBackupTime = "02:00",
                MaxBackupCount = 50
            };
            _context.BackupSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        return View(settings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update([FromBody] BackupSettings settings)
    {
        _logger.LogInformation("Received backup settings update request: DatabasePath={DatabasePath}, FilesPath={FilesPath}",
            settings?.DatabaseBackupPath, settings?.FilesBackupPath);

        if (settings == null)
        {
            _logger.LogWarning("Settings object is null");
            return Json(new { success = false, message = "البيانات المستلمة فارغة" });
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return Json(new { success = false, message = "البيانات غير صالحة: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)) });
        }

        try
        {
            var existingSettings = await _context.BackupSettings.FirstOrDefaultAsync();
            if (existingSettings != null)
            {
                _logger.LogInformation("Updating existing settings with ID: {SettingsId}", existingSettings.Id);
                existingSettings.DatabaseBackupPath = settings.DatabaseBackupPath;
                existingSettings.FilesBackupPath = settings.FilesBackupPath;
                existingSettings.RetentionDays = settings.RetentionDays;
                existingSettings.EnableAutoBackup = settings.EnableAutoBackup;
                existingSettings.AutoBackupTime = settings.AutoBackupTime;
                existingSettings.MaxBackupCount = settings.MaxBackupCount;
                existingSettings.UpdatedAt = DateTime.UtcNow;
                existingSettings.UpdatedBy = User.Identity?.Name;
            }
            else
            {
                _logger.LogInformation("Creating new backup settings");
                settings.CreatedAt = DateTime.UtcNow;
                settings.CreatedBy = User.Identity?.Name;
                _context.BackupSettings.Add(settings);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Backup settings updated successfully: DatabasePath={DatabasePath}, FilesPath={FilesPath}",
                settings.DatabaseBackupPath, settings.FilesBackupPath);
            return Json(new { success = true, message = "تم تحديث إعدادات النسخ الاحتياطية بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating backup settings");
            return Json(new { success = false, message = "فشل تحديث الإعدادات: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> TestPath(string path)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Json(new { success = false, message = "المسار مطلوب" });
            }

            // Test if directory can be created/accessed
            var testDir = Path.Combine(path, "test");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Test write permission
            var testFile = Path.Combine(path, "test.txt");
            await System.IO.File.WriteAllTextAsync(testFile, "test");
            System.IO.File.Delete(testFile);

            return Json(new { success = true, message = "المسار صالح للاستخدام" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "المسار غير صالح: " + ex.Message });
        }
    }
}
