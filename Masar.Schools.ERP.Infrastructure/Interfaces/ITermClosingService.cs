using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة إغلاق الفصل الدراسي
/// </summary>
public interface ITermClosingService
{
    /// <summary>
    /// فحص التحقق قبل الإغلاق
    /// </summary>
    Task<TermClosingValidationResult> ValidateBeforeClosingAsync(Guid termId);

    /// <summary>
    /// إغلاق الفصل الدراسي (ينفذ جميع الخطوات في معاملة واحدة صارمة)
    /// </summary>
    Task<CloseAcademicTermResult> CloseAcademicTermAsync(CloseAcademicTermRequest request);

    /// <summary>
    /// الحصول على جميع الأرشيف
    /// </summary>
    Task<List<TermArchiveDto>> GetAllArchivesAsync();

    /// <summary>
    /// الحصول على أرشيف بواسطة المعرف
    /// </summary>
    Task<TermArchiveDto?> GetArchiveByIdAsync(Guid id);

    /// <summary>
    /// استعادة أرشيف (إن لزم)
    /// </summary>
    Task<bool> RestoreArchiveAsync(Guid archiveId, string restoredBy);
}
