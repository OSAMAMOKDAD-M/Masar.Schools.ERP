using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities.Canteen;

namespace Masar.Schools.ERP.Application.Interfaces;

/// <summary>
/// واجهة خدمة الكانتين ونقاط البيع
/// </summary>
public interface ICanteenService
{
    // ==================== Product Categories ====================
    Task<CanteenResponseDto<ProductCategoryDto>> CreateProductCategoryAsync(CreateProductCategoryDto dto);
    Task<CanteenResponseDto<ProductCategoryDto>> UpdateProductCategoryAsync(UpdateProductCategoryDto dto);
    Task<CanteenResponseDto<bool>> DeleteProductCategoryAsync(Guid id);
    Task<CanteenResponseDto<ProductCategoryDto>> GetProductCategoryAsync(Guid id);
    Task<CanteenPagedResponseDto<ProductCategoryDto>> GetProductCategoriesAsync(CanteenSearchDto search);

    // ==================== Products ====================
    Task<CanteenResponseDto<CanteenProductDto>> CreateCanteenProductAsync(CreateCanteenProductDto dto);
    Task<CanteenResponseDto<CanteenProductDto>> UpdateCanteenProductAsync(UpdateCanteenProductDto dto);
    Task<CanteenResponseDto<bool>> DeleteCanteenProductAsync(Guid id);
    Task<CanteenResponseDto<CanteenProductDto>> GetCanteenProductAsync(Guid id);
    Task<CanteenPagedResponseDto<CanteenProductDto>> GetCanteenProductsAsync(CanteenSearchDto search);
    Task<CanteenResponseDto<bool>> UpdateStockAsync(Guid productId, int quantity);

    // ==================== Cashier Shifts ====================
    Task<CanteenResponseDto<CashierShiftDto>> OpenShiftAsync(CreateCashierShiftDto dto);
    Task<CanteenResponseDto<CashierShiftDto>> CloseShiftAsync(CloseCashierShiftDto dto);
    Task<CanteenResponseDto<CashierShiftDto>> GetCurrentShiftAsync(Guid branchId, Guid employeeId);
    Task<CanteenResponseDto<ShiftReportDto>> GetShiftReportAsync(Guid shiftId);
    Task<CanteenPagedResponseDto<CashierShiftDto>> GetShiftsAsync(ShiftSearchDto search);

    // ==================== Orders ====================
    Task<CanteenResponseDto<CanteenOrderDto>> CreateOrderAsync(CreateCanteenOrderDto dto);
    Task<CanteenResponseDto<CanteenOrderDto>> UpdateOrderAsync(UpdateCanteenOrderDto dto);
    Task<CanteenResponseDto<bool>> CancelOrderAsync(Guid id);
    Task<CanteenResponseDto<CanteenOrderDto>> GetOrderAsync(Guid id);
    Task<CanteenPagedResponseDto<CanteenOrderDto>> GetOrdersAsync(CanteenOrderSearchDto search);
    Task<CanteenResponseDto<CanteenPaymentDto>> ProcessPaymentAsync(ProcessPaymentDto dto);

    // ==================== Delivery Zones ====================
    Task<CanteenResponseDto<DeliveryZoneDto>> CreateDeliveryZoneAsync(CreateDeliveryZoneDto dto);
    Task<CanteenResponseDto<DeliveryZoneDto>> UpdateDeliveryZoneAsync(UpdateDeliveryZoneDto dto);
    Task<CanteenResponseDto<bool>> DeleteDeliveryZoneAsync(Guid id);
    Task<CanteenResponseDto<DeliveryZoneDto>> GetDeliveryZoneAsync(Guid id);
    Task<CanteenPagedResponseDto<DeliveryZoneDto>> GetDeliveryZonesAsync(CanteenSearchDto search);

    // ==================== PickUp Points ====================
    Task<CanteenResponseDto<PickUpPointDto>> CreatePickUpPointAsync(CreatePickUpPointDto dto);
    Task<CanteenResponseDto<PickUpPointDto>> UpdatePickUpPointAsync(UpdatePickUpPointDto dto);
    Task<CanteenResponseDto<bool>> DeletePickUpPointAsync(Guid id);
    Task<CanteenResponseDto<PickUpPointDto>> GetPickUpPointAsync(Guid id);
    Task<CanteenPagedResponseDto<PickUpPointDto>> GetPickUpPointsAsync(CanteenSearchDto search);

    // ==================== Tables ====================
    Task<CanteenResponseDto<CanteenTableDto>> CreateCanteenTableAsync(CreateCanteenTableDto dto);
    Task<CanteenResponseDto<CanteenTableDto>> UpdateCanteenTableAsync(UpdateCanteenTableDto dto);
    Task<CanteenResponseDto<bool>> DeleteCanteenTableAsync(Guid id);
    Task<CanteenResponseDto<CanteenTableDto>> GetCanteenTableAsync(Guid id);
    Task<CanteenPagedResponseDto<CanteenTableDto>> GetCanteenTablesAsync(CanteenSearchDto search);
}