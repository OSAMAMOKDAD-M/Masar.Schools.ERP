using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class TransportController : ControllerBase
{
    private readonly MasarDbContext _context;
    private readonly ILogger<TransportController> _logger;

    public TransportController(MasarDbContext context, ILogger<TransportController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Bus Operations - Reimplemented
    [HttpGet("buses")]
    public async Task<ActionResult> GetAllBuses()
    {
        try
        {
            var buses = await _context.Buses
                .Include(b => b.Drivers)
                .ThenInclude(d => d.Employee)
                .Where(b => !b.IsDeleted && b.IsActive)
                .ToListAsync();

            var result = buses.Select(b => new
            {
                b.Id,
                b.BusNumber,
                b.PlateNumber,
                b.Capacity,
                b.Type,
                b.Status,
                b.NextMaintenanceDate,
                DriverName = b.Drivers.FirstOrDefault()?.Employee?.FullNameArabic,
                DriverPhone = b.Drivers.FirstOrDefault()?.Employee?.PhoneNumber
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all buses");
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب الحافلات" });
        }
    }

    [HttpGet("buses/{id}")]
    public async Task<ActionResult> GetBusById(Guid id)
    {
        try
        {
            var bus = await _context.Buses
                .Include(b => b.Drivers)
                .ThenInclude(d => d.Employee)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (bus == null) return NotFound();

            return Ok(bus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bus with ID: {BusId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب الحافلة" });
        }
    }

    [HttpPost("buses")]
    public async Task<ActionResult> CreateBus([FromBody] BusCreateDto dto)
    {
        try
        {
            var bus = new Bus
            {
                Id = Guid.NewGuid(),
                BusNumber = dto.BusNumber,
                PlateNumber = dto.PlateNumber,
                Capacity = dto.Capacity,
                Type = dto.Type,
                Status = dto.Status ?? "Active",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBusById), new { id = bus.Id }, bus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bus");
            return StatusCode(500, new { error = "حدث خطأ أثناء إنشاء الحافلة" });
        }
    }

    [HttpPut("buses/{id}")]
    public async Task<ActionResult> UpdateBus(Guid id, [FromBody] BusCreateDto dto)
    {
        try
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return NotFound();

            bus.BusNumber = dto.BusNumber;
            bus.PlateNumber = dto.PlateNumber;
            bus.Capacity = dto.Capacity;
            bus.Type = dto.Type;
            bus.Status = dto.Status ?? bus.Status;
            bus.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(bus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bus with ID: {BusId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء تحديث الحافلة" });
        }
    }

    [HttpDelete("buses/{id}")]
    public async Task<ActionResult> DeleteBus(Guid id)
    {
        try
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return NotFound();

            bus.IsDeleted = true;
            bus.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bus with ID: {BusId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء حذف الحافلة" });
        }
    }

    // Route Operations - Reimplemented
    [HttpGet("routes")]
    public async Task<ActionResult> GetAllRoutes()
    {
        try
        {
            var routes = await _context.BusRoutes
                .Include(r => r.Stops)
                .Where(r => !r.IsDeleted && r.IsActive)
                .ToListAsync();

            var result = routes.Select(r => new
            {
                r.Id,
                r.RouteName,
                r.StartLocation,
                r.EndLocation,
                StopCount = r.Stops.Count
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all routes");
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب خطوط السير" });
        }
    }

    [HttpGet("routes/{id}")]
    public async Task<ActionResult> GetRouteById(Guid id)
    {
        try
        {
            var route = await _context.BusRoutes
                .Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (route == null) return NotFound();

            return Ok(route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting route with ID: {RouteId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب خط السير" });
        }
    }

    [HttpPost("routes")]
    public async Task<ActionResult> CreateRoute([FromBody] BusRouteCreateDto dto)
    {
        try
        {
            var route = new BusRoute
            {
                Id = Guid.NewGuid(),
                RouteName = dto.RouteName,
                StartLocation = dto.StartLocation,
                EndLocation = dto.EndLocation,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.BusRoutes.Add(route);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating route");
            return StatusCode(500, new { error = "حدث خطأ أثناء إنشاء خط السير" });
        }
    }

    [HttpPut("routes/{id}")]
    public async Task<ActionResult> UpdateRoute(Guid id, [FromBody] BusRouteCreateDto dto)
    {
        try
        {
            var route = await _context.BusRoutes.FindAsync(id);
            if (route == null) return NotFound();

            route.RouteName = dto.RouteName;
            route.StartLocation = dto.StartLocation;
            route.EndLocation = dto.EndLocation;
            route.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating route with ID: {RouteId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء تحديث خط السير" });
        }
    }

    [HttpDelete("routes/{id}")]
    public async Task<ActionResult> DeleteRoute(Guid id)
    {
        try
        {
            var route = await _context.BusRoutes.FindAsync(id);
            if (route == null) return NotFound();

            route.IsDeleted = true;
            route.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting route with ID: {RouteId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء حذف خط السير" });
        }
    }

    // Stop Operations
    [HttpGet("routes/{routeId}/stops")]
    public async Task<ActionResult> GetStopsByRouteId(Guid routeId)
    {
        try
        {
            var stops = await _context.RouteStops
                .Where(s => s.RouteId == routeId && !s.IsDeleted)
                .Select(s => new
                {
                    s.Id,
                    s.StopName,
                    s.Latitude,
                    s.Longitude,
                    s.StopOrder
                })
                .OrderBy(s => s.StopOrder)
                .ToListAsync();

            return Ok(stops);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting stops for route ID: {RouteId}", routeId);
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب المحطات" });
        }
    }

    // Schedule Operations
    [HttpGet("schedules")]
    public async Task<ActionResult> GetAllSchedules()
    {
        try
        {
            var schedules = await _context.BusSchedules
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .Where(s => !s.IsDeleted && s.IsActive)
                .Select(s => new
                {
                    s.Id,
                    s.BusId,
                    s.Bus.BusNumber,
                    s.RouteId,
                    s.Route.RouteName,
                    s.DepartureTime,
                    s.ArrivalTime
                })
                .ToListAsync();

            return Ok(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all schedules");
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب جداول الحافلات" });
        }
    }

    [HttpPost("schedules")]
    public async Task<ActionResult> CreateSchedule([FromBody] BusScheduleCreateDto dto)
    {
        try
        {
            var schedule = new BusSchedule
            {
                Id = Guid.NewGuid(),
                BusId = dto.BusId,
                RouteId = dto.RouteId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.BusSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetScheduleById), new { id = schedule.Id }, schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule");
            return StatusCode(500, new { error = "حدث خطأ أثناء إنشاء جدول الحافلة" });
        }
    }

    [HttpGet("schedules/{id}")]
    public async Task<ActionResult> GetScheduleById(Guid id)
    {
        try
        {
            var schedule = await _context.BusSchedules
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (schedule == null) return NotFound();

            return Ok(schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule with ID: {ScheduleId}", id);
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب جدول الحافلة" });
        }
    }

    // Subscription Operations
    [HttpGet("subscriptions")]
    public async Task<ActionResult> GetAllSubscriptions()
    {
        try
        {
            var subscriptions = await _context.StudentTransportSubscriptions
                .Include(s => s.Student)
                .Include(s => s.BusSchedule)
                    .ThenInclude(bs => bs.Bus)
                .Where(s => !s.IsDeleted)
                .Select(s => new
                {
                    s.Id,
                    s.StudentId,
                    s.Student.FullNameArabic,
                    s.BusScheduleId,
                    s.BusSchedule.Bus.BusNumber,
                    s.MonthlyFee,
                    s.SubscriptionStartDate,
                    s.SubscriptionEndDate,
                    s.PaymentStatus,
                    s.IsActive
                })
                .ToListAsync();

            return Ok(subscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all subscriptions");
            return StatusCode(500, new { error = "حدث خطأ أثناء جلب اشتراكات النقل" });
        }
    }
}
