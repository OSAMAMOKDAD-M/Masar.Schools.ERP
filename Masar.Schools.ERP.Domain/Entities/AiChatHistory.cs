using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// سجل محادثات مساعد مَسَار الذكي
/// </summary>
public class AiChatHistory : BaseEntity
{
    /// <summary>
    /// معرف المستخدم في ASP.NET Identity
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// معرف المستأجر (Tenant)
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// رسالة المستخدم
    /// </summary>
    public string UserMessage { get; set; } = string.Empty;

    /// <summary>
    /// استجابة المساعد الذكي
    /// </summary>
    public string AssistantResponse { get; set; } = string.Empty;

    /// <summary>
    /// سياق الصفحة التي كان عليها المستخدم
    /// </summary>
    public string PageContext { get; set; } = string.Empty;

    /// <summary>
    /// دور المستخدم وقت المحادثة
    /// </summary>
    public string UserRole { get; set; } = string.Empty;

    /// <summary>
    /// اسم المستخدم وقت المحادثة
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// هل كانت المحادثة ناجحة
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// رسالة الخطأ إذا فشلت المحادثة
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// الوقت الذي تم فيه إنشاء السجل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
