using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

/// <summary>
/// سكريبت إنشاء الأدوار الجديدة مع الصلاحيات المناسبة
/// </summary>
public class CreateRolesWithPermissions
{
    public static async Task CreateRoles(MasarDbContext context, RoleManager<MasarRole> roleManager)
    {
        // الحصول على الـ Tenant الأول
        var tenant = await context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
        if (tenant == null)
        {
            Console.WriteLine("لم يتم العثور على Tenant");
            return;
        }

        // 1. دور المعلم (Teacher)
        await CreateRoleIfNotExists(roleManager, "Teacher", "TEACHER", tenant.Id);
        
        // 2. دور الموظف (Employee)
        await CreateRoleIfNotExists(roleManager, "Employee", "EMPLOYEE", tenant.Id);
        
        // 3. دور الكنترول (Controller)
        await CreateRoleIfNotExists(roleManager, "Controller", "CONTROLLER", tenant.Id);
        
        // 4. دور ولي الأمر (Guardian)
        await CreateRoleIfNotExists(roleManager, "Guardian", "GUARDIAN", tenant.Id);

        Console.WriteLine("تم إنشاء الأدوار الجديدة بنجاح:");
        Console.WriteLine("- Teacher - دور المعلم");
        Console.WriteLine("- Employee - دور الموظف");
        Console.WriteLine("- Controller - دور الكنترول");
        Console.WriteLine("- Guardian - دور ولي الأمر");
    }

    private static async Task CreateRoleIfNotExists(RoleManager<MasarRole> roleManager, string roleName, string normalizedName, Guid tenantId)
    {
        var existingRole = await roleManager.FindByNameAsync(roleName);
        if (existingRole == null)
        {
            var role = new MasarRole
            {
                Name = roleName,
                NormalizedName = normalizedName,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                TenantId = tenantId
            };

            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                Console.WriteLine($"تم إنشاء الدور: {roleName}");
            }
            else
            {
                Console.WriteLine($"فشل إنشاء الدور {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine($"الدور {roleName} موجود بالفعل");
        }
    }
}