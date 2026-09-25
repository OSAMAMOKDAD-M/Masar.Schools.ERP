using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة المخازن
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(MasarDbContext context, ILogger<InventoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ==================== Warehouse Operations ====================

    public async Task<List<WarehouseDto>> GetAllWarehousesAsync()
    {
        var warehouses = await _context.Warehouses
            .Include(w => w.School)
            .Include(w => w.Branch)
            .ToListAsync();

        return warehouses.Select(w => new WarehouseDto
        {
            Id = w.Id,
            Name = w.Name,
            NameArabic = w.NameArabic,
            Code = w.Code,
            SchoolId = w.SchoolId,
            SchoolName = w.School?.Name,
            BranchId = w.BranchId,
            BranchName = w.Branch?.Name,
            WarehouseType = w.WarehouseType,
            WarehouseTypeArabic = w.WarehouseTypeArabic,
            Location = w.Location,
            Manager = w.Manager,
            ManagerPhone = w.ManagerPhone,
            Capacity = w.Capacity,
            CapacityUnit = w.CapacityUnit,
            IsActive = w.IsActive,
            Notes = w.Notes,
            CreatedAt = w.CreatedAt,
            UpdatedAt = w.UpdatedAt
        }).ToList();
    }

    public async Task<WarehouseDto?> GetWarehouseByIdAsync(Guid id)
    {
        var warehouse = await _context.Warehouses
            .Include(w => w.School)
            .Include(w => w.Branch)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (warehouse == null) return null;

        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            NameArabic = warehouse.NameArabic,
            Code = warehouse.Code,
            SchoolId = warehouse.SchoolId,
            SchoolName = warehouse.School?.Name,
            BranchId = warehouse.BranchId,
            BranchName = warehouse.Branch?.Name,
            WarehouseType = warehouse.WarehouseType,
            WarehouseTypeArabic = warehouse.WarehouseTypeArabic,
            Location = warehouse.Location,
            Manager = warehouse.Manager,
            ManagerPhone = warehouse.ManagerPhone,
            Capacity = warehouse.Capacity,
            CapacityUnit = warehouse.CapacityUnit,
            IsActive = warehouse.IsActive,
            Notes = warehouse.Notes,
            CreatedAt = warehouse.CreatedAt,
            UpdatedAt = warehouse.UpdatedAt
        };
    }

    public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request)
    {
        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NameArabic = request.NameArabic,
            Code = request.Code,
            SchoolId = request.SchoolId,
            BranchId = request.BranchId,
            WarehouseType = request.WarehouseType,
            WarehouseTypeArabic = request.WarehouseTypeArabic,
            Location = request.Location,
            Manager = request.Manager,
            ManagerPhone = request.ManagerPhone,
            Capacity = request.Capacity,
            CapacityUnit = request.CapacityUnit,
            IsActive = true,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.Warehouses.Add(warehouse);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Warehouse created: {warehouse.Name}");

        return await GetWarehouseByIdAsync(warehouse.Id);
    }

    public async Task<WarehouseDto> UpdateWarehouseAsync(Guid id, CreateWarehouseRequest request)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null)
            throw new Exception("Warehouse not found");

        warehouse.Name = request.Name;
        warehouse.NameArabic = request.NameArabic;
        warehouse.Code = request.Code;
        warehouse.BranchId = request.BranchId;
        warehouse.WarehouseType = request.WarehouseType;
        warehouse.WarehouseTypeArabic = request.WarehouseTypeArabic;
        warehouse.Location = request.Location;
        warehouse.Manager = request.Manager;
        warehouse.ManagerPhone = request.ManagerPhone;
        warehouse.Capacity = request.Capacity;
        warehouse.CapacityUnit = request.CapacityUnit;
        warehouse.Notes = request.Notes;
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Warehouse updated: {warehouse.Name}");

        return await GetWarehouseByIdAsync(id);
    }

    public async Task<bool> DeleteWarehouseAsync(Guid id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null) return false;

        warehouse.IsDeleted = true;
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Warehouse deleted: {warehouse.Name}");

        return true;
    }

    // ==================== Inventory Item Operations ====================

    public async Task<List<InventoryItemDto>> GetAllInventoryItemsAsync()
    {
        var items = await _context.InventoryItems
            .Include(i => i.PreferredSupplier)
            .ToListAsync();

        var result = new List<InventoryItemDto>();
        foreach (var item in items)
        {
            var currentStock = await GetCurrentStockAsync(item.Id);
            result.Add(new InventoryItemDto
            {
                Id = item.Id,
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
                IsActive = item.IsActive,
                IsStockItem = item.IsStockItem,
                Description = item.Description,
                DescriptionArabic = item.DescriptionArabic,
                PreferredSupplierId = item.PreferredSupplierId,
                PreferredSupplierName = item.PreferredSupplier?.Name,
                Notes = item.Notes,
                CurrentStock = currentStock,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            });
        }

        return result;
    }

    public async Task<InventoryItemDto?> GetInventoryItemByIdAsync(Guid id)
    {
        var item = await _context.InventoryItems
            .Include(i => i.PreferredSupplier)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null) return null;

        var currentStock = await GetCurrentStockAsync(item.Id);

        return new InventoryItemDto
        {
            Id = item.Id,
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
            IsActive = item.IsActive,
            IsStockItem = item.IsStockItem,
            Description = item.Description,
            DescriptionArabic = item.DescriptionArabic,
            PreferredSupplierId = item.PreferredSupplierId,
            PreferredSupplierName = item.PreferredSupplier?.Name,
            Notes = item.Notes,
            CurrentStock = currentStock,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }

    public async Task<InventoryItemDto> CreateInventoryItemAsync(CreateInventoryItemRequest request)
    {
        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NameArabic = request.NameArabic,
            SKU = request.SKU,
            Barcode = request.Barcode,
            Category = request.Category,
            CategoryArabic = request.CategoryArabic,
            UnitOfMeasure = request.UnitOfMeasure,
            UnitOfMeasureArabic = request.UnitOfMeasureArabic,
            ReorderLevel = request.ReorderLevel,
            MaxStockLevel = request.MaxStockLevel,
            ReorderQuantity = request.ReorderQuantity,
            AverageCost = request.AverageCost,
            SellingPrice = request.SellingPrice,
            Currency = request.Currency,
            IsActive = true,
            IsStockItem = request.IsStockItem,
            Description = request.Description,
            DescriptionArabic = request.DescriptionArabic,
            PreferredSupplierId = request.PreferredSupplierId,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory item created: {item.Name}");

        return await GetInventoryItemByIdAsync(item.Id);
    }

    public async Task<InventoryItemDto> UpdateInventoryItemAsync(Guid id, CreateInventoryItemRequest request)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            throw new Exception("Inventory item not found");

        item.Name = request.Name;
        item.NameArabic = request.NameArabic;
        item.SKU = request.SKU;
        item.Barcode = request.Barcode;
        item.Category = request.Category;
        item.CategoryArabic = request.CategoryArabic;
        item.UnitOfMeasure = request.UnitOfMeasure;
        item.UnitOfMeasureArabic = request.UnitOfMeasureArabic;
        item.ReorderLevel = request.ReorderLevel;
        item.MaxStockLevel = request.MaxStockLevel;
        item.ReorderQuantity = request.ReorderQuantity;
        item.AverageCost = request.AverageCost;
        item.SellingPrice = request.SellingPrice;
        item.IsStockItem = request.IsStockItem;
        item.Description = request.Description;
        item.DescriptionArabic = request.DescriptionArabic;
        item.PreferredSupplierId = request.PreferredSupplierId;
        item.Notes = request.Notes;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory item updated: {item.Name}");

        return await GetInventoryItemByIdAsync(id);
    }

    public async Task<bool> DeleteInventoryItemAsync(Guid id)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null) return false;

        item.IsDeleted = true;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Inventory item deleted: {item.Name}");

        return true;
    }

    // ==================== Stock Transaction Operations ====================

    public async Task<List<StockTransactionDto>> GetAllStockTransactionsAsync()
    {
        var transactions = await _context.StockTransactions
            .Include(t => t.InventoryItem)
            .Include(t => t.Warehouse)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();

        return transactions.Select(t => new StockTransactionDto
        {
            Id = t.Id,
            TransactionNumber = t.TransactionNumber,
            TransactionType = t.TransactionType,
            TransactionTypeArabic = t.TransactionTypeArabic,
            InventoryItemId = t.InventoryItemId,
            ItemName = t.InventoryItem?.Name,
            WarehouseId = t.WarehouseId,
            WarehouseName = t.Warehouse?.Name,
            Quantity = t.Quantity,
            UnitCost = t.UnitCost,
            TotalCost = t.TotalCost,
            Currency = t.Currency,
            BalanceBefore = t.BalanceBefore,
            BalanceAfter = t.BalanceAfter,
            ReceiverName = t.ReceiverName,
            TransactionDate = t.TransactionDate,
            Reference = t.Reference,
            Notes = t.Notes,
            PerformedBy = t.PerformedBy,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public async Task<List<StockTransactionDto>> GetStockTransactionsByWarehouseAsync(Guid warehouseId)
    {
        var transactions = await _context.StockTransactions
            .Include(t => t.InventoryItem)
            .Include(t => t.Warehouse)
            .Where(t => t.WarehouseId == warehouseId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();

        return transactions.Select(t => new StockTransactionDto
        {
            Id = t.Id,
            TransactionNumber = t.TransactionNumber,
            TransactionType = t.TransactionType,
            TransactionTypeArabic = t.TransactionTypeArabic,
            InventoryItemId = t.InventoryItemId,
            ItemName = t.InventoryItem?.Name,
            WarehouseId = t.WarehouseId,
            WarehouseName = t.Warehouse?.Name,
            Quantity = t.Quantity,
            UnitCost = t.UnitCost,
            TotalCost = t.TotalCost,
            Currency = t.Currency,
            BalanceBefore = t.BalanceBefore,
            BalanceAfter = t.BalanceAfter,
            ReceiverName = t.ReceiverName,
            TransactionDate = t.TransactionDate,
            Reference = t.Reference,
            Notes = t.Notes,
            PerformedBy = t.PerformedBy,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public async Task<List<StockTransactionDto>> GetStockTransactionsByItemAsync(Guid itemId)
    {
        var transactions = await _context.StockTransactions
            .Include(t => t.InventoryItem)
            .Include(t => t.Warehouse)
            .Where(t => t.InventoryItemId == itemId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();

        return transactions.Select(t => new StockTransactionDto
        {
            Id = t.Id,
            TransactionNumber = t.TransactionNumber,
            TransactionType = t.TransactionType,
            TransactionTypeArabic = t.TransactionTypeArabic,
            InventoryItemId = t.InventoryItemId,
            ItemName = t.InventoryItem?.Name,
            WarehouseId = t.WarehouseId,
            WarehouseName = t.Warehouse?.Name,
            Quantity = t.Quantity,
            UnitCost = t.UnitCost,
            TotalCost = t.TotalCost,
            Currency = t.Currency,
            BalanceBefore = t.BalanceBefore,
            BalanceAfter = t.BalanceAfter,
            ReceiverName = t.ReceiverName,
            TransactionDate = t.TransactionDate,
            Reference = t.Reference,
            Notes = t.Notes,
            PerformedBy = t.PerformedBy,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public async Task<StockTransactionDto> CreateStockTransactionAsync(CreateStockTransactionRequest request)
    {
        var currentBalance = await GetCurrentStockForWarehouseAsync(request.InventoryItemId, request.WarehouseId);
        
        var transaction = new StockTransaction
        {
            Id = Guid.NewGuid(),
            TransactionNumber = $"STX-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            TransactionType = request.TransactionType,
            TransactionTypeArabic = request.TransactionTypeArabic,
            InventoryItemId = request.InventoryItemId,
            WarehouseId = request.WarehouseId,
            Quantity = request.Quantity,
            UnitCost = request.UnitCost,
            TotalCost = request.Quantity * request.UnitCost,
            Currency = request.Currency,
            BalanceBefore = currentBalance,
            BalanceAfter = currentBalance + request.Quantity,
            ReceiverName = request.ReceiverName,
            TransactionDate = request.TransactionDate,
            Reference = request.Reference,
            Notes = request.Notes,
            PerformedBy = "System",
            CreatedAt = DateTime.UtcNow
        };

        _context.StockTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Stock transaction created: {transaction.TransactionNumber}");

        return await GetStockTransactionByIdAsync(transaction.Id);
    }

    // ==================== Stock Adjustment Operations ====================

    public async Task<List<StockAdjustmentDto>> GetAllStockAdjustmentsAsync()
    {
        var adjustments = await _context.StockAdjustments
            .Include(a => a.Warehouse)
            .OrderByDescending(a => a.AdjustmentDate)
            .ToListAsync();

        return adjustments.Select(a => new StockAdjustmentDto
        {
            Id = a.Id,
            AdjustmentNumber = a.AdjustmentNumber,
            AdjustmentType = a.AdjustmentType,
            AdjustmentTypeArabic = a.AdjustmentTypeArabic,
            WarehouseId = a.WarehouseId,
            WarehouseName = a.Warehouse?.Name,
            AdjustmentDate = a.AdjustmentDate,
            Reason = a.Reason,
            ReasonArabic = a.ReasonArabic,
            Description = a.Description,
            TotalValue = a.TotalValue,
            Status = a.Status,
            StatusArabic = a.StatusArabic,
            ApprovedBy = a.ApprovedBy,
            ApprovedByName = null, // Would need to fetch user name
            ApprovedAt = a.ApprovedAt,
            ApprovalNotes = a.ApprovalNotes,
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<StockAdjustmentDto?> GetStockAdjustmentByIdAsync(Guid id)
    {
        var adjustment = await _context.StockAdjustments
            .Include(a => a.Warehouse)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null) return null;

        return new StockAdjustmentDto
        {
            Id = adjustment.Id,
            AdjustmentNumber = adjustment.AdjustmentNumber,
            AdjustmentType = adjustment.AdjustmentType,
            AdjustmentTypeArabic = adjustment.AdjustmentTypeArabic,
            WarehouseId = adjustment.WarehouseId,
            WarehouseName = adjustment.Warehouse?.Name,
            AdjustmentDate = adjustment.AdjustmentDate,
            Reason = adjustment.Reason,
            ReasonArabic = adjustment.ReasonArabic,
            Description = adjustment.Description,
            TotalValue = adjustment.TotalValue,
            Status = adjustment.Status,
            StatusArabic = adjustment.StatusArabic,
            ApprovedBy = adjustment.ApprovedBy,
            ApprovedByName = null,
            ApprovedAt = adjustment.ApprovedAt,
            ApprovalNotes = adjustment.ApprovalNotes,
            CreatedAt = adjustment.CreatedAt
        };
    }

    public async Task<StockAdjustmentDto> CreateStockAdjustmentAsync(StockAdjustmentDto request)
    {
        var adjustment = new StockAdjustment
        {
            Id = Guid.NewGuid(),
            AdjustmentNumber = $"SAD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
            AdjustmentType = request.AdjustmentType,
            AdjustmentTypeArabic = request.AdjustmentTypeArabic,
            WarehouseId = request.WarehouseId,
            AdjustmentDate = request.AdjustmentDate,
            Reason = request.Reason,
            ReasonArabic = request.ReasonArabic,
            Description = request.Description,
            TotalValue = request.TotalValue,
            Status = "Pending",
            StatusArabic = "قيد الانتظار",
            CreatedAt = DateTime.UtcNow
        };

        _context.StockAdjustments.Add(adjustment);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Stock adjustment created: {adjustment.AdjustmentNumber}");

        return await GetStockAdjustmentByIdAsync(adjustment.Id);
    }

    public async Task<bool> ApproveStockAdjustmentAsync(Guid id, string approvedBy, string? notes)
    {
        var adjustment = await _context.StockAdjustments.FindAsync(id);
        if (adjustment == null) return false;

        adjustment.Status = "Approved";
        adjustment.StatusArabic = "معتم";
        adjustment.ApprovedBy = Guid.Parse(approvedBy);
        adjustment.ApprovedAt = DateTime.UtcNow;
        adjustment.ApprovalNotes = notes;
        adjustment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Stock adjustment approved: {adjustment.AdjustmentNumber}");

        return true;
    }

    // ==================== Stock Reports ====================

    public async Task<List<StockReportDto>> GetStockReportAsync(Guid? warehouseId = null)
    {
        var items = await _context.InventoryItems
            .Where(i => i.IsActive && i.IsStockItem)
            .ToListAsync();

        var report = new List<StockReportDto>();
        foreach (var item in items)
        {
            var currentStock = warehouseId.HasValue 
                ? await GetCurrentStockForWarehouseAsync(item.Id, warehouseId.Value)
                : await GetCurrentStockAsync(item.Id);

            report.Add(new StockReportDto
            {
                ItemId = item.Id,
                ItemName = item.Name,
                ItemNameArabic = item.NameArabic,
                SKU = item.SKU,
                Category = item.Category,
                CurrentStock = currentStock,
                ReorderLevel = item.ReorderLevel,
                MaxStockLevel = item.MaxStockLevel,
                AverageCost = item.AverageCost,
                TotalValue = currentStock * item.AverageCost,
                Currency = item.Currency,
                IsBelowReorderLevel = currentStock <= item.ReorderLevel,
                IsAboveMaxLevel = currentStock >= item.MaxStockLevel
            });
        }

        return report;
    }

    public async Task<List<StockReportDto>> GetLowStockItemsAsync()
    {
        var allItems = await GetStockReportAsync();
        return allItems.Where(i => i.IsBelowReorderLevel).ToList();
    }

    public async Task<List<StockReportDto>> GetOverStockItemsAsync()
    {
        var allItems = await GetStockReportAsync();
        return allItems.Where(i => i.IsAboveMaxLevel).ToList();
    }

    // ==================== Helper Methods ====================

    private async Task<decimal> GetCurrentStockAsync(Guid itemId)
    {
        var transactions = await _context.StockTransactions
            .Where(t => t.InventoryItemId == itemId)
            .ToListAsync();

        return transactions.Sum(t => t.Quantity);
    }

    private async Task<decimal> GetCurrentStockForWarehouseAsync(Guid itemId, Guid warehouseId)
    {
        var transactions = await _context.StockTransactions
            .Where(t => t.InventoryItemId == itemId && t.WarehouseId == warehouseId)
            .ToListAsync();

        return transactions.Sum(t => t.Quantity);
    }

    private async Task<StockTransactionDto> GetStockTransactionByIdAsync(Guid id)
    {
        var transaction = await _context.StockTransactions
            .Include(t => t.InventoryItem)
            .Include(t => t.Warehouse)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null) return null;

        return new StockTransactionDto
        {
            Id = transaction.Id,
            TransactionNumber = transaction.TransactionNumber,
            TransactionType = transaction.TransactionType,
            TransactionTypeArabic = transaction.TransactionTypeArabic,
            InventoryItemId = transaction.InventoryItemId,
            ItemName = transaction.InventoryItem?.Name,
            WarehouseId = transaction.WarehouseId,
            WarehouseName = transaction.Warehouse?.Name,
            Quantity = transaction.Quantity,
            UnitCost = transaction.UnitCost,
            TotalCost = transaction.TotalCost,
            Currency = transaction.Currency,
            BalanceBefore = transaction.BalanceBefore,
            BalanceAfter = transaction.BalanceAfter,
            ReceiverName = transaction.ReceiverName,
            TransactionDate = transaction.TransactionDate,
            Reference = transaction.Reference,
            Notes = transaction.Notes,
            PerformedBy = transaction.PerformedBy,
            CreatedAt = transaction.CreatedAt
        };
    }
}
