using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly SignInManager<MasarUser> _signInManager;
    private readonly UserManager<MasarUser> _userManager;
    private readonly RoleManager<MasarRole> _roleManager;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(
        UserManager<MasarUser> userManager,
        SignInManager<MasarUser> signInManager,
        RoleManager<MasarRole> roleManager,
        ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string ReturnUrl { get; set; } = "/";

    public class InputModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون {2} إلى {1} حرف", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/";
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        
        if (ModelState.IsValid)
        {
            var user = new MasarUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                NormalizedUserName = Input.Email.ToUpper(),
                NormalizedEmail = Input.Email.ToUpper(),
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                FullName = $"{Input.FirstName} {Input.LastName}",
                PhoneNumber = Input.PhoneNumber,
                EmailConfirmed = true,
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("تم إنشاء مستخدم جديد");

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
                await _userManager.AddToRoleAsync(user, "SuperAdmin");

                await _signInManager.SignInAsync(user, isPersistent: false);
                
                // Redirect to home/index instead of return URL
                return Redirect("/");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}
