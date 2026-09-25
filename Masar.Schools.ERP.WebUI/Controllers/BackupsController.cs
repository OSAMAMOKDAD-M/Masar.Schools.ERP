using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Services;
using Masar.Schools.ERP.Infrastructure.Data;
using System;
    using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.WebUI.Controllers;

public class BackupsController : Controller
{
    private readonly IDatabaseBackupService _databaseBackupService;
    private readonly IFileBackupService _fileBackupService;
    private readonly ILogger<BackupsController> _logger;
    private readonly MasarDbContext _context;

    public BackupsController(
        IDatabaseBackupService databaseBackupService,
        IFileBackupService fileBackupService,
        ILogger<BackupsController> logger,
        MasarDbContext context)
    {
        _databaseBackupService = databaseBackupService;
        _fileBackupService = fileBackupService;
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var databaseBackups = await _databaseBackupService.GetBackupHistoryAsync(20);
        var fileBackups = await _fileBackupService.GetFileBackupHistoryAsync(20);

        var viewModel = new BackupListVM
        {
            DatabaseBackups = databaseBackups,
            FileBackups = fileBackups
        };

        // Get backup settings from database
        var settings = await _context.BackupSettings.FirstOrDefaultAsync();
        if (settings != null)
        {
            ViewBag.DatabaseBackupPath = settings.DatabaseBackupPath;
            ViewBag.FilesBackupPath = settings.FilesBackupPath;
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDatabaseBackup(string? description = null)
    {
        try
        {
            var backup = await _databaseBackupService.CreateBackupAsync(BackupType.Database, description, false);
            return Json(new { success = true, message = "تم إنشاء نسخة احتياطية لقاعدة البيانات بنجاح", backup });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating database backup");
            return Json(new { success = false, message = "فشل إنشاء النسخة الاحتياطية: " + ex.Message });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFilesBackup(string? description = null)
    {
        try
        {
            var backup = await _fileBackupService.CreateFilesBackupAsync(description, false);
            return Json(new { success = true, message = "تم إنشاء نسخة احتياطية للملفات بنجاح", backup });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating files backup");
            return Json(new { success = false, message = "فشل إنشاء النسخة الاحتياطية: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RestoreDatabaseBackup(Guid id)
    {
        try
        {
            var result = await _databaseBackupService.RestoreBackupAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "تم استعادة قاعدة البيانات بنجاح" });
            }
            return Json(new { success = false, message = "فشل استعادة قاعدة البيانات" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring database backup");
            return Json(new { success = false, message = "فشل استعادة قاعدة البيانات: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBackup(Guid id)
    {
        try
        {
            var result = await _databaseBackupService.DeleteBackupAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "تم حذف النسخة الاحتياطية بنجاح" });
            }
            return Json(new { success = false, message = "فشل حذف النسخة الاحتياطية" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backup");
            return Json(new { success = false, message = "فشل حذف النسخة الاحتياطية: " + ex.Message });
        }
    }
}

public class BackupListVM
{
    public List<BackupRecord> DatabaseBackups { get; set; } = new();
    public List<BackupRecord> FileBackups { get; set; } = new();
}
