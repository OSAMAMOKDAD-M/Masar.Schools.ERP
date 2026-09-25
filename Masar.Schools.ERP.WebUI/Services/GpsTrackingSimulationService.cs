using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.WebUI.Services;

/// <summary>
/// Background service for simulating GPS tracking data
/// In production, this would be replaced with actual GPS device integration
/// </summary>
public class GpsTrackingSimulationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GpsTrackingSimulationService> _logger;
    private readonly Random _random = new Random();

    public GpsTrackingSimulationService(
        IServiceProvider serviceProvider,
        ILogger<GpsTrackingSimulationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("GPS Tracking Simulation Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var transportService = scope.ServiceProvider.GetRequiredService<ITransportService>();
                    
                    // Get all active buses
                    var buses = await transportService.GetActiveBusesAsync();
                    
                    foreach (var bus in buses)
                    {
                        // Simulate GPS data
                        var trackingData = await SimulateGpsData(bus.Id);
                        
                        if (trackingData != null)
                        {
                            // Create tracking log
                            await transportService.CreateTrackingLogAsync(trackingData);
                            
                            _logger.LogDebug("Updated GPS data for bus {BusNumber} at {Lat}, {Lng}", 
                                bus.BusNumber, trackingData.Latitude, trackingData.Longitude);
                        }
                    }
                }

                // Wait 10 seconds before next update
                await Task.Delay(10000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GPS tracking simulation");
                await Task.Delay(30000, stoppingToken); // Wait longer on error
            }
        }

        _logger.LogInformation("GPS Tracking Simulation Service stopped");
    }

    private async Task<BusTrackingLogDto?> SimulateGpsData(Guid busId)
    {
        try
        {
            // Simulate movement around Riyadh (24.7136, 46.6753)
            var baseLat = 24.7136;
            var baseLng = 46.6753;
            
            // Add random movement
            var latOffset = (_random.NextDouble() - 0.5) * 0.01; // ±0.005 degrees
            var lngOffset = (_random.NextDouble() - 0.5) * 0.01;
            
            var speed = _random.NextDouble() * 60; // 0-60 km/h
            var direction = _random.NextDouble() * 360; // 0-360 degrees
            var isMoving = speed > 5;
            
            return new BusTrackingLogDto
            {
                BusId = busId,
                Latitude = (decimal)(baseLat + latOffset),
                Longitude = (decimal)(baseLng + lngOffset),
                Speed = (decimal)speed,
                Direction = (decimal)direction,
                Timestamp = DateTime.UtcNow,
                IsMoving = isMoving,
                BatteryLevel = _random.Next(50, 100),
                SignalStrength = _random.Next(60, 100),
                OdometerReading = _random.Next(10000, 50000)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating GPS data for bus {BusId}", busId);
            return null;
        }
    }
}
