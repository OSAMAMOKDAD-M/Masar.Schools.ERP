using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

// إعداد التكوين
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// إعداد Services
var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole());
services.AddDbContext<MasarDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddIdentity<MasarUser, MasarRole>()
    .AddEntityFrameworkStores<MasarDbContext>()
    .AddDefaultTokenProviders();

var serviceProvider = services.BuildServiceProvider();

// إنشاء المستخدم
using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MasarDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MasarUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<MasarRole>>();

    Console.WriteLine("جاري إنشاء المستخدم...");

    // إنشاء Tenant افتراضي
    var tenant = await context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
    if (tenant == null)
    {
        tenant = new Tenant
        {
            Name = "مدرسة مَسَر الافتراضية",
            NameArabic = "مدرسة مَسَر الافتراضية",
            IsActive = true,
            LicenseNumber = "DEFAULT"
        };
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();
        Console.WriteLine("تم إنشاء Tenant افتراضي");
    }

    // إنشاء دور SuperAdmin
    var role = await roleManager.FindByNameAsync("SuperAdmin");
    if (role == null)
    {
        role = new MasarRole
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Description = "مدير النظام بصلاحيات كاملة"
        };
        await roleManager.CreateAsync(role);
        Console.WriteLine("تم إنشاء دور SuperAdmin");
    }

    // إنشاء المستخدم
    var user = await userManager.FindByEmailAsync("admin@masar.com");
    if (user == null)
    {
        user = new MasarUser
        {
            UserName = "admin@masar.com",
            Email = "admin@masar.com",
            EmailConfirmed = true,
            PhoneNumber = "01006765664",
            FirstName = "مدير",
            LastName = "النظام",
            FullName = "مدير النظام",
            IsActive = true,
            TenantId = tenant.Id
        };

        var result = await userManager.CreateAsync(user, "Admin@123456");
        if (result.Succeeded)
        {
            Console.WriteLine("✅ تم إنشاء المستخدم بنجاح!");
            Console.WriteLine("📧 البريد الإلكتروني: admin@masar.com");
            Console.WriteLine("🔑 كلمة المرور: Admin@123456");
        }
        else
        {
            Console.WriteLine("❌ فشل إنشاء المستخدم:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("⚠️ المستخدم موجود بالفعل");
    }

    // إضافة المستخدم إلى الدور
    if (!await userManager.IsInRoleAsync(user, "SuperAdmin"))
    {
        await userManager.AddToRoleAsync(user, "SuperAdmin");
        Console.WriteLine("✅ تم إضافة المستخدم إلى دور SuperAdmin");
    }
}

Console.WriteLine("====================================");
Console.WriteLine("تم الانتهاء!");
Console.WriteLine("====================================");
