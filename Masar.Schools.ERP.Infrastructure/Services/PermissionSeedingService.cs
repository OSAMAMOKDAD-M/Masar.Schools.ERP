using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة تشغيل الخلفية لمزامنة الصلاحيات عند بدء التطبيق
/// </summary>
public class PermissionSeedingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PermissionSeedingService> _logger;

    public PermissionSeedingService(
        IServiceProvider serviceProvider,
        ILogger<PermissionSeedingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting permission seeding service...");

        using var scope = _serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();

        try
        {
            await seeder.SyncPermissionsAsync(cancellationToken);
            _logger.LogInformation("Permission seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during permission seeding");
            // لا نريد إيقاف التطبيق إذا فشلت مزامنة الصلاحيات
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
