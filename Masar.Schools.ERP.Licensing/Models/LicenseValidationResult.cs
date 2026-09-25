namespace Masar.Schools.ERP.Licensing.Models;

/// <summary>
/// نتيجة التحقق من صحة الترخيص
/// </summary>
public class LicenseValidationResult
{
    /// <summary>
    /// هل الترخيص صحيح؟
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// حالة الترخيص
    /// </summary>
    public LicenseStatus Status { get; set; }

    /// <summary>
    /// رسالة الخطأ أو التفاصيل
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// بيانات الترخيص المفككة (إذا كان صحيحاً)
    /// </summary>
    public LicenseModel? LicenseData { get; set; }

    /// <summary>
    /// تاريخ التحقق
    /// </summary>
    public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// إنشاء نتيجة ناجحة
    /// </summary>
    public static LicenseValidationResult Success(LicenseModel license)
    {
        return new LicenseValidationResult
        {
            IsValid = true,
            Status = LicenseStatus.Valid,
            LicenseData = license,
            Message = "الترخيص صحيح وساري المفعول"
        };
    }

    /// <summary>
    /// إنشاء نتيجة فاشلة
    /// </summary>
    public static LicenseValidationResult Failure(LicenseStatus status, string message)
    {
        return new LicenseValidationResult
        {
            IsValid = false,
            Status = status,
            Message = message
        };
    }
}

/// <summary>
/// حالة الترخيص
/// </summary>
public enum LicenseStatus
{
    Valid,                  // صحيح وساري
    Expired,                // منتهي
    HardwareMismatch,       // عدم تطابق العتاد
    InvalidSignature,       // توقيع غير صحيح
    ExceededLimits,         // تجاوز الحدود المسموحة
    Corrupted,              // ملف تالف
    NotActivated,           // غير مفعل
    Revoked                 // ملغى
}