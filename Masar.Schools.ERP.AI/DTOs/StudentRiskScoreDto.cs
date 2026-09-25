namespace Masar.Schools.ERP.AI.DTOs;

/// <summary>
/// DTO لنتيجة تقييم خطر الطالب
/// </summary>
public class StudentRiskScoreDto
{
    public Guid StudentId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string? ClassName { get; set; }
    public decimal RiskScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty; // Safe, Warning, Critical
    public decimal AttendanceRisk { get; set; }
    public decimal AcademicRisk { get; set; }
    public decimal FinancialRisk { get; set; }
    public decimal DataCompleteness { get; set; }
    public decimal AttendanceWeight { get; set; }
    public decimal AcademicWeight { get; set; }
    public decimal FinancialWeight { get; set; }
    public List<RiskFactorDto> PrimaryRiskFactors { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public DateTime CalculatedAt { get; set; }
    public RiskHistoryDto? RiskHistory { get; set; }
}

/// <summary>
/// DTO لعامل خطر معين
/// </summary>
public class RiskFactorDto
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public string Impact { get; set; } = string.Empty; // High, Medium, Low
}

/// <summary>
/// DTO لتاريخ مستوى الخطر
/// </summary>
public class RiskHistoryDto
{
    public List<RiskScoreSnapshot> Snapshots { get; set; } = new();
    public string Trend { get; set; } = string.Empty; // Improving, Stable, Declining
}

public class RiskScoreSnapshot
{
    public DateTime Date { get; set; }
    public decimal Score { get; set; }
    public string Level { get; set; } = string.Empty; // Safe, Warning, Critical
}
