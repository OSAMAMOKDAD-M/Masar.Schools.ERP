using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Alumni;

public class GraduationDocument : BaseEntity
{
    public Guid Id { get; set; }
    public Guid AlumniRecordId { get; set; }
    public AlumniRecord AlumniRecord { get; set; } = null!;
    
    // نوع الوثيقة
    public string DocumentType { get; set; } = string.Empty; // Certificate, Transcript, ClearanceLetter, etc.
    public string DocumentTypeArabic { get; set; } = string.Empty;
    
    // معلومات الوثيقة
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    
    // الملف الرقمي
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public long? FileSize { get; set; }
    
    // حالة الوثيقة
    public bool IsIssued { get; set; } = false;
    public bool IsDelivered { get; set; } = false;
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryMethod { get; set; } // InPerson, Email, Courier
    public string? ReceivedBy { get; set; }
    public string? ReceivedByArabic { get; set; }
    
    // إبراء الذمة المالي
    public bool FinancialCleared { get; set; } = false;
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    
    // إبراء الذمة المكتبي
    public bool AdministrativeCleared { get; set; } = false;
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    
    // ملاحظات
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    // Multi-tenancy
    public Guid TenantId { get; set; }
    public Guid SchoolId { get; set; }
    
    // Audit fields
    public Guid? CreatedByEmployeeId { get; set; }
    public Employee? CreatedByEmployee { get; set; }
    public DateTime? LastUpdated { get; set; }
}