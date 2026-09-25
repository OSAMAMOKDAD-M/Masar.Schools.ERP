using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

var connectionString = "Server=(localdb)\\mssqllocaldb;Database=MasarSchoolsERP;Trusted_Connection=True;MultipleActiveResultSets=true";

var services = new ServiceCollection();

services.AddDbContext<MasarDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddIdentity<MasarUser, MasarRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<MasarDbContext>()
.AddDefaultTokenProviders();

services.AddLogging();

var serviceProvider = services.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MasarUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<MasarRole>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    // Check if user already exists
    var user = await userManager.FindByEmailAsync("admin@masar.com");
    
    if (user == null)
    {
        user = new MasarUser
        {
            UserName = "admin@masar.com",
            NormalizedUserName = "ADMIN@MASAR.COM",
            Email = "admin@masar.com",
            NormalizedEmail = "ADMIN@MASAR.COM",
            EmailConfirmed = true,
            PhoneNumber = "01006765664",
            FirstName = "مدير",
            LastName = "النظام",
            FullName = "مدير النظام",
            IsActive = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, "Admin@123456");
        if (result.Succeeded)
        {
            Console.WriteLine("✅ تم إنشاء المستخدم بنجاح");
            Console.WriteLine("📧 البريد: admin@masar.com");
            Console.WriteLine("🔑 كلمة المرور: Admin@123456");

            // Try to add to SuperAdmin role (might not exist, that's ok)
            try
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
                Console.WriteLine("✅ تم إضافة المستخدم إلى دور SuperAdmin");
            }
            catch
            {
                Console.WriteLine("⚠️ لم يتم العثور على دور SuperAdmin، لكن المستخدم تم إنشاؤه");
            }
        }
        else
        {
            Console.WriteLine("❌ فشل إنشاء المستخدم:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"   - {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("⚠️ المستخدم موجود بالفعل");
        Console.WriteLine("📧 البريد: admin@masar.com");

        // Reset password
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, "Admin@123456");
        if (result.Succeeded)
        {
            Console.WriteLine("✅ تم إعادة تعيين كلمة المرور");
            Console.WriteLine("🔑 كلمة المرور الجديدة: Admin@123456");
        }
        else
        {
            Console.WriteLine("❌ فشل إعادة تعيين كلمة المرور:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"   - {error.Description}");
            }
        }

        // Try to add to SuperAdmin role
        try
        {
            if (!await userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
                Console.WriteLine("✅ تم إضافة المستخدم إلى دور SuperAdmin");
            }
        }
        catch
        {
            Console.WriteLine("⚠️ لم يتم العثور على دور SuperAdmin");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ حدث خطأ: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}
