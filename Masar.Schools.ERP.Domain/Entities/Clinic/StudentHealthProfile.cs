using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Clinic;

/// <summary>
/// الملف الصحي الشامل للطالب
/// </summary>
public class StudentHealthProfile : BaseEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    /// <summary>
    /// فصيلة الدم
    /// </summary>
    public string? BloodType { get; set; }

    /// <summary>
    /// عامل Rh
    /// </summary>
    public string? RhFactor { get; set; }

    /// <summary>
    /// الحساسية
    /// </summary>
    public string? Allergies { get; set; }

    /// <summary>
    /// الحساسية بالعربية
    /// </summary>
    public string? AllergiesArabic { get; set; }

    /// <summary>
    /// الأطعمة الممنوعة
    /// </summary>
    public string? ProhibitedFoods { get; set; }

    /// <summary>
    /// الأطعمة الممنوعة بالعربية
    /// </summary>
    public string? ProhibitedFoodsArabic { get; set; }

    /// <summary>
    /// الأمراض المزمنة
    /// </summary>
    public string? ChronicDiseases { get; set; }

    /// <summary>
    /// الأمراض المزمنة بالعربية
    /// </summary>
    public string? ChronicDiseasesArabic { get; set; }

    /// <summary>
    /// الإعاقات الجسدية
    /// </summary>
    public string? PhysicalDisabilities { get; set; }

    /// <summary>
    /// الإعاقات الجسدية بالعربية
    /// </summary>
    public string? PhysicalDisabilitiesArabic { get; set; }

    /// <summary>
    /// التطعيمات
    /// </summary>
    public string? Vaccinations { get; set; }

    /// <summary>
    /// التطعيمات بالعربية
    /// </summary>
    public string? VaccinationsArabic { get; set; }

    /// <summary>
    /// تاريخ آخر فحص طبي شامل
    /// </summary>
    public DateTime? LastComprehensiveCheckup { get; set; }

    /// <summary>
    /// القيود الغذائية
    /// </summary>
    public string? DietaryRestrictions { get; set; }

    /// <summary>
    /// القيود الغذائية بالعربية
    /// </summary>
    public string? DietaryRestrictionsArabic { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? AdditionalNotes { get; set; }

    /// <summary>
    /// ملاحظات إضافية بالعربية
    /// </summary>
    public string? AdditionalNotesArabic { get; set; }

    /// <summary>
    /// هل الطالب يحتاج إلى رعاية خاصة
    /// </summary>
    public bool RequiresSpecialCare { get; set; }

    /// <summary>
    /// نوع الرعاية المطلوبة
    /// </summary>
    public string? SpecialCareType { get; set; }

    /// <summary>
    /// نوع الرعاية المطلوبة بالعربية
    /// </summary>
    public string? SpecialCareTypeArabic { get; set; }

    /// <summary>
    /// اسم الطبيب المعالج
    /// </summary>
    public string? TreatingPhysician { get; set; }

    /// <summary>
    /// اسم الطبيب المعالج بالعربية
    /// </summary>
    public string? TreatingPhysicianArabic { get; set; }

    /// <summary>
    /// رقم هاتف الطبيب
    /// </summary>
    public string? PhysicianPhone { get; set; }

    /// <summary>
    /// مستشفى العلاج المفضل
    /// </summary>
    public string? PreferredHospital { get; set; }

    /// <summary>
    /// رقم التأمين الصحي
    /// </summary>
    public string? InsuranceNumber { get; set; }

    /// <summary>
    /// اسم شركة التأمين
    /// </summary>
    public string? InsuranceCompany { get; set; }

    /// <summary>
    /// تاريخ إنشاء الملف الصحي
    /// </summary>
    public DateTime ProfileCreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// آخر تحديث للملف الصحي
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// الموظف الذي أنشأ الملف
    /// </summary>
    public Guid? CreatedByEmployeeId { get; set; }
    public Employee? CreatedByEmployee { get; set; }

    /// <summary>
    /// زيارات العيادة للطالب
    /// </summary>
    public ICollection<ClinicVisit> ClinicVisits { get; set; } = new List<ClinicVisit>();
}