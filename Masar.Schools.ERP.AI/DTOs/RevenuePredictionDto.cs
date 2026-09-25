namespace Masar.Schools.ERP.AI.DTOs;

/// <summary>
/// DTO للتنبؤ بالتحصيل المالي
/// </summary>
public class RevenuePredictionDto
{
    public DateTime Month { get; set; }
    public decimal PredictedCollection { get; set; }
    public decimal PredictedCollectionPercentage { get; set; }
    public decimal ExpectedOutstanding { get; set; }
    public decimal Confidence { get; set; } // 0-1
    public string ConfidenceLevel { get; set; } = string.Empty; // High, Medium, Low
    public decimal? ActualCollection { get; set; }
    public decimal? Variance { get; set; }
    public int TotalStudents { get; set; }
    public int StudentsWithPayment { get; set; }
    public int StudentsWithOutstanding { get; set; }
    public decimal AveragePaymentAmount { get; set; }
    public List<MonthlyTrend> HistoricalTrend { get; set; } = new();
}

public class MonthlyTrend
{
    public DateTime Month { get; set; }
    public decimal ActualCollection { get; set; }
    public decimal CollectionPercentage { get; set; }
}
