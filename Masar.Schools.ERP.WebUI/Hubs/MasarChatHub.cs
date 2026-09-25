using Microsoft.AspNetCore.SignalR;

namespace Masar.Schools.ERP.WebUI.Hubs;

public class MasarChatHub : Hub
{
    private readonly ILogger<MasarChatHub> _logger;

    public MasarChatHub(ILogger<MasarChatHub> logger)
    {
        _logger = logger;
    }

    public async Task SendMessage(string senderId, string receiverId, string message, string chatRoomId)
    {
        _logger.LogInformation("Message from {SenderId} to {ReceiverId} in room {ChatRoomId}: {Message}", 
            senderId, receiverId, chatRoomId, message);

        // Send to specific user
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);

        // Send to all members in the chat room
        await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);
    }

    public async Task JoinRoom(string chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        _logger.LogInformation("User {ConnectionId} joined room {ChatRoomId}", Context.ConnectionId, chatRoomId);
        
        await Clients.Group(chatRoomId).SendAsync("UserJoined", Context.ConnectionId);
    }

    public async Task LeaveRoom(string chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        _logger.LogInformation("User {ConnectionId} left room {ChatRoomId}", Context.ConnectionId, chatRoomId);
        
        await Clients.Group(chatRoomId).SendAsync("UserLeft", Context.ConnectionId);
    }

    public async Task Typing(string chatRoomId, string userId, bool isTyping)
    {
        await Clients.Group(chatRoomId).SendAsync("UserTyping", userId, isTyping);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
