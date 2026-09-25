using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة إنشاء مستخدم الأدمن عند التشغيل الأول
/// </summary>
public class AdminUserSeeder
{
    private readonly MasarDbContext _context;
    private readonly UserManager<MasarUser> _userManager;
    private readonly RoleManager<MasarRole> _roleManager;
    private readonly ILogger<AdminUserSeeder> _logger;

    public AdminUserSeeder(
        MasarDbContext context,
        UserManager<MasarUser> userManager,
        RoleManager<MasarRole> roleManager,
        ILogger<AdminUserSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAdminUserAsync()
    {
        try
        {
            _logger.LogInformation("بدء إنشاء مستخدم الأدمن...");

            // إنشاء Tenant افتراضي إذا لم يكن موجوداً
            var defaultTenant = await _context.Tenants.FirstOrDefaultAsync(t => !t.IsDeleted);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant
                {
                    Name = "مدرسة مَسَر الافتراضية",
                    NameArabic = "مدرسة مَسَر الافتراضية",
                    IsActive = true,
                    LicenseNumber = "DEFAULT"
                };
                _context.Tenants.Add(defaultTenant);
                await _context.SaveChangesAsync();
                _logger.LogInformation("تم إنشاء Tenant افتراضي بنجاح");
            }

            // إنشاء دور SuperAdmin إذا لم يكن موجوداً
            var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
            if (superAdminRole == null)
            {
                superAdminRole = new MasarRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    Description = "مدير النظام بصلاحيات كاملة"
                };
                var roleResult = await _roleManager.CreateAsync(superAdminRole);
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("تم إنشاء دور SuperAdmin بنجاح");
                }
                else
                {
                    _logger.LogError("فشل إنشاء دور SuperAdmin");
                    foreach (var error in roleResult.Errors)
                    {
                        _logger.LogError($"- {error.Description}");
                    }
                    return;
                }
            }

            // إنشاء مستخدم أدمن
            var adminUser = await _userManager.FindByEmailAsync("admin@masar.com");
            if (adminUser == null)
            {
                adminUser = new MasarUser
                {
                    UserName = "admin@masar.com",
                    Email = "admin@masar.com",
                    EmailConfirmed = true,
                    PhoneNumber = "01006765664",
                    FirstName = "مدير",
                    LastName = "النظام",
                    FullName = "مدير النظام",
                    IsActive = true,
                    TenantId = defaultTenant.Id
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123456");
                if (result.Succeeded)
                {
                    _logger.LogInformation("تم إنشاء مستخدم الأدمن بنجاح");
                    _logger.LogInformation("البريد الإلكتروني: admin@masar.com");
                    _logger.LogInformation("كلمة المرور: Admin@123456");
                }
                else
                {
                    _logger.LogError("فشل إنشاء المستخدم");
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"- {error.Description}");
                    }
                    return;
                }
            }
            else
            {
                _logger.LogInformation("مستخدم الأدمن موجود بالفعل");
            }

            // إضافة المستخدم إلى دور SuperAdmin
            if (!await _userManager.IsInRoleAsync(adminUser, "SuperAdmin"))
            {
                await _userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                _logger.LogInformation("تم إضافة المستخدم إلى دور SuperAdmin");
            }

            _logger.LogInformation("تم إنشاء مستخدم الأدمن بنجاح!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء إنشاء مستخدم الأدمن");
        }
    }
}