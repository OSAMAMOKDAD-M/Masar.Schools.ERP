-- =====================================================
-- سكريبت إضافة صلاحيات إدارة الفصول الدراسية
-- نظام إدارة المدارس - مَسَار
-- =====================================================

USE [MasarSchoolsERP]
GO

-- إضافة صلاحيات إدارة الفصول
IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Code] = 'Classrooms.View')
BEGIN
    INSERT INTO [Permissions] ([Id], [Code], [NameArabic], [NameEnglish], [Module], [Description], [DisplayOrder], [IsActive])
    VALUES 
    (NEWID(), 'Classrooms.View', 'عرض الفصول', 'View Classrooms', 'Classrooms', 'القدرة على عرض قائمة الفصول الدراسية', 1, 1),
    (NEWID(), 'Classrooms.Create', 'إضافة فصل', 'Create Classroom', 'Classrooms', 'القدرة على إضافة فصل دراسي جديد', 2, 1),
    (NEWID(), 'Classrooms.Edit', 'تعديل فصل', 'Edit Classroom', 'Classrooms', 'القدرة على تعديل بيانات الفصل الدراسي', 3, 1),
    (NEWID(), 'Classrooms.Delete', 'حذف فصل', 'Delete Classroom', 'Classrooms', 'القدرة على حذف الفصل الدراسي', 4, 1),
    (NEWID(), 'Classrooms.Details', 'تفاصيل الفصل', 'Classroom Details', 'Classrooms', 'القدرة على عرض تفاصيل الفصل الدراسي', 5, 1),
    (NEWID(), 'Classrooms.ManageCapacity', 'إدارة السعة', 'Manage Classroom Capacity', 'Classrooms', 'القدرة على إدارة سعة الفصل وتوزيع الطلاب', 6, 1),
    (NEWID(), 'Classrooms.AssignTeacher', 'تعيين معلم', 'Assign Classroom Teacher', 'Classrooms', 'القدرة على تعيين معلم للفصل', 7, 1);
    
    PRINT 'تم إضافة صلاحيات إدارة الفصول بنجاح'
END
ELSE
BEGIN
    PRINT 'صلاحيات إدارة الفصول موجودة بالفعل'
END
GO

-- إضافة الصلاحيات للدور الإداري (Admin Role)
IF EXISTS (SELECT * FROM [MasarRoles] WHERE [Name] = 'Admin')
BEGIN
    DECLARE @AdminRoleId UNIQUEIDENTIFIER
    SELECT @AdminRoleId = [Id] FROM [MasarRoles] WHERE [Name] = 'Admin'
    
    -- إضافة صلاحيات الفصول للدور الإداري
    INSERT INTO [RolePermissions] ([Id], [RoleId], [PermissionCode])
    SELECT NEWID(), @AdminRoleId, [Code] 
    FROM [Permissions] 
    WHERE [Module] = 'Classrooms' 
    AND [Code] NOT IN (
        SELECT [PermissionCode] 
        FROM [RolePermissions] 
        WHERE [RoleId] = @AdminRoleId
    )
    
    PRINT 'تم إضافة صلاحيات الفصول للدور الإداري'
END
GO

PRINT '============================================='
PRINT 'تم إضافة صلاحيات إدارة الفصول بنجاح'
PRINT '============================================='
GO