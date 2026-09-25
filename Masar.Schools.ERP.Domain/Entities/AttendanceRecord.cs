using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// طرق تسجيل الحضور المتاحة
/// </summary>
public enum AttendanceMethod
{
    Fingerprint = 1,
    RFID = 2,
    FaceRecognition = 3,
    Manual = 4,
    MobileApp = 5,
    QRCode = 6
}

public class AttendanceRecord : BaseEntity
{
    public DateTime Date { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Status { get; set; } = string.Empty; // Present, Absent, Late, Excused
    public string? Notes { get; set; }
    public string? DeviceId { get; set; }
    public bool IsAutomated { get; set; }
    public bool WhatsAppNotificationSent { get; set; }
    public DateTime? WhatsAppNotificationSentAt { get; set; }
    
    // الحقول الإضافية الجديدة
    /// <summary>
    /// طريقة تسجيل الحضور
    /// </summary>
    public AttendanceMethod Method { get; set; } = AttendanceMethod.Fingerprint;
    
    /// <summary>
    /// موقع الجهاز (للتحقق من الموقع)
    /// </summary>
    public string? DeviceLocation { get; set; }
    
    /// <summary>
    /// صورة إثبات (للتعرف على الوجه)
    /// </summary>
    public string? PhotoProof { get; set; }
    
    /// <summary>
    /// هل تسجيل يدوي يخالف البصمة
    /// </summary>
    public bool IsOverride { get; set; }
    
    /// <summary>
    /// سبب التسجيل اليدوي
    /// </summary>
    public string? OverrideReason { get; set; }
    
    /// <summary>
    /// من قام بالتسجيل اليدوي
    /// </summary>
    public Guid? OverrideBy { get; set; }
    
    /// <summary>
    /// عنوان IP للمستخدم
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User Agent (للتطبيق الجوال)
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// مدة التأخير بالدقائق
    /// </summary>
    public int? LateMinutes { get; set; }
    
    /// <summary>
    /// نوع التأخير (Tardy, ExcusedLate, etc.)
    /// </summary>
    public string? LateType { get; set; }
    
    /// <summary>
    /// هل الغياب مبرر
    /// </summary>
    public bool IsExcused { get; set; }
    
    /// <summary>
    /// سبب الغياب المبرر
    /// </summary>
    public string? ExcuseReason { get; set; }
    
    /// <summary>
    /// تاريخ تقديم العذر
    /// </summary>
    public DateTime? ExcuseSubmittedAt { get; set; }
    
    /// <summary>
    /// هل تم اعتماد الحضور
    /// </summary>
    public bool IsApproved { get; set; } = true;
    
    /// <summary>
    /// من اعتمد الحضور
    /// </summary>
    public Guid? ApprovedBy { get; set; }
    
    /// <summary>
    /// تاريخ الاعتماد
    /// </summary>
    public DateTime? ApprovedAt { get; set; }
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    
    public Guid ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;
}
