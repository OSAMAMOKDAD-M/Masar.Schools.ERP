using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة النقل
/// </summary>
public class TransportService : ITransportService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<TransportService> _logger;

    public TransportService(MasarDbContext context, ILogger<TransportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Bus Operations
    public async Task<List<BusDto>> GetAllBusesAsync()
    {
        var buses = await _context.Buses
            .Include(b => b.Drivers)
            .Include(b => b.Supervisors)
            .ToListAsync();

        return buses.Select(b => new BusDto
        {
            Id = b.Id,
            BusNumber = b.BusNumber,
            PlateNumber = b.PlateNumber,
            Capacity = b.Capacity,
            Type = b.Type,
            Status = b.Status,
            VehicleIdentificationNumber = b.VehicleIdentificationNumber,
            PurchaseDate = b.PurchaseDate,
            LastMaintenanceDate = b.LastMaintenanceDate,
            NextMaintenanceDate = b.NextMaintenanceDate,
            InsuranceExpiryDate = b.InsuranceExpiryDate,
            LicenseExpiryDate = b.LicenseExpiryDate,
            ImagePath = b.ImagePath,
            TenantId = b.TenantId,
            IsActive = b.IsActive,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt,
            DriverName = b.Drivers?.FirstOrDefault()?.Employee?.FullName,
            SupervisorName = b.Supervisors?.FirstOrDefault()?.Employee?.FullName
        }).ToList();
    }

    public async Task<BusDto?> GetBusByIdAsync(Guid id)
    {
        var bus = await _context.Buses
            .Include(b => b.Drivers)
            .Include(b => b.Supervisors)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bus == null) return null;

        return new BusDto
        {
            Id = bus.Id,
            BusNumber = bus.BusNumber,
            PlateNumber = bus.PlateNumber,
            Capacity = bus.Capacity,
            Type = bus.Type,
            Status = bus.Status,
            VehicleIdentificationNumber = bus.VehicleIdentificationNumber,
            PurchaseDate = bus.PurchaseDate,
            LastMaintenanceDate = bus.LastMaintenanceDate,
            NextMaintenanceDate = bus.NextMaintenanceDate,
            InsuranceExpiryDate = bus.InsuranceExpiryDate,
            LicenseExpiryDate = bus.LicenseExpiryDate,
            ImagePath = bus.ImagePath,
            TenantId = bus.TenantId,
            IsActive = bus.IsActive,
            CreatedAt = bus.CreatedAt,
            UpdatedAt = bus.UpdatedAt
        };
    }

    public async Task<BusDto> CreateBusAsync(BusCreateDto dto)
    {
        var bus = new Bus
        {
            Id = Guid.NewGuid(),
            BusNumber = dto.BusNumber,
            PlateNumber = dto.PlateNumber,
            Capacity = dto.Capacity,
            Type = dto.Type,
            Status = dto.Status,
            VehicleIdentificationNumber = dto.VehicleIdentificationNumber,
            PurchaseDate = dto.PurchaseDate,
            InsuranceExpiryDate = dto.InsuranceExpiryDate,
            LicenseExpiryDate = dto.LicenseExpiryDate,
            ImagePath = dto.ImagePath,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Buses.Add(bus);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new bus with ID: {BusId}", bus.Id);

        return new BusDto
        {
            Id = bus.Id,
            BusNumber = bus.BusNumber,
            PlateNumber = bus.PlateNumber,
            Capacity = bus.Capacity,
            Type = bus.Type,
            Status = bus.Status,
            VehicleIdentificationNumber = bus.VehicleIdentificationNumber,
            PurchaseDate = bus.PurchaseDate,
            InsuranceExpiryDate = bus.InsuranceExpiryDate,
            LicenseExpiryDate = bus.LicenseExpiryDate,
            ImagePath = bus.ImagePath,
            IsActive = bus.IsActive,
            CreatedAt = bus.CreatedAt
        };
    }

    public async Task<BusDto?> UpdateBusAsync(Guid id, BusCreateDto dto)
    {
        var bus = await _context.Buses.FindAsync(id);
        if (bus == null) return null;

        bus.BusNumber = dto.BusNumber;
        bus.PlateNumber = dto.PlateNumber;
        bus.Capacity = dto.Capacity;
        bus.Type = dto.Type;
        bus.Status = dto.Status;
        bus.VehicleIdentificationNumber = dto.VehicleIdentificationNumber;
        bus.PurchaseDate = dto.PurchaseDate;
        bus.InsuranceExpiryDate = dto.InsuranceExpiryDate;
        bus.LicenseExpiryDate = dto.LicenseExpiryDate;
        bus.ImagePath = dto.ImagePath;
        bus.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated bus with ID: {BusId}", bus.Id);

        return new BusDto
        {
            Id = bus.Id,
            BusNumber = bus.BusNumber,
            PlateNumber = bus.PlateNumber,
            Capacity = bus.Capacity,
            Type = bus.Type,
            Status = bus.Status,
            VehicleIdentificationNumber = bus.VehicleIdentificationNumber,
            PurchaseDate = bus.PurchaseDate,
            InsuranceExpiryDate = dto.InsuranceExpiryDate,
            LicenseExpiryDate = dto.LicenseExpiryDate,
            ImagePath = dto.ImagePath,
            TenantId = bus.TenantId,
            IsActive = bus.IsActive,
            CreatedAt = bus.CreatedAt,
            UpdatedAt = bus.UpdatedAt
        };
    }

    public async Task<bool> DeleteBusAsync(Guid id)
    {
        var bus = await _context.Buses.FindAsync(id);
        if (bus == null) return false;

        bus.IsDeleted = true;
        bus.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted bus with ID: {BusId}", bus.Id);

        return true;
    }

    public async Task<List<BusDto>> GetActiveBusesAsync()
    {
        var buses = await _context.Buses
            .Where(b => b.IsActive && b.Status == "Active")
            .ToListAsync();

        return buses.Select(b => new BusDto
        {
            Id = b.Id,
            BusNumber = b.BusNumber,
            PlateNumber = b.PlateNumber,
            Capacity = b.Capacity,
            Type = b.Type,
            Status = b.Status,
            IsActive = b.IsActive,
            CreatedAt = b.CreatedAt
        }).ToList();
    }

    public async Task<List<BusDto>> GetBusesInMaintenanceAsync()
    {
        var buses = await _context.Buses
            .Where(b => b.Status == "Maintenance")
            .ToListAsync();

        return buses.Select(b => new BusDto
        {
            Id = b.Id,
            BusNumber = b.BusNumber,
            PlateNumber = b.PlateNumber,
            Capacity = b.Capacity,
            Type = b.Type,
            Status = b.Status,
            IsActive = b.IsActive,
            CreatedAt = b.CreatedAt
        }).ToList();
    }

    // Route Operations
    public async Task<List<BusRouteDto>> GetAllRoutesAsync()
    {
        var routes = await _context.BusRoutes
            .Include(r => r.Stops)
            .ToListAsync();

        return routes.Select(r => new BusRouteDto
        {
            Id = r.Id,
            RouteName = r.RouteName,
            RouteCode = r.RouteCode,
            Description = r.Description,
            StartLocation = r.StartLocation,
            EndLocation = r.EndLocation,
            TotalDistance = r.TotalDistance,
            EstimatedDuration = r.EstimatedDuration,
            Direction = r.Direction,
            IsActive = r.IsActive,
            TenantId = r.TenantId,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            Stops = r.Stops.Select(s => new RouteStopDto
            {
                Id = s.Id,
                RouteId = s.RouteId,
                StopName = s.StopName,
                StopOrder = s.StopOrder,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Address = s.Address,
                EstimatedArrivalTime = s.EstimatedArrivalTime,
                Landmark = s.Landmark,
                TenantId = s.TenantId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList()
        }).ToList();
    }

    public async Task<BusRouteDto?> GetRouteByIdAsync(Guid id)
    {
        var route = await _context.BusRoutes
            .Include(r => r.Stops)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (route == null) return null;

        return new BusRouteDto
        {
            Id = route.Id,
            RouteName = route.RouteName,
            RouteCode = route.RouteCode,
            Description = route.Description,
            StartLocation = route.StartLocation,
            EndLocation = route.EndLocation,
            TotalDistance = route.TotalDistance,
            EstimatedDuration = route.EstimatedDuration,
            Direction = route.Direction,
            IsActive = route.IsActive,
            TenantId = route.TenantId,
            CreatedAt = route.CreatedAt,
            UpdatedAt = route.UpdatedAt,
            Stops = route.Stops.Select(s => new RouteStopDto
            {
                Id = s.Id,
                RouteId = s.RouteId,
                StopName = s.StopName,
                StopOrder = s.StopOrder,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Address = s.Address,
                EstimatedArrivalTime = s.EstimatedArrivalTime,
                Landmark = s.Landmark,
                TenantId = s.TenantId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList()
        };
    }

    public async Task<BusRouteDto> CreateRouteAsync(BusRouteCreateDto dto)
    {
        var route = new BusRoute
        {
            Id = Guid.NewGuid(),
            RouteName = dto.RouteName,
            RouteCode = dto.RouteCode,
            Description = dto.Description,
            StartLocation = dto.StartLocation,
            EndLocation = dto.EndLocation,
            TotalDistance = dto.TotalDistance,
            EstimatedDuration = dto.EstimatedDuration,
            Direction = dto.Direction,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.BusRoutes.Add(route);
        await _context.SaveChangesAsync();

        // Add stops
        foreach (var stopDto in dto.Stops)
        {
            var stop = new RouteStop
            {
                Id = Guid.NewGuid(),
                RouteId = route.Id,
                StopName = stopDto.StopName,
                StopOrder = stopDto.StopOrder,
                Latitude = stopDto.Latitude,
                Longitude = stopDto.Longitude,
                Address = stopDto.Address,
                EstimatedArrivalTime = stopDto.EstimatedArrivalTime,
                Landmark = stopDto.Landmark,
                CreatedAt = DateTime.UtcNow
            };
            _context.RouteStops.Add(stop);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new route with ID: {RouteId}", route.Id);

        return new BusRouteDto
        {
            Id = route.Id,
            RouteName = route.RouteName,
            RouteCode = route.RouteCode,
            Description = route.Description,
            StartLocation = route.StartLocation,
            EndLocation = route.EndLocation,
            TotalDistance = route.TotalDistance,
            EstimatedDuration = route.EstimatedDuration,
            Direction = route.Direction,
            IsActive = route.IsActive,
            CreatedAt = route.CreatedAt
        };
    }

    public async Task<BusRouteDto?> UpdateRouteAsync(Guid id, BusRouteCreateDto dto)
    {
        var route = await _context.BusRoutes.FindAsync(id);
        if (route == null) return null;

        route.RouteName = dto.RouteName;
        route.RouteCode = dto.RouteCode;
        route.Description = dto.Description;
        route.StartLocation = dto.StartLocation;
        route.EndLocation = dto.EndLocation;
        route.TotalDistance = dto.TotalDistance;
        route.EstimatedDuration = dto.EstimatedDuration;
        route.Direction = dto.Direction;
        route.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated route with ID: {RouteId}", route.Id);

        return new BusRouteDto
        {
            Id = route.Id,
            RouteName = route.RouteName,
            RouteCode = route.RouteCode,
            Description = route.Description,
            StartLocation = route.StartLocation,
            EndLocation = route.EndLocation,
            TotalDistance = route.TotalDistance,
            EstimatedDuration = route.EstimatedDuration,
            Direction = route.Direction,
            IsActive = route.IsActive,
            TenantId = route.TenantId,
            CreatedAt = route.CreatedAt,
            UpdatedAt = route.UpdatedAt
        };
    }

    public async Task<bool> DeleteRouteAsync(Guid id)
    {
        var route = await _context.BusRoutes.FindAsync(id);
        if (route == null) return false;

        route.IsDeleted = true;
        route.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted route with ID: {RouteId}", route.Id);

        return true;
    }

    public async Task<List<BusRouteDto>> GetActiveRoutesAsync()
    {
        var routes = await _context.BusRoutes
            .Where(r => r.IsActive)
            .ToListAsync();

        return routes.Select(r => new BusRouteDto
        {
            Id = r.Id,
            RouteName = r.RouteName,
            RouteCode = r.RouteCode,
            Description = r.Description,
            StartLocation = r.StartLocation,
            EndLocation = r.EndLocation,
            TotalDistance = r.TotalDistance,
            EstimatedDuration = r.EstimatedDuration,
            Direction = r.Direction,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    // Stop Operations
    public async Task<List<RouteStopDto>> GetStopsByRouteIdAsync(Guid routeId)
    {
        var stops = await _context.RouteStops
            .Where(s => s.RouteId == routeId)
            .OrderBy(s => s.StopOrder)
            .ToListAsync();

        return stops.Select(s => new RouteStopDto
        {
            Id = s.Id,
            RouteId = s.RouteId,
            StopName = s.StopName,
            StopOrder = s.StopOrder,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            Address = s.Address,
            EstimatedArrivalTime = s.EstimatedArrivalTime,
            Landmark = s.Landmark,
            TenantId = s.TenantId,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }

    public async Task<RouteStopDto?> GetStopByIdAsync(Guid id)
    {
        var stop = await _context.RouteStops.FindAsync(id);
        if (stop == null) return null;

        return new RouteStopDto
        {
            Id = stop.Id,
            RouteId = stop.RouteId,
            StopName = stop.StopName,
            StopOrder = stop.StopOrder,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,
            Address = stop.Address,
            EstimatedArrivalTime = stop.EstimatedArrivalTime,
            Landmark = stop.Landmark,
            TenantId = stop.TenantId,
            CreatedAt = stop.CreatedAt,
            UpdatedAt = stop.UpdatedAt
        };
    }

    public async Task<RouteStopDto> CreateStopAsync(RouteStopCreateDto dto)
    {
        var stop = new RouteStop
        {
            Id = Guid.NewGuid(),
            StopName = dto.StopName,
            StopOrder = dto.StopOrder,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Address = dto.Address,
            EstimatedArrivalTime = dto.EstimatedArrivalTime,
            Landmark = dto.Landmark,
            CreatedAt = DateTime.UtcNow
        };

        _context.RouteStops.Add(stop);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new stop with ID: {StopId}", stop.Id);

        return new RouteStopDto
        {
            Id = stop.Id,
            StopName = stop.StopName,
            StopOrder = stop.StopOrder,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,
            Address = stop.Address,
            EstimatedArrivalTime = stop.EstimatedArrivalTime,
            Landmark = stop.Landmark,
            CreatedAt = stop.CreatedAt
        };
    }

    public async Task<RouteStopDto?> UpdateStopAsync(Guid id, RouteStopCreateDto dto)
    {
        var stop = await _context.RouteStops.FindAsync(id);
        if (stop == null) return null;

        stop.StopName = dto.StopName;
        stop.StopOrder = dto.StopOrder;
        stop.Latitude = dto.Latitude;
        stop.Longitude = dto.Longitude;
        stop.Address = dto.Address;
        stop.EstimatedArrivalTime = dto.EstimatedArrivalTime;
        stop.Landmark = dto.Landmark;
        stop.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated stop with ID: {StopId}", stop.Id);

        return new RouteStopDto
        {
            Id = stop.Id,
            RouteId = stop.RouteId,
            StopName = stop.StopName,
            StopOrder = stop.StopOrder,
            Latitude = stop.Latitude,
            Longitude = stop.Longitude,
            Address = stop.Address,
            EstimatedArrivalTime = stop.EstimatedArrivalTime,
            Landmark = stop.Landmark,
            TenantId = stop.TenantId,
            CreatedAt = stop.CreatedAt,
            UpdatedAt = stop.UpdatedAt
        };
    }

    public async Task<bool> DeleteStopAsync(Guid id)
    {
        var stop = await _context.RouteStops.FindAsync(id);
        if (stop == null) return false;

        stop.IsDeleted = true;
        stop.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted stop with ID: {StopId}", stop.Id);

        return true;
    }

    // Driver Operations
    public async Task<List<DriverDto>> GetAllDriversAsync()
    {
        var drivers = await _context.Drivers
            .Include(d => d.Employee)
            .Include(d => d.AssignedBus)
            .ToListAsync();

        return drivers.Select(d => new DriverDto
        {
            Id = d.Id,
            EmployeeId = d.EmployeeId,
            LicenseNumber = d.LicenseNumber,
            LicenseExpiryDate = d.LicenseExpiryDate,
            LicenseType = d.LicenseType,
            YearsOfExperience = d.YearsOfExperience,
            IsActive = d.IsActive,
            AssignedBusId = d.AssignedBusId,
            TenantId = d.TenantId,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt,
            EmployeeName = d.Employee?.FullName,
            BusNumber = d.AssignedBus?.BusNumber
        }).ToList();
    }

    public async Task<DriverDto?> GetDriverByIdAsync(Guid id)
    {
        var driver = await _context.Drivers
            .Include(d => d.Employee)
            .Include(d => d.AssignedBus)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (driver == null) return null;

        return new DriverDto
        {
            Id = driver.Id,
            EmployeeId = driver.EmployeeId,
            LicenseNumber = driver.LicenseNumber,
            LicenseExpiryDate = driver.LicenseExpiryDate,
            LicenseType = driver.LicenseType,
            YearsOfExperience = driver.YearsOfExperience,
            IsActive = driver.IsActive,
            AssignedBusId = driver.AssignedBusId,
            TenantId = driver.TenantId,
            CreatedAt = driver.CreatedAt,
            UpdatedAt = driver.UpdatedAt,
            EmployeeName = driver.Employee?.FullName,
            BusNumber = driver.AssignedBus?.BusNumber
        };
    }

    public async Task<DriverDto> CreateDriverAsync(DriverCreateDto dto)
    {
        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            LicenseNumber = dto.LicenseNumber,
            LicenseExpiryDate = dto.LicenseExpiryDate,
            LicenseType = dto.LicenseType,
            YearsOfExperience = dto.YearsOfExperience,
            IsActive = dto.IsActive,
            AssignedBusId = dto.AssignedBusId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new driver with ID: {DriverId}", driver.Id);

        return new DriverDto
        {
            Id = driver.Id,
            EmployeeId = driver.EmployeeId,
            LicenseNumber = driver.LicenseNumber,
            LicenseExpiryDate = driver.LicenseExpiryDate,
            LicenseType = driver.LicenseType,
            YearsOfExperience = driver.YearsOfExperience,
            IsActive = driver.IsActive,
            AssignedBusId = driver.AssignedBusId,
            CreatedAt = driver.CreatedAt
        };
    }

    public async Task<DriverDto?> UpdateDriverAsync(Guid id, DriverCreateDto dto)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null) return null;

        driver.EmployeeId = dto.EmployeeId;
        driver.LicenseNumber = dto.LicenseNumber;
        driver.LicenseExpiryDate = dto.LicenseExpiryDate;
        driver.LicenseType = dto.LicenseType;
        driver.YearsOfExperience = dto.YearsOfExperience;
        driver.IsActive = dto.IsActive;
        driver.AssignedBusId = dto.AssignedBusId;
        driver.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated driver with ID: {DriverId}", driver.Id);

        return new DriverDto
        {
            Id = driver.Id,
            EmployeeId = driver.EmployeeId,
            LicenseNumber = driver.LicenseNumber,
            LicenseExpiryDate = driver.LicenseExpiryDate,
            LicenseType = driver.LicenseType,
            YearsOfExperience = driver.YearsOfExperience,
            IsActive = driver.IsActive,
            AssignedBusId = driver.AssignedBusId,
            TenantId = driver.TenantId,
            CreatedAt = driver.CreatedAt,
            UpdatedAt = driver.UpdatedAt
        };
    }

    public async Task<bool> DeleteDriverAsync(Guid id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null) return false;

        driver.IsDeleted = true;
        driver.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted driver with ID: {DriverId}", driver.Id);

        return true;
    }

    public async Task<List<DriverDto>> GetActiveDriversAsync()
    {
        var drivers = await _context.Drivers
            .Where(d => d.IsActive)
            .ToListAsync();

        return drivers.Select(d => new DriverDto
        {
            Id = d.Id,
            EmployeeId = d.EmployeeId,
            LicenseNumber = d.LicenseNumber,
            LicenseExpiryDate = d.LicenseExpiryDate,
            LicenseType = d.LicenseType,
            YearsOfExperience = d.YearsOfExperience,
            IsActive = d.IsActive,
            AssignedBusId = d.AssignedBusId,
            CreatedAt = d.CreatedAt
        }).ToList();
    }

    // Supervisor Operations
    public async Task<List<BusSupervisorDto>> GetAllSupervisorsAsync()
    {
        var supervisors = await _context.BusSupervisors
            .Include(s => s.Employee)
            .Include(s => s.AssignedBus)
            .ToListAsync();

        return supervisors.Select(s => new BusSupervisorDto
        {
            Id = s.Id,
            EmployeeId = s.EmployeeId,
            Phone = s.Phone,
            EmergencyContact = s.EmergencyContact,
            IsActive = s.IsActive,
            AssignedBusId = s.AssignedBusId,
            TenantId = s.TenantId,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            EmployeeName = s.Employee?.FullName,
            BusNumber = s.AssignedBus?.BusNumber
        }).ToList();
    }

    public async Task<BusSupervisorDto?> GetSupervisorByIdAsync(Guid id)
    {
        var supervisor = await _context.BusSupervisors
            .Include(s => s.Employee)
            .Include(s => s.AssignedBus)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (supervisor == null) return null;

        return new BusSupervisorDto
        {
            Id = supervisor.Id,
            EmployeeId = supervisor.EmployeeId,
            Phone = supervisor.Phone,
            EmergencyContact = supervisor.EmergencyContact,
            IsActive = supervisor.IsActive,
            AssignedBusId = supervisor.AssignedBusId,
            TenantId = supervisor.TenantId,
            CreatedAt = supervisor.CreatedAt,
            UpdatedAt = supervisor.UpdatedAt,
            EmployeeName = supervisor.Employee?.FullName,
            BusNumber = supervisor.AssignedBus?.BusNumber
        };
    }

    public async Task<BusSupervisorDto> CreateSupervisorAsync(BusSupervisorCreateDto dto)
    {
        var supervisor = new BusSupervisor
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            Phone = dto.Phone,
            EmergencyContact = dto.EmergencyContact,
            IsActive = dto.IsActive,
            AssignedBusId = dto.AssignedBusId,
            CreatedAt = DateTime.UtcNow
        };

        _context.BusSupervisors.Add(supervisor);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new supervisor with ID: {SupervisorId}", supervisor.Id);

        return new BusSupervisorDto
        {
            Id = supervisor.Id,
            EmployeeId = supervisor.EmployeeId,
            Phone = supervisor.Phone,
            EmergencyContact = supervisor.EmergencyContact,
            IsActive = supervisor.IsActive,
            AssignedBusId = supervisor.AssignedBusId,
            CreatedAt = supervisor.CreatedAt
        };
    }

    public async Task<BusSupervisorDto?> UpdateSupervisorAsync(Guid id, BusSupervisorCreateDto dto)
    {
        var supervisor = await _context.BusSupervisors.FindAsync(id);
        if (supervisor == null) return null;

        supervisor.EmployeeId = dto.EmployeeId;
        supervisor.Phone = dto.Phone;
        supervisor.EmergencyContact = dto.EmergencyContact;
        supervisor.IsActive = dto.IsActive;
        supervisor.AssignedBusId = dto.AssignedBusId;
        supervisor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated supervisor with ID: {SupervisorId}", supervisor.Id);

        return new BusSupervisorDto
        {
            Id = supervisor.Id,
            EmployeeId = supervisor.EmployeeId,
            Phone = supervisor.Phone,
            EmergencyContact = supervisor.EmergencyContact,
            IsActive = supervisor.IsActive,
            AssignedBusId = supervisor.AssignedBusId,
            TenantId = supervisor.TenantId,
            CreatedAt = supervisor.CreatedAt,
            UpdatedAt = supervisor.UpdatedAt
        };
    }

    public async Task<bool> DeleteSupervisorAsync(Guid id)
    {
        var supervisor = await _context.BusSupervisors.FindAsync(id);
        if (supervisor == null) return false;

        supervisor.IsDeleted = true;
        supervisor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted supervisor with ID: {SupervisorId}", supervisor.Id);

        return true;
    }

    public async Task<List<BusSupervisorDto>> GetActiveSupervisorsAsync()
    {
        var supervisors = await _context.BusSupervisors
            .Where(s => s.IsActive)
            .ToListAsync();

        return supervisors.Select(s => new BusSupervisorDto
        {
            Id = s.Id,
            EmployeeId = s.EmployeeId,
            Phone = s.Phone,
            EmergencyContact = s.EmergencyContact,
            IsActive = s.IsActive,
            AssignedBusId = s.AssignedBusId,
            CreatedAt = s.CreatedAt
        }).ToList();
    }

    // Schedule Operations
    public async Task<List<BusScheduleDto>> GetAllSchedulesAsync()
    {
        var schedules = await _context.BusSchedules
            .Include(s => s.Bus)
            .Include(s => s.Route)
            .Include(s => s.Driver)
            .Include(s => s.Supervisor)
            .Include(s => s.School)
            .ToListAsync();

        return schedules.Select(s => new BusScheduleDto
        {
            Id = s.Id,
            BusId = s.BusId,
            RouteId = s.RouteId,
            DriverId = s.DriverId,
            SupervisorId = s.SupervisorId,
            TripType = s.TripType,
            DepartureTime = s.DepartureTime,
            ArrivalTime = s.ArrivalTime,
            SchoolId = s.SchoolId,
            IsActive = s.IsActive,
            TenantId = s.TenantId,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            BusNumber = s.Bus?.BusNumber,
            RouteName = s.Route?.RouteName,
            DriverName = s.Driver?.Employee?.FullName,
            SupervisorName = s.Supervisor?.Employee?.FullName,
            SchoolName = s.School?.NameArabic
        }).ToList();
    }

    public async Task<BusScheduleDto?> GetScheduleByIdAsync(Guid id)
    {
        var schedule = await _context.BusSchedules
            .Include(s => s.Bus)
            .Include(s => s.Route)
            .Include(s => s.Driver)
            .Include(s => s.Supervisor)
            .Include(s => s.School)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (schedule == null) return null;

        return new BusScheduleDto
        {
            Id = schedule.Id,
            BusId = schedule.BusId,
            RouteId = schedule.RouteId,
            DriverId = schedule.DriverId,
            SupervisorId = schedule.SupervisorId,
            TripType = schedule.TripType,
            DepartureTime = schedule.DepartureTime,
            ArrivalTime = schedule.ArrivalTime,
            SchoolId = schedule.SchoolId,
            IsActive = schedule.IsActive,
            TenantId = schedule.TenantId,
            CreatedAt = schedule.CreatedAt,
            UpdatedAt = schedule.UpdatedAt,
            BusNumber = schedule.Bus?.BusNumber,
            RouteName = schedule.Route?.RouteName,
            DriverName = schedule.Driver?.Employee?.FullName,
            SupervisorName = schedule.Supervisor?.Employee?.FullName,
            SchoolName = schedule.School?.NameArabic
        };
    }

    public async Task<BusScheduleDto> CreateScheduleAsync(BusScheduleCreateDto dto)
    {
        var schedule = new BusSchedule
        {
            Id = Guid.NewGuid(),
            BusId = dto.BusId,
            RouteId = dto.RouteId,
            DriverId = dto.DriverId,
            SupervisorId = dto.SupervisorId,
            TripType = dto.TripType,
            DepartureTime = dto.DepartureTime,
            ArrivalTime = dto.ArrivalTime,
            SchoolId = dto.SchoolId,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.BusSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new schedule with ID: {ScheduleId}", schedule.Id);

        return new BusScheduleDto
        {
            Id = schedule.Id,
            BusId = schedule.BusId,
            RouteId = schedule.RouteId,
            DriverId = schedule.DriverId,
            SupervisorId = schedule.SupervisorId,
            TripType = schedule.TripType,
            DepartureTime = schedule.DepartureTime,
            ArrivalTime = schedule.ArrivalTime,
            SchoolId = schedule.SchoolId,
            IsActive = schedule.IsActive,
            CreatedAt = schedule.CreatedAt
        };
    }

    public async Task<BusScheduleDto?> UpdateScheduleAsync(Guid id, BusScheduleCreateDto dto)
    {
        var schedule = await _context.BusSchedules.FindAsync(id);
        if (schedule == null) return null;

        schedule.BusId = dto.BusId;
        schedule.RouteId = dto.RouteId;
        schedule.DriverId = dto.DriverId;
        schedule.SupervisorId = dto.SupervisorId;
        schedule.TripType = dto.TripType;
        schedule.DepartureTime = dto.DepartureTime;
        schedule.ArrivalTime = dto.ArrivalTime;
        schedule.SchoolId = dto.SchoolId;
        schedule.IsActive = dto.IsActive;
        schedule.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated schedule with ID: {ScheduleId}", schedule.Id);

        return new BusScheduleDto
        {
            Id = schedule.Id,
            BusId = schedule.BusId,
            RouteId = schedule.RouteId,
            DriverId = schedule.DriverId,
            SupervisorId = schedule.SupervisorId,
            TripType = schedule.TripType,
            DepartureTime = schedule.DepartureTime,
            ArrivalTime = schedule.ArrivalTime,
            SchoolId = schedule.SchoolId,
            IsActive = schedule.IsActive,
            TenantId = schedule.TenantId,
            CreatedAt = schedule.CreatedAt,
            UpdatedAt = schedule.UpdatedAt
        };
    }

    public async Task<bool> DeleteScheduleAsync(Guid id)
    {
        var schedule = await _context.BusSchedules.FindAsync(id);
        if (schedule == null) return false;

        schedule.IsDeleted = true;
        schedule.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted schedule with ID: {ScheduleId}", schedule.Id);

        return true;
    }

    public async Task<List<BusScheduleDto>> GetSchedulesByBusIdAsync(Guid busId)
    {
        var schedules = await _context.BusSchedules
            .Where(s => s.BusId == busId)
            .Include(s => s.Bus)
            .Include(s => s.Route)
            .ToListAsync();

        return schedules.Select(s => new BusScheduleDto
        {
            Id = s.Id,
            BusId = s.BusId,
            RouteId = s.RouteId,
            DriverId = s.DriverId,
            SupervisorId = s.SupervisorId,
            TripType = s.TripType,
            DepartureTime = s.DepartureTime,
            ArrivalTime = s.ArrivalTime,
            SchoolId = s.SchoolId,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            BusNumber = s.Bus?.BusNumber,
            RouteName = s.Route?.RouteName
        }).ToList();
    }

    public async Task<List<BusScheduleDto>> GetSchedulesByRouteIdAsync(Guid routeId)
    {
        var schedules = await _context.BusSchedules
            .Where(s => s.RouteId == routeId)
            .Include(s => s.Bus)
            .Include(s => s.Route)
            .ToListAsync();

        return schedules.Select(s => new BusScheduleDto
        {
            Id = s.Id,
            BusId = s.BusId,
            RouteId = s.RouteId,
            DriverId = s.DriverId,
            SupervisorId = s.SupervisorId,
            TripType = s.TripType,
            DepartureTime = s.DepartureTime,
            ArrivalTime = s.ArrivalTime,
            SchoolId = s.SchoolId,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            BusNumber = s.Bus?.BusNumber,
            RouteName = s.Route?.RouteName
        }).ToList();
    }

    // Subscription Operations
    public async Task<List<StudentTransportSubscriptionDto>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _context.StudentTransportSubscriptions
            .Include(s => s.Student)
            .Include(s => s.BusSchedule)
            .Include(s => s.RouteStop)
            .ToListAsync();

        return subscriptions.Select(s => new StudentTransportSubscriptionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            BusScheduleId = s.BusScheduleId,
            RouteStopId = s.RouteStopId,
            SubscriptionStartDate = s.SubscriptionStartDate,
            SubscriptionEndDate = s.SubscriptionEndDate,
            MonthlyFee = s.MonthlyFee,
            PaymentStatus = s.PaymentStatus,
            IsActive = s.IsActive,
            TenantId = s.TenantId,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            StudentName = s.Student?.FullNameArabic,
            RouteName = s.BusSchedule?.Route?.RouteName,
            StopName = s.RouteStop?.StopName,
            BusNumber = s.BusSchedule?.Bus?.BusNumber
        }).ToList();
    }

    public async Task<StudentTransportSubscriptionDto?> GetSubscriptionByIdAsync(Guid id)
    {
        var subscription = await _context.StudentTransportSubscriptions
            .Include(s => s.Student)
            .Include(s => s.BusSchedule)
            .Include(s => s.RouteStop)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (subscription == null) return null;

        return new StudentTransportSubscriptionDto
        {
            Id = subscription.Id,
            StudentId = subscription.StudentId,
            BusScheduleId = subscription.BusScheduleId,
            RouteStopId = subscription.RouteStopId,
            SubscriptionStartDate = subscription.SubscriptionStartDate,
            SubscriptionEndDate = subscription.SubscriptionEndDate,
            MonthlyFee = subscription.MonthlyFee,
            PaymentStatus = subscription.PaymentStatus,
            IsActive = subscription.IsActive,
            TenantId = subscription.TenantId,
            CreatedAt = subscription.CreatedAt,
            UpdatedAt = subscription.UpdatedAt,
            StudentName = subscription.Student?.FullNameArabic,
            RouteName = subscription.BusSchedule?.Route?.RouteName,
            StopName = subscription.RouteStop?.StopName,
            BusNumber = subscription.BusSchedule?.Bus?.BusNumber
        };
    }

    public async Task<StudentTransportSubscriptionDto> CreateSubscriptionAsync(StudentTransportSubscriptionCreateDto dto)
    {
        var subscription = new StudentTransportSubscription
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            BusScheduleId = dto.BusScheduleId,
            RouteStopId = dto.RouteStopId,
            SubscriptionStartDate = dto.SubscriptionStartDate,
            SubscriptionEndDate = dto.SubscriptionEndDate,
            MonthlyFee = dto.MonthlyFee,
            PaymentStatus = dto.PaymentStatus,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.StudentTransportSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new subscription with ID: {SubscriptionId}", subscription.Id);

        return new StudentTransportSubscriptionDto
        {
            Id = subscription.Id,
            StudentId = subscription.StudentId,
            BusScheduleId = subscription.BusScheduleId,
            RouteStopId = subscription.RouteStopId,
            SubscriptionStartDate = subscription.SubscriptionStartDate,
            SubscriptionEndDate = subscription.SubscriptionEndDate,
            MonthlyFee = subscription.MonthlyFee,
            PaymentStatus = subscription.PaymentStatus,
            IsActive = subscription.IsActive,
            CreatedAt = subscription.CreatedAt
        };
    }

    public async Task<StudentTransportSubscriptionDto?> UpdateSubscriptionAsync(Guid id, StudentTransportSubscriptionCreateDto dto)
    {
        var subscription = await _context.StudentTransportSubscriptions.FindAsync(id);
        if (subscription == null) return null;

        subscription.StudentId = dto.StudentId;
        subscription.BusScheduleId = dto.BusScheduleId;
        subscription.RouteStopId = dto.RouteStopId;
        subscription.SubscriptionStartDate = dto.SubscriptionStartDate;
        subscription.SubscriptionEndDate = dto.SubscriptionEndDate;
        subscription.MonthlyFee = dto.MonthlyFee;
        subscription.PaymentStatus = dto.PaymentStatus;
        subscription.IsActive = dto.IsActive;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated subscription with ID: {SubscriptionId}", subscription.Id);

        return new StudentTransportSubscriptionDto
        {
            Id = subscription.Id,
            StudentId = subscription.StudentId,
            BusScheduleId = subscription.BusScheduleId,
            RouteStopId = subscription.RouteStopId,
            SubscriptionStartDate = subscription.SubscriptionStartDate,
            SubscriptionEndDate = subscription.SubscriptionEndDate,
            MonthlyFee = subscription.MonthlyFee,
            PaymentStatus = subscription.PaymentStatus,
            IsActive = subscription.IsActive,
            TenantId = subscription.TenantId,
            CreatedAt = subscription.CreatedAt,
            UpdatedAt = subscription.UpdatedAt
        };
    }

    public async Task<bool> DeleteSubscriptionAsync(Guid id)
    {
        var subscription = await _context.StudentTransportSubscriptions.FindAsync(id);
        if (subscription == null) return false;

        subscription.IsDeleted = true;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted subscription with ID: {SubscriptionId}", subscription.Id);

        return true;
    }

    public async Task<List<StudentTransportSubscriptionDto>> GetSubscriptionsByStudentIdAsync(Guid studentId)
    {
        var subscriptions = await _context.StudentTransportSubscriptions
            .Where(s => s.StudentId == studentId)
            .Include(s => s.Student)
            .Include(s => s.BusSchedule)
            .Include(s => s.RouteStop)
            .ToListAsync();

        return subscriptions.Select(s => new StudentTransportSubscriptionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            BusScheduleId = s.BusScheduleId,
            RouteStopId = s.RouteStopId,
            SubscriptionStartDate = s.SubscriptionStartDate,
            SubscriptionEndDate = s.SubscriptionEndDate,
            MonthlyFee = s.MonthlyFee,
            PaymentStatus = s.PaymentStatus,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            StudentName = s.Student?.FullNameArabic,
            RouteName = s.BusSchedule?.Route?.RouteName,
            StopName = s.RouteStop?.StopName,
            BusNumber = s.BusSchedule?.Bus?.BusNumber
        }).ToList();
    }

    public async Task<List<StudentTransportSubscriptionDto>> GetSubscriptionsByScheduleIdAsync(Guid scheduleId)
    {
        var subscriptions = await _context.StudentTransportSubscriptions
            .Where(s => s.BusScheduleId == scheduleId)
            .Include(s => s.Student)
            .Include(s => s.BusSchedule)
            .Include(s => s.RouteStop)
            .ToListAsync();

        return subscriptions.Select(s => new StudentTransportSubscriptionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            BusScheduleId = s.BusScheduleId,
            RouteStopId = s.RouteStopId,
            SubscriptionStartDate = s.SubscriptionStartDate,
            SubscriptionEndDate = s.SubscriptionEndDate,
            MonthlyFee = s.MonthlyFee,
            PaymentStatus = s.PaymentStatus,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            StudentName = s.Student?.FullNameArabic,
            RouteName = s.BusSchedule?.Route?.RouteName,
            StopName = s.RouteStop?.StopName,
            BusNumber = s.BusSchedule?.Bus?.BusNumber
        }).ToList();
    }

    public async Task<List<StudentTransportSubscriptionDto>> GetActiveSubscriptionsAsync()
    {
        var subscriptions = await _context.StudentTransportSubscriptions
            .Where(s => s.IsActive)
            .Include(s => s.Student)
            .Include(s => s.BusSchedule)
            .Include(s => s.RouteStop)
            .ToListAsync();

        return subscriptions.Select(s => new StudentTransportSubscriptionDto
        {
            Id = s.Id,
            StudentId = s.StudentId,
            BusScheduleId = s.BusScheduleId,
            RouteStopId = s.RouteStopId,
            SubscriptionStartDate = s.SubscriptionStartDate,
            SubscriptionEndDate = s.SubscriptionEndDate,
            MonthlyFee = s.MonthlyFee,
            PaymentStatus = s.PaymentStatus,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            StudentName = s.Student?.FullNameArabic,
            RouteName = s.BusSchedule?.Route?.RouteName,
            StopName = s.RouteStop?.StopName,
            BusNumber = s.BusSchedule?.Bus?.BusNumber
        }).ToList();
    }

    // Tracking Operations
    public async Task<List<BusTrackingLogDto>> GetTrackingLogsByBusIdAsync(Guid busId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.BusTrackingLogs
            .Where(l => l.BusId == busId);

        if (startDate.HasValue)
            query = query.Where(l => l.Timestamp >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(l => l.Timestamp <= endDate.Value);

        var logs = await query
            .Include(l => l.Bus)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();

        return logs.Select(l => new BusTrackingLogDto
        {
            Id = l.Id,
            BusId = l.BusId,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Speed = l.Speed,
            Direction = l.Direction,
            Timestamp = l.Timestamp,
            IsMoving = l.IsMoving,
            BatteryLevel = l.BatteryLevel,
            SignalStrength = l.SignalStrength,
            OdometerReading = l.OdometerReading,
            BusNumber = l.Bus?.BusNumber
        }).ToList();
    }

    public async Task<BusTrackingLogDto?> GetLatestTrackingLogAsync(Guid busId)
    {
        var log = await _context.BusTrackingLogs
            .Include(l => l.Bus)
            .Where(l => l.BusId == busId)
            .OrderByDescending(l => l.Timestamp)
            .FirstOrDefaultAsync();

        if (log == null) return null;

        return new BusTrackingLogDto
        {
            Id = log.Id,
            BusId = log.BusId,
            Latitude = log.Latitude,
            Longitude = log.Longitude,
            Speed = log.Speed,
            Direction = log.Direction,
            Timestamp = log.Timestamp,
            IsMoving = log.IsMoving,
            BatteryLevel = log.BatteryLevel,
            SignalStrength = log.SignalStrength,
            OdometerReading = log.OdometerReading,
            BusNumber = log.Bus?.BusNumber
        };
    }

    public async Task<BusTrackingLogDto> CreateTrackingLogAsync(BusTrackingLogDto dto)
    {
        var log = new BusTrackingLog
        {
            Id = Guid.NewGuid(),
            BusId = dto.BusId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Speed = dto.Speed,
            Direction = dto.Direction,
            Timestamp = DateTime.UtcNow,
            IsMoving = dto.IsMoving,
            BatteryLevel = dto.BatteryLevel,
            SignalStrength = dto.SignalStrength,
            OdometerReading = dto.OdometerReading
        };

        _context.BusTrackingLogs.Add(log);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new tracking log with ID: {LogId}", log.Id);

        return new BusTrackingLogDto
        {
            Id = log.Id,
            BusId = log.BusId,
            Latitude = log.Latitude,
            Longitude = log.Longitude,
            Speed = log.Speed,
            Direction = log.Direction,
            Timestamp = log.Timestamp,
            IsMoving = log.IsMoving,
            BatteryLevel = log.BatteryLevel,
            SignalStrength = log.SignalStrength,
            OdometerReading = log.OdometerReading
        };
    }

    // Attendance Operations
    public async Task<List<BusAttendanceDto>> GetAttendanceByDateAsync(DateTime date)
    {
        var attendance = await _context.BusAttendances
            .Where(a => a.AttendanceDate.Date == date.Date)
            .Include(a => a.Student)
            .Include(a => a.Stop)
            .ToListAsync();

        return attendance.Select(a => new BusAttendanceDto
        {
            Id = a.Id,
            StudentId = a.StudentId,
            BusScheduleId = a.BusScheduleId,
            AttendanceDate = a.AttendanceDate,
            AttendanceType = a.AttendanceType,
            StopId = a.StopId,
            CheckInTime = a.CheckInTime,
            CheckOutTime = a.CheckOutTime,
            Status = a.Status,
            RecordedBy = a.RecordedBy,
            Notes = a.Notes,
            TenantId = a.TenantId,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            StudentName = a.Student?.FullNameArabic,
            StopName = a.Stop?.StopName
        }).ToList();
    }

    public async Task<List<BusAttendanceDto>> GetAttendanceByStudentIdAsync(Guid studentId)
    {
        var attendance = await _context.BusAttendances
            .Where(a => a.StudentId == studentId)
            .Include(a => a.Student)
            .Include(a => a.Stop)
            .ToListAsync();

        return attendance.Select(a => new BusAttendanceDto
        {
            Id = a.Id,
            StudentId = a.StudentId,
            BusScheduleId = a.BusScheduleId,
            AttendanceDate = a.AttendanceDate,
            AttendanceType = a.AttendanceType,
            StopId = a.StopId,
            CheckInTime = a.CheckInTime,
            CheckOutTime = a.CheckOutTime,
            Status = a.Status,
            RecordedBy = a.RecordedBy,
            Notes = a.Notes,
            TenantId = a.TenantId,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            StudentName = a.Student?.FullNameArabic,
            StopName = a.Stop?.StopName
        }).ToList();
    }

    public async Task<List<BusAttendanceDto>> GetAttendanceByScheduleIdAsync(Guid scheduleId, DateTime date)
    {
        var attendance = await _context.BusAttendances
            .Where(a => a.BusScheduleId == scheduleId && a.AttendanceDate.Date == date.Date)
            .Include(a => a.Student)
            .Include(a => a.Stop)
            .ToListAsync();

        return attendance.Select(a => new BusAttendanceDto
        {
            Id = a.Id,
            StudentId = a.StudentId,
            BusScheduleId = a.BusScheduleId,
            AttendanceDate = a.AttendanceDate,
            AttendanceType = a.AttendanceType,
            StopId = a.StopId,
            CheckInTime = a.CheckInTime,
            CheckOutTime = a.CheckOutTime,
            Status = a.Status,
            RecordedBy = a.RecordedBy,
            Notes = a.Notes,
            TenantId = a.TenantId,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            StudentName = a.Student?.FullNameArabic,
            StopName = a.Stop?.StopName
        }).ToList();
    }

    public async Task<BusAttendanceDto> CreateAttendanceAsync(BusAttendanceDto dto)
    {
        var attendance = new BusAttendance
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            BusScheduleId = dto.BusScheduleId,
            AttendanceDate = dto.AttendanceDate,
            AttendanceType = dto.AttendanceType,
            StopId = dto.StopId,
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime,
            Status = dto.Status,
            RecordedBy = dto.RecordedBy,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.BusAttendances.Add(attendance);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new attendance record with ID: {AttendanceId}", attendance.Id);

        return new BusAttendanceDto
        {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            BusScheduleId = attendance.BusScheduleId,
            AttendanceDate = attendance.AttendanceDate,
            AttendanceType = attendance.AttendanceType,
            StopId = attendance.StopId,
            CheckInTime = attendance.CheckInTime,
            CheckOutTime = attendance.CheckOutTime,
            Status = attendance.Status,
            RecordedBy = attendance.RecordedBy,
            Notes = attendance.Notes,
            CreatedAt = attendance.CreatedAt
        };
    }

    public async Task<BusAttendanceDto?> UpdateAttendanceAsync(Guid id, BusAttendanceDto dto)
    {
        var attendance = await _context.BusAttendances.FindAsync(id);
        if (attendance == null) return null;

        attendance.CheckInTime = dto.CheckInTime;
        attendance.CheckOutTime = dto.CheckOutTime;
        attendance.Status = dto.Status;
        attendance.Notes = dto.Notes;
        attendance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated attendance record with ID: {AttendanceId}", attendance.Id);

        return new BusAttendanceDto
        {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            BusScheduleId = attendance.BusScheduleId,
            AttendanceDate = attendance.AttendanceDate,
            AttendanceType = attendance.AttendanceType,
            StopId = attendance.StopId,
            CheckInTime = attendance.CheckInTime,
            CheckOutTime = attendance.CheckOutTime,
            Status = attendance.Status,
            RecordedBy = attendance.RecordedBy,
            Notes = attendance.Notes,
            TenantId = attendance.TenantId,
            CreatedAt = attendance.CreatedAt,
            UpdatedAt = attendance.UpdatedAt
        };
    }

    // Incident Operations
    public async Task<List<BusIncidentDto>> GetAllIncidentsAsync()
    {
        var incidents = await _context.BusIncidents
            .Include(i => i.Bus)
            .ToListAsync();

        return incidents.Select(i => new BusIncidentDto
        {
            Id = i.Id,
            BusId = i.BusId,
            IncidentDate = i.IncidentDate,
            IncidentType = i.IncidentType,
            Description = i.Description,
            Location = i.Location,
            Severity = i.Severity,
            ReportedBy = i.ReportedBy,
            ResolvedAt = i.ResolvedAt,
            ResolutionNotes = i.ResolutionNotes,
            TenantId = i.TenantId,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt,
            BusNumber = i.Bus?.BusNumber
        }).ToList();
    }

    public async Task<BusIncidentDto?> GetIncidentByIdAsync(Guid id)
    {
        var incident = await _context.BusIncidents
            .Include(i => i.Bus)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (incident == null) return null;

        return new BusIncidentDto
        {
            Id = incident.Id,
            BusId = incident.BusId,
            IncidentDate = incident.IncidentDate,
            IncidentType = incident.IncidentType,
            Description = incident.Description,
            Location = incident.Location,
            Severity = incident.Severity,
            ReportedBy = incident.ReportedBy,
            ResolvedAt = incident.ResolvedAt,
            ResolutionNotes = incident.ResolutionNotes,
            TenantId = incident.TenantId,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt,
            BusNumber = incident.Bus?.BusNumber
        };
    }

    public async Task<BusIncidentDto> CreateIncidentAsync(BusIncidentDto dto)
    {
        var incident = new BusIncident
        {
            Id = Guid.NewGuid(),
            BusId = dto.BusId,
            IncidentDate = dto.IncidentDate,
            IncidentType = dto.IncidentType,
            Description = dto.Description,
            Location = dto.Location,
            Severity = dto.Severity,
            ReportedBy = dto.ReportedBy,
            ResolvedAt = dto.ResolvedAt,
            ResolutionNotes = dto.ResolutionNotes,
            CreatedAt = DateTime.UtcNow
        };

        _context.BusIncidents.Add(incident);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new incident with ID: {IncidentId}", incident.Id);

        return new BusIncidentDto
        {
            Id = incident.Id,
            BusId = incident.BusId,
            IncidentDate = incident.IncidentDate,
            IncidentType = incident.IncidentType,
            Description = incident.Description,
            Location = incident.Location,
            Severity = incident.Severity,
            ReportedBy = incident.ReportedBy,
            ResolvedAt = incident.ResolvedAt,
            ResolutionNotes = incident.ResolutionNotes,
            CreatedAt = incident.CreatedAt
        };
    }

    public async Task<BusIncidentDto?> UpdateIncidentAsync(Guid id, BusIncidentDto dto)
    {
        var incident = await _context.BusIncidents.FindAsync(id);
        if (incident == null) return null;

        incident.IncidentDate = dto.IncidentDate;
        incident.IncidentType = dto.IncidentType;
        incident.Description = dto.Description;
        incident.Location = dto.Location;
        incident.Severity = dto.Severity;
        incident.ReportedBy = dto.ReportedBy;
        incident.ResolvedAt = dto.ResolvedAt;
        incident.ResolutionNotes = dto.ResolutionNotes;
        incident.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated incident with ID: {IncidentId}", incident.Id);

        return new BusIncidentDto
        {
            Id = incident.Id,
            BusId = incident.BusId,
            IncidentDate = incident.IncidentDate,
            IncidentType = incident.IncidentType,
            Description = incident.Description,
            Location = incident.Location,
            Severity = incident.Severity,
            ReportedBy = incident.ReportedBy,
            ResolvedAt = incident.ResolvedAt,
            ResolutionNotes = incident.ResolutionNotes,
            TenantId = incident.TenantId,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt
        };
    }

    public async Task<bool> DeleteIncidentAsync(Guid id)
    {
        var incident = await _context.BusIncidents.FindAsync(id);
        if (incident == null) return false;

        incident.IsDeleted = true;
        incident.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted incident with ID: {IncidentId}", incident.Id);

        return true;
    }

    public async Task<List<BusIncidentDto>> GetIncidentsByBusIdAsync(Guid busId)
    {
        var incidents = await _context.BusIncidents
            .Where(i => i.BusId == busId)
            .Include(i => i.Bus)
            .ToListAsync();

        return incidents.Select(i => new BusIncidentDto
        {
            Id = i.Id,
            BusId = i.BusId,
            IncidentDate = i.IncidentDate,
            IncidentType = i.IncidentType,
            Description = i.Description,
            Location = i.Location,
            Severity = i.Severity,
            ReportedBy = i.ReportedBy,
            ResolvedAt = i.ResolvedAt,
            ResolutionNotes = i.ResolutionNotes,
            CreatedAt = i.CreatedAt,
            BusNumber = i.Bus?.BusNumber
        }).ToList();
    }

    public async Task<List<BusIncidentDto>> GetActiveIncidentsAsync()
    {
        var incidents = await _context.BusIncidents
            .Where(i => i.ResolvedAt == null)
            .Include(i => i.Bus)
            .ToListAsync();

        return incidents.Select(i => new BusIncidentDto
        {
            Id = i.Id,
            BusId = i.BusId,
            IncidentDate = i.IncidentDate,
            IncidentType = i.IncidentType,
            Description = i.Description,
            Location = i.Location,
            Severity = i.Severity,
            ReportedBy = i.ReportedBy,
            ResolvedAt = i.ResolvedAt,
            ResolutionNotes = i.ResolutionNotes,
            CreatedAt = i.CreatedAt,
            BusNumber = i.Bus?.BusNumber
        }).ToList();
    }

    // Fuel Operations
    public async Task<List<FuelRecordDto>> GetFuelRecordsByBusIdAsync(Guid busId)
    {
        var records = await _context.FuelRecords
            .Where(f => f.BusId == busId)
            .Include(f => f.Bus)
            .ToListAsync();

        return records.Select(f => new FuelRecordDto
        {
            Id = f.Id,
            BusId = f.BusId,
            RefuelDate = f.RefuelDate,
            FuelAmount = f.FuelAmount,
            Cost = f.Cost,
            OdometerReading = f.OdometerReading,
            FuelStation = f.FuelStation,
            RecordedBy = f.RecordedBy,
            TenantId = f.TenantId,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt,
            BusNumber = f.Bus?.BusNumber
        }).ToList();
    }

    public async Task<FuelRecordDto> CreateFuelRecordAsync(FuelRecordDto dto)
    {
        var record = new FuelRecord
        {
            Id = Guid.NewGuid(),
            BusId = dto.BusId,
            RefuelDate = dto.RefuelDate,
            FuelAmount = dto.FuelAmount,
            Cost = dto.Cost,
            OdometerReading = dto.OdometerReading,
            FuelStation = dto.FuelStation,
            RecordedBy = dto.RecordedBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.FuelRecords.Add(record);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new fuel record with ID: {RecordId}", record.Id);

        return new FuelRecordDto
        {
            Id = record.Id,
            BusId = record.BusId,
            RefuelDate = record.RefuelDate,
            FuelAmount = record.FuelAmount,
            Cost = record.Cost,
            OdometerReading = record.OdometerReading,
            FuelStation = record.FuelStation,
            RecordedBy = record.RecordedBy,
            CreatedAt = record.CreatedAt
        };
    }

    public async Task<FuelRecordDto?> UpdateFuelRecordAsync(Guid id, FuelRecordDto dto)
    {
        var record = await _context.FuelRecords.FindAsync(id);
        if (record == null) return null;

        record.RefuelDate = dto.RefuelDate;
        record.FuelAmount = dto.FuelAmount;
        record.Cost = dto.Cost;
        record.OdometerReading = dto.OdometerReading;
        record.FuelStation = dto.FuelStation;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated fuel record with ID: {RecordId}", record.Id);

        return new FuelRecordDto
        {
            Id = record.Id,
            BusId = record.BusId,
            RefuelDate = record.RefuelDate,
            FuelAmount = record.FuelAmount,
            Cost = record.Cost,
            OdometerReading = record.OdometerReading,
            FuelStation = record.FuelStation,
            RecordedBy = record.RecordedBy,
            TenantId = record.TenantId,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }

    public async Task<bool> DeleteFuelRecordAsync(Guid id)
    {
        var record = await _context.FuelRecords.FindAsync(id);
        if (record == null) return false;

        record.IsDeleted = true;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted fuel record with ID: {RecordId}", record.Id);

        return true;
    }

    // Maintenance Operations
    public async Task<List<MaintenanceRecordDto>> GetMaintenanceRecordsByBusIdAsync(Guid busId)
    {
        var records = await _context.MaintenanceRecords
            .Where(m => m.BusId == busId)
            .Include(m => m.Bus)
            .ToListAsync();

        return records.Select(m => new MaintenanceRecordDto
        {
            Id = m.Id,
            BusId = m.BusId,
            MaintenanceDate = m.MaintenanceDate,
            MaintenanceType = m.MaintenanceType,
            Description = m.Description,
            Cost = m.Cost,
            Workshop = m.Workshop,
            NextMaintenanceDate = m.NextMaintenanceDate,
            RecordedBy = m.RecordedBy,
            TenantId = m.TenantId,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt,
            BusNumber = m.Bus?.BusNumber
        }).ToList();
    }

    public async Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(MaintenanceRecordDto dto)
    {
        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid(),
            BusId = dto.BusId,
            MaintenanceDate = dto.MaintenanceDate,
            MaintenanceType = dto.MaintenanceType,
            Description = dto.Description,
            Cost = dto.Cost,
            Workshop = dto.Workshop,
            NextMaintenanceDate = dto.NextMaintenanceDate,
            RecordedBy = dto.RecordedBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.MaintenanceRecords.Add(record);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new maintenance record with ID: {RecordId}", record.Id);

        return new MaintenanceRecordDto
        {
            Id = record.Id,
            BusId = record.BusId,
            MaintenanceDate = record.MaintenanceDate,
            MaintenanceType = record.MaintenanceType,
            Description = record.Description,
            Cost = record.Cost,
            Workshop = record.Workshop,
            NextMaintenanceDate = record.NextMaintenanceDate,
            RecordedBy = record.RecordedBy,
            CreatedAt = record.CreatedAt
        };
    }

    public async Task<MaintenanceRecordDto?> UpdateMaintenanceRecordAsync(Guid id, MaintenanceRecordDto dto)
    {
        var record = await _context.MaintenanceRecords.FindAsync(id);
        if (record == null) return null;

        record.MaintenanceDate = dto.MaintenanceDate;
        record.MaintenanceType = dto.MaintenanceType;
        record.Description = dto.Description;
        record.Cost = dto.Cost;
        record.Workshop = dto.Workshop;
        record.NextMaintenanceDate = dto.NextMaintenanceDate;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated maintenance record with ID: {RecordId}", record.Id);

        return new MaintenanceRecordDto
        {
            Id = record.Id,
            BusId = record.BusId,
            MaintenanceDate = record.MaintenanceDate,
            MaintenanceType = record.MaintenanceType,
            Description = record.Description,
            Cost = record.Cost,
            Workshop = record.Workshop,
            NextMaintenanceDate = record.NextMaintenanceDate,
            RecordedBy = record.RecordedBy,
            TenantId = record.TenantId,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }

    public async Task<bool> DeleteMaintenanceRecordAsync(Guid id)
    {
        var record = await _context.MaintenanceRecords.FindAsync(id);
        if (record == null) return false;

        record.IsDeleted = true;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted maintenance record with ID: {RecordId}", record.Id);

        return true;
    }

    // Statistics
    public async Task<TransportStatisticsDto> GetStatisticsAsync()
    {
        var totalBuses = await _context.Buses.CountAsync();
        var activeBuses = await _context.Buses.CountAsync(b => b.IsActive && b.Status == "Active");
        var busesInMaintenance = await _context.Buses.CountAsync(b => b.Status == "Maintenance");
        var totalRoutes = await _context.BusRoutes.CountAsync();
        var activeRoutes = await _context.BusRoutes.CountAsync(r => r.IsActive);
        var totalDrivers = await _context.Drivers.CountAsync();
        var activeDrivers = await _context.Drivers.CountAsync(d => d.IsActive);
        var totalSupervisors = await _context.BusSupervisors.CountAsync();
        var activeSupervisors = await _context.BusSupervisors.CountAsync(s => s.IsActive);
        var totalSubscriptions = await _context.StudentTransportSubscriptions.CountAsync();
        var activeSubscriptions = await _context.StudentTransportSubscriptions.CountAsync(s => s.IsActive);
        var totalIncidents = await _context.BusIncidents.CountAsync();
        var activeIncidents = await _context.BusIncidents.CountAsync(i => i.ResolvedAt == null);
        var totalFuelCost = await _context.FuelRecords.SumAsync(f => f.Cost);
        var totalMaintenanceCost = await _context.MaintenanceRecords.SumAsync(m => m.Cost);

        return new TransportStatisticsDto
        {
            TotalBuses = totalBuses,
            ActiveBuses = activeBuses,
            BusesInMaintenance = busesInMaintenance,
            TotalRoutes = totalRoutes,
            ActiveRoutes = activeRoutes,
            TotalDrivers = totalDrivers,
            ActiveDrivers = activeDrivers,
            TotalSupervisors = totalSupervisors,
            ActiveSupervisors = activeSupervisors,
            TotalSubscriptions = totalSubscriptions,
            ActiveSubscriptions = activeSubscriptions,
            TotalIncidents = totalIncidents,
            ActiveIncidents = activeIncidents,
            TotalFuelCost = totalFuelCost,
            TotalMaintenanceCost = totalMaintenanceCost
        };
    }
}
