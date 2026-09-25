using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان سجل الوقود
/// </summary>
public class FuelRecord : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الحافلة
    /// </summary>
    public Guid BusId { get; set; }

    /// <summary>
    /// تاريخ التزود
    /// </summary>
    public DateTime RefuelDate { get; set; }

    /// <summary>
    /// كمية الوقود باللتر
    /// </summary>
    public decimal FuelAmount { get; set; }

    /// <summary>
    /// التكلفة
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// عداد الكيلومترات
    /// </summary>
    public decimal? OdometerReading { get; set; }

    /// <summary>
    /// محطة الوقود
    /// </summary>
    public string? FuelStation { get; set; }

    /// <summary>
    /// المسجل بواسطة
    /// </summary>
    public string? RecordedBy { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Bus Bus { get; set; } = null!;
    public virtual Tenant? Tenant { get; set; }
}
