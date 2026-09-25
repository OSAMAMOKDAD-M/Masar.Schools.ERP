using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace CheckUserPermissions;

/// <summary>
/// أداة للتحقق من صلاحيات مستخدم معين
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        
        // إضافة الـ DbContext
        serviceCollection.AddDbContext<MasarDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MasarSchoolsERP;Trusted_Connection=True;MultipleActiveResultSets=true"));
        
        // إضافة UserManager
        serviceCollection.AddIdentity<MasarUser, MasarRole>()
            .AddEntityFrameworkStores<MasarDbContext>()
            .AddDefaultTokenProviders();
        
        // إضافة الـ Logger
        serviceCollection.AddLogging();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        try
        {
            var context = serviceProvider.GetRequiredService<MasarDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<MasarUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            
            Console.WriteLine("بدء التحقق من صلاحيات المستخدم admin@masar.com...");
            
            // الحصول على المستخدم
            var user = await userManager.FindByEmailAsync("admin@masar.com");
            
            if (user == null)
            {
                Console.WriteLine("المستخدم admin@masar.com غير موجود!");
                return;
            }
            
            Console.WriteLine($"تم العثور على المستخدم: {user.Email} (ID: {user.Id})");
            
            // الحصول على أدوار المستخدم
            var roles = await userManager.GetRolesAsync(user);
            Console.WriteLine($"الأدوار: {string.Join(", ", roles)}");
            
            // الحصول على صلاحيات الأدوار
            var roleIds = await context.Roles
                .Where(r => roles.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();
            
            var rolePermissions = await context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => roleIds.Contains(rp.RoleId) && rp.IsGranted && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();
            
            Console.WriteLine($"عدد صلاحيات الأدوار: {rolePermissions.Count}");
            
            // الحصول على الصلاحيات المباشرة للمستخدم
            var directPermissions = await context.UserPermissions
                .Include(up => up.Permission)
                .Where(up => up.UserId == user.Id && up.Permission.IsActive)
                .ToListAsync();
            
            var grantedDirectPermissions = directPermissions
                .Where(up => up.IsGranted)
                .Select(up => up.Permission.Code)
                .ToList();
            
            var deniedDirectPermissions = directPermissions
                .Where(up => !up.IsGranted)
                .Select(up => up.Permission.Code)
                .ToList();
            
            Console.WriteLine($"عدد الصلاحيات المباشرة الممنوحة: {grantedDirectPermissions.Count}");
            Console.WriteLine($"عدد الصلاحيات المباشرة المستثناة: {deniedDirectPermissions.Count}");
            
            // دمج الصلاحيات
            var effectivePermissions = new HashSet<string>(rolePermissions);
            
            foreach (var perm in grantedDirectPermissions)
            {
                effectivePermissions.Add(perm);
            }
            
            foreach (var perm in deniedDirectPermissions)
            {
                effectivePermissions.Remove(perm);
            }
            
            Console.WriteLine($"عدد الصلاحيات الفعالة: {effectivePermissions.Count}");
            
            // التحقق من بعض الصلاحيات المهمة
            var importantPermissions = new[]
            {
                "Students.View",
                "Financial.ClosePeriod",
                "Settings.ManagePermissions",
                "Settings.ManageRoles"
            };
            
            Console.WriteLine("\nالتحقق من الصلاحيات المهمة:");
            foreach (var permCode in importantPermissions)
            {
                var hasPermission = effectivePermissions.Contains(permCode);
                Console.WriteLine($"- {permCode}: {(hasPermission ? "✅ موجود" : "❌ غير موجود")}");
            }
            
            // التحقق من بعض الـ Controllers المهمة
            Console.WriteLine("\nالتحقق من الصلاحيات للـ Controllers:");
            var controllerPermissions = new[]
            {
                "Students.View",
                "Employees.View",
                "Attendance.View",
                "Financial.View",
                "Settings.View"
            };
            
            foreach (var permCode in controllerPermissions)
            {
                var hasPermission = effectivePermissions.Contains(permCode);
                Console.WriteLine($"- {permCode}: {(hasPermission ? "✅ موجود" : "❌ غير موجود")}");
            }
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            Console.WriteLine($"حدث خطأ: {ex.Message}");
            logger.LogError(ex, "حدث خطأ أثناء التحقق من الصلاحيات");
            throw;
        }
    }
}
