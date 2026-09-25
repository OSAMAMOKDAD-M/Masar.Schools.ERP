using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Masar.Schools.ERP.AI.Workers;

/// <summary>
/// عامل الخلفية لحساب درجات خطر الطلاب بشكل دوري
/// يعمل مع Hangfire
/// </summary>
public class StudentRiskAnalyzerWorker
{
    private readonly MasarDbContext _context;
    private readonly IRiskScoringEngine _riskScoringEngine;
    private readonly IRiskAlertService _riskAlertService;
    private readonly MasarAIOptions _options;
    private readonly ILogger<StudentRiskAnalyzerWorker> _logger;

    public StudentRiskAnalyzerWorker(
        MasarDbContext context,
        IRiskScoringEngine riskScoringEngine,
        IRiskAlertService riskAlertService,
        IOptions<MasarAIOptions> options,
        ILogger<StudentRiskAnalyzerWorker> logger)
    {
        _context = context;
        _riskScoringEngine = riskScoringEngine;
        _riskAlertService = riskAlertService;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// المهمة الرئيسية للعامل - تحليل جميع الطلاب وحساب درجات الخطر
    /// </summary>
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting student risk analysis job");

        var startTime = DateTime.UtcNow;
        var processedCount = 0;
        var criticalCount = 0;
        var alertsCreated = 0;

        try
        {
            // التحقق من صحة الإعدادات
            if (!_options.Validate())
            {
                _logger.LogError("MasarAI options validation failed. Job aborted.");
                return;
            }

            // جلب جميع الطلاب النشطين
            var students = await _context.Students
                .AsNoTracking()
                .Where(s => s.IsActive && !s.IsDeleted)
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Processing {Count} students", students.Count);

            // تقسيم الطلاب إلى دفعات للمعالجة
            const int batchSize = 50;
            var studentBatches = students
                .Select((id, index) => new { id, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.id).ToList())
                .ToList();

            foreach (var batch in studentBatches)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Job cancelled by user");
                    break;
                }

                try
                {
                    var riskScores = await _riskScoringEngine.CalculateBatchRiskAsync(batch, cancellationToken);

                    foreach (var riskScore in riskScores)
                    {
                        // حفظ النتيجة
                        await SaveRiskScoreAsync(riskScore, cancellationToken);

                        // التحقق من الانتقال إلى Critical
                        if (riskScore.RiskLevel == "Critical")
                        {
                            criticalCount++;

                            // التحقق من آخر نتيجة للطالب
                            var previousScore = await _context.StudentRiskScores
                                .Where(r => r.StudentId == riskScore.StudentId && r.Id != riskScore.StudentId)
                                .OrderByDescending(r => r.CalculatedAt)
                                .FirstOrDefaultAsync(cancellationToken);

                            if (previousScore != null && previousScore.RiskLevel != "Critical")
                            {
                                // إنشاء تنبيه
                                if (_options.NotificationSettings.NotifyOnCriticalTransition)
                                {
                                    if (await _riskAlertService.CanSendAlertAsync(riskScore.StudentId, "CriticalTransition", cancellationToken))
                                    {
                                        var alert = await _riskAlertService.CreateRiskTransitionAlertAsync(
                                            riskScore.StudentId,
                                            previousScore.RiskLevel,
                                            previousScore.RiskScore,
                                            riskScore.RiskLevel,
                                            riskScore.RiskScore,
                                            cancellationToken);

                                        // إرسال التنبيهات
                                        await _riskAlertService.SendSupervisorAlertAsync(alert, cancellationToken);

                                        if (_options.NotificationSettings.NotifyParentOnCritical)
                                        {
                                            await _riskAlertService.SendParentWhatsAppAlertAsync(alert, cancellationToken);
                                        }

                                        alertsCreated++;
                                    }
                                }
                            }
                        }

                        processedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing batch of students");
                }
            }

            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                "Risk analysis completed. Processed: {Processed}, Critical: {Critical}, Alerts: {Alerts}, Duration: {Duration}",
                processedCount, criticalCount, alertsCreated, duration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fatal error in student risk analysis job");
            throw;
        }
    }

    private async Task SaveRiskScoreAsync(StudentRiskScoreDto riskScore, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.StudentRiskScore
        {
            Id = Guid.NewGuid(),
            StudentId = riskScore.StudentId,
            RiskScore = riskScore.RiskScore,
            RiskLevel = riskScore.RiskLevel,
            AttendanceRisk = riskScore.AttendanceRisk,
            AcademicRisk = riskScore.AcademicRisk,
            FinancialRisk = riskScore.FinancialRisk,
            AttendanceWeight = riskScore.AttendanceWeight,
            AcademicWeight = riskScore.AcademicWeight,
            FinancialWeight = riskScore.FinancialWeight,
            DataCompleteness = riskScore.DataCompleteness,
            PrimaryRiskFactors = System.Text.Json.JsonSerializer.Serialize(riskScore.PrimaryRiskFactors),
            Recommendations = System.Text.Json.JsonSerializer.Serialize(riskScore.Recommendations),
            CalculatedAt = riskScore.CalculatedAt,
            CalculatedBy = "MasarAIWorker",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "MasarAIWorker"
        };

        _context.StudentRiskScores.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
