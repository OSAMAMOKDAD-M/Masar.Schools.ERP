using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة التنبيهات للمخزون
/// </summary>
public class InventoryAlertService
{
    private readonly MasarDbContext _context;
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryAlertService> _logger;

    public InventoryAlertService(
        MasarDbContext context,
        IInventoryService inventoryService,
        ILogger<InventoryAlertService> logger)
    {
        _context = context;
        _inventoryService = inventoryService;
        _logger = logger;
    }

    /// <summary>
    /// فحص الأصناف منخفضة المخزون وإرسال التنبيهات
    /// </summary>
    public async Task CheckLowStockAlertsAsync()
    {
        try
        {
            var lowStockItems = await _inventoryService.GetLowStockItemsAsync();

            if (lowStockItems.Any())
            {
                _logger.LogWarning($"تم العثور على {lowStockItems.Count} صنف منخفض المخزون");

                foreach (var item in lowStockItems)
                {
                    await CreateLowStockAlertAsync(item);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking low stock alerts");
        }
    }

    /// <summary>
    /// إنشاء تنبيه للرصيد المنخفض
    /// </summary>
    private async Task CreateLowStockAlertAsync(StockReportDto item)
    {
        try
        {
            // يمكن إضافة منطق لإرسال الإشعارات أو إنشاء سجلات التنبيهات
            _logger.LogWarning($"تنبيه: الصنف {item.ItemName} ({item.SKU}) رصيده منخفض: {item.CurrentStock} - الحد الأدنى: {item.ReorderLevel}");

            // يمكن إضافة إدخال في جدول التنبيهات إذا وجد
            // أو إرسال إشعار للمستخدمين المعنيين
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating low stock alert for item {item.ItemName}");
        }
    }

    /// <summary>
    /// فحص الأصناف الزائدة عن الحد الأقصى
    /// </summary>
    public async Task CheckOverStockAlertsAsync()
    {
        try
        {
            var overStockItems = await _inventoryService.GetOverStockItemsAsync();

            if (overStockItems.Any())
            {
                _logger.LogWarning($"تم العثور على {overStockItems.Count} صنف زائد عن الحد الأقصى");

                foreach (var item in overStockItems)
                {
                    await CreateOverStockAlertAsync(item);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking over stock alerts");
        }
    }

    /// <summary>
    /// إنشاء تنبيه للرصيد الزائد
    /// </summary>
    private async Task CreateOverStockAlertAsync(StockReportDto item)
    {
        try
        {
            _logger.LogWarning($"تنبيه: الصنف {item.ItemName} ({item.SKU}) رصيده زائد: {item.CurrentStock} - الحد الأقصى: {item.MaxStockLevel}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating over stock alert for item {item.ItemName}");
        }
    }

    /// <summary>
    /// الحصول على ملخص التنبيهات الحالية
    /// </summary>
    public async Task<InventoryAlertSummaryDto> GetAlertSummaryAsync()
    {
        try
        {
            var lowStockItems = await _inventoryService.GetLowStockItemsAsync();
            var overStockItems = await _inventoryService.GetOverStockItemsAsync();

            return new InventoryAlertSummaryDto
            {
                LowStockCount = lowStockItems.Count,
                OverStockCount = overStockItems.Count,
                TotalValueAtRisk = lowStockItems.Sum(i => i.TotalValue),
                LowStockItems = lowStockItems.Take(10).ToList(),
                OverStockItems = overStockItems.Take(10).ToList(),
                LastChecked = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting alert summary");
            return new InventoryAlertSummaryDto();
        }
    }
}

/// <summary>
/// DTO لملخص التنبيهات
/// </summary>
public class InventoryAlertSummaryDto
{
    public int LowStockCount { get; set; }
    public int OverStockCount { get; set; }
    public decimal TotalValueAtRisk { get; set; }
    public List<StockReportDto> LowStockItems { get; set; } = new();
    public List<StockReportDto> OverStockItems { get; set; } = new();
    public DateTime LastChecked { get; set; }
}
