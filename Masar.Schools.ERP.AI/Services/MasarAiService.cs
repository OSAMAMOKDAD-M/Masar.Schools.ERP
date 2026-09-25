using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Masar.Schools.ERP.AI.Services;

/// <summary>
/// خدمة مساعد مَسَار الذكي
/// </summary>
public class MasarAiService : IMasarAiService
{
    private readonly MasarDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly MasarAIOptions _options;
    private readonly ILogger<MasarAiService> _logger;

    // System Prompt for the AI Assistant
    private const string SystemPrompt = @"أنت 'مسار الذكي' (Masar AI)، المساعد الرقمي والذكاء الاصطناعي المدمج في واجهة نظام 'مَسَار للمدارس' (Masar Schools ERP System v1.0.1).

دورك: تقديم الدعم الفني والتنفيذي المباشر لجميع المستخدمين (مدراء، معلمين، محاسبين، شؤون طلاب، أولياء أمور) وإرشادهم داخل الشاشات بحسب دورهم.

معرفتك الكاملة بالنظام تشمل:

1. إدارة المؤسسات والشعارات:
   - رفع الشعار بسحب وإسقاط الملفات
   - المعاينة الفورية للشعار
   - تخصيص هوية وألوان المؤسسة

2. محرك التنبؤ الذكي لمؤشر الخطر للطلاب (Student Risk Index):
   - الحضور: 35%
   - الأداء الأكاديمي: 45%
   - المالية: 20%
   - المستويات: آمن (0-39)، تحذير (40-69)، خطر حرج (70-100)
   - إرسال التنبيهات للمشرفين

3. إدارة الصلاحيات الديناميكية:
   - مصفوفة الصلاحيات المربوطة 100% بقاعدة البيانات
   - تحديث الصلاحيات الفوري دون إعادة نشر الكود

4. الشؤون المالية وفواتير ZATCA:
   - إدارة الأقساط المدرسية
   - الفواتير المعتمدة ضريبياً من ZATCA
   - متابعة المتأخرات
   - إصدار الإيصالات

5. شؤون الطلاب والكنترول:
   - التوزيع على الفصول
   - البطاقة الرقمية للطالب
   - شيت الكنترول المعتمد
   - رصد الدرجات وإصدار الشهادات

6. الحضور والغياب:
   - تسجيل الحضور ببصمة الإصبع
   - تسجيل التأخيرات
   - إرسال إشعارات تلقائية عبر WhatsApp لأولياء الأمور

7. الملف الشخصي والواجهة:
   - تعديل البيانات والصورة الشخصية
   - القائمة الجانبية (Sidebar) المحدثة
   - إدارة الجلسات والأمان

القواعد والتعليمات:
- أجِب باللغة العربية فقط
- استخدم أسلوب راقٍ ومباشر ومهني
- نسق الشاشات والأزرار بخط عريض (مثال: **إدارة المؤسسات**، **رفع الشعار**)
- التزم بحدود صلاحيات دور المستخدم الحالي
- إذا طلب المستخدم إجراءً غير مسموح لدوره، وجهه بأسلوب لبق للشخص المسؤول أو للشاشة التي يملك صلاحية الوصول إليها
- اشرح الأخطاء والاستثناءات بشكل بسيط وواضح
- قدم إرشادات خطوة بخطوة عند الحاجة
- احترم خصوصية البيانات ولا تطلب معلومات حساسة";

    public MasarAiService(
        MasarDbContext context,
        HttpClient httpClient,
        IOptions<MasarAIOptions> options,
        ILogger<MasarAiService> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ChatResponseDto> ChatAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = new ChatResponseDto
        {
            Success = false,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            // Get chat history from database OR use provided history from client
            var history = request.ChatHistory ?? await GetChatHistoryAsync(request.UserContext?.UserId ?? string.Empty, 10, cancellationToken);

            // Construct the AI request payload
            var aiMessages = new List<object>
            {
                new { role = "system", content = SystemPrompt }
            };

            // Add conversation history from client or database
            foreach (var msg in history)
            {
                var role = msg.Role ?? msg.Sender ?? "user";
                var content = msg.Content ?? msg.Message ?? string.Empty;
                aiMessages.Add(new { role = role, content = content });
            }

            // Add current user message
            aiMessages.Add(new { role = "user", content = request.UserMessage });

            _logger.LogInformation("Sending {MessageCount} messages to AI", aiMessages.Count);

            // Add context information
            var contextInfo = $"\n\n--- معلومات السياق الحالي ---\n" +
                             $"المستخدم: {request.UserContext?.UserName}\n" +
                             $"الدور: {request.UserContext?.RoleName}\n" +
                             $"الصفحة الحالية: {request.UserContext?.CurrentPageTitle}\n" +
                             $"معرف المستأجر: {request.UserContext?.TenantId}\n" +
                             $"----------------------------";

            var userMessageWithContext = request.UserMessage + contextInfo;
            aiMessages[^1] = new { role = "user", content = userMessageWithContext };

            // Call AI endpoint (OpenAI-compatible format)
            var aiResponse = await CallAiEndpointAsync(aiMessages, cancellationToken);

            response.ResponseText = aiResponse;
            response.Success = true;

            // Save to database
            await SaveChatHistoryAsync(request, response, cancellationToken);

            _logger.LogInformation("AI chat successful for user {UserId}", request.UserContext?.UserId);
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.ResponseText = "عذراً، حدث خطأ أثناء معالجة طلبك. يرجى المحاولة مرة أخرى لاحقاً.";
            _logger.LogError(ex, "AI chat failed for user {UserId}", request.UserContext?.UserId);
        }

        return response;
    }

    public async Task<List<ChatMessageDto>> GetChatHistoryAsync(string userId, int limit = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return new List<ChatMessageDto>();

        var history = await _context.AiChatHistories
            .AsNoTracking()
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.CreatedAt)
            .Take(limit)
            .OrderBy(h => h.CreatedAt)
            .ToListAsync(cancellationToken);

        var messages = new List<ChatMessageDto>();

        foreach (var record in history)
        {
            messages.Add(new ChatMessageDto
            {
                Sender = "user",
                Message = record.UserMessage,
                Timestamp = record.CreatedAt
            });

            messages.Add(new ChatMessageDto
            {
                Sender = "assistant",
                Message = record.AssistantResponse,
                Timestamp = record.CreatedAt.AddSeconds(1)
            });
        }

        return messages;
    }

    public async Task ClearChatHistoryAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var history = await _context.AiChatHistories
            .Where(h => h.UserId == userId)
            .ToListAsync(cancellationToken);

        _context.AiChatHistories.RemoveRange(history);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleared chat history for user {UserId}", userId);
    }

    private async Task<string> CallAiEndpointAsync(List<object> messages, CancellationToken cancellationToken)
    {
        // Check if AI assistant is enabled
        if (!_options.AssistantSettings.Enabled)
        {
            return "المساعد الذكي غير مفعل حالياً. يرجى التواصل مع مسؤول النظام.";
        }

        // Check provider type
        var provider = _options.AssistantSettings.Provider?.ToLowerInvariant();

        if (provider == "simulated")
        {
            // Use simulated responses
            await Task.Delay(500, cancellationToken); // Simulate API latency

            var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
            var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;

            return GenerateSimulatedResponse(userText);
        }
        else if (provider == "openai" || provider == "azure")
        {
            // Use OpenAI API
            return await CallOpenAiEndpointAsync(messages, cancellationToken);
        }
        else if (provider == "gemini")
        {
            // Use Google Gemini API
            return await CallGeminiEndpointAsync(messages, cancellationToken);
        }
        else
        {
            // Default to simulated
            await Task.Delay(500, cancellationToken);

            var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
            var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;

            return GenerateSimulatedResponse(userText);
        }
    }

    private async Task<string> CallOpenAiEndpointAsync(List<object> messages, CancellationToken cancellationToken)
    {
        try
        {
            var apiKey = _options.AssistantSettings.ApiKey;
            var apiUrl = _options.AssistantSettings.ApiUrl ?? "https://api.openai.com/v1/chat/completions";
            var model = _options.AssistantSettings.Model ?? "gpt-4";
            var maxTokens = _options.AssistantSettings.MaxTokens;
            var temperature = _options.AssistantSettings.Temperature;

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("OpenAI API key not configured, falling back to simulated response");
                var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
                var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;
                return GenerateSimulatedResponse(userText);
            }

            var payload = new
            {
                model = model,
                messages = messages,
                temperature = temperature,
                max_tokens = maxTokens
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.PostAsync(apiUrl, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var aiResponse = JsonSerializer.Deserialize<JsonDocument>(responseJson);
            return aiResponse.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling OpenAI API, falling back to simulated response");
            var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
            var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;
            return GenerateSimulatedResponse(userText);
        }
    }

    private async Task<string> CallGeminiEndpointAsync(List<object> messages, CancellationToken cancellationToken)
    {
        try
        {
            var apiKey = _options.AssistantSettings.ApiKey;
            var apiUrl = _options.AssistantSettings.ApiUrl ?? "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
            var model = _options.AssistantSettings.Model ?? "gemini-pro";

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Gemini API key not configured, falling back to simulated response");
                var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
                var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;
                return GenerateSimulatedResponse(userText);
            }

            // Convert messages to Gemini format
            var contents = messages.Select(m => new
            {
                role = m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "assistant" ? "model" : "user",
                parts = new[] { new { text = m.GetType().GetProperty("content")?.GetValue(m)?.ToString() } }
            }).ToList();

            var payload = new
            {
                contents = contents,
                generationConfig = new
                {
                    temperature = _options.AssistantSettings.Temperature,
                    maxOutputTokens = _options.AssistantSettings.MaxTokens
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var urlWithKey = $"{apiUrl}?key={apiKey}";
            var response = await _httpClient.PostAsync(urlWithKey, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var aiResponse = JsonSerializer.Deserialize<JsonDocument>(responseJson);
            return aiResponse.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Gemini API, falling back to simulated response");
            var lastUserMessage = messages.LastOrDefault(m => m.GetType().GetProperty("role")?.GetValue(m)?.ToString() == "user");
            var userText = lastUserMessage?.GetType().GetProperty("content")?.GetValue(lastUserMessage)?.ToString() ?? string.Empty;
            return GenerateSimulatedResponse(userText);
        }
    }

    private string GenerateSimulatedResponse(string userMessage)
    {
        // Simple rule-based responses for demonstration
        // In production, this would be replaced with actual AI

        if (userMessage.Contains("شعار") || userMessage.Contains("هوية"))
        {
            return "لرفع شعار المؤسسة وتخصيص الهوية، اتبع الخطوات التالية:\n\n1. اذهب إلى **إدارة المؤسسات**\n2. اختر **إعدادات الهوية**\n3. اسحب وأفلت ملف الشعار في منطقة الرفع\n4. ستظهر معاينة فورية للشعار\n5. اختر الألوان الأساسية والثانوية للمؤسسة\n6. اضغط **حفظ** لتطبيق التغييرات\n\nملاحظة: يجب أن يكون الشعار بصيغة PNG أو JPG ولا يتجاوز 5MB.";
        }

        if (userMessage.Contains("خطر") || userMessage.Contains("مؤشر"))
        {
            return "مؤشر الخطر للطلاب (Student Risk Index) هو نظام ذكي يحلل ثلاثة عوامل رئيسية:\n\n- **الحضور والغياب (35%)**: نسبة الحضور والغياب\n- **الأداء الأكاديمي (45%)**: الدرجات والاختبارات\n- **المالية (20%)**: الأقساط والمدفوعات\n\n**مستويات الخطر:**\n- 🔵 آمن: 0-39\n- 🟡 تحذير: 40-69\n- 🔴 خطر حرج: 70-100\n\nعند وصول طالب لمستوى الخطر الحرج، يتم إرسال تنبيه تلقائي للمشرفين وأولياء الأمور.";
        }

        if (userMessage.Contains("فاتورة") || userMessage.Contains("ZATCA"))
        {
            return "لإصدار فاتورة معتمدة من ZATCA:\n\n1. اذهب إلى **الفواتير**\n2. اضغط **إصدار فاتورة جديدة**\n3. اختر الطالب أو ولي الأمر\n4. حدد نوع الفاتورة (قسط، رسوم إضافية، إلخ)\n5. سيتم إنشاء الفاتورة مع رمز ضريبي ZATCA تلقائياً\n6. يمكن تحميل الفاتورة بصيغة PDF XML للإيداع الضريبي\n\nملاحظة: تأكد من تكوين مفاتيح ZATCA في إعدادات النظام.";
        }

        if (userMessage.Contains("كنترول") || userMessage.Contains("درجات"))
        {
            return "لرصد الدرجات في شيت الكنترول:\n\n1. اذهب إلى **الدرجات والامتحانات**\n2. اختر الفصل الدراسي المطلوب\n3. اضغط **الكنترول**\n4. ستظهر قائمة الطلاب مع حقول الدرجات\n5. أدخل درجات الفصل الأول، الثاني، والنهائي\n6. اضغط **حفظ** لحفظ الدرجات\n7. يمكنك طباعة شيت الكنترول من زر **طباعة**\n\nلإصدار الشهادات، اضغط زر **الشهادات** في نفس الصفحة.";
        }

        if (userMessage.Contains("حضور") || userMessage.Contains("غياب"))
        {
            return "تسجيل الحضور والغياب يتم بعدة طرق:\n\n1. **بصمة الإصبع**: من خلال أجهزة البصمة المتصلة بالنظام\n2. **المناداة الإلكترونية**: من واجهة المعلم\n3. **التسجيل اليدوي**: من صفحة الحضور\n\nعند تسجيل غياب طالب، يتم إرسال إشعار تلقائي عبر WhatsApp لولي الأمر. يمكنك إعدادات هذه الإشعارات من **الإعدادات > إشعارات WhatsApp**.";
        }

        if (userMessage.Contains("صلاحية") || userMessage.Contains("دور"))
        {
            return "إدارة الصلاحيات في النظام:\n\n1. اذهب إلى **الأدوار والصلاحيات**\n2. ستجد قائمة الأدور الموجودة (مدير، معلم، محاسب، إلخ)\n3. لكل دور، يمكنك تحديد الصلاحيات المسموح بها\n4. التغييرات تُطبق فوراً دون الحاجة لإعادة تشغيل النظام\n5. يمكنك إنشاء أدور جديدة مخصصة\n\nالصلاحيات مربوطة 100% بقاعدة البيانات وتُحمل بشكل ديناميكي.";
        }

        // Default response
        return "أهلاً بك! أنا مسار الذكي، المساعد الرقمي لنظام مَسَار للمدارس.\n\nيمكنني مساعدتك في:\n\n- 🏢 **إدارة المؤسسات والشعارات**\n- 🎓 **شؤون الطلاب والكنترول**\n- 💰 **الفواتير والمالية ZATCA**\n- 📊 **مؤشر الخطر للطلاب**\n- 👥 **إدارة الصلاحيات**\n- 📅 **الحضور والغياب**\n- 🔐 **الملف الشخصي والأمان**\n\nكيف يمكنني مساعدتك اليوم؟ يرجى سؤالي عن أي ميزة من ميزات النظام.";
    }

    private async Task SaveChatHistoryAsync(ChatRequestDto request, ChatResponseDto response, CancellationToken cancellationToken)
    {
        var chatRecord = new AiChatHistory
        {
            Id = Guid.NewGuid(),
            UserId = request.UserContext?.UserId ?? string.Empty,
            TenantId = request.UserContext?.TenantId,
            UserMessage = request.UserMessage,
            AssistantResponse = response.ResponseText,
            PageContext = request.UserContext?.CurrentPageTitle ?? string.Empty,
            UserRole = request.UserContext?.RoleName ?? string.Empty,
            UserName = request.UserContext?.UserName ?? string.Empty,
            IsSuccessful = response.Success,
            ErrorMessage = response.ErrorMessage,
            CreatedAt = DateTime.UtcNow
        };

        _context.AiChatHistories.Add(chatRecord);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
