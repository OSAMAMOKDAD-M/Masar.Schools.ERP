using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller إدارة المستخدمين
/// </summary>
[Authorize]
public class UsersController : Controller
{
    private readonly MasarDbContext _context;
    private readonly UserManager<MasarUser> _userManager;
    private readonly RoleManager<MasarRole> _roleManager;

    public UsersController(
        MasarDbContext context,
        UserManager<MasarUser> userManager,
        RoleManager<MasarRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET: /Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .Where(u => !u.IsSuperAdmin)
            .OrderBy(u => u.FullName)
            .ToListAsync();

        var roles = await _roleManager.Roles.ToListAsync();

        var model = new UsersIndexViewModel
        {
            Users = users,
            Roles = roles
        };

        return View(model);
    }

    // GET: /Users/Create
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> Create()
    {
        var tenants = await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();
        if (!tenants.Any())
        {
            TempData["Error"] = "لم يتم العثور على Tenant صالح. يرجى إنشاء Tenant أولاً.";
            return RedirectToAction("Index");
        }

        var tenant = tenants.First();
        var roles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();

        var model = new CreateUserViewModel
        {
            TenantId = tenant.Id,
            AvailableRoles = roles
        };

        return View(model);
    }

    // POST: /Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();
            return View(model);
        }

        // التأكد من وجود Tenant صالح
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == model.TenantId && !t.IsDeleted);
        if (tenant == null)
        {
            ModelState.AddModelError(string.Empty, $"Tenant غير صالح. TenantId المرسل: {model.TenantId}");
            model.AvailableRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();
            return View(model);
        }

        var user = new MasarUser
        {
            Id = Guid.NewGuid(),
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            FullNameArabic = model.FullNameArabic,
            PhoneNumber = model.PhoneNumber,
            TenantId = model.TenantId,
            IsActive = true,
            IsSuperAdmin = false
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            model.AvailableRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();
            return View(model);
        }

        // إضافة الأدوار المحددة ونسخ الصلاحيات
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            foreach (var roleId in model.SelectedRoles)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);

                    // نسخ الصلاحيات من الدور إلى المستخدم
                    var rolePermissions = await _context.RolePermissions
                        .Include(rp => rp.Permission)
                        .Where(rp => rp.RoleId == Guid.Parse(roleId) && rp.IsGranted)
                        .ToListAsync();

                    foreach (var rolePermission in rolePermissions)
                    {
                        // التحقق من أن الصلاحية غير موجودة مسبقاً للمستخدم
                        var existingUserPermission = await _context.UserPermissions
                            .FirstOrDefaultAsync(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId);

                        if (existingUserPermission == null)
                        {
                            _context.UserPermissions.Add(new UserPermission
                            {
                                UserId = user.Id,
                                PermissionId = rolePermission.PermissionId,
                                IsGranted = true
                            });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        TempData["Success"] = "تم إنشاء المستخدم بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Users/Edit/5
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            FullNameArabic = user.FullNameArabic,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            TenantId = user.TenantId,
            CurrentRoles = userRoles.ToList(),
            AvailableRoles = allRoles
        };

        return View(model);
    }

    // POST: /Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = model.FullName;
        user.FullNameArabic = model.FullNameArabic;
        user.PhoneNumber = model.PhoneNumber;
        user.IsActive = model.IsActive;

        if (model.Email != user.Email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.AvailableRoles = await _roleManager.Roles.Where(r => r.IsActive).ToListAsync();
                return View(model);
            }
            user.UserName = model.Email;
        }

        await _userManager.UpdateAsync(user);

        // تحديث الأدوار
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // حذف جميع الصلاحيات الحالية للمستخدم
        var existingUserPermissions = await _context.UserPermissions
            .Where(up => up.UserId == user.Id)
            .ToListAsync();
        _context.UserPermissions.RemoveRange(existingUserPermissions);

        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
            foreach (var roleId in model.SelectedRoles)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);

                    // نسخ الصلاحيات من الدور إلى المستخدم
                    var rolePermissions = await _context.RolePermissions
                        .Include(rp => rp.Permission)
                        .Where(rp => rp.RoleId == Guid.Parse(roleId) && rp.IsGranted)
                        .ToListAsync();

                    foreach (var rolePermission in rolePermissions)
                    {
                        _context.UserPermissions.Add(new UserPermission
                        {
                            UserId = user.Id,
                            PermissionId = rolePermission.PermissionId,
                            IsGranted = true
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        TempData["Success"] = "تم تحديث المستخدم بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Users/Delete/5
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var model = new DeleteUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            FullNameArabic = user.FullNameArabic
        };

        return View(model);
    }

    // POST: /Users/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageUsers")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Success"] = "تم حذف المستخدم بنجاح";
        }
        else
        {
            TempData["Error"] = "حدث خطأ أثناء حذف المستخدم";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Users/ManagePermissions/5
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> ManagePermissions(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("معرف المستخدم غير صالح");
        }

        return RedirectToAction("ManageUserPermissions", "RolesAndPermissions", new { userId = id.ToString() });
    }
}

// ViewModels
public class UsersIndexViewModel
{
    public List<MasarUser> Users { get; set; } = new();
    public List<MasarRole> Roles { get; set; } = new();
}

public class CreateUserViewModel
{
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string>? SelectedRoles { get; set; }
    public List<MasarRole> AvailableRoles { get; set; } = new();
}

public class EditUserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }
    public List<string> CurrentRoles { get; set; } = new();
    public List<string>? SelectedRoles { get; set; }
    public List<MasarRole> AvailableRoles { get; set; } = new();
}

public class DeleteUserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
}
