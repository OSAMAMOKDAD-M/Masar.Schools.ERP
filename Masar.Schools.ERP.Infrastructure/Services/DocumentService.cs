using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة المستندات - إدارة أرشيف المستندات المتقدم
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(MasarDbContext context, ILogger<DocumentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// البحث والتصفية المتقدمة للمستندات
    /// </summary>
    public async Task<DocumentSearchResult> SearchDocumentsAsync(DocumentSearchRequest request)
    {
        var query = _context.StudentDocuments
            .Include(d => d.Student)
            .Where(d => !d.IsDeleted);

        // تصفية حسب الطالب
        if (request.StudentId.HasValue)
        {
            query = query.Where(d => d.StudentId == request.StudentId.Value);
        }

        // تصفية حسب اسم الطالب
        if (!string.IsNullOrWhiteSpace(request.StudentName))
        {
            query = query.Where(d => d.Student.FullName.Contains(request.StudentName) ||
                                     d.Student.FullNameArabic.Contains(request.StudentName));
        }

        // تصفية حسب نوع المستند
        if (!string.IsNullOrWhiteSpace(request.DocumentType))
        {
            query = query.Where(d => d.DocumentType == request.DocumentType);
        }

        // تصفية حسب التصنيف
        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            query = query.Where(d => d.Category == request.Category);
        }

        // تصفية حسب الحالة
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(d => d.Status == request.Status);
        }

        // تصفية حسب التوثيق
        if (request.IsVerified.HasValue)
        {
            query = query.Where(d => d.IsVerified == request.IsVerified.Value);
        }

        // تصفية حسب السرية
        if (request.IsConfidential.HasValue)
        {
            query = query.Where(d => d.IsConfidential == request.IsConfidential.Value);
        }

        // تصفية حسب تاريخ الرفع
        if (request.StartDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt <= request.EndDate.Value);
        }

        // تصفية حسب تاريخ الانتهاء
        if (request.IsExpired.HasValue && request.IsExpired.Value)
        {
            query = query.Where(d => d.ExpiryDate.HasValue && d.ExpiryDate < DateTime.Now);
        }

        if (request.ExpiryBefore.HasValue)
        {
            query = query.Where(d => d.ExpiryDate.HasValue && d.ExpiryDate <= request.ExpiryBefore.Value);
        }

        if (request.ExpiryAfter.HasValue)
        {
            query = query.Where(d => d.ExpiryDate.HasValue && d.ExpiryDate >= request.ExpiryAfter.Value);
        }

        // تصفية حسب الوسوم
        if (!string.IsNullOrWhiteSpace(request.Tags))
        {
            query = query.Where(d => d.Tags != null && d.Tags.Contains(request.Tags));
        }

        // الترتيب
        query = request.SortDirection.ToUpper() == "ASC"
            ? query.OrderBy(d => EF.Property<object>(d, request.SortBy))
            : query.OrderByDescending(d => EF.Property<object>(d, request.SortBy));

        // العدد الإجمالي
        var totalCount = await query.CountAsync();

        // التصفح
        var documents = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new StudentDocumentDto
            {
                Id = d.Id,
                StudentId = d.StudentId,
                StudentName = d.Student.FullName,
                StudentNameArabic = d.Student.FullNameArabic,
                DocumentType = d.DocumentType,
                DocumentTypeArabic = d.DocumentTypeArabic,
                OriginalFileName = d.OriginalFileName,
                FileName = d.FileName,
                FilePath = d.FilePath,
                ThumbnailPath = d.ThumbnailPath,
                FileSize = d.FileSize,
                MimeType = d.MimeType,
                ExpiryDate = d.ExpiryDate,
                IsVerified = d.IsVerified,
                VerifiedAt = d.VerifiedAt,
                VerifiedBy = d.VerifiedBy,
                VerificationNotes = d.VerificationNotes,
                Category = d.Category,
                CategoryArabic = d.CategoryArabic,
                Description = d.Description,
                DescriptionArabic = d.DescriptionArabic,
                Tags = d.Tags,
                IsConfidential = d.IsConfidential,
                RetentionDate = d.RetentionDate,
                Version = d.Version,
                PreviousDocumentId = d.PreviousDocumentId,
                IsRequired = d.IsRequired,
                Status = d.Status,
                StatusArabic = d.StatusArabic,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                UpdatedAt = d.UpdatedAt,
                UpdatedBy = d.UpdatedBy
            })
            .ToListAsync();

        return new DocumentSearchResult
        {
            Documents = documents,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
        };
    }

    /// <summary>
    /// تسجيل نشاط على مستند
    /// </summary>
    public async Task LogActivityAsync(Guid documentId, Guid studentId, string activityType,
        string activityTypeArabic, string? description = null, string? descriptionArabic = null,
        string? ipAddress = null, string? userAgent = null, bool isSuccess = true, string? errorMessage = null)
    {
        var log = new DocumentActivityLog
        {
            Id = Guid.NewGuid(),
            DocumentId = documentId,
            StudentId = studentId,
            ActivityType = activityType,
            ActivityTypeArabic = activityTypeArabic,
            Description = description,
            DescriptionArabic = descriptionArabic,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsSuccess = isSuccess,
            ErrorMessage = errorMessage,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.DocumentActivityLogs.Add(log);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Document activity logged: {ActivityType} for document {DocumentId}", activityType, documentId);
    }

    /// <summary>
    /// الحصول على سجل نشاط مستند
    /// </summary>
    public async Task<List<DocumentActivityLogDto>> GetDocumentActivityLogAsync(Guid documentId)
    {
        return await _context.DocumentActivityLogs
            .Include(l => l.Student)
            .Where(l => l.DocumentId == documentId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new DocumentActivityLogDto
            {
                Id = l.Id,
                DocumentId = l.DocumentId,
                StudentId = l.StudentId ?? Guid.Empty,
                StudentName = l.Student != null ? l.Student.FullName : string.Empty,
                ActivityType = l.ActivityType,
                ActivityTypeArabic = l.ActivityTypeArabic,
                Description = l.Description,
                DescriptionArabic = l.DescriptionArabic,
                IpAddress = l.IpAddress,
                UserAgent = l.UserAgent,
                IsSuccess = l.IsSuccess,
                ErrorMessage = l.ErrorMessage,
                CreatedAt = l.CreatedAt,
                CreatedBy = l.CreatedBy
            })
            .ToListAsync();
    }

    /// <summary>
    /// إنشاء رابط مشاركة لمستند
    /// </summary>
    public async Task<DocumentShareLink> CreateShareLinkAsync(DocumentShareRequest request)
    {
        var token = Guid.NewGuid().ToString("N");
        var shareLink = new DocumentShareLink
        {
            Id = Guid.NewGuid(),
            DocumentId = request.DocumentId,
            ShareToken = token,
            ShareUrl = $"/documents/shared/{token}",
            CreatedAt = DateTime.Now,
            ExpiryDate = request.ExpiryDate,
            AllowDownload = request.AllowDownload,
            AllowPrint = request.AllowPrint,
            ViewCount = 0,
            DownloadCount = 0,
            IsActive = true
        };

        // Note: This would require a DocumentShareLink entity to be created
        // For now, we'll return a mock implementation
        _logger.LogInformation("Share link created for document {DocumentId}", request.DocumentId);

        return shareLink;
    }

    /// <summary>
    /// التحقق من صحة رابط مشاركة
    /// </summary>
    public async Task<DocumentShareLink?> ValidateShareLinkAsync(string token)
    {
        // Note: This would query a DocumentShareLink entity
        // For now, return null
        await Task.CompletedTask;
        return null;
    }

    /// <summary>
    /// إلغاء رابط مشاركة
    /// </summary>
    public async Task<bool> RevokeShareLinkAsync(Guid shareLinkId)
    {
        // Note: This would update a DocumentShareLink entity
        await Task.CompletedTask;
        return true;
    }

    /// <summary>
    /// رفع جماعي للمستندات
    /// </summary>
    public async Task<List<StudentDocumentDto>> BulkUploadDocumentsAsync(BulkDocumentUploadRequest request)
    {
        var uploadedDocuments = new List<StudentDocumentDto>();

        foreach (var docItem in request.Documents)
        {
            var document = new StudentDocument
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                DocumentType = docItem.DocumentType,
                DocumentTypeArabic = docItem.DocumentTypeArabic,
                OriginalFileName = docItem.FileName,
                FileName = $"{Guid.NewGuid()}_{docItem.FileName}",
                FilePath = $"uploads/documents/{request.StudentId}/{Guid.NewGuid()}_{docItem.FileName}",
                FileSize = docItem.FileSize,
                MimeType = docItem.MimeType,
                Category = docItem.Category,
                CategoryArabic = docItem.CategoryArabic,
                Description = docItem.Description,
                DescriptionArabic = docItem.DescriptionArabic,
                ExpiryDate = docItem.ExpiryDate,
                IsVerified = false,
                IsConfidential = docItem.IsConfidential,
                IsRequired = false,
                Status = "Pending",
                StatusArabic = "قيد المراجعة",
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            _context.StudentDocuments.Add(document);
            await _context.SaveChangesAsync();

            // Log activity
            await LogActivityAsync(document.Id, request.StudentId, "Uploaded", "تم الرفع",
                $"Document uploaded via bulk upload: {docItem.FileName}",
                $"تم رفع المستند عبر الرفع الجماعي: {docItem.FileName}");

            uploadedDocuments.Add(new StudentDocumentDto
            {
                Id = document.Id,
                StudentId = document.StudentId,
                DocumentType = document.DocumentType,
                DocumentTypeArabic = document.DocumentTypeArabic,
                OriginalFileName = document.OriginalFileName,
                FileName = document.FileName,
                FilePath = document.FilePath,
                FileSize = document.FileSize,
                MimeType = document.MimeType,
                Category = document.Category,
                CategoryArabic = document.CategoryArabic,
                Status = document.Status,
                StatusArabic = document.StatusArabic,
                CreatedAt = document.CreatedAt
            });
        }

        _logger.LogInformation("Bulk upload completed: {Count} documents for student {StudentId}", 
            uploadedDocuments.Count, request.StudentId);

        return uploadedDocuments;
    }

    /// <summary>
    /// الحصول على المستندات منتهية الصلاحية أو القريبة من الانتهاء
    /// </summary>
    public async Task<List<DocumentExpiryNotification>> GetExpiringDocumentsAsync(int daysBeforeExpiry = 30)
    {
        var expiryDate = DateTime.Now.AddDays(daysBeforeExpiry);

        var expiringDocuments = await _context.StudentDocuments
            .Include(d => d.Student)
            .Where(d => !d.IsDeleted && 
                       d.ExpiryDate.HasValue && 
                       d.ExpiryDate <= expiryDate &&
                       d.Status != "Expired")
            .Select(d => new DocumentExpiryNotification
            {
                DocumentId = d.Id,
                StudentId = d.StudentId,
                StudentName = d.Student.FullName,
                StudentNameArabic = d.Student.FullNameArabic,
                DocumentType = d.DocumentType,
                DocumentTypeArabic = d.DocumentTypeArabic,
                ExpiryDate = d.ExpiryDate!.Value,
                DaysUntilExpiry = (int)(d.ExpiryDate.Value - DateTime.Now).TotalDays,
                NotificationType = d.ExpiryDate < DateTime.Now ? "Immediate" : 
                                   d.ExpiryDate < DateTime.Now.AddDays(7) ? "7Days" : "30Days",
                NotificationTypeArabic = d.ExpiryDate < DateTime.Now ? "فوري" : 
                                         d.ExpiryDate < DateTime.Now.AddDays(7) ? "7 أيام" : "30 يوم"
            })
            .ToListAsync();

        return expiringDocuments;
    }

    /// <summary>
    /// تحديث حالة انتهاء صلاحية المستندات
    /// </summary>
    public async Task UpdateExpiredDocumentsStatusAsync()
    {
        var expiredDocuments = await _context.StudentDocuments
            .Where(d => !d.IsDeleted && 
                       d.ExpiryDate.HasValue && 
                       d.ExpiryDate < DateTime.Now &&
                       d.Status != "Expired")
            .ToListAsync();

        foreach (var document in expiredDocuments)
        {
            document.Status = "Expired";
            document.StatusArabic = "منتهي الصلاحية";
            document.UpdatedAt = DateTime.Now;
            document.UpdatedBy = "System";

            await LogActivityAsync(document.Id, document.StudentId, "Expired", "منتهي الصلاحية",
                $"Document expired on {document.ExpiryDate}",
                $"انتهت صلاحية المستند في {document.ExpiryDate}");
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated {Count} expired documents", expiredDocuments.Count);
    }

    /// <summary>
    /// الحصول على إحصائيات المستندات
    /// </summary>
    public async Task<DocumentStatistics> GetDocumentStatisticsAsync(Guid? studentId = null)
    {
        var query = _context.StudentDocuments.Where(d => !d.IsDeleted);

        if (studentId.HasValue)
        {
            query = query.Where(d => d.StudentId == studentId.Value);
        }

        var documents = await query.ToListAsync();

        var statistics = new DocumentStatistics
        {
            TotalDocuments = documents.Count,
            VerifiedDocuments = documents.Count(d => d.IsVerified),
            PendingDocuments = documents.Count(d => d.Status == "Pending"),
            ExpiredDocuments = documents.Count(d => d.ExpiryDate.HasValue && d.ExpiryDate < DateTime.Now),
            ExpiringSoonDocuments = documents.Count(d => d.ExpiryDate.HasValue && 
                                                      d.ExpiryDate < DateTime.Now.AddDays(30) && 
                                                      d.ExpiryDate >= DateTime.Now),
            ConfidentialDocuments = documents.Count(d => d.IsConfidential),
            TotalFileSize = documents.Sum(d => d.FileSize),
            DocumentsByType = documents.GroupBy(d => d.DocumentType)
                .ToDictionary(g => g.Key, g => g.Count()),
            DocumentsByCategory = documents.GroupBy(d => d.Category)
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return statistics;
    }
}
