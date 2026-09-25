using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان جدول الحافلة
/// </summary>
public class BusSchedule : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الحافلة
    /// </summary>
    public Guid BusId { get; set; }

    /// <summary>
    /// معرف الخط
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// معرف السائق
    /// </summary>
    public Guid? DriverId { get; set; }

    /// <summary>
    /// معرف المشرف
    /// </summary>
    public Guid? SupervisorId { get; set; }

    /// <summary>
    /// نوع الرحلة (Morning/Afternoon)
    /// </summary>
    public string TripType { get; set; } = "Morning";

    /// <summary>
    /// وقت المغادرة
    /// </summary>
    public TimeSpan DepartureTime { get; set; }

    /// <summary>
    /// وقت الوصول
    /// </summary>
    public TimeSpan ArrivalTime { get; set; }

    /// <summary>
    /// معرف المدرسة
    /// </summary>
    public Guid? SchoolId { get; set; }

    /// <summary>
    /// نشط/غير نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Bus Bus { get; set; } = null!;
    public virtual BusRoute Route { get; set; } = null!;
    public virtual Driver? Driver { get; set; }
    public virtual BusSupervisor? Supervisor { get; set; }
    public virtual School? School { get; set; }
    public virtual Tenant? Tenant { get; set; }
    public virtual ICollection<StudentTransportSubscription> Subscriptions { get; set; } = new List<StudentTransportSubscription>();
    public virtual ICollection<BusAttendance> AttendanceRecords { get; set; } = new List<BusAttendance>();
}
