namespace Masar.Schools.ERP.Domain.Entities.Admissions;

/// <summary>
/// حالة طلب الالتحاق
/// </summary>
public enum AdmissionStatus
{
    /// <summary>
    /// طلب جديد
    /// </summary>
    New = 0,

    /// <summary>
    /// قيد المراجعة
    /// </summary>
    UnderReview = 1,

    /// <summary>
    /// في انتظار المقابلة
    /// </summary>
    InterviewScheduled = 2,

    /// <summary>
    /// في انتظار الاختبار
    /// </summary>
    ExamScheduled = 3,

    /// <summary>
    /// قيد الدراسة
    /// </summary>
    UnderConsideration = 4,

    /// <summary>
    /// مقبول
    /// </summary>
    Accepted = 5,

    /// <summary>
    /// مرفوض
    /// </summary>
    Rejected = 6,

    /// <summary>
    /// قائمة الانتظار
    /// </summary>
    Waitlisted = 7,

    /// <summary>
    /// مسحوب
    /// </summary>
    Withdrawn = 8,

    /// <summary>
    /// تم التحويل لطالب رسمي
    /// </summary>
    Enrolled = 9
}

/// <summary>
/// نوع طلب الالتحاق
/// </summary>
public enum ApplicationType
{
    /// <summary>
    /// جديد (طالب لم يسبق له الالتحاق)
    /// </summary>
    NewStudent = 0,

    /// <summary>
    /// نقل من مدرسة أخرى
    /// </summary>
    Transfer = 1,

    /// <summary>
    /// إعادة تسجيل
    /// </summary>
    ReEnrollment = 2,

    /// <summary>
    /// طالب معاد
    /// </summary>
    Equivalency = 3
}

/// <summary>
/// حالة المقابلة
/// </summary>
public enum InterviewStatus
{
    /// <summary>
    /// لم يتم تحديد موعد
    /// </summary>
    NotScheduled = 0,

    /// <summary>
    /// مجدول
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// مكتمل
    /// </summary>
    Completed = 2,

    /// <summary>
    /// تأجيل
    /// </summary>
    Postponed = 3,

    /// <summary>
    /// لم يحضر
    /// </summary>
    NoShow = 4,

    /// <summary>
    /// ملغي
    /// </summary>
    Cancelled = 5
}

/// <summary>
/// حالة الاختبار
/// </summary>
public enum ExamStatus
{
    /// <summary>
    /// لم يتم تحديد موعد
    /// </summary>
    NotScheduled = 0,

    /// <summary>
    /// مجدول
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// جاري
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// مكتمل
    /// </summary>
    Completed = 3,

    /// <summary>
    /// تأجيل
    /// </summary>
    Postponed = 4,

    /// <summary>
    /// لم يحضر
    /// </summary>
    NoShow = 5,

    /// <summary>
    /// ملغي
    /// </summary>
    Cancelled = 6
}

/// <summary>
/// نوع المستند
/// </summary>
public enum DocumentType
{
    /// <summary>
    /// صورة الهوية
    /// </summary>
    IDCard = 0,

    /// <summary>
    /// شهادة الميلاد
    /// </summary>
    BirthCertificate = 1,

    /// <summary>
    /// شهادة النجاح
    /// </summary>
    AcademicTranscript = 2,

    /// <summary>
    /// سجل التطعيم
    /// </summary>
    VaccinationRecord = 3,

    /// <summary>
    /// شهادة صحية
    /// </summary>
    MedicalCertificate = 4,

    /// <summary>
    /// صورة شخصية
    /// </summary>
    Photo = 5,

    /// <summary>
    /// إثبات سكن
    /// </summary>
    ResidenceProof = 6,

    /// <summary>
    /// شهادة نقل
    /// </summary>
    TransferCertificate = 7,

    /// <summary>
    /// مستندات أخرى
    /// </summary>
    Other = 99
}