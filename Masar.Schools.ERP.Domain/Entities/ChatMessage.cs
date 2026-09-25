using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string MessageType { get; set; } = string.Empty; // Text, Image, File, Voice, Video
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    
    public Guid SenderId { get; set; }
    public MasarUser Sender { get; set; } = null!;
    
    public Guid ReceiverId { get; set; }
    public MasarUser Receiver { get; set; } = null!;
    
    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
    
    public Guid? ReplyToMessageId { get; set; }
    public ChatMessage? ReplyToMessage { get; set; }
}
