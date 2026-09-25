using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller إدارة الأدوار
/// </summary>
[Authorize]
public class RolesController : Controller
{
    private readonly MasarDbContext _context;
    private readonly RoleManager<MasarRole> _roleManager;

    public RolesController(
        MasarDbContext context,
        RoleManager<MasarRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    // GET: /Roles
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles
            .Where(r => r.Name != "SuperAdmin")
            .OrderBy(r => r.Name)
            .ToListAsync();

        return View(roles);
    }

    // GET: /Roles/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    // GET: /Roles/Create
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> Create()
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync();

        var model = new CreateRoleViewModel
        {
            TenantId = tenant?.Id ?? Guid.Empty
        };

        return View(model);
    }

    // POST: /Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var role = new MasarRole
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            DescriptionArabic = model.DescriptionArabic,
            TenantId = model.TenantId,
            IsActive = true
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        TempData["Success"] = "تم إنشاء الدور بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Roles/Edit/5
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        var model = new EditRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            DescriptionArabic = role.DescriptionArabic,
            IsActive = role.IsActive,
            TenantId = role.TenantId
        };

        return View(model);
    }

    // POST: /Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var role = await _roleManager.FindByIdAsync(model.Id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        role.Name = model.Name;
        role.Description = model.Description;
        role.DescriptionArabic = model.DescriptionArabic;
        role.IsActive = model.IsActive;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        TempData["Success"] = "تم تحديث الدور بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Roles/Delete/5
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        var model = new DeleteRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            DescriptionArabic = role.DescriptionArabic
        };

        return View(model);
    }

    // POST: /Roles/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            TempData["Success"] = "تم حذف الدور بنجاح";
        }
        else
        {
            TempData["Error"] = "حدث خطأ أثناء حذف الدور";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Roles/ManagePermissions/5
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> ManagePermissions(Guid id)
    {
        return RedirectToAction("ManageRolePermissions", "RolesAndPermissions", new { roleId = id.ToString() });
    }

    // POST: /Roles/CreateDefaultRoles
    [HttpPost]
    [Authorize(Policy = "Settings.ManageRoles")]
    public async Task<IActionResult> CreateDefaultRoles()
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
        if (tenant == null)
        {
            TempData["Error"] = "لم يتم العثور على Tenant";
            return RedirectToAction(nameof(Index));
        }

        // إنشاء الأدوار الافتراضية
        var roles = new[]
        {
            new { Name = "Teacher", Arabic = "المعلم", Description = "دور المعلم - يسمح بإدارة الفصول والدرجات والتواصل مع أولياء الأمور" },
            new { Name = "Employee", Arabic = "الموظف", Description = "دور الموظف - يسمح بإدارة الموارد البشرية والرواتب" },
            new { Name = "Controller", Arabic = "الكنترول", Description = "دور الكنترول - يسمح بالإدارة الكاملة للنظام" },
            new { Name = "Guardian", Arabic = "ولي الأمر", Description = "دور ولي الأمر - يسمح بمشاهدة بيانات الأبناء والتواصل مع المدرسة" }
        };

        var createdCount = 0;
        foreach (var roleInfo in roles)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleInfo.Name);
            if (existingRole == null)
            {
                var role = new MasarRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleInfo.Name,
                    NormalizedName = roleInfo.Name.ToUpper(),
                    Description = roleInfo.Description,
                    DescriptionArabic = roleInfo.Arabic,
                    TenantId = tenant.Id,
                    IsActive = true
                };

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    createdCount++;
                }
            }
        }

        TempData["Success"] = $"تم إنشاء {createdCount} دور جديد بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Roles/AssignDefaultPermissions
    [HttpPost]
    [Authorize(Policy = "Settings.ManagePermissions")]
    public async Task<IActionResult> AssignDefaultPermissions()
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
        if (tenant == null)
        {
            TempData["Error"] = "لم يتم العثور على Tenant";
            return RedirectToAction(nameof(Index));
        }

        // الصلاحيات الافتراضية لكل دور
        var rolePermissions = new Dictionary<string, string[]>
        {
            // دور المعلم - صلاحيات الفصول والدرجات والتواصل
            ["Teacher"] = new[]
            {
                "Students.View", "Students.ViewAcademic", "Grades.View", "Grades.Create", "Grades.Edit",
                "Attendance.View", "Attendance.TakeAttendance", "Classrooms.View", "Classrooms.AssignTeacher",
                "Timetable.View", "Timetable.ViewSchedule", "Chat.View", "Chat.SendMessage",
                "Behavior.View", "Behavior.RecordIncident", "Reports.View", "Reports.StudentReports"
            },
            
            // دور الموظف - صلاحيات الموارد البشرية والرواتب
            ["Employee"] = new[]
            {
                "Employees.View", "Employees.ViewAttendance", "Employees.ManageLeaves", "Employees.ViewSalary",
                "HR.EmployeesView", "HR.LeavesView", "HR.LeaveBalanceView", "HR.AttendanceView",
                "Attendance.View", "Attendance.ManageNotifications", "Settings.View"
            },
            
            // دور الكنترول - صلاحيات الإدارة الكاملة
            ["Controller"] = new[]
            {
                "Students.View", "Students.Create", "Students.Edit", "Students.Delete",
                "Employees.View", "Employees.Create", "Employees.Edit", "Employees.Delete",
                "Guardians.View", "Guardians.Create", "Guardians.Edit", "Guardians.Delete",
                "Transport.View", "Transport.ManageBuses", "Transport.ManageRoutes",
                "Canteen.View", "Canteen.ManageItems", "Canteen.ManageSales",
                "Inventory.View", "Inventory.ManageItems", "Inventory.ManageTransactions",
                "Financial.View", "Financial.ViewLedger", "Invoices.View", "Invoices.Create",
                "HR.Dashboard", "HR.EmployeesView", "HR.PayrollView", "HR.LeavesView",
                "Reports.View", "Reports.StudentReports", "Reports.FinancialReports", "Reports.AttendanceReports",
                "Settings.View", "Settings.ManageSchool", "Settings.ManageUsers", "Settings.ManageRoles"
            },
            
            // دور ولي الأمر - صلاحيات مشاهدة بيانات الأبناء والتواصل
            ["Guardian"] = new[]
            {
                "Students.View", "Students.ViewAcademic", "Grades.View",
                "Attendance.View", "Invoices.View", "Invoices.Pay",
                "Chat.View", "Chat.SendMessage"
            }
        };

        var assignedCount = 0;
        foreach (var rolePerm in rolePermissions)
        {
            var role = await _roleManager.FindByNameAsync(rolePerm.Key);
            if (role != null)
            {
                try
                {
                    // حذف الصلاحيات القديمة
                    var existingPermissions = await _context.RolePermissions
                        .Where(rp => rp.RoleId == role.Id)
                        .ToListAsync();
                    
                    if (existingPermissions.Any())
                    {
                        _context.RolePermissions.RemoveRange(existingPermissions);
                    }

                    // إضافة الصلاحيات الجديدة
                    foreach (var permissionCode in rolePerm.Value)
                    {
                        var permission = await _context.Permissions
                            .FirstOrDefaultAsync(p => p.Code == permissionCode);
                        
                        if (permission != null)
                        {
                            var rolePermission = new RolePermission
                            {
                                Id = Guid.NewGuid().GetHashCode(),
                                RoleId = role.Id,
                                PermissionId = permission.Id
                            };
                            _context.RolePermissions.Add(rolePermission);
                        }
                    }
                    
                    assignedCount++;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"حدث خطأ أثناء تعيين صلاحيات الدور {rolePerm.Key}: {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = $"تم تعيين الصلاحيات الافتراضية لـ {assignedCount} دور بنجاح";
        return RedirectToAction(nameof(Index));
    }
}

// ViewModels
public class CreateRoleViewModel
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
}

public class EditRoleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid TenantId { get; set; }
}

public class DeleteRoleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
}
