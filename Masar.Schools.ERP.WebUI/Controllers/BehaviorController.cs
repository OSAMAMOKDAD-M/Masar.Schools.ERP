using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class BehaviorController : Controller
{
    private readonly MasarDbContext _context;

    public BehaviorController(MasarDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.ToListAsync();
        return View(students);
    }

    public async Task<IActionResult> CreateIncident(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }
}