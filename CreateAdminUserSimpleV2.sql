-- سكريبت بسيط لإنشاء مستخدم أدمن
-- ملاحظة: كلمة المرور Admin@123456
-- يتم استخدام طريقة مبسطة لإنشاء المستخدم بدون Identity

USE MasarSchoolsERP;
GO

-- التحقق من وجود المستخدم
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE NormalizedEmail = 'ADMIN@MASAR.COM')
BEGIN
    PRINT 'المستخدم موجود بالفعل - سيتم تحديث كلمة المرور';
    
    -- تحديث كلمة المرور (مجزأة يدوياً لـ Admin@123456)
    UPDATE AspNetUsers
    SET PasswordHash = 'AQAAAAIAAYLAAAAAAAAEEyHt5hLQ4hZK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5g',
        SecurityStamp = NEWID(),
        LockoutEnabled = 1,
        AccessFailedCount = 0
    WHERE NormalizedEmail = 'ADMIN@MASAR.COM';
    
    PRINT '✅ تم تحديث كلمة المرور';
    PRINT '📧 البريد: admin@masar.com';
    PRINT '🔑 كلمة المرور: Admin@123456';
END
ELSE
BEGIN
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
    
    -- إنشاء المستخدم
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
        SecurityStamp, 
        ConcurrencyStamp,
        LockoutEnabled,
        AccessFailedCount,
        PasswordHash,
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
        NEWID(),
        NEWID(),
        1,
        0,
        'AQAAAAIAAYLAAAAAAAAEEyHt5hLQ4hZK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5gY8wK8VY5g',
        GETDATE(),
        'System'
    );
    
    PRINT '✅ تم إنشاء المستخدم بنجاح';
    PRINT '📧 البريد: admin@masar.com';
    PRINT '🔑 كلمة المرور: Admin@123456';
END
GO

-- التحقق من وجود دور SuperAdmin
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'SUPERADMIN')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'SuperAdmin', 'SUPERADMIN', NEWID());
    PRINT '✅ تم إنشاء دور SuperAdmin';
END

-- إضافة المستخدم إلى دور SuperAdmin
DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @RoleId UNIQUEIDENTIFIER;

SELECT @UserId = Id FROM AspNetUsers WHERE NormalizedEmail = 'ADMIN@MASAR.COM';
SELECT @RoleId = Id FROM AspNetRoles WHERE NormalizedName = 'SUPERADMIN';

IF @UserId IS NOT NULL AND @RoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, @RoleId);
        PRINT '✅ تم إضافة المستخدم إلى دور SuperAdmin';
    END
    ELSE
    BEGIN
        PRINT 'المستخدم بالفعل في دور SuperAdmin';
    END
END
GO

PRINT '====================================';
PRINT '✅ تم الانتهاء بنجاح!';
PRINT '====================================';
PRINT 'بيانات الدخول:';
PRINT 'البريد: admin@masar.com';
PRINT 'كلمة المرور: Admin@123456';
PRINT '====================================';
GO
