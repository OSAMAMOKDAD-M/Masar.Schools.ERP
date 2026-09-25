using Masar.Schools.ERP.AI.DTOs;

namespace Masar.Schools.ERP.AI.Interfaces;

/// <summary>
/// واجهة خدمة مساعد مَسَار الذكي
/// </summary>
public interface IMasarAiService
{
    /// <summary>
    /// إرسال رسالة للمساعد الذكي والحصول على استجابة
    /// </summary>
    /// <param name="request">طلب المحادثة</param>
    /// <param name="cancellationToken">رمز الإلغاء</param>
    /// <returns>استجابة المحادثة</returns>
    Task<ChatResponseDto> ChatAsync(ChatRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// الحصول على سجل المحادثات للمستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="limit">عدد الرسائل المطلوبة</param>
    /// <param name="cancellationToken">رمز الإلغاء</param>
    /// <returns>قائمة رسائل المحادثة</returns>
    Task<List<ChatMessageDto>> GetChatHistoryAsync(string userId, int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// مسح سجل المحادثات للمستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="cancellationToken">رمز الإلغاء</param>
    /// <returns>مهمة غير متزامنة</returns>
    Task ClearChatHistoryAsync(string userId, CancellationToken cancellationToken = default);
}
