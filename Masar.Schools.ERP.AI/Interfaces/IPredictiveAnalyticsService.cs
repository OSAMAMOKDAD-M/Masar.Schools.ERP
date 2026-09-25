using Masar.Schools.ERP.AI.DTOs;

namespace Masar.Schools.ERP.AI.Interfaces;

/// <summary>
/// واجهة خدمة التحليلات التنبؤية
/// </summary>
public interface IPredictiveAnalyticsService
{
    /// <summary>
    /// الحصول على لوحة التحكم بالإنذار المبكر
    /// </summary>
    Task<EarlyWarningDashboardDto> GetDashboardAsync(
        RiskMatrixFilterDto? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// الحصول على مصفوفة المخاطر
    /// </summary>
    Task<List<StudentRiskMatrixDto>> GetRiskMatrixAsync(
        RiskMatrixFilterDto filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// الحصول على تفاصيل خطر طالب معين
    /// </summary>
    Task<StudentRiskScoreDto> GetStudentRiskDetailsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// التنبؤ بالتحصيل المالي الشهري
    /// </summary>
    Task<RevenuePredictionDto> PredictMonthlyRevenueAsync(
        DateTime month,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// الحصول على تاريخ خطر طالب
    /// </summary>
    Task<List<RiskScoreSnapshot>> GetStudentRiskHistoryAsync(
        Guid studentId,
        int daysToLookBack = 90,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تحليل اتجاه خطر طالب
    /// </summary>
    Task<string> AnalyzeRiskTrendAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}
