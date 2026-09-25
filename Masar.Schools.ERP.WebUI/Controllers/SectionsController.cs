using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.WebUI.ViewModels;

namespace Masar.Schools.ERP.WebUI.Controllers
{
    [Authorize]
    public class SectionsController : Controller
    {
        private readonly MasarDbContext _context;
        private readonly ILogger<SectionsController> _logger;

        public SectionsController(MasarDbContext context, ILogger<SectionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sections
        public async Task<IActionResult> Index()
        {
            try
            {
                var sections = await _context.Sections
                    .Where(s => !s.IsDeleted)
                    .OrderBy(s => s.DisplayOrder)
                    .ThenBy(s => s.Name)
                    .ToListAsync();

                return View(sections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading sections");
                return View("Error");
            }
        }

        // GET: Sections/Create
        public IActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: Sections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Section section)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateModal", section);
            }

            try
            {
                // Check if code already exists
                var existingCode = await _context.Sections
                    .Where(s => s.Code == section.Code && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingCode != null)
                {
                    ModelState.AddModelError("Code", "هذا الكود مستخدم بالفعل");
                    return PartialView("_CreateModal", section);
                }

                section.Id = Guid.NewGuid();
                section.CreatedAt = DateTime.UtcNow;
                section.UpdatedAt = DateTime.UtcNow;
                section.CreatedBy = User.Identity?.Name ?? "System";
                section.UpdatedBy = User.Identity?.Name ?? "System";
                section.IsActive = true;
                section.IsDeleted = false;

                _context.Sections.Add(section);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم إضافة الشعبة بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating section");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة الشعبة");
                return PartialView("_CreateModal", section);
            }
        }

        // GET: Sections/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var section = await _context.Sections
                    .Where(s => s.Id == id && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (section == null)
                {
                    return NotFound();
                }

                return PartialView("_EditModal", section);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading section for edit");
                return View("Error");
            }
        }

        // POST: Sections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Section section)
        {
            if (id != section.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_EditModal", section);
            }

            try
            {
                var existing = await _context.Sections
                    .Where(s => s.Id == id && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existing == null)
                {
                    return NotFound();
                }

                // Check if code already exists (excluding current record)
                var existingCode = await _context.Sections
                    .Where(s => s.Code == section.Code && s.Id != id && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingCode != null)
                {
                    ModelState.AddModelError("Code", "هذا الكود مستخدم بالفعل");
                    return PartialView("_EditModal", section);
                }

                existing.Name = section.Name;
                existing.NameArabic = section.NameArabic;
                existing.Code = section.Code;
                existing.DisplayOrder = section.DisplayOrder;
                existing.IsActive = section.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = User.Identity?.Name ?? "System";

                _context.Sections.Update(existing);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم تحديث الشعبة بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating section");
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث الشعبة");
                return PartialView("_EditModal", section);
            }
        }

        // GET: Sections/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var section = await _context.Sections
                    .Where(s => s.Id == id && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (section == null)
                {
                    return NotFound();
                }

                // Check if section is used by any classrooms
                var hasClassrooms = await _context.ClassRooms
                    .Where(c => c.SectionId == id && !c.IsDeleted)
                    .AnyAsync();

                if (hasClassrooms)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذه الشعبة لأنها مستخدمة في فصول دراسية" });
                }

                return PartialView("_DeleteModal", section);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading section for delete");
                return Json(new { success = false, message = "حدث خطأ أثناء تحميل الشعبة" });
            }
        }

        // POST: Sections/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var section = await _context.Sections
                    .Where(s => s.Id == id && !s.IsDeleted)
                    .FirstOrDefaultAsync();

                if (section == null)
                {
                    return Json(new { success = false, message = "الشعبة غير موجودة" });
                }

                // Check if section is used by any classrooms
                var hasClassrooms = await _context.ClassRooms
                    .Where(c => c.SectionId == id && !c.IsDeleted)
                    .AnyAsync();

                if (hasClassrooms)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذه الشعبة لأنها مستخدمة في فصول دراسية" });
                }

                section.IsDeleted = true;
                section.DeletedAt = DateTime.UtcNow;
                section.UpdatedAt = DateTime.UtcNow;
                section.UpdatedBy = User.Identity?.Name ?? "System";

                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف الشعبة بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting section");
                return Json(new { success = false, message = "حدث خطأ أثناء حذف الشعبة" });
            }
        }
    }
}
