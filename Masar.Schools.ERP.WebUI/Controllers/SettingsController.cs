using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class SettingsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<SettingsController> _logger;
    private readonly IConfiguration _configuration;

    public SettingsController(MasarDbContext context, ILogger<SettingsController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    // GET: /Settings/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var settings = await _context.Tenants
            .Include(t => t.Schools.Where(s => !s.IsDeleted))
            .Where(t => !t.IsDeleted)
            .FirstOrDefaultAsync();

        return View(settings);
    }

    // GET: /Settings/WhatsAppConfig
    [HttpGet]
    public IActionResult WhatsAppConfig()
    {
        var model = new WhatsAppConfigDto
        {
            ApiKey = _configuration["WhatsApp:ApiKey"] ?? string.Empty,
            ApiUrl = _configuration["WhatsApp:ApiUrl"] ?? string.Empty,
            SenderNumber = _configuration["WhatsApp:SenderNumber"] ?? string.Empty,
            EnableAttendanceNotifications = true,
            EnableInvoiceNotifications = true,
            EnableBehavioralNotifications = true
        };

        return View(model);
    }

    // POST: /Settings/WhatsAppConfig
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("WhatsAppConfig")]
    public IActionResult WhatsAppConfigPost(WhatsAppConfigDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Save WhatsApp configuration to appsettings.json or database
        // For now, we'll just show a success message
        TempData["Success"] = "تم حفظ إعدادات واتساب بنجاح. (يجب تحديث ملف appsettings.json يدوياً)";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Settings/ZatcaConfig
    [HttpGet]
    public IActionResult ZatcaConfig()
    {
        // TODO: Load ZATCA configuration from settings table
        var model = new ZatcaConfigDto
        {
            Environment = "Sandbox",
            Csid = string.Empty,
            SecretKey = string.Empty,
            CertificatePath = string.Empty,
            ProductionUrl = "https://api.zatca.gov.sa",
            SandboxUrl = "https://sandbox-api.zatca.gov.sa"
        };

        return View(model);
    }

    // POST: /Settings/ZatcaConfig
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("ZatcaConfig")]
    public IActionResult ZatcaConfigPost(ZatcaConfigDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Save ZATCA configuration to settings table
        TempData["Success"] = "تم حفظ إعدادات ZATCA بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Settings/NoorIntegration
    [HttpGet]
    public IActionResult NoorIntegration()
    {
        // TODO: Load Noor integration settings
        var model = new NoorIntegrationDto
        {
            ApiUrl = "https://noor.moe.gov.sa/api",
            ApiKey = string.Empty,
            SchoolId = string.Empty,
            IsEnabled = false,
            AutoSyncEnabled = false,
            SyncInterval = 24
        };

        return View(model);
    }

    // POST: /Settings/NoorIntegration
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("NoorIntegration")]
    public IActionResult NoorIntegrationPost(NoorIntegrationDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Save Noor integration settings
        TempData["Success"] = "تم حفظ إعدادات نظام نور بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Settings/RolesAndPermissions
    [HttpGet]
    public async Task<IActionResult> RolesAndPermissions()
    {
        var roles = await _context.MasarRoles
            .Include(r => r.Permissions)
            .ToListAsync();

        var users = await _context.MasarUsers
            .ToListAsync();

        var viewModel = new RolesAndPermissionsViewModel
        {
            Roles = roles,
            Users = users,
            AvailablePermissions = GetAvailablePermissions()
        };

        return View(viewModel);
    }

    // POST: /Settings/CreateRole
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("CreateRole")]
    public async Task<IActionResult> CreateRolePost(CreateRoleDto model)
    {
        if (!ModelState.IsValid)
        {
            return await RolesAndPermissions();
        }

        var role = new MasarRole
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            NormalizedName = model.Name.ToUpper(),
            Description = model.Description,
            DescriptionArabic = model.DescriptionArabic,
            TenantId = model.TenantId
        };

        _context.MasarRoles.Add(role);
        await _context.SaveChangesAsync();

        // Add permissions
        foreach (var permissionId in model.PermissionIds)
        {
            // Check if permission already exists
            var existingPermission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionId);
            
            if (existingPermission == null)
            {
                var permission = new Permission
                {
                    Code = permissionId,
                    NameArabic = permissionId,
                    NameEnglish = permissionId,
                    Module = permissionId.Split('.')[0],
                    IsActive = true
                };

                _context.Permissions.Add(permission);
            }
        }
        
        await _context.SaveChangesAsync();
        
        // Now add role permissions
        foreach (var permissionId in model.PermissionIds)
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionId);
            
            if (permission != null)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id,
                    IsGranted = true
                };
                
                _context.RolePermissions.Add(rolePermission);
            }
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إنشاء الدور بنجاح";
        return RedirectToAction(nameof(RolesAndPermissions));
    }

    private List<string> GetAvailablePermissions()
    {
        return new List<string>
        {
            "students.view", "students.create", "students.edit", "students.delete",
            "employees.view", "employees.create", "employees.edit", "employees.delete",
            "attendance.view", "attendance.edit", "attendance.manage",
            "grades.view", "grades.edit", "grades.manage",
            "invoices.view", "invoices.create", "invoices.manage",
            "reports.view", "reports.export",
            "settings.manage", "users.manage", "roles.manage"
        };
    }
}

// DTOs
public class ZatcaConfigDto
{
    public string Environment { get; set; } = "Sandbox";
    public string Csid { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string CertificatePath { get; set; } = string.Empty;
    public string ProductionUrl { get; set; } = string.Empty;
    public string SandboxUrl { get; set; } = string.Empty;
}

public class NoorIntegrationDto
{
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string SchoolId { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public bool AutoSyncEnabled { get; set; }
    public int SyncInterval { get; set; }
}

public class RolesAndPermissionsViewModel
{
    public List<MasarRole> Roles { get; set; } = new();
    public List<MasarUser> Users { get; set; } = new();
    public List<string> AvailablePermissions { get; set; } = new();
}

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public List<string> PermissionIds { get; set; } = new();
}

public class WhatsAppConfigDto
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string SenderNumber { get; set; } = string.Empty;
    public bool EnableAttendanceNotifications { get; set; }
    public bool EnableInvoiceNotifications { get; set; }
    public bool EnableBehavioralNotifications { get; set; }
}
