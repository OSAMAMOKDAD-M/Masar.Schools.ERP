namespace Masar.Schools.ERP.AI.DTOs;

/// <summary>
/// DTO لوحة التحكم بالإنذار المبكر
/// </summary>
public class EarlyWarningDashboardDto
{
    /// <summary>
    /// عدد الطلاب في خطر حرج
    /// </summary>
    public int CriticalStudentsCount { get; set; }

    /// <summary>
    /// عدد الطلاب المعرضين للخطر (Warning + Critical)
    /// </summary>
    public int AtRiskStudentsCount { get; set; }

    /// <summary>
    /// نسبة التحصيل المتوقعة
    /// </summary>
    public decimal PredictedCollectionPercentage { get; set; }

    /// <summary>
    /// عدد التنبيهات المرسلة
    /// </summary>
    public int AlertsSentCount { get; set; }

    /// <summary>
    /// آخر مزامنة
    /// </summary>
    public DateTime LastSynchronizedAt { get; set; }

    /// <summary>
    /// إجمالي الطلاب
    /// </summary>
    public int TotalStudents { get; set; }

    /// <summary>
    /// عدد الطلاب في مستوى آمن
    /// </summary>
    public int SafeStudentsCount { get; set; }

    /// <summary>
    /// عدد الطلاب في مستوى تحذير
    /// </summary>
    public int WarningStudentsCount { get; set; }

    /// <summary>
    /// متوسط درجة الخطر
    /// </summary>
    public decimal AverageRiskScore { get; set; }

    /// <summary>
    /// مصفوفة المخاطر
    /// </summary>
    public List<StudentRiskMatrixDto> RiskMatrix { get; set; } = new();

    /// <summary>
    /// التنبؤ بالتحصيل
    /// </summary>
    public RevenuePredictionDto? RevenuePrediction { get; set; }

    /// <summary>
    /// توزيع مستويات الخطر حسب الصفوف
    /// </summary>
    public List<RiskDistributionByClass> RiskDistributionByClasses { get; set; } = new();
}

public class RiskDistributionByClass
{
    public string ClassName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int SafeCount { get; set; }
    public int WarningCount { get; set; }
    public int CriticalCount { get; set; }
    public decimal AverageRiskScore { get; set; }
}
