using Masar.Schools.ERP.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة خلفية لفحص تنبيهات المخزون بشكل دوري
/// </summary>
public class InventoryAlertBackgroundService : BackgroundService
{
    private readonly InventoryAlertService _alertService;
    private readonly ILogger<InventoryAlertBackgroundService> _logger;

    public InventoryAlertBackgroundService(
        InventoryAlertService alertService,
        ILogger<InventoryAlertBackgroundService> logger)
    {
        _alertService = alertService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Inventory Alert Background Service is starting.");

        // فحص التنبيهات كل ساعة
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Running inventory alert check...");

                await _alertService.CheckLowStockAlertsAsync();
                await _alertService.CheckOverStockAlertsAsync();

                _logger.LogInformation("Inventory alert check completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in inventory alert background service");
            }

            // انتظار ساعة قبل الفحص التالي
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }

        _logger.LogInformation("Inventory Alert Background Service is stopping.");
    }
}
