using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Authorization;

namespace AssignUserToSuperAdmin;

/// <summary>
/// أداة لإضافة مستخدم إلى دور SuperAdmin
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
        
        // إضافة Memory Cache لمسح الصلاحيات
        serviceCollection.AddMemoryCache();
        
        // إضافة الـ Logger
        serviceCollection.AddLogging();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        try
        {
            var context = serviceProvider.GetRequiredService<MasarDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<MasarUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            
            Console.WriteLine("بدء إضافة المستخدم إلى دور SuperAdmin...");
            logger.LogInformation("بدء إضافة المستخدم إلى دور SuperAdmin...");
            
            // الحصول على المستخدم
            var user = await userManager.FindByEmailAsync("admin@masar.com");
            
            if (user == null)
            {
                Console.WriteLine("المستخدم admin@masar.com غير موجود!");
                logger.LogError("المستخدم admin@masar.com غير موجود!");
                return;
            }
            
            Console.WriteLine($"تم العثور على المستخدم: {user.Email} (ID: {user.Id})");
            logger.LogInformation("تم العثور على المستخدم: {Email}", user.Email);
            
            // التحقق من وجود دور SuperAdmin
            var superAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            
            if (superAdminRole == null)
            {
                Console.WriteLine("دور SuperAdmin غير موجود!");
                logger.LogError("دور SuperAdmin غير موجود!");
                return;
            }
            
            Console.WriteLine($"تم العثور على دور SuperAdmin: {superAdminRole.Id}");
            logger.LogInformation("تم العثور على دور SuperAdmin: {RoleId}", superAdminRole.Id);
            
            // التحقق من أن المستخدم ليس بالفعل في الدور
            var isInRole = await userManager.IsInRoleAsync(user, "SuperAdmin");
            
            if (isInRole)
            {
                Console.WriteLine("المستخدم بالفعل في دور SuperAdmin!");
                logger.LogInformation("المستخدم بالفعل في دور SuperAdmin");
                return;
            }
            
            // إضافة المستخدم إلى الدور
            var result = await userManager.AddToRoleAsync(user, "SuperAdmin");
            
            if (result.Succeeded)
            {
                // مسح ذاكرة الصلاحيات المؤقتة للمستخدم
                var cache = serviceProvider.GetRequiredService<IMemoryCache>();
                PermissionAuthorizationHandler.InvalidateUserPermissionsCache(cache, user.Id);
                
                Console.WriteLine("تم بنجاح! المستخدم admin@masar.com الآن في دور SuperAdmin");
                Console.WriteLine("تم مسح ذاكرة الصلاحيات المؤقتة للمستخدم.");
                Console.WriteLine("المستخدم لديه الآن جميع الصلاحيات في النظام.");
                logger.LogInformation("تم إضافة المستخدم إلى دور SuperAdmin بنجاح");
                logger.LogInformation("تم مسح ذاكرة الصلاحيات المؤقتة للمستخدم");
            }
            else
            {
                Console.WriteLine("فشل إضافة المستخدم إلى الدور:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                    logger.LogError("خطأ: {Description}", error.Description);
                }
            }
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            Console.WriteLine($"حدث خطأ: {ex.Message}");
            logger.LogError(ex, "حدث خطأ أثناء إضافة المستخدم إلى الدور");
            throw;
        }
    }
}
