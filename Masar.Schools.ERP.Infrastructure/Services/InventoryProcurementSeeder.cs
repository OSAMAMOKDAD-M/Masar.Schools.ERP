using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة إضافة بيانات تجريبية للمخازن والمشتريات
/// </summary>
public class InventoryProcurementSeeder
{
    private readonly MasarDbContext _context;
    private readonly ILogger<InventoryProcurementSeeder> _logger;

    public InventoryProcurementSeeder(MasarDbContext context, ILogger<InventoryProcurementSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedInventoryProcurementDataAsync()
    {
        try
        {
            // الحصول على المدرسة الأولى للتجربة
            var school = await _context.Schools.FirstOrDefaultAsync(s => !s.IsDeleted);
            if (school == null)
            {
                _logger.LogWarning("No school found for seeding inventory data");
                return;
            }

            _logger.LogInformation($"Starting to seed inventory and procurement data for school: {school.NameArabic}");

            // إنشاء مخازن تجريبية
            await SeedWarehousesAsync(school.Id);

            // إنشاء موردين تجريبيين
            await SeedSuppliersAsync(school.Id);

            // إنشاء أصناف تجريبية
            await SeedInventoryItemsAsync(school.Id);

            // إنشاء فصل دراسي تجريبي
            await SeedAcademicTermAsync(school.Id);

            _logger.LogInformation("Inventory and procurement data seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding inventory and procurement data");
        }
    }

    private async Task SeedWarehousesAsync(Guid schoolId)
    {
        if (await _context.Warehouses.AnyAsync())
        {
            _logger.LogInformation("Warehouses already exist, skipping seeding");
            return;
        }

        var warehouses = new List<Warehouse>
        {
            new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "المخزن الرئيسي",
                NameArabic = "المخزن الرئيسي",
                Code = "WH-001",
                SchoolId = schoolId,
                WarehouseType = "Main Storage",
                WarehouseTypeArabic = "مخزن رئيسي",
                Location = "الدور الأرضي - المبنى الرئيسي",
                Manager = "أحمد محمد",
                ManagerPhone = "0501234567",
                Capacity = 1000,
                CapacityUnit = "متر مكعب",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "مخزن الورق والمستلزمات",
                NameArabic = "مخزن الورق والمستلزمات",
                Code = "WH-002",
                SchoolId = schoolId,
                WarehouseType = "Office Supplies",
                WarehouseTypeArabic = "مستلزمات مكتبية",
                Location = "الدور الأول - غرفة الإدارة",
                Manager = "سارة علي",
                ManagerPhone = "0507654321",
                Capacity = 200,
                CapacityUnit = "متر مكعب",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "مخزن الأدوات والمعدات",
                NameArabic = "مخزن الأدوات والمعدات",
                Code = "WH-003",
                SchoolId = schoolId,
                WarehouseType = "Equipment",
                WarehouseTypeArabic = "أدوات ومعدات",
                Location = "الملحق الخارجي - المخزن",
                Manager = "خالد عبدالله",
                ManagerPhone = "0509876543",
                Capacity = 500,
                CapacityUnit = "متر مكعب",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await _context.Warehouses.AddRangeAsync(warehouses);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Created {warehouses.Count} warehouses");
    }

    private async Task SeedSuppliersAsync(Guid schoolId)
    {
        if (await _context.Suppliers.AnyAsync())
        {
            _logger.LogInformation("Suppliers already exist, skipping seeding");
            return;
        }

        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "شركة التعليم للقرطاسية",
                NameArabic = "شركة التعليم للقرطاسية",
                Code = "SUP-001",
                TaxNumber = "300123456700003",
                ContactPerson = "محمد السعيد",
                Email = "info@educational-paper.sa",
                Phone = "0112345678",
                ContactPhone = "0501234567",
                Address = "الرياض - حي العليا",
                City = "الرياض",
                CreditLimit = 50000,
                PaymentTerms = "30 يوم",
                Rating = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "مؤسسة الأدوات المدرسية",
                NameArabic = "مؤسسة الأدوات المدرسية",
                Code = "SUP-002",
                TaxNumber = "300123456700004",
                ContactPerson = "عبدالرحمن العتيبي",
                Email = "sales@school-tools.sa",
                Phone = "0113456789",
                ContactPhone = "0502345678",
                Address = "جدة - حي الروضة",
                City = "جدة",
                CreditLimit = 30000,
                PaymentTerms = "45 يوم",
                Rating = 4,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "شركة التقنية التعليمية",
                NameArabic = "شركة التقنية التعليمية",
                Code = "SUP-003",
                TaxNumber = "300123456700005",
                ContactPerson = "فهد القحطاني",
                Email = "info@edu-tech.sa",
                Phone = "0114567890",
                ContactPhone = "0503456789",
                Address = "الدمام - حي الشاطئ",
                City = "الدمام",
                CreditLimit = 100000,
                PaymentTerms = "30 يوم",
                Rating = 5,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await _context.Suppliers.AddRangeAsync(suppliers);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Created {suppliers.Count} suppliers");
    }

    private async Task SeedInventoryItemsAsync(Guid schoolId)
    {
        if (await _context.InventoryItems.AnyAsync())
        {
            _logger.LogInformation("Inventory items already exist, skipping seeding");
            return;
        }

        var warehouse = await _context.Warehouses.FirstOrDefaultAsync();
        var supplier = await _context.Suppliers.FirstOrDefaultAsync();

        if (warehouse == null || supplier == null)
        {
            _logger.LogWarning("Warehouse or supplier not found, skipping inventory items seeding");
            return;
        }

        var items = new List<InventoryItem>
        {
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "ورق A4 - 80 جرام",
                NameArabic = "ورق A4 - 80 جرام",
                SKU = "ITM-001",
                Barcode = "1234567890123",
                Category = "مستلزمات مكتبية",
                CategoryArabic = "مستلزمات مكتبية",
                UnitOfMeasure = "رز",
                UnitOfMeasureArabic = "رز",
                ReorderLevel = 50,
                MaxStockLevel = 500,
                ReorderQuantity = 100,
                AverageCost = 45,
                SellingPrice = 55,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "ورق A4 للطباعة - 80 جرام - 500 ورقة في الرز",
                DescriptionArabic = "ورق A4 للطباعة - 80 جرام - 500 ورقة في الرز",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "أقلام حبر سوداء",
                NameArabic = "أقلام حبر سوداء",
                SKU = "ITM-002",
                Barcode = "1234567890124",
                Category = "أدوات كتابية",
                CategoryArabic = "أدوات كتابية",
                UnitOfMeasure = "صندوق",
                UnitOfMeasureArabic = "صندوق",
                ReorderLevel = 20,
                MaxStockLevel = 200,
                ReorderQuantity = 50,
                AverageCost = 120,
                SellingPrice = 150,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "أقلام حبر سوداء - صندوق 12 قلم",
                DescriptionArabic = "أقلام حبر سوداء - صندوق 12 قلم",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "ممحاة أبيض",
                NameArabic = "ممحاة أبيض",
                SKU = "ITM-003",
                Barcode = "1234567890125",
                Category = "أدوات كتابية",
                CategoryArabic = "أدوات كتابية",
                UnitOfMeasure = "علبة",
                UnitOfMeasureArabic = "علبة",
                ReorderLevel = 30,
                MaxStockLevel = 300,
                ReorderQuantity = 60,
                AverageCost = 15,
                SellingPrice = 20,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "ممحاة أبيض - علبة 10 محاة",
                DescriptionArabic = "ممحاة أبيض - علبة 10 محاة",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "دفتر 100 ورق",
                NameArabic = "دفتر 100 ورق",
                SKU = "ITM-004",
                Barcode = "1234567890126",
                Category = "كتب ومذكرات",
                CategoryArabic = "كتب ومذكرات",
                UnitOfMeasure = "حزمة",
                UnitOfMeasureArabic = "حزمة",
                ReorderLevel = 40,
                MaxStockLevel = 400,
                ReorderQuantity = 80,
                AverageCost = 25,
                SellingPrice = 30,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "دفتر مدرسي 100 ورق - حزمة 10 دفاتر",
                DescriptionArabic = "دفتر مدرسي 100 ورق - حزمة 10 دفاتر",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "حقيبة مدرسية",
                NameArabic = "حقيبة مدرسية",
                SKU = "ITM-005",
                Barcode = "1234567890127",
                Category = "معدات مدرسية",
                CategoryArabic = "معدات مدرسية",
                UnitOfMeasure = "قطعة",
                UnitOfMeasureArabic = "قطعة",
                ReorderLevel = 10,
                MaxStockLevel = 100,
                ReorderQuantity = 20,
                AverageCost = 80,
                SellingPrice = 100,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "حقيبة مدرسية - معدة للطلاب",
                DescriptionArabic = "حقيبة مدرسية - معدة للطلاب",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "باكت ألوان 12 لون",
                NameArabic = "باكت ألوان 12 لون",
                SKU = "ITM-006",
                Barcode = "1234567890128",
                Category = "أدوات فنية",
                CategoryArabic = "أدوات فنية",
                UnitOfMeasure = "صندوق",
                UnitOfMeasureArabic = "صندوق",
                ReorderLevel = 15,
                MaxStockLevel = 150,
                ReorderQuantity = 30,
                AverageCost = 35,
                SellingPrice = 45,
                Currency = "SAR",
                PreferredSupplierId = supplier.Id,
                IsStockItem = true,
                Description = "باكت ألوان - صندوق 12 باكت",
                DescriptionArabic = "باكت ألوان - صندوق 12 باكت",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await _context.InventoryItems.AddRangeAsync(items);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Created {items.Count} inventory items");

        // إنشاء حركات مخزون أولية لضبط المخزون
        await SeedInitialStockTransactionsAsync(items, warehouse.Id);
    }

    private async Task SeedInitialStockTransactionsAsync(List<InventoryItem> items, Guid warehouseId)
    {
        var transactions = new List<StockTransaction>();

        foreach (var item in items)
        {
            var targetStock = item.SKU switch
            {
                "ITM-001" => 200, // ورق A4
                "ITM-002" => 80,  // أقلام حبر
                "ITM-003" => 150, // محاية
                "ITM-004" => 300, // دفتر
                "ITM-005" => 45,  // حقيبة
                "ITM-006" => 60,  // باكت ألوان
                _ => 100  // افتراضي
            };

            transactions.Add(new StockTransaction
            {
                Id = Guid.NewGuid(),
                InventoryItemId = item.Id,
                TransactionType = "Receipt",
                TransactionTypeArabic = "استلام",
                Quantity = targetStock,
                UnitCost = item.AverageCost,
                TotalCost = targetStock * item.AverageCost,
                WarehouseId = warehouseId,
                Reference = "INITIAL-SEED",
                TransactionDate = DateTime.UtcNow,
                Notes = "حركة مخزون أولية من البيانات التجريبية",
                PerformedBy = "System"
            });
        }

        await _context.StockTransactions.AddRangeAsync(transactions);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Created {transactions.Count} initial stock transactions");
    }

    private async Task SeedAcademicTermAsync(Guid schoolId)
    {
        if (await _context.AcademicTerms.AnyAsync())
        {
            _logger.LogInformation("Academic terms already exist, skipping seeding");
            return;
        }

        var term = new AcademicTerm
        {
            Id = Guid.NewGuid(),
            TermName = "الفصل الدراسي الأول 1446",
            TermNameArabic = "الفصل الدراسي الأول 1446",
            SchoolId = schoolId,
            StartDate = new DateTime(2024, 9, 1),
            EndDate = new DateTime(2025, 1, 31),
            Status = "Active",
            StatusArabic = "نشط",
            IsActive = true,
            BiometricAttendanceEnabled = true,
            RiskPredictionEnabled = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        await _context.AcademicTerms.AddAsync(term);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Created academic term: {term.TermNameArabic}");
    }
}
