using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class BackupSettings : BaseEntity
{
    public string? DatabaseBackupPath { get; set; }
    public string? FilesBackupPath { get; set; }
    public int RetentionDays { get; set; } = 30;
    public bool EnableAutoBackup { get; set; } = true;
    public string AutoBackupTime { get; set; } = "02:00";
    public int MaxBackupCount { get; set; } = 50;
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}
