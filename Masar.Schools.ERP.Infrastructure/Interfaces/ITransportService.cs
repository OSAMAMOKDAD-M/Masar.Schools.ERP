using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة النقل
/// </summary>
public interface ITransportService
{
    // Bus Operations
    Task<List<BusDto>> GetAllBusesAsync();
    Task<BusDto?> GetBusByIdAsync(Guid id);
    Task<BusDto> CreateBusAsync(BusCreateDto dto);
    Task<BusDto?> UpdateBusAsync(Guid id, BusCreateDto dto);
    Task<bool> DeleteBusAsync(Guid id);
    Task<List<BusDto>> GetActiveBusesAsync();
    Task<List<BusDto>> GetBusesInMaintenanceAsync();

    // Route Operations
    Task<List<BusRouteDto>> GetAllRoutesAsync();
    Task<BusRouteDto?> GetRouteByIdAsync(Guid id);
    Task<BusRouteDto> CreateRouteAsync(BusRouteCreateDto dto);
    Task<BusRouteDto?> UpdateRouteAsync(Guid id, BusRouteCreateDto dto);
    Task<bool> DeleteRouteAsync(Guid id);
    Task<List<BusRouteDto>> GetActiveRoutesAsync();

    // Stop Operations
    Task<List<RouteStopDto>> GetStopsByRouteIdAsync(Guid routeId);
    Task<RouteStopDto?> GetStopByIdAsync(Guid id);
    Task<RouteStopDto> CreateStopAsync(RouteStopCreateDto dto);
    Task<RouteStopDto?> UpdateStopAsync(Guid id, RouteStopCreateDto dto);
    Task<bool> DeleteStopAsync(Guid id);

    // Driver Operations
    Task<List<DriverDto>> GetAllDriversAsync();
    Task<DriverDto?> GetDriverByIdAsync(Guid id);
    Task<DriverDto> CreateDriverAsync(DriverCreateDto dto);
    Task<DriverDto?> UpdateDriverAsync(Guid id, DriverCreateDto dto);
    Task<bool> DeleteDriverAsync(Guid id);
    Task<List<DriverDto>> GetActiveDriversAsync();

    // Supervisor Operations
    Task<List<BusSupervisorDto>> GetAllSupervisorsAsync();
    Task<BusSupervisorDto?> GetSupervisorByIdAsync(Guid id);
    Task<BusSupervisorDto> CreateSupervisorAsync(BusSupervisorCreateDto dto);
    Task<BusSupervisorDto?> UpdateSupervisorAsync(Guid id, BusSupervisorCreateDto dto);
    Task<bool> DeleteSupervisorAsync(Guid id);
    Task<List<BusSupervisorDto>> GetActiveSupervisorsAsync();

    // Schedule Operations
    Task<List<BusScheduleDto>> GetAllSchedulesAsync();
    Task<BusScheduleDto?> GetScheduleByIdAsync(Guid id);
    Task<BusScheduleDto> CreateScheduleAsync(BusScheduleCreateDto dto);
    Task<BusScheduleDto?> UpdateScheduleAsync(Guid id, BusScheduleCreateDto dto);
    Task<bool> DeleteScheduleAsync(Guid id);
    Task<List<BusScheduleDto>> GetSchedulesByBusIdAsync(Guid busId);
    Task<List<BusScheduleDto>> GetSchedulesByRouteIdAsync(Guid routeId);

    // Subscription Operations
    Task<List<StudentTransportSubscriptionDto>> GetAllSubscriptionsAsync();
    Task<StudentTransportSubscriptionDto?> GetSubscriptionByIdAsync(Guid id);
    Task<StudentTransportSubscriptionDto> CreateSubscriptionAsync(StudentTransportSubscriptionCreateDto dto);
    Task<StudentTransportSubscriptionDto?> UpdateSubscriptionAsync(Guid id, StudentTransportSubscriptionCreateDto dto);
    Task<bool> DeleteSubscriptionAsync(Guid id);
    Task<List<StudentTransportSubscriptionDto>> GetSubscriptionsByStudentIdAsync(Guid studentId);
    Task<List<StudentTransportSubscriptionDto>> GetSubscriptionsByScheduleIdAsync(Guid scheduleId);
    Task<List<StudentTransportSubscriptionDto>> GetActiveSubscriptionsAsync();

    // Tracking Operations
    Task<List<BusTrackingLogDto>> GetTrackingLogsByBusIdAsync(Guid busId, DateTime? startDate = null, DateTime? endDate = null);
    Task<BusTrackingLogDto?> GetLatestTrackingLogAsync(Guid busId);
    Task<BusTrackingLogDto> CreateTrackingLogAsync(BusTrackingLogDto dto);

    // Attendance Operations
    Task<List<BusAttendanceDto>> GetAttendanceByDateAsync(DateTime date);
    Task<List<BusAttendanceDto>> GetAttendanceByStudentIdAsync(Guid studentId);
    Task<List<BusAttendanceDto>> GetAttendanceByScheduleIdAsync(Guid scheduleId, DateTime date);
    Task<BusAttendanceDto> CreateAttendanceAsync(BusAttendanceDto dto);
    Task<BusAttendanceDto?> UpdateAttendanceAsync(Guid id, BusAttendanceDto dto);

    // Incident Operations
    Task<List<BusIncidentDto>> GetAllIncidentsAsync();
    Task<BusIncidentDto?> GetIncidentByIdAsync(Guid id);
    Task<BusIncidentDto> CreateIncidentAsync(BusIncidentDto dto);
    Task<BusIncidentDto?> UpdateIncidentAsync(Guid id, BusIncidentDto dto);
    Task<bool> DeleteIncidentAsync(Guid id);
    Task<List<BusIncidentDto>> GetIncidentsByBusIdAsync(Guid busId);
    Task<List<BusIncidentDto>> GetActiveIncidentsAsync();

    // Fuel Operations
    Task<List<FuelRecordDto>> GetFuelRecordsByBusIdAsync(Guid busId);
    Task<FuelRecordDto> CreateFuelRecordAsync(FuelRecordDto dto);
    Task<FuelRecordDto?> UpdateFuelRecordAsync(Guid id, FuelRecordDto dto);
    Task<bool> DeleteFuelRecordAsync(Guid id);

    // Maintenance Operations
    Task<List<MaintenanceRecordDto>> GetMaintenanceRecordsByBusIdAsync(Guid busId);
    Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(MaintenanceRecordDto dto);
    Task<MaintenanceRecordDto?> UpdateMaintenanceRecordAsync(Guid id, MaintenanceRecordDto dto);
    Task<bool> DeleteMaintenanceRecordAsync(Guid id);

    // Statistics
    Task<TransportStatisticsDto> GetStatisticsAsync();
}
