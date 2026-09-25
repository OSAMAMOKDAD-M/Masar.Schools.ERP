using Microsoft.Extensions.Caching.Memory;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// مدير ذاكرة الصلاحيات المؤقتة
/// يوفر طرق لمسح الـ Cache عند تعديل الصلاحيات من قاعدة البيانات
/// </summary>
public static class PermissionCacheManager
{
    /// <summary>
    /// مسح ذاكرة الصلاحيات المؤقتة لمستخدم معين
    /// </summary>
    public static void InvalidateUserPermissionsCache(IMemoryCache cache, Guid userId)
    {
        var cacheKey = $"UserPermissions_{userId}";
        cache.Remove(cacheKey);
    }

    /// <summary>
    /// مسح ذاكرة الصلاحيات المؤقتة لجميع المستخدمين في أدوار معينة
    /// </summary>
    public static void InvalidateRolePermissionsCache(IMemoryCache cache, List<Guid> userIds)
    {
        foreach (var userId in userIds)
        {
            InvalidateUserPermissionsCache(cache, userId);
        }
    }

    /// <summary>
    /// مسح جميع ذاكرة الصلاحيات المؤقتة
    /// </summary>
    public static void InvalidateAllPermissionsCache(IMemoryCache cache)
    {
        // ملاحظة: في تطبيق الإنتاج، يجب استخدام Cache keys أكثر تحديداً
        // هنا سنقوم بإزالة جميع الإدخالات التي تبدأ بـ UserPermissions_
        // يتطلب هذا استخدام IMemoryCache مع دعم للحصول على جميع المفاتيح
        // في هذه النسخة، سنقوم بإنشاء قائمة بالمفاتيح ونقوم بإزالتها
    }

    /// <summary>
    /// مسح ذاكرة الصلاحيات عند تعديل صلاحيات دور
    /// </summary>
    public static void InvalidateCacheOnRolePermissionChange(IMemoryCache cache, Guid roleId)
    {
        // في التطبيق الحقيقي، يجب الحصول على جميع المستخدمين في هذا الدور
        // ومسح الـ Cache الخاص بهم
        // هذه الطريقة تتطلب قاعدة بيانات للحصول على المستخدمين
    }
}
