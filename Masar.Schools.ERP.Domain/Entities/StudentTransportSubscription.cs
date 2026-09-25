using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان اشتراك النقل للطالب
/// </summary>
public class StudentTransportSubscription : BaseEntity
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
    /// معرف محطة الركوب
    /// </summary>
    public Guid RouteStopId { get; set; }

    /// <summary>
    /// تاريخ بداية الاشتراك
    /// </summary>
    public DateTime SubscriptionStartDate { get; set; }

    /// <summary>
    /// تاريخ نهاية الاشتراك
    /// </summary>
    public DateTime? SubscriptionEndDate { get; set; }

    /// <summary>
    /// الرسوم الشهرية
    /// </summary>
    public decimal MonthlyFee { get; set; }

    /// <summary>
    /// حالة الدفع
    /// </summary>
    public string PaymentStatus { get; set; } = "Pending";

    /// <summary>
    /// نشط/غير نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Student Student { get; set; } = null!;
    public virtual BusSchedule BusSchedule { get; set; } = null!;
    public virtual RouteStop RouteStop { get; set; } = null!;
    public virtual Tenant? Tenant { get; set; }
}
