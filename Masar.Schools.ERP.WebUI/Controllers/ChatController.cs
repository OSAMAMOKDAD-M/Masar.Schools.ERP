using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Services;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly MasarDbContext _context;
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(
        MasarDbContext context, 
        IWhatsAppService whatsAppService,
        ILogger<ChatController> logger)
    {
        _context = context;
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    // GET: /Chat/Index
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        var chatRooms = await _context.ChatRooms
            .Where(c => c.IsActive && !c.IsDeleted)
            .Include(c => c.Members)
            .Include(c => c.Messages.Where(m => !m.IsDeleted))
                .ThenInclude(m => m.Sender)
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();

        return View(chatRooms);
    }

    // GET: /Chat/GetMessages
    [HttpGet]
    public async Task<IActionResult> GetMessages(Guid? conversationId)
    {
        if (conversationId == null)
        {
            return Json(new List<object>());
        }

        var messages = await _context.ChatMessages
            .Where(m => m.ChatRoomId == conversationId.Value && !m.IsDeleted)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new
            {
                m.Id,
                m.Content,
                m.MessageType,
                m.FilePath,
                m.FileName,
                m.FileSize,
                m.IsRead,
                m.ReadAt,
                m.CreatedAt,
                SenderName = m.Sender.FullName,
                SenderId = m.SenderId,
                ReceiverName = m.Receiver.FullName
            })
            .ToListAsync();

        return Json(messages);
    }

    // POST: /Chat/SendMessage
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(SendMessageDto input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized();
        }
        var currentUserId = Guid.Parse(userIdClaim);
        
        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            Content = input.Content,
            MessageType = input.MessageType,
            FilePath = input.FilePath,
            FileName = input.FileName,
            FileSize = input.FileSize,
            IsRead = false,
            SenderId = currentUserId,
            ReceiverId = input.ReceiverId,
            ChatRoomId = input.ChatRoomId,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.ChatMessages.Add(message);
        
        // Update chat room last message timestamp
        var chatRoom = await _context.ChatRooms.FindAsync(input.ChatRoomId);
        if (chatRoom != null)
        {
            chatRoom.LastMessageAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true, messageId = message.Id });
    }

    // POST: /Chat/SendWhatsAppAlert
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendWhatsAppAlert(WhatsAppAlertDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _whatsAppService.SendGeneralNotificationAsync(
                model.PhoneNumber,
                "تنبيه من نظام مَسَار للمدارس",
                model.Message
            );

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Failed to send WhatsApp message" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending WhatsApp alert");
            return Json(new { success = false, error = ex.Message });
        }
    }
}

public class SendMessageDto
{
    public Guid ChatRoomId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string MessageType { get; set; } = "Text";
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
}

public class WhatsAppAlertDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? StudentName { get; set; }
}
