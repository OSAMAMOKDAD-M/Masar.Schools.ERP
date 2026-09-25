using Microsoft.AspNetCore.Identity;

namespace Masar.Schools.ERP.Domain.Entities;

public class MasarUser : IdentityUser<Guid>
{
    public string? NationalId { get; set; }
    public string? FirstName { get; set; }
    public string? FirstNameArabic { get; set; }
    public string? LastName { get; set; }
    public string? LastNameArabic { get; set; }
    public string? FullName { get; set; }
    public string? FullNameArabic { get; set; }
    public string? ProfileImagePath { get; set; }
    public bool IsActive { get; set; }
    public bool IsSuperAdmin { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public new DateTime? LockoutEnd { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid? SchoolId { get; set; }
    public School? School { get; set; }
    
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    
    public ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
