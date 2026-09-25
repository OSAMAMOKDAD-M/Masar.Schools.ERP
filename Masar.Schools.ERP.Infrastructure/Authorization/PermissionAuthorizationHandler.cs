using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.Infrastructure.Authorization;

/// <summary>
/// معالج التفويض المخصص للصلاحيات
/// يتحقق من امتلاك المستخدم للصلاحية المطلوبة
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly MasarDbContext _context;
    private readonly UserManager<MasarUser> _userManager;
    private readonly IMemoryCache _cache;

    public PermissionAuthorizationHandler(
        MasarDbContext context,
        UserManager<MasarUser> userManager,
        IMemoryCache cache)
    {
        _context = context;
        _userManager = userManager;
        _cache = cache;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = context.User;

        // التحقق من أن المستخدم مسجل الدخول
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return;
        }

        // الحصول على معرف المستخدم
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return;
        }

        // الحصول على الصلاحيات الفعالة للمستخدم من قاعدة البيانات
        var userPermissions = await GetUserEffectivePermissionsAsync(userId);

        // التحقق من امتلاك الصلاحية المطلوبة
        if (userPermissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }

    /// <summary>
    /// الحصول على الصلاحيات الفعالة للمستخدم من قاعدة البيانات
    /// الصلاحيات = (صلاحيات الأدوار) + (الصلاحيات المباشرة) - (الصلاحيات المستثناة)
    /// هذه الطريقة تعتمد 100% على قاعدة البيانات - لا أدوار ثابتة
    /// </summary>
    private async Task<HashSet<string>> GetUserEffectivePermissionsAsync(string userId)
    {
        var userIdGuid = Guid.Parse(userId);
        var cacheKey = $"UserPermissions_{userIdGuid}";

        // محاولة الحصول من الذاكرة المؤقتة
        if (_cache.TryGetValue(cacheKey, out HashSet<string>? cachedPermissions))
        {
            return cachedPermissions ?? new HashSet<string>();
        }

        // الحصول على الصلاحيات من قاعدة البيانات
        var effectivePermissions = new HashSet<string>();

        // 1. الحصول على صلاحيات الأدوار المخصصة للمستخدم من قاعدة البيانات
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new HashSet<string>();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        
        if (userRoles.Any())
        {
            // الحصول على معرفات الأدور (Guid) من أسماء الأدوار
            var roleIds = await _context.Roles
                .Where(r => userRoles.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            // الحصول على صلاحيات الأدوار من قاعدة البيانات فقط
            var rolePermissions = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => roleIds.Contains(rp.RoleId) && rp.IsGranted && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();

            foreach (var permission in rolePermissions)
            {
                effectivePermissions.Add(permission);
            }
        }

        // 2. الحصول على الصلاحيات المباشرة للمستخدم من قاعدة البيانات
        var directPermissions = await _context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == userIdGuid && up.Permission.IsActive)
            .ToListAsync();

        foreach (var userPermission in directPermissions)
        {
            if (userPermission.IsGranted)
            {
                effectivePermissions.Add(userPermission.Permission.Code);
            }
            else
            {
                // إزالة الصلاحية المستثناة
                effectivePermissions.Remove(userPermission.Permission.Code);
            }
        }

        // تخزين في الذاكرة المؤقتة لمدة 15 دقيقة (تقليل المدة للتحديثات السريعة)
        _cache.Set(cacheKey, effectivePermissions, TimeSpan.FromMinutes(15));

        return effectivePermissions;
    }

    /// <summary>
    /// مسح ذاكرة الصلاحيات المؤقتة للمستخدم
    /// يستخدم عند تعديل الصلاحيات من قاعدة البيانات
    /// </summary>
    public static void InvalidateUserPermissionsCache(IMemoryCache cache, Guid userId)
    {
        var cacheKey = $"UserPermissions_{userId}";
        cache.Remove(cacheKey);
    }
}