using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان سجل التتبع
/// </summary>
public class BusTrackingLog : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الحافلة
    /// </summary>
    public Guid BusId { get; set; }

    /// <summary>
    /// خط العرض
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// خط الطول
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// السرعة بالكيلومتر/ساعة
    /// </summary>
    public decimal Speed { get; set; }

    /// <summary>
    /// الاتجاه بالدرجات
    /// </summary>
    public decimal Direction { get; set; }

    /// <summary>
    /// وقت التسجيل
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// يتحرك/متوقف
    /// </summary>
    public bool IsMoving { get; set; }

    /// <summary>
    /// مستوى البطارية
    /// </summary>
    public int? BatteryLevel { get; set; }

    /// <summary>
    /// قوة الإشارة
    /// </summary>
    public int? SignalStrength { get; set; }

    /// <summary>
    /// عداد الكيلومترات
    /// </summary>
    public decimal? OdometerReading { get; set; }

    // Navigation Properties
    public virtual Bus Bus { get; set; } = null!;
}
