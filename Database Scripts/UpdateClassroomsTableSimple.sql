-- =====================================================
-- سكريبت تحديث جدول الفصول الدراسية الموجود (مبسط)
-- نظام إدارة المدارس - مَسَار
-- =====================================================

USE [MasarSchoolsERP]
GO

-- إضافة الأعمدة الجديدة إذا لم تكن موجودة
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ClassRooms') AND name = 'TenantId')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] ADD [TenantId] UNIQUEIDENTIFIER NULL
    PRINT 'تم إضافة عمود TenantId'
END
ELSE
BEGIN
    PRINT 'عمود TenantId موجود بالفعل'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ClassRooms') AND name = 'SchoolId')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] ADD [SchoolId] UNIQUEIDENTIFIER NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'
    PRINT 'تم إضافة عمود SchoolId'
END
ELSE
BEGIN
    PRINT 'عمود SchoolId موجود بالفعل'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ClassRooms') AND name = 'ClassTeacherId')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] ADD [ClassTeacherId] UNIQUEIDENTIFIER NULL
    PRINT 'تم إضافة عمود ClassTeacherId'
END
ELSE
BEGIN
    PRINT 'عمود ClassTeacherId موجود بالفعل'
END
GO

-- التحقق من وجود عمود Capacity وتعديله إذا لزم الأمر
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ClassRooms') AND name = 'Capacity')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] ADD [Capacity] INT NOT NULL DEFAULT 30
    PRINT 'تم إضافة عمود Capacity'
END
ELSE
BEGIN
    PRINT 'عمود Capacity موجود بالفعل'
END
GO

-- التحقق من وجود عمود CurrentCount
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ClassRooms') AND name = 'CurrentCount')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] ADD [CurrentCount] INT NOT NULL DEFAULT 0
    PRINT 'تم إضافة عمود CurrentCount'
END
ELSE
BEGIN
    PRINT 'عمود CurrentCount موجود بالفعل'
END
GO

-- إضافة المفاتيح الخارجية إذا لم تكن موجودة
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Tenants')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Tenants] FOREIGN KEY ([TenantId]) 
    REFERENCES [dbo].[Tenants] ([Id]) ON DELETE SET NULL
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Tenants'
END
ELSE
BEGIN
    PRINT 'مفتاح خارجي FK_Classrooms_Tenants موجود بالفعل'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Schools')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Schools] FOREIGN KEY ([SchoolId]) 
    REFERENCES [dbo].[Schools] ([Id]) ON DELETE CASCADE
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Schools'
END
ELSE
BEGIN
    PRINT 'مفتاح خارجي FK_Classrooms_Schools موجود بالفعل'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Employees_ClassTeacher')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Employees_ClassTeacher] FOREIGN KEY ([ClassTeacherId]) 
    REFERENCES [dbo].[Employees] ([Id]) ON DELETE SET NULL
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Employees_ClassTeacher'
END
ELSE
BEGIN
    PRINT 'مفتاح خارجي FK_Classrooms_Employees_ClassTeacher موجود بالفعل'
END
GO

-- تحديث البيانات الموجودة إذا لزم الأمر
UPDATE [dbo].[ClassRooms] 
SET [Capacity] = 30 
WHERE [Capacity] IS NULL OR [Capacity] = 0
GO

UPDATE [dbo].[ClassRooms] 
SET [CurrentCount] = 0 
WHERE [CurrentCount] IS NULL
GO

PRINT '============================================='
PRINT 'تم تحديث جدول Classrooms بنجاح'
PRINT '============================================='
GO