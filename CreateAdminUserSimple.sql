-- سكريبت بسيط لإنشاء مستخدم أدمن لنظام مَسَر للمدارس
-- ملاحظة: كلمة المرور يجب أن يتم إنشاؤها باستخدام C# لإنشاء الهاش الصحيح

USE MasarSchoolsERP;
GO

-- التحقق من وجود المستخدم
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'ADMIN@MASAR.COM')
BEGIN
    -- إنشاء Tenant إذا لم يكن موجوداً
    IF NOT EXISTS (SELECT 1 FROM Tenants WHERE IsDeleted = 0)
    BEGIN
        INSERT INTO Tenants (Id, Name, NameArabic, IsActive, IsDeleted, LicenseNumber, CreatedAt, CreatedBy)
        VALUES (NEWID(), 'مدرسة مَسَر الافتراضية', 'مدرسة مَسَر الافتراضية', 1, 0, 'DEFAULT', GETDATE(), 'System');
        PRINT 'تم إنشاء Tenant';
    END
    
    -- إنشاء دور SuperAdmin إذا لم يكن موجوداً
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'SUPERADMIN')
    BEGIN
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (NEWID(), 'SuperAdmin', 'SUPERADMIN', NEWID());
        PRINT 'تم إنشاء دور SuperAdmin';
    END
    
    -- الحصول على Tenant ID
    DECLARE @TenantId UNIQUEIDENTIFIER;
    SELECT TOP 1 @TenantId = Id FROM Tenants WHERE IsDeleted = 0;
    
    -- إنشاء المستخدم (بدون كلمة مرور في البداية)
    DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO AspNetUsers (
        Id, 
        UserName, 
        NormalizedUserName, 
        Email, 
        NormalizedEmail, 
        EmailConfirmed, 
        PhoneNumber, 
        FirstName, 
        LastName, 
        FullName, 
        IsActive, 
        TenantId, 
        SecurityStamp, 
        ConcurrencyStamp,
        LockoutEnabled,
        AccessFailedCount,
        CreatedAt,
        CreatedBy
    )
    VALUES (
        @UserId,
        'admin@masar.com',
        'ADMIN@MASAR.COM',
        'admin@masar.com',
        'ADMIN@MASAR.COM',
        1,
        '01006765664',
        'مدير',
        'النظام',
        'مدير النظام',
        1,
        @TenantId,
        NEWID(),
        NEWID(),
        1,
        0,
        GETDATE(),
        'System'
    );
    
    PRINT 'تم إنشاء المستخدم';
    PRINT 'معرف المستخدم: ' + CAST(@UserId AS NVARCHAR(50));
    PRINT 'البريد الإلكتروني: admin@masar.com';
    PRINT '====================================';
    PRINT '⚠️ ملاحظة مهمة:';
    PRINT 'المستخدم تم إنشاؤه بدون كلمة مرور.';
    PRINT 'يجب إضافة كلمة المرور باستخدام C# Console Application.';
    PRINT 'أو يمكنك استخدام ميزة "نسيت كلمة المرور" في صفحة تسجيل الدخول.';
    PRINT '====================================';
END
ELSE
BEGIN
    PRINT 'المستخدم موجود بالفعل';
END
GO
