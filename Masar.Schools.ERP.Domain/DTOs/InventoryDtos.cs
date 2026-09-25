namespace Masar.Schools.ERP.Domain.DTOs;

// ==================== Inventory DTOs ====================

/// <summary>
/// DTO للمخزن
/// </summary>
public class WarehouseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string WarehouseType { get; set; } = string.Empty;
    public string WarehouseTypeArabic { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Manager { get; set; }
    public string? ManagerPhone { get; set; }
    public decimal? Capacity { get; set; }
    public string? CapacityUnit { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لصنف المخزون
/// </summary>
public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public string UnitOfMeasureArabic { get; set; } = string.Empty;
    public decimal ReorderLevel { get; set; }
    public decimal MaxStockLevel { get; set; }
    public decimal ReorderQuantity { get; set; }
    public decimal AverageCost { get; set; }
    public decimal SellingPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsStockItem { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public Guid? PreferredSupplierId { get; set; }
    public string? PreferredSupplierName { get; set; }
    public string? Notes { get; set; }
    public decimal CurrentStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لحركة المخزون
/// </summary>
public class StockTransactionDto
{
    public Guid Id { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public string TransactionTypeArabic { get; set; } = string.Empty;
    public Guid InventoryItemId { get; set; }
    public string? ItemName { get; set; }
    public Guid WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public string? ReceiverName { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO لتعديل المخزون
/// </summary>
public class StockAdjustmentDto
{
    public Guid Id { get; set; }
    public string AdjustmentNumber { get; set; } = string.Empty;
    public string AdjustmentType { get; set; } = string.Empty;
    public string AdjustmentTypeArabic { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public DateTime AdjustmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string ReasonArabic { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TotalValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public Guid? ApprovedBy { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovalNotes { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ==================== Procurement DTOs ====================

/// <summary>
/// DTO للمورد
/// </summary>
public class SupplierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? CommercialRegistration { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string SupplierCategory { get; set; } = string.Empty;
    public string SupplierCategoryArabic { get; set; } = string.Empty;
    public string PaymentTerms { get; set; } = string.Empty;
    public decimal? CreditLimit { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لأمر الشراء
/// </summary>
public class PurchaseOrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public Guid WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentTerms { get; set; } = string.Empty;
    public string? DeliveryTerms { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public Guid? ApprovedBy { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لفاتورة الشراء
/// </summary>
public class PurchaseInvoiceDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string? TaxInvoiceNumber { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? PurchaseOrderNumber { get; set; }
    public Guid SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public bool IsZatcaApproved { get; set; }
    public string? ZatcaId { get; set; }
    public DateTime? ZatcaApprovedAt { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لطلب الشراء
/// </summary>
public class PurchaseRequestDto
{
    public Guid Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime RequiredBy { get; set; }
    public string RequestedBy { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string PurposeArabic { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public Guid? ApprovedBy { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovalNotes { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO لنقل المخزون
/// </summary>
public class InventoryTransferDto
{
    public Guid Id { get; set; }
    public string TransferNumber { get; set; } = string.Empty;
    public Guid FromWarehouseId { get; set; }
    public string? FromWarehouseName { get; set; }
    public Guid ToWarehouseId { get; set; }
    public string? ToWarehouseName { get; set; }
    public DateTime TransferDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusArabic { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string ReasonArabic { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid? ApprovedBy { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ==================== Request DTOs ====================

/// <summary>
/// DTO لإنشاء مخزن
/// </summary>
public class CreateWarehouseRequest
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid SchoolId { get; set; }
    public Guid? BranchId { get; set; }
    public string WarehouseType { get; set; } = "Main";
    public string WarehouseTypeArabic { get; set; } = "رئيسي";
    public string? Location { get; set; }
    public string? Manager { get; set; }
    public string? ManagerPhone { get; set; }
    public decimal? Capacity { get; set; }
    public string? CapacityUnit { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لإنشاء صنف مخزون
/// </summary>
public class CreateInventoryItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategoryArabic { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = "Unit";
    public string UnitOfMeasureArabic { get; set; } = "وحدة";
    public decimal ReorderLevel { get; set; }
    public decimal MaxStockLevel { get; set; }
    public decimal ReorderQuantity { get; set; }
    public decimal AverageCost { get; set; }
    public decimal SellingPrice { get; set; }
    public string Currency { get; set; } = "SAR";
    public bool IsStockItem { get; set; } = true;
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public Guid? PreferredSupplierId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لإنشاء حركة مخزون
/// </summary>
public class CreateStockTransactionRequest
{
    public string TransactionType { get; set; } = string.Empty;
    public string TransactionTypeArabic { get; set; } = string.Empty;
    public Guid InventoryItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string Currency { get; set; } = "SAR";
    public string? ReceiverName { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لإنشاء أمر شراء
/// </summary>
public class CreatePurchaseOrderRequest
{
    public Guid SupplierId { get; set; }
    public Guid WarehouseId { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string PaymentTerms { get; set; } = "Net 30";
    public string? DeliveryTerms { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public List<PurchaseOrderItemRequest> Items { get; set; } = new();
}

/// <summary>
/// DTO لبند أمر الشراء
/// </summary>
public class PurchaseOrderItemRequest
{
    public Guid InventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لإنشاء فاتورة شراء
/// </summary>
public class CreatePurchaseInvoiceRequest
{
    public Guid? PurchaseOrderId { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public string? TaxInvoiceNumber { get; set; }
    public decimal Discount { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public List<PurchaseInvoiceItemRequest> Items { get; set; } = new();
}

/// <summary>
/// DTO لبند فاتورة الشراء
/// </summary>
public class PurchaseInvoiceItemRequest
{
    public Guid InventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Tax { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لإنشاء طلب شراء
/// </summary>
public class CreatePurchaseRequestRequest
{
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid SchoolId { get; set; }
    public DateTime RequiredBy { get; set; }
    public string RequestedBy { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string PurposeArabic { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public string Currency { get; set; } = "SAR";
    public string? Notes { get; set; }
    public List<PurchaseRequestItemRequest> Items { get; set; } = new();
}

/// <summary>
/// DTO لبند طلب الشراء
/// </summary>
public class PurchaseRequestItemRequest
{
    public Guid InventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal EstimatedPrice { get; set; }
    public string? PreferredSupplier { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO لتقرير المخزون
/// </summary>
public class StockReportDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemNameArabic { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal MaxStockLevel { get; set; }
    public decimal AverageCost { get; set; }
    public decimal TotalValue { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsBelowReorderLevel { get; set; }
    public bool IsAboveMaxLevel { get; set; }
}

/// <summary>
/// DTO لتقرير المشتريات
/// </summary>
public class ProcurementReportDto
{
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Rating { get; set; }
}
