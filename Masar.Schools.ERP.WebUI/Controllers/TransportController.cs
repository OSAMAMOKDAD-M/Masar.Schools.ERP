using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

public class TransportController : Controller
{
    private readonly ITransportService _transportService;
    private readonly ILogger<TransportController> _logger;
    private readonly MasarDbContext _context;

    public TransportController(ITransportService transportService, ILogger<TransportController> logger, MasarDbContext context)
    {
        _transportService = transportService;
        _logger = logger;
        _context = context;
    }

    // Dashboard
    public async Task<IActionResult> Index()
    {
        try
        {
            var statistics = await _transportService.GetStatisticsAsync();
            return View(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading transport dashboard");
            return View("Error");
        }
    }

    // Live Tracking
    public async Task<IActionResult> LiveTracking()
    {
        try
        {
            var buses = await _transportService.GetActiveBusesAsync();
            return View(buses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading live tracking");
            return View("Error");
        }
    }

    // Buses
    public async Task<IActionResult> Buses()
    {
        try
        {
            var buses = await _transportService.GetAllBusesAsync();
            return View(buses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading buses");
            return View("Error");
        }
    }

    public IActionResult CreateBus()
    {
        return View(new BusCreateDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateBus(BusCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.CreateBusAsync(dto);
            return RedirectToAction(nameof(Buses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bus");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء الحافلة");
            return View(dto);
        }
    }

    public async Task<IActionResult> EditBus(Guid id)
    {
        try
        {
            var bus = await _transportService.GetBusByIdAsync(id);
            if (bus == null) return NotFound();

            ViewBag.BusId = id;

            var dto = new BusCreateDto
            {
                BusNumber = bus.BusNumber,
                PlateNumber = bus.PlateNumber,
                Capacity = bus.Capacity,
                Type = bus.Type,
                Status = bus.Status,
                VehicleIdentificationNumber = bus.VehicleIdentificationNumber,
                PurchaseDate = bus.PurchaseDate,
                InsuranceExpiryDate = bus.InsuranceExpiryDate,
                LicenseExpiryDate = bus.LicenseExpiryDate,
                ImagePath = bus.ImagePath
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bus for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditBus(Guid id, BusCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateBusAsync(id, dto);
            return RedirectToAction(nameof(Buses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bus");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث الحافلة");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteBus(Guid id)
    {
        try
        {
            await _transportService.DeleteBusAsync(id);
            return RedirectToAction(nameof(Buses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bus");
            return RedirectToAction(nameof(Buses));
        }
    }

    // Routes
    public async Task<IActionResult> Routes()
    {
        try
        {
            var routes = await _transportService.GetAllRoutesAsync();
            return View(routes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading routes");
            return View("Error");
        }
    }

    public async Task<IActionResult> CreateRoute()
    {
        try
        {
            // No ViewBag data needed for CreateRoute currently
            return View(new BusRouteCreateDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create route form");
            return View(new BusRouteCreateDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoute(BusRouteCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.CreateRouteAsync(dto);
            return RedirectToAction(nameof(Routes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating route");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء خط السير");
            return View(dto);
        }
    }

    public async Task<IActionResult> EditRoute(Guid id)
    {
        try
        {
            var route = await _transportService.GetRouteByIdAsync(id);
            if (route == null) return NotFound();

            ViewBag.RouteId = id;

            var dto = new BusRouteCreateDto
            {
                RouteName = route.RouteName,
                RouteCode = route.RouteCode,
                Description = route.Description,
                StartLocation = route.StartLocation,
                EndLocation = route.EndLocation,
                TotalDistance = route.TotalDistance,
                EstimatedDuration = route.EstimatedDuration,
                Direction = route.Direction
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading route for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditRoute(Guid id, BusRouteCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateRouteAsync(id, dto);
            return RedirectToAction(nameof(Routes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating route");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث خط السير");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteRoute(Guid id)
    {
        try
        {
            await _transportService.DeleteRouteAsync(id);
            return RedirectToAction(nameof(Routes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting route");
            return RedirectToAction(nameof(Routes));
        }
    }

    // Drivers
    public async Task<IActionResult> Drivers()
    {
        try
        {
            var drivers = await _transportService.GetAllDriversAsync();
            return View(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading drivers");
            return View("Error");
        }
    }

    public async Task<IActionResult> CreateDriver()
    {
        try
        {
            // Populate Employees dropdown from DbContext
            var employees = await _context.Employees
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .Select(e => new { e.Id, Name = e.FullNameArabic })
                .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name");

            // Populate Buses dropdown
            var buses = await _transportService.GetAllBusesAsync();
            ViewBag.Buses = new SelectList(buses, "Id", "BusNumber");

            return View(new DriverCreateDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create driver form");
            // Return the view with empty dropdowns instead of Error page
            ViewBag.Employees = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Buses = new SelectList(Enumerable.Empty<SelectListItem>());
            return View(new DriverCreateDto());
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver(DriverCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns on validation error
                var employees = await _context.Employees
                    .Where(e => !e.IsDeleted)
                    .OrderBy(e => e.FullNameArabic)
                    .Select(e => new { e.Id, Name = e.FullNameArabic })
                    .ToListAsync();
                ViewBag.Employees = new SelectList(employees, "Id", "Name");

                var buses = await _transportService.GetAllBusesAsync();
                ViewBag.Buses = new SelectList(buses, "Id", "BusNumber");

                return View(dto);
            }

            await _transportService.CreateDriverAsync(dto);
            return RedirectToAction(nameof(Drivers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating driver");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء السائق");

            // Re-populate dropdowns on error
            var employees = await _context.Employees
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.FullNameArabic)
                .Select(e => new { e.Id, Name = e.FullNameArabic })
                .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "Name");

            var buses = await _transportService.GetAllBusesAsync();
            ViewBag.Buses = new SelectList(buses, "Id", "BusNumber");

            return View(dto);
        }
    }

    public async Task<IActionResult> EditDriver(Guid id)
    {
        try
        {
            var driver = await _transportService.GetDriverByIdAsync(id);
            if (driver == null) return NotFound();

            ViewBag.DriverId = id;

            var dto = new DriverCreateDto
            {
                EmployeeId = driver.EmployeeId,
                LicenseNumber = driver.LicenseNumber,
                LicenseExpiryDate = driver.LicenseExpiryDate,
                LicenseType = driver.LicenseType,
                YearsOfExperience = driver.YearsOfExperience,
                IsActive = driver.IsActive,
                AssignedBusId = driver.AssignedBusId
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading driver for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditDriver(Guid id, DriverCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateDriverAsync(id, dto);
            return RedirectToAction(nameof(Drivers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating driver");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث السائق");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        try
        {
            await _transportService.DeleteDriverAsync(id);
            return RedirectToAction(nameof(Drivers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting driver");
            return RedirectToAction(nameof(Drivers));
        }
    }

    // Supervisors
    public async Task<IActionResult> Supervisors()
    {
        try
        {
            var supervisors = await _transportService.GetAllSupervisorsAsync();
            return View(supervisors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading supervisors");
            return View("Error");
        }
    }

    public async Task<IActionResult> CreateSupervisor()
    {
        try
        {
            // Populate Buses dropdown
            var buses = await _transportService.GetAllBusesAsync();
            ViewBag.Buses = new SelectList(buses, "Id", "BusNumber");

            return View(new BusSupervisorCreateDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create supervisor form");
            ViewBag.Buses = new SelectList(Enumerable.Empty<SelectListItem>());
            return View(new BusSupervisorCreateDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupervisor(BusSupervisorCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.CreateSupervisorAsync(dto);
            return RedirectToAction(nameof(Supervisors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supervisor");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء المشرف");
            return View(dto);
        }
    }

    public async Task<IActionResult> EditSupervisor(Guid id)
    {
        try
        {
            var supervisor = await _transportService.GetSupervisorByIdAsync(id);
            if (supervisor == null) return NotFound();

            ViewBag.SupervisorId = id;

            var dto = new BusSupervisorCreateDto
            {
                EmployeeId = supervisor.EmployeeId,
                Phone = supervisor.Phone,
                EmergencyContact = supervisor.EmergencyContact,
                IsActive = supervisor.IsActive,
                AssignedBusId = supervisor.AssignedBusId
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading supervisor for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditSupervisor(Guid id, BusSupervisorCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateSupervisorAsync(id, dto);
            return RedirectToAction(nameof(Supervisors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating supervisor");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث المشرف");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteSupervisor(Guid id)
    {
        try
        {
            await _transportService.DeleteSupervisorAsync(id);
            return RedirectToAction(nameof(Supervisors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting supervisor");
            return RedirectToAction(nameof(Supervisors));
        }
    }

    // Schedules
    public async Task<IActionResult> Schedules()
    {
        try
        {
            var schedules = await _transportService.GetAllSchedulesAsync();
            return View(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading schedules");
            return View("Error");
        }
    }

    public async Task<IActionResult> CreateSchedule()
    {
        try
        {
            // Populate Buses dropdown
            var buses = await _transportService.GetAllBusesAsync();
            ViewBag.Buses = new SelectList(buses, "Id", "BusNumber");

            // Populate Routes dropdown
            var routes = await _transportService.GetAllRoutesAsync();
            ViewBag.Routes = new SelectList(routes, "Id", "RouteName");

            // Populate Drivers dropdown
            var drivers = await _transportService.GetAllDriversAsync();
            ViewBag.Drivers = new SelectList(drivers, "Id", "LicenseNumber");

            // Populate Supervisors dropdown
            var supervisors = await _transportService.GetAllSupervisorsAsync();
            ViewBag.Supervisors = new SelectList(supervisors, "Id", "Phone");

            // Populate Schools dropdown
            var schools = await _context.Schools
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.NameArabic)
                .Select(s => new { s.Id, Name = s.NameArabic })
                .ToListAsync();
            ViewBag.Schools = new SelectList(schools, "Id", "Name");

            return View(new BusScheduleCreateDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create schedule form");
            ViewBag.Buses = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Routes = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Drivers = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Supervisors = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Schools = new SelectList(Enumerable.Empty<SelectListItem>());
            return View(new BusScheduleCreateDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchedule(BusScheduleCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.CreateScheduleAsync(dto);
            return RedirectToAction(nameof(Schedules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء جدول الحافلة");
            return View(dto);
        }
    }

    public async Task<IActionResult> EditSchedule(Guid id)
    {
        try
        {
            var schedule = await _transportService.GetScheduleByIdAsync(id);
            if (schedule == null) return NotFound();

            ViewBag.ScheduleId = id;

            var dto = new BusScheduleCreateDto
            {
                BusId = schedule.BusId,
                RouteId = schedule.RouteId,
                DriverId = schedule.DriverId,
                SupervisorId = schedule.SupervisorId,
                TripType = schedule.TripType,
                DepartureTime = schedule.DepartureTime,
                ArrivalTime = schedule.ArrivalTime,
                SchoolId = schedule.SchoolId,
                IsActive = schedule.IsActive
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading schedule for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditSchedule(Guid id, BusScheduleCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateScheduleAsync(id, dto);
            return RedirectToAction(nameof(Schedules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating schedule");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث جدول الحافلة");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        try
        {
            await _transportService.DeleteScheduleAsync(id);
            return RedirectToAction(nameof(Schedules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting schedule");
            return RedirectToAction(nameof(Schedules));
        }
    }

    // Subscriptions
    public async Task<IActionResult> Subscriptions()
    {
        try
        {
            var subscriptions = await _transportService.GetAllSubscriptionsAsync();
            return View(subscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading subscriptions");
            return View("Error");
        }
    }

    public async Task<IActionResult> CreateSubscription()
    {
        try
        {
            // Populate Students dropdown
            var students = await _context.Students
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.FullNameArabic)
                .Select(s => new { s.Id, Name = s.FullNameArabic })
                .ToListAsync();
            ViewBag.Students = new SelectList(students, "Id", "Name");

            // Populate Schedules dropdown
            var schedules = await _transportService.GetAllSchedulesAsync();
            ViewBag.Schedules = new SelectList(schedules, "Id", "TripType");

            return View(new StudentTransportSubscriptionCreateDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create subscription form");
            ViewBag.Students = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Schedules = new SelectList(Enumerable.Empty<SelectListItem>());
            return View(new StudentTransportSubscriptionCreateDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(StudentTransportSubscriptionCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.CreateSubscriptionAsync(dto);
            return RedirectToAction(nameof(Subscriptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription");
            ModelState.AddModelError("", "حدث خطأ أثناء إنشاء اشتراك النقل");
            return View(dto);
        }
    }

    public async Task<IActionResult> EditSubscription(Guid id)
    {
        try
        {
            var subscription = await _transportService.GetSubscriptionByIdAsync(id);
            if (subscription == null) return NotFound();

            ViewBag.SubscriptionId = id;

            var dto = new StudentTransportSubscriptionCreateDto
            {
                StudentId = subscription.StudentId,
                BusScheduleId = subscription.BusScheduleId,
                RouteStopId = subscription.RouteStopId,
                SubscriptionStartDate = subscription.SubscriptionStartDate,
                SubscriptionEndDate = subscription.SubscriptionEndDate,
                MonthlyFee = subscription.MonthlyFee,
                PaymentStatus = subscription.PaymentStatus,
                IsActive = subscription.IsActive
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading subscription for edit");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditSubscription(Guid id, StudentTransportSubscriptionCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _transportService.UpdateSubscriptionAsync(id, dto);
            return RedirectToAction(nameof(Subscriptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث اشتراك النقل");
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        try
        {
            await _transportService.DeleteSubscriptionAsync(id);
            return RedirectToAction(nameof(Subscriptions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription");
            return RedirectToAction(nameof(Subscriptions));
        }
    }
}
