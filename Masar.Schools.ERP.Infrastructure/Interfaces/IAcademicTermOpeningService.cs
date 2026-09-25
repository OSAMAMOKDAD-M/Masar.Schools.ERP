using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة فتح الفصل الدراسي
/// </summary>
public interface IAcademicTermOpeningService
{
    /// <summary>
    /// الحصول على ملخص البيانات قبل فتح الفصل
    /// </summary>
    Task<AcademicTermOpeningSummary> GetOpeningSummaryAsync(OpenAcademicTermRequest request);

    /// <summary>
    /// فتح فصل دراسي جديد (ينفذ جميع الخطوات في معاملة واحدة)
    /// </summary>
    Task<OpenAcademicTermResult> OpenAcademicTermAsync(OpenAcademicTermRequest request);

    /// <summary>
    /// الحصول على جميع الفصول الدراسية
    /// </summary>
    Task<List<AcademicTermDto>> GetAllAcademicTermsAsync();

    /// <summary>
    /// الحصول على فصل دراسي بواسطة المعرف
    /// </summary>
    Task<AcademicTermDto?> GetAcademicTermByIdAsync(Guid id);

    /// <summary>
    /// تفعيل/تعطيل فصل دراسي
    /// </summary>
    Task<bool> ToggleAcademicTermStatusAsync(Guid id, bool isActive);
}
