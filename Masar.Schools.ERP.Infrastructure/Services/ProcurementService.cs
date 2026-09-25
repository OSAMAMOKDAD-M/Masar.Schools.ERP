using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة المشتريات
/// </summary>
public class ProcurementService : IProcurementService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<ProcurementService> _logger;

    public ProcurementService(MasarDbContext context, ILogger<ProcurementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ==================== Supplier Operations ====================

    public async Task<List<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            NameArabic = s.NameArabic,
            Code = s.Code,
            CommercialRegistration = s.CommercialRegistration,
            TaxNumber = s.TaxNumber,
            Address = s.Address,
            City = s.City,
            Phone = s.Phone,
            Email = s.Email,
            ContactPerson = s.ContactPerson,
            ContactPhone = s.ContactPhone,
            SupplierCategory = s.SupplierCategory,
            SupplierCategoryArabic = s.SupplierCategoryArabic,
            PaymentTerms = s.PaymentTerms,
            CreditLimit = s.CreditLimit,
            Currency = s.Currency,
            IsActive = s.IsActive,
            Notes = s.Notes,
            Rating = s.Rating,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return null;

        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            NameArabic = supplier.NameArabic,
            Code = supplier.Code,
            CommercialRegistration = supplier.CommercialRegistration,
            TaxNumber = supplier.TaxNumber,
            Address = supplier.Address,
            City = supplier.City,
            Phone = supplier.Phone,
            Email = supplier.Email,
            ContactPerson = supplier.ContactPerson,
            ContactPhone = supplier.ContactPhone,
            SupplierCategory = supplier.SupplierCategory,
            SupplierCategoryArabic = supplier.SupplierCategoryArabic,
            PaymentTerms = supplier.PaymentTerms,
            CreditLimit = supplier.CreditLimit,
            Currency = supplier.Currency,
            IsActive = supplier.IsActive,
            Notes = supplier.Notes,
            Rating = supplier.Rating,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }

    public async Task<SupplierDto> CreateSupplierAsync(SupplierDto request)
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NameArabic = request.NameArabic,
            Code = request.Code,
            CommercialRegistration = request.CommercialRegistration,
            TaxNumber = request.TaxNumber,
            Address = request.Address,
            City = request.City,
            Phone = request.Phone,
            Email = request.Email,
            ContactPerson = request.ContactPerson,
            ContactPhone = request.ContactPhone,
            SupplierCategory = request.SupplierCategory,
            SupplierCategoryArabic = request.SupplierCategoryArabic,
            PaymentTerms = request.PaymentTerms,
            CreditLimit = request.CreditLimit,
            Currency = request.Currency,
            IsActive = true,
            Notes = request.Notes,
            Rating = request.Rating,
            CreatedAt = DateTime.UtcNow
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Supplier created: {supplier.Name}");

        return await GetSupplierByIdAsync(supplier.Id);
    }

    public async Task<SupplierDto> UpdateSupplierAsync(Guid id, SupplierDto request)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
            throw new Exception("Supplier not found");

        supplier.Name = request.Name;
        supplier.NameArabic = request.NameArabic;
        supplier.Code = request.Code;
        supplier.CommercialRegistration = request.CommercialRegistration;
        supplier.TaxNumber = request.TaxNumber;
        supplier.Address = request.Address;
        supplier.City = request.City;
        supplier.Phone = request.Phone;
        supplier.Email = request.Email;
        supplier.ContactPerson = request.ContactPerson;
        supplier.ContactPhone = request.ContactPhone;
        supplier.SupplierCategory = request.SupplierCategory;
        supplier.SupplierCategoryArabic = request.SupplierCategoryArabic;
        supplier.PaymentTerms = request.PaymentTerms;
        supplier.CreditLimit = request.CreditLimit;
        supplier.Notes = request.Notes;
        supplier.Rating = request.Rating;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Supplier updated: {supplier.Name}");

        return await GetSupplierByIdAsync(id);
    }

    public async Task<bool> DeleteSupplierAsync(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null) return false;

        supplier.IsDeleted = true;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Supplier deleted: {supplier.Name}");

        return true;
    }

    // ==================== Purchase Order Operations ====================

    public async Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync()
    {
        var orders = await _context.PurchaseOrders
            .Include(p => p.Supplier)
            .Include(p => p.Warehouse)
            .OrderByDescending(p => p.OrderDate)
            .ToListAsync();

        return orders.Select(p => new PurchaseOrderDto
        {
            Id = p.Id,
            OrderNumber = p.OrderNumber,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier?.Name,
            WarehouseId = p.WarehouseId,
            WarehouseName = p.Warehouse?.Name,
            OrderDate = p.OrderDate,
            ExpectedDeliveryDate = p.ExpectedDeliveryDate,
            ActualDeliveryDate = p.ActualDeliveryDate,
            Status = p.Status,
            StatusArabic = p.StatusArabic,
            TotalAmount = p.TotalAmount,
            PaidAmount = p.PaidAmount,
            RemainingAmount = p.RemainingAmount,
            Currency = p.Currency,
            PaymentTerms = p.PaymentTerms,
            DeliveryTerms = p.DeliveryTerms,
            Notes = p.Notes,
            NotesArabic = p.NotesArabic,
            ApprovedBy = p.ApprovedBy,
            ApprovedByName = p.ApprovedByName,
            ApprovedAt = p.ApprovedAt,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(Guid id)
    {
        var order = await _context.PurchaseOrders
            .Include(p => p.Supplier)
            .Include(p => p.Warehouse)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (order == null) return null;

        return new PurchaseOrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier?.Name,
            WarehouseId = order.WarehouseId,
            WarehouseName = order.Warehouse?.Name,
            OrderDate = order.OrderDate,
            ExpectedDeliveryDate = order.ExpectedDeliveryDate,
            ActualDeliveryDate = order.ActualDeliveryDate,
            Status = order.Status,
            StatusArabic = order.StatusArabic,
            TotalAmount = order.TotalAmount,
            PaidAmount = order.PaidAmount,
            RemainingAmount = order.RemainingAmount,
            Currency = order.Currency,
            PaymentTerms = order.PaymentTerms,
            DeliveryTerms = order.DeliveryTerms,
            Notes = order.Notes,
            NotesArabic = order.NotesArabic,
            ApprovedBy = order.ApprovedBy,
            ApprovedByName = order.ApprovedByName,
            ApprovedAt = order.ApprovedAt,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request)
    {
        var order = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            SupplierId = request.SupplierId,
            WarehouseId = request.WarehouseId,
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            Status = "Pending",
            StatusArabic = "قيد الانتظار",
            PaymentTerms = request.PaymentTerms,
            DeliveryTerms = request.DeliveryTerms,
            Notes = request.Notes,
            NotesArabic = request.NotesArabic,
            Currency = "SAR",
            CreatedAt = DateTime.UtcNow
        };

        // Calculate total amount from items
        decimal totalAmount = 0;
        foreach (var itemRequest in request.Items)
        {
            var itemTotal = itemRequest.Quantity * itemRequest.UnitPrice;
            totalAmount += itemTotal - itemRequest.Discount + itemRequest.Tax;
        }

        order.TotalAmount = totalAmount;
        order.RemainingAmount = totalAmount;

        _context.PurchaseOrders.Add(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase order created: {order.OrderNumber}");

        return await GetPurchaseOrderByIdAsync(order.Id);
    }

    public async Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(Guid id, CreatePurchaseOrderRequest request)
    {
        var order = await _context.PurchaseOrders.FindAsync(id);
        if (order == null)
            throw new Exception("Purchase order not found");

        order.SupplierId = request.SupplierId;
        order.WarehouseId = request.WarehouseId;
        order.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
        order.PaymentTerms = request.PaymentTerms;
        order.DeliveryTerms = request.DeliveryTerms;
        order.Notes = request.Notes;
        order.NotesArabic = request.NotesArabic;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase order updated: {order.OrderNumber}");

        return await GetPurchaseOrderByIdAsync(id);
    }

    public async Task<bool> ApprovePurchaseOrderAsync(Guid id, string approvedBy)
    {
        var order = await _context.PurchaseOrders.FindAsync(id);
        if (order == null) return false;

        order.Status = "Approved";
        order.StatusArabic = "معتم";
        order.ApprovedBy = Guid.Parse(approvedBy);
        order.ApprovedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase order approved: {order.OrderNumber}");

        return true;
    }

    public async Task<bool> ReceivePurchaseOrderAsync(Guid id, string receivedBy)
    {
        var order = await _context.PurchaseOrders.FindAsync(id);
        if (order == null) return false;

        order.Status = "Received";
        order.StatusArabic = "مستلم";
        order.ActualDeliveryDate = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase order received: {order.OrderNumber}");

        return true;
    }

    public async Task<bool> CancelPurchaseOrderAsync(Guid id)
    {
        var order = await _context.PurchaseOrders.FindAsync(id);
        if (order == null) return false;

        order.Status = "Cancelled";
        order.StatusArabic = "ملغي";
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase order cancelled: {order.OrderNumber}");

        return true;
    }

    // ==================== Purchase Invoice Operations ====================

    public async Task<List<PurchaseInvoiceDto>> GetAllPurchaseInvoicesAsync()
    {
        var invoices = await _context.PurchaseInvoices
            .Include(i => i.Supplier)
            .Include(i => i.School)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();

        return invoices.Select(i => new PurchaseInvoiceDto
        {
            Id = i.Id,
            InvoiceNumber = i.InvoiceNumber,
            TaxInvoiceNumber = i.TaxInvoiceNumber,
            PurchaseOrderId = i.PurchaseOrderId,
            SupplierId = i.SupplierId,
            SupplierName = i.Supplier?.Name,
            InvoiceDate = i.InvoiceDate,
            DueDate = i.DueDate,
            SubTotal = i.SubTotal,
            TaxAmount = i.TaxAmount,
            Discount = i.Discount,
            TotalAmount = i.TotalAmount,
            PaidAmount = i.PaidAmount,
            RemainingAmount = i.RemainingAmount,
            Currency = i.Currency,
            Status = i.Status,
            StatusArabic = i.StatusArabic,
            IsZatcaApproved = i.IsZatcaApproved,
            ZatcaId = i.ZatcaId,
            ZatcaApprovedAt = i.ZatcaApprovedAt,
            Notes = i.Notes,
            NotesArabic = i.NotesArabic,
            SchoolId = i.SchoolId,
            SchoolName = i.School?.Name,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        }).ToList();
    }

    public async Task<PurchaseInvoiceDto?> GetPurchaseInvoiceByIdAsync(Guid id)
    {
        var invoice = await _context.PurchaseInvoices
            .Include(i => i.Supplier)
            .Include(i => i.School)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null) return null;

        return new PurchaseInvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            TaxInvoiceNumber = invoice.TaxInvoiceNumber,
            PurchaseOrderId = invoice.PurchaseOrderId,
            SupplierId = invoice.SupplierId,
            SupplierName = invoice.Supplier?.Name,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            SubTotal = invoice.SubTotal,
            TaxAmount = invoice.TaxAmount,
            Discount = invoice.Discount,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            RemainingAmount = invoice.RemainingAmount,
            Currency = invoice.Currency,
            Status = invoice.Status,
            StatusArabic = invoice.StatusArabic,
            IsZatcaApproved = invoice.IsZatcaApproved,
            ZatcaId = invoice.ZatcaId,
            ZatcaApprovedAt = invoice.ZatcaApprovedAt,
            Notes = invoice.Notes,
            NotesArabic = invoice.NotesArabic,
            SchoolId = invoice.SchoolId,
            SchoolName = invoice.School?.Name,
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };
    }

    public async Task<PurchaseInvoiceDto> CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request)
    {
        var invoice = new PurchaseInvoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = $"PINV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            PurchaseOrderId = request.PurchaseOrderId,
            SupplierId = request.SupplierId,
            InvoiceDate = request.InvoiceDate,
            DueDate = request.DueDate,
            TaxInvoiceNumber = request.TaxInvoiceNumber,
            Discount = request.Discount,
            Notes = request.Notes,
            NotesArabic = request.NotesArabic,
            Status = "Pending",
            StatusArabic = "قيد الانتظار",
            Currency = "SAR",
            SchoolId = Guid.NewGuid(), // Should be from request
            CreatedAt = DateTime.UtcNow
        };

        // Calculate totals from items
        decimal subTotal = 0;
        decimal taxAmount = 0;
        foreach (var itemRequest in request.Items)
        {
            subTotal += itemRequest.Quantity * itemRequest.UnitPrice;
            taxAmount += itemRequest.Tax;
        }

        invoice.SubTotal = subTotal;
        invoice.TaxAmount = taxAmount;
        invoice.TotalAmount = subTotal + taxAmount - invoice.Discount;
        invoice.RemainingAmount = invoice.TotalAmount;

        _context.PurchaseInvoices.Add(invoice);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase invoice created: {invoice.InvoiceNumber}");

        return await GetPurchaseInvoiceByIdAsync(invoice.Id);
    }

    public async Task<PurchaseInvoiceDto> UpdatePurchaseInvoiceAsync(Guid id, CreatePurchaseInvoiceRequest request)
    {
        var invoice = await _context.PurchaseInvoices.FindAsync(id);
        if (invoice == null)
            throw new Exception("Purchase invoice not found");

        invoice.PurchaseOrderId = request.PurchaseOrderId;
        invoice.SupplierId = request.SupplierId;
        invoice.InvoiceDate = request.InvoiceDate;
        invoice.DueDate = request.DueDate;
        invoice.TaxInvoiceNumber = request.TaxInvoiceNumber;
        invoice.Discount = request.Discount;
        invoice.Notes = request.Notes;
        invoice.NotesArabic = request.NotesArabic;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase invoice updated: {invoice.InvoiceNumber}");

        return await GetPurchaseInvoiceByIdAsync(id);
    }

    public async Task<bool> ApproveZatcaInvoiceAsync(Guid id)
    {
        var invoice = await _context.PurchaseInvoices.FindAsync(id);
        if (invoice == null) return false;

        invoice.IsZatcaApproved = true;
        invoice.ZatcaId = $"ZATCA-{Guid.NewGuid()}";
        invoice.ZatcaApprovedAt = DateTime.UtcNow;
        invoice.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase invoice ZATCA approved: {invoice.InvoiceNumber}");

        return true;
    }

    // ==================== Purchase Request Operations ====================

    public async Task<List<PurchaseRequestDto>> GetAllPurchaseRequestsAsync()
    {
        var requests = await _context.PurchaseRequests
            .Include(r => r.School)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync();

        return requests.Select(r => new PurchaseRequestDto
        {
            Id = r.Id,
            RequestNumber = r.RequestNumber,
            DepartmentId = r.DepartmentId,
            DepartmentName = r.DepartmentName,
            SchoolId = r.SchoolId,
            SchoolName = r.School?.Name,
            RequestDate = r.RequestDate,
            RequiredBy = r.RequiredBy,
            RequestedBy = r.RequestedBy,
            Purpose = r.Purpose,
            PurposeArabic = r.PurposeArabic,
            BudgetAmount = r.BudgetAmount,
            Currency = r.Currency,
            Status = r.Status,
            StatusArabic = r.StatusArabic,
            ApprovedBy = r.ApprovedBy,
            ApprovedByName = null,
            ApprovedAt = r.ApprovedAt,
            ApprovalNotes = r.ApprovalNotes,
            Notes = r.Notes,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        }).ToList();
    }

    public async Task<PurchaseRequestDto?> GetPurchaseRequestByIdAsync(Guid id)
    {
        var request = await _context.PurchaseRequests
            .Include(r => r.School)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null) return null;

        return new PurchaseRequestDto
        {
            Id = request.Id,
            RequestNumber = request.RequestNumber,
            DepartmentId = request.DepartmentId,
            DepartmentName = request.DepartmentName,
            SchoolId = request.SchoolId,
            SchoolName = request.School?.Name,
            RequestDate = request.RequestDate,
            RequiredBy = request.RequiredBy,
            RequestedBy = request.RequestedBy,
            Purpose = request.Purpose,
            PurposeArabic = request.PurposeArabic,
            BudgetAmount = request.BudgetAmount,
            Currency = request.Currency,
            Status = request.Status,
            StatusArabic = request.StatusArabic,
            ApprovedBy = request.ApprovedBy,
            ApprovedByName = null,
            ApprovedAt = request.ApprovedAt,
            ApprovalNotes = request.ApprovalNotes,
            Notes = request.Notes,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt
        };
    }

    public async Task<PurchaseRequestDto> CreatePurchaseRequestAsync(CreatePurchaseRequestRequest request)
    {
        var purchaseRequest = new PurchaseRequest
        {
            Id = Guid.NewGuid(),
            RequestNumber = $"PR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            DepartmentId = request.DepartmentId,
            DepartmentName = request.DepartmentName,
            SchoolId = request.SchoolId,
            RequestDate = DateTime.UtcNow,
            RequiredBy = request.RequiredBy,
            RequestedBy = request.RequestedBy,
            Purpose = request.Purpose,
            PurposeArabic = request.PurposeArabic,
            BudgetAmount = request.BudgetAmount,
            Currency = request.Currency,
            Status = "Draft",
            StatusArabic = "مسودة",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.PurchaseRequests.Add(purchaseRequest);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase request created: {purchaseRequest.RequestNumber}");

        return await GetPurchaseRequestByIdAsync(purchaseRequest.Id);
    }

    public async Task<bool> ApprovePurchaseRequestAsync(Guid id, string approvedBy, string? notes)
    {
        var request = await _context.PurchaseRequests.FindAsync(id);
        if (request == null) return false;

        request.Status = "Approved";
        request.StatusArabic = "معتم";
        request.ApprovedBy = Guid.Parse(approvedBy);
        request.ApprovedAt = DateTime.UtcNow;
        request.ApprovalNotes = notes;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase request approved: {request.RequestNumber}");

        return true;
    }

    public async Task<bool> RejectPurchaseRequestAsync(Guid id, string approvedBy, string? notes)
    {
        var request = await _context.PurchaseRequests.FindAsync(id);
        if (request == null) return false;

        request.Status = "Rejected";
        request.StatusArabic = "مرفوض";
        request.ApprovedBy = Guid.Parse(approvedBy);
        request.ApprovedAt = DateTime.UtcNow;
        request.ApprovalNotes = notes;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Purchase request rejected: {request.RequestNumber}");

        return true;
    }

    // ==================== Inventory Transfer Operations ====================

    public async Task<List<InventoryTransferDto>> GetAllInventoryTransfersAsync()
    {
        var transfers = await _context.InventoryTransfers
            .Include(t => t.FromWarehouse)
            .Include(t => t.ToWarehouse)
            .OrderByDescending(t => t.TransferDate)
            .ToListAsync();

        return transfers.Select(t => new InventoryTransferDto
        {
            Id = t.Id,
            TransferNumber = t.TransferNumber,
            FromWarehouseId = t.FromWarehouseId,
            FromWarehouseName = t.FromWarehouse?.Name,
            ToWarehouseId = t.ToWarehouseId,
            ToWarehouseName = t.ToWarehouse?.Name,
            TransferDate = t.TransferDate,
            Status = t.Status,
            StatusArabic = t.StatusArabic,
            TotalValue = t.TotalValue,
            Currency = t.Currency,
            Reason = t.Reason,
            ReasonArabic = t.ReasonArabic,
            Notes = t.Notes,
            ApprovedBy = t.ApprovedBy,
            ApprovedByName = null,
            ApprovedAt = t.ApprovedAt,
            PerformedBy = t.PerformedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();
    }

    public async Task<InventoryTransferDto?> GetInventoryTransferByIdAsync(Guid id)
    {
        var transfer = await _context.InventoryTransfers
            .Include(t => t.FromWarehouse)
            .Include(t => t.ToWarehouse)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transfer == null) return null;

        return new InventoryTransferDto
        {
            Id = transfer.Id,
            TransferNumber = transfer.TransferNumber,
            FromWarehouseId = transfer.FromWarehouseId,
            FromWarehouseName = transfer.FromWarehouse?.Name,
            ToWarehouseId = transfer.ToWarehouseId,
            ToWarehouseName = transfer.ToWarehouse?.Name,
            TransferDate = transfer.TransferDate,
            Status = transfer.Status,
            StatusArabic = transfer.StatusArabic,
            TotalValue = transfer.TotalValue,
            Currency = transfer.Currency,
            Reason = transfer.Reason,
            ReasonArabic = transfer.ReasonArabic,
            Notes = transfer.Notes,
            ApprovedBy = transfer.ApprovedBy,
            ApprovedByName = null,
            ApprovedAt = transfer.ApprovedAt,
            PerformedBy = transfer.PerformedBy,
            CreatedAt = transfer.CreatedAt,
            UpdatedAt = transfer.UpdatedAt
        };
    }

    public async Task<InventoryTransferDto> CreateInventoryTransferAsync(InventoryTransferDto request)
    {
        var transfer = new InventoryTransfer
        {
            Id = Guid.NewGuid(),
            TransferNumber = $"TRN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            FromWarehouseId = request.FromWarehouseId,
            ToWarehouseId = request.ToWarehouseId,
            TransferDate = request.TransferDate,
            Status = "Draft",
            StatusArabic = "مسودة",
            TotalValue = request.TotalValue,
            Currency = request.Currency,
            Reason = request.Reason,
            ReasonArabic = request.ReasonArabic,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryTransfers.Add(transfer);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory transfer created: {transfer.TransferNumber}");

        return await GetInventoryTransferByIdAsync(transfer.Id);
    }

    public async Task<bool> ApproveInventoryTransferAsync(Guid id, string approvedBy)
    {
        var transfer = await _context.InventoryTransfers.FindAsync(id);
        if (transfer == null) return false;

        transfer.Status = "InTransit";
        transfer.StatusArabic = "في الطريق";
        transfer.ApprovedBy = Guid.Parse(approvedBy);
        transfer.ApprovedAt = DateTime.UtcNow;
        transfer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory transfer approved: {transfer.TransferNumber}");

        return true;
    }

    public async Task<bool> CompleteInventoryTransferAsync(Guid id, string performedBy)
    {
        var transfer = await _context.InventoryTransfers.FindAsync(id);
        if (transfer == null) return false;

        transfer.Status = "Completed";
        transfer.StatusArabic = "مكتمل";
        transfer.PerformedBy = performedBy;
        transfer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory transfer completed: {transfer.TransferNumber}");

        return true;
    }

    // ==================== Procurement Reports ====================

    public async Task<List<ProcurementReportDto>> GetProcurementReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.PurchaseOrders.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(p => p.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.OrderDate <= endDate.Value);

        var orders = await query
            .Include(p => p.Supplier)
            .ToListAsync();

        return orders.GroupBy(p => p.SupplierId)
            .Select(g => new ProcurementReportDto
            {
                SupplierId = g.Key,
                SupplierName = g.FirstOrDefault()?.Supplier?.Name ?? "",
                TotalOrders = g.Count(),
                TotalAmount = g.Sum(p => p.TotalAmount),
                PaidAmount = g.Sum(p => p.PaidAmount),
                PendingAmount = g.Sum(p => p.RemainingAmount),
                Currency = "SAR",
                Rating = g.FirstOrDefault()?.Supplier?.Rating ?? 3
            })
            .ToList();
    }

    public async Task<List<ProcurementReportDto>> GetSupplierPerformanceReportAsync()
    {
        var suppliers = await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return suppliers.Select(s => new ProcurementReportDto
        {
            SupplierId = s.Id,
            SupplierName = s.Name,
            TotalOrders = 0, // Would need to calculate from orders
            TotalAmount = 0,
            PaidAmount = 0,
            PendingAmount = 0,
            Currency = s.Currency,
            Rating = s.Rating
        }).ToList();
    }
}
