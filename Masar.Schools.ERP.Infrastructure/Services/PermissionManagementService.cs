using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Authorization;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة إدارة الصلاحيات ديناميكياً من قاعدة البيانات
/// </summary>
public class PermissionManagementService
{
    private readonly MasarDbContext _context;
    private readonly IMemoryCache _cache;

    public PermissionManagementService(
        MasarDbContext context,
        IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    /// <summary>
    /// الحصول على جميع الصلاحيات مقسمة حسب الموديول
    /// </summary>
    public async Task<Dictionary<string, List<Permission>>> GetPermissionsByModuleAsync()
    {
        var permissions = await _context.Permissions
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        return permissions
            .GroupBy(p => p.Module)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(p => p.DisplayOrder).ToList()
            );
    }

    /// <summary>
    /// الحصول على صلاحيات دور معين
    /// </summary>
    public async Task<List<Guid>> GetRolePermissionsAsync(Guid roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && rp.IsGranted)
            .Select(rp => rp.PermissionId)
            .ToListAsync();
    }

    /// <summary>
    /// الحصول على صلاحيات مستخدم معين (المباشرة)
    /// </summary>
    public async Task<List<Guid>> GetUserPermissionsAsync(Guid userId)
    {
        return await _context.UserPermissions
            .Where(up => up.UserId == userId && up.IsGranted)
            .Select(up => up.PermissionId)
            .ToListAsync();
    }

    /// <summary>
    /// تحديث صلاحيات دور
    /// </summary>
    public async Task UpdateRolePermissionsAsync(Guid roleId, List<Guid> permissionIds)
    {
        // الحصول على الصلاحيات الحالية
        var currentPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        // إزالة الصلاحيات الموجودة
        _context.RolePermissions.RemoveRange(currentPermissions);

        // إضافة الصلاحيات الجديدة
        foreach (var permissionId in permissionIds)
        {
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                IsGranted = true
            });
        }

        await _context.SaveChangesAsync();

        // مسح Cache لجميع المستخدمين في هذا الدور
        var userIdsInRole = await GetUserIdsInRoleAsync(roleId);
        foreach (var userId in userIdsInRole)
        {
            PermissionAuthorizationHandler.InvalidateUserPermissionsCache(_cache, userId);
        }
    }

    /// <summary>
    /// تحديث صلاحيات مستخدم مباشرة
    /// </summary>
    public async Task UpdateUserPermissionsAsync(Guid userId, List<Guid> permissionIds)
    {
        // الحصول على الصلاحيات الحالية
        var currentPermissions = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .ToListAsync();

        // إزالة الصلاحيات الموجودة
        _context.UserPermissions.RemoveRange(currentPermissions);

        // إضافة الصلاحيات الجديدة
        foreach (var permissionId in permissionIds)
        {
            _context.UserPermissions.Add(new UserPermission
            {
                UserId = userId,
                PermissionId = permissionId,
                IsGranted = true
            });
        }

        await _context.SaveChangesAsync();

        // مسح Cache للمستخدم
        PermissionAuthorizationHandler.InvalidateUserPermissionsCache(_cache, userId);
    }

    /// <summary>
    /// إضافة صلاحية مباشرة لمستخدم (استثناء أو منح)
    /// </summary>
    public async Task AddUserPermissionOverrideAsync(Guid userId, Guid permissionId, bool isGranted)
    {
        // التحقق من وجود الصلاحية مسبقاً
        var existing = await _context.UserPermissions
            .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

        if (existing != null)
        {
            existing.IsGranted = isGranted;
        }
        else
        {
            _context.UserPermissions.Add(new UserPermission
            {
                UserId = userId,
                PermissionId = permissionId,
                IsGranted = isGranted
            });
        }

        await _context.SaveChangesAsync();

        // مسح Cache للمستخدم
        PermissionAuthorizationHandler.InvalidateUserPermissionsCache(_cache, userId);
    }

    /// <summary>
    /// إزالة صلاحية مباشرة من مستخدم
    /// </summary>
    public async Task RemoveUserPermissionOverrideAsync(Guid userId, Guid permissionId)
    {
        var permission = await _context.UserPermissions
            .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

        if (permission != null)
        {
            _context.UserPermissions.Remove(permission);
            await _context.SaveChangesAsync();

            // مسح Cache للمستخدم
            PermissionAuthorizationHandler.InvalidateUserPermissionsCache(_cache, userId);
        }
    }

    /// <summary>
    /// الحصول على معرفات المستخدمين في دور معين
    /// </summary>
    private async Task<List<Guid>> GetUserIdsInRoleAsync(Guid roleId)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
        {
            return new List<Guid>();
        }

        // الحصول على المستخدمين في هذا الدور من خلال جدول UserRoles
        var userRoles = await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return userRoles;
    }

    /// <summary>
    /// الحصول على صلاحيات المستخدم الفعالة (مع التحقق من الدور)
    /// </summary>
    public async Task<HashSet<string>> GetUserEffectivePermissionsAsync(Guid userId)
    {
        var effectivePermissions = new HashSet<string>();

        // الحصول على صلاحيات الأدوار من خلال UserRoles
        var userRoleIds = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        if (userRoleIds.Any())
        {
            var rolePermissions = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => userRoleIds.Contains(rp.RoleId) && rp.IsGranted && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();

            foreach (var permission in rolePermissions)
            {
                effectivePermissions.Add(permission);
            }
        }

        // الحصول على الصلاحيات المباشرة
        var directPermissions = await _context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == userId && up.Permission.IsActive)
            .ToListAsync();

        foreach (var userPermission in directPermissions)
        {
            if (userPermission.IsGranted)
            {
                effectivePermissions.Add(userPermission.Permission.Code);
            }
            else
            {
                effectivePermissions.Remove(userPermission.Permission.Code);
            }
        }

        return effectivePermissions;
    }
}
