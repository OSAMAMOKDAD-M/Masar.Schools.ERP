using Masar.Schools.ERP.Domain.Entities.Admissions;

namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTOs لموديول القبول والتسجيل
/// </summary>

// ==================== Admission Application DTOs ====================
public class AdmissionApplicationDto
{
    public Guid Id { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public ApplicationType ApplicationType { get; set; }
    public AdmissionStatus Status { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
    public string? GradeLevelName { get; set; }
    public string? SectionName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string FullNameEnglish { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? NationalId { get; set; }
    public string GuardianFullNameArabic { get; set; } = string.Empty;
    public string GuardianPhoneNumber { get; set; } = string.Empty;
    public string GuardianWhatsAppNumber { get; set; } = string.Empty;
    public string? GuardianEmail { get; set; }
    public DateTime ApplicationDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? SubmittedByEmployeeName { get; set; }
    public string? ReviewedByEmployeeName { get; set; }
    public DateTime? ReviewDate { get; set; }
    public string? RejectionReason { get; set; }
    public string? RejectionReasonArabic { get; set; }
    public string? Notes { get; set; }
    public int Priority { get; set; }
    public DateTime? DecisionDate { get; set; }
    public Guid? ConvertedStudentId { get; set; }
    public DateTime? ConversionDate { get; set; }
    public bool WhatsAppNotificationSent { get; set; }
    public DateTime? WhatsAppNotificationSentAt { get; set; }
    public bool EmailNotificationSent { get; set; }
    public DateTime? EmailNotificationSentAt { get; set; }
    public List<ApplicationDocumentDto> Documents { get; set; } = new();
    public AdmissionExamDto? Exam { get; set; }
}

public class CreateAdmissionApplicationDto
{
    public Guid SchoolId { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
    public Guid? GradeLevelId { get; set; }
    public Guid? SectionId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string FullNameEnglish { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Religion { get; set; }
    public string? CivilId { get; set; }
    public bool IsSaudiCitizen { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool HasSpecialNeeds { get; set; }
    public string? SpecialNeedsDescription { get; set; }
    public bool IsGifted { get; set; }
    public string? GiftedDescription { get; set; }
    public string GuardianFirstName { get; set; } = string.Empty;
    public string GuardianLastName { get; set; } = string.Empty;
    public string GuardianFullNameArabic { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string RelationshipArabic { get; set; } = string.Empty;
    public string? GuardianNationalId { get; set; }
    public string GuardianPhoneNumber { get; set; } = string.Empty;
    public string GuardianWhatsAppNumber { get; set; } = string.Empty;
    public string? GuardianEmail { get; set; }
    public string? GuardianOccupation { get; set; }
    public string? GuardianOccupationArabic { get; set; }
    public string? GuardianWorkplace { get; set; }
    public string? GuardianWorkAddress { get; set; }
    public string? GuardianAddress { get; set; }
    public string? PreviousSchool { get; set; }
    public string? PreviousSchoolArabic { get; set; }
    public string? PreviousSchoolCity { get; set; }
    public string? TransferReason { get; set; }
    public string? TransferReasonArabic { get; set; }
    public string? Notes { get; set; }
    public int Priority { get; set; } = 1;
    public List<CreateApplicationDocumentDto> Documents { get; set; } = new();
}

public class UpdateAdmissionApplicationDto
{
    public Guid Id { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public AdmissionStatus Status { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
    public Guid? GradeLevelId { get; set; }
    public Guid? SectionId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string FullNameEnglish { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNumber { get; set; }
    public string? Religion { get; set; }
    public string? CivilId { get; set; }
    public bool IsSaudiCitizen { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool HasSpecialNeeds { get; set; }
    public string? SpecialNeedsDescription { get; set; }
    public bool IsGifted { get; set; }
    public string? GiftedDescription { get; set; }
    public string GuardianFirstName { get; set; } = string.Empty;
    public string GuardianLastName { get; set; } = string.Empty;
    public string GuardianFullNameArabic { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string RelationshipArabic { get; set; } = string.Empty;
    public string? GuardianNationalId { get; set; }
    public string GuardianPhoneNumber { get; set; } = string.Empty;
    public string GuardianWhatsAppNumber { get; set; } = string.Empty;
    public string? GuardianEmail { get; set; }
    public string? GuardianOccupation { get; set; }
    public string? GuardianOccupationArabic { get; set; }
    public string? GuardianWorkplace { get; set; }
    public string? GuardianWorkAddress { get; set; }
    public string? GuardianAddress { get; set; }
    public string? PreviousSchool { get; set; }
    public string? PreviousSchoolArabic { get; set; }
    public string? PreviousSchoolCity { get; set; }
    public string? TransferReason { get; set; }
    public string? TransferReasonArabic { get; set; }
    public string? Notes { get; set; }
    public int Priority { get; set; }
    public string? RejectionReason { get; set; }
    public string? RejectionReasonArabic { get; set; }
}

public class UpdateApplicationStatusDto
{
    public Guid Id { get; set; }
    public AdmissionStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public string? RejectionReasonArabic { get; set; }
    public string? Notes { get; set; }
}

// ==================== Admission Exam DTOs ====================
public class AdmissionExamDto
{
    public Guid Id { get; set; }
    public Guid AdmissionApplicationId { get; set; }
    public InterviewStatus InterviewStatus { get; set; }
    public DateTime? InterviewDate { get; set; }
    public TimeSpan? InterviewTime { get; set; }
    public string? InterviewLocation { get; set; }
    public string? InterviewerName { get; set; }
    public int? InterviewScore { get; set; }
    public string? InterviewNotes { get; set; }
    public ExamStatus ExamStatus { get; set; }
    public DateTime? ExamDate { get; set; }
    public TimeSpan? ExamTime { get; set; }
    public string? ExamLocation { get; set; }
    public string? ExaminerName { get; set; }
    public decimal? ArabicScore { get; set; }
    public decimal? MathScore { get; set; }
    public decimal? ScienceScore { get; set; }
    public decimal? EnglishScore { get; set; }
    public decimal? TotalScore { get; set; }
    public decimal? Percentage { get; set; }
    public decimal? PassingScore { get; set; }
    public bool? PassedExam { get; set; }
    public string? ExamNotes { get; set; }
    public string? MedicalNotes { get; set; }
    public string? BehavioralNotes { get; set; }
    public string? FinalRecommendation { get; set; }
    public string? FinalRecommendationArabic { get; set; }
    public DateTime? EvaluationDate { get; set; }
    public string? EvaluatedByEmployeeName { get; set; }
    public bool InterviewWhatsAppSent { get; set; }
    public DateTime? InterviewWhatsAppSentAt { get; set; }
    public bool ExamWhatsAppSent { get; set; }
    public DateTime? ExamWhatsAppSentAt { get; set; }
}

public class CreateAdmissionExamDto
{
    public Guid AdmissionApplicationId { get; set; }
    public DateTime? InterviewDate { get; set; }
    public TimeSpan? InterviewTime { get; set; }
    public string? InterviewLocation { get; set; }
    public Guid? InterviewerId { get; set; }
    public DateTime? ExamDate { get; set; }
    public TimeSpan? ExamTime { get; set; }
    public string? ExamLocation { get; set; }
    public Guid? ExaminerId { get; set; }
    public decimal? PassingScore { get; set; } = 60;
}

public class UpdateAdmissionExamDto
{
    public Guid Id { get; set; }
    public InterviewStatus InterviewStatus { get; set; }
    public DateTime? InterviewDate { get; set; }
    public TimeSpan? InterviewTime { get; set; }
    public string? InterviewLocation { get; set; }
    public Guid? InterviewerId { get; set; }
    public int? InterviewScore { get; set; }
    public string? InterviewNotes { get; set; }
    public ExamStatus ExamStatus { get; set; }
    public DateTime? ExamDate { get; set; }
    public TimeSpan? ExamTime { get; set; }
    public string? ExamLocation { get; set; }
    public Guid? ExaminerId { get; set; }
    public decimal? ArabicScore { get; set; }
    public decimal? MathScore { get; set; }
    public decimal? ScienceScore { get; set; }
    public decimal? EnglishScore { get; set; }
    public decimal? TotalScore { get; set; }
    public decimal? Percentage { get; set; }
    public decimal? PassingScore { get; set; }
    public bool? PassedExam { get; set; }
    public string? ExamNotes { get; set; }
    public string? MedicalNotes { get; set; }
    public string? BehavioralNotes { get; set; }
    public string? FinalRecommendation { get; set; }
    public string? FinalRecommendationArabic { get; set; }
    public DateTime? EvaluationDate { get; set; }
    public Guid? EvaluatedByEmployeeId { get; set; }
}

// ==================== Application Document DTOs ====================
public class ApplicationDocumentDto
{
    public Guid Id { get; set; }
    public Guid AdmissionApplicationId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? FileType { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public string? VerifiedByEmployeeName { get; set; }
    public string? VerificationNotes { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? UploadedByEmployeeName { get; set; }
    public string? Description { get; set; }
}

public class CreateApplicationDocumentDto
{
    public Guid AdmissionApplicationId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? FileType { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public string? Description { get; set; }
}

public class UpdateApplicationDocumentDto
{
    public Guid Id { get; set; }
    public bool IsVerified { get; set; }
    public string? VerificationNotes { get; set; }
}

// ==================== Search and Filter DTOs ====================
public class AdmissionSearchDto
{
    public string? SearchTerm { get; set; }
    public Guid? SchoolId { get; set; }
    public ApplicationType? ApplicationType { get; set; }
    public AdmissionStatus? Status { get; set; }
    public string? AcademicYear { get; set; }
    public Guid? GradeLevelId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class AdmissionStatisticsDto
{
    public int TotalApplications { get; set; }
    public int NewApplications { get; set; }
    public int UnderReview { get; set; }
    public int InterviewScheduled { get; set; }
    public int ExamScheduled { get; set; }
    public int Accepted { get; set; }
    public int Rejected { get; set; }
    public int Waitlisted { get; set; }
    public int Withdrawn { get; set; }
    public int Enrolled { get; set; }
    public decimal AcceptanceRate { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableSlots { get; set; }
}

// ==================== Response DTOs ====================
public class AdmissionResponseDto<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; } = new();
}

public class AdmissionPagedResponseDto<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

// ==================== Conversion DTOs ====================
public class ConvertToStudentDto
{
    public Guid AdmissionApplicationId { get; set; }
    public Guid ClassRoomId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string? NoorStudentId { get; set; }
    public string? Notes { get; set; }
}

public class ConversionResultDto
{
    public Guid StudentId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public Guid? GuardianId { get; set; }
    public bool AccountCreated { get; set; }
    public string? Message { get; set; }
}