using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة تشغيل الخلفية لإنشاء مستخدم الأدمن عند بدء التطبيق
/// </summary>
public class AdminUserSeedingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AdminUserSeedingService> _logger;

    public AdminUserSeedingService(
        IServiceProvider serviceProvider,
        ILogger<AdminUserSeedingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting admin user seeding service...");

        using var scope = _serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<AdminUserSeeder>();

        try
        {
            await seeder.SeedAdminUserAsync();
            _logger.LogInformation("Admin user seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin user seeding");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}