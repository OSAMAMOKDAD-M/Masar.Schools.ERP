using Microsoft.AspNetCore.Authorization;

namespace Masar.Schools.ERP.Infrastructure.Authorization;

/// <summary>
/// متطلب الصلاحية المخصص
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// كود الصلاحية المطلوبة
    /// </summary>
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}