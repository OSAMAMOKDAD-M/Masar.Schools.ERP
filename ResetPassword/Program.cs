using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

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
    var userManager = serviceProvider.GetRequiredService<UserManager<MasarUser>>();
    
    Console.WriteLine("=== إعادة تعيين كلمة المرور ===\n");
    
    // إعادة تعيين كلمة المرور لـ admin@masar.com
    var user = await userManager.FindByEmailAsync("admin@masar.com");
    
    if (user != null)
    {
        // إزالة Lockout إذا كان موجود
        await userManager.ResetAccessFailedCountAsync(user);
        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
        
        // إعادة تعيين كلمة المرور
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, resetToken, "Admin@123456");
        
        if (result.Succeeded)
        {
            Console.WriteLine("✅ تم إعادة تعيين كلمة المرور بنجاح!");
            Console.WriteLine("📧 البريد الإلكتروني: admin@masar.com");
            Console.WriteLine("🔑 كلمة المرور الجديدة: Admin@123456");
            Console.WriteLine("\nيمكنك الآن تسجيل الدخول باستخدام هذه البيانات.");
        }
        else
        {
            Console.WriteLine("❌ فشل إعادة تعيين كلمة المرور:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("❌ المستخدم admin@masar.com غير موجود");
    }
    
    // إعادة تعيين كلمة المرور لـ superadmin@masar.sa
    Console.WriteLine("\n---\n");
    var superAdmin = await userManager.FindByEmailAsync("superadmin@masar.sa");
    
    if (superAdmin != null)
    {
        await userManager.ResetAccessFailedCountAsync(superAdmin);
        await userManager.SetLockoutEndDateAsync(superAdmin, DateTimeOffset.MinValue);
        
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(superAdmin);
        var result = await userManager.ResetPasswordAsync(superAdmin, resetToken, "SuperAdmin@123");
        
        if (result.Succeeded)
        {
            Console.WriteLine("✅ تم إعادة تعيين كلمة المرور بنجاح!");
            Console.WriteLine("📧 البريد الإلكتروني: superadmin@masar.sa");
            Console.WriteLine("🔑 كلمة المرور الجديدة: SuperAdmin@123");
        }
        else
        {
            Console.WriteLine("❌ فشل إعادة تعيين كلمة المرور:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ حدث خطأ: {ex.Message}");
    Console.WriteLine($"تفاصيل الخطأ: {ex.StackTrace}");
}