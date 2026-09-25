using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace CheckSuperAdminPermissions;

/// <summary>
/// أداة للتحقق من صلاحيات SuperAdmin
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        
        // إضافة الـ DbContext
        serviceCollection.AddDbContext<MasarDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MasarSchoolsERP;Trusted_Connection=True;MultipleActiveResultSets=true"));
        
        // إضافة الـ Logger
        serviceCollection.AddLogging();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        try
        {
            var context = serviceProvider.GetRequiredService<MasarDbContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            
            Console.WriteLine("بدء التحقق من صلاحيات SuperAdmin...");
            
            // الحصول على دور SuperAdmin
            var superAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            
            if (superAdminRole == null)
            {
                Console.WriteLine("دور SuperAdmin غير موجود!");
                return;
            }
            
            Console.WriteLine($"تم العثور على دور SuperAdmin: {superAdminRole.Id}");
            
            // الحصول على صلاحيات الدور
            var rolePermissions = await context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == superAdminRole.Id && rp.IsGranted)
                .ToListAsync();
            
            Console.WriteLine($"عدد صلاحيات الدور: {rolePermissions.Count}");
            
            // عرض الصلاحيات
            Console.WriteLine("\nالصلاحيات الموجودة:");
            foreach (var rp in rolePermissions.OrderBy(rp => rp.Permission?.Module).ThenBy(rp => rp.Permission?.Code))
            {
                Console.WriteLine($"- {rp.Permission?.Module}: {rp.Permission?.Code} ({rp.Permission?.NameArabic})");
            }
            
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
                var hasPermission = rolePermissions.Any(rp => rp.Permission?.Code == permCode);
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
