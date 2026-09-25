namespace Masar.Schools.ERP.AI.Configuration;

/// <summary>
/// إعدادات مُساعد مَسَار الذكي للتنبؤ ومتابعة الطلاب
/// </summary>
public class MasarAIOptions
{
    public const string SectionName = "MasarAI";

    /// <summary>
    /// أوزان حساب المخاطر
    /// </summary>
    public RiskWeightsOptions RiskWeights { get; set; } = new();

    /// <summary>
    /// حدود مستويات الخطر
    /// </summary>
    public RiskThresholdsOptions RiskThresholds { get; set; } = new();

    /// <summary>
    /// إعدادات التحليل
    /// </summary>
    public AnalysisSettingsOptions AnalysisSettings { get; set; } = new();

    /// <summary>
    /// إعدادات الإشعارات
    /// </summary>
    public NotificationSettingsOptions NotificationSettings { get; set; } = new();

    /// <summary>
    /// إعدادات التخزين المؤقت
    /// </summary>
    public CachingSettingsOptions CachingSettings { get; set; } = new();

    /// <summary>
    /// إعدادات مساعد الذكاء الاصطناعي
    /// </summary>
    public AssistantSettingsOptions AssistantSettings { get; set; } = new();

    /// <summary>
    /// التحقق من صحة الإعدادات
    /// </summary>
    public bool Validate()
    {
        // التحقق من مجموع الأوزان
        var totalWeight = RiskWeights.Attendance + RiskWeights.Academic + RiskWeights.Financial;
        if (Math.Abs(totalWeight - 1.0) > 0.01)
        {
            return false;
        }

        // التحقق من الحدود
        if (RiskThresholds.SafeMaximum >= RiskThresholds.WarningMaximum)
            return false;

        if (RiskThresholds.WarningMaximum >= RiskThresholds.CriticalMinimum)
            return false;

        // التحقق من الفترات
        if (AnalysisSettings.AttendanceDaysToAnalyze <= 0)
            return false;

        if (AnalysisSettings.BackgroundJobIntervalMinutes <= 0)
            return false;

        return true;
    }
}

public class RiskWeightsOptions
{
    public double Attendance { get; set; } = 0.35;
    public double Academic { get; set; } = 0.45;
    public double Financial { get; set; } = 0.20;
}

public class RiskThresholdsOptions
{
    public int SafeMaximum { get; set; } = 39;
    public int WarningMaximum { get; set; } = 69;
    public int CriticalMinimum { get; set; } = 70;
}

public class AnalysisSettingsOptions
{
    public int AttendanceDaysToAnalyze { get; set; } = 30;
    public int AcademicPeriodDays { get; set; } = 90;
    public int FinancialPeriodDays { get; set; } = 60;
    public int BackgroundJobIntervalMinutes { get; set; } = 1440; // 24 ساعة
    public bool EnableScreenDiscovery { get; set; } = true;
    public int ScreenDiscoveryIntervalMinutes { get; set; } = 60;
}

public class NotificationSettingsOptions
{
    public bool NotifyOnCriticalTransition { get; set; } = true;
    public bool NotifyOnWarningTransition { get; set; } = false;
    public bool NotifyParentOnCritical { get; set; } = true;
    public string WhatsAppTemplateId { get; set; } = "student_risk_alert";
    public int CooldownPeriodHours { get; set; } = 24;
}

public class CachingSettingsOptions
{
    public int DashboardCacheDurationMinutes { get; set; } = 15;
    public int RiskMatrixCacheDurationMinutes { get; set; } = 10;
    public int PredictionCacheDurationMinutes { get; set; } = 30;
}

public class AssistantSettingsOptions
{
    /// <summary>
    /// تفعيل المساعد الذكي
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// مزود الخدمة (Simulated, OpenAI, Azure, Gemini)
    /// </summary>
    public string Provider { get; set; } = "Simulated";

    /// <summary>
    /// مفتاح API
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// رابط API
    /// </summary>
    public string ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// نموذج الذكاء الاصطناعي
    /// </summary>
    public string Model { get; set; } = "gpt-4";

    /// <summary>
    /// الحد الأقصى للرموز
    /// </summary>
    public int MaxTokens { get; set; } = 2000;

    /// <summary>
    /// درجة الإبداع (0-1)
    /// </summary>
    public double Temperature { get; set; } = 0.7;

    /// <summary>
    /// حد سجل المحادثات
    /// </summary>
    public int ChatHistoryLimit { get; set; } = 10;

    /// <summary>
    /// تفعيل الحفظ الدائم للمحادثات
    /// </summary>
    public bool EnablePersistentHistory { get; set; } = true;
}
