using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.WebUI.ViewComponents;

public class TenantNameViewComponent : ViewComponent
{
    private readonly MasarDbContext _context;

    public TenantNameViewComponent(MasarDbContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var tenant = _context.Tenants
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefault();

        return Content(tenant?.NameArabic ?? "نظام مَسَار للمدارس");
    }
}