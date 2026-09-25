using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.AdminTools;

/// <summary>
/// أداة لإعطاء جميع الصلاحيات لدور SuperAdmin
/// </summary>
public class AssignSuperAdminPermissions
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
            var logger = serviceProvider.GetRequiredService<ILogger<AssignSuperAdminPermissions>>();
            
            logger.LogInformation("بدء إعطاء الصلاحيات لدور SuperAdmin...");
            
            // الحصول على دور SuperAdmin
            var superAdminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            
            if (superAdminRole == null)
            {
                logger.LogError("دور SuperAdmin غير موجود!");
                return;
            }
            
            logger.LogInformation("تم العثور على دور SuperAdmin: {RoleId}", superAdminRole.Id);
            
            // الحصول على جميع الصلاحيات
            var allPermissions = await context.Permissions.ToListAsync();
            logger.LogInformation("عدد الصلاحيات الموجودة: {Count}", allPermissions.Count);
            
            // الحصول على الصلاحيات الحالية للدور
            var existingRolePermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == superAdminRole.Id)
                .ToListAsync();
            
            logger.LogInformation("عدد الصلاحيات الحالية للدور: {Count}", existingRolePermissions.Count);
            
            // حذف الصلاحيات الحالية
            if (existingRolePermissions.Any())
            {
                context.RolePermissions.RemoveRange(existingRolePermissions);
                await context.SaveChangesAsync();
                logger.LogInformation("تم حذف الصلاحيات القديمة");
            }
            
            // إضافة جميع الصلاحيات الجديدة
            var newRolePermissions = allPermissions.Select(permission => new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = superAdminRole.Id,
                PermissionId = permission.Id,
                IsGranted = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }).ToList();
            
            await context.RolePermissions.AddRangeAsync(newRolePermissions);
            await context.SaveChangesAsync();
            
            logger.LogInformation("تم إضافة {Count} صلاحية جديدة لدور SuperAdmin", newRolePermissions.Count);
            logger.LogInformation("تم بنجاح! دور SuperAdmin لديه الآن جميع الصلاحيات في النظام.");
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<AssignSuperAdminPermissions>>();
            logger.LogError(ex, "حدث خطأ أثناء إعطاء الصلاحيات");
            throw;
        }
    }
}
