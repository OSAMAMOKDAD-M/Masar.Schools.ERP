namespace Masar.Schools.ERP.AI.DTOs;

/// <summary>
/// سياق المستخدم الحالي في النظام
/// </summary>
public class UserContextDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string CurrentPageTitle { get; set; } = string.Empty;
    public int? TenantId { get; set; }
    public string UserRoleArabic { get; set; } = string.Empty;
}

/// <summary>
/// رسالة المحادثة
/// </summary>
public class ChatMessageDto
{
    public string Sender { get; set; } = string.Empty; // "user" or "assistant"
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    // For client-side dynamic properties
    public string? Role { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// طلب المحادثة من المستخدم
/// </summary>
public class ChatRequestDto
{
    public string UserMessage { get; set; } = string.Empty;
    public UserContextDto? UserContext { get; set; }
    public List<ChatMessageDto> ChatHistory { get; set; } = new();
}

/// <summary>
/// استجابة المحادثة من المساعد الذكي
/// </summary>
public class ChatResponseDto
{
    public string ResponseText { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime Timestamp { get; set; }
}
