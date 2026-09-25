using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities.Canteen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class CanteenController : Controller
{
    private readonly ICanteenService _canteenService;

    public CanteenController(ICanteenService canteenService)
    {
        _canteenService = canteenService;
    }

    // ==================== Shift Management ====================
    public async Task<IActionResult> Shifts()
    {
        var search = new ShiftSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetShiftsAsync(search);
        return View(result.Data);
    }

    public async Task<IActionResult> OpenShift()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> OpenShift(CreateCashierShiftDto dto)
    {
        // TODO: Get BranchId and EmployeeId from current user
        dto.BranchId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // Temporary
        dto.EmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // Temporary

        var result = await _canteenService.OpenShiftAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Shifts");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> CloseShift(Guid id)
    {
        var result = await _canteenService.GetShiftReportAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CloseShift(CloseCashierShiftDto dto)
    {
        var result = await _canteenService.CloseShiftAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Shifts");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> ShiftReport(Guid id)
    {
        var result = await _canteenService.GetShiftReportAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    // ==================== Products Management ====================
    public async Task<IActionResult> Products()
    {
        var search = new CanteenSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetCanteenProductsAsync(search);
        return View(result.Data);
    }

    public async Task<IActionResult> CreateProduct()
    {
        var categories = await _canteenService.GetProductCategoriesAsync(new CanteenSearchDto { Page = 1, PageSize = 100 });
        ViewBag.Categories = categories.Data;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateCanteenProductDto dto)
    {
        var result = await _canteenService.CreateCanteenProductAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Products");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> EditProduct(Guid id)
    {
        var result = await _canteenService.GetCanteenProductAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(UpdateCanteenProductDto dto)
    {
        var result = await _canteenService.UpdateCanteenProductAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Products");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _canteenService.DeleteCanteenProductAsync(id);
        if (result.Success)
        {
            return RedirectToAction("Products");
        }
        return BadRequest(result.Message);
    }

    // ==================== Categories ====================
    public async Task<IActionResult> Categories()
    {
        var search = new CanteenSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetProductCategoriesAsync(search);
        return View(result.Data);
    }

    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateProductCategoryDto dto)
    {
        var result = await _canteenService.CreateProductCategoryAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Categories");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> EditCategory(Guid id)
    {
        var result = await _canteenService.GetProductCategoryAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(UpdateProductCategoryDto dto)
    {
        var result = await _canteenService.UpdateProductCategoryAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Categories");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    // ==================== POS ====================
    public async Task<IActionResult> POS()
    {
        // Check if shift is open
        // TODO: Get BranchId and EmployeeId from current user
        var branchId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var employeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var currentShift = await _canteenService.GetCurrentShiftAsync(branchId, employeeId);
        if (!currentShift.Success)
        {
            return RedirectToAction("OpenShift");
        }

        ViewBag.CurrentShift = currentShift.Data;
        return View();
    }

    public async Task<IActionResult> DiningIn()
    {
        // Check if shift is open
        var branchId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var employeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var currentShift = await _canteenService.GetCurrentShiftAsync(branchId, employeeId);
        if (!currentShift.Success)
        {
            return RedirectToAction("OpenShift");
        }

        ViewBag.CurrentShift = currentShift.Data;
        return View();
    }

    public async Task<IActionResult> Takeaway()
    {
        // Check if shift is open
        var branchId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var employeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var currentShift = await _canteenService.GetCurrentShiftAsync(branchId, employeeId);
        if (!currentShift.Success)
        {
            return RedirectToAction("OpenShift");
        }

        ViewBag.CurrentShift = currentShift.Data;
        return View();
    }

    public async Task<IActionResult> Delivery()
    {
        // Check if shift is open
        var branchId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var employeeId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var currentShift = await _canteenService.GetCurrentShiftAsync(branchId, employeeId);
        if (!currentShift.Success)
        {
            return RedirectToAction("OpenShift");
        }

        ViewBag.CurrentShift = currentShift.Data;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateCanteenOrderDto dto)
    {
        // TODO: Get BranchId from current user
        dto.BranchId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = await _canteenService.CreateOrderAsync(dto);
        if (result.Success)
        {
            return Json(new { success = true, orderId = result.Data.Id });
        }
        return Json(new { success = false, message = result.Message });
    }

    // ==================== Orders ====================
    public async Task<IActionResult> Orders()
    {
        var search = new CanteenOrderSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetOrdersAsync(search);
        return View(result.Data);
    }

    public async Task<IActionResult> OrderDetails(Guid id)
    {
        var result = await _canteenService.GetOrderAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(UpdateCanteenOrderDto dto)
    {
        var result = await _canteenService.UpdateOrderAsync(dto);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var result = await _canteenService.CancelOrderAsync(id);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    // ==================== Delivery Zones ====================
    public async Task<IActionResult> DeliveryZones()
    {
        var search = new CanteenSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetDeliveryZonesAsync(search);
        return View(result.Data);
    }

    public IActionResult CreateDeliveryZone()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeliveryZone(CreateDeliveryZoneDto dto)
    {
        // TODO: Get BranchId from current user
        dto.BranchId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = await _canteenService.CreateDeliveryZoneAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("DeliveryZones");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> EditDeliveryZone(Guid id)
    {
        var result = await _canteenService.GetDeliveryZoneAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> EditDeliveryZone(UpdateDeliveryZoneDto dto)
    {
        var result = await _canteenService.UpdateDeliveryZoneAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("DeliveryZones");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    // ==================== Tables ====================
    public async Task<IActionResult> Tables()
    {
        var search = new CanteenSearchDto { Page = 1, PageSize = 20 };
        var result = await _canteenService.GetCanteenTablesAsync(search);
        return View(result.Data);
    }

    public IActionResult CreateTable()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable(CreateCanteenTableDto dto)
    {
        // TODO: Get BranchId from current user
        dto.BranchId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = await _canteenService.CreateCanteenTableAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Tables");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }
}