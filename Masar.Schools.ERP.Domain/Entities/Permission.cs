namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان الصلاحية
/// </summary>
public class Permission
{
    /// <summary>
    /// معرف الصلاحية
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// كود الصلاحية الفريد (مثال: Students.View)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// اسم الصلاحية بالعربية
    /// </summary>
    public string NameArabic { get; set; } = string.Empty;

    /// <summary>
    /// اسم الصلاحية بالإنجليزية
    /// </summary>
    public string NameEnglish { get; set; } = string.Empty;

    /// <summary>
    /// اسم الموديول التابع له
    /// </summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// وصف الصلاحية
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// هل الصلاحية نشطة؟
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// الصلاحيات المترابطة مع هذه الصلاحية
    /// </summary>
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}