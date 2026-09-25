-- سكريبت إضافة بيانات تجريبية للمخازن والمشتريات
-- قم بتشغيل هذا السكريبت بعد تسجيل الدخول وإنشاء مدرسة

-- الحصول على معرف المدرسة الأولى (استبدل بقيمة فعلية)
DECLARE @SchoolId UNIQUEIDENTIFIER;
SELECT TOP 1 @SchoolId = Id FROM Schools WHERE IsDeleted = 0;

IF @SchoolId IS NULL
BEGIN
    PRINT 'لا توجد مدرسة في قاعدة البيانات. الرجاء إنشاء مدرسة أولاً.';
END
ELSE
BEGIN
    PRINT 'استخدام المدرسة: ' + CAST(@SchoolId AS NVARCHAR(50));
    
    -- إضافة مخازن تجريبية
    IF NOT EXISTS (SELECT 1 FROM Warehouses WHERE Code = 'WH-001')
    BEGIN
        INSERT INTO Warehouses (Id, Name, NameArabic, Code, SchoolId, WarehouseType, WarehouseTypeArabic, Location, Manager, ManagerPhone, Capacity, CapacityUnit, IsActive, CreatedAt, CreatedBy, IsDeleted)
        VALUES 
        (NEWID(), 'المخزن الرئيسي', 'المخزن الرئيسي', 'WH-001', @SchoolId, 'Main Storage', 'مخزن رئيسي', 'الدور الأرضي - المبنى الرئيسي', 'أحمد محمد', '0501234567', 1000, 'متر مكعب', 1, GETDATE(), 'System', 0),
        (NEWID(), 'مخزن الورق والمستلزمات', 'مخزن الورق والمستلزمات', 'WH-002', @SchoolId, 'Office Supplies', 'مستلزمات مكتبية', 'الدور الأول - غرفة الإدارة', 'سارة علي', '0507654321', 200, 'متر مكعب', 1, GETDATE(), 'System', 0),
        (NEWID(), 'مخزن الأدوات والمعدات', 'مخزن الأدوات والمعدات', 'WH-003', @SchoolId, 'Equipment', 'أدوات ومعدات', 'الملحق الخارجي - المخزن', 'خالد عبدالله', '0509876543', 500, 'متر مكعب', 1, GETDATE(), 'System', 0);
        
        PRINT 'تم إضافة 3 مخازن';
    END
    
    -- الحصول على معرف المخزن الأول
    DECLARE @WarehouseId UNIQUEIDENTIFIER;
    SELECT TOP 1 @WarehouseId = Id FROM Warehouses WHERE Code = 'WH-001';
    
    -- إضافة موردين تجريبيين
    IF NOT EXISTS (SELECT 1 FROM Suppliers WHERE Code = 'SUP-001')
    BEGIN
        INSERT INTO Suppliers (Id, Name, NameArabic, Code, TaxNumber, ContactPerson, Email, Phone, ContactPhone, Address, City, CreditLimit, PaymentTerms, Rating, IsActive, CreatedAt, CreatedBy, IsDeleted)
        VALUES 
        (NEWID(), 'شركة التعليم للقرطاسية', 'شركة التعليم للقرطاسية', 'SUP-001', '300123456700003', 'محمد السعيد', 'info@educational-paper.sa', '0112345678', '0501234567', 'الرياض - حي العليا', 'الرياض', 50000, '30 يوم', 5, 1, GETDATE(), 'System', 0),
        (NEWID(), 'مؤسسة الأدوات المدرسية', 'مؤسسة الأدوات المدرسية', 'SUP-002', '300123456700004', 'عبدالرحمن العتيبي', 'sales@school-tools.sa', '0113456789', '0502345678', 'جدة - حي الروضة', 'جدة', 30000, '45 يوم', 4, 1, GETDATE(), 'System', 0),
        (NEWID(), 'شركة التقنية التعليمية', 'شركة التقنية التعليمية', 'SUP-003', '300123456700005', 'فهد القحطاني', 'info@edu-tech.sa', '0114567890', '0503456789', 'الدمام - حي الشاطئ', 'الدمام', 100000, '30 يوم', 5, 1, GETDATE(), 'System', 0);
        
        PRINT 'تم إضافة 3 موردين';
    END
    
    -- الحصول على معرف المورد الأول
    DECLARE @SupplierId UNIQUEIDENTIFIER;
    SELECT TOP 1 @SupplierId = Id FROM Suppliers WHERE Code = 'SUP-001';
    
    -- إضافة أصناف تجريبية
    IF NOT EXISTS (SELECT 1 FROM InventoryItems WHERE SKU = 'ITM-001')
    BEGIN
        INSERT INTO InventoryItems (Id, Name, NameArabic, SKU, Barcode, Category, CategoryArabic, UnitOfMeasure, UnitOfMeasureArabic, ReorderLevel, MaxStockLevel, ReorderQuantity, AverageCost, SellingPrice, Currency, IsActive, IsStockItem, Description, DescriptionArabic, PreferredSupplierId, CreatedAt, CreatedBy, IsDeleted)
        VALUES 
        (NEWID(), 'ورق A4 - 80 جرام', 'ورق A4 - 80 جرام', 'ITM-001', '1234567890123', 'مستلزمات مكتبية', 'مستلزمات مكتبية', 'رز', 'رز', 50, 500, 100, 45, 55, 'SAR', 1, 1, 'ورق A4 للطباعة - 80 جرام - 500 ورقة في الرز', 'ورق A4 للطباعة - 80 جرام - 500 ورقة في الرز', @SupplierId, GETDATE(), 'System', 0),
        (NEWID(), 'أقلام حبر سوداء', 'أقلام حبر سوداء', 'ITM-002', '1234567890124', 'أدوات كتابية', 'أدوات كتابية', 'صندوق', 'صندوق', 20, 200, 50, 120, 150, 'SAR', 1, 1, 'أقلام حبر سوداء - صندوق 12 قلم', 'أقلام حبر سوداء - صندوق 12 قلم', @SupplierId, GETDATE(), 'System', 0),
        (NEWID(), 'ممحاة أبيض', 'ممحاة أبيض', 'ITM-003', '1234567890125', 'أدوات كتابية', 'أدوات كتابية', 'علبة', 'علبة', 30, 300, 60, 15, 20, 'SAR', 1, 1, 'ممحاة أبيض - علبة 10 محاة', 'ممحاة أبيض - علبة 10 محاة', @SupplierId, GETDATE(), 'System', 0),
        (NEWID(), 'دفتر 100 ورق', 'دفتر 100 ورق', 'ITM-004', '1234567890126', 'كتب ومذكرات', 'كتب ومذكرات', 'حزمة', 'حزمة', 40, 400, 80, 25, 30, 'SAR', 1, 1, 'دفتر مدرسي 100 ورق - حزمة 10 دفاتر', 'دفتر مدرسي 100 ورق - حزمة 10 دفاتر', @SupplierId, GETDATE(), 'System', 0),
        (NEWID(), 'حقيبة مدرسية', 'حقيبة مدرسية', 'ITM-005', '1234567890127', 'معدات مدرسية', 'معدات مدرسية', 'قطعة', 'قطعة', 10, 100, 20, 80, 100, 'SAR', 1, 1, 'حقيبة مدرسية - معدة للطلاب', 'حقيبة مدرسية - معدة للطلاب', @SupplierId, GETDATE(), 'System', 0),
        (NEWID(), 'باكت ألوان 12 لون', 'باكت ألوان 12 لون', 'ITM-006', '1234567890128', 'أدوات فنية', 'أدوات فنية', 'صندوق', 'صندوق', 15, 150, 30, 35, 45, 'SAR', 1, 1, 'باكت ألوان - صندوق 12 باكت', 'باكت ألوان - صندوق 12 باكت', @SupplierId, GETDATE(), 'System', 0);
        
        PRINT 'تم إضافة 6 أصناف';
    END
    
    -- إضافة فصل دراسي تجريبي
    IF NOT EXISTS (SELECT 1 FROM AcademicTerms WHERE Status = 'Active')
    BEGIN
        INSERT INTO AcademicTerms (Id, TermName, TermNameArabic, SchoolId, StartDate, EndDate, Status, StatusArabic, IsActive, BiometricAttendanceEnabled, RiskPredictionEnabled, CreatedAt, CreatedBy, IsDeleted)
        VALUES (NEWID(), 'الفصل الدراسي الأول 1446', 'الفصل الدراسي الأول 1446', @SchoolId, '2024-09-01', '2025-01-31', 'Active', 'نشط', 1, 1, 1, GETDATE(), 'System', 0);
        
        PRINT 'تم إضافة فصل دراسي تجريبي';
    END
    
    PRINT 'تم إكمال إضافة البيانات التجريبية بنجاح!';
END
