using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.Infrastructure.Services;

public interface IClassroomLookupService
{
    /// <summary>
    /// الحصول على جميع الفصول في مدرسة معينة
    /// </summary>
    Task<List<ClassRoom>> GetClassroomsBySchoolAsync(Guid schoolId);
    
    /// <summary>
    /// الحصول على الفصول حسب المرحلة الدراسية في مدرسة معينة
    /// </summary>
    Task<List<ClassRoom>> GetClassroomsByGradeAsync(Guid schoolId, string gradeLevel);
    
    /// <summary>
    /// الحصول على السعة المتاحة لفصل معين
    /// </summary>
    Task<int> GetClassroomAvailableCapacityAsync(Guid classroomId);
    
    /// <summary>
    /// الحصول على الفصول المتاحة (ليست ممتلئة) في مدرسة معينة
    /// </summary>
    Task<List<ClassRoom>> GetAvailableClassroomsAsync(Guid schoolId);
    
    /// <summary>
    /// الحصول على فصل معين بالمعرف
    /// </summary>
    Task<ClassRoom?> GetClassroomByIdAsync(Guid classroomId);
    
    /// <summary>
    /// الحصول على جميع الفصول النشطة
    /// </summary>
    Task<List<ClassRoom>> GetActiveClassroomsAsync();
    
    /// <summary>
    /// التحقق من كود الفصل الفريد داخل المدرسة
    /// </summary>
    Task<bool> IsClassroomCodeUniqueAsync(Guid schoolId, string code, Guid? excludeClassroomId = null);
}