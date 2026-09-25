namespace Masar.Schools.ERP.AI.DTOs;

/// <summary>
/// DTO لمصفوفة مخاطر الطلاب
/// </summary>
public class StudentRiskMatrixDto
{
    public Guid StudentId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string? ClassName { get; set; }
    public string? GradeLevel { get; set; }
    public decimal RiskScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty; // Safe, Warning, Critical
    public decimal AttendanceRisk { get; set; }
    public decimal AcademicRisk { get; set; }
    public decimal FinancialRisk { get; set; }
    public DateTime CalculatedAt { get; set; }
}

/// <summary>
/// DTO لفلاتر مصفوفة المخاطر
/// </summary>
public class RiskMatrixFilterDto
{
    public Guid? ClassRoomId { get; set; }
    public string? GradeLevel { get; set; }
    public string? RiskLevel { get; set; } // Safe, Warning, Critical
    public decimal? MinRiskScore { get; set; }
    public decimal? MaxRiskScore { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
