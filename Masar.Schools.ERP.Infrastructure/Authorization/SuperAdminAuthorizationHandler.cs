using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.Infrastructure.Authorization;

/// <summary>
/// معالج التفويض الخاص بالسوبر أدمن
/// يسمح للسوبر أدمن بالوصول إلى كل شيء بغض النظر عن الصلاحيات
/// </summary>
public class SuperAdminAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly MasarDbContext _context;
    private readonly UserManager<MasarUser> _userManager;

    public SuperAdminAuthorizationHandler(
        MasarDbContext context,
        UserManager<MasarUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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

        // الحصول على المستخدم من قاعدة البيانات
        var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null)
        {
            return;
        }

        // التحقق من أن المستخدم سوبر أدمن
        var isSuperAdmin = appUser.IsSuperAdmin || await _userManager.IsInRoleAsync(appUser, "SuperAdmin");
        
        if (isSuperAdmin)
        {
            // السوبر أدمن لديه كل الصلاحيات
            context.Succeed(requirement);
            return;
        }

        // إذا لم يكن سوبر أدمن، نستخدم المعالج العادي للصلاحيات
        // لكن لن نفعل ذلك هنا لأن هذا المعالج مخصص للسوبر أدمن فقط
    }
}
