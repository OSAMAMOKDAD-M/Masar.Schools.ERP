using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller لإدارة المشتريات
/// </summary>
[Authorize]
public class ProcurementController : Controller
{
    private readonly IProcurementService _procurementService;
    private readonly FinancialIntegrationService _financialIntegrationService;
    private readonly ILogger<ProcurementController> _logger;

    public ProcurementController(IProcurementService procurementService, FinancialIntegrationService financialIntegrationService, ILogger<ProcurementController> logger)
    {
        _procurementService = procurementService;
        _financialIntegrationService = financialIntegrationService;
        _logger = logger;
    }

    // ==================== Supplier Management ====================

    /// <summary>
    /// صفحة قائمة الموردين
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Suppliers()
    {
        var suppliers = await _procurementService.GetAllSuppliersAsync();
        return View(suppliers);
    }

    /// <summary>
    /// صفحة إنشاء مورد جديد
    /// </summary>
    [HttpGet]
    public IActionResult CreateSupplier()
    {
        return View();
    }

    /// <summary>
    /// إنشاء مورد جديد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSupplier(SupplierDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var supplier = await _procurementService.CreateSupplierAsync(request);
            return RedirectToAction(nameof(Suppliers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supplier");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء المورد");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تعديل المورد
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> EditSupplier(Guid id)
    {
        var supplier = await _procurementService.GetSupplierByIdAsync(id);
        if (supplier == null)
            return NotFound();

        return View(supplier);
    }

    /// <summary>
    /// تعديل المورد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSupplier(Guid id, SupplierDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var supplier = await _procurementService.UpdateSupplierAsync(id, request);
            return RedirectToAction(nameof(Suppliers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating supplier");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تعديل المورد");
            return View(request);
        }
    }

    /// <summary>
    /// حذف المورد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSupplier(Guid id)
    {
        try
        {
            await _procurementService.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Suppliers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting supplier");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حذف المورد");
            return RedirectToAction(nameof(Suppliers));
        }
    }

    // ==================== Purchase Order Management ====================

    /// <summary>
    /// صفحة قائمة أوامر الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseOrders()
    {
        var orders = await _procurementService.GetAllPurchaseOrdersAsync();
        return View(orders);
    }

    /// <summary>
    /// صفحة إنشاء أمر شراء
    /// </summary>
    [HttpGet]
    public IActionResult CreatePurchaseOrder()
    {
        return View();
    }

    /// <summary>
    /// إنشاء أمر شراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var order = await _procurementService.CreatePurchaseOrderAsync(request);
            return RedirectToAction(nameof(PurchaseOrders));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase order");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء أمر الشراء");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تفاصيل أمر الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseOrderDetails(Guid id)
    {
        var order = await _procurementService.GetPurchaseOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        return View(order);
    }

    /// <summary>
    /// اعتماد أمر الشراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePurchaseOrder(Guid id, string approvedBy)
    {
        try
        {
            await _procurementService.ApprovePurchaseOrderAsync(id, approvedBy);
            return RedirectToAction(nameof(PurchaseOrders));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving purchase order");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء اعتماد أمر الشراء");
            return RedirectToAction(nameof(PurchaseOrders));
        }
    }

    /// <summary>
    /// استلام أمر الشراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReceivePurchaseOrder(Guid id, string receivedBy)
    {
        try
        {
            var result = await _procurementService.ReceivePurchaseOrderAsync(id, receivedBy);
            
            // إنشاء قيد دفتري تلقائي
            var order = await _procurementService.GetPurchaseOrderByIdAsync(id);
            if (order != null)
            {
                await _financialIntegrationService.CreatePurchaseOrderEntryAsync(order);
            }
            
            return RedirectToAction(nameof(PurchaseOrders));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving purchase order");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء استلام أمر الشراء");
            return RedirectToAction(nameof(PurchaseOrders));
        }
    }

    /// <summary>
    /// إلغاء أمر الشراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelPurchaseOrder(Guid id)
    {
        try
        {
            await _procurementService.CancelPurchaseOrderAsync(id);
            return RedirectToAction(nameof(PurchaseOrders));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling purchase order");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إلغاء أمر الشراء");
            return RedirectToAction(nameof(PurchaseOrders));
        }
    }

    // ==================== Purchase Invoice Management ====================

    /// <summary>
    /// صفحة قائمة فواتير الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseInvoices()
    {
        var invoices = await _procurementService.GetAllPurchaseInvoicesAsync();
        return View(invoices);
    }

    /// <summary>
    /// صفحة إنشاء فاتورة شراء
    /// </summary>
    [HttpGet]
    public IActionResult CreatePurchaseInvoice()
    {
        return View();
    }

    /// <summary>
    /// إنشاء فاتورة شراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePurchaseInvoice(CreatePurchaseInvoiceRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var invoice = await _procurementService.CreatePurchaseInvoiceAsync(request);
            return RedirectToAction(nameof(PurchaseInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase invoice");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء الفاتورة");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تفاصيل فاتورة الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseInvoiceDetails(Guid id)
    {
        var invoice = await _procurementService.GetPurchaseInvoiceByIdAsync(id);
        if (invoice == null)
            return NotFound();

        return View(invoice);
    }

    /// <summary>
    /// اعتماد الفاتورة مع ZATCA
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveZatcaInvoice(Guid id)
    {
        try
        {
            await _procurementService.ApproveZatcaInvoiceAsync(id);
            return RedirectToAction(nameof(PurchaseInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving ZATCA invoice");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء اعتماد الفاتورة مع ZATCA");
            return RedirectToAction(nameof(PurchaseInvoices));
        }
    }

    /// <summary>
    /// تسجيل دفع فاتورة
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordInvoicePayment(Guid id, decimal paymentAmount)
    {
        try
        {
            var invoice = await _procurementService.GetPurchaseInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            // إنشاء قيد دفتري للدفع
            await _financialIntegrationService.CreatePurchaseInvoicePaymentEntryAsync(invoice, paymentAmount);

            TempData["Success"] = "تم تسجيل الدفع بنجاح";
            return RedirectToAction(nameof(PurchaseInvoices));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording invoice payment");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تسجيل الدفع");
            return RedirectToAction(nameof(PurchaseInvoices));
        }
    }

    // ==================== Purchase Request Management ====================

    /// <summary>
    /// صفحة قائمة طلبات الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseRequests()
    {
        var requests = await _procurementService.GetAllPurchaseRequestsAsync();
        return View(requests);
    }

    /// <summary>
    /// صفحة إنشاء طلب شراء
    /// </summary>
    [HttpGet]
    public IActionResult CreatePurchaseRequest()
    {
        return View();
    }

    /// <summary>
    /// إنشاء طلب شراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePurchaseRequest(CreatePurchaseRequestRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var purchaseRequest = await _procurementService.CreatePurchaseRequestAsync(request);
            return RedirectToAction(nameof(PurchaseRequests));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase request");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء الطلب");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تفاصيل طلب الشراء
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseRequestDetails(Guid id)
    {
        var request = await _procurementService.GetPurchaseRequestByIdAsync(id);
        if (request == null)
            return NotFound();

        return View(request);
    }

    /// <summary>
    /// اعتماد طلب الشراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePurchaseRequest(Guid id, string approvedBy, string? notes)
    {
        try
        {
            await _procurementService.ApprovePurchaseRequestAsync(id, approvedBy, notes);
            return RedirectToAction(nameof(PurchaseRequests));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving purchase request");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء اعتماد الطلب");
            return RedirectToAction(nameof(PurchaseRequests));
        }
    }

    /// <summary>
    /// رفض طلب الشراء
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectPurchaseRequest(Guid id, string approvedBy, string? notes)
    {
        try
        {
            await _procurementService.RejectPurchaseRequestAsync(id, approvedBy, notes);
            return RedirectToAction(nameof(PurchaseRequests));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting purchase request");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء رفض الطلب");
            return RedirectToAction(nameof(PurchaseRequests));
        }
    }

    // ==================== Inventory Transfer Management ====================

    /// <summary>
    /// صفحة قائمة عمليات النقل
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> InventoryTransfers()
    {
        var transfers = await _procurementService.GetAllInventoryTransfersAsync();
        return View(transfers);
    }

    /// <summary>
    /// صفحة إنشاء عملية نقل
    /// </summary>
    [HttpGet]
    public IActionResult CreateInventoryTransfer()
    {
        return View();
    }

    /// <summary>
    /// إنشاء عملية نقل
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateInventoryTransfer(InventoryTransferDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var transfer = await _procurementService.CreateInventoryTransferAsync(request);
            return RedirectToAction(nameof(InventoryTransfers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory transfer");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء عملية النقل");
            return View(request);
        }
    }

    /// <summary>
    /// صفحة تفاصيل عملية النقل
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> InventoryTransferDetails(Guid id)
    {
        var transfer = await _procurementService.GetInventoryTransferByIdAsync(id);
        if (transfer == null)
            return NotFound();

        return View(transfer);
    }

    /// <summary>
    /// اعتماد عملية النقل
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveInventoryTransfer(Guid id, string approvedBy)
    {
        try
        {
            await _procurementService.ApproveInventoryTransferAsync(id, approvedBy);
            return RedirectToAction(nameof(InventoryTransfers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving inventory transfer");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء اعتماد عملية النقل");
            return RedirectToAction(nameof(InventoryTransfers));
        }
    }

    /// <summary>
    /// إكمال عملية النقل
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteInventoryTransfer(Guid id, string performedBy)
    {
        try
        {
            await _procurementService.CompleteInventoryTransferAsync(id, performedBy);
            return RedirectToAction(nameof(InventoryTransfers));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing inventory transfer");
            ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إكمال عملية النقل");
            return RedirectToAction(nameof(InventoryTransfers));
        }
    }

    // ==================== Procurement Reports ====================

    /// <summary>
    /// تقرير المشتريات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ProcurementReport(DateTime? startDate, DateTime? endDate)
    {
        var report = await _procurementService.GetProcurementReportAsync(startDate, endDate);
        return View(report);
    }

    /// <summary>
    /// تقرير أداء الموردين
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> SupplierPerformanceReport()
    {
        var report = await _procurementService.GetSupplierPerformanceReportAsync();
        return View(report);
    }

    // ==================== Financial Integration Reports ====================

    /// <summary>
    /// تقرير التكامل المالي
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> FinancialIntegrationReport(DateTime? startDate, DateTime? endDate)
    {
        var report = await _financialIntegrationService.GetFinancialIntegrationReportAsync(startDate, endDate);
        return View(report);
    }
}
