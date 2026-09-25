using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Masar.Schools.ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.Pages.Auth;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<MasarUser> _userManager;
    private readonly RoleManager<MasarRole> _roleManager;
    private readonly MasarDbContext _context;

    public ForgotPasswordModel(UserManager<MasarUser> userManager, RoleManager<MasarRole> roleManager, MasarDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون {2} إلى {1} حرف", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        public string LastName { get; set; } = string.Empty;
    }

    public bool ShowConfirmation { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool UserCreated { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            
            if (user == null)
            {
                // User doesn't exist - create new account
                // Get or create a default tenant
                var tenant = await _context.Tenants
                    .Where(t => !t.IsDeleted)
                    .FirstOrDefaultAsync();

                if (tenant == null)
                {
                    // Create a default tenant
                    tenant = new Tenant
                    {
                        Name = "الإدارة المركزية",
                        NameArabic = "الإدارة المركزية",
                        Email = Input.Email,
                        Phone = "01006765664",
                        IsActive = true
                    };
                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();
                }

                var newUser = new MasarUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    NormalizedUserName = Input.Email.ToUpper(),
                    NormalizedEmail = Input.Email.ToUpper(),
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    FullName = $"{Input.FirstName} {Input.LastName}",
                    EmailConfirmed = true,
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TenantId = tenant.Id  // Use the tenant's ID
                };

                var result = await _userManager.CreateAsync(newUser, Input.Password);

                if (result.Succeeded)
                {
                    // Create SuperAdmin role if it doesn't exist
                    var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
                    if (superAdminRole == null)
                    {
                        superAdminRole = new MasarRole
                        {
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN",
                            Description = "مسؤول النظام الرئيسي",
                            DescriptionArabic = "مسؤول النظام الرئيسي"
                        };
                        await _roleManager.CreateAsync(superAdminRole);
                    }

                    // Add user to SuperAdmin role
                    await _userManager.AddToRoleAsync(newUser, "SuperAdmin");

                    ShowConfirmation = true;
                    UserCreated = true;
                    Message = "تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.";
                    return Page();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            else
            {
                // User exists - update password
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, Input.Password);

                if (result.Succeeded)
                {
                    ShowConfirmation = true;
                    UserCreated = false;
                    Message = "تم تحديث كلمة المرور بنجاح. يمكنك الآن تسجيل الدخول بكلمة المرور الجديدة.";
                    return Page();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        return Page();
    }
}
