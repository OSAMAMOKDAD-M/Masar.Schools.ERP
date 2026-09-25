namespace Masar.Schools.ERP.Domain.DTOs;

// AlumniRecord DTOs
public class AlumniRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string AlumniNumber { get; set; } = string.Empty;
    
    // معلومات التخرج
    public DateTime GraduationDate { get; set; }
    public int GraduationYear { get; set; }
    public decimal? FinalGPA { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    public string? Section { get; set; }
    public string? SectionArabic { get; set; }
    
    // معلومات الجامعة/الكلية
    public string? University { get; set; }
    public string? UniversityArabic { get; set; }
    public string? Major { get; set; }
    public string? MajorArabic { get; set; }
    public DateTime? UniversityEnrollmentDate { get; set; }
    public DateTime? UniversityGraduationDate { get; set; }
    
    // معلومات التوظيف
    public string? EmploymentStatus { get; set; }
    public string? EmploymentStatusArabic { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyNameArabic { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Industry { get; set; }
    public string? IndustryArabic { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    
    // معلومات التواصل
    public string? PersonalEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? Website { get; set; }
    public string? CurrentAddress { get; set; }
    public string? CurrentAddressArabic { get; set; }
    
    // ملاحظات
    public string? Achievements { get; set; }
    public string? AchievementsArabic { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    // حالة السجل
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? LastContactDate { get; set; }
    
    // معلومات النظام
    public DateTime CreatedAt { get; set; }
    public string? CreatedByEmployeeName { get; set; }
    public DateTime? LastUpdated { get; set;}

    public int DocumentCount { get; set; }
}

public class CreateAlumniRecordDto
{
    public Guid StudentId { get; set; }
    public DateTime GraduationDate { get; set; }
    public int GraduationYear { get; set; }
    public decimal? FinalGPA { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    public string? Section { get; set; }
    public string? SectionArabic { get; set; }
    
    public string? University { get; set; }
    public string? UniversityArabic { get; set; }
    public string? Major { get; set; }
    public string? MajorArabic { get; set; }
    public DateTime? UniversityEnrollmentDate { get; set; }
    public DateTime? UniversityGraduationDate { get; set; }
    
    public string? EmploymentStatus { get; set; }
    public string? EmploymentStatusArabic { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyNameArabic { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Industry { get; set; }
    public string? IndustryArabic { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    
    public string? PersonalEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? Website { get; set; }
    public string? CurrentAddress { get; set; }
    public string? CurrentAddressArabic { get; set; }
    
    public string? Achievements { get; set; }
    public string? AchievementsArabic { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateAlumniRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    
    public string? University { get; set; }
    public string? UniversityArabic { get; set; }
    public string? Major { get; set; }
    public string? MajorArabic { get; set; }
    public DateTime? UniversityEnrollmentDate { get; set; }
    public DateTime? UniversityGraduationDate { get; set; }
    
    public string? EmploymentStatus { get; set; }
    public string? EmploymentStatusArabic { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyNameArabic { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Industry { get; set; }
    public string? IndustryArabic { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    
    public string? PersonalEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? Website { get; set; }
    public string? CurrentAddress { get; set; }
    public string? CurrentAddressArabic { get; set; }
    
    public string? Achievements { get; set; }
    public string? AchievementsArabic { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? LastContactDate { get; set; }
}

// GraduationDocument DTOs
public class GraduationDocumentDto
{
    public Guid Id { get; set; }
    public Guid AlumniRecordId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public long? FileSize { get; set; }
    
    public bool IsIssued { get; set; }
    public bool IsDelivered { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? ReceivedBy { get; set; }
    public string? ReceivedByArabic { get; set; }
    
    public bool FinancialCleared { get; set; }
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    
    public bool AdministrativeCleared { get; set; }
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string? CreatedByEmployeeName { get; set; }
}

public class CreateGraduationDocumentDto
{
    public Guid AlumniRecordId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentTypeArabic { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public long? FileSize { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateGraduationDocumentDto
{
    public Guid Id { get; set; }
    public bool IsIssued { get; set; }
    public bool IsDelivered { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryMethod { get; set; }
    public string? ReceivedBy { get; set; }
    public string? ReceivedByArabic { get; set; }
    
    public bool FinancialCleared { get; set; }
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    public string? FinancialClearanceNotesArabic { get; set; }
    
    public bool AdministrativeCleared { get; set; }
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    public string? AdministrativeClearanceNotesArabic { get; set; }
    
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

// GraduationClearanceRecord DTOs
public class GraduationClearanceRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    
    public int GraduationYear { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    public string? Section { get; set; }
    public string? SectionArabic { get; set; }
    
    public string ClearanceStatus { get; set; } = string.Empty;
    public string ClearanceStatusArabic { get; set; } = string.Empty;
    
    public bool FinancialCleared { get; set; }
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    public string? FinancialClearanceNotesArabic { get; set; }
    public decimal? OutstandingBalance { get; set; }
    
    public bool AdministrativeCleared { get; set; }
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    public string? AdministrativeClearanceNotesArabic { get; set; }
    public bool ReturnedLibraryBooks { get; set; }
    public bool ReturnedEquipment { get; set; }
    
    public bool AcademicCleared { get; set; }
    public DateTime? AcademicClearanceDate { get; set; }
    public string? AcademicClearanceNotes { get; set; }
    public string? AcademicClearanceNotesArabic { get; set; }
    public bool AllGradesRecorded { get; set; }
    public bool AllRequirementsMet { get; set; }
    
    public bool ClinicCleared { get; set; }
    public DateTime? ClinicClearanceDate { get; set; }
    public string? ClinicClearanceNotes { get; set; }
    public string? ClinicClearanceNotesArabic { get; set; }
    
    public bool FinalApproval { get; set; }
    public DateTime? FinalApprovalDate { get; set; }
    public string? ApprovedByEmployeeName { get; set; }
    public string? FinalApprovalNotes { get; set; }
    public string? FinalApprovalNotesArabic { get; set; }
    
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string? CreatedByEmployeeName { get; set; }
    public DateTime? LastUpdated { get; set; }
}

public class CreateGraduationClearanceRecordDto
{
    public Guid StudentId { get; set; }
    public int GraduationYear { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    public string ClearanceStatus { get; set; } = "Pending";
    public string ClearanceStatusArabic { get; set; } = "معلق";
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateGraduationClearanceRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    
    public string ClearanceStatus { get; set; } = string.Empty;
    public string ClearanceStatusArabic { get; set; } = string.Empty;
    
    public bool FinancialCleared { get; set; }
    public DateTime? FinancialClearanceDate { get; set; }
    public string? FinancialClearanceNotes { get; set; }
    public string? FinancialClearanceNotesArabic { get; set; }
    public decimal? OutstandingBalance { get; set; }
    
    public bool AdministrativeCleared { get; set; }
    public DateTime? AdministrativeClearanceDate { get; set; }
    public string? AdministrativeClearanceNotes { get; set; }
    public string? AdministrativeClearanceNotesArabic { get; set; }
    public bool ReturnedLibraryBooks { get; set; }
    public bool ReturnedEquipment { get; set; }
    
    public bool AcademicCleared { get; set; }
    public DateTime? AcademicClearanceDate { get; set; }
    public string? AcademicClearanceNotes { get; set; }
    public string? AcademicClearanceNotesArabic { get; set; }
    public bool AllGradesRecorded { get; set; }
    public bool AllRequirementsMet { get; set; }
    
    public bool ClinicCleared { get; set; }
    public DateTime? ClinicClearanceDate { get; set; }
    public string? ClinicClearanceNotes { get; set; }
    public string? ClinicClearanceNotesArabic { get; set; }
    
    public bool FinalApproval { get; set; }
    public DateTime? FinalApprovalDate { get; set; }
    public string? FinalApprovalNotes { get; set; }
    public string? FinalApprovalNotesArabic { get; set; }
    
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

// Dashboard DTO
public class AlumniDashboardDto
{
    public int TotalAlumni { get; set; }
    public int ActiveAlumni { get; set; }
    public int VerifiedAlumni { get; set; }
    public int PendingClearance { get; set; }
    public int ClearedThisYear { get; set; }
    public int AlumniThisYear { get; set; }
    public int DocumentsIssued { get; set; }
    
    public int EmployedAlumni { get; set; }
    public int UnemployedAlumni { get; set; }
    public int UniversityStudents { get; set; }
    
    public List<RecentGraduateDto> RecentGraduates { get; set; } = new();
    public List<AlumniStatDto> EmploymentStats { get; set; } = new();
    public List<AlumniStatDto> UniversityStats { get; set; } = new();
}

public class RecentGraduateDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public DateTime GraduationDate { get; set; }
    public decimal? FinalGPA { get; set; }
    public string? University { get; set; }
    public string? Major { get; set; }
    public string? EmploymentStatus { get; set; }
}

public class AlumniStatDto
{
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}