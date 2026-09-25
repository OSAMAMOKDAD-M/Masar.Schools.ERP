using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

// ==================== Inventory Service Interfaces ====================

/// <summary>
/// واجهة خدمة المخازن
/// </summary>
public interface IInventoryService
{
    // Warehouse Operations
    Task<List<WarehouseDto>> GetAllWarehousesAsync();
    Task<WarehouseDto?> GetWarehouseByIdAsync(Guid id);
    Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request);
    Task<WarehouseDto> UpdateWarehouseAsync(Guid id, CreateWarehouseRequest request);
    Task<bool> DeleteWarehouseAsync(Guid id);

    // Inventory Item Operations
    Task<List<InventoryItemDto>> GetAllInventoryItemsAsync();
    Task<InventoryItemDto?> GetInventoryItemByIdAsync(Guid id);
    Task<InventoryItemDto> CreateInventoryItemAsync(CreateInventoryItemRequest request);
    Task<InventoryItemDto> UpdateInventoryItemAsync(Guid id, CreateInventoryItemRequest request);
    Task<bool> DeleteInventoryItemAsync(Guid id);

    // Stock Transaction Operations
    Task<List<StockTransactionDto>> GetAllStockTransactionsAsync();
    Task<List<StockTransactionDto>> GetStockTransactionsByWarehouseAsync(Guid warehouseId);
    Task<List<StockTransactionDto>> GetStockTransactionsByItemAsync(Guid itemId);
    Task<StockTransactionDto> CreateStockTransactionAsync(CreateStockTransactionRequest request);

    // Stock Adjustment Operations
    Task<List<StockAdjustmentDto>> GetAllStockAdjustmentsAsync();
    Task<StockAdjustmentDto?> GetStockAdjustmentByIdAsync(Guid id);
    Task<StockAdjustmentDto> CreateStockAdjustmentAsync(StockAdjustmentDto request);
    Task<bool> ApproveStockAdjustmentAsync(Guid id, string approvedBy, string? notes);

    // Stock Reports
    Task<List<StockReportDto>> GetStockReportAsync(Guid? warehouseId = null);
    Task<List<StockReportDto>> GetLowStockItemsAsync();
    Task<List<StockReportDto>> GetOverStockItemsAsync();
}

/// <summary>
/// واجهة خدمة المشتريات
/// </summary>
public interface IProcurementService
{
    // Supplier Operations
    Task<List<SupplierDto>> GetAllSuppliersAsync();
    Task<SupplierDto?> GetSupplierByIdAsync(Guid id);
    Task<SupplierDto> CreateSupplierAsync(SupplierDto request);
    Task<SupplierDto> UpdateSupplierAsync(Guid id, SupplierDto request);
    Task<bool> DeleteSupplierAsync(Guid id);

    // Purchase Order Operations
    Task<List<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(Guid id);
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request);
    Task<PurchaseOrderDto> UpdatePurchaseOrderAsync(Guid id, CreatePurchaseOrderRequest request);
    Task<bool> ApprovePurchaseOrderAsync(Guid id, string approvedBy);
    Task<bool> ReceivePurchaseOrderAsync(Guid id, string receivedBy);
    Task<bool> CancelPurchaseOrderAsync(Guid id);

    // Purchase Invoice Operations
    Task<List<PurchaseInvoiceDto>> GetAllPurchaseInvoicesAsync();
    Task<PurchaseInvoiceDto?> GetPurchaseInvoiceByIdAsync(Guid id);
    Task<PurchaseInvoiceDto> CreatePurchaseInvoiceAsync(CreatePurchaseInvoiceRequest request);
    Task<PurchaseInvoiceDto> UpdatePurchaseInvoiceAsync(Guid id, CreatePurchaseInvoiceRequest request);
    Task<bool> ApproveZatcaInvoiceAsync(Guid id);

    // Purchase Request Operations
    Task<List<PurchaseRequestDto>> GetAllPurchaseRequestsAsync();
    Task<PurchaseRequestDto?> GetPurchaseRequestByIdAsync(Guid id);
    Task<PurchaseRequestDto> CreatePurchaseRequestAsync(CreatePurchaseRequestRequest request);
    Task<bool> ApprovePurchaseRequestAsync(Guid id, string approvedBy, string? notes);
    Task<bool> RejectPurchaseRequestAsync(Guid id, string approvedBy, string? notes);

    // Inventory Transfer Operations
    Task<List<InventoryTransferDto>> GetAllInventoryTransfersAsync();
    Task<InventoryTransferDto?> GetInventoryTransferByIdAsync(Guid id);
    Task<InventoryTransferDto> CreateInventoryTransferAsync(InventoryTransferDto request);
    Task<bool> ApproveInventoryTransferAsync(Guid id, string approvedBy);
    Task<bool> CompleteInventoryTransferAsync(Guid id, string performedBy);

    // Procurement Reports
    Task<List<ProcurementReportDto>> GetProcurementReportAsync(DateTime? startDate, DateTime? endDate);
    Task<List<ProcurementReportDto>> GetSupplierPerformanceReportAsync();
}
