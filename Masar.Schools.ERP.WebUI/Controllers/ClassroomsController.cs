using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.WebUI.ViewModels;
using Masar.Schools.ERP.Infrastructure.Services;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class ClassroomsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<ClassroomsController> _logger;
    private readonly IClassroomLookupService _classroomLookupService;

    public ClassroomsController(
        MasarDbContext context, 
        ILogger<ClassroomsController> logger,
        IClassroomLookupService classroomLookupService)
    {
        _context = context;
        _logger = logger;
        _classroomLookupService = classroomLookupService;
    }

    // GET: /Classrooms
    [Authorize]
    public async Task<IActionResult> Index(
        Guid? schoolId = null, 
        Guid? gradeLevelId = null, 
        int page = 1, 
        int pageSize = 10)
    {
        try
        {
            var query = _context.ClassRooms
                .Include(c => c.School)
                .Include(c => c.Branch)
                .Include(c => c.ClassTeacher)
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .Where(c => !c.IsDeleted);

            // تطبيق الفلترة
            if (schoolId.HasValue)
            {
                query = query.Where(c => c.SchoolId == schoolId.Value);
            }

            if (gradeLevelId.HasValue)
            {
                query = query.Where(c => c.GradeLevelId == gradeLevelId.Value);
            }

            // ترتيب النتائج
            query = query
                .OrderBy(c => c.School.NameArabic)
                .ThenBy(c => c.GradeLevelEntity != null ? c.GradeLevelEntity.NameArabic : "")
                .ThenBy(c => c.SectionEntity != null ? c.SectionEntity.NameArabic : "")
                .ThenBy(c => c.Code);

            // Pagination
            var totalCount = await query.CountAsync();
            var classrooms = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // تحويل إلى ViewModel
            var viewModel = classrooms.Select(c => new ClassroomListVM
            {
                Id = c.Id,
                Name = c.Name,
                NameArabic = c.NameArabic,
                Code = c.Code,
                GradeLevel = c.GradeLevelLegacy,
                Section = c.SectionLegacy,
                GradeLevelName = c.GradeLevelEntity?.NameArabic,
                SectionName = c.SectionEntity?.NameArabic,
                Capacity = c.Capacity,
                CurrentCount = c.CurrentCount,
                IsActive = c.IsActive,
                SchoolName = c.School?.NameArabic,
                TenantName = c.School?.Tenant?.NameArabic,
                ClassTeacherName = c.ClassTeacher?.FullNameArabic,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy
            }).ToList();

            // تحضير القوائم المنسدلة للفلترة
            var schools = await _context.Schools
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.NameArabic)
                .ToListAsync();

            var gradeLevels = await _context.GradeLevels
                .Where(g => !g.IsDeleted && g.IsActive)
                .OrderBy(g => g.DisplayOrder)
                .ThenBy(g => g.NameArabic)
                .ToListAsync();

            ViewBag.Schools = new SelectList(schools, "Id", "NameArabic", schoolId);
            ViewBag.GradeLevels = new SelectList(gradeLevels, "Id", "NameArabic", gradeLevelId);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading classrooms index");
            TempData["Error"] = "حدث خطأ أثناء تحميل بيانات الفصول";
            return View(new List<ClassroomListVM>());
        }
    }

    // GET: /Classrooms/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        try
        {
            var schools = await _context.Schools
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.NameArabic)
                .ToListAsync();

            var branches = await _context.Branches
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.NameArabic)
                .ToListAsync();

            var teachers = await _context.Employees
                .Where(e => !e.IsDeleted && e.IsActive)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync();

            var gradeLevels = await _context.GradeLevels
                .Where(g => !g.IsDeleted && g.IsActive)
                .OrderBy(g => g.DisplayOrder)
                .ThenBy(g => g.NameArabic)
                .ToListAsync();

            var sections = await _context.Sections
                .Where(s => !s.IsDeleted && s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.NameArabic)
                .ToListAsync();

            ViewBag.Schools = new SelectList(schools, "Id", "NameArabic");
            ViewBag.Branches = new SelectList(branches, "Id", "NameArabic");
            ViewBag.Teachers = new SelectList(teachers, "Id", "FullNameArabic");
            ViewBag.GradeLevels = new SelectList(gradeLevels, "Id", "NameArabic");
            ViewBag.Sections = new SelectList(sections, "Id", "NameArabic");

            return PartialView("_CreateEditModal", new ClassroomCreateVM());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create classroom form");
            return PartialView("_CreateEditModal", new ClassroomCreateVM());
        }
    }

    // POST: /Classrooms/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(ClassroomCreateVM model)
    {
        _logger.LogInformation("Create classroom POST called");
        _logger.LogInformation("Model state valid: {IsValid}", ModelState.IsValid);
        
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogWarning($"Validation error: {error.ErrorMessage}");
            }
            return Json(new { success = false, message = "بيانات غير صحيحة", errors = errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            // التحقق من تفرد الكود (فقط إذا كان الكود مُدخل يدوياً)
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                var isCodeUnique = await _classroomLookupService.IsClassroomCodeUniqueAsync(model.SchoolId, model.Code);
                if (!isCodeUnique)
                {
                    return Json(new { success = false, message = "كود الفصل مستخدم بالفعل في هذه المدرسة" });
                }
            }

            // توليد كود أوتوماتيكي إذا لم يُحدد
            var code = model.Code;
            if (string.IsNullOrWhiteSpace(code))
            {
                code = await GenerateClassroomCodeAsync(model.SchoolId, model.GradeLevelId, model.SectionId);
                _logger.LogInformation("Generated classroom code: {Code}", code);
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                code = "CLS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                _logger.LogWarning("Using fallback code: {Code}", code);
            }

            var classroom = new ClassRoom
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                NameArabic = model.NameArabic,
                Code = code,
                GradeLevelId = model.GradeLevelId,
                SectionId = model.SectionId,
                Capacity = model.Capacity,
                CurrentCount = 0,
                IsActive = model.IsActive,
                SchoolId = model.SchoolId,
                BranchId = model.BranchId,
                ClassTeacherId = model.ClassTeacherId,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name
            };

            _context.ClassRooms.Add(classroom);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Classroom created successfully: {ClassroomId}, {Code}", classroom.Id, classroom.Code);
            return Json(new { success = true, message = "تم إضافة الفصل بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating classroom");
            return Json(new { success = false, message = "حدث خطأ أثناء إضافة الفصل: " + ex.Message });
        }
    }

    // GET: /Classrooms/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var classroom = await _context.ClassRooms
                .Include(c => c.School)
                .Include(c => c.Branch)
                .Include(c => c.ClassTeacher)
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (classroom == null)
            {
                return Json(new { success = false, message = "الفصل غير موجود" });
            }

            var schools = await _context.Schools
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.NameArabic)
                .ToListAsync();

            var branches = await _context.Branches
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.NameArabic)
                .ToListAsync();

            var teachers = await _context.Employees
                .Where(e => !e.IsDeleted && e.IsActive)
                .OrderBy(e => e.FullNameArabic)
                .ToListAsync();

            var gradeLevels = await _context.GradeLevels
                .Where(g => !g.IsDeleted && g.IsActive)
                .OrderBy(g => g.DisplayOrder)
                .ThenBy(g => g.NameArabic)
                .ToListAsync();

            var sections = await _context.Sections
                .Where(s => !s.IsDeleted && s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.NameArabic)
                .ToListAsync();

            ViewBag.Schools = new SelectList(schools, "Id", "NameArabic", classroom.SchoolId);
            ViewBag.Branches = new SelectList(branches, "Id", "NameArabic", classroom.BranchId);
            ViewBag.Teachers = new SelectList(teachers, "Id", "FullNameArabic", classroom.ClassTeacherId);
            ViewBag.GradeLevels = new SelectList(gradeLevels, "Id", "NameArabic", classroom.GradeLevelId);
            ViewBag.Sections = new SelectList(sections, "Id", "NameArabic", classroom.SectionId);
            ViewBag.IsEdit = true;
            ViewBag.ClassroomId = classroom.Id;
            ViewBag.CurrentCount = classroom.CurrentCount;
            ViewBag.ClassroomCode = classroom.Code;

            var viewModel = new ClassroomEditVM
            {
                Id = classroom.Id,
                Name = classroom.Name,
                NameArabic = classroom.NameArabic,
                Code = classroom.Code,
                GradeLevelId = classroom.GradeLevelId ?? Guid.Empty,
                SectionId = classroom.SectionId ?? Guid.Empty,
                Capacity = classroom.Capacity,
                SchoolId = classroom.SchoolId,
                BranchId = classroom.BranchId,
                ClassTeacherId = classroom.ClassTeacherId,
                IsActive = classroom.IsActive,
                CurrentCount = classroom.CurrentCount
            };

            return PartialView("_CreateEditModal", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit classroom form: {ClassroomId}", id);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل بيانات الفصل" });
        }
    }

    // POST: /Classrooms/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(ClassroomEditVM model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return Json(new { success = false, message = "بيانات غير صحيحة", errors = errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var classroom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.Id == model.Id && !c.IsDeleted);

            if (classroom == null)
            {
                return Json(new { success = false, message = "الفصل غير موجود" });
            }

            // التحقق من تفرد الكود (فقط إذا تم تغيير الكود)
            if (!string.IsNullOrWhiteSpace(model.Code) && model.Code != classroom.Code)
            {
                var isCodeUnique = await _classroomLookupService.IsClassroomCodeUniqueAsync(model.SchoolId, model.Code, model.Id);
                if (!isCodeUnique)
                {
                    return Json(new { success = false, message = "كود الفصل مستخدم بالفعل في هذه المدرسة" });
                }
                classroom.Code = model.Code;
            }

            classroom.Name = model.Name;
            classroom.NameArabic = model.NameArabic;
            classroom.GradeLevelId = model.GradeLevelId;
            classroom.SectionId = model.SectionId;
            classroom.Capacity = model.Capacity;
            classroom.SchoolId = model.SchoolId;
            classroom.BranchId = model.BranchId;
            classroom.ClassTeacherId = model.ClassTeacherId;
            classroom.IsActive = model.IsActive;
            classroom.UpdatedAt = DateTime.Now;
            classroom.UpdatedBy = User.Identity?.Name;

            _context.Update(classroom);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Classroom updated successfully: {ClassroomId}, {Code}", classroom.Id, classroom.Code);
            return Json(new { success = true, message = "تم تحديث بيانات الفصل بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating classroom: {ClassroomId}", model.Id);
            return Json(new { success = false, message = "حدث خطأ أثناء تحديث بيانات الفصل" });
        }
    }

    // GET: /Classrooms/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var classroom = await _context.ClassRooms
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (classroom == null)
            {
                return Json(new { success = false, message = "الفصل غير موجود" });
            }

            // التحقق من وجود طلاب في الفصل
            if (classroom.Students.Any(s => !s.IsDeleted))
            {
                return Json(new { success = false, message = "لا يمكن حذف الفصل لأنه يحتوي على طلاب" });
            }

            // التحقق من وجود سجلات حضور مرتبطة
            var hasAttendanceRecords = await _context.AttendanceRecords
                .AnyAsync(a => a.ClassRoomId == id && !a.IsDeleted);

            if (hasAttendanceRecords)
            {
                return Json(new { success = false, message = "لا يمكن حذف الفصل لأنه يحتوي على سجلات حضور" });
            }

            return PartialView("_DeleteModal", classroom);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading delete classroom confirmation: {ClassroomId}", id);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل تأكيد الحذف" });
        }
    }

    // POST: /Classrooms/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var classroom = await _context.ClassRooms.FindAsync(id);
            if (classroom == null || classroom.IsDeleted)
            {
                return Json(new { success = false, message = "الفصل غير موجود" });
            }

            classroom.IsDeleted = true;
            classroom.DeletedAt = DateTime.Now;
            classroom.UpdatedBy = User.Identity?.Name;

            _context.Update(classroom);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Classroom deleted successfully: {ClassroomId}, {Code}", classroom.Id, classroom.Code);
            return Json(new { success = true, message = "تم حذف الفصل بنجاح" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting classroom: {ClassroomId}", id);
            return Json(new { success = false, message = "حدث خطأ أثناء حذف الفصل" });
        }
    }

    // GET: /Classrooms/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var classroom = await _context.ClassRooms
                .Include(c => c.School)
                .Include(c => c.Branch)
                .Include(c => c.ClassTeacher)
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .Include(c => c.Students.Where(s => !s.IsDeleted))
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (classroom == null)
            {
                return NotFound();
            }

            var viewModel = new ClassroomListVM
            {
                Id = classroom.Id,
                Name = classroom.Name,
                NameArabic = classroom.NameArabic,
                Code = classroom.Code,
                GradeLevel = classroom.GradeLevelLegacy,
                Section = classroom.SectionLegacy,
                GradeLevelName = classroom.GradeLevelEntity?.NameArabic,
                SectionName = classroom.SectionEntity?.NameArabic,
                Capacity = classroom.Capacity,
                CurrentCount = classroom.CurrentCount,
                IsActive = classroom.IsActive,
                SchoolName = classroom.School?.NameArabic,
                TenantName = classroom.School?.Tenant?.NameArabic,
                ClassTeacherName = classroom.ClassTeacher?.FullNameArabic,
                CreatedAt = classroom.CreatedAt,
                CreatedBy = classroom.CreatedBy
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading classroom details: {ClassroomId}", id);
            TempData["Error"] = "حدث خطأ أثناء تحميل تفاصيل الفصل";
            return RedirectToAction(nameof(Index));
        }
    }

    // API Endpoints for Cascading Dropdowns
    [HttpGet]
    public async Task<IActionResult> GetBySchool(Guid schoolId)
    {
        try
        {
            var classrooms = await _classroomLookupService.GetClassroomsBySchoolAsync(schoolId);
            return Json(new { success = true, data = classrooms.Select(c => new { 
                id = c.Id, 
                name = c.NameArabic, 
                code = c.Code,
                gradeLevel = c.GradeLevelLegacy,
                capacity = c.Capacity,
                currentCount = c.CurrentCount,
                availableCapacity = c.Capacity - c.CurrentCount
            }) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classrooms by school: {SchoolId}", schoolId);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل الفصول" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByGrade(Guid schoolId, string gradeLevel)
    {
        try
        {
            var classrooms = await _classroomLookupService.GetClassroomsByGradeAsync(schoolId, gradeLevel);
            return Json(new { success = true, data = classrooms.Select(c => new { 
                id = c.Id, 
                name = c.NameArabic, 
                code = c.Code,
                section = c.SectionLegacy,
                capacity = c.Capacity,
                currentCount = c.CurrentCount,
                availableCapacity = c.Capacity - c.CurrentCount
            }) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classrooms by grade: {SchoolId}, {GradeLevel}", schoolId, gradeLevel);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل الفصول" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailable(Guid schoolId)
    {
        try
        {
            var classrooms = await _classroomLookupService.GetAvailableClassroomsAsync(schoolId);
            return Json(new { success = true, data = classrooms.Select(c => new { 
                id = c.Id, 
                name = c.NameArabic, 
                code = c.Code,
                gradeLevel = c.GradeLevelLegacy,
                availableCapacity = c.Capacity - c.CurrentCount
            }) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available classrooms: {SchoolId}", schoolId);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل الفصول المتاحة" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCapacity(Guid classroomId)
    {
        try
        {
            var capacity = await _classroomLookupService.GetClassroomAvailableCapacityAsync(classroomId);
            return Json(new { success = true, data = new { classroomId, availableCapacity = capacity } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classroom capacity: {ClassroomId}", classroomId);
            return Json(new { success = false, message = "حدث خطأ أثناء تحميل السعة المتاحة" });
        }
    }

    // دالة مساعدة لتوليد كود الفصل الأوتوماتيكي
    private async Task<string> GenerateClassroomCodeAsync(Guid schoolId, Guid gradeLevelId, Guid sectionId)
    {
        try
        {
            var school = await _context.Schools.FindAsync(schoolId);
            var schoolCode = school?.Code ?? "SCH";
            
            // أخذ أول 3 أحرف من كود المدرسة
            if (schoolCode.Length > 3)
            {
                schoolCode = schoolCode.Substring(0, 3);
            }
            
            var gradeLevel = await _context.GradeLevels.FindAsync(gradeLevelId);
            var gradeCode = gradeLevel?.Code ?? "G0";
            
            var section = await _context.Sections.FindAsync(sectionId);
            var sectionCode = section?.Code ?? "A";
            
            // التأكد من تفرد الكود
            string code;
            int attempts = 0;
            do
            {
                var randomNum = new Random().Next(100, 999);
                code = $"{schoolCode}-{gradeCode}-{sectionCode}-{randomNum}";
                attempts++;
                
                if (attempts > 10)
                {
                    // إذا فشل عدة مرات، استخدام GUID مختصر
                    code = $"{schoolCode}-{gradeCode}-{sectionCode}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
                    break;
                }
            } while (await _context.ClassRooms.AnyAsync(c => c.SchoolId == schoolId && c.Code == code));
            
            return code;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating classroom code");
            // كود افتراضي في حالة الخطأ
            return $"CLS-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}