using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.Infrastructure.Configuration;

public class ClassroomConfiguration : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        // تكوين المفتاح الأساسي
        builder.HasKey(c => c.Id);
        
        // تكوين الخصائص الأساسية
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.NameArabic)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(c => c.GradeLevelLegacy)
            .HasMaxLength(50);
            
        builder.Property(c => c.SectionLegacy)
            .HasMaxLength(20);
            
        builder.Property(c => c.Capacity)
            .IsRequired()
            .HasDefaultValue(30);
            
        builder.Property(c => c.CurrentCount)
            .IsRequired()
            .HasDefaultValue(0);
            
        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        // تكوين العلاقات مع المؤسسة
        builder.HasOne(c => c.Tenant)
            .WithMany()
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // تكوين العلاقة مع المدرسة (Cascade Delete)
        builder.HasOne(c => c.School)
            .WithMany(s => s.ClassRooms)
            .HasForeignKey(c => c.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // تكوين العلاقة مع الفرع
        builder.HasOne(c => c.Branch)
            .WithMany(b => b.ClassRooms)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // تكوين العلاقة مع معلم الفصل
        builder.HasOne(c => c.ClassTeacher)
            .WithMany()
            .HasForeignKey(c => c.ClassTeacherId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // تكوين العلاقة مع الطلاب
        builder.HasMany(c => c.Students)
            .WithOne(s => s.ClassRoom)
            .HasForeignKey(s => s.ClassRoomId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // تكوين العلاقة مع سجلات الحضور
        builder.HasMany(c => c.AttendanceRecords)
            .WithOne(a => a.ClassRoom)
            .HasForeignKey(a => a.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // تكوين العلاقة مع غرف الدردشة
        builder.HasMany(c => c.ChatRooms)
            .WithOne(cr => cr.ClassRoom)
            .HasForeignKey(cr => cr.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // تكوين العلاقات مع المرحلة والشعبة
        builder.HasOne(c => c.GradeLevelEntity)
            .WithMany(g => g.ClassRooms)
            .HasForeignKey(c => c.GradeLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(c => c.SectionEntity)
            .WithMany(s => s.ClassRooms)
            .HasForeignKey(c => c.SectionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        // تكوين الفهارس (Indexes)
        builder.HasIndex(c => new { c.SchoolId, c.GradeLevelLegacy, c.IsDeleted })
            .HasDatabaseName("IX_Classrooms_School_GradeLevel");
        
        builder.HasIndex(c => new { c.SchoolId, c.Code })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("UX_Classrooms_School_Code");
        
        // تكوين الحذف الناعم (Soft Delete)
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}