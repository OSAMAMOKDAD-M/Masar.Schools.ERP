using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان السائق
/// </summary>
public class Driver : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// رقم الرخصة
    /// </summary>
    public string LicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ انتهاء الرخصة
    /// </summary>
    public DateTime? LicenseExpiryDate { get; set; }

    /// <summary>
    /// نوع الرخصة
    /// </summary>
    public string? LicenseType { get; set; }

    /// <summary>
    /// سنوات الخبرة
    /// </summary>
    public int? YearsOfExperience { get; set; }

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
