using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// نتيجة تقييم خطر الطالب - يتم حسابها دورياً بواسطة محرك مَسَار الذكي
/// </summary>
public class StudentRiskScore : BaseEntity
{
    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// الطالب المرتبط
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// درجة الخطر النهائية (0-100)
    /// </summary>
    public decimal RiskScore { get; set; }

    /// <summary>
    /// مخاطر الحضور (0-100)
    /// </summary>
    public decimal AttendanceRisk { get; set; }

    /// <summary>
    /// المخاطر الأكاديمية (0-100)
    /// </summary>
    public decimal AcademicRisk { get; set; }

    /// <summary>
    /// المخاطر المالية (0-100)
    /// </summary>
    public decimal FinancialRisk { get; set; }

    /// <summary>
    /// وزن الحضور المستخدم في الحساب
    /// </summary>
    public decimal AttendanceWeight { get; set; }

    /// <summary>
    /// وزن الأداء الأكاديمي المستخدم في الحساب
    /// </summary>
    public decimal AcademicWeight { get; set; }

    /// <summary>
    /// وزن السلوك المالي المستخدم في الحساب
    /// </summary>
    public decimal FinancialWeight { get; set; }

    /// <summary>
    /// مستوى الخطر
    /// </summary>
    public string RiskLevel { get; set; } = string.Empty; // Safe, Warning, Critical

    /// <summary>
    /// إصدار خوارزمية الحساب (للتتبع والتدقيق)
    /// </summary>
    public string CalculationVersion { get; set; } = "1.0";

    /// <summary>
    /// نسبة اكتمال البيانات المستخدمة في الحساب (0-1)
    /// </summary>
    public decimal DataCompleteness { get; set; }

    /// <summary>
    /// العوامل الرئيسية التي ساهمت في مستوى الخطر (JSON)
    /// </summary>
    public string? PrimaryRiskFactors { get; set; }

    /// <summary>
    /// التوصيات المقترحة (JSON)
    /// </summary>
    public string? Recommendations { get; set; }

    /// <summary>
    /// تاريخ الحساب
    /// </summary>
    public DateTime CalculatedAt { get; set; }

    /// <summary>
    /// المستخدم الذي قام بالحساب
    /// </summary>
    public string? CalculatedBy { get; set; }
}
