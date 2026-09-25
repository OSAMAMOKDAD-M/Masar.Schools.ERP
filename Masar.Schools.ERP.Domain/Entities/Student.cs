using Masar.Schools.ERP.Domain.Common;
using Masar.Schools.ERP.Domain.Entities.Clinic;

namespace Masar.Schools.ERP.Domain.Entities;

public class Student : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? StudentNumber { get; set; }
    public string? NoorStudentId { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public DateTime? GraduationDate { get; set; }
    public bool IsActive { get; set; }
    
    // حقول التخرج للنظام الجديد
    public bool IsGraduated { get; set; } = false;
    public int? GraduationYear { get; set; }
    public decimal? FinalGPA { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; }
    public string? FingerprintTemplate { get; set; }
    public bool IsSaudiCitizen { get; set; } = false; // For VAT exemption for private education
    
    // الحقول الإضافية الجديدة
    public string? PreviousSchool { get; set; }          // المدرسة السابقة
    public string? PreviousSchoolArabic { get; set; }
    public DateTime? TransferDate { get; set; }           // تاريخ التحويل
    public string? TransferReason { get; set; }          // سبب التحويل
    public string? AcademicStanding { get; set; }        // الوضع الأكاديمي (Good, Probation, Suspended)
    public string? SpecialNeeds { get; set; }            // الاحتياجات الخاصة
    public string? SpecialNeedsArabic { get; set; }
    public bool IsGifted { get; set; }                   // متميز/موهوب
    public string? GiftedProgram { get; set; }           // برنامج الموهوبين
    public DateTime? LastMedicalCheckup { get; set; }    // آخر فحص طبي
    public string? Allergies { get; set; }               // الحساسية
    public string? AllergiesArabic { get; set; }
    public string? DietaryRestrictions { get; set; }      // القيود الغذائية
    public string? DietaryRestrictionsArabic { get; set; }
    
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    
    public Guid? ClassRoomId { get; set; }
    public ClassRoom? ClassRoom { get; set; }
    
    public Guid? GuardianId { get; set; }
    public Guardian? Guardian { get; set; }
    
    public StudentAccount? StudentAccount { get; set; }
    
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
    public ICollection<StudentDocument> Documents { get; set; } = new List<StudentDocument>();
    public StudentHealthProfile? HealthProfile { get; set; }
}
