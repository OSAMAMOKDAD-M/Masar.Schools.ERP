namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTO لطلب إغلاق الفصل الدراسي
/// </summary>
public class CloseAcademicTermRequest
{
    /// <summary>
    /// معرف الفصل الدراسي
    /// </summary>
    public Guid TermId { get; set; }

    /// <summary>
    /// هل إغلاق الكنترول والدرجات نهائياً
    /// </summary>
    public bool LockGradesAndControl { get; set; } = true;

    /// <summary>
    /// هل حساب النسب النهائية للحضور
    /// </summary>
    public bool FinalizeAttendance { get; set; } = true;

    /// <summary>
    /// هل إرسال تقارير الحضور لأولياء الأمور
    /// </summary>
    public bool SendAttendanceReports { get; set; } = true;

    /// <summary>
    /// هل إغلاق الدورة المحاسبية
    /// </summary>
    public bool CloseFinancialPeriod { get; set; } = true;

    /// <summary>
    /// هل مطابقة الفواتير مع ZATCA
    /// </summary>
    public bool AuditZatcaInvoices { get; set; } = true;

    /// <summary>
    /// هل ترحيل الطلاب للصف الأعلى
    /// </summary>
    public bool PromoteStudents { get; set; } = true;

    /// <summary>
    /// هل أرشفة السجلات
    /// </summary>
    public bool ArchiveRecords { get; set; } = true;

    /// <summary>
    /// هل فك ارتباط النقل
    /// </summary>
    public bool UnlinkTransport { get; set; } = true;

    /// <summary>
    /// ملاحظات الإغلاق
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لنتيجة إغلاق الفصل الدراسي
/// </summary>
public class CloseAcademicTermResult
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
    /// تفاصيل كل خطوة
    /// </summary>
    public List<TermClosingStepResult> Steps { get; set; } = new();

    /// <summary>
    /// الأخطاء التي حدثت
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// معرف الأرشيف المولد
    /// </summary>
    public Guid? ArchiveId { get; set; }
}

/// <summary>
/// DTO لنتيجة خطوة واحدة من الإغلاق
/// </summary>
public class TermClosingStepResult
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

    /// <summary>
    /// التحذيرات
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// DTO لفحص التحقق قبل الإغلاق
/// </summary>
public class TermClosingValidationResult
{
    /// <summary>
    /// هل يمكن إغلاق الفصل
    /// </summary>
    public bool CanClose { get; set; }

    /// <summary>
    /// رسالة الحالة
    /// </summary>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>
    /// حالة الكنترول والدرجات
    /// </summary>
    public ModuleValidationStatus GradesStatus { get; set; } = new();

    /// <summary>
    /// حالة الفواتير
    /// </summary>
    public ModuleValidationStatus InvoicesStatus { get; set; } = new();

    /// <summary>
    /// حالة الحضور
    /// </summary>
    public ModuleValidationStatus AttendanceStatus { get; set; } = new();

    /// <summary>
    /// حالة النقل
    /// </summary>
    public ModuleValidationStatus TransportStatus { get; set; } = new();

    /// <summary>
    /// عدد الطلاب الناجحين للترحيل
    /// </summary>
    public int StudentsToPromote { get; set; }

    /// <summary>
    /// عدد الطلاب الفاشلين
    /// </summary>
    public int StudentsToRetain { get; set; }

    /// <summary>
    /// إجمالي الفواتير المستحقة
    /// </summary>
    public decimal TotalInvoicesAmount { get; set; }

    /// <summary>
    /// إجمالي المبالغ المدفوعة
    /// </summary>
    public decimal TotalPaidAmount { get; set; }

    /// <summary>
    /// إجمالي المتأخرات
    /// </summary>
    public decimal TotalOutstandingAmount { get; set; }
}

/// <summary>
/// DTO لحالة موديول
/// </summary>
public class ModuleValidationStatus
{
    /// <summary>
    /// اسم الموديول
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الموديول بالعربية
    /// </summary>
    public string ModuleNameArabic { get; set; } = string.Empty;

    /// <summary>
    /// هل جاهز للإغلاق
    /// </summary>
    public bool IsReady { get; set; }

    /// <summary>
    /// النسبة المئوية للاكتمال
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// عدد السجلات المعلقة
    /// </summary>
    public int PendingRecords { get; set; }

    /// <summary>
    /// رسالة الحالة
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// قائمة التحذيرات
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// DTO لبيانات الأرشيف
/// </summary>
public class TermArchiveDto
{
    public Guid Id { get; set; }
    public Guid TermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public string TermNameArabic { get; set; } = string.Empty;
    public DateTime ClosingDate { get; set; }
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public int ArchivedStudentsCount { get; set; }
    public int ArchivedGradesCount { get; set; }
    public decimal ArchivedInvoicesTotal { get; set; }
    public string Status { get; set; } = "Archived";
    public string StatusArabic { get; set; } = "مؤرشف";
    public DateTime CreatedAt { get; set; }
}
