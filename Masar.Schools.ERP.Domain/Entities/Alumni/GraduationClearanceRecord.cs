using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Alumni;

public class GraduationClearanceRecord : BaseEntity
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    // معلومات التخرج
    public int GraduationYear { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    
    // حالة الإبراء
    public string ClearanceStatus { get; set; } = string.Empty; // Pending, Cleared, Rejected
    public string ClearanceStatusArabic { get; set; } = string.Empty;
    
    // الإبراء المالي
    public bool FinancialCleared { get; set; } = false;
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    public string? FinancialClearanceNotesArabic { get; set; }
    public decimal? OutstandingBalance { get; set; }
    
    // الإبراء المكتبي
    public bool AdministrativeCleared { get; set; } = false;
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    public string? AdministrativeClearanceNotesArabic { get; set; }
    public bool ReturnedLibraryBooks { get; set; } = false;
    public bool ReturnedEquipment { get; set; } = false;
    
    // الإبراء الأكاديمي
    public bool AcademicCleared { get; set; } = false;
    public DateTime? AcademicClearanceDate { get; set; }
    public string? AcademicClearanceNotes { get; set; }
    public string? AcademicClearanceNotesArabic { get; set; }
    public bool AllGradesRecorded { get; set; } = false;
    public bool AllRequirementsMet { get; set; } = false;
    
    // إبراء العيادة
    public bool ClinicCleared { get; set; } = false;
    public DateTime? ClinicClearanceDate { get; set; }
    public string? ClinicClearanceNotes { get; set; }
    public string? ClinicClearanceNotesArabic { get; set; }
    
    // الموافقة النهائية
    public bool FinalApproval { get; set; } = false;
    public DateTime? FinalApprovalDate { get; set; }
    public Guid? ApprovedByEmployeeId { get; set; }
    public string? ApprovedByEmployeeName { get; set; }
    public string? FinalApprovalNotes { get; set; }
    public string? FinalApprovalNotesArabic { get; set; }
    
    // ملاحظات عامة
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