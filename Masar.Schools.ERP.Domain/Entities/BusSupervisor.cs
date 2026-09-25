using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان مشرف الحافلة
/// </summary>
public class BusSupervisor : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// رقم الهاتف
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// رقم الطوارئ
    /// </summary>
    public string? EmergencyContact { get; set; }

    /// <summary>
    /// نشط/غير نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// معرف الحافلة المخصصة
    /// </summary>
    public Guid? AssignedBusId { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Employee? Employee { get; set; }
    public virtual Bus? AssignedBus { get; set; }
    public virtual Tenant? Tenant { get; set; }
    public virtual ICollection<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
}
