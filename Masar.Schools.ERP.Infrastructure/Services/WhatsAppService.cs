using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Masar.Schools.ERP.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WhatsAppService> _logger;
    private readonly HttpClient _httpClient;

    public WhatsAppService(
        IConfiguration configuration,
        ILogger<WhatsAppService> logger,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<bool> SendAttendanceNotificationAsync(string phoneNumber, string studentName, string status, DateTime attendanceDate)
    {
        try
        {
            var message = $"نظام مَسَار للمدارس 🎓\n\n" +
                         $"مرحباً ولي أمر الطالب: {studentName}\n" +
                         $"حضور الطالب بتاريخ: {attendanceDate:yyyy-MM-dd}\n" +
                         $"الحالة: {GetStatusArabic(status)}\n\n" +
                         $"يرجى التواصل مع الإدارة في حال وجود أي استفسار.\n\n" +
                         $"© 2026 مَسَار للمدارس";

            return await SendMessageAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending attendance notification to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendInvoiceNotificationAsync(string phoneNumber, string studentName, string invoiceNumber, decimal amount, DateTime dueDate)
    {
        try
        {
            var message = $"نظام مَسَار للمدارس 💰\n\n" +
                         $"مرحباً ولي أمر الطالب: {studentName}\n" +
                         $"فاتورة جديدة: {invoiceNumber}\n" +
                         $"المبلغ: {amount:F2} ريال\n" +
                         $"تاريخ الاستحقاق: {dueDate:yyyy-MM-dd}\n\n" +
                         $"يرجى سداد الفاتورة في الموعد المحدد.\n\n" +
                         $"© 2026 مَسَار للمدارس";

            return await SendMessageAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending invoice notification to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendBehavioralNotificationAsync(string phoneNumber, string studentName, string violationType, string description)
    {
        try
        {
            var message = $"نظام مَسَار للمدارس ⚠️\n\n" +
                         $"مرحباً ولي أمر الطالب: {studentName}\n" +
                         $"نود إبلاغكم بمخالفة سلوكية:\n" +
                         $"نوع المخالفة: {violationType}\n" +
                         $"التفاصيل: {description}\n\n" +
                         $"يرجى التواصل مع الإدارة لمناقشة الموضوع.\n\n" +
                         $"© 2026 مَسَار للمدارس";

            return await SendMessageAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending behavioral notification to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetAsync(string phoneNumber, string otpCode)
    {
        try
        {
            var message = $"نظام مَسَار للمدارس 🔐\n\n" +
                         $"رمز استعادة كلمة المرور:\n" +
                         $"【 {otpCode} 】\n\n" +
                         $"هذا الرمز صالح لمدة 10 دقائق فقط.\n" +
                         $"لا تشارك هذا الرمز مع أي شخص.\n\n" +
                         $"© 2026 مَسَار للمدارس";

            return await SendMessageAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendGeneralNotificationAsync(string phoneNumber, string title, string message)
    {
        try
        {
            var fullMessage = $"نظام مَسَار للمدارس 📢\n\n" +
                            $"{title}\n\n" +
                            $"{message}\n\n" +
                            $"© 2026 مَسَار للمدارس";

            return await SendMessageAsync(phoneNumber, fullMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending general notification to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    private async Task<bool> SendMessageAsync(string phoneNumber, string message)
    {
        try
        {
            var apiKey = _configuration["WhatsApp:ApiKey"];
            var apiUrl = _configuration["WhatsApp:ApiUrl"];
            var senderNumber = _configuration["WhatsApp:SenderNumber"];

            // Format phone number (ensure it starts with country code)
            var formattedPhone = FormatPhoneNumber(phoneNumber);

            var payload = new
            {
                api_key = apiKey,
                sender = senderNumber,
                to = formattedPhone,
                message = message
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("WhatsApp message sent successfully to {PhoneNumber}", formattedPhone);
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to send WhatsApp message to {PhoneNumber}. Status: {StatusCode}, Error: {Error}", 
                    formattedPhone, response.StatusCode, errorContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending WhatsApp message to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        // Remove any non-digit characters
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
        
        // Ensure it starts with Saudi country code
        if (digits.StartsWith("0"))
        {
            digits = "966" + digits.Substring(1);
        }
        else if (!digits.StartsWith("966"))
        {
            digits = "966" + digits;
        }
        
        return digits;
    }

    private string GetStatusArabic(string status)
    {
        return status.ToLower() switch
        {
            "present" => "حاضر",
            "absent" => "غائب",
            "late" => "متأخر",
            "excused" => "غائب بعذر",
            _ => status
        };
    }
}

public interface IWhatsAppService
{
    Task<bool> SendAttendanceNotificationAsync(string phoneNumber, string studentName, string status, DateTime attendanceDate);
    Task<bool> SendInvoiceNotificationAsync(string phoneNumber, string studentName, string invoiceNumber, decimal amount, DateTime dueDate);
    Task<bool> SendBehavioralNotificationAsync(string phoneNumber, string studentName, string violationType, string description);
    Task<bool> SendPasswordResetAsync(string phoneNumber, string otpCode);
    Task<bool> SendGeneralNotificationAsync(string phoneNumber, string title, string message);
}