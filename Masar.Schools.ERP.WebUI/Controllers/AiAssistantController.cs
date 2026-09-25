using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using System.Security.Claims;

namespace Masar.Schools.ERP.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiAssistantController : ControllerBase
{
    private readonly IMasarAiService _masarAiService;
    private readonly ILogger<AiAssistantController> _logger;

    public AiAssistantController(
        IMasarAiService masarAiService,
        ILogger<AiAssistantController> logger)
    {
        _masarAiService = masarAiService;
        _logger = logger;
    }

    /// <summary>
    /// إرسال رسالة للمساعد الذكي والحصول على استجابة
    /// </summary>
    [HttpPost("chat")]
    public async Task<ActionResult<ChatResponseDto>> Chat([FromBody] ChatRequestDto request)
    {
        try
        {
            // Extract user context from claims if not provided
            if (request.UserContext == null)
            {
                request.UserContext = new UserContextDto
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                    UserName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                    RoleName = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty,
                    TenantId = int.TryParse(User.FindFirstValue("TenantId"), out var tenantId) ? tenantId : null
                };
            }

            // Get current page title from request or header
            if (string.IsNullOrEmpty(request.UserContext.CurrentPageTitle))
            {
                request.UserContext.CurrentPageTitle = Request.Headers["X-Current-Page"].FirstOrDefault() ?? string.Empty;
            }

            var response = await _masarAiService.ChatAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AI chat endpoint");
            return StatusCode(500, new ChatResponseDto
            {
                Success = false,
                ErrorMessage = "حدث خطأ في الخادم",
                ResponseText = "عذراً، حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى.",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// الحصول على سجل المحادثات للمستخدم الحالي
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetHistory([FromQuery] int limit = 10)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var history = await _masarAiService.GetChatHistoryAsync(userId, limit);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chat history");
            return StatusCode(500, new List<ChatMessageDto>());
        }
    }

    /// <summary>
    /// مسح سجل المحادثات للمستخدم الحالي
    /// </summary>
    [HttpDelete("history")]
    public async Task<ActionResult> ClearHistory()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _masarAiService.ClearChatHistoryAsync(userId);
            return Ok(new { success = true, message = "تم مسح سجل المحادثات بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing chat history");
            return StatusCode(500, new { success = false, message = "حدث خطأ أثناء مسح السجل" });
        }
    }
}
