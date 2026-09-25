using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities.Canteen;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Application.Services;

/// <summary>
/// خدمة الكانتين ونقاط البيع
/// </summary>
public class CanteenService : ICanteenService
{
    private readonly MasarDbContext _context;

    public CanteenService(MasarDbContext context)
    {
        _context = context;
    }

    // ==================== Product Categories ====================
    public async Task<CanteenResponseDto<ProductCategoryDto>> CreateProductCategoryAsync(CreateProductCategoryDto dto)
    {
        var category = new ProductCategory
        {
            Id = Guid.NewGuid(),
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.ProductCategories.Add(category);
        await _context.SaveChangesAsync();

        var categoryDto = new ProductCategoryDto
        {
            Id = category.Id,
            NameAr = category.NameAr,
            NameEn = category.NameEn,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive,
            Description = category.Description,
            ProductCount = 0
        };

        return new CanteenResponseDto<ProductCategoryDto>
        {
            Success = true,
            Message = "تم إنشاء التصنيف بنجاح",
            Data = categoryDto
        };
    }

    public async Task<CanteenResponseDto<ProductCategoryDto>> UpdateProductCategoryAsync(UpdateProductCategoryDto dto)
    {
        var category = await _context.ProductCategories.FindAsync(dto.Id);
        if (category == null)
        {
            return new CanteenResponseDto<ProductCategoryDto>
            {
                Success = false,
                Message = "التصنيف غير موجود"
            };
        }

        category.NameAr = dto.NameAr;
        category.NameEn = dto.NameEn;
        category.DisplayOrder = dto.DisplayOrder;
        category.IsActive = dto.IsActive;
        category.Description = dto.Description;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var categoryDto = new ProductCategoryDto
        {
            Id = category.Id,
            NameAr = category.NameAr,
            NameEn = category.NameEn,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive,
            Description = category.Description,
            ProductCount = await _context.CanteenProducts.CountAsync(p => p.ProductCategoryId == category.Id)
        };

        return new CanteenResponseDto<ProductCategoryDto>
        {
            Success = true,
            Message = "تم تحديث التصنيف بنجاح",
            Data = categoryDto
        };
    }

    public async Task<CanteenResponseDto<bool>> DeleteProductCategoryAsync(Guid id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "التصنيف غير موجود"
            };
        }

        _context.ProductCategories.Remove(category);
        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف التصنيف بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<ProductCategoryDto>> GetProductCategoryAsync(Guid id)
    {
        var category = await _context.ProductCategories.FindAsync(id);
        if (category == null)
        {
            return new CanteenResponseDto<ProductCategoryDto>
            {
                Success = false,
                Message = "التصنيف غير موجود"
            };
        }

        var categoryDto = new ProductCategoryDto
        {
            Id = category.Id,
            NameAr = category.NameAr,
            NameEn = category.NameEn,
            DisplayOrder = category.DisplayOrder,
            IsActive = category.IsActive,
            Description = category.Description,
            ProductCount = await _context.CanteenProducts.CountAsync(p => p.ProductCategoryId == category.Id)
        };

        return new CanteenResponseDto<ProductCategoryDto>
        {
            Success = true,
            Data = categoryDto
        };
    }

    public async Task<CanteenPagedResponseDto<ProductCategoryDto>> GetProductCategoriesAsync(CanteenSearchDto search)
    {
        var query = _context.ProductCategories.AsQueryable();

        if (!string.IsNullOrEmpty(search.SearchTerm))
        {
            query = query.Where(c => c.NameAr.Contains(search.SearchTerm) || c.NameEn != null && c.NameEn.Contains(search.SearchTerm));
        }

        var totalCount = await query.CountAsync();
        var categories = await query
            .OrderBy(c => c.DisplayOrder)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(c => new ProductCategoryDto
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                DisplayOrder = c.DisplayOrder,
                IsActive = c.IsActive,
                Description = c.Description,
                ProductCount = _context.CanteenProducts.Count(p => p.ProductCategoryId == c.Id)
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<ProductCategoryDto>
        {
            Success = true,
            Data = categories,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    // ==================== Products ====================
    public async Task<CanteenResponseDto<CanteenProductDto>> CreateCanteenProductAsync(CreateCanteenProductDto dto)
    {
        var product = new CanteenProduct
        {
            Id = Guid.NewGuid(),
            ProductCategoryId = dto.ProductCategoryId,
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            DescriptionAr = dto.DescriptionAr,
            BasePrice = dto.BasePrice,
            Cost = dto.Cost,
            StockQuantity = dto.StockQuantity,
            Barcode = dto.Barcode,
            ImagePath = dto.ImagePath,
            IsAvailable = dto.IsAvailable,
            AllowStudentBalance = dto.AllowStudentBalance,
            AllowEmployeeBalance = dto.AllowEmployeeBalance,
            MaxOrderQuantity = dto.MaxOrderQuantity,
            MinOrderQuantity = dto.MinOrderQuantity,
            IsFavorite = dto.IsFavorite,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.CanteenProducts.Add(product);
        await _context.SaveChangesAsync();

        var productDto = new CanteenProductDto
        {
            Id = product.Id,
            ProductCategoryId = product.ProductCategoryId,
            NameAr = product.NameAr,
            NameEn = product.NameEn,
            DescriptionAr = product.DescriptionAr,
            BasePrice = product.BasePrice,
            Cost = product.Cost,
            StockQuantity = product.StockQuantity,
            Barcode = product.Barcode,
            ImagePath = product.ImagePath,
            IsAvailable = product.IsAvailable,
            AllowStudentBalance = product.AllowStudentBalance,
            AllowEmployeeBalance = product.AllowEmployeeBalance,
            MaxOrderQuantity = product.MaxOrderQuantity,
            MinOrderQuantity = product.MinOrderQuantity,
            IsFavorite = product.IsFavorite,
            DisplayOrder = product.DisplayOrder
        };

        return new CanteenResponseDto<CanteenProductDto>
        {
            Success = true,
            Message = "تم إنشاء المنتج بنجاح",
            Data = productDto
        };
    }

    public async Task<CanteenResponseDto<CanteenProductDto>> UpdateCanteenProductAsync(UpdateCanteenProductDto dto)
    {
        var product = await _context.CanteenProducts.FindAsync(dto.Id);
        if (product == null)
        {
            return new CanteenResponseDto<CanteenProductDto>
            {
                Success = false,
                Message = "المنتج غير موجود"
            };
        }

        product.ProductCategoryId = dto.ProductCategoryId;
        product.NameAr = dto.NameAr;
        product.NameEn = dto.NameEn;
        product.DescriptionAr = dto.DescriptionAr;
        product.BasePrice = dto.BasePrice;
        product.Cost = dto.Cost;
        product.StockQuantity = dto.StockQuantity;
        product.Barcode = dto.Barcode;
        product.ImagePath = dto.ImagePath;
        product.IsAvailable = dto.IsAvailable;
        product.AllowStudentBalance = dto.AllowStudentBalance;
        product.AllowEmployeeBalance = dto.AllowEmployeeBalance;
        product.MaxOrderQuantity = dto.MaxOrderQuantity;
        product.MinOrderQuantity = dto.MinOrderQuantity;
        product.IsFavorite = dto.IsFavorite;
        product.DisplayOrder = dto.DisplayOrder;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var productDto = new CanteenProductDto
        {
            Id = product.Id,
            ProductCategoryId = product.ProductCategoryId,
            NameAr = product.NameAr,
            NameEn = product.NameEn,
            DescriptionAr = product.DescriptionAr,
            BasePrice = product.BasePrice,
            Cost = product.Cost,
            StockQuantity = product.StockQuantity,
            Barcode = product.Barcode,
            ImagePath = product.ImagePath,
            IsAvailable = product.IsAvailable,
            AllowStudentBalance = product.AllowStudentBalance,
            AllowEmployeeBalance = product.AllowEmployeeBalance,
            MaxOrderQuantity = product.MaxOrderQuantity,
            MinOrderQuantity = product.MinOrderQuantity,
            IsFavorite = product.IsFavorite,
            DisplayOrder = product.DisplayOrder
        };

        return new CanteenResponseDto<CanteenProductDto>
        {
            Success = true,
            Message = "تم تحديث المنتج بنجاح",
            Data = productDto
        };
    }

    public async Task<CanteenResponseDto<bool>> DeleteCanteenProductAsync(Guid id)
    {
        var product = await _context.CanteenProducts.FindAsync(id);
        if (product == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "المنتج غير موجود"
            };
        }

        _context.CanteenProducts.Remove(product);
        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف المنتج بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<CanteenProductDto>> GetCanteenProductAsync(Guid id)
    {
        var product = await _context.CanteenProducts.FindAsync(id);
        if (product == null)
        {
            return new CanteenResponseDto<CanteenProductDto>
            {
                Success = false,
                Message = "المنتج غير موجود"
            };
        }

        var productDto = new CanteenProductDto
        {
            Id = product.Id,
            ProductCategoryId = product.ProductCategoryId,
            NameAr = product.NameAr,
            NameEn = product.NameEn,
            DescriptionAr = product.DescriptionAr,
            BasePrice = product.BasePrice,
            Cost = product.Cost,
            StockQuantity = product.StockQuantity,
            Barcode = product.Barcode,
            ImagePath = product.ImagePath,
            IsAvailable = product.IsAvailable,
            AllowStudentBalance = product.AllowStudentBalance,
            AllowEmployeeBalance = product.AllowEmployeeBalance,
            MaxOrderQuantity = product.MaxOrderQuantity,
            MinOrderQuantity = product.MinOrderQuantity,
            IsFavorite = product.IsFavorite,
            DisplayOrder = product.DisplayOrder
        };

        return new CanteenResponseDto<CanteenProductDto>
        {
            Success = true,
            Data = productDto
        };
    }

    public async Task<CanteenPagedResponseDto<CanteenProductDto>> GetCanteenProductsAsync(CanteenSearchDto search)
    {
        var query = _context.CanteenProducts.AsQueryable();

        if (!string.IsNullOrEmpty(search.SearchTerm))
        {
            query = query.Where(p => p.NameAr.Contains(search.SearchTerm) || p.NameEn != null && p.NameEn.Contains(search.SearchTerm));
        }

        if (search.CategoryId.HasValue)
        {
            query = query.Where(p => p.ProductCategoryId == search.CategoryId.Value);
        }

        if (search.IsAvailable.HasValue)
        {
            query = query.Where(p => p.IsAvailable == search.IsAvailable.Value);
        }

        if (search.IsFavorite.HasValue)
        {
            query = query.Where(p => p.IsFavorite == search.IsFavorite.Value);
        }

        var totalCount = await query.CountAsync();
        var products = await query
            .OrderBy(p => p.DisplayOrder)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(p => new CanteenProductDto
            {
                Id = p.Id,
                ProductCategoryId = p.ProductCategoryId,
                NameAr = p.NameAr,
                NameEn = p.NameEn,
                DescriptionAr = p.DescriptionAr,
                BasePrice = p.BasePrice,
                Cost = p.Cost,
                StockQuantity = p.StockQuantity,
                Barcode = p.Barcode,
                ImagePath = p.ImagePath,
                IsAvailable = p.IsAvailable,
                AllowStudentBalance = p.AllowStudentBalance,
                AllowEmployeeBalance = p.AllowEmployeeBalance,
                MaxOrderQuantity = p.MaxOrderQuantity,
                MinOrderQuantity = p.MinOrderQuantity,
                IsFavorite = p.IsFavorite,
                DisplayOrder = p.DisplayOrder
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<CanteenProductDto>
        {
            Success = true,
            Data = products,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    public async Task<CanteenResponseDto<bool>> UpdateStockAsync(Guid productId, int quantity)
    {
        var product = await _context.CanteenProducts.FindAsync(productId);
        if (product == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "المنتج غير موجود"
            };
        }

        product.StockQuantity = quantity;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم تحديث المخزون بنجاح",
            Data = true
        };
    }

    // ==================== Cashier Shifts ====================
    public async Task<CanteenResponseDto<CashierShiftDto>> OpenShiftAsync(CreateCashierShiftDto dto)
    {
        var existingShift = await _context.CashierShifts
            .FirstOrDefaultAsync(s => s.BranchId == dto.BranchId && s.EmployeeId == dto.EmployeeId && !s.IsClosed);

        if (existingShift != null)
        {
            return new CanteenResponseDto<CashierShiftDto>
            {
                Success = false,
                Message = "توجد وردية مفتوحة بالفعل لهذا الموظف في هذا الفرع"
            };
        }

        var shift = new CashierShift
        {
            Id = Guid.NewGuid(),
            BranchId = dto.BranchId,
            EmployeeId = dto.EmployeeId,
            StartTime = DateTime.UtcNow,
            OpeningBalance = dto.OpeningBalance,
            OpeningCardBalance = dto.OpeningCardBalance,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.CashierShifts.Add(shift);
        await _context.SaveChangesAsync();

        var shiftDto = new CashierShiftDto
        {
            Id = shift.Id,
            BranchId = shift.BranchId,
            EmployeeId = shift.EmployeeId,
            StartTime = shift.StartTime,
            OpeningBalance = shift.OpeningBalance,
            OpeningCardBalance = shift.OpeningCardBalance,
            IsClosed = shift.IsClosed,
            Notes = shift.Notes
        };

        return new CanteenResponseDto<CashierShiftDto>
        {
            Success = true,
            Message = "تم فتح الوردية بنجاح",
            Data = shiftDto
        };
    }

    public async Task<CanteenResponseDto<CashierShiftDto>> CloseShiftAsync(CloseCashierShiftDto dto)
    {
        var shift = await _context.CashierShifts.FindAsync(dto.Id);
        if (shift == null)
        {
            return new CanteenResponseDto<CashierShiftDto>
            {
                Success = false,
                Message = "الوردية غير موجودة"
            };
        }

        if (shift.IsClosed)
        {
            return new CanteenResponseDto<CashierShiftDto>
            {
                Success = false,
                Message = "الوردية مغلقة بالفعل"
            };
        }

        shift.EndTime = DateTime.UtcNow;
        shift.ActualCash = dto.ActualCash;
        shift.ShortageOverage = shift.ExpectedCash - dto.ActualCash;
        shift.ShortageOverageReason = dto.ShortageOverageReason;
        shift.Notes = dto.Notes;
        shift.IsClosed = true;
        shift.ClosedBy = "System";
        shift.UpdatedAt = DateTime.UtcNow;
        shift.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var shiftDto = new CashierShiftDto
        {
            Id = shift.Id,
            BranchId = shift.BranchId,
            EmployeeId = shift.EmployeeId,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            OpeningBalance = shift.OpeningBalance,
            ExpectedCash = shift.ExpectedCash,
            ActualCash = shift.ActualCash,
            TotalSales = shift.TotalSales,
            OrderCount = shift.OrderCount,
            InvoiceCount = shift.InvoiceCount,
            ShortageOverage = shift.ShortageOverage,
            IsClosed = shift.IsClosed,
            Notes = shift.Notes,
            ClosedBy = shift.ClosedBy,
            ShortageOverageReason = shift.ShortageOverageReason,
            Duration = shift.EndTime - shift.StartTime
        };

        return new CanteenResponseDto<CashierShiftDto>
        {
            Success = true,
            Message = "تم إغلاق الوردية بنجاح",
            Data = shiftDto
        };
    }

    public async Task<CanteenResponseDto<CashierShiftDto>> GetCurrentShiftAsync(Guid branchId, Guid employeeId)
    {
        var shift = await _context.CashierShifts
            .FirstOrDefaultAsync(s => s.BranchId == branchId && s.EmployeeId == employeeId && !s.IsClosed);

        if (shift == null)
        {
            return new CanteenResponseDto<CashierShiftDto>
            {
                Success = false,
                Message = "لا توجد وردية مفتوحة"
            };
        }

        var shiftDto = new CashierShiftDto
        {
            Id = shift.Id,
            BranchId = shift.BranchId,
            EmployeeId = shift.EmployeeId,
            StartTime = shift.StartTime,
            OpeningBalance = shift.OpeningBalance,
            TotalSales = shift.TotalSales,
            OrderCount = shift.OrderCount,
            InvoiceCount = shift.InvoiceCount,
            IsClosed = shift.IsClosed,
            Duration = DateTime.UtcNow - shift.StartTime
        };

        return new CanteenResponseDto<CashierShiftDto>
        {
            Success = true,
            Data = shiftDto
        };
    }

    public async Task<CanteenResponseDto<ShiftReportDto>> GetShiftReportAsync(Guid shiftId)
    {
        var shift = await _context.CashierShifts.FindAsync(shiftId);
        if (shift == null)
        {
            return new CanteenResponseDto<ShiftReportDto>
            {
                Success = false,
                Message = "الوردية غير موجودة"
            };
        }

        var orders = await _context.CanteenOrders
            .Where(o => o.CashierShiftId == shiftId)
            .ToListAsync();

        var diningInOrders = orders.Count(o => o.OrderType == OrderType.DiningIn);
        var takeawayOrders = orders.Count(o => o.OrderType == OrderType.Takeaway);
        var deliveryOrders = orders.Count(o => o.OrderType == OrderType.Delivery);

        var cashSales = orders.Where(o => o.Payment != null && o.Payment.Method == PaymentMethod.Cash).Sum(o => o.Total);
        var studentBalanceSales = orders.Where(o => o.Payment != null && o.Payment.Method == PaymentMethod.StudentBalance).Sum(o => o.Total);
        var employeeBalanceSales = orders.Where(o => o.Payment != null && o.Payment.Method == PaymentMethod.EmployeeBalance).Sum(o => o.Total);
        var cardSales = orders.Where(o => o.Payment != null && o.Payment.Method == PaymentMethod.Card).Sum(o => o.Total);

        var report = new ShiftReportDto
        {
            ShiftId = shift.Id,
            EmployeeName = shift.Employee.FullNameArabic,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            OpeningBalance = shift.OpeningBalance,
            TotalSales = shift.TotalSales,
            CashSales = cashSales,
            CardSales = cardSales,
            StudentBalanceSales = studentBalanceSales,
            EmployeeBalanceSales = employeeBalanceSales,
            OrderCount = shift.OrderCount,
            InvoiceCount = shift.InvoiceCount,
            DiningInOrders = diningInOrders,
            TakeawayOrders = takeawayOrders,
            DeliveryOrders = deliveryOrders,
            ExpectedCash = shift.ExpectedCash,
            ActualCash = shift.ActualCash,
            ShortageOverage = shift.ShortageOverage,
            ShortageOverageReason = shift.ShortageOverageReason,
            IsClosed = shift.IsClosed
        };

        return new CanteenResponseDto<ShiftReportDto>
        {
            Success = true,
            Data = report
        };
    }

    public async Task<CanteenPagedResponseDto<CashierShiftDto>> GetShiftsAsync(ShiftSearchDto search)
    {
        var query = _context.CashierShifts.AsQueryable();

        if (search.BranchId.HasValue)
        {
            query = query.Where(s => s.BranchId == search.BranchId.Value);
        }

        if (search.EmployeeId.HasValue)
        {
            query = query.Where(s => s.EmployeeId == search.EmployeeId.Value);
        }

        if (search.IsClosed.HasValue)
        {
            query = query.Where(s => s.IsClosed == search.IsClosed.Value);
        }

        var totalCount = await query.CountAsync();
        var shifts = await query
            .OrderByDescending(s => s.StartTime)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(s => new CashierShiftDto
            {
                Id = s.Id,
                BranchId = s.BranchId,
                EmployeeId = s.EmployeeId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                OpeningBalance = s.OpeningBalance,
                TotalSales = s.TotalSales,
                OrderCount = s.OrderCount,
                InvoiceCount = s.InvoiceCount,
                ShortageOverage = s.ShortageOverage,
                IsClosed = s.IsClosed,
                Notes = s.Notes,
                ClosedBy = s.ClosedBy,
                Duration = s.EndTime - s.StartTime
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<CashierShiftDto>
        {
            Success = true,
            Data = shifts,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    // ==================== Orders ====================
    public async Task<CanteenResponseDto<CanteenOrderDto>> CreateOrderAsync(CreateCanteenOrderDto dto)
    {
        var currentShift = await _context.CashierShifts
            .FirstOrDefaultAsync(s => s.BranchId == dto.BranchId && !s.IsClosed);

        if (currentShift == null)
        {
            return new CanteenResponseDto<CanteenOrderDto>
            {
                Success = false,
                Message = "لا توجد وردية مفتوحة، يرجى فتح وردية أولاً"
            };
        }

        var order = new CanteenOrder
        {
            Id = Guid.NewGuid(),
            BranchId = dto.BranchId,
            CashierShiftId = currentShift.Id,
            OrderType = dto.OrderType,
            CanteenTableId = dto.CanteenTableId,
            DeliveryZoneId = dto.DeliveryZoneId,
            PickUpPointId = dto.PickUpPointId,
            DeliveryEmployeeId = dto.DeliveryEmployeeId,
            DeliveryFee = dto.DeliveryFee,
            StudentId = dto.StudentId,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            DeliveryAddress = dto.DeliveryAddress,
            Notes = dto.Notes,
            PartySize = dto.PartySize,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        // Calculate totals
        decimal subtotal = 0;
        foreach (var itemDto in dto.Items)
        {
            var product = await _context.CanteenProducts.FindAsync(itemDto.CanteenProductId);
            if (product == null)
            {
                return new CanteenResponseDto<CanteenOrderDto>
                {
                    Success = false,
                    Message = $"المنتج {itemDto.CanteenProductId} غير موجود"
                };
            }

            var orderItem = new CanteenOrderItem
            {
                Id = Guid.NewGuid(),
                CanteenProductId = itemDto.CanteenProductId,
                VariantId = itemDto.VariantId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.BasePrice,
                Notes = itemDto.Notes
            };

            // Add extras
            foreach (var extraDto in itemDto.Extras)
            {
                var extra = await _context.CanteenProductExtras.FindAsync(extraDto.CanteenProductExtraId);
                if (extra != null)
                {
                    orderItem.Extras.Add(new CanteenOrderItemExtra
                    {
                        CanteenProductExtraId = extraDto.CanteenProductExtraId,
                        Price = extra.Price,
                        Quantity = extraDto.Quantity
                    });
                }
            }

            orderItem.LineTotal = orderItem.UnitPrice * orderItem.Quantity + orderItem.Extras.Sum(e => e.Price * e.Quantity);
            subtotal += orderItem.LineTotal;
            order.Items.Add(orderItem);
        }

        order.Subtotal = subtotal;
        order.DiscountAmount = 0; // Can be calculated based on discounts
        order.TaxAmount = subtotal * 0.15m; // 15% tax
        order.Total = order.Subtotal - order.DiscountAmount + order.TaxAmount + order.DeliveryFee;

        _context.CanteenOrders.Add(order);
        await _context.SaveChangesAsync();

        // Update shift statistics
        currentShift.TotalSales += order.Total;
        currentShift.OrderCount += 1;
        currentShift.InvoiceCount += 1;
        currentShift.ExpectedCash = currentShift.OpeningBalance + currentShift.TotalSales;
        await _context.SaveChangesAsync();

        var orderDto = new CanteenOrderDto
        {
            Id = order.Id,
            BranchId = order.BranchId,
            CashierShiftId = order.CashierShiftId,
            OrderType = order.OrderType,
            Status = order.Status,
            CanteenTableId = order.CanteenTableId,
            DeliveryZoneId = order.DeliveryZoneId,
            PickUpPointId = order.PickUpPointId,
            DeliveryEmployeeId = order.DeliveryEmployeeId,
            DeliveryFee = order.DeliveryFee,
            StudentId = order.StudentId,
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            DeliveryAddress = order.DeliveryAddress,
            Subtotal = order.Subtotal,
            DiscountAmount = order.DiscountAmount,
            TaxAmount = order.TaxAmount,
            Total = order.Total,
            Notes = order.Notes,
            PartySize = order.PartySize,
            CreatedAt = order.CreatedAt
        };

        return new CanteenResponseDto<CanteenOrderDto>
        {
            Success = true,
            Message = "تم إنشاء الطلب بنجاح",
            Data = orderDto
        };
    }

    public async Task<CanteenResponseDto<CanteenOrderDto>> UpdateOrderAsync(UpdateCanteenOrderDto dto)
    {
        var order = await _context.CanteenOrders.FindAsync(dto.Id);
        if (order == null)
        {
            return new CanteenResponseDto<CanteenOrderDto>
            {
                Success = false,
                Message = "الطلب غير موجود"
            };
        }

        order.Status = dto.Status;
        order.DeliveryEmployeeId = dto.DeliveryEmployeeId;
        order.ActualDeliveryTime = dto.ActualDeliveryTime;
        order.Notes = dto.Notes;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var orderDto = new CanteenOrderDto
        {
            Id = order.Id,
            BranchId = order.BranchId,
            CashierShiftId = order.CashierShiftId,
            OrderType = order.OrderType,
            Status = order.Status,
            Total = order.Total,
            CreatedAt = order.CreatedAt
        };

        return new CanteenResponseDto<CanteenOrderDto>
        {
            Success = true,
            Message = "تم تحديث الطلب بنجاح",
            Data = orderDto
        };
    }

    public async Task<CanteenResponseDto<bool>> CancelOrderAsync(Guid id)
    {
        var order = await _context.CanteenOrders.FindAsync(id);
        if (order == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "الطلب غير موجود"
            };
        }

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم إلغاء الطلب بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<CanteenOrderDto>> GetOrderAsync(Guid id)
    {
        var order = await _context.CanteenOrders.FindAsync(id);
        if (order == null)
        {
            return new CanteenResponseDto<CanteenOrderDto>
            {
                Success = false,
                Message = "الطلب غير موجود"
            };
        }

        var orderDto = new CanteenOrderDto
        {
            Id = order.Id,
            BranchId = order.BranchId,
            CashierShiftId = order.CashierShiftId,
            OrderType = order.OrderType,
            Status = order.Status,
            Total = order.Total,
            CreatedAt = order.CreatedAt
        };

        return new CanteenResponseDto<CanteenOrderDto>
        {
            Success = true,
            Data = orderDto
        };
    }

    public async Task<CanteenPagedResponseDto<CanteenOrderDto>> GetOrdersAsync(CanteenOrderSearchDto search)
    {
        var query = _context.CanteenOrders.AsQueryable();

        if (search.BranchId.HasValue)
        {
            query = query.Where(o => o.BranchId == search.BranchId.Value);
        }

        if (search.OrderType.HasValue)
        {
            query = query.Where(o => o.OrderType == search.OrderType.Value);
        }

        if (search.Status.HasValue)
        {
            query = query.Where(o => o.Status == search.Status.Value);
        }

        if (search.StudentId.HasValue)
        {
            query = query.Where(o => o.StudentId == search.StudentId.Value);
        }

        var totalCount = await query.CountAsync();
        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(o => new CanteenOrderDto
            {
                Id = o.Id,
                BranchId = o.BranchId,
                CashierShiftId = o.CashierShiftId,
                OrderType = o.OrderType,
                Status = o.Status,
                Total = o.Total,
                CreatedAt = o.CreatedAt
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<CanteenOrderDto>
        {
            Success = true,
            Data = orders,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    public async Task<CanteenResponseDto<CanteenPaymentDto>> ProcessPaymentAsync(ProcessPaymentDto dto)
    {
        var order = await _context.CanteenOrders.FindAsync(dto.CanteenOrderId);
        if (order == null)
        {
            return new CanteenResponseDto<CanteenPaymentDto>
            {
                Success = false,
                Message = "الطلب غير موجود"
            };
        }

        var payment = new CanteenPayment
        {
            Id = Guid.NewGuid(),
            CanteenOrderId = dto.CanteenOrderId,
            Method = dto.Method,
            Status = PaymentStatus.Paid,
            Amount = order.Total,
            TransactionRef = dto.TransactionRef,
            PaidAt = DateTime.UtcNow,
            ProcessedBy = dto.ProcessedBy,
            Notes = dto.Notes,
            StudentNumber = dto.StudentNumber,
            EmployeeNumber = dto.EmployeeNumber
        };

        _context.CanteenPayments.Add(payment);
        await _context.SaveChangesAsync();

        var paymentDto = new CanteenPaymentDto
        {
            Id = payment.Id,
            CanteenOrderId = payment.CanteenOrderId,
            Method = payment.Method,
            Status = payment.Status,
            Amount = payment.Amount,
            TransactionRef = payment.TransactionRef,
            PaidAt = payment.PaidAt,
            ProcessedBy = payment.ProcessedBy,
            Notes = payment.Notes,
            StudentNumber = payment.StudentNumber,
            EmployeeNumber = payment.EmployeeNumber
        };

        return new CanteenResponseDto<CanteenPaymentDto>
        {
            Success = true,
            Message = "تم معالجة الدفع بنجاح",
            Data = paymentDto
        };
    }

    // ==================== Delivery Zones ====================
    public async Task<CanteenResponseDto<DeliveryZoneDto>> CreateDeliveryZoneAsync(CreateDeliveryZoneDto dto)
    {
        var zone = new DeliveryZone
        {
            Id = Guid.NewGuid(),
            BranchId = dto.BranchId,
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            DeliveryFee = dto.DeliveryFee,
            Description = dto.Description,
            IsActive = dto.IsActive,
            MinimumOrderAmount = dto.MinimumOrderAmount,
            EstimatedDeliveryTime = dto.EstimatedDeliveryTime,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.DeliveryZones.Add(zone);
        await _context.SaveChangesAsync();

        var zoneDto = new DeliveryZoneDto
        {
            Id = zone.Id,
            BranchId = zone.BranchId,
            NameAr = zone.NameAr,
            NameEn = zone.NameEn,
            DeliveryFee = zone.DeliveryFee,
            Description = zone.Description,
            IsActive = zone.IsActive,
            MinimumOrderAmount = zone.MinimumOrderAmount,
            EstimatedDeliveryTime = zone.EstimatedDeliveryTime,
            DisplayOrder = zone.DisplayOrder
        };

        return new CanteenResponseDto<DeliveryZoneDto>
        {
            Success = true,
            Message = "تم إنشاء منطقة التوصيل بنجاح",
            Data = zoneDto
        };
    }

    public async Task<CanteenResponseDto<DeliveryZoneDto>> UpdateDeliveryZoneAsync(UpdateDeliveryZoneDto dto)
    {
        var zone = await _context.DeliveryZones.FindAsync(dto.Id);
        if (zone == null)
        {
            return new CanteenResponseDto<DeliveryZoneDto>
            {
                Success = false,
                Message = "منطقة التوصيل غير موجودة"
            };
        }

        zone.BranchId = dto.BranchId;
        zone.NameAr = dto.NameAr;
        zone.NameEn = dto.NameEn;
        zone.DeliveryFee = dto.DeliveryFee;
        zone.Description = dto.Description;
        zone.IsActive = dto.IsActive;
        zone.MinimumOrderAmount = dto.MinimumOrderAmount;
        zone.EstimatedDeliveryTime = dto.EstimatedDeliveryTime;
        zone.DisplayOrder = dto.DisplayOrder;
        zone.UpdatedAt = DateTime.UtcNow;
        zone.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var zoneDto = new DeliveryZoneDto
        {
            Id = zone.Id,
            BranchId = zone.BranchId,
            NameAr = zone.NameAr,
            NameEn = zone.NameEn,
            DeliveryFee = zone.DeliveryFee,
            Description = zone.Description,
            IsActive = zone.IsActive,
            MinimumOrderAmount = zone.MinimumOrderAmount,
            EstimatedDeliveryTime = zone.EstimatedDeliveryTime,
            DisplayOrder = zone.DisplayOrder
        };

        return new CanteenResponseDto<DeliveryZoneDto>
        {
            Success = true,
            Message = "تم تحديث منطقة التوصيل بنجاح",
            Data = zoneDto
        };
    }

    public async Task<CanteenResponseDto<bool>> DeleteDeliveryZoneAsync(Guid id)
    {
        var zone = await _context.DeliveryZones.FindAsync(id);
        if (zone == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "منطقة التوصيل غير موجودة"
            };
        }

        _context.DeliveryZones.Remove(zone);
        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف منطقة التوصيل بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<DeliveryZoneDto>> GetDeliveryZoneAsync(Guid id)
    {
        var zone = await _context.DeliveryZones.FindAsync(id);
        if (zone == null)
        {
            return new CanteenResponseDto<DeliveryZoneDto>
            {
                Success = false,
                Message = "منطقة التوصيل غير موجودة"
            };
        }

        var zoneDto = new DeliveryZoneDto
        {
            Id = zone.Id,
            BranchId = zone.BranchId,
            NameAr = zone.NameAr,
            NameEn = zone.NameEn,
            DeliveryFee = zone.DeliveryFee,
            Description = zone.Description,
            IsActive = zone.IsActive,
            MinimumOrderAmount = zone.MinimumOrderAmount,
            EstimatedDeliveryTime = zone.EstimatedDeliveryTime,
            DisplayOrder = zone.DisplayOrder
        };

        return new CanteenResponseDto<DeliveryZoneDto>
        {
            Success = true,
            Data = zoneDto
        };
    }

    public async Task<CanteenPagedResponseDto<DeliveryZoneDto>> GetDeliveryZonesAsync(CanteenSearchDto search)
    {
        var query = _context.DeliveryZones.AsQueryable();

        var totalCount = await query.CountAsync();
        var zones = await query
            .OrderBy(z => z.DisplayOrder)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(z => new DeliveryZoneDto
            {
                Id = z.Id,
                BranchId = z.BranchId,
                NameAr = z.NameAr,
                NameEn = z.NameEn,
                DeliveryFee = z.DeliveryFee,
                Description = z.Description,
                IsActive = z.IsActive,
                MinimumOrderAmount = z.MinimumOrderAmount,
                EstimatedDeliveryTime = z.EstimatedDeliveryTime,
                DisplayOrder = z.DisplayOrder
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<DeliveryZoneDto>
        {
            Success = true,
            Data = zones,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    // ==================== PickUp Points ====================
    public async Task<CanteenResponseDto<PickUpPointDto>> CreatePickUpPointAsync(CreatePickUpPointDto dto)
    {
        var point = new PickUpPoint
        {
            Id = Guid.NewGuid(),
            DeliveryZoneId = dto.DeliveryZoneId,
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Location = dto.Location,
            Description = dto.Description,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.PickUpPoints.Add(point);
        await _context.SaveChangesAsync();

        var pointDto = new PickUpPointDto
        {
            Id = point.Id,
            DeliveryZoneId = point.DeliveryZoneId,
            NameAr = point.NameAr,
            NameEn = point.NameEn,
            Location = point.Location,
            Description = point.Description,
            IsActive = point.IsActive,
            DisplayOrder = point.DisplayOrder,
            Notes = point.Notes
        };

        return new CanteenResponseDto<PickUpPointDto>
        {
            Success = true,
            Message = "تم إنشاء نقطة الاستلام بنجاح",
            Data = pointDto
        };
    }

    public async Task<CanteenResponseDto<PickUpPointDto>> UpdatePickUpPointAsync(UpdatePickUpPointDto dto)
    {
        var point = await _context.PickUpPoints.FindAsync(dto.Id);
        if (point == null)
        {
            return new CanteenResponseDto<PickUpPointDto>
            {
                Success = false,
                Message = "نقطة الاستلام غير موجودة"
            };
        }

        point.DeliveryZoneId = dto.DeliveryZoneId;
        point.NameAr = dto.NameAr;
        point.NameEn = dto.NameEn;
        point.Location = dto.Location;
        point.Description = dto.Description;
        point.IsActive = dto.IsActive;
        point.DisplayOrder = dto.DisplayOrder;
        point.Notes = dto.Notes;
        point.UpdatedAt = DateTime.UtcNow;
        point.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var pointDto = new PickUpPointDto
        {
            Id = point.Id,
            DeliveryZoneId = point.DeliveryZoneId,
            NameAr = point.NameAr,
            NameEn = point.NameEn,
            Location = point.Location,
            Description = point.Description,
            IsActive = point.IsActive,
            DisplayOrder = point.DisplayOrder,
            Notes = point.Notes
        };

        return new CanteenResponseDto<PickUpPointDto>
        {
            Success = true,
            Message = "تم تحديث نقطة الاستلام بنجاح",
            Data = pointDto
        };
    }

    public async Task<CanteenResponseDto<bool>> DeletePickUpPointAsync(Guid id)
    {
        var point = await _context.PickUpPoints.FindAsync(id);
        if (point == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "نقطة الاستلام غير موجودة"
            };
        }

        _context.PickUpPoints.Remove(point);
        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف نقطة الاستلام بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<PickUpPointDto>> GetPickUpPointAsync(Guid id)
    {
        var point = await _context.PickUpPoints.FindAsync(id);
        if (point == null)
        {
            return new CanteenResponseDto<PickUpPointDto>
            {
                Success = false,
                Message = "نقطة الاستلام غير موجودة"
            };
        }

        var pointDto = new PickUpPointDto
        {
            Id = point.Id,
            DeliveryZoneId = point.DeliveryZoneId,
            NameAr = point.NameAr,
            NameEn = point.NameEn,
            Location = point.Location,
            Description = point.Description,
            IsActive = point.IsActive,
            DisplayOrder = point.DisplayOrder,
            Notes = point.Notes
        };

        return new CanteenResponseDto<PickUpPointDto>
        {
            Success = true,
            Data = pointDto
        };
    }

    public async Task<CanteenPagedResponseDto<PickUpPointDto>> GetPickUpPointsAsync(CanteenSearchDto search)
    {
        var query = _context.PickUpPoints.AsQueryable();

        var totalCount = await query.CountAsync();
        var points = await query
            .OrderBy(p => p.DisplayOrder)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(p => new PickUpPointDto
            {
                Id = p.Id,
                DeliveryZoneId = p.DeliveryZoneId,
                NameAr = p.NameAr,
                NameEn = p.NameEn,
                Location = p.Location,
                Description = p.Description,
                IsActive = p.IsActive,
                DisplayOrder = p.DisplayOrder,
                Notes = p.Notes
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<PickUpPointDto>
        {
            Success = true,
            Data = points,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    // ==================== Tables ====================
    public async Task<CanteenResponseDto<CanteenTableDto>> CreateCanteenTableAsync(CreateCanteenTableDto dto)
    {
        var table = new CanteenTable
        {
            Id = Guid.NewGuid(),
            BranchId = dto.BranchId,
            TableNumber = dto.TableNumber,
            Name = dto.Name,
            Capacity = dto.Capacity,
            Location = dto.Location,
            IsAvailable = dto.IsAvailable,
            Status = dto.Status,
            DisplayOrder = dto.DisplayOrder,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.CanteenTables.Add(table);
        await _context.SaveChangesAsync();

        var tableDto = new CanteenTableDto
        {
            Id = table.Id,
            BranchId = table.BranchId,
            TableNumber = table.TableNumber,
            Name = table.Name,
            Capacity = table.Capacity,
            Location = table.Location,
            IsAvailable = table.IsAvailable,
            Status = table.Status,
            DisplayOrder = table.DisplayOrder,
            Notes = table.Notes
        };

        return new CanteenResponseDto<CanteenTableDto>
        {
            Success = true,
            Message = "تم إنشاء الطاولة بنجاح",
            Data = tableDto
        };
    }

    public async Task<CanteenResponseDto<CanteenTableDto>> UpdateCanteenTableAsync(UpdateCanteenTableDto dto)
    {
        var table = await _context.CanteenTables.FindAsync(dto.Id);
        if (table == null)
        {
            return new CanteenResponseDto<CanteenTableDto>
            {
                Success = false,
                Message = "الطاولة غير موجودة"
            };
        }

        table.BranchId = dto.BranchId;
        table.TableNumber = dto.TableNumber;
        table.Name = dto.Name;
        table.Capacity = dto.Capacity;
        table.Location = dto.Location;
        table.IsAvailable = dto.IsAvailable;
        table.Status = dto.Status;
        table.DisplayOrder = dto.DisplayOrder;
        table.Notes = dto.Notes;
        table.UpdatedAt = DateTime.UtcNow;
        table.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var tableDto = new CanteenTableDto
        {
            Id = table.Id,
            BranchId = table.BranchId,
            TableNumber = table.TableNumber,
            Name = table.Name,
            Capacity = table.Capacity,
            Location = table.Location,
            IsAvailable = table.IsAvailable,
            Status = table.Status,
            DisplayOrder = table.DisplayOrder,
            Notes = table.Notes
        };

        return new CanteenResponseDto<CanteenTableDto>
        {
            Success = true,
            Message = "تم تحديث الطاولة بنجاح",
            Data = tableDto
        };
    }

    public async Task<CanteenResponseDto<bool>> DeleteCanteenTableAsync(Guid id)
    {
        var table = await _context.CanteenTables.FindAsync(id);
        if (table == null)
        {
            return new CanteenResponseDto<bool>
            {
                Success = false,
                Message = "الطاولة غير موجودة"
            };
        }

        _context.CanteenTables.Remove(table);
        await _context.SaveChangesAsync();

        return new CanteenResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف الطاولة بنجاح",
            Data = true
        };
    }

    public async Task<CanteenResponseDto<CanteenTableDto>> GetCanteenTableAsync(Guid id)
    {
        var table = await _context.CanteenTables.FindAsync(id);
        if (table == null)
        {
            return new CanteenResponseDto<CanteenTableDto>
            {
                Success = false,
                Message = "الطاولة غير موجودة"
            };
        }

        var tableDto = new CanteenTableDto
        {
            Id = table.Id,
            BranchId = table.BranchId,
            TableNumber = table.TableNumber,
            Name = table.Name,
            Capacity = table.Capacity,
            Location = table.Location,
            IsAvailable = table.IsAvailable,
            Status = table.Status,
            DisplayOrder = table.DisplayOrder,
            Notes = table.Notes
        };

        return new CanteenResponseDto<CanteenTableDto>
        {
            Success = true,
            Data = tableDto
        };
    }

    public async Task<CanteenPagedResponseDto<CanteenTableDto>> GetCanteenTablesAsync(CanteenSearchDto search)
    {
        var query = _context.CanteenTables.AsQueryable();

        var totalCount = await query.CountAsync();
        var tables = await query
            .OrderBy(t => t.TableNumber)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(t => new CanteenTableDto
            {
                Id = t.Id,
                BranchId = t.BranchId,
                TableNumber = t.TableNumber,
                Name = t.Name,
                Capacity = t.Capacity,
                Location = t.Location,
                IsAvailable = t.IsAvailable,
                Status = t.Status,
                DisplayOrder = t.DisplayOrder,
                Notes = t.Notes
            })
            .ToListAsync();

        return new CanteenPagedResponseDto<CanteenTableDto>
        {
            Success = true,
            Data = tables,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }
}