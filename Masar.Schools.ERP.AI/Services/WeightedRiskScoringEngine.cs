using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Masar.Schools.ERP.AI.Services;

/// <summary>
/// محرك حساب المخاطر المرجح - يستخدم أوزان ثابتة قابلة للتهيئة
/// </summary>
public class WeightedRiskScoringEngine : IRiskScoringEngine
{
    private readonly MasarDbContext _context;
    private readonly MasarAIOptions _options;
    private readonly ILogger<WeightedRiskScoringEngine> _logger;

    public WeightedRiskScoringEngine(
        MasarDbContext context,
        IOptions<MasarAIOptions> options,
        ILogger<WeightedRiskScoringEngine> logger)
    {
        _context = context;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<StudentRiskScoreDto> CalculateStudentRiskAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating risk score for student {StudentId}", studentId);

        // التحقق من وجود الطالب
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student == null)
        {
            throw new ArgumentException($"Student with ID {studentId} not found");
        }

        // حساب مخاطر كل عامل
        var attendanceRisk = await CalculateAttendanceRiskAsync(
            studentId,
            _options.AnalysisSettings.AttendanceDaysToAnalyze,
            cancellationToken);

        var academicRisk = await CalculateAcademicRiskAsync(
            studentId,
            _options.AnalysisSettings.AcademicPeriodDays,
            cancellationToken);

        var financialRisk = await CalculateFinancialRiskAsync(
            studentId,
            _options.AnalysisSettings.FinancialPeriodDays,
            cancellationToken);

        // التحقق من توفر البيانات
        var hasAttendanceData = attendanceRisk >= 0;
        var hasAcademicData = academicRisk >= 0;
        var hasFinancialData = financialRisk >= 0;

        // إعادة توزيع الأوزان إذا كانت بيانات مفقودة
        var (attendanceWeight, academicWeight, financialWeight) = AdjustWeightsForMissingData(
            hasAttendanceData,
            hasAcademicData,
            hasFinancialData,
            (decimal)_options.RiskWeights.Attendance,
            (decimal)_options.RiskWeights.Academic,
            (decimal)_options.RiskWeights.Financial);

        // حساب الدرجة النهائية
        var finalScore = 0m;
        var dataCompleteness = 0m;
        var availableWeights = 0m;

        if (hasAttendanceData)
        {
            finalScore += attendanceRisk * attendanceWeight;
            availableWeights += attendanceWeight;
            dataCompleteness += attendanceWeight;
        }

        if (hasAcademicData)
        {
            finalScore += academicRisk * academicWeight;
            availableWeights += academicWeight;
            dataCompleteness += academicWeight;
        }

        if (hasFinancialData)
        {
            finalScore += financialRisk * financialWeight;
            availableWeights += financialWeight;
            dataCompleteness += financialWeight;
        }

        // تطبيع النتيجة إذا كانت الأوزان غير مكتملة
        if (availableWeights > 0 && availableWeights < 1m)
        {
            finalScore = finalScore / availableWeights;
        }

        // Clamp بين 0 و 100
        finalScore = Math.Clamp(finalScore, 0, 100);

        // تحديد مستوى الخطر
        var riskLevel = DetermineRiskLevel(finalScore);

        // إنشاء عوامل الخطر والتوصيات
        var riskFactors = new List<RiskFactorDto>();
        var recommendations = new List<string>();

        if (hasAttendanceData && attendanceRisk > 50)
        {
            riskFactors.Add(new RiskFactorDto
            {
                Name = "Attendance",
                NameArabic = "الحضور",
                Score = attendanceRisk,
                Impact = attendanceRisk > 70 ? "High" : "Medium"
            });
            recommendations.Add("متابعة الحضور والغياب");
        }

        if (hasAcademicData && academicRisk > 50)
        {
            riskFactors.Add(new RiskFactorDto
            {
                Name = "Academic",
                NameArabic = "الأداء الأكاديمي",
                Score = academicRisk,
                Impact = academicRisk > 70 ? "High" : "Medium"
            });
            recommendations.Add("متابعة أكاديمية");
        }

        if (hasFinancialData && financialRisk > 50)
        {
            riskFactors.Add(new RiskFactorDto
            {
                Name = "Financial",
                NameArabic = "الحالة المالية",
                Score = financialRisk,
                Impact = financialRisk > 70 ? "High" : "Medium"
            });
            recommendations.Add("متابعة الحالة المالية");
        }

        if (riskLevel == "Critical")
        {
            recommendations.Add("تدخل شامل من المشرف");
        }

        // ترتيب العوامل حسب الأهمية
        riskFactors = riskFactors.OrderByDescending(f => f.Score).ToList();

        return new StudentRiskScoreDto
        {
            StudentId = studentId,
            StudentNumber = student.StudentNumber ?? string.Empty,
            StudentNameArabic = student.FullNameArabic,
            StudentName = student.FullName,
            ClassName = student.ClassRoom?.NameArabic,
            RiskScore = finalScore,
            RiskLevel = riskLevel,
            AttendanceRisk = hasAttendanceData ? attendanceRisk : 0,
            AcademicRisk = hasAcademicData ? academicRisk : 0,
            FinancialRisk = hasFinancialData ? financialRisk : 0,
            AttendanceWeight = attendanceWeight,
            AcademicWeight = academicWeight,
            FinancialWeight = financialWeight,
            DataCompleteness = dataCompleteness,
            PrimaryRiskFactors = riskFactors,
            Recommendations = recommendations,
            CalculatedAt = DateTime.UtcNow
        };
    }

    public async Task<List<StudentRiskScoreDto>> CalculateBatchRiskAsync(
        List<Guid> studentIds,
        CancellationToken cancellationToken = default)
    {
        var results = new List<StudentRiskScoreDto>();

        foreach (var studentId in studentIds)
        {
            try
            {
                var riskScore = await CalculateStudentRiskAsync(studentId, cancellationToken);
                results.Add(riskScore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate risk for student {StudentId}", studentId);
            }
        }

        return results;
    }

    public async Task<decimal> CalculateAttendanceRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.Today.AddDays(-daysToAnalyze);
        var endDate = DateTime.Today;

        var attendanceRecords = await _context.AttendanceRecords
            .AsNoTracking()
            .Where(a => a.StudentId == studentId &&
                       a.Date >= startDate &&
                       a.Date <= endDate)
            .ToListAsync(cancellationToken);

        if (!attendanceRecords.Any())
        {
            return -1; // لا توجد بيانات
        }

        var totalDays = attendanceRecords.Count;
        var absentDays = attendanceRecords.Count(a => a.Status == "Absent");
        var lateDays = attendanceRecords.Count(a => a.Status == "Late");
        var presentDays = attendanceRecords.Count(a => a.Status == "Present");

        if (totalDays == 0) return -1;

        // حساب نسبة الحضور
        var attendanceRate = (decimal)presentDays / totalDays * 100;

        // حساب نسبة الغياب
        var absenceRate = (decimal)absentDays / totalDays * 100;

        // حساب نسبة التأخير
        var lateRate = (decimal)lateDays / totalDays * 100;

        // تحويل إلى درجة خطر (0-100)
        // كلما قلت نسبة الحضور، زادت درجة الخطر
        var riskScore = 0m;

        if (attendanceRate >= 95)
        {
            riskScore = 0;
        }
        else if (attendanceRate >= 90)
        {
            riskScore = 20;
        }
        else if (attendanceRate >= 80)
        {
            riskScore = 40;
        }
        else if (attendanceRate >= 70)
        {
            riskScore = 60;
        }
        else if (attendanceRate >= 60)
        {
            riskScore = 80;
        }
        else
        {
            riskScore = 100;
        }

        // إضافة وزن للتأخير المتكرر
        if (lateRate > 10)
        {
            riskScore += 10;
        }

        return Math.Clamp(riskScore, 0, 100);
    }

    public async Task<decimal> CalculateAcademicRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.Today.AddDays(-daysToAnalyze);

        var grades = await _context.Grades
            .AsNoTracking()
            .Include(g => g.Subject)
            .Where(g => g.StudentId == studentId &&
                       g.CreatedAt >= startDate)
            .ToListAsync(cancellationToken);

        if (!grades.Any())
        {
            return -1; // لا توجد بيانات
        }

        // حساب متوسط الدرجات
        var averageScore = grades.Average(g => g.TotalScore ?? 0);

        // حساب الانحراف المعياري (لقياس الاستقرار)
        var scores = grades.Select(g => g.TotalScore ?? 0).ToList();
        var variance = scores.Any() ? scores.Average(s => Math.Pow((double)(s - averageScore), 2)) : 0;
        var stdDev = Math.Sqrt(variance);

        // حساب الاتجاه (مقارنة بالأول والأخير)
        if (grades.Count >= 2)
        {
            var firstScore = grades.First().TotalScore ?? 0;
            var lastScore = grades.Last().TotalScore ?? 0;
            var trend = lastScore - firstScore;

            // تحويل إلى درجة خطر
            var riskScore = 0m;

            if (averageScore >= 90)
            {
                riskScore = 0;
            }
            else if (averageScore >= 80)
            {
                riskScore = 20;
            }
            else if (averageScore >= 70)
            {
                riskScore = 40;
            }
            else if (averageScore >= 60)
            {
                riskScore = 60;
            }
            else if (averageScore >= 50)
            {
                riskScore = 80;
            }
            else
            {
                riskScore = 100;
            }

            // تعديل حسب الاتجاه
            if (trend < -10) // تراجع ملحوظ
            {
                riskScore += 20;
            }
            else if (trend < -5) // تراجع بسيط
            {
                riskScore += 10;
            }
            else if (trend > 5) // تحسن
            {
                riskScore -= 10;
            }

            // تعديل حسب الاستقرار
            if (stdDev > 20) // عدم استقرار
            {
                riskScore += 15;
            }

            return Math.Clamp(riskScore, 0, 100);
        }

        return -1;
    }

    public async Task<decimal> CalculateFinancialRiskAsync(
        Guid studentId,
        int daysToAnalyze,
        CancellationToken cancellationToken = default)
    {
        var studentAccount = await _context.StudentAccounts
            .AsNoTracking()
            .Include(sa => sa.Student)
            .FirstOrDefaultAsync(sa => sa.StudentId == studentId, cancellationToken);

        if (studentAccount == null)
        {
            return -1; // لا توجد بيانات
        }

        var outstandingBalance = studentAccount.OutstandingBalance;
        var totalBalance = studentAccount.TotalBalance;

        if (totalBalance == 0)
        {
            return 0; // لا يوجد التزامات مالية
        }

        // حساب نسبة المتأخر
        var overduePercentage = (decimal)outstandingBalance / totalBalance * 100;

        // حساب درجة الخطر
        var riskScore = 0m;

        if (overduePercentage == 0)
        {
            riskScore = 0;
        }
        else if (overduePercentage <= 10)
        {
            riskScore = 20;
        }
        else if (overduePercentage <= 25)
        {
            riskScore = 40;
        }
        else if (overduePercentage <= 50)
        {
            riskScore = 60;
        }
        else if (overduePercentage <= 75)
        {
            riskScore = 80;
        }
        else
        {
            riskScore = 100;
        }

        // التحقق من سجل المدفوعات
        var recentPayments = await _context.StudentPayments
            .AsNoTracking()
            .Where(sp => sp.StudentAccountId == studentAccount.AccountId &&
                       sp.PaymentDate >= DateTime.Today.AddDays(-daysToAnalyze))
            .ToListAsync(cancellationToken);

        if (recentPayments.Any())
        {
            // إذا كان هناك مدفوعات حديثة، قلل درجة الخطر
            riskScore -= 10;
        }

        return Math.Clamp(riskScore, 0, 100);
    }

    public (decimal attendanceWeight, decimal academicWeight, decimal financialWeight) AdjustWeightsForMissingData(
        bool hasAttendanceData,
        bool hasAcademicData,
        bool hasFinancialData,
        decimal originalAttendanceWeight,
        decimal originalAcademicWeight,
        decimal originalFinancialWeight)
    {
        var availableWeights = 0m;

        if (hasAttendanceData) availableWeights += originalAttendanceWeight;
        if (hasAcademicData) availableWeights += originalAcademicWeight;
        if (hasFinancialData) availableWeights += originalFinancialWeight;

        if (availableWeights == 0)
        {
            // لا توجد بيانات - توزيع متساوي افتراضي
            return (0.33m, 0.33m, 0.34m);
        }

        if (availableWeights == 1)
        {
            // جميع البيانات متاحة - لا تغيير
            return (originalAttendanceWeight, originalAcademicWeight, originalFinancialWeight);
        }

        // إعادة توزيع الأوزان
        var attendanceWeight = hasAttendanceData ? originalAttendanceWeight / availableWeights : 0;
        var academicWeight = hasAcademicData ? originalAcademicWeight / availableWeights : 0;
        var financialWeight = hasFinancialData ? originalFinancialWeight / availableWeights : 0;

        return (attendanceWeight, academicWeight, financialWeight);
    }

    private string DetermineRiskLevel(decimal riskScore)
    {
        if (riskScore <= _options.RiskThresholds.SafeMaximum)
            return "Safe";

        if (riskScore <= _options.RiskThresholds.WarningMaximum)
            return "Warning";

        return "Critical";
    }
}
