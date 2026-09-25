using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller لإدارة المخازن
/// </summary>
[Authorize]
public class InventoryController : Controller
{
    private readonly IInventoryService _inventoryService;
    private readonly InventoryAlertService _alertService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryService inventoryService, InventoryAlertService alertService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService;
        _alertService = alertService;
        _logger = logger;
    }

    // ==================== Warehouse Management ====================

    /// <summary>
    /// صفحة قائمة المخازن
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Warehouses()
    {
        var warehouses = await _inventoryService.GetAllWarehousesAsync();
        return View(warehouses);
    }

    /// <summary>
    /// صفحة إنشاء مخزن جديد
    /// </summary>
    [HttpGet]
    public IActionResult CreateWarehouse()
    {
        return View();
    }

    /// <summary>
    /// إنشاء مخزن جديد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateWarehouse(CreateWarehouseRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var warehouse = await _inventoryService.CreateWarehouseAsync(request);
            return RedirectToAction(nameof(Warehouses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating warehouse");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء المخزون");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تعديل المخزن
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EditWarehouse(Guid id)
    {
        var warehouse = await _inventoryService.GetWarehouseByIdAsync(id);
        if (warehouse == null)
            return NotFound();

        var request = new CreateWarehouseRequest
        {
            Name = warehouse.Name,
            NameArabic = warehouse.NameArabic,
            Code = warehouse.Code,
            SchoolId = warehouse.SchoolId,
            BranchId = warehouse.BranchId,
            WarehouseType = warehouse.WarehouseType,
            WarehouseTypeArabic = warehouse.WarehouseTypeArabic,
            Location = warehouse.Location,
            Manager = warehouse.Manager,
            ManagerPhone = warehouse.ManagerPhone,
            Capacity = warehouse.Capacity,
            CapacityUnit = warehouse.CapacityUnit,
            Notes = warehouse.Notes
        };

        return View(request);
    }

    /// <summary>
    /// تعديل المخزن
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditWarehouse(Guid id, CreateWarehouseRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var warehouse = await _inventoryService.UpdateWarehouseAsync(id, request);
            return RedirectToAction(nameof(Warehouses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating warehouse");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تعديل المخزون");
            return View(request);
        }
    }

    /// <summary>
    /// حذف المخزن
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteWarehouse(Guid id)
    {
        try
        {
            await _inventoryService.DeleteWarehouseAsync(id);
            return RedirectToAction(nameof(Warehouses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting warehouse");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حذف المخزون");
            return RedirectToAction(nameof(Warehouses));
        }
    }

    // ==================== Inventory Item Management ====================

    /// <summary>
    /// صفحة قائمة الأصناف
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Items()
    {
        var items = await _inventoryService.GetAllInventoryItemsAsync();
        return View(items);
    }

    /// <summary>
    /// صفحة إنشاء صنف جديد
    /// </summary>
    [HttpGet]
    public IActionResult CreateItem()
    {
        return View();
    }

    /// <summary>
    /// إنشاء صنف جديد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateItem(CreateInventoryItemRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var item = await _inventoryService.CreateInventoryItemAsync(request);
            return RedirectToAction(nameof(Items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء الصنف");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تعديل الصنف
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EditItem(Guid id)
    {
        var item = await _inventoryService.GetInventoryItemByIdAsync(id);
        if (item == null)
            return NotFound();

        var request = new CreateInventoryItemRequest
        {
            Name = item.Name,
            NameArabic = item.NameArabic,
            SKU = item.SKU,
            Barcode = item.Barcode,
            Category = item.Category,
            CategoryArabic = item.CategoryArabic,
            UnitOfMeasure = item.UnitOfMeasure,
            UnitOfMeasureArabic = item.UnitOfMeasureArabic,
            ReorderLevel = item.ReorderLevel,
            MaxStockLevel = item.MaxStockLevel,
            ReorderQuantity = item.ReorderQuantity,
            AverageCost = item.AverageCost,
            SellingPrice = item.SellingPrice,
            Currency = item.Currency,
            IsStockItem = item.IsStockItem,
            Description = item.Description,
            DescriptionArabic = item.DescriptionArabic,
            PreferredSupplierId = item.PreferredSupplierId,
            Notes = item.Notes
        };

        return View(request);
    }

    /// <summary>
    /// تعديل الصنف
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditItem(Guid id, CreateInventoryItemRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var item = await _inventoryService.UpdateInventoryItemAsync(id, request);
            return RedirectToAction(nameof(Items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تعديل الصنف");
            return View(request);
        }
    }

    /// <summary>
    /// حذف الصنف
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        try
        {
            await _inventoryService.DeleteInventoryItemAsync(id);
            return RedirectToAction(nameof(Items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inventory item");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حذف الصنف");
            return RedirectToAction(nameof(Items));
        }
    }

    // ==================== Stock Transaction Management ====================

    /// <summary>
    /// صفحة قائمة حركات المخزون
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Transactions()
    {
        var transactions = await _inventoryService.GetAllStockTransactionsAsync();
        return View(transactions);
    }

    /// <summary>
    /// صفحة إنشاء حركة مخزون
    /// </summary>
    [HttpGet]
    public IActionResult CreateTransaction()
    {
        return View();
    }

    /// <summary>
    /// إنشاء حركة مخزون
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTransaction(CreateStockTransactionRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var transaction = await _inventoryService.CreateStockTransactionAsync(request);
            return RedirectToAction(nameof(Transactions));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock transaction");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء الحركة");
            return View(request);
        }
    }

    // ==================== Stock Adjustment Management ====================

    /// <summary>
    /// صفحة قائمة تعديلات المخزون
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Adjustments()
    {
        var adjustments = await _inventoryService.GetAllStockAdjustmentsAsync();
        return View(adjustments);
    }

    /// <summary>
    /// صفحة إنشاء تعديل مخزون
    /// </summary>
    [HttpGet]
    public IActionResult CreateAdjustment()
    {
        return View();
    }

    /// <summary>
    /// إنشاء تعديل مخزون
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdjustment(StockAdjustmentDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var adjustment = await _inventoryService.CreateStockAdjustmentAsync(request);
            return RedirectToAction(nameof(Adjustments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating stock adjustment");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء التعديل");
            return View(request);
        }
    }

    /// <summary>
    /// اعتماد تعديل المخزون
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveAdjustment(Guid id, string approvedBy, string? notes)
    {
        try
        {
            await _inventoryService.ApproveStockAdjustmentAsync(id, approvedBy, notes);
            return RedirectToAction(nameof(Adjustments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving stock adjustment");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء اعتماد التعديل");
            return RedirectToAction(nameof(Adjustments));
        }
    }

    // ==================== Stock Reports ====================

    /// <summary>
    /// تقرير المخزون
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> StockReport(Guid? warehouseId)
    {
        var report = await _inventoryService.GetStockReportAsync(warehouseId);
        return View(report);
    }

    /// <summary>
    /// تقرير الأصناف منخفضة المخزون
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> LowStockReport()
    {
        var report = await _inventoryService.GetLowStockItemsAsync();
        return View(report);
    }

    /// <summary>
    /// تقرير الأصناف الزائدة عن الحد الأقصى
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> OverStockReport()
    {
        var report = await _inventoryService.GetOverStockItemsAsync();
        return View(report);
    }

    // ==================== Alerts Management ====================

    /// <summary>
    /// ملخص التنبيهات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Alerts()
    {
        var summary = await _alertService.GetAlertSummaryAsync();
        return View(summary);
    }

    /// <summary>
    /// فحص التنبيهات يدوياً
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckAlerts()
    {
        try
        {
            await _alertService.CheckLowStockAlertsAsync();
            await _alertService.CheckOverStockAlertsAsync();
            
            TempData["Success"] = "تم فحص التنبيهات بنجاح";
            return RedirectToAction(nameof(Alerts));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking alerts");
            TempData["Error"] = "حدث خطأ أثناء فحص التنبيهات";
            return RedirectToAction(nameof(Alerts));
        }
    }
}
