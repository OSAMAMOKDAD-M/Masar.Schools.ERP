namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTO للحافلة
/// </summary>
public class BusDto
{
    public Guid Id { get; set; }
    public string BusNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public string? VehicleIdentificationNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public string? ImagePath { get; set; }
    public int? TenantId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? DriverName { get; set; }
    public string? SupervisorName { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث الحافلة
/// </summary>
public class BusCreateDto
{
    public string BusNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public string? VehicleIdentificationNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public string? ImagePath { get; set; }
}

/// <summary>
/// DTO لخط السير
/// </summary>
public class BusRouteDto
{
    public Guid Id { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public string RouteCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public decimal TotalDistance { get; set; }
    public int EstimatedDuration { get; set; }
    public string Direction { get; set; } = "Outbound";
    public bool IsActive { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<RouteStopDto> Stops { get; set; } = new();
}

/// <summary>
/// DTO لإنشاء/تحديث خط السير
/// </summary>
public class BusRouteCreateDto
{
    public string RouteName { get; set; } = string.Empty;
    public string RouteCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public decimal TotalDistance { get; set; }
    public int EstimatedDuration { get; set; }
    public string Direction { get; set; } = "Outbound";
    public List<RouteStopCreateDto> Stops { get; set; } = new();
}

/// <summary>
/// DTO للمحطة
/// </summary>
public class RouteStopDto
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public string StopName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Address { get; set; }
    public TimeSpan? EstimatedArrivalTime { get; set; }
    public string? Landmark { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث المحطة
/// </summary>
public class RouteStopCreateDto
{
    public string StopName { get; set; } = string.Empty;
    public int StopOrder { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Address { get; set; }
    public TimeSpan? EstimatedArrivalTime { get; set; }
    public string? Landmark { get; set; }
}

/// <summary>
/// DTO للسائق
/// </summary>
public class DriverDto
{
    public Guid Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime? LicenseExpiryDate { get; set; }
    public string? LicenseType { get; set; }
    public int? YearsOfExperience { get; set; }
    public bool IsActive { get; set; }
    public Guid? AssignedBusId { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? EmployeeName { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث السائق
/// </summary>
public class DriverCreateDto
{
    public Guid? EmployeeId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime? LicenseExpiryDate { get; set; }
    public string? LicenseType { get; set; }
    public int? YearsOfExperience { get; set; }
    public bool IsActive { get; set; }
    public Guid? AssignedBusId { get; set; }
}

/// <summary>
/// DTO للمشرف
/// </summary>
public class BusSupervisorDto
{
    public Guid Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContact { get; set; }
    public bool IsActive { get; set; }
    public Guid? AssignedBusId { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? EmployeeName { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث المشرف
/// </summary>
public class BusSupervisorCreateDto
{
    public Guid? EmployeeId { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContact { get; set; }
    public bool IsActive { get; set; }
    public Guid? AssignedBusId { get; set; }
}

/// <summary>
/// DTO لجدول الحافلة
/// </summary>
public class BusScheduleDto
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public Guid RouteId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? SupervisorId { get; set; }
    public string TripType { get; set; } = "Morning";
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public Guid? SchoolId { get; set; }
    public bool IsActive { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? BusNumber { get; set; }
    public string? RouteName { get; set; }
    public string? DriverName { get; set; }
    public string? SupervisorName { get; set; }
    public string? SchoolName { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث جدول الحافلة
/// </summary>
public class BusScheduleCreateDto
{
    public Guid BusId { get; set; }
    public Guid RouteId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? SupervisorId { get; set; }
    public string TripType { get; set; } = "Morning";
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public Guid? SchoolId { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO لاشتراك النقل
/// </summary>
public class StudentTransportSubscriptionDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid BusScheduleId { get; set; }
    public Guid RouteStopId { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public decimal MonthlyFee { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public bool IsActive { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? StudentName { get; set; }
    public string? RouteName { get; set; }
    public string? StopName { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث اشتراك النقل
/// </summary>
public class StudentTransportSubscriptionCreateDto
{
    public Guid StudentId { get; set; }
    public Guid BusScheduleId { get; set; }
    public Guid RouteStopId { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public decimal MonthlyFee { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO لسجل التتبع
/// </summary>
public class BusTrackingLogDto
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Speed { get; set; }
    public decimal Direction { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsMoving { get; set; }
    public int? BatteryLevel { get; set; }
    public int? SignalStrength { get; set; }
    public decimal? OdometerReading { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لحضور الحافلة
/// </summary>
public class BusAttendanceDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid BusScheduleId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public string AttendanceType { get; set; } = "Pickup";
    public Guid? StopId { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Status { get; set; } = "Present";
    public string? RecordedBy { get; set; }
    public string? Notes { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? StudentName { get; set; }
    public string? StopName { get; set; }
}

/// <summary>
/// DTO لحادث الحافلة
/// </summary>
public class BusIncidentDto
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public DateTime IncidentDate { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string Severity { get; set; } = "Medium";
    public string? ReportedBy { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لسجل الوقود
/// </summary>
public class FuelRecordDto
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public DateTime RefuelDate { get; set; }
    public decimal FuelAmount { get; set; }
    public decimal Cost { get; set; }
    public decimal? OdometerReading { get; set; }
    public string? FuelStation { get; set; }
    public string? RecordedBy { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO لسجل الصيانة
/// </summary>
public class MaintenanceRecordDto
{
    public Guid Id { get; set; }
    public Guid BusId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = "Routine";
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public string? Workshop { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public string? RecordedBy { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? BusNumber { get; set; }
}

/// <summary>
/// DTO للإحصائيات
/// </summary>
public class TransportStatisticsDto
{
    public int TotalBuses { get; set; }
    public int ActiveBuses { get; set; }
    public int BusesInMaintenance { get; set; }
    public int TotalRoutes { get; set; }
    public int ActiveRoutes { get; set; }
    public int TotalDrivers { get; set; }
    public int ActiveDrivers { get; set; }
    public int TotalSupervisors { get; set; }
    public int ActiveSupervisors { get; set; }
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int TotalIncidents { get; set; }
    public int ActiveIncidents { get; set; }
    public decimal TotalFuelCost { get; set; }
    public decimal TotalMaintenanceCost { get; set; }
}

/// <summary>
/// DTO لطلب الإجازة
/// </summary>
public class LeaveRequestDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? SchoolId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string LeaveTypeArabic { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalDays { get; set; }
    public string Status { get; set; } = "Pending";
    public string StatusArabic { get; set; } = "قيد الانتظار";
    public bool IsPaid { get; set; }
    public string? LeaveNumber { get; set; }
    public string? Reason { get; set; }
    public string? ReasonArabic { get; set; }
    public string? RejectionReason { get; set; }
    public string? RejectionReasonArabic { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public string? RejectedBy { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? EmployeeName { get; set; }
    public string? SchoolName { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث طلب الإجازة
/// </summary>
public class LeaveRequestCreateDto
{
    public Guid EmployeeId { get; set; }
    public Guid? SchoolId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string LeaveTypeArabic { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPaid { get; set; }
    public string? Reason { get; set; }
    public string? ReasonArabic { get; set; }
}

/// <summary>
/// DTO للموافقة/الرفض على طلب الإجازة
/// </summary>
public class LeaveRequestActionDto
{
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public string? RejectionReasonArabic { get; set; }
}

/// <summary>
/// DTO لرصيد الإجازات
/// </summary>
public class LeaveBalanceDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? SchoolId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string LeaveTypeArabic { get; set; } = string.Empty;
    public decimal TotalDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal RemainingDays { get; set; }
    public int FiscalYear { get; set; }
    public bool IsActive { get; set; }
    public int? TenantId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? EmployeeName { get; set; }
    public string? SchoolName { get; set; }
}

/// <summary>
/// DTO لإنشاء/تحديث رصيد الإجازات
/// </summary>
public class LeaveBalanceCreateDto
{
    public Guid EmployeeId { get; set; }
    public Guid? SchoolId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string LeaveTypeArabic { get; set; } = string.Empty;
    public decimal TotalDays { get; set; }
}
