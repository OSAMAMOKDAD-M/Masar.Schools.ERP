using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان حضور الحافلة
/// </summary>
public class BusAttendance : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الطالب
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// معرف جدول الحافلة
    /// </summary>
    public Guid BusScheduleId { get; set; }

    /// <summary>
    /// تاريخ الحضور
    /// </summary>
    public DateTime AttendanceDate { get; set; }

    /// <summary>
    /// نوع الحضور (Pickup/Dropoff)
    /// </summary>
    public string AttendanceType { get; set; } = "Pickup";

    /// <summary>
    /// معرف المحطة
    /// </summary>
    public Guid? StopId { get; set; }

    /// <summary>
    /// وقت الصعود
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// وقت النزول
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// الحالة (Present/Absent)
    /// </summary>
    public string Status { get; set; } = "Present";

    /// <summary>
    /// المسجل بواسطة
    /// </summary>
    public string? RecordedBy { get; set; }

    /// <summary>
    /// ملاحظات
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Student Student { get; set; } = null!;
    public virtual BusSchedule BusSchedule { get; set; } = null!;
    public virtual RouteStop? Stop { get; set; }
    public virtual Tenant? Tenant { get; set; }
}
