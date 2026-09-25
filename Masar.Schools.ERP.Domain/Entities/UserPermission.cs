namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان ربط الصلاحية بالمستخدم
/// </summary>
public class UserPermission
{
    /// <summary>
    /// معرف الربط
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// معرف المستخدم
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// معرف الصلاحية
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// هل الصلاحية ممنوحة (true) أو مستثناة (false)
    /// </summary>
    public bool IsGranted { get; set; } = true;

    /// <summary>
    /// المستخدم المرتبط
    /// </summary>
    public virtual MasarUser? User { get; set; }

    /// <summary>
    /// الصلاحية المرتبطة
    /// </summary>
    public virtual Permission? Permission { get; set; }
}