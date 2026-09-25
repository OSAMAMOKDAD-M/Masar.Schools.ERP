using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string? LogoPath { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public string? ZatcaCertificatePath { get; set; }
    public string? ZatcaSecretKey { get; set; }
    
    public ICollection<School> Schools { get; set; } = new List<School>();
    public ICollection<MasarUser> MasarUsers { get; set; } = new List<MasarUser>();
    public ICollection<MasarRole> MasarRoles { get; set; } = new List<MasarRole>();
}
