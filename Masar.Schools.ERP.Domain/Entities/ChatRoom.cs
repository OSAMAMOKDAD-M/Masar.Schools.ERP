using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class ChatRoom : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public string Type { get; set; } = string.Empty; // Direct, Group, Class, Subject
    public string? Icon { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastMessageAt { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid? SchoolId { get; set; }
    public School? School { get; set; }
    
    public Guid? ClassRoomId { get; set; }
    public ClassRoom? ClassRoom { get; set; }
    
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
}
