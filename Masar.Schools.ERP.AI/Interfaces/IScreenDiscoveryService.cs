namespace Masar.Schools.ERP.AI.Interfaces;

/// <summary>
/// واجهة خدمة اكتشاف الشاشات الجديدة في النظام
/// تتيح للنظام التكيف التلقائي مع الشاشات المضافة
/// </summary>
public interface IScreenDiscoveryService
{
    /// <summary>
    /// اكتشاف جميع الشاشات/الـ Controllers المتاحة في النظام
    /// </summary>
    Task<List<DiscoveredScreen>> DiscoverScreensAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اكتشاف الشاشات الجديدة المضافة منذ آخر فحص
    /// </summary>
    Task<List<DiscoveredScreen>> DiscoverNewScreensAsync(
        DateTime since,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تحليل شاشة معينة وتصنيفها (Students, Employees, Financial, إلخ)
    /// </summary>
    Task<ScreenCategory> AnalyzeScreenCategoryAsync(
        string controllerName,
        string actionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اقتراح عوامل تحليل مناسبة لشاشة معينة
    /// </summary>
    Task<List<string>> SuggestRiskFactorsForScreenAsync(
        string controllerName,
        string actionName,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// شاشة مكتشفة في النظام
/// </summary>
public class DiscoveredScreen
{
    public string ControllerName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public ScreenCategory Category { get; set; }
    public DateTime DiscoveredAt { get; set; }
    public bool IsNew { get; set; }
    public List<string> RiskFactors { get; set; } = new();
}

/// <summary>
/// تصنيف الشاشة
/// </summary>
public enum ScreenCategory
{
    /// <summary>
    /// شاشات الطلاب
    /// </summary>
    Students = 0,

    /// <summary>
    /// شاشات الموظفين
    /// </summary>
    Employees = 1,

    /// <summary>
    /// شاشات الحضور
    /// </summary>
    Attendance = 2,

    /// <summary>
    /// شاشات الدرجات
    /// </summary>
    Grades = 3,

    /// <summary>
    /// شاشات مالية
    /// </summary>
    Financial = 4,

    /// <summary>
    /// شاشات أولياء الأمور
    /// </summary>
    Guardians = 5,

    /// <summary>
    /// شاشات الإعدادات
    /// </summary>
    Settings = 6,

    /// <summary>
    /// شاشات عامة/أخرى
    /// </summary>
    Other = 7
}
