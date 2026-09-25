using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان خط السير
/// </summary>
public class BusRoute : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// اسم الخط
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// كود الخط
    /// </summary>
    public string RouteCode { get; set; } = string.Empty;

    /// <summary>
    /// الوصف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// نقطة البداية
    /// </summary>
    public string StartLocation { get; set; } = string.Empty;

    /// <summary>
    /// نقطة النهاية
    /// </summary>
    public string EndLocation { get; set; } = string.Empty;

    /// <summary>
    /// المسافة الكلية بالكيلومتر
    /// </summary>
    public decimal TotalDistance { get; set; }

    /// <summary>
    /// المدة المتوقعة بالدقائق
    /// </summary>
    public int EstimatedDuration { get; set; }

    /// <summary>
    /// الاتجاه (Inbound/Outbound)
    /// </summary>
    public string Direction { get; set; } = "Outbound";

    /// <summary>
    /// نشط/غير نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Tenant? Tenant { get; set; }
    public virtual ICollection<RouteStop> Stops { get; set; } = new List<RouteStop>();
    public virtual ICollection<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
    public virtual ICollection<StudentTransportSubscription> Subscriptions { get; set; } = new List<StudentTransportSubscription>();
}
