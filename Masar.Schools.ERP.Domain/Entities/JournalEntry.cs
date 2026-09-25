using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class JournalEntry : BaseEntity
{
    public Guid EntryId { get; set; }
    public string EntryNumber { get; set; } = string.Empty; // JE-2026-0001
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPosted { get; set; } = false; // Posted to GL
    public DateTime? PostedDate { get; set; }
    public string? PostedBy { get; set; }
    public EntryType EntryType { get; set; }
    public Guid? ReferenceId { get; set; } // Reference to invoice, payment, etc.
    public string? ReferenceType { get; set; } // Invoice, Payment, etc.
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public ICollection<JournalEntryLine> EntryLines { get; set; } = new List<JournalEntryLine>();
}

public class JournalEntryLine : BaseEntity
{
    public Guid LineId { get; set; }
    public Guid EntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    public string? Notes { get; set; }
    public int LineNumber { get; set; }
}

public enum EntryType
{
    Manual = 1,           // قيد يدوي
    Invoice = 2,          // قيد فاتورة
    Payment = 3,          // قيد سداد
    Receipt = 4,          // قيد قبض
    Adjustment = 5,       // قيد تسوية
    Discount = 6,         // قيد خصم
    OpeningBalance = 7,   // قيد رصيد افتتاحي
    Closing = 8           // قيد إقفال
}
