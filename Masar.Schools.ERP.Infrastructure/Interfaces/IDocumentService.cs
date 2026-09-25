using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة المستندات - إدارة أرشيف المستندات المتقدم
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// البحث والتصفية المتقدمة للمستندات
    /// </summary>
    Task<DocumentSearchResult> SearchDocumentsAsync(DocumentSearchRequest request);

    /// <summary>
    /// تسجيل نشاط على مستند
    /// </summary>
    Task LogActivityAsync(Guid documentId, Guid studentId, string activityType, 
        string activityTypeArabic, string? description = null, string? descriptionArabic = null,
        string? ipAddress = null, string? userAgent = null, bool isSuccess = true, string? errorMessage = null);

    /// <summary>
    /// الحصول على سجل نشاط مستند
    /// </summary>
    Task<List<DocumentActivityLogDto>> GetDocumentActivityLogAsync(Guid documentId);

    /// <summary>
    /// إنشاء رابط مشاركة لمستند
    /// </summary>
    Task<DocumentShareLink> CreateShareLinkAsync(DocumentShareRequest request);

    /// <summary>
    /// التحقق من صحة رابط مشاركة
    /// </summary>
    Task<DocumentShareLink?> ValidateShareLinkAsync(string token);

    /// <summary>
    /// إلغاء رابط مشاركة
    /// </summary>
    Task<bool> RevokeShareLinkAsync(Guid shareLinkId);

    /// <summary>
    /// رفع جماعي للمستندات
    /// </summary>
    Task<List<StudentDocumentDto>> BulkUploadDocumentsAsync(BulkDocumentUploadRequest request);

    /// <summary>
    /// الحصول على المستندات منتهية الصلاحية أو القريبة من الانتهاء
    /// </summary>
    Task<List<DocumentExpiryNotification>> GetExpiringDocumentsAsync(int daysBeforeExpiry = 30);

    /// <summary>
    /// تحديث حالة انتهاء صلاحية المستندات
    /// </summary>
    Task UpdateExpiredDocumentsStatusAsync();

    /// <summary>
    /// الحصول على إحصائيات المستندات
    /// </summary>
    Task<DocumentStatistics> GetDocumentStatisticsAsync(Guid? studentId = null);
}

/// <summary>
/// إحصائيات المستندات
/// </summary>
public class DocumentStatistics
{
    public int TotalDocuments { get; set; }
    public int VerifiedDocuments { get; set; }
    public int PendingDocuments { get; set; }
    public int ExpiredDocuments { get; set; }
    public int ExpiringSoonDocuments { get; set; }
    public int ConfidentialDocuments { get; set; }
    public long TotalFileSize { get; set; }
    public Dictionary<string, int> DocumentsByType { get; set; } = new();
    public Dictionary<string, int> DocumentsByCategory { get; set; } = new();
}
