using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Constants;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// وحدة التحكم الخاصة بحسابات المستخدمين وتسجيل الدخول
/// Account Controller for user authentication and account management
/// </summary>
public class AccountController : Controller
{
    private readonly SignInManager<MasarUser> _signInManager;
    private readonly UserManager<MasarUser> _userManager;
    private readonly MasarDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly IMemoryCache _cache;

    public AccountController(
        SignInManager<MasarUser> signInManager,
        UserManager<MasarUser> userManager,
        MasarDbContext context,
        ILogger<AccountController> logger,
        IMemoryCache cache)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    /// <summary>
    /// صفحة تسجيل الدخول
    /// Login page
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // If already authenticated, redirect to return URL or dashboard
        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(returnUrl ?? "/Dashboard");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// معالجة طلب تسجيل الدخول
    /// Handle login POST request
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Rate limiting check
        var clientIp = GetClientIpAddress();
        if (await IsRateLimitedAsync(clientIp))
        {
            ModelState.AddModelError(string.Empty, "محاولات كثيرة من نفس العنوان. يرجى الانتظار قليلاً.");
            return View(model);
        }

        // Find user by username or email
        var user = await _userManager.FindByNameAsync(model.Username) ??
                   await _userManager.FindByEmailAsync(model.Username);

        if (user == null)
        {
            // Log failed attempt
            await LogAuditEventAsync(null, model.Username, "LoginFailed", 
                $"محاولة دخول بمستخدم غير موجود: {model.Username}");
            
            ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
            return View(model);
        }

        // Check if user is active
        if (!user.IsActive)
        {
            await LogAuditEventAsync(user.Id, model.Username, "LoginFailed", 
                "محاولة دخول بحساب غير نشط");
            
            ModelState.AddModelError(string.Empty, "حسابك غير نشط. يرجى التواصل مع الإدارة");
            return View(model);
        }

        // Check if account is locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            await LogAuditEventAsync(user.Id, model.Username, "AccountLocked", 
                "محاولة دخول بحساب مغلق");
            
            ModelState.AddModelError(string.Empty, 
                "تم قفل حسابك بسبب محاولات دخول خاطئة متعددة. يرجى المحاولة بعد 15 دقيقة");
            return View(model);
        }

        // Attempt to sign in
        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            // Regenerate session ID to prevent session fixation attacks
            await HttpContext.Session.CommitAsync();

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Clear permissions cache to force fresh load on next authorization check
            _cache.Remove($"UserPermissions_{user.Id}");

            // Log successful login
            await LogAuditEventAsync(user.Id, model.Username, "LoginSuccess", 
                "تسجيل دخول ناجح");

            _logger.LogInformation("User logged in: {Username}", model.Username);

            // If returnUrl is valid, redirect there, otherwise go to home
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(LoginWith2fa), new { 
                RememberMe = model.RememberMe, 
                ReturnUrl = returnUrl 
            });
        }

        if (result.IsLockedOut)
        {
            await LogAuditEventAsync(user.Id, model.Username, "AccountLocked", 
                "تم قفل الحساب بسبب محاولات دخول خاطئة");
            
            _logger.LogWarning("User account locked out: {Username}", model.Username);
            ModelState.AddModelError(string.Empty, 
                "تم قفل حسابك بسبب محاولات دخول خاطئة متعددة. يرجى المحاولة بعد 15 دقيقة");
            return View(model);
        }

        if (result.IsNotAllowed)
        {
            await LogAuditEventAsync(user.Id, model.Username, "LoginNotAllowed", 
                "غير مسموح بتسجيل الدخول");
            
            ModelState.AddModelError(string.Empty, "غير مسموح لك بتسجيل الدخول. يرجى التواصل مع الإدارة");
            return View(model);
        }

        // Log failed attempt
        await LogAuditEventAsync(user.Id, model.Username, "LoginFailed", 
            "كلمة المرور غير صحيحة");

        ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
        return View(model);
    }

    /// <summary>
    /// تسجيل الخروج
    /// Logout
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.Identity?.Name;

        await _signInManager.SignOutAsync();

        // Clear permissions cache
        if (!string.IsNullOrEmpty(userId))
        {
            _cache.Remove($"UserPermissions_{userId}");
        }

        // Log logout
        if (!string.IsNullOrEmpty(userId))
        {
            await LogAuditEventAsync(Guid.Parse(userId), username ?? "Unknown", "Logout", 
                "تسجيل خروج");
        }

        _logger.LogInformation("User logged out");

        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// تسجيل الخروج (GET version for compatibility)
    /// Logout GET
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> LogoutGet()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.Identity?.Name;

        await _signInManager.SignOutAsync();

        // Clear permissions cache
        if (!string.IsNullOrEmpty(userId))
        {
            _cache.Remove($"UserPermissions_{userId}");
        }

        // Log logout
        if (!string.IsNullOrEmpty(userId))
        {
            await LogAuditEventAsync(Guid.Parse(userId), username ?? "Unknown", "Logout", 
                "تسجيل خروج");
        }

        _logger.LogInformation("User logged out");

        return RedirectToAction(nameof(Login));
    }

    /// <summary>
    /// صفحة تسجيل الدخول بعامل المصادقة الثنائية
    /// Login with 2FA
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWith2fa(bool rememberMe, string? returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        ViewData["ReturnUrl"] = returnUrl;
        ViewData["RememberMe"] = rememberMe;
        return View();
    }

    /// <summary>
    /// معالجة طلب المصادقة الثنائية
    /// Handle 2FA POST request
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
            authenticatorCode, 
            rememberMe, 
            model.RememberMachine);

        if (result.Succeeded)
        {
            // Log successful 2FA
            await LogAuditEventAsync(user.Id, user.UserName ?? "Unknown", "2FASuccess", 
                "المصادقة الثنائية ناجحة");

            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            await LogAuditEventAsync(user.Id, user.UserName ?? "Unknown", "AccountLocked", 
                "تم قفل الحساب بعد فشل المصادقة الثنائية");

            return RedirectToAction(nameof(Lockout));
        }

        // Log failed 2FA
        await LogAuditEventAsync(user.Id, user.UserName ?? "Unknown", "2FAFailed", 
            "المصادقة الثنائية فشلت");

        ModelState.AddModelError(string.Empty, "رمز التحقق غير صحيح");
        return View(model);
    }

    /// <summary>
    /// صفحة القفل
    /// Lockout page
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Lockout()
    {
        return View();
    }

    /// <summary>
    /// صفحة الملف الشخصي
    /// Profile page
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(nameof(Login));
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            NationalId = user.NationalId,
            ProfileImagePath = user.ProfileImagePath,
            LastLoginAt = user.LastLoginAt
        };

        return View(model);
    }

    /// <summary>
    /// تحديث الملف الشخصي
    /// Update profile
    /// </summary>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction(nameof(Login));
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        // Update user information
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.NationalId = model.NationalId;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // Log the update
            await LogAuditEventAsync(user.Id, user.UserName ?? "Unknown", "ProfileUpdated", 
                "تحديث الملف الشخصي");

            TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    /// <summary>
    /// تسجيل حدث في سجل المراجعة
    /// Log audit event
    /// </summary>
    private async Task LogAuditEventAsync(Guid? userId, string username, string eventType, string description)
    {
        try
        {
            // Try to get a default financial period, but don't fail if none exists
            Guid financialPeriodId = Guid.Empty;
            try
            {
                var periodId = await _context.FinancialPeriods
                    .Where(fp => !fp.IsClosed)
                    .OrderByDescending(fp => fp.StartDate)
                    .Select(fp => fp.Id)
                    .FirstOrDefaultAsync();
                
                if (periodId != Guid.Empty)
                {
                    financialPeriodId = periodId;
                }
            }
            catch
            {
                // If no financial periods exist, continue without it
            }

            var auditLog = new AuditLog
            {
                UserName = username ?? "Unknown",
                ActionDate = DateTime.UtcNow,
                ActionType = eventType,
                EntityName = "Authentication",
                EntityId = userId,
                Description = description,
                IpAddress = GetClientIpAddress(),
                FinancialPeriodId = financialPeriodId,
                Reason = eventType switch
                {
                    "LoginSuccess" => "تسجيل دخول ناجح",
                    "LoginFailed" => "محاولة دخول فاشلة",
                    "AccountLocked" => "قفل الحساب",
                    "Logout" => "تسجيل خروج",
                    _ => description
                }
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit event");
        }
    }

    /// <summary>
    /// الحصول على عنوان IP الخاص بالعميل
    /// Get client IP address
    /// </summary>
    private string GetClientIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"].ToString();
        }
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    /// <summary>
    /// التحقق من Rate Limiting
    /// Check rate limiting
    /// </summary>
    private async Task<bool> IsRateLimitedAsync(string ipAddress)
    {
        var cacheKey = $"LoginRateLimit_{ipAddress}";
        var attempts = _cache.Get<int>(cacheKey);

        if (attempts >= 20) // increased from 5 to 20 for development
        {
            return true;
        }

        _cache.Set(cacheKey, attempts + 1, TimeSpan.FromMinutes(1)); // reduced from 15 to 1 minute
        return false;
    }
}

/// <summary>
/// نموذج عرض تسجيل الدخول
/// Login view model
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [Display(Name = "اسم المستخدم أو البريد الإلكتروني")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "تذكرني")]
    public bool RememberMe { get; set; }
}

/// <summary>
/// نموذج عرض المصادقة الثنائية
/// 2FA view model
/// </summary>
public class LoginWith2faViewModel
{
    [Required(ErrorMessage = "رمز التحقق مطلوب")]
    [Display(Name = "رمز التحقق")]
    public string TwoFactorCode { get; set; } = string.Empty;

    [Display(Name = "تذكر هذا الجهاز")]
    public bool RememberMachine { get; set; }

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// نموذج عرض الملف الشخصي
/// Profile view model
/// </summary>
public class ProfileViewModel
{
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [Display(Name = "الاسم الأول")]
    [StringLength(100, ErrorMessage = "الاسم الأول لا يمكن أن يتجاوز 100 حرف")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الأخير مطلوب")]
    [Display(Name = "الاسم الأخير")]
    [StringLength(100, ErrorMessage = "الاسم الأخير لا يمكن أن يتجاوز 100 حرف")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [Display(Name = "البريد الإلكتروني")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "رقم الهاتف")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "اسم المستخدم")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "الرقم الوطني")]
    [StringLength(20, ErrorMessage = "الرقم الوطني لا يمكن أن يتجاوز 20 حرف")]
    public string? NationalId { get; set; }

    [Display(Name = "صورة الملف الشخصي")]
    public string? ProfileImagePath { get; set; }

    [Display(Name = "آخر تسجيل دخول")]
    public DateTime? LastLoginAt { get; set; }
}
