using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    
    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
}
