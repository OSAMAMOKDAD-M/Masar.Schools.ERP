using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Clinic;

/// <summary>
/// زيارات العيادة المدرسية
/// </summary>
public class ClinicVisit : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    /// <summary>
    /// معرف الموظف الطبي/الممرض
    /// </summary>
    public Guid? AttendingEmployeeId { get; set; }
    public Employee? AttendingEmployee { get; set; }

    /// <summary>
    /// تاريخ الزيارة
    /// </summary>
    public DateTime VisitDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// وقت البدء
    /// </summary>
    public TimeSpan? StartTime { get; set; }

    /// <summary>
    /// وقت الانتهاء
    /// </summary>
    public TimeSpan? EndTime { get; set; }

    /// <summary>
    /// سبب الزيارة
    /// </summary>
    public string? VisitReason { get; set; }

    /// <summary>
    /// سبب الزيارة بالعربية
    /// </summary>
    public string? VisitReasonArabic { get; set; }

    /// <summary>
    /// نوع الزيارة (روتينية، عادية، طارئة)
    /// </summary>
    public string? VisitType { get; set; }

    /// <summary>
    /// نوع الزيارة بالعربية
    /// </summary>
    public string? VisitTypeArabic { get; set; }

    /// <summary>
    /// الأعراض المبلغ عنها
    /// </summary>
    public string? Symptoms { get; set; }

    /// <summary>
    /// الأعراض بالعربية
    /// </summary>
    public string? SymptomsArabic { get; set; }

    /// <summary>
    /// درجة الحرارة
    /// </summary>
    public decimal? Temperature { get; set; }

    /// <summary>
    /// ضغط الدم
    /// </summary>
    public int? BloodPressureSystolic { get; set; }

    /// <summary>
    /// ضغط الدم الانبسطي
    /// </summary>
    public int? BloodPressureDiastolic { get; set; }

    /// <summary>
    /// النبض
    /// </summary>
    public int? HeartRate { get; set; }

    /// <summary>
    /// معدل التنفس
    /// </summary>
    public int? RespiratoryRate { get; set; }

    /// <summary>
    /// الأكسجين
    /// </summary>
    public int? OxygenSaturation { get; set; }

    /// <summary>
    /// الوزن (كجم)
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// الطول (سم)
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// التشخيص المبدئي
    /// </summary>
    public string? PreliminaryDiagnosis { get; set; }

    /// <summary>
    /// التشخيص المبدئي بالعربية
    /// </summary>
    public string? PreliminaryDiagnosisArabic { get; set; }

    /// <summary>
    /// الإجراء المتبع
    /// </summary>
    public string? Treatment { get; set; }

    /// <summary>
    /// الإجراء المتبع بالعربية
    /// </summary>
    public string? TreatmentArabic { get; set; }

    /// <summary>
    /// الدواء المصروف
    /// </summary>
    public string? MedicationPrescribed { get; set; }

    /// <summary>
    /// الدواء المصروف بالعربية
    /// </summary>
    public string? MedicationPrescribedArabic { get; set; }

    /// <summary>
    /// الجرعة
    /// </summary>
    public string? Dosage { get; set; }

    /// <summary>
    /// الجرعة بالعربية
    /// </summary>
    public string? DosageArabic { get; set; }

    /// <summary>
    /// المدة
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// المدة بالعربية
    /// </summary>
    public string? DurationArabic { get; set; }

    /// <summary>
    /// حالة الطالب بعد الزيارة
    /// </summary>
    public string? PatientCondition { get; set; }

    /// <summary>
    /// حالة الطالب بالعربية
    /// </summary>
    public string? PatientConditionArabic { get; set; }

    /// <summary>
    /// هل تم إرجاع الطالب للفصل
    /// </summary>
    public bool ReturnedToClass { get; set; }

    /// <summary>
    /// تم تحويل الطالب للمستشفى
    /// </summary>
    public bool ReferredToHospital { get; set; }

    /// <summary>
    /// تم تحويل الطالب للمنزل
    /// </summary>
    public bool SentHome { get; set; }

    /// <summary>
    /// اسم المستشفى المحول إليه
    /// </summary>
    public string? ReferredHospital { get; set; }

    /// <summary>
    /// اسم المستشفى بالعربية
    /// </summary>
    public string? ReferredHospitalArabic { get; set; }

    /// <summary>
    /// سبب التحويل
    /// </summary>
    public string? ReferralReason { get; set; }

    /// <summary>
    /// سبب التحويل بالعربية
    /// </summary>
    public string? ReferralReasonArabic { get; set; }

    /// <summary>
    /// هل الزيارة طارئة
    /// </summary>
    public bool IsEmergency { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ملاحظات إضافية بالعربية
    /// </summary>
    public string? NotesArabic { get; set; }

    /// <summary>
    /// تاريخ الإنشاء
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// تم إرسال إشعار لولي الأمر
    /// </summary>
    public bool WhatsAppNotificationSent { get; set; }

    /// <summary>
    /// تاريخ إرسال الإشعار
    /// </summary>
    public DateTime? WhatsAppNotificationSentAt { get; set; }

    /// <summary>
    /// الطالب الصحي الكامل
    /// </summary>
    public StudentHealthProfile? HealthProfile { get; set; }
}