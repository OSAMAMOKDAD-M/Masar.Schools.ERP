using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة تحليلات الحضور والغياب
/// </summary>
public class AttendanceAnalyticsService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<AttendanceAnalyticsService> _logger;

    public AttendanceAnalyticsService(MasarDbContext context, ILogger<AttendanceAnalyticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// تحليل نمط حضور طالب محدد
    /// </summary>
    public async Task<AttendancePatternDto> AnalyzeStudentPattern(Guid studentId, int days = 30)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-days);

        var attendanceRecords = await _context.AttendanceRecords
            .Where(a => a.StudentId == studentId && 
                       a.Date >= startDate && 
                       a.Date <= endDate &&
                       !a.IsDeleted)
            .OrderBy(a => a.Date)
            .ToListAsync();

        var totalDays = attendanceRecords.Count;
        var presentDays = attendanceRecords.Count(a => a.Status == "Present");
        var absentDays = attendanceRecords.Count(a => a.Status == "Absent");
        var lateDays = attendanceRecords.Count(a => a.Status == "Late");
        var excusedDays = attendanceRecords.Count(a => a.Status == "Excused");

        var attendanceRate = totalDays > 0 ? (presentDays / (decimal)totalDays) * 100 : 0;
        var absenceRate = totalDays > 0 ? (absentDays / (decimal)totalDays) * 100 : 0;
        var lateRate = totalDays > 0 ? (lateDays / (decimal)totalDays) * 100 : 0;

        // تحليل الأنماط
        var frequentAbsenceDays = attendanceRecords
            .Where(a => a.Status == "Absent")
            .GroupBy(a => a.Date.DayOfWeek)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Day = g.Key, Count = g.Count() })
            .FirstOrDefault();

        var averageLateMinutes = attendanceRecords
            .Where(a => a.LateMinutes.HasValue)
            .Average(a => a.LateMinutes.Value);

        return new AttendancePatternDto
        {
            StudentId = studentId,
            AnalysisPeriodDays = days,
            TotalDays = totalDays,
            PresentDays = presentDays,
            AbsentDays = absentDays,
            LateDays = lateDays,
            ExcusedDays = excusedDays,
            AttendanceRate = Math.Round(attendanceRate, 2),
            AbsenceRate = Math.Round(absenceRate, 2),
            LateRate = Math.Round(lateRate, 2),
            FrequentAbsenceDay = frequentAbsenceDays?.Day.ToString(),
            AverageLateMinutes = averageLateMinutes,
            RiskLevel = DetermineAttendanceRisk(attendanceRate),
            Recommendations = GenerateAttendanceRecommendations(attendanceRate, absenceRate, lateRate)
        };
    }

    /// <summary>
    /// الحصول على الطلاب المعرضين للخطر بناءً على الحضور
    /// </summary>
    public async Task<List<Student>> GetAtRiskStudents(int threshold = 85)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-30);

        var studentAttendance = await _context.AttendanceRecords
            .Where(a => a.Date >= startDate && 
                       a.Date <= endDate &&
                       !a.IsDeleted)
            .GroupBy(a => a.StudentId)
            .Select(g => new
            {
                StudentId = g.Key,
                TotalDays = g.Count(),
                PresentDays = g.Count(a => a.Status == "Present"),
                AttendanceRate = g.Count() > 0 ? (g.Count(a => a.Status == "Present") / (decimal)g.Count()) * 100 : 0
            })
            .Where(a => a.AttendanceRate < threshold)
            .ToListAsync();

        var studentIds = studentAttendance.Select(a => a.StudentId).ToList();

        return await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .Include(s => s.Guardian)
            .Where(s => studentIds.Contains(s.Id) && !s.IsDeleted)
            .ToListAsync();
    }

    /// <summary>
    /// الحصول على تقرير الحضور الأسبوعي لفصل دراسي
    /// </summary>
    public async Task<WeeklyAttendanceReport> GetWeeklyReport(Guid classRoomId)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-7);

        var attendanceRecords = await _context.AttendanceRecords
            .Where(a => a.ClassRoomId == classRoomId && 
                       a.Date >= startDate && 
                       a.Date <= endDate &&
                       !a.IsDeleted)
            .Include(a => a.Student)
            .ToListAsync();

        var classRoom = await _context.ClassRooms
            .Include(c => c.Branch)
            .FirstOrDefaultAsync(c => c.Id == classRoomId);

        var totalStudents = await _context.Students
            .CountAsync(s => s.ClassRoomId == classRoomId && !s.IsDeleted);

        var weeklyData = attendanceRecords
            .GroupBy(a => a.Date.Date)
            .Select(g => new DailyAttendanceData
            {
                Date = g.Key,
                TotalStudents = g.Count(),
                PresentStudents = g.Count(a => a.Status == "Present"),
                AbsentStudents = g.Count(a => a.Status == "Absent"),
                LateStudents = g.Count(a => a.Status == "Late")
            })
            .OrderBy(d => d.Date)
            .ToList();

        return new WeeklyAttendanceReport
        {
            ClassRoomId = classRoomId,
            ClassRoomName = classRoom?.NameArabic ?? string.Empty,
            BranchName = classRoom?.Branch?.NameArabic ?? string.Empty,
            ReportStartDate = startDate,
            ReportEndDate = endDate,
            TotalStudents = totalStudents,
            WeeklyData = weeklyData,
            AverageAttendanceRate = weeklyData.Any() ? 
                weeklyData.Average(d => d.TotalStudents > 0 ? (d.PresentStudents / (decimal)d.TotalStudents) * 100 : 0) : 0
        };
    }

    /// <summary>
    /// تحليل اتجاه الحضور لطالب محدد
    /// </summary>
    public async Task<AttendanceTrendDto> GetTrendAnalysis(Guid studentId, int months = 6)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddMonths(-months);

        var monthlyData = await _context.AttendanceRecords
            .Where(a => a.StudentId == studentId && 
                       a.Date >= startDate && 
                       a.Date <= endDate &&
                       !a.IsDeleted)
            .GroupBy(a => new { a.Date.Year, a.Date.Month })
            .Select(g => new MonthlyAttendanceData
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalDays = g.Count(),
                PresentDays = g.Count(a => a.Status == "Present"),
                AbsentDays = g.Count(a => a.Status == "Absent"),
                LateDays = g.Count(a => a.Status == "Late")
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToListAsync();

        var trend = CalculateTrend(monthlyData);

        return new AttendanceTrendDto
        {
            StudentId = studentId,
            AnalysisPeriodMonths = months,
            MonthlyData = monthlyData,
            TrendDirection = trend.Direction,
            TrendPercentage = trend.Percentage,
            OverallImprovement = trend.Improvement
        };
    }

    /// <summary>
    /// تحديد مستوى الخطر بناءً على نسبة الحضور
    /// </summary>
    private string DetermineAttendanceRisk(decimal attendanceRate)
    {
        return attendanceRate switch
        {
            >= 95 => "Excellent",
            >= 90 => "Good",
            >= 85 => "Satisfactory",
            >= 75 => "Warning",
            _ => "Critical"
        };
    }

    /// <summary>
    /// توليد توصيات بناءً على بيانات الحضور
    /// </summary>
    private List<string> GenerateAttendanceRecommendations(decimal attendanceRate, decimal absenceRate, decimal lateRate)
    {
        var recommendations = new List<string>();

        if (attendanceRate < 85)
        {
            recommendations.Add("يحتاج الطالب لمتابعة فورية بسبب انخفاض نسبة الحضور");
        }

        if (absenceRate > 10)
        {
            recommendations.Add("يرجى التواصل مع ولي الأمر لمعرفة أسباب الغياب المتكرر");
        }

        if (lateRate > 15)
        {
            recommendations.Add("ينصح بمراجعة أسباب التأخير المتكرر ووضع خطة تحسين");
        }

        if (attendanceRate >= 95)
        {
            recommendations.Add("أداء ممتاز! يُنصح بتشجيع الطالب للحفاظ على هذا المستوى");
        }

        return recommendations;
    }

    /// <summary>
    /// حساب اتجاه الحضور
    /// </summary>
    private (string Direction, decimal Percentage, bool Improvement) CalculateTrend(List<MonthlyAttendanceData> monthlyData)
    {
        if (monthlyData.Count < 2)
        {
            return ("Insufficient Data", 0, false);
        }

        var firstMonth = monthlyData.First();
        var lastMonth = monthlyData.Last();

        var firstRate = firstMonth.TotalDays > 0 ? (firstMonth.PresentDays / (decimal)firstMonth.TotalDays) * 100 : 0;
        var lastRate = lastMonth.TotalDays > 0 ? (lastMonth.PresentDays / (decimal)lastMonth.TotalDays) * 100 : 0;

        var difference = lastRate - firstRate;
        var direction = difference > 0 ? "Improving" : (difference < 0 ? "Declining" : "Stable");
        var improvement = difference > 0;

        return (direction, Math.Abs(difference), improvement);
    }
}

// DTOs
public class AttendancePatternDto
{
    public Guid StudentId { get; set; }
    public int AnalysisPeriodDays { get; set; }
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public int ExcusedDays { get; set; }
    public decimal AttendanceRate { get; set; }
    public decimal AbsenceRate { get; set; }
    public decimal LateRate { get; set; }
    public string? FrequentAbsenceDay { get; set; }
    public double AverageLateMinutes { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = new();
}

public class WeeklyAttendanceReport
{
    public Guid ClassRoomId { get; set; }
    public string ClassRoomName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public DateTime ReportStartDate { get; set; }
    public DateTime ReportEndDate { get; set; }
    public int TotalStudents { get; set; }
    public List<DailyAttendanceData> WeeklyData { get; set; } = new();
    public decimal AverageAttendanceRate { get; set; }
}

public class DailyAttendanceData
{
    public DateTime Date { get; set; }
    public int TotalStudents { get; set; }
    public int PresentStudents { get; set; }
    public int AbsentStudents { get; set; }
    public int LateStudents { get; set; }
}

public class AttendanceTrendDto
{
    public Guid StudentId { get; set; }
    public int AnalysisPeriodMonths { get; set; }
    public List<MonthlyAttendanceData> MonthlyData { get; set; } = new();
    public string TrendDirection { get; set; } = string.Empty;
    public decimal TrendPercentage { get; set; }
    public bool OverallImprovement { get; set; }
}

public class MonthlyAttendanceData
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
}