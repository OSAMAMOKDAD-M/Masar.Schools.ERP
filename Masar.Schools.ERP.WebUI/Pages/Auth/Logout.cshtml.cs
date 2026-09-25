using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Masar.Schools.ERP.WebUI.Pages.Auth;

[Authorize]
public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Auth/Login");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Auth/Login");
    }
}
