using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Admissions;

/// <summary>
/// مستندات طلب الالتحاق
/// </summary>
public class ApplicationDocument : BaseEntity
{
    /// <summary>
    /// معرف طلب الالتحاق
    /// </summary>
    public Guid AdmissionApplicationId { get; set; }
    public AdmissionApplication AdmissionApplication { get; set; } = null!;

    /// <summary>
    /// نوع المستند
    /// </summary>
    public DocumentType DocumentType { get; set; }

    /// <summary>
    /// اسم المستند
    /// </summary>
    public string DocumentName { get; set; } = string.Empty;

    /// <summary>
    /// مسار الملف
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// حجم الملف (بالبايت)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// نوع الملف (MIME)
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// اسم الملف الأصلي
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// هل المستند مطلوب
    /// </summary>
    public bool IsRequired { get; set; } = true;

    /// <summary>
    /// هل المستند تم التحقق منه
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// تاريخ التحقق
    /// </summary>
    public DateTime? VerifiedDate { get; set; }

    /// <summary>
    /// الموظف الذي تحقق من المستند
    /// </summary>
    public Guid? VerifiedByEmployeeId { get; set; }
    public Employee? VerifiedByEmployee { get; set; }

    /// <summary>
    /// ملاحظات التحقق
    /// </summary>
    public string? VerificationNotes { get; set; }

    /// <summary>
    /// تاريخ الرفع
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// الموظف الذي رفع المستند
    /// </summary>
    public Guid? UploadedByEmployeeId { get; set; }
    public Employee? UploadedByEmployee { get; set; }

    /// <summary>
    /// وصف المستند
    /// </summary>
    public string? Description { get; set; }
}