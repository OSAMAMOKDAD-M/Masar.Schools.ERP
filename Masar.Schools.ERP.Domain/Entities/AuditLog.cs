using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Masar.Schools.ERP.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string UserName { get; set; } // اسم المستخدم الذي قام بالتعديل

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public required string ActionType { get; set; } // مثال: "Override", "Edit", "Delete", "Create"

        [Required]
        [StringLength(200)]
        public required string EntityName { get; set; } // مثال: "Invoice", "JournalEntry", "AttendanceRecord"

        public Guid? EntityId { get; set; } // معرف الكيان الذي تم تعديله

        [StringLength(500)]
        public string? Description { get; set; } // وصف العملية

        public string? Reason { get; set; } // سبب التعديل (للسوبر أدمن)

        public string? OldValues { get; set; } // البيانات قبل التعديل (JSON)

        public string? NewValues { get; set; } // البيانات بعد التعديل (JSON)

        [Required]
        public string? IpAddress { get; set; } // عنوان IP للمستخدم

        // العلاقات
        public Guid? FinancialPeriodId { get; set; }
    }
}
