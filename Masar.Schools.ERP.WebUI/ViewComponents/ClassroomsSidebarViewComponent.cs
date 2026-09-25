using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.ViewComponents;

/// <summary>
/// ViewComponent لعرض الفصول في القائمة الجانبية من قاعدة البيانات
/// </summary>
public class ClassroomsSidebarViewComponent : ViewComponent
{
    private readonly MasarDbContext _context;

    public ClassroomsSidebarViewComponent(MasarDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var classrooms = await _context.ClassRooms
            .Where(c => !c.IsDeleted && c.IsActive)
            .OrderBy(c => c.NameArabic)
            .Select(c => new
            {
                c.Id,
                c.NameArabic,
                c.Code,
                c.GradeLevelLegacy
            })
            .ToListAsync();

        return View(classrooms);
    }
}
