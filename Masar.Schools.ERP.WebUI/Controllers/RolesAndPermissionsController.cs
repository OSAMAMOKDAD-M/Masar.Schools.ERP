using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller إدارة الأدوار والصلاحيات
/// </summary>
[Authorize]
public class RolesAndPermissionsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly UserManager<MasarUser> _userManager;
    private readonly RoleManager<MasarRole> _roleManager;

    public RolesAndPermissionsController(
        MasarDbContext context,
        UserManager<MasarUser> userManager,
        RoleManager<MasarRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: /Settings/RolesAndPermissions
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .Where(u => !u.IsSuperAdmin)
            .OrderBy(u => u.FullName)
            .ToListAsync();

        var roles = await _roleManager.Roles
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync();

        var model = new RolesAndPermissionsIndexViewModel
        {
            Users = users,
            Roles = roles
        };

        return View(model);
    }

    // GET: /Settings/RolesAndPermissions/ManageUserPermissions?userId=xxx
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> ManageUserPermissions(string userId)
    {
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest("معرف المستخدم غير صالح");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var userPermissions = await _context.UserPermissions
            .Include(up => up.Permission)
            .Where(up => up.UserId == userGuid)
            .ToListAsync();

        var model = new ManageUserPermissionsViewModel
        {
            UserId = userId,
            UserName = user.FullName ?? user.Email ?? "Unknown",
            UserEmail = user.Email ?? string.Empty,
            CurrentRoles = userRoles.ToList(),
            CurrentPermissions = userPermissions.Select(up => up.Permission.Code).ToHashSet(),
            AvailablePresets = PermissionConstants.RolePresets.AllPresets.Keys.ToList()
        };

        return View(model);
    }

    // POST: /Settings/RolesAndPermissions/UpdateUserPermissions
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> UpdateUserPermissions(UpdateUserPermissionsDto model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ManageUserPermissions), new { userId = model.UserId });
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound();
        }

        // حذف الصلاحيات الحالية
        var existingPermissions = await _context.UserPermissions
            .Where(up => up.UserId == Guid.Parse(model.UserId))
            .ToListAsync();
        _context.UserPermissions.RemoveRange(existingPermissions);

        // إضافة الصلاحيات الجديدة
        if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
        {
            var permissions = await _context.Permissions
                .Where(p => model.SelectedPermissions.Contains(p.Code))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = Guid.Parse(model.UserId),
                    PermissionId = permission.Id,
                    IsGranted = true
                });
            }
        }

        await _context.SaveChangesAsync();

        // تحديث Security Stamp لإبطال الجلسة الحالية
        await _userManager.UpdateSecurityStampAsync(user);

        TempData["Success"] = "تم تحديث صلاحيات المستخدم بنجاح";
        return RedirectToAction(nameof(ManageUserPermissions), new { userId = model.UserId });
    }

    // GET: /Settings/RolesAndPermissions/GetRolePermissionsJson?roleId=xxx
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> GetRolePermissionsJson(string roleId)
    {
        var rolePermissions = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == Guid.Parse(roleId) && rp.IsGranted && rp.Permission.IsActive)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return Json(new { permissions = rolePermissions });
    }

    // GET: /Settings/RolesAndPermissions/GetPresetPermissionsJson?presetName=xxx
    [Authorize(Policy = "Settings.ManagePermissions")]
    public IActionResult GetPresetPermissionsJson(string presetName)
    {
        try
        {
            if (string.IsNullOrEmpty(presetName))
            {
                return Json(new { permissions = new List<string>(), error = "اسم القالب فارغ" });
            }

            if (PermissionConstants.RolePresets.AllPresets.TryGetValue(presetName, out var preset))
            {
                var allPermissions = preset.SelectMany(p => p.Value).Where(p => !string.IsNullOrEmpty(p)).ToList();
                return Json(new { permissions = allPermissions });
            }

            return Json(new { permissions = new List<string>(), error = $"القالب '{presetName}' غير موجود" });
        }
        catch (Exception ex)
        {
            return Json(new { permissions = new List<string>(), error = $"خطأ: {ex.Message}" });
        }
    }

    // GET: /Settings/RolesAndPermissions/ManageRolePermissions?roleId=xxx
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> ManageRolePermissions(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound();
        }

        var rolePermissions = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == Guid.Parse(roleId))
            .ToListAsync();

        var model = new ManageRolePermissionsViewModel
        {
            RoleId = roleId,
            RoleName = role.Name ?? string.Empty,
            RoleDescription = role.DescriptionArabic ?? role.Description ?? string.Empty,
            CurrentPermissions = rolePermissions.Where(rp => rp.IsGranted).Select(rp => rp.Permission.Code).ToHashSet(),
            ExcludedPermissions = rolePermissions.Where(rp => !rp.IsGranted).Select(rp => rp.Permission.Code).ToHashSet()
        };

        return View(model);
    }

    // POST: /Settings/RolesAndPermissions/UpdateRolePermissions
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> UpdateRolePermissions(UpdateRolePermissionsDto model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ManageRolePermissions), new { roleId = model.RoleId });
        }

        var role = await _roleManager.FindByIdAsync(model.RoleId);
        if (role == null)
        {
            return NotFound();
        }

        // حذف الصلاحيات الحالية
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == Guid.Parse(model.RoleId))
            .ToListAsync();
        _context.RolePermissions.RemoveRange(existingPermissions);

        // إضافة الصلاحيات الجديدة
        if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
        {
            var permissions = await _context.Permissions
                .Where(p => model.SelectedPermissions.Contains(p.Code))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = Guid.Parse(model.RoleId),
                    PermissionId = permission.Id,
                    IsGranted = true
                });
            }
        }

        // إضافة الصلاحيات المستثناة
        if (model.ExcludedPermissions != null && model.ExcludedPermissions.Any())
        {
            var excludedPermissions = await _context.Permissions
                .Where(p => model.ExcludedPermissions.Contains(p.Code))
                .ToListAsync();

            foreach (var permission in excludedPermissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = Guid.Parse(model.RoleId),
                    PermissionId = permission.Id,
                    IsGranted = false
                });
            }
        }

        await _context.SaveChangesAsync();

        // مسح ذاكرة الصلاحيات المؤقتة لجميع المستخدمين في هذا الدور
        var roleUsers = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);
        foreach (var user in roleUsers)
        {
            Infrastructure.Authorization.PermissionAuthorizationHandler.InvalidateUserPermissionsCache(
                HttpContext.RequestServices.GetRequiredService<IMemoryCache>(),
                user.Id);
        }

        TempData["Success"] = "تم تحديث صلاحيات الدور بنجاح";
        return RedirectToAction(nameof(ManageRolePermissions), new { roleId = model.RoleId });
    }
}

// ViewModels
public class RolesAndPermissionsIndexViewModel
{
    public List<MasarUser> Users { get; set; } = new();
    public List<MasarRole> Roles { get; set; } = new();
}

public class ManageUserPermissionsViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<string> CurrentRoles { get; set; } = new();
    public HashSet<string> CurrentPermissions { get; set; } = new();
    public List<string> AvailablePresets { get; set; } = new();
}

public class ManageRolePermissionsViewModel
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
    public HashSet<string> CurrentPermissions { get; set; } = new();
    public HashSet<string> ExcludedPermissions { get; set; } = new();
}

// DTOs
public class UpdateUserPermissionsDto
{
    public string UserId { get; set; } = string.Empty;
    public List<string>? SelectedPermissions { get; set; }
}

public class UpdateRolePermissionsDto
{
    public string RoleId { get; set; } = string.Empty;
    public List<string>? SelectedPermissions { get; set; }
    public List<string>? ExcludedPermissions { get; set; }
}