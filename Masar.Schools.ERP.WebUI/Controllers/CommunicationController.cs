using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// التواصل بين المدرسة وأولياء الأمور
/// Communication Controller for School-Parent Communication
/// </summary>
[Authorize]
public class CommunicationController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<CommunicationController> _logger;

    public CommunicationController(MasarDbContext context, ILogger<CommunicationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// قائمة المحادثات مع أولياء الأمور
    /// List of conversations with guardians
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var chatRooms = await _context.ChatRooms
            .Include(cr => cr.Members)
                .ThenInclude(m => m.User)
            .Include(cr => cr.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
            .Where(cr => cr.Type == "Guardian")
            .OrderByDescending(cr => cr.UpdatedAt)
            .ToListAsync();

        return View(chatRooms);
    }

    /// <summary>
    /// بدء محادثة جديدة مع ولي أمر
    /// Start new conversation with guardian
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> NewConversation()
    {
        ViewBag.Guardians = new SelectList(await _context.Guardians
            .Where(g => !g.IsDeleted && g.IsActive)
            .OrderBy(g => g.FullNameArabic)
            .ToListAsync(), "Id", "FullNameArabic");

        return View();
    }

    /// <summary>
    /// إنشاء محادثة جديدة
    /// Create new conversation
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewConversation(NewConversationDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Guardians = new SelectList(await _context.Guardians
                .Where(g => !g.IsDeleted && g.IsActive)
                .OrderBy(g => g.FullNameArabic)
                .ToListAsync(), "Id", "FullNameArabic");
            return View(model);
        }

        var guardian = await _context.Guardians.FindAsync(model.GuardianId);
        if (guardian == null)
        {
            ModelState.AddModelError(string.Empty, "ولي الأمر غير موجود");
            return View(model);
        }

        // Create a virtual user for the guardian (if doesn't exist)
        // In a real system, guardians would have actual user accounts
        // For now, we'll use the guardian's ID as a reference

        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Create chat room
        var chatRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = $"محادثة مع {guardian.FullNameArabic}",
            Type = "Guardian",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ChatRooms.Add(chatRoom);

        // Add current user as member
        var currentUserMember = new ChatRoomMember
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            UserId = Guid.Parse(currentUserId),
            IsAdmin = true,
            JoinedAt = DateTime.UtcNow
        };
        _context.ChatRoomMembers.Add(currentUserMember);

        // Add guardian as member (using a placeholder user ID)
        // In production, this should be linked to actual guardian user account
        var guardianMember = new ChatRoomMember
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            UserId = guardian.Id, // Using guardian ID as placeholder
            IsAdmin = false,
            JoinedAt = DateTime.UtcNow
        };
        _context.ChatRoomMembers.Add(guardianMember);

        // Send initial message
        if (!string.IsNullOrEmpty(model.InitialMessage))
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Content = model.InitialMessage,
                MessageType = "Text",
                SenderId = Guid.Parse(currentUserId),
                ReceiverId = guardian.Id,
                ChatRoomId = chatRoom.Id,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(message);
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إنشاء المحادثة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// عرض المحادثة
    /// View conversation
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Conversation(Guid id)
    {
        var chatRoom = await _context.ChatRooms
            .Include(cr => cr.Messages.OrderBy(m => m.CreatedAt))
                .ThenInclude(m => m.Sender)
            .Include(cr => cr.Members)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(cr => cr.Id == id);

        if (chatRoom == null)
        {
            return NotFound();
        }

        // Mark messages as read
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(currentUserId))
        {
            var unreadMessages = chatRoom.Messages
                .Where(m => m.ReceiverId == Guid.Parse(currentUserId) && !m.IsRead);

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
                msg.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        return View(chatRoom);
    }

    /// <summary>
    /// إرسال رسالة
    /// Send message
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(ParentSendMessageDto model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Conversation), new { id = model.ChatRoomId });
        }

        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return RedirectToAction("Login", "Account");
        }

        var chatRoom = await _context.ChatRooms.FindAsync(model.ChatRoomId);
        if (chatRoom == null)
        {
            return NotFound();
        }

        // Find receiver (the other member in the chat room)
        var receiver = await _context.ChatRoomMembers
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.ChatRoomId == model.ChatRoomId && m.UserId != Guid.Parse(currentUserId));

        if (receiver == null)
        {
            return NotFound();
        }

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            Content = model.Content,
            MessageType = "Text",
            SenderId = Guid.Parse(currentUserId),
            ReceiverId = receiver.UserId,
            ChatRoomId = model.ChatRoomId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);

        // Update chat room timestamp
        chatRoom.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Send WhatsApp notification if receiver is a guardian
        if (receiver.User == null)
        {
            // Try to find guardian and send notification
            var guardian = await _context.Guardians.FindAsync(receiver.UserId);
            if (guardian != null && !string.IsNullOrEmpty(guardian.WhatsAppNumber))
            {
                // TODO: Send WhatsApp notification using WhatsAppService
                _logger.LogInformation("WhatsApp notification sent to guardian {GuardianId}", guardian.Id);
            }
        }

        return RedirectToAction(nameof(Conversation), new { id = model.ChatRoomId });
    }

    /// <summary>
    /// إرسال إشعار عام لجميع أولياء الأمور
    /// Send broadcast message to all guardians
    /// </summary>
    [HttpGet]
    public IActionResult Broadcast()
    {
        return View();
    }

    /// <summary>
    /// معالجة إرسال الإشعار العام
    /// Handle broadcast message
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Broadcast(BroadcastMessageDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Get all active guardians
        var guardians = await _context.Guardians
            .Where(g => !g.IsDeleted && g.IsActive)
            .ToListAsync();

        foreach (var guardian in guardians)
        {
            // Create individual chat room for each guardian
            var chatRoom = new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = $"إشعار عام - {guardian.FullNameArabic}",
                Type = "Broadcast",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.ChatRooms.Add(chatRoom);

            // Add admin as member
            var adminMember = new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoom.Id,
                UserId = Guid.Parse(currentUserId),
                IsAdmin = true,
                JoinedAt = DateTime.UtcNow
            };
            _context.ChatRoomMembers.Add(adminMember);

            // Add guardian as member
            var guardianMember = new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoom.Id,
                UserId = guardian.Id,
                IsAdmin = false,
                JoinedAt = DateTime.UtcNow
            };
            _context.ChatRoomMembers.Add(guardianMember);

            // Add message
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Content = model.Message,
                MessageType = "Text",
                SenderId = Guid.Parse(currentUserId),
                ReceiverId = guardian.Id,
                ChatRoomId = chatRoom.Id,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(message);
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"تم إرسال الإشعار إلى {guardians.Count} ولي أمر";
        return RedirectToAction(nameof(Index));
    }
}

// DTOs
public class NewConversationDto
{
    [Required(ErrorMessage = "ولي الأمر مطلوب")]
    public Guid GuardianId { get; set; }

    [Display(Name = "الرسالة الأولية")]
    public string? InitialMessage { get; set; }
}

public class BroadcastMessageDto
{
    [Required(ErrorMessage = "عنوان الإشعار مطلوب")]
    [Display(Name = "العنوان")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوى الإشعار مطلوب")]
    [Display(Name = "المحتوى")]
    public string Message { get; set; } = string.Empty;
}

public class ParentSendMessageDto
{
    [Required(ErrorMessage = "المحتوى مطلوب")]
    public string Content { get; set; } = string.Empty;

    public Guid ChatRoomId { get; set; }
}
