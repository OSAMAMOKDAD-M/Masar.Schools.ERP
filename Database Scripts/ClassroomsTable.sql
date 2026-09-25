-- =====================================================
-- سكريبت إنشاء جدول الفصول الدراسية (Classrooms)
-- نظام إدارة المدارس - مَسَار
-- =====================================================

USE [MasarSchoolsERP]
GO

-- إنشاء جدول الفصول الدراسية
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClassRooms')
BEGIN
    CREATE TABLE [dbo].[ClassRooms] (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [NameArabic] NVARCHAR(100) NOT NULL,
        [Code] NVARCHAR(20) NOT NULL,
        [GradeLevel] NVARCHAR(50) NULL,
        [Section] NVARCHAR(20) NULL,
        [Capacity] INT NOT NULL DEFAULT 30,
        [CurrentCount] INT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        
        -- العلاقات مع المؤسسة والمدرسة
        [TenantId] UNIQUEIDENTIFIER NULL,
        [SchoolId] UNIQUEIDENTIFIER NOT NULL,
        [BranchId] UNIQUEIDENTIFIER NULL,
        [ClassTeacherId] UNIQUEIDENTIFIER NULL,
        
        -- حقول التدقيق
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [CreatedBy] NVARCHAR(100) NULL,
        [UpdatedAt] DATETIME2 NULL,
        [UpdatedBy] NVARCHAR(100) NULL,
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [DeletedAt] DATETIME2 NULL,
        
        CONSTRAINT [PK_Classrooms] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    PRINT 'تم إنشاء جدول Classrooms بنجاح'
END
ELSE
BEGIN
    PRINT 'جدول Classrooms موجود بالفعل'
END
GO

-- إضافة الفهارس (Indexes)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Classrooms_School_GradeLevel')
BEGIN
    CREATE INDEX [IX_Classrooms_School_GradeLevel] ON [dbo].[Classrooms] ([SchoolId], [GradeLevel], [IsDeleted])
    PRINT 'تم إنشاء فهرس IX_Classrooms_School_GradeLevel'
END
GO

-- فهرس فريد لمنع تكرار كود الفصل داخل نفس المدرسة
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_Classrooms_School_Code')
BEGIN
    CREATE UNIQUE INDEX [UX_Classrooms_School_Code] ON [dbo].[ClassRooms] ([SchoolId], [Code]) WHERE [IsDeleted] = 0
    PRINT 'تم إنشاء فهرس فريد UX_Classrooms_School_Code'
END
GO

-- إضافة المفاتيح الخارجية (Foreign Keys)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Tenants')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Tenants] FOREIGN KEY ([TenantId]) 
    REFERENCES [dbo].[Tenants] ([Id]) ON DELETE SET NULL
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Tenants'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Schools')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Schools] FOREIGN KEY ([SchoolId]) 
    REFERENCES [dbo].[Schools] ([Id]) ON DELETE CASCADE
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Schools'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Branches')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Branches] FOREIGN KEY ([BranchId]) 
    REFERENCES [dbo].[Branches] ([Id]) ON DELETE SET NULL
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Branches'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Classrooms_Employees_ClassTeacher')
BEGIN
    ALTER TABLE [dbo].[ClassRooms] 
    ADD CONSTRAINT [FK_Classrooms_Employees_ClassTeacher] FOREIGN KEY ([ClassTeacherId]) 
    REFERENCES [dbo].[Employees] ([Id]) ON DELETE SET NULL
    PRINT 'تم إضافة مفتاح خارجي FK_Classrooms_Employees_ClassTeacher'
END
GO

-- إضافة أوصاف للحقول
EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'جدول الفصول الدراسية في النظام', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'اسم الفصل بالإنجليزي', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'Name'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'اسم الفصل بالعربي', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'NameArabic'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'كود الفصل الفريد داخل المدرسة', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'Code'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'المرحلة الدراسية (الصف)', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'GradeLevel'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'الشعبة/القسم', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'Section'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'السعة القصوى للفصل (عدد الطلاب)', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'Capacity'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'عدد الطلاب الحالي في الفصل', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'CurrentCount'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'حالة الفصل (نشط/غير نشط)', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'IsActive'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'معلم الفصل المسؤول', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'ClassRooms',
    @level2type = N'COLUMN', @level2name = N'ClassTeacherId'
GO

PRINT '============================================='
PRINT 'تم إنشاء بنية جدول Classrooms بنجاح'
PRINT '============================================='
GO