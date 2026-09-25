using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Schools.ERP.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Domain.Entities
{
    public class FinancialPeriod : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string PeriodName { get; set; } // مثال: "2024-2025" أو "يناير 2025"

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsClosed { get; set; } = false;

        public DateTime? ClosedDate { get; set; }

        public string? ClosedBy { get; set; }

        public string? ClosingNotes { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // العلاقات
        public Guid? TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        public Guid? SchoolId { get; set; }
        public School? School { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
