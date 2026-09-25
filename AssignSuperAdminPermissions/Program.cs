using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace AssignSuperAdminPermissions;

/// <summary>
/// أداة لإعطاء جميع الصلاحيات لدور SuperAdmin
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
            
            Console.WriteLine("بدء إعطاء الصلاحيات لدور SuperAdmin...");
            logger.LogInformation("بدء إعطاء الصلاحيات لدور SuperAdmin...");
            
            // الحصول على أول Tenant موجود أو إنشاء واحد افتراضي
            var tenant = await context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
            if (tenant == null)
            {
                Console.WriteLine("لا يوجد Tenant، جاري إنشاء Tenant افتراضي...");
                tenant = new Tenant
                {
                    Name = "الإدارة المركزية",
                    NameArabic = "الإدارة المركزية",
                    IsActive = true
                };
                context.Tenants.Add(tenant);
                await context.SaveChangesAsync();
                Console.WriteLine($"تم إنشاء Tenant: {tenant.Id}");
            }
            
            // الحصول على دور SuperAdmin أو إنشاؤه
            var superAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            
            if (superAdminRole == null)
            {
                // إنشاء دور SuperAdmin إذا لم يكن موجوداً
                Console.WriteLine("دور SuperAdmin غير موجود، جاري إنشائه...");
                logger.LogInformation("دور SuperAdmin غير موجود، جاري إنشائه...");
                
                superAdminRole = new MasarRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    Description = "مسؤول النظام الرئيسي",
                    DescriptionArabic = "مسؤول النظام الرئيسي",
                    IsActive = true,
                    TenantId = tenant.Id,
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                
                context.Roles.Add(superAdminRole);
                await context.SaveChangesAsync();
                
                Console.WriteLine($"تم إنشاء دور SuperAdmin: {superAdminRole.Id}");
                logger.LogInformation("تم إنشاء دور SuperAdmin: {RoleId}", superAdminRole.Id);
            }
            else
            {
                Console.WriteLine($"تم العثور على دور SuperAdmin: {superAdminRole.Id}");
                logger.LogInformation("تم العثور على دور SuperAdmin: {RoleId}", superAdminRole.Id);
            }
            
            // الحصول على جميع الصلاحيات
            var allPermissions = await context.Permissions.ToListAsync();
            Console.WriteLine($"عدد الصلاحيات الموجودة: {allPermissions.Count}");
            logger.LogInformation("عدد الصلاحيات الموجودة: {Count}", allPermissions.Count);
            
            // الحصول على الصلاحيات الحالية للدور
            var existingRolePermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == superAdminRole.Id)
                .ToListAsync();
            
            Console.WriteLine($"عدد الصلاحيات الحالية للدور: {existingRolePermissions.Count}");
            logger.LogInformation("عدد الصلاحيات الحالية للدور: {Count}", existingRolePermissions.Count);
            
            // حذف الصلاحيات الحالية
            if (existingRolePermissions.Any())
            {
                context.RolePermissions.RemoveRange(existingRolePermissions);
                await context.SaveChangesAsync();
                Console.WriteLine("تم حذف الصلاحيات القديمة");
                logger.LogInformation("تم حذف الصلاحيات القديمة");
            }
            
            // إضافة جميع الصلاحيات الجديدة
            var newRolePermissions = allPermissions.Select(permission => new RolePermission
            {
                RoleId = superAdminRole.Id,
                PermissionId = permission.Id,
                IsGranted = true
            }).ToList();
            
            await context.RolePermissions.AddRangeAsync(newRolePermissions);
            await context.SaveChangesAsync();
            
            Console.WriteLine($"تم إضافة {newRolePermissions.Count} صلاحية جديدة لدور SuperAdmin");
            Console.WriteLine("تم بنجاح! دور SuperAdmin لديه الآن جميع الصلاحيات في النظام.");
            logger.LogInformation("تم إضافة {Count} صلاحية جديدة لدور SuperAdmin", newRolePermissions.Count);
            logger.LogInformation("تم بنجاح! دور SuperAdmin لديه الآن جميع الصلاحيات في النظام.");
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            Console.WriteLine($"حدث خطأ: {ex.Message}");
            logger.LogError(ex, "حدث خطأ أثناء إعطاء الصلاحيات");
            throw;
        }
    }
}
