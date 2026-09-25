using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Services;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Infrastructure.Authorization;
using Masar.Schools.ERP.WebUI.Hubs;
using Masar.Schools.ERP.WebUI.ViewComponents;
using Masar.Schools.ERP.WebUI.Services;
using Masar.Schools.ERP.Licensing.Services;
using Masar.Schools.ERP.Licensing.Models;
using Masar.Schools.ERP.AI.Configuration;
using Masar.Schools.ERP.AI.Interfaces;
using Masar.Schools.ERP.AI.Services;
using Masar.Schools.ERP.AI.Workers;
using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Application.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to handle larger request headers (prevents 400 errors from large cookies)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestHeadersTotalSize = 64 * 1024; // 64KB
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<TenantNameViewComponent>();
builder.Services.AddScoped<Masar.Schools.ERP.WebUI.ViewComponents.ClassroomsSidebarViewComponent>();

// Add HttpClient
builder.Services.AddHttpClient();

// Add Memory Cache for permissions caching
builder.Services.AddMemoryCache();

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
});

// Configure Database
builder.Services.AddDbContext<MasarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Data Protection to persist keys to file system (prevents cookie corruption on restart)
var dataProtectionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MasarSchoolsERP", "DataProtection");
if (!Directory.Exists(dataProtectionPath))
{
    Directory.CreateDirectory(dataProtectionPath);
}

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath))
    .SetApplicationName("MasarSchoolsERP");

// Configure Identity
builder.Services.AddIdentity<MasarUser, MasarRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddEntityFrameworkStores<MasarDbContext>()
.AddDefaultTokenProviders();

// Configure Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LogoutPath = "/Auth/Logout";
    options.SlidingExpiration = true;
    options.Cookie.Name = "MasarSchools.Auth";
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.ReturnUrlParameter = "ReturnUrl";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Auth/Login");
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/Auth/AccessDenied");
            return Task.CompletedTask;
        }
    };
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // إضافة سياسات الصلاحيات الديناميكية فقط - لا أدوار ثابتة
    foreach (var permission in PermissionConstants.AllPermissions)
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});

// تسجيل معالج التفويض المخصص للصلاحيات
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Add SignalR
builder.Services.AddSignalR();

// Add Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MasarUser).Assembly));

// Register Infrastructure Services
builder.Services.AddScoped<IZatcaService, ZatcaService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<PermissionSeeder>();
builder.Services.AddScoped<PermissionManagementService>();
builder.Services.AddScoped<AttendanceAnalyticsService>();
builder.Services.AddScoped<FinancialClosingService>();
builder.Services.AddHostedService<AttendanceAutomationService>();
builder.Services.AddHostedService<PermissionSeedingService>();
// builder.Services.AddScoped<AdminUserSeeder>();
// builder.Services.AddHostedService<AdminUserSeedingService>();

// Register GPS Tracking Simulation Service
builder.Services.AddHostedService<GpsTrackingSimulationService>();

// Register Classroom Services
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Services.IClassroomLookupService, Masar.Schools.ERP.Infrastructure.Services.ClassroomLookupService>();

// Register Backup Services
builder.Services.AddScoped<IDatabaseBackupService, DatabaseBackupService>();
builder.Services.AddScoped<IFileBackupService, FileBackupService>();

// Register Licensing Services
builder.Services.AddSingleton<HardwareFingerprintService>();
builder.Services.AddSingleton<LicenseEngine>();
builder.Services.AddScoped<ILicenseService, LicenseService>();
// builder.Services.AddHostedService<LicenseValidationWorker>();

// Register Masar AI Services
builder.Services.Configure<MasarAIOptions>(
    builder.Configuration.GetSection(MasarAIOptions.SectionName));

builder.Services.AddScoped<IRiskScoringEngine, WeightedRiskScoringEngine>();
builder.Services.AddScoped<IPredictiveAnalyticsService, PredictiveAnalyticsService>();
builder.Services.AddScoped<IRiskAlertService, RiskAlertService>();
builder.Services.AddScoped<IScreenDiscoveryService, ScreenDiscoveryService>();
builder.Services.AddScoped<IMasarAiService, MasarAiService>();

// Register Transport Management Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.ITransportService, Masar.Schools.ERP.Infrastructure.Services.TransportService>();

// Register Leave Management Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.ILeaveService, Masar.Schools.ERP.Infrastructure.Services.LeaveService>();

// Register HR Services
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IHRContractService, Masar.Schools.ERP.Infrastructure.Services.HRContractService>();
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IHRPayrollService, Masar.Schools.ERP.Infrastructure.Services.HRPayrollService>();
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IHRAttendanceService, Masar.Schools.ERP.Infrastructure.Services.HRAttendanceService>();
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IHRGratuityService, Masar.Schools.ERP.Infrastructure.Services.HRGratuityService>();

// Register Academic Term Opening Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IAcademicTermOpeningService, Masar.Schools.ERP.Infrastructure.Services.AcademicTermOpeningService>();

// Register Term Closing Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.ITermClosingService, Masar.Schools.ERP.Infrastructure.Services.TermClosingService>();

// Register Inventory Management Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IInventoryService, Masar.Schools.ERP.Infrastructure.Services.InventoryService>();

// Register Procurement Management Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IProcurementService, Masar.Schools.ERP.Infrastructure.Services.ProcurementService>();

// Register Inventory Alert Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Services.InventoryAlertService>();

// Register Financial Integration Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Services.FinancialIntegrationService>();

// Register Document Service
builder.Services.AddScoped<Masar.Schools.ERP.Infrastructure.Interfaces.IDocumentService, Masar.Schools.ERP.Infrastructure.Services.DocumentService>();

// Register Canteen & POS Service
builder.Services.AddScoped<ICanteenService, CanteenService>();

// Register Admissions & Registration Service
builder.Services.AddScoped<IAdmissionService, AdmissionService>();

// Register School Clinic Service
builder.Services.AddScoped<IClinicService, ClinicService>();

// Register Alumni Management Service
builder.Services.AddScoped<IAlumniService, AlumniService>();

// Register Hangfire job for risk analysis
builder.Services.AddScoped<StudentRiskAnalyzerWorker>();

// Configure Licensing Options
builder.Services.Configure<LicenseOptions>(options =>
{
    options.LicenseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MasarSchoolsERP");
    // مفتاح عام RSA - يجب استبداله بالمفتاح العام الحقيقي من السوبر أدمن
    options.PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA..."; // تجريبي
});

var app = builder.Build();

// Seed database in development mode
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        
        try
        {
            await DbInitializer.Initialize(serviceProvider);
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Use Hangfire Dashboard
app.UseHangfireDashboard();

// Dashboard route
app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard",
    defaults: new { controller = "Home", action = "Index" });

// Default route - supports both /Controller and /Controller/Action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Account route
app.MapControllerRoute(
    name: "account",
    pattern: "Account/{action=Login}/{id?}",
    defaults: new { controller = "Account" });

app.MapRazorPages();

// Map SignalR hubs
app.MapHub<Masar.Schools.ERP.WebUI.Hubs.MasarChatHub>("/chathub");
app.MapHub<Masar.Schools.ERP.WebUI.Hubs.BusTrackingHub>("/bustrackinghub");

app.Run();
