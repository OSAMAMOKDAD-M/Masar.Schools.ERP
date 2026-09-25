using Masar.Schools.ERP.Domain.Entities.Canteen;

namespace Masar.Schools.ERP.Domain.DTOs;

/// <summary>
/// DTOs للموديول الجديد - الكانتين ونقاط البيع
/// </summary>

// ==================== Product Category DTOs ====================
public class ProductCategoryDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public int ProductCount { get; set; }
}

public class CreateProductCategoryDto
{
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}

public class UpdateProductCategoryDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}

// ==================== Product DTOs ====================
public class CanteenProductDto
{
    public Guid Id { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Cost { get; set; }
    public int StockQuantity { get; set; }
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsAvailable { get; set; }
    public bool AllowStudentBalance { get; set; }
    public bool AllowEmployeeBalance { get; set; }
    public int MaxOrderQuantity { get; set; }
    public int MinOrderQuantity { get; set; }
    public bool IsFavorite { get; set; }
    public int DisplayOrder { get; set; }
    public List<CanteenProductImageDto> Images { get; set; } = new();
    public List<CanteenProductVariantDto> Variants { get; set; } = new();
    public List<CanteenProductExtraDto> Extras { get; set; } = new();
}

public class CreateCanteenProductDto
{
    public Guid ProductCategoryId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Cost { get; set; }
    public int StockQuantity { get; set; }
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool AllowStudentBalance { get; set; } = true;
    public bool AllowEmployeeBalance { get; set; } = true;
    public int MaxOrderQuantity { get; set; } = 10;
    public int MinOrderQuantity { get; set; } = 1;
    public bool IsFavorite { get; set; } = false;
    public int DisplayOrder { get; set; }
    public List<CreateCanteenProductImageDto> Images { get; set; } = new();
    public List<CreateCanteenProductVariantDto> Variants { get; set; } = new();
    public List<Guid> ExtraIds { get; set; } = new();
}

public class UpdateCanteenProductDto
{
    public Guid Id { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Cost { get; set; }
    public int StockQuantity { get; set; }
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsAvailable { get; set; }
    public bool AllowStudentBalance { get; set; }
    public bool AllowEmployeeBalance { get; set; }
    public int MaxOrderQuantity { get; set; }
    public int MinOrderQuantity { get; set; }
    public bool IsFavorite { get; set; }
    public int DisplayOrder { get; set; }
    public List<CreateCanteenProductImageDto> Images { get; set; } = new();
    public List<CreateCanteenProductVariantDto> Variants { get; set; } = new();
    public List<Guid> ExtraIds { get; set; } = new();
}

public class CanteenProductImageDto
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public string? AltText { get; set; }
}

public class CreateCanteenProductImageDto
{
    public string ImagePath { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public string? AltText { get; set; }
}

public class CanteenProductVariantDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal ExtraPrice { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsAvailable { get; set; }
    public string? Description { get; set; }
}

public class CreateCanteenProductVariantDto
{
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal ExtraPrice { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? Description { get; set; }
}

public class CanteenProductExtraDto
{
    public Guid Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public int DisplayOrder { get; set; }
    public string? Description { get; set; }
}

// ==================== Cashier Shift DTOs ====================
public class CashierShiftDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal OpeningCardBalance { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal ActualCash { get; set; }
    public decimal CardPayments { get; set; }
    public decimal StudentBalancePayments { get; set; }
    public decimal EmployeeBalancePayments { get; set; }
    public decimal TotalSales { get; set; }
    public int OrderCount { get; set; }
    public int InvoiceCount { get; set; }
    public decimal ShortageOverage { get; set; }
    public bool IsClosed { get; set; }
    public string? Notes { get; set; }
    public string? ClosedBy { get; set; }
    public string? ShortageOverageReason { get; set; }
    public TimeSpan? Duration { get; set; }
}

public class CreateCashierShiftDto
{
    public Guid BranchId { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal OpeningBalance { get; set; } = 0;
    public decimal OpeningCardBalance { get; set; } = 0;
    public string? Notes { get; set; }
}

public class CloseCashierShiftDto
{
    public Guid Id { get; set; }
    public decimal ActualCash { get; set; }
    public decimal ActualCardBalance { get; set; } = 0;
    public string? ShortageOverageReason { get; set; }
    public string? Notes { get; set; }
}

public class ShiftReportDto
{
    public Guid ShiftId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal TotalSales { get; set; }
    public decimal CashSales { get; set; }
    public decimal CardSales { get; set; }
    public decimal StudentBalanceSales { get; set; }
    public decimal EmployeeBalanceSales { get; set; }
    public int OrderCount { get; set; }
    public int InvoiceCount { get; set; }
    public int DiningInOrders { get; set; }
    public int TakeawayOrders { get; set; }
    public int DeliveryOrders { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal ActualCash { get; set; }
    public decimal ShortageOverage { get; set; }
    public string? ShortageOverageReason { get; set; }
    public bool IsClosed { get; set; }
}

// ==================== Order DTOs ====================
public class CanteenOrderDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public Guid CashierShiftId { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus Status { get; set; }
    public Guid? CanteenTableId { get; set; }
    public string? TableName { get; set; }
    public Guid? DeliveryZoneId { get; set; }
    public string? DeliveryZoneName { get; set; }
    public Guid? PickUpPointId { get; set; }
    public string? PickUpPointName { get; set; }
    public Guid? DeliveryEmployeeId { get; set; }
    public string? DeliveryEmployeeName { get; set; }
    public decimal DeliveryFee { get; set; }
    public Guid? StudentId { get; set; }
    public string? StudentName { get; set; }
    public decimal StudentBalanceDeduction { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? DeliveryAddress { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }
    public int? PartySize { get; set; }
    public DateTime? EstimatedDeliveryTime { get; set; }
    public DateTime? ActualDeliveryTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CanteenOrderItemDto> Items { get; set; } = new();
    public CanteenPaymentDto? Payment { get; set; }
}

public class CreateCanteenOrderDto
{
    public Guid BranchId { get; set; }
    public OrderType OrderType { get; set; }
    public Guid? CanteenTableId { get; set; }
    public Guid? DeliveryZoneId { get; set; }
    public Guid? PickUpPointId { get; set; }
    public Guid? DeliveryEmployeeId { get; set; }
    public decimal DeliveryFee { get; set; } = 0;
    public Guid? StudentId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? Notes { get; set; }
    public int? PartySize { get; set; }
    public List<CreateCanteenOrderItemDto> Items { get; set; } = new();
    public PaymentMethod PaymentMethod { get; set; }
}

public class UpdateCanteenOrderDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public Guid? DeliveryEmployeeId { get; set; }
    public DateTime? ActualDeliveryTime { get; set; }
    public string? Notes { get; set; }
}

public class CanteenOrderItemDto
{
    public Guid Id { get; set; }
    public Guid CanteenProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? VariantId { get; set; }
    public string? VariantName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string? Notes { get; set; }
    public List<CanteenOrderItemExtraDto> Extras { get; set; } = new();
}

public class CreateCanteenOrderItemDto
{
    public Guid CanteenProductId { get; set; }
    public Guid? VariantId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Notes { get; set; }
    public List<CreateCanteenOrderItemExtraDto> Extras { get; set; } = new();
}

public class CanteenOrderItemExtraDto
{
    public Guid CanteenProductExtraId { get; set; }
    public string ExtraName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
}

public class CreateCanteenOrderItemExtraDto
{
    public Guid CanteenProductExtraId { get; set; }
    public int Quantity { get; set; } = 1;
}

// ==================== Payment DTOs ====================
public class CanteenPaymentDto
{
    public Guid Id { get; set; }
    public Guid CanteenOrderId { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string? TransactionRef { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? ProcessedBy { get; set; }
    public string? Notes { get; set; }
    public string? StudentNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
}

public class ProcessPaymentDto
{
    public Guid CanteenOrderId { get; set; }
    public PaymentMethod Method { get; set; }
    public string? StudentNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? TransactionRef { get; set; }
    public string? Notes { get; set; }
    public string? ProcessedBy { get; set; }
}

// ==================== Delivery Zone DTOs ====================
public class DeliveryZoneDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal DeliveryFee { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public int EstimatedDeliveryTime { get; set; }
    public int DisplayOrder { get; set; }
    public int PickUpPointCount { get; set; }
    public List<PickUpPointDto> PickUpPoints { get; set; } = new();
}

public class CreateDeliveryZoneDto
{
    public Guid BranchId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal DeliveryFee { get; set; } = 0;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal MinimumOrderAmount { get; set; } = 0;
    public int EstimatedDeliveryTime { get; set; } = 30;
    public int DisplayOrder { get; set; }
}

public class UpdateDeliveryZoneDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public decimal DeliveryFee { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public decimal MinimumOrderAmount { get; set; }
    public int EstimatedDeliveryTime { get; set; }
    public int DisplayOrder { get; set; }
}

// ==================== PickUp Point DTOs ====================
public class PickUpPointDto
{
    public Guid Id { get; set; }
    public Guid DeliveryZoneId { get; set; }
    public string DeliveryZoneName { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

public class CreatePickUpPointDto
{
    public Guid DeliveryZoneId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

public class UpdatePickUpPointDto
{
    public Guid Id { get; set; }
    public Guid DeliveryZoneId { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

// ==================== Table DTOs ====================
public class CanteenTableDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int TableNumber { get; set; }
    public string? Name { get; set; }
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsAvailable { get; set; }
    public TableStatus Status { get; set; }
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

public class CreateCanteenTableDto
{
    public Guid BranchId { get; set; }
    public int TableNumber { get; set; }
    public string? Name { get; set; }
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsAvailable { get; set; } = true;
    public TableStatus Status { get; set; } = TableStatus.Available;
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

public class UpdateCanteenTableDto
{
    public Guid Id { get; set; }
    public Guid BranchId { get; set; }
    public int TableNumber { get; set; }
    public string? Name { get; set; }
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsAvailable { get; set; }
    public TableStatus Status { get; set; }
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}

// ==================== Search and Filter DTOs ====================
public class CanteenSearchDto
{
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsFavorite { get; set; }
    public bool? AllowStudentBalance { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class CanteenOrderSearchDto
{
    public string? SearchTerm { get; set; }
    public Guid? BranchId { get; set; }
    public OrderType? OrderType { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? StudentId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ShiftSearchDto
{
    public Guid? BranchId { get; set; }
    public Guid? EmployeeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsClosed { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// ==================== Response DTOs ====================
public class CanteenResponseDto<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; } = new();
}

public class CanteenPagedResponseDto<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}