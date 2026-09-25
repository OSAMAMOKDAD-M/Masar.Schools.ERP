using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Masar.Schools.ERP.WebUI.Pages.Chat;

public class IndexModel : PageModel
{
    public string CurrentUserId { get; set; } = string.Empty;

    public void OnGet()
    {
        CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}