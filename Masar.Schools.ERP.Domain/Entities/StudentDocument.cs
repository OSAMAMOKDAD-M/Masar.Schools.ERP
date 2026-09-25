using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// مستندات الطالب - نظام أرشفة المستندات
/// </summary>
public class StudentDocument : BaseEntity
{
    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// الطالب المرتبط
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// نوع المستند (BirthCertificate, ID, ReportCard, Medical, etc.)
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// نوع المستند بالعربية
    /// </summary>
    public string DocumentTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// اسم المستند الأصلي
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الملف المحفوظ
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// مسار الملف
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// مسار الصورة المصغرة
    /// </summary>
    public string? ThumbnailPath { get; set; }

    /// <summary>
    /// حجم الملف بالبايت
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// نوع الملف (MIME type)
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ انتهاء صلاحية المستند (إن وجد)
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// هل المستند موثق/محقق
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// تاريخ التوثيق
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// من قام بالتوثيق
    /// </summary>
    public string? VerifiedBy { get; set; }

    /// <summary>
    /// ملاحظات التوثيق
    /// </summary>
    public string? VerificationNotes { get; set; }

    /// <summary>
    /// تصنيف المستند (Academic, Medical, Financial, Administrative, Legal)
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// التصنيف بالعربية
    /// </summary>
    public string CategoryArabic { get; set; } = string.Empty;

    /// <summary>
    /// وصف المستند
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// وصف المستند بالعربية
    /// </summary>
    public string? DescriptionArabic { get; set; }

    /// <summary>
    /// الوسوم (Tags) - JSON array
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// هل المستند سري/مconfidential
    /// </summary>
    public bool IsConfidential { get; set; }

    /// <summary>
    /// تاريخ الاحتفاظ (للحذف التلقائي)
    /// </summary>
    public DateTime? RetentionDate { get; set; }

    /// <summary>
    /// إصدار المستند (للتعديل)
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// معرف المستند السابق (للتعديل)
    /// </summary>
    public Guid? PreviousDocumentId { get; set; }

    /// <summary>
    /// المستند السابق
    /// </summary>
    public StudentDocument? PreviousDocument { get; set; }

    /// <summary>
    /// المستندات التالية (التعديلات)
    /// </summary>
    public ICollection<StudentDocument> NextDocuments { get; set; } = new List<StudentDocument>();

    /// <summary>
    /// هل المستند مطلوب إلزامياً
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// حالة المستند (Pending, Approved, Rejected, Expired)
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// الحالة بالعربية
    /// </summary>
    public string StatusArabic { get; set; } = "قيد المراجعة";
}