using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class BackupRecord : BaseEntity
{
    public BackupType Type { get; set; }
    public BackupStatus Status { get; set; }
    public string? FilePath { get; set; }
    public long FileSizeBytes { get; set; }
    public string? StorageLocation { get; set; }
    public string? Description { get; set; }
    public bool IsAutomated { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public DateTime? CompletedTime { get; set; }
    public string? ErrorMessage { get; set; }
    public string? CreatedBy { get; set; }
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}

public enum BackupType
{
    Database = 1,
    Files = 2,
    Full = 3
}

public enum BackupStatus
{
    Pending = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5
}
