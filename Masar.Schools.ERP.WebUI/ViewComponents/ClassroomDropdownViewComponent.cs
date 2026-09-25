using Microsoft.AspNetCore.Mvc;
using Masar.Schools.ERP.Infrastructure.Services;

namespace Masar.Schools.ERP.WebUI.ViewComponents;

public class ClassroomDropdownViewComponent : ViewComponent
{
    private readonly IClassroomLookupService _classroomLookupService;
    private readonly ILogger<ClassroomDropdownViewComponent> _logger;

    public ClassroomDropdownViewComponent(
        IClassroomLookupService classroomLookupService,
        ILogger<ClassroomDropdownViewComponent> logger)
    {
        _classroomLookupService = classroomLookupService;
        _logger = logger;
    }

    /// <summary>
    /// مكون عرض قائمة الفصول المنسدلة
    /// يمكن استدعاؤه في أي صفحة في النظام
    /// 
    /// الاستخدام:
    /// @await Component.InvokeAsync("ClassroomDropdown", new { schoolId = Guid, gradeLevel = "string", selectedId = Guid, name = "string" })
    /// </summary>
    public async Task<IViewComponentResult> InvokeAsync(
        Guid? schoolId = null, 
        string? gradeLevel = null, 
        Guid? selectedId = null,
        string name = "ClassroomId",
        string label = "الفصل",
        bool showCapacity = false,
        bool showGrade = false)
    {
        try
        {
            var classrooms = new List<Masar.Schools.ERP.Domain.Entities.ClassRoom>();

            // تحميل الفصول حسب المعايير
            if (schoolId.HasValue)
            {
                if (!string.IsNullOrEmpty(gradeLevel))
                {
                    classrooms = await _classroomLookupService.GetClassroomsByGradeAsync(schoolId.Value, gradeLevel);
                }
                else
                {
                    classrooms = await _classroomLookupService.GetClassroomsBySchoolAsync(schoolId.Value);
                }
            }
            else
            {
                // إذا لم يتم تحديد مدرسة، تحميل جميع الفصول النشطة
                classrooms = await _classroomLookupService.GetActiveClassroomsAsync();
            }

            ViewBag.SchoolId = schoolId;
            ViewBag.GradeLevel = gradeLevel;
            ViewBag.SelectedId = selectedId;
            ViewBag.Name = name;
            ViewBag.Label = label;
            ViewBag.ShowCapacity = showCapacity;
            ViewBag.ShowGrade = showGrade;

            return View(classrooms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading classroom dropdown");
            return View(new List<Masar.Schools.ERP.Domain.Entities.ClassRoom>());
        }
    }
}