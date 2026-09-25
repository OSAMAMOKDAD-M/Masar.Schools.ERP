using Masar.Schools.ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masar.Schools.ERP.Infrastructure.Configuration;

/// <summary>
/// تكوين Entity RiskAlertHistory لـ EF Core
/// </summary>
public class RiskAlertHistoryConfiguration : IEntityTypeConfiguration<RiskAlertHistory>
{
    public void Configure(EntityTypeBuilder<RiskAlertHistory> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.PreviousRiskScore)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.NewRiskScore)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(r => r.TransitionType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.AlertMessage)
            .IsRequired(false);

        builder.Property(r => r.Recipient)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(r => r.Channel)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(r => r.ErrorMessage)
            .IsRequired(false);

        builder.Property(r => r.NotificationId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(r => r.SentAt)
            .IsRequired(false);

        builder.HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
