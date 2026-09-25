using Masar.Schools.ERP.AI.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Masar.Schools.ERP.AI.Services;

/// <summary>
/// خدمة اكتشاف الشاشات الجديدة في النظام للتكيف التلقائي
/// </summary>
public class ScreenDiscoveryService : IScreenDiscoveryService
{
    private readonly ILogger<ScreenDiscoveryService> _logger;
    private static readonly List<DiscoveredScreen> _discoveredScreens = new();
    private static DateTime _lastDiscoveryTime = DateTime.MinValue;

    public ScreenDiscoveryService(ILogger<ScreenDiscoveryService> logger)
    {
        _logger = logger;
    }

    public async Task<List<DiscoveredScreen>> DiscoverScreensAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting screen discovery...");

        // في النسخة الحالية، سنقوم بإرجاع قائمة شاشات افتراضية
        // في النسخة الكاملة، سنستخدم Reflection لاكتشاف الـ Controllers الفعلية
        var screens = new List<DiscoveredScreen>
        {
            new DiscoveredScreen
            {
                ControllerName = "Students",
                ActionName = "Index",
                Route = "Students/Index",
                Category = ScreenCategory.Students,
                DiscoveredAt = DateTime.UtcNow,
                IsNew = false,
                RiskFactors = new List<string> { "Attendance", "Academic", "Financial" }
            },
            new DiscoveredScreen
            {
                ControllerName = "Attendance",
                ActionName = "Index",
                Route = "Attendance/Index",
                Category = ScreenCategory.Attendance,
                DiscoveredAt = DateTime.UtcNow,
                IsNew = false,
                RiskFactors = new List<string> { "AttendanceRate", "AbsenceRate", "LateArrivals" }
            },
            new DiscoveredScreen
            {
                ControllerName = "Grades",
                ActionName = "Index",
                Route = "Grades/Index",
                Category = ScreenCategory.Grades,
                DiscoveredAt = DateTime.UtcNow,
                IsNew = false,
                RiskFactors = new List<string> { "AverageScore", "GradeTrend", "SubjectPerformance" }
            }
        };

        _discoveredScreens.Clear();
        _discoveredScreens.AddRange(screens);
        _lastDiscoveryTime = DateTime.UtcNow;

        _logger.LogInformation("Discovered {Count} screens", screens.Count);

        return screens;
    }

    public async Task<List<DiscoveredScreen>> DiscoverNewScreensAsync(
        DateTime since,
        CancellationToken cancellationToken = default)
    {
        if (_lastDiscoveryTime <= since)
        {
            await DiscoverScreensAsync(cancellationToken);
        }

        return _discoveredScreens
            .Where(s => s.DiscoveredAt >= since)
            .ToList();
    }

    public Task<ScreenCategory> AnalyzeScreenCategoryAsync(
        string controllerName,
        string actionName,
        CancellationToken cancellationToken = default)
    {
        var controllerLower = controllerName.ToLower();
        var actionLower = actionName.ToLower();

        if (controllerLower.Contains("student"))
            return Task.FromResult(ScreenCategory.Students);
        
        if (controllerLower.Contains("employee") || controllerLower.Contains("teacher"))
            return Task.FromResult(ScreenCategory.Employees);
        
        if (controllerLower.Contains("attendance"))
            return Task.FromResult(ScreenCategory.Attendance);
        
        if (controllerLower.Contains("grade"))
            return Task.FromResult(ScreenCategory.Grades);
        
        if (controllerLower.Contains("invoice") || controllerLower.Contains("payment") || controllerLower.Contains("account"))
            return Task.FromResult(ScreenCategory.Financial);
        
        if (controllerLower.Contains("guardian"))
            return Task.FromResult(ScreenCategory.Guardians);
        
        if (controllerLower.Contains("setting"))
            return Task.FromResult(ScreenCategory.Settings);

        return Task.FromResult(ScreenCategory.Other);
    }

    public Task<List<string>> SuggestRiskFactorsForScreenAsync(
        string controllerName,
        string actionName,
        CancellationToken cancellationToken = default)
    {
        var factors = new List<string>();
        var controllerLower = controllerName.ToLower();

        if (controllerLower.Contains("student"))
        {
            factors.AddRange(new[] { "Attendance", "Academic", "Financial" });
        }
        else if (controllerLower.Contains("attendance"))
        {
            factors.AddRange(new[] { "AttendanceRate", "AbsenceRate", "LateArrivals" });
        }
        else if (controllerLower.Contains("grade"))
        {
            factors.AddRange(new[] { "AverageScore", "GradeTrend", "SubjectPerformance" });
        }
        else if (controllerLower.Contains("invoice") || controllerLower.Contains("payment"))
        {
            factors.AddRange(new[] { "PaymentHistory", "OverdueAmount", "CollectionRate" });
        }

        return Task.FromResult(factors);
    }
}
