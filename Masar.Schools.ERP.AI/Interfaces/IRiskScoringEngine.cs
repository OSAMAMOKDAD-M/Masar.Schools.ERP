using Masar.Schools.ERP.AI.DTOs;

namespace Masar.Schools.ERP.AI.Interfaces;

/// <summary>
/// واجهة محرك حساب المخاطر
/// </summary>
public interface IRiskScoringEngine
{
    /// <summary>
    /// حساب درجة خطر طالب معين
    /// </summary>
    Task<StudentRiskScoreDto> CalculateStudentRiskAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حساب درجات خطر مجموعة من الطلاب (Batch Processing)
    /// </summary>
    Task<List<StudentRiskScoreDto>> CalculateBatchRiskAsync(
        List<Guid> studentIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حساب درجة خطر الحضور
    /// </summary>
    Task<decimal> CalculateAttendanceRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حساب درجة الخطر الأكاديمي
    /// </summary>
    Task<decimal> CalculateAcademicRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حساب درجة الخطر المالي
    /// </summary>
    Task<decimal> CalculateFinancialRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// إعادة توزيع الأوزان عند عدم توفر بيانات
    /// </summary>
    (decimal attendanceWeight, decimal academicWeight, decimal financialWeight) AdjustWeightsForMissingData(
        bool hasAttendanceData,
        bool hasAcademicData,
        bool hasFinancialData,
        decimal originalAttendanceWeight,
        decimal originalAcademicWeight,
        decimal originalFinancialWeight);
}
