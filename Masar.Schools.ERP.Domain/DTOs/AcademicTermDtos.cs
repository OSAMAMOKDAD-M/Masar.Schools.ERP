namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTO لطلب فتح فصل دراسي جديد
/// </summary>
public class OpenAcademicTermRequest
{
    /// <summary>
    /// اسم الفصل الدراسي (Term 1, Term 2, Term 3)
    /// </summary>
    public string TermName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الفصل بالعربية
    /// </summary>
    public string TermNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ البدء
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// تاريخ الانتهاء
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid SchoolId { get; set; }

    /// <summary>
    /// المراحل الدراسية المختارة للربط
    /// </summary>
    public List<Guid> SelectedGradeLevels { get; set; } = new();

    /// <summary>
    /// الصفوف المختارة للربط
    /// </summary>
    public List<Guid> SelectedGrades { get; set; } = new();

    /// <summary>
    /// الشعب المختارة للربط
    /// </summary>
    public List<Guid> SelectedSections { get; set; } = new();

    /// <summary>
    /// هل يتم نقل الطلاب تلقائياً
    /// </summary>
    public bool AutoPromoteStudents { get; set; } = true;

    /// <summary>
    /// هل يتم توليد جدول الرسوم تلقائياً
    /// </summary>
    public bool AutoGenerateFees { get; set; } = true;

    /// <summary>
    /// هل يتم تفعيل نظام الحضور بالبصمة
    /// </summary>
    public bool EnableBiometricAttendance { get; set; } = true;

    /// <summary>
    /// هل يتم تفعيل محرك التنبؤ بالمخاطر
    /// </summary>
    public bool EnableRiskPrediction { get; set; } = true;

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لنتيجة فتح الفصل الدراسي
/// </summary>
public class OpenAcademicTermResult
{
    /// <summary>
    /// هل تمت العملية بنجاح
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// رسالة النتيجة
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// معرف الفصل الدراسي المفتوح
    /// </summary>
    public Guid? TermId { get; set; }

    /// <summary>
    /// تفاصيل كل خطوة
    /// </summary>
    public List<TermOpeningStepResult> Steps { get; set; } = new();

    /// <summary>
    /// الأخطاء التي حدثت
    /// </summary>
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// DTO لنتيجة خطوة واحدة
/// </summary>
public class TermOpeningStepResult
{
    /// <summary>
    /// اسم الخطوة
    /// </summary>
    public string StepName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الخطوة بالعربية
    /// </summary>
    public string StepNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// هل تمت الخطوة بنجاح
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// رسالة الخطوة
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// عدد السجلات المعالجة
    /// </summary>
    public int RecordsProcessed { get; set; }

    /// <summary>
    /// وقت التنفيذ
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
}

/// <summary>
/// DTO لملخص البيانات قبل الفتح
/// </summary>
public class AcademicTermOpeningSummary
{
    /// <summary>
    /// عدد المراحل الدراسية
    /// </summary>
    public int TotalGradeLevels { get; set; }

    /// <summary>
    /// عدد الصفوف
    /// </summary>
    public int TotalGrades { get; set; }

    /// <summary>
    /// عدد الشعب
    /// </summary>
    public int TotalSections { get; set; }

    /// <summary>
    /// عدد الطلاب المتوقع نقلهم
    /// </summary>
    public int TotalStudentsToPromote { get; set; }

    /// <summary>
    /// عدد المعلمين المتاحين
    /// </summary>
    public int TotalTeachers { get; set; }

    /// <summary>
    /// عدد المواد الدراسية
    /// </summary>
    public int TotalSubjects { get; set; }

    /// <summary>
    /// القيمة المالية المتوقعة للرسوم
    /// </summary>
    public decimal ExpectedFeesAmount { get; set; }
}

/// <summary>
/// DTO لبيانات الفصل الدراسي للعرض
/// </summary>
public class AcademicTermDto
{
    public Guid Id { get; set; }
    public string TermName { get; set; } = string.Empty;
    public string TermNameArabic { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public string Status { get; set; } = "Pending";
    public string StatusArabic { get; set; } = "قيد الانتظار";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
