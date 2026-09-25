using Masar.Schools.ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masar.Schools.ERP.Infrastructure.Configuration;

/// <summary>
/// تكوين Entity StudentRiskScore لـ EF Core
/// </summary>
public class StudentRiskScoreConfiguration : IEntityTypeConfiguration<StudentRiskScore>
{
    public void Configure(EntityTypeBuilder<StudentRiskScore> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RiskScore)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.AttendanceRisk)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.AcademicRisk)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.FinancialRisk)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.AttendanceWeight)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.AcademicWeight)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.FinancialWeight)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.DataCompleteness)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.PrimaryRiskFactors)
            .IsRequired(false);

        builder.Property(r => r.Recommendations)
            .IsRequired(false);

        builder.Property(r => r.CalculatedAt)
            .IsRequired();

        builder.Property(r => r.CalculatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.CalculationVersion)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
