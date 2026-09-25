using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class ChatRoomMember : BaseEntity
{
    public DateTime? JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsMuted { get; set; }
    public DateTime? MutedUntil { get; set; }
    
    public Guid UserId { get; set; }
    public MasarUser User { get; set; } = null!;
    
    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}
