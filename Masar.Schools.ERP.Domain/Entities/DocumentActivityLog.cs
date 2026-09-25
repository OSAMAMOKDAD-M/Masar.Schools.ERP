using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// سجل نشاط المستندات - تتبع العمليات على المستندات
/// </summary>
public class DocumentActivityLog : BaseEntity
{
    /// <summary>
    /// معرف المستند
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// المستند المرتبط
    /// </summary>
    public StudentDocument Document { get; set; } = null!;

    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid? StudentId { get; set; }

    /// <summary>
    /// الطالب المرتبط
    /// </summary>
    public Student? Student { get; set; }

    /// <summary>
    /// نوع النشاط (Viewed, Downloaded, Uploaded, Modified, Verified, Deleted, Shared)
    /// </summary>
    public string ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// نوع النشاط بالعربية
    /// </summary>
    public string ActivityTypeArabic { get; set; } = string.Empty;

    /// <summary>
    /// وصف النشاط
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// وصف النشاط بالعربية
    /// </summary>
    public string? DescriptionArabic { get; set; }

    /// <summary>
    /// عنوان IP الخاص بالمستخدم
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent (معلومات المتصفح)
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// هل النشاط ناجح
    /// </summary>
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// رسالة الخطأ (إذا فشل النشاط)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// بيانات إضافية (JSON)
    /// </summary>
    public string? AdditionalData { get; set; }
}
