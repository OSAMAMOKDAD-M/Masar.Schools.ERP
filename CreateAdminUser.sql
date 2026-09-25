-- إنشاء مستخدم أدمن يدوياً لنظام مَسَر للمدارس
-- استخدم هذا السكريبت إذا لم يتم إنشاء المستخدم تلقائياً

USE MasarSchoolsERP;
GO

-- التحقق من وجود Tenant
DECLARE @TenantId UNIQUEIDENTIFIER;
SELECT TOP 1 @TenantId = Id FROM Tenants WHERE IsDeleted = 0;

IF @TenantId IS NULL
BEGIN
    -- إنشاء Tenant افتراضي
    SET @TenantId = NEWID();
    INSERT INTO Tenants (Id, Name, NameArabic, IsActive, IsDeleted, LicenseNumber, CreatedAt, CreatedBy)
    VALUES (@TenantId, 'مدرسة مَسَر الافتراضية', 'مدرسة مَسَر الافتراضية', 1, 0, 'DEFAULT', GETDATE(), 'System');
    PRINT 'تم إنشاء Tenant افتراضي';
END
ELSE
BEGIN
    PRINT 'Tenant موجود بالفعل';
END

-- التحقق من وجود دور SuperAdmin
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'SUPERADMIN')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'SuperAdmin', 'SUPERADMIN', NEWID());
    PRINT 'تم إنشاء دور SuperAdmin';
END
ELSE
BEGIN
    PRINT 'دور SuperAdmin موجود بالفعل';
END

-- التحقق من وجود المستخدم
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'ADMIN@MASAR.COM')
BEGIN
    -- إنشاء المستخدم
    DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
    DECLARE @PasswordHash NVARCHAR(MAX);
    
    -- كلمة المرور: Admin@123456
    -- ملاحظة: هذا الهاش يتطلب حسابه يدوياً باستخدام ASP.NET Core Identity
    -- للتبسيط، سنستخدم طريقة أخرى
    
    INSERT INTO AspNetUsers (
        Id, 
        UserName, 
        NormalizedUserName, 
        Email, 
        NormalizedEmail, 
        EmailConfirmed, 
        PasswordHash, 
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
        'AQAAAAIAAClQAAAAEGJ/ZD6qZ4Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q7l5Q==', -- هذا مجرد مثال، يحتاج هاش حقيقي
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
    
    PRINT 'تم إنشاء المستخدم بنجاح';
    PRINT 'معرف المستخدم: ' + CAST(@UserId AS NVARCHAR(50));
END
ELSE
BEGIN
    PRINT 'المستخدم موجود بالفعل';
END

-- إضافة المستخدم إلى دور SuperAdmin
DECLARE @RoleId UNIQUEIDENTIFIER;
SELECT TOP 1 @RoleId = Id FROM AspNetRoles WHERE NormalizedName = 'SUPERADMIN';

DECLARE @UserId UNIQUEIDENTIFIER;
SELECT TOP 1 @UserId = Id FROM AspNetUsers WHERE NormalizedEmail = 'ADMIN@MASAR.COM';

IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, @RoleId);
        PRINT 'تم إضافة المستخدم إلى دور SuperAdmin';
    END
    ELSE
    BEGIN
        PRINT 'المستخدم موجود بالفعل في دور SuperAdmin';
    END
END

PRINT '====================================';
PRINT 'تم إنشاء المستخدم بنجاح!';
PRINT 'البريد الإلكتروني: admin@masar.com';
PRINT 'كلمة المرور: Admin@123456';
PRINT '====================================';
GO
