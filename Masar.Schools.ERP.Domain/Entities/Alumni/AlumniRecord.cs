using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Alumni;

public class AlumniRecord : BaseEntity
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    // معلومات التخرج
    public DateTime GraduationDate { get; set; }
    public int GraduationYear { get; set; }
    public decimal? FinalGPA { get; set; }
    public string? GradeLevel { get; set; }
    public string? GradeLevelArabic { get; set; }
    public string? Section { get; set; }
    public string? SectionArabic { get; set; }
    
    // معلومات الجامعة/الكلية بعد التخرج
    public string? University { get; set; }
    public string? UniversityArabic { get; set; }
    public string? Major { get; set; }
    public string? MajorArabic { get; set; }
    public DateTime? UniversityEnrollmentDate { get; set; }
    public DateTime? UniversityGraduationDate { get; set; }
    
    // معلومات التوظيف
    public string? EmploymentStatus { get; set; } // Employed, Unemployed, SelfEmployed, Student
    public string? EmploymentStatusArabic { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyNameArabic { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Industry { get; set; }
    public string? IndustryArabic { get; set; }
    public DateTime? EmploymentStartDate { get; set; }
    
    // معلومات التواصل بعد التخرج
    public string? PersonalEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? Website { get; set; }
    public string? CurrentAddress { get; set; }
    public string? CurrentAddressArabic { get; set; }
    
    // ملاحظات إضافية
    public string? Achievements { get; set; }
    public string? AchievementsArabic { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    
    // حالة السجل
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public DateTime? LastContactDate { get; set; }
    
    // Multi-tenancy
    public Guid TenantId { get; set; }
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    
    // Audit fields
    public Guid? CreatedByEmployeeId { get; set; }
    public Employee? CreatedByEmployee { get; set; }
    public DateTime? LastUpdated { get; set; }
    
    // Navigation properties
    public ICollection<GraduationDocument> GraduationDocuments { get; set; } = new List<GraduationDocument>();
}