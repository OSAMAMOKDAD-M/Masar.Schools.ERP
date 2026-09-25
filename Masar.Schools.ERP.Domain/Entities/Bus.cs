using Masar.Schools.ERP.Domain.Common;
using System;

namespace Masar.Schools.ERP.Domain.Entities;

/// <summary>
/// كيان الحافلة
/// </summary>
public class Bus : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// رقم الحافلة
    /// </summary>
    public string BusNumber { get; set; } = string.Empty;

    /// <summary>
    /// رقم اللوحة
    /// </summary>
    public string PlateNumber { get; set; } = string.Empty;

    /// <summary>
    /// السعة (عدد الركاب)
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// نوع الحافلة (صغيرة/كبيرة/مكيفة)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// حالة الحافلة (Active/Maintenance/OutOfService)
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// رقم التعريف للمركبة (VIN)
    /// </summary>
    public string? VehicleIdentificationNumber { get; set; }

    /// <summary>
    /// تاريخ الشراء
    /// </summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>
    /// تاريخ آخر صيانة
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// تاريخ الصيانة القادمة
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// تاريخ انتهاء التأمين
    /// </summary>
    public DateTime? InsuranceExpiryDate { get; set; }

    /// <summary>
    /// تاريخ انتهاء الترخيص
    /// </summary>
    public DateTime? LicenseExpiryDate { get; set; }

    /// <summary>
    /// مسار صورة الحافلة
    /// </summary>
    public string? ImagePath { get; set; }

    /// <summary>
    /// معرف المستأجر
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// النشاط
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual Tenant? Tenant { get; set; }
    public virtual ICollection<BusRoute> Routes { get; set; } = new List<BusRoute>();
    public virtual ICollection<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
    public virtual ICollection<BusSupervisor> Supervisors { get; set; } = new List<BusSupervisor>();
    public virtual ICollection<BusTrackingLog> TrackingLogs { get; set; } = new List<BusTrackingLog>();
    public virtual ICollection<BusAttendance> AttendanceRecords { get; set; } = new List<BusAttendance>();
    public virtual ICollection<BusIncident> Incidents { get; set; } = new List<BusIncident>();
    public virtual ICollection<FuelRecord> FuelRecords { get; set; } = new List<FuelRecord>();
    public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
}
