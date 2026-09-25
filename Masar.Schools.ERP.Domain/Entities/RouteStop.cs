using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان محطة الخط
/// </summary>
public class RouteStop : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الخط
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// اسم المحطة
    /// </summary>
    public string StopName { get; set; } = string.Empty;

    /// <summary>
    /// ترتيب المحطة
    /// </summary>
    public int StopOrder { get; set; }

    /// <summary>
    /// خط العرض
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// خط الطول
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// العنوان
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// وقت الوصول المتوقع
    /// </summary>
    public TimeSpan? EstimatedArrivalTime { get; set; }

    /// <summary>
    /// معلم مميز
    /// </summary>
    public string? Landmark { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual BusRoute Route { get; set; } = null!;
    public virtual Tenant? Tenant { get; set; }
    public virtual ICollection<StudentTransportSubscription> Subscriptions { get; set; } = new List<StudentTransportSubscription>();
    public virtual ICollection<BusAttendance> AttendanceRecords { get; set; } = new List<BusAttendance>();
}
