-- إنشاء الأدوار الجديدة مع الصلاحيات المناسبة
-- Masar Schools ERP

-- 1. دور المعلم (Teacher)
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Teacher')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp, IsActive, CreatedAt, TenantId)
    VALUES (NEWID(), 'Teacher', 'TEACHER', NEWID(), 1, GETDATE(), 
            (SELECT TOP 1 Id FROM Tenants WHERE IsDeleted = 0))
END

-- 2. دور الموظف (Employee)
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Employee')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp, IsActive, CreatedAt, TenantId)
    VALUES (NEWID(), 'Employee', 'EMPLOYEE', NEWID(), 1, GETDATE(), 
            (SELECT TOP 1 Id FROM Tenants WHERE IsDeleted = 0))
END

-- 3. دور الكنترول (Admin/Controller)
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Controller')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp, IsActive, CreatedAt, TenantId)
    VALUES (NEWID(), 'Controller', 'CONTROLLER', NEWID(), 1, GETDATE(), 
            (SELECT TOP 1 Id FROM Tenants WHERE IsDeleted = 0))
END

-- 4. دور ولي الأمر (Guardian)
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Guardian')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp, IsActive, CreatedAt, TenantId)
    VALUES (NEWID(), 'Guardian', 'GUARDIAN', NEWID(), 1, GETDATE(), 
            (SELECT TOP 1 Id FROM Tenants WHERE IsDeleted = 0))
END

PRINT 'تم إنشاء الأدوار الجديدة بنجاح'
PRINT 'Teacher - دور المعلم'
PRINT 'Employee - دور الموظف' 
PRINT 'Controller - دور الكنترول'
PRINT 'Guardian - دور ولي الأمر'