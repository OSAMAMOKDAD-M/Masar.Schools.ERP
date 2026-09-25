using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان سجل الصيانة
/// </summary>
public class MaintenanceRecord : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الحافلة
    /// </summary>
    public Guid BusId { get; set; }

    /// <summary>
    /// تاريخ الصيانة
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// نوع الصيانة (Routine/Repair/Inspection)
    /// </summary>
    public string MaintenanceType { get; set; } = "Routine";

    /// <summary>
    /// الوصف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// التكلفة
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// الورشة
    /// </summary>
    public string? Workshop { get; set; }

    /// <summary>
    /// تاريخ الصيانة القادمة
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

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
