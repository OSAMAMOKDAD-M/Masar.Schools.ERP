using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.WebUI.ViewModels;

namespace Masar.Schools.ERP.WebUI.Controllers
{
    [Authorize]
    public class GradeLevelsController : Controller
    {
        private readonly MasarDbContext _context;
        private readonly ILogger<GradeLevelsController> _logger;

        public GradeLevelsController(MasarDbContext context, ILogger<GradeLevelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: GradeLevels
        public async Task<IActionResult> Index()
        {
            try
            {
                var gradeLevels = await _context.GradeLevels
                    .Where(g => !g.IsDeleted)
                    .OrderBy(g => g.DisplayOrder)
                    .ThenBy(g => g.Name)
                    .ToListAsync();

                return View(gradeLevels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading grade levels");
                return View("Error");
            }
        }

        // GET: GradeLevels/Create
        public IActionResult Create()
        {
            return PartialView("_CreateModal");
        }

        // POST: GradeLevels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeLevel gradeLevel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateModal", gradeLevel);
            }

            try
            {
                // Check if code already exists
                var existingCode = await _context.GradeLevels
                    .Where(g => g.Code == gradeLevel.Code && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingCode != null)
                {
                    ModelState.AddModelError("Code", "هذا الكود مستخدم بالفعل");
                    return PartialView("_CreateModal", gradeLevel);
                }

                gradeLevel.Id = Guid.NewGuid();
                gradeLevel.CreatedAt = DateTime.UtcNow;
                gradeLevel.UpdatedAt = DateTime.UtcNow;
                gradeLevel.CreatedBy = User.Identity?.Name ?? "System";
                gradeLevel.UpdatedBy = User.Identity?.Name ?? "System";
                gradeLevel.IsActive = true;
                gradeLevel.IsDeleted = false;

                _context.GradeLevels.Add(gradeLevel);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم إضافة المرحلة الدراسية بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade level");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة المرحلة الدراسية");
                return PartialView("_CreateModal", gradeLevel);
            }
        }

        // GET: GradeLevels/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var gradeLevel = await _context.GradeLevels
                    .Where(g => g.Id == id && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (gradeLevel == null)
                {
                    return NotFound();
                }

                return PartialView("_EditModal", gradeLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading grade level for edit");
                return View("Error");
            }
        }

        // POST: GradeLevels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, GradeLevel gradeLevel)
        {
            if (id != gradeLevel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_EditModal", gradeLevel);
            }

            try
            {
                var existing = await _context.GradeLevels
                    .Where(g => g.Id == id && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existing == null)
                {
                    return NotFound();
                }

                // Check if code already exists (excluding current record)
                var existingCode = await _context.GradeLevels
                    .Where(g => g.Code == gradeLevel.Code && g.Id != id && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingCode != null)
                {
                    ModelState.AddModelError("Code", "هذا الكود مستخدم بالفعل");
                    return PartialView("_EditModal", gradeLevel);
                }

                existing.Name = gradeLevel.Name;
                existing.NameArabic = gradeLevel.NameArabic;
                existing.Code = gradeLevel.Code;
                existing.DisplayOrder = gradeLevel.DisplayOrder;
                existing.IsActive = gradeLevel.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = User.Identity?.Name ?? "System";

                _context.GradeLevels.Update(existing);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم تحديث المرحلة الدراسية بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade level");
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث المرحلة الدراسية");
                return PartialView("_EditModal", gradeLevel);
            }
        }

        // GET: GradeLevels/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var gradeLevel = await _context.GradeLevels
                    .Where(g => g.Id == id && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (gradeLevel == null)
                {
                    return NotFound();
                }

                // Check if grade level is used by any classrooms
                var hasClassrooms = await _context.ClassRooms
                    .Where(c => c.GradeLevelId == id && !c.IsDeleted)
                    .AnyAsync();

                if (hasClassrooms)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذه المرحلة لأنها مستخدمة في فصول دراسية" });
                }

                return PartialView("_DeleteModal", gradeLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading grade level for delete");
                return Json(new { success = false, message = "حدث خطأ أثناء تحميل المرحلة الدراسية" });
            }
        }

        // POST: GradeLevels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var gradeLevel = await _context.GradeLevels
                    .Where(g => g.Id == id && !g.IsDeleted)
                    .FirstOrDefaultAsync();

                if (gradeLevel == null)
                {
                    return Json(new { success = false, message = "المرحلة الدراسية غير موجودة" });
                }

                // Check if grade level is used by any classrooms
                var hasClassrooms = await _context.ClassRooms
                    .Where(c => c.GradeLevelId == id && !c.IsDeleted)
                    .AnyAsync();

                if (hasClassrooms)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذه المرحلة لأنها مستخدمة في فصول دراسية" });
                }

                gradeLevel.IsDeleted = true;
                gradeLevel.DeletedAt = DateTime.UtcNow;
                gradeLevel.UpdatedAt = DateTime.UtcNow;
                gradeLevel.UpdatedBy = User.Identity?.Name ?? "System";

                _context.GradeLevels.Update(gradeLevel);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف المرحلة الدراسية بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade level");
                return Json(new { success = false, message = "حدث خطأ أثناء حذف المرحلة الدراسية" });
            }
        }
    }
}
