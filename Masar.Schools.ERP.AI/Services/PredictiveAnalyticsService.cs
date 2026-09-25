using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Masar.Schools.ERP.AI.Services;

/// <summary>
/// خدمة التحليلات التنبؤية للإنذار المبكر
/// </summary>
public class PredictiveAnalyticsService : IPredictiveAnalyticsService
{
    private readonly MasarDbContext _context;
    private readonly IRiskScoringEngine _riskScoringEngine;
    private readonly IMemoryCache _cache;
    private readonly MasarAIOptions _options;
    private readonly ILogger<PredictiveAnalyticsService> _logger;

    public PredictiveAnalyticsService(
        MasarDbContext context,
        IRiskScoringEngine riskScoringEngine,
        IMemoryCache cache,
        IOptions<MasarAIOptions> options,
        ILogger<PredictiveAnalyticsService> logger)
    {
        _context = context;
        _riskScoringEngine = riskScoringEngine;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<EarlyWarningDashboardDto> GetDashboardAsync(
        RiskMatrixFilterDto? filter = null,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Dashboard_{filter?.ClassRoomId}_{filter?.RiskLevel}_{DateTime.UtcNow:yyyyMMddHH}";
        
        if (_cache.TryGetValue(cacheKey, out EarlyWarningDashboardDto? cached))
        {
            return cached!;
        }

        var dashboard = new EarlyWarningDashboardDto
        {
            LastSynchronizedAt = DateTime.UtcNow
        };

        // جلب جميع الطلاب حسب الفلتر
        var studentsQuery = _context.Students
            .AsNoTracking()
            .Include(s => s.ClassRoom)
            .Where(s => !s.IsDeleted);

        if (filter?.ClassRoomId.HasValue == true)
        {
            studentsQuery = studentsQuery.Where(s => s.ClassRoomId == filter.ClassRoomId.Value);
        }

        var students = await studentsQuery.ToListAsync(cancellationToken);
        dashboard.TotalStudents = students.Count;

        // جلب آخر درجات الخطر
        var studentIds = students.Select(s => s.Id).ToList();
        var riskScores = await _context.StudentRiskScores
            .AsNoTracking()
            .Where(r => studentIds.Contains(r.StudentId))
            .GroupBy(r => r.StudentId)
            .Select(g => g.OrderByDescending(r => r.CalculatedAt).First())
            .ToListAsync(cancellationToken);

        // حساب الإحصائيات
        dashboard.SafeStudentsCount = riskScores.Count(r => r.RiskLevel == "Safe");
        dashboard.WarningStudentsCount = riskScores.Count(r => r.RiskLevel == "Warning");
        dashboard.CriticalStudentsCount = riskScores.Count(r => r.RiskLevel == "Critical");
        dashboard.AtRiskStudentsCount = dashboard.WarningStudentsCount + dashboard.CriticalStudentsCount;

        if (riskScores.Any())
        {
            dashboard.AverageRiskScore = riskScores.Average(r => r.RiskScore);
        }

        // الحصول على مصفوفة المخاطر
        dashboard.RiskMatrix = await GetRiskMatrixAsync(filter ?? new RiskMatrixFilterDto(), cancellationToken);

        // الحصول على التنبؤ بالتحصيل
        dashboard.RevenuePrediction = await PredictMonthlyRevenueAsync(DateTime.UtcNow, cancellationToken);

        // التخزين المؤقت
        _cache.Set(cacheKey, dashboard, TimeSpan.FromMinutes(_options.CachingSettings.DashboardCacheDurationMinutes));

        return dashboard;
    }

    public async Task<List<StudentRiskMatrixDto>> GetRiskMatrixAsync(
        RiskMatrixFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"RiskMatrix_{filter.ClassRoomId}_{filter.RiskLevel}_{filter.GradeLevel}_{DateTime.UtcNow:yyyyMMddHHmm}";
        
        if (_cache.TryGetValue(cacheKey, out List<StudentRiskMatrixDto>? cached))
        {
            return cached!;
        }

        var query = _context.StudentRiskScores
            .AsNoTracking()
            .Include(r => r.Student)
                .ThenInclude(s => s.ClassRoom)
            .AsQueryable();

        if (filter.ClassRoomId.HasValue)
        {
            query = query.Where(r => r.Student.ClassRoomId == filter.ClassRoomId.Value);
        }

        if (!string.IsNullOrEmpty(filter.RiskLevel))
        {
            query = query.Where(r => r.RiskLevel == filter.RiskLevel);
        }

        if (filter.MinRiskScore.HasValue)
        {
            query = query.Where(r => r.RiskScore >= filter.MinRiskScore.Value);
        }

        if (filter.MaxRiskScore.HasValue)
        {
            query = query.Where(r => r.RiskScore <= filter.MaxRiskScore.Value);
        }

        var riskScores = await query
            .OrderByDescending(r => r.RiskScore)
            .ToListAsync(cancellationToken);

        var matrix = riskScores.Select(r => new StudentRiskMatrixDto
        {
            StudentId = r.StudentId,
            StudentNumber = r.Student.StudentNumber ?? string.Empty,
            StudentNameArabic = r.Student.FullNameArabic,
            ClassName = r.Student.ClassRoom?.NameArabic,
            GradeLevel = r.Student.ClassRoom?.GradeLevelLegacy,
            RiskScore = r.RiskScore,
            RiskLevel = r.RiskLevel,
            AttendanceRisk = r.AttendanceRisk,
            AcademicRisk = r.AcademicRisk,
            FinancialRisk = r.FinancialRisk,
            CalculatedAt = r.CalculatedAt
        }).ToList();

        _cache.Set(cacheKey, matrix, TimeSpan.FromMinutes(_options.CachingSettings.RiskMatrixCacheDurationMinutes));

        return matrix;
    }

    public async Task<StudentRiskScoreDto> GetStudentRiskDetailsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await _riskScoringEngine.CalculateStudentRiskAsync(studentId, cancellationToken);
    }

    public async Task<RevenuePredictionDto> PredictMonthlyRevenueAsync(
        DateTime month,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"RevenuePrediction_{month:yyyyMM}";
        
        if (_cache.TryGetValue(cacheKey, out RevenuePredictionDto? cached))
        {
            return cached!;
        }

        var monthStart = new DateTime(month.Year, month.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        // جلب البيانات التاريخية للتحصيل
        var historicalPayments = await _context.StudentPayments
            .AsNoTracking()
            .Where(p => p.PaymentDate >= monthStart.AddMonths(-6) &&
                       p.PaymentDate <= monthEnd)
            .ToListAsync(cancellationToken);

        // جلب الفواتير المستحقة في الشهر
        var invoices = await _context.StudentInvoices
            .AsNoTracking()
            .Where(i => i.IssueDate >= monthStart &&
                       i.IssueDate <= monthEnd)
            .ToListAsync(cancellationToken);

        var totalBilled = invoices.Sum(i => i.TotalAmount);
        var totalCollected = historicalPayments
            .Where(p => p.PaymentDate >= monthStart && p.PaymentDate <= monthEnd)
            .Sum(p => p.Amount);

        // حساب متوسط نسبة التحصيل التاريخي
        var historicalCollectionRates = new List<decimal>();
        for (var i = 1; i <= 6; i++)
        {
            var historicalMonth = monthStart.AddMonths(-i);
            var historicalMonthEnd = historicalMonth.AddMonths(1).AddDays(-1);
            
            var monthBilled = invoices
                .Where(i => i.IssueDate >= historicalMonth && i.IssueDate <= historicalMonthEnd)
                .Sum(i => i.TotalAmount);
            
            var monthCollected = historicalPayments
                .Where(p => p.PaymentDate >= historicalMonth && p.PaymentDate <= historicalMonthEnd)
                .Sum(p => p.Amount);

            if (monthBilled > 0)
            {
                historicalCollectionRates.Add(monthCollected / monthBilled * 100);
            }
        }

        var averageCollectionRate = historicalCollectionRates.Any() 
            ? historicalCollectionRates.Average() 
            : 85m; // افتراضي 85%

        // التنبؤ بالتحصيل
        var predictedCollection = totalBilled * (averageCollectionRate / 100);
        var predictedPercentage = averageCollectionRate;
        var expectedOutstanding = totalBilled - predictedCollection;

        // حساب الثقة بناءً على تباين البيانات التاريخية
        var variance = historicalCollectionRates.Any() 
            ? historicalCollectionRates.Average(r => Math.Pow((double)(r - averageCollectionRate), 2)) 
            : 0;
        var stdDev = Math.Sqrt(variance);
        var confidence = Math.Clamp(1m - ((decimal)stdDev / 100m), 0.5m, 1m);
        var confidenceLevel = confidence > 0.8m ? "High" : confidence > 0.6m ? "Medium" : "Low";

        var prediction = new RevenuePredictionDto
        {
            Month = month,
            PredictedCollection = predictedCollection,
            PredictedCollectionPercentage = predictedPercentage,
            ExpectedOutstanding = expectedOutstanding,
            Confidence = confidence,
            ConfidenceLevel = confidenceLevel,
            TotalStudents = await _context.Students.CountAsync(cancellationToken),
            StudentsWithPayment = await _context.StudentAccounts
                .CountAsync(sa => sa.OutstandingBalance == 0, cancellationToken),
            StudentsWithOutstanding = await _context.StudentAccounts
                .CountAsync(sa => sa.OutstandingBalance > 0, cancellationToken),
            AveragePaymentAmount = historicalPayments.Any() 
                ? historicalPayments.Average(p => p.Amount) 
                : 0
        };

        _cache.Set(cacheKey, prediction, TimeSpan.FromMinutes(_options.CachingSettings.PredictionCacheDurationMinutes));

        return prediction;
    }

    public async Task<List<RiskScoreSnapshot>> GetStudentRiskHistoryAsync(
        Guid studentId,
        int daysToLookBack = 90,
        CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.Today.AddDays(-daysToLookBack);

        var history = await _context.StudentRiskScores
            .AsNoTracking()
            .Where(r => r.StudentId == studentId &&
                       r.CalculatedAt >= startDate)
            .OrderBy(r => r.CalculatedAt)
            .ToListAsync(cancellationToken);

        return history.Select(r => new RiskScoreSnapshot
        {
            Date = r.CalculatedAt,
            Score = r.RiskScore,
            Level = r.RiskLevel
        }).ToList();
    }

    public async Task<string> AnalyzeRiskTrendAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var history = await GetStudentRiskHistoryAsync(studentId, 30, cancellationToken);

        if (history.Count < 2)
        {
            return "InsufficientData";
        }

        var first = history.First();
        var last = history.Last();
        var change = last.Score - first.Score;

        if (change > 10)
        {
            return "Declining";
        }
        else if (change < -10)
        {
            return "Improving";
        }
        else
        {
            return "Stable";
        }
    }
}
