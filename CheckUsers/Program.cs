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
    var context = serviceProvider.GetRequiredService<MasarDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<MasarUser>>();
    
    Console.WriteLine("=== المستخدمون الموجودون في قاعدة البيانات ===\n");
    
    // الحصول على جميع المستخدمين
    var users = await context.MasarUsers
        .Where(u => u.IsActive)
        .ToListAsync();
    
    if (users.Count == 0)
    {
        Console.WriteLine("❌ لا يوجد مستخدمين نشطين في قاعدة البيانات");
        Console.WriteLine("\nسأقوم بإنشاء مستخدم admin@masar.com كلمة المرور: Admin@123456");
        
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
        }
        
        // إنشاء دور SuperAdmin
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
        if (role == null)
        {
            role = new MasarRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Description = "مدير النظام بصلاحيات كاملة"
            };
            context.Roles.Add(role);
            await context.SaveChangesAsync();
        }
        
        // إنشاء المستخدم
        var newUser = new MasarUser
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
        
        var result = await userManager.CreateAsync(newUser, "Admin@123456");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "SuperAdmin");
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
        Console.WriteLine($"📊 عدد المستخدمين: {users.Count}\n");
        
        foreach (var user in users)
        {
            Console.WriteLine($"👤 اسم المستخدم: {user.UserName}");
            Console.WriteLine($"📧 البريد الإلكتروني: {user.Email}");
            Console.WriteLine($"🆔 المعرف: {user.Id}");
            Console.WriteLine($"✓ الحالة: {(user.IsActive ? "نشط" : "غير نشط")}");
            Console.WriteLine($"🏢 Tenant ID: {user.TenantId}");
            
            // الحصول على أدوار المستخدم
            var roles = await userManager.GetRolesAsync(user);
            Console.WriteLine($"🔑 الأدوار: {string.Join(", ", roles)}");
            
            Console.WriteLine("---");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ حدث خطأ: {ex.Message}");
    Console.WriteLine($"تفاصيل الخطأ: {ex.StackTrace}");
}