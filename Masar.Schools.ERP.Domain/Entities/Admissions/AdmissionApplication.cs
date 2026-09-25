using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Admissions;

/// <summary>
/// طلب الالتحاق بالمدرسة
/// </summary>
public class AdmissionApplication : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// رقم الطلب الآلي
    /// </summary>
    public string ApplicationNumber { get; set; } = string.Empty;

    /// <summary>
    /// نوع طلب الالتحاق
    /// </summary>
    public ApplicationType ApplicationType { get; set; }

    /// <summary>
    /// حالة الطلب
    /// </summary>
    public AdmissionStatus Status { get; set; } = AdmissionStatus.New;

    /// <summary>
    /// السنة الدراسية المطلوبة
    /// </summary>
    public string AcademicYear { get; set; } = string.Empty;

    /// <summary>
    /// المرحلة الدراسية المطلوبة
    /// </summary>
    public Guid? GradeLevelId { get; set; }
    public GradeLevel? GradeLevel { get; set; }

    /// <summary>
    /// الصف المطلوب
    /// </summary>
    public Guid? SectionId { get; set; }
    public Section? Section { get; set; }

    /// <summary>
    /// اسم الطالب الأول
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الطالب الثاني
    /// </summary>
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// اسم العائلة
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الكامل بالعربية
    /// </summary>
    public string FullNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الكامل بالإنجليزية
    /// </summary>
    public string FullNameEnglish { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ الميلاد
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// الجنس
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// الجنسية
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// رقم الهوية الوطنية
    /// </summary>
    public string? NationalId { get; set; }

    /// <summary>
    /// رقم الجواز
    /// </summary>
    public string? PassportNumber { get; set; }

    /// <summary>
    /// دين الطالب
    /// </summary>
    public string? Religion { get; set; }

    /// <summary>
    /// رقم السجل المدني
    /// </summary>
    public string? CivilId { get; set; }

    /// <summary>
    /// حالة إقامة الطالب
    /// </summary>
    public bool IsSaudiCitizen { get; set; }

    /// <summary>
    /// عنوان المنزل
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// المدينة
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// رمز المنطقة
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// رقم هاتف الطالب
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// هل الطالب لديه احتياجات خاصة
    /// </summary>
    public bool HasSpecialNeeds { get; set; }

    /// <summary>
    /// وصف الاحتياجات الخاصة
    /// </summary>
    public string? SpecialNeedsDescription { get; set; }

    /// <summary>
    /// هل الطالب موهوب
    /// </summary>
    public bool IsGifted { get; set; }

    /// <summary>
    /// وصف الموهبة
    /// </summary>
    public string? GiftedDescription { get; set; }

    /// <summary>
    /// اسم ولي الأمر الأول
    /// </summary>
    public string GuardianFirstName { get; set; } = string.Empty;

    /// <summary>
    /// اسم ولي الأمر الأخير
    /// </summary>
    public string GuardianLastName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الكامل لولي الأمر بالعربية
    /// </summary>
    public string GuardianFullNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// صلة القرابة
    /// </summary>
    public string Relationship { get; set; } = string.Empty;

    /// <summary>
    /// صلة القرابة بالعربية
    /// </summary>
    public string RelationshipArabic { get; set; } = string.Empty;

    /// <summary>
    /// رقم الهوية لولي الأمر
    /// </summary>
    public string? GuardianNationalId { get; set; }

    /// <summary>
    /// رقم هاتف ولي الأمر
    /// </summary>
    public string GuardianPhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// رقم واتساب ولي الأمر
    /// </summary>
    public string GuardianWhatsAppNumber { get; set; } = string.Empty;

    /// <summary>
    /// البريد الإلكتروني لولي الأمر
    /// </summary>
    public string? GuardianEmail { get; set; }

    /// <summary>
    /// مهنة ولي الأمر
    /// </summary>
    public string? GuardianOccupation { get; set; }

    /// <summary>
    /// مهنة ولي الأمر بالعربية
    /// </summary>
    public string? GuardianOccupationArabic { get; set; }

    /// <summary>
    /// مكان عمل ولي الأمر
    /// </summary>
    public string? GuardianWorkplace { get; set; }

    /// <summary>
    /// عنوان عمل ولي الأمر
    /// </summary>
    public string? GuardianWorkAddress { get; set; }

    /// <summary>
    /// عنوان منزل ولي الأمر
    /// </summary>
    public string? GuardianAddress { get; set; }

    /// <summary>
    /// المدرسة السابقة
    /// </summary>
    public string? PreviousSchool { get; set; }

    /// <summary>
    /// المدرسة السابقة بالعربية
    /// </summary>
    public string? PreviousSchoolArabic { get; set; }

    /// <summary>
    /// مدينة المدرسة السابقة
    /// </summary>
    public string? PreviousSchoolCity { get; set; }

    /// <summary>
    /// سبب النقل
    /// </summary>
    public string? TransferReason { get; set; }

    /// <summary>
    /// سبب النقل بالعربية
    /// </summary>
    public string? TransferReasonArabic { get; set; }

    /// <summary>
    /// تاريخ التقديم
    /// </summary>
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// تاريخ آخر تحديث
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// الموظف الذي قدم الطلب
    /// </summary>
    public Guid? SubmittedByEmployeeId { get; set; }
    public Employee? SubmittedByEmployee { get; set; }

    /// <summary>
    /// الموظف الذي راجع الطلب
    /// </summary>
    public Guid? ReviewedByEmployeeId { get; set; }
    public Employee? ReviewedByEmployee { get; set; }

    /// <summary>
    /// تاريخ المراجعة
    /// </summary>
    public DateTime? ReviewDate { get; set; }

    /// <summary>
    /// سبب الرفض
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// سبب الرفض بالعربية
    /// </summary>
    public string? RejectionReasonArabic { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// أولوية الطلب
    /// </summary>
    public int Priority { get; set; } = 1;

    /// <summary>
    /// تاريخ القرار النهائي
    /// </summary>
    public DateTime? DecisionDate { get; set; }

    /// <summary>
    /// معرف الطالب بعد التحويل
    /// </summary>
    public Guid? ConvertedStudentId { get; set; }
    public Student? ConvertedStudent { get; set; }

    /// <summary>
    /// تاريخ التحويل لطالب رسمي
    /// </summary>
    public DateTime? ConversionDate { get; set; }

    /// <summary>
    /// تم إرسال إشعار واتساب
    /// </summary>
    public bool WhatsAppNotificationSent { get; set; }

    /// <summary>
    /// تاريخ إرسال إشعار واتساب
    /// </summary>
    public DateTime? WhatsAppNotificationSentAt { get; set; }

    /// <summary>
    /// تم إرسال إشعار بريد إلكتروني
    /// </summary>
    public bool EmailNotificationSent { get; set; }

    /// <summary>
    /// تاريخ إرسال إشعار البريد الإلكتروني
    /// </summary>
    public DateTime? EmailNotificationSentAt { get; set; }

    /// <summary>
    /// مستندات الطلب
    /// </summary>
    public ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();

    /// <summary>
    /// المقابلة والاختبار
    /// </summary>
    public AdmissionExam? Exam { get; set; }
}