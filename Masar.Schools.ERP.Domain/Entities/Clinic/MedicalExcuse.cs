using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Clinic;

/// <summary>
/// الإجازات والتقارير الطبية
/// </summary>
public class MedicalExcuse : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    /// <summary>
    /// معرف الزيارة المرتبطة
    /// </summary>
    public Guid? ClinicVisitId { get; set; }
    public ClinicVisit? ClinicVisit { get; set; }

    /// <summary>
    /// نوع الإجازة (مرضية، إصابة، رياضية، غذائية)
    /// </summary>
    public string ExcuseType { get; set; } = string.Empty;

    /// <summary>
    /// نوع الإجازة بالعربية
    /// </summary>
    public string? ExcuseTypeArabic { get; set; }

    /// <summary>
    /// سبب الإجازة
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// سبب الإجازة بالعربية
    /// </summary>
    public string? ReasonArabic { get; set; }

    /// <summary>
    /// الوصف التفصيلي
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// الوصف التفصيلي بالعربية
    /// </summary>
    public string? DescriptionArabic { get; set; }

    /// <summary>
    /// تاريخ بداية الإجازة
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ نهاية الإجازة
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// عدد أيام الإجازة
    /// </summary>
    public int NumberOfDays { get; set; }

    /// <summary>
    /// هل الإجازة مخصومة من الحضور
    /// </summary>
    public bool IsDeductible { get; set; }

    /// <summary>
    /// التوصيات الرياضية
    /// </summary>
    public string? PhysicalActivityRecommendations { get; set; }

    /// <summary>
    /// التوصيات الرياضية بالعربية
    /// </summary>
    public string? PhysicalActivityRecommendationsArabic { get; set; }

    /// <summary>
    /// التوصيات الغذائية
    /// </summary>
    public string? DietaryRecommendations { get; set; }

    /// <summary>
    /// التوصيات الغذائية بالعربية
    /// </summary>
    public string? DietaryRecommendationsArabic { get; set; }

    /// <summary>
    /// هل يمنع الطالب من ممارسة الرياضة
    /// </summary>
    public bool IsPhysicalActivityRestricted { get; set; }

    /// <summary>
    /// مدة منع الرياضة
    /// </summary>
    public string? PhysicalActivityRestrictionDuration { get; set; }

    /// <summary>
    /// مدة منع الرياضة بالعربية
    /// </summary>
    public string? PhysicalActivityRestrictionDurationArabic { get; set; }

    /// <summary>
    /// هل يمنع الطالب من بعض الأنشطة
    /// </summary>
    public bool IsActivityRestricted { get; set; }

    /// <summary>
    /// الأنشطة الممنوعة
    /// </summary>
    public string? RestrictedActivities { get; set; }

    /// <summary>
    /// الأنشطة الممنوعة بالعربية
    /// </summary>
    public string? RestrictedActivitiesArabic { get; set; }

    /// <summary>
    /// هل الإجازة تتطلب متابعة طبية
    /// </summary>
    public bool RequiresFollowUp { get; set; }

    /// <summary>
    /// تاريخ المتابعة التالية
    /// </summary>
    public DateTime? FollowUpDate { get; set; }

    /// <summary>
    /// ملاحظات المتابعة
    /// </summary>
    public string? FollowUpNotes { get; set; }

    /// <summary>
    /// ملاحظات المتابعة بالعربية
    /// </summary>
    public string? FollowUpNotesArabic { get; set; }

    /// <summary>
    /// الطبيب المعالج
    /// </summary>
    public string? PhysicianName { get; set; }

    /// <summary>
    /// الطبيب المعالج بالعربية
    /// </summary>
    public string? PhysicianNameArabic { get; set; }

    /// <summary>
    /// رقم تسجيل الطبيب
    /// </summary>
    public string? PhysicianLicenseNumber { get; set; }

    /// <summary>
    /// اسم المستشفى
    /// </summary>
    public string? HospitalName { get; set; }

    /// <summary>
    /// اسم المستشفى بالعربية
    /// </summary>
    public string? HospitalNameArabic { get; set; }

    /// <summary>
    /// هل الإجازة معتمدة من جهة خارجية
    /// </summary>
    public bool IsExternalReport { get; set; }

    /// <summary>
    /// تاريخ تقرير الطبيب الخارجي
    /// </summary>
    public DateTime? ExternalReportDate { get; set; }

    /// <summary>
    /// هل الإجازة نشطة
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// هل تم إرسال الإجازة للمعلمين
    /// </summary>
    public bool SentToTeachers { get; set; }

    /// <summary>
    /// تاريخ إرسال الإجازة للمعلمين
    /// </summary>
    public DateTime? SentToTeachersAt { get; set; }

    /// <summary>
    /// الموظف الطبي الذي أصدر الإجازة
    /// </summary>
    public Guid? IssuedByEmployeeId { get; set; }
    public Employee? IssuedByEmployee { get; set; }

    /// <summary>
    /// تاريخ الإصدار
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ملاحظات إضافية بالعربية
    /// </summary>
    public string? NotesArabic { get; set; }
}