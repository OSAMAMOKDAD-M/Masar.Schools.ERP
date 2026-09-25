using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Admissions;

/// <summary>
/// مقابلة واختبار القبول
/// </summary>
public class AdmissionExam : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// معرف طلب الالتحاق
    /// </summary>
    public Guid AdmissionApplicationId { get; set; }
    public AdmissionApplication AdmissionApplication { get; set; } = null!;

    /// <summary>
    /// حالة المقابلة
    /// </summary>
    public InterviewStatus InterviewStatus { get; set; } = InterviewStatus.NotScheduled;

    /// <summary>
    /// تاريخ المقابلة المجدول
    /// </summary>
    public DateTime? InterviewDate { get; set; }

    /// <summary>
    /// وقت المقابلة
    /// </summary>
    public TimeSpan? InterviewTime { get; set; }

    /// <summary>
    /// مكان المقابلة
    /// </summary>
    public string? InterviewLocation { get; set; }

    /// <summary>
    /// الموظف الذي أجرى المقابلة
    /// </summary>
    public Guid? InterviewerId { get; set; }
    public Employee? Interviewer { get; set; }

    /// <summary>
    /// تقييم المقابلة (من 1 إلى 10)
    /// </summary>
    public int? InterviewScore { get; set; }

    /// <summary>
    /// ملاحظات المقابلة
    /// </summary>
    public string? InterviewNotes { get; set; }

    /// <summary>
    /// حالة الاختبار
    /// </summary>
    public ExamStatus ExamStatus { get; set; } = ExamStatus.NotScheduled;

    /// <summary>
    /// تاريخ الاختبار المجدول
    /// </summary>
    public DateTime? ExamDate { get; set; }

    /// <summary>
    /// وقت الاختبار
    /// </summary>
    public TimeSpan? ExamTime { get; set; }

    /// <summary>
    /// مكان الاختبار
    /// </summary>
    public string? ExamLocation { get; set; }

    /// <summary>
    /// الموظف الذي أجرى الاختبار
    /// </summary>
    public Guid? ExaminerId { get; set; }
    public Employee? Examiner { get; set; }

    /// <summary>
    /// درجة اختبار العربية
    /// </summary>
    public decimal? ArabicScore { get; set; }

    /// <summary>
    /// درجة اختبار الرياضيات
    /// </summary>
    public decimal? MathScore { get; set; }

    /// <summary>
    /// درجة اختبار العلوم
    /// </summary>
    public decimal? ScienceScore { get; set; }

    /// <summary>
    /// درجة اختبار الإنجليزية
    /// </summary>
    public decimal? EnglishScore { get; set; }

    /// <summary>
    /// الدرجة الكلية
    /// </summary>
    public decimal? TotalScore { get; set; }

    /// <summary>
    /// النسبة المئوية
    /// </summary>
    public decimal? Percentage { get; set; }

    /// <summary>
    /// الدرجة الناجحة
    /// </summary>
    public decimal? PassingScore { get; set; }

    /// <summary>
    /// هل اجتاز الاختبار
    /// </summary>
    public bool? PassedExam { get; set; }

    /// <summary>
    /// ملاحظات الاختبار
    /// </summary>
    public string? ExamNotes { get; set; }

    /// <summary>
    /// ملاحظات طبية
    /// </summary>
    public string? MedicalNotes { get; set; }

    /// <summary>
    /// ملاحظات سلوكية
    /// </summary>
    public string? BehavioralNotes { get; set; }

    /// <summary>
    /// التوصية النهائية
    /// </summary>
    public string? FinalRecommendation { get; set; }

    /// <summary>
    /// التوصية النهائية بالعربية
    /// </summary>
    public string? FinalRecommendationArabic { get; set; }

    /// <summary>
    /// تاريخ التقييم النهائي
    /// </summary>
    public DateTime? EvaluationDate { get; set; }

    /// <summary>
    /// الموظف الذي قيم النتيجة النهائية
    /// </summary>
    public Guid? EvaluatedByEmployeeId { get; set; }
    public Employee? EvaluatedByEmployee { get; set; }

    /// <summary>
    /// تم إرسال إشعار واتساب للمقابلة
    /// </summary>
    public bool InterviewWhatsAppSent { get; set; }

    /// <summary>
    /// تاريخ إرسال إشعار المقابلة
    /// </summary>
    public DateTime? InterviewWhatsAppSentAt { get; set; }

    /// <summary>
    /// تم إرسال إشعار واتساب للاختبار
    /// </summary>
    public bool ExamWhatsAppSent { get; set; }

    /// <summary>
    /// تاريخ إرسال إشعار الاختبار
    /// </summary>
    public DateTime? ExamWhatsAppSentAt { get; set; }
}