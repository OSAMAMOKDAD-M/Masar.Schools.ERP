namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان ربط الصلاحية بالدور
/// </summary>
public class RolePermission
{
    /// <summary>
    /// معرف الربط
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// معرف الدور
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// معرف الصلاحية
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// هل الصلاحية ممنوحة (true) أو مستثناة (false)
    /// </summary>
    public bool IsGranted { get; set; } = true;

    /// <summary>
    /// الدور المرتبط
    /// </summary>
    public virtual MasarRole? Role { get; set; }

    /// <summary>
    /// الصلاحية المرتبطة
    /// </summary>
    public virtual Permission? Permission { get; set; }
}