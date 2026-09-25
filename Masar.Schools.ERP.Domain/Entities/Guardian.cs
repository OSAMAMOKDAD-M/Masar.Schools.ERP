using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Guardian : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Relationship { get; set; } // Father, Mother, Guardian
    public string? RelationshipArabic { get; set; }
    public string? Occupation { get; set; }
    public string? OccupationArabic { get; set; }
    public bool IsActive { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
