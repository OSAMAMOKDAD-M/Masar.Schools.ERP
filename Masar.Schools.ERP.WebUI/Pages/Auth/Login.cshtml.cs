using Masar.Schools.ERP.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Masar.Schools.ERP.WebUI.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly SignInManager<MasarUser> _signInManager;
    private readonly UserManager<MasarUser> _userManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        SignInManager<MasarUser> signInManager,
        UserManager<MasarUser> userManager,
        ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; set; }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
        // Clear any existing error messages
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "يرجى التأكد من إدخال جميع البيانات المطلوبة";
            return Page();
        }

        // Find user by username or email
        var user = await _userManager.FindByNameAsync(Username) ?? 
                   await _userManager.FindByEmailAsync(Username);

        if (user == null)
        {
            ErrorMessage = "اسم المستخدم أو كلمة المرور غير صحيحة";
            _logger.LogWarning("Login attempt with non-existent user: {Username}", Username);
            return Page();
        }

        // Check if user is active
        if (!user.IsActive)
        {
            ErrorMessage = "حسابك غير نشط. يرجى التواصل مع الإدارة";
            return Page();
        }

        // Check if account is locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            ErrorMessage = "تم قفل حسابك بسبب محاولات دخول خاطئة متعددة. يرجى المحاولة لاحقاً";
            return Page();
        }

        // Attempt to sign in
        var result = await _signInManager.PasswordSignInAsync(
            user, 
            Password, 
            RememberMe, 
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in: {Username}", Username);
            
            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Redirect to home/index instead of dashboard
            return Redirect("/");
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { RememberMe = RememberMe });
        }

        if (result.IsLockedOut)
        {
            ErrorMessage = "تم قفل حسابك بسبب محاولات دخول خاطئة متعددة. يرجى المحاولة لاحقاً";
            _logger.LogWarning("User account locked out: {Username}", Username);
            return Page();
        }

        if (result.IsNotAllowed)
        {
            ErrorMessage = "غير مسموح لك بتسجيل الدخول. يرجى التواصل مع الإدارة";
            return Page();
        }

        ErrorMessage = "اسم المستخدم أو كلمة المرور غير صحيحة";
        _logger.LogWarning("Failed login attempt for user: {Username}", Username);
        return Page();
    }
}
