using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class TimetableController : Controller
{
    private readonly MasarDbContext _context;

    public TimetableController(MasarDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var classrooms = await _context.ClassRooms.ToListAsync();
        return View(classrooms);
    }

    public async Task<IActionResult> Generate()
    {
        var classrooms = await _context.ClassRooms.ToListAsync();
        return View(classrooms);
    }
}