using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان حادث الحافلة
/// </summary>
public class BusIncident : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// معرف الحافلة
    /// </summary>
    public Guid BusId { get; set; }

    /// <summary>
    /// تاريخ الحادث
    /// </summary>
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// نوع الحادث (Accident/Breakdown/Delay)
    /// </summary>
    public string IncidentType { get; set; } = string.Empty;

    /// <summary>
    /// الوصف
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// الموقع
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// الشدة (Low/Medium/High)
    /// </summary>
    public string Severity { get; set; } = "Medium";

    /// <summary>
    /// تم الإبلاغ بواسطة
    /// </summary>
    public string? ReportedBy { get; set; }

    /// <summary>
    /// وقت الحل
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// ملاحظات الحل
    /// </summary>
    public string? ResolutionNotes { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    // Navigation Properties
    public virtual Bus Bus { get; set; } = null!;
    public virtual Tenant? Tenant { get; set; }
}
