namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTO لطلب البحث والتصفية المتقدمة للمستندات
/// </summary>
public class DocumentSearchRequest
{
    public Guid? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? DocumentType { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsConfidential { get; set; }
    public bool? IsExpired { get; set; }
    public DateTime? ExpiryBefore { get; set; }
    public DateTime? ExpiryAfter { get; set; }
    public string? Tags { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "CreatedAt";
    public string SortDirection { get; set; } = "DESC";
}

/// <summary>
/// DTO لنتيجة البحث عن المستندات
/// </summary>
public class DocumentSearchResult
{
    public List<StudentDocumentDto> Documents { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

/// <summary>
/// DTO للمستند
/// </summary>
public class StudentDocumentDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? ThumbnailPath { get; set; }
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerifiedBy { get; set; }
    public string? VerificationNotes { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public string? Tags { get; set; }
    public bool IsConfidential { get; set; }
    public DateTime? RetentionDate { get; set; }
    public int Version { get; set; }
    public Guid? PreviousDocumentId { get; set; }
    public bool IsRequired { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// DTO لمشاركة المستند
/// </summary>
public class DocumentShareRequest
{
    public Guid DocumentId { get; set; }
    public List<string> RecipientEmails { get; set; } = new();
    public string? Message { get; set; }
    public string? MessageArabic { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool AllowDownload { get; set; } = true;
    public bool AllowPrint { get; set; } = false;
}

/// <summary>
/// DTO لرابط مشاركة المستند
/// </summary>
public class DocumentShareLink
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string ShareToken { get; set; } = string.Empty;
    public string ShareUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool AllowDownload { get; set; }
    public bool AllowPrint { get; set; }
    public int ViewCount { get; set; }
    public int DownloadCount { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO لرفع جماعي للمستندات
/// </summary>
public class BulkDocumentUploadRequest
{
    public Guid StudentId { get; set; }
    public List<DocumentUploadItem> Documents { get; set; } = new();
}

/// <summary>
/// عنصر في الرفع الجماعي
/// </summary>
public class DocumentUploadItem
{
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileData { get; set; } = string.Empty; // Base64
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsConfidential { get; set; }
}

/// <summary>
/// DTO لإشعار انتهاء صلاحية المستند
/// </summary>
public class DocumentExpiryNotification
{
    public Guid DocumentId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public int DaysUntilExpiry { get; set; }
    public string NotificationType { get; set; } = string.Empty; // Immediate, 7Days, 30Days
    public string NotificationTypeArabic { get; set; } = string.Empty;
}

/// <summary>
/// DTO لسجل نشاط المستند
/// </summary>
public class DocumentActivityLogDto
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty;
    public string ActivityTypeArabic { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
