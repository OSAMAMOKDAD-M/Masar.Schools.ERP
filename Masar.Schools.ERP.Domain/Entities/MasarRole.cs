using Microsoft.AspNetCore.Identity;

namespace Masar.Schools.ERP.Domain.Entities;

public class MasarRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public bool IsActive { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
