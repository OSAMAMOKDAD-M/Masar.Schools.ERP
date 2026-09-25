using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class StudentAccount : BaseEntity
{
    public Guid AccountId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public string AccountNumber { get; set; } = string.Empty; // SA-2026-0001
    public decimal TotalBalance { get; set; } // Total invoiced amount
    public decimal OutstandingBalance { get; set; } // Amount due
    public decimal PaidBalance { get; set; } // Amount paid
    public decimal DiscountBalance { get; set; } // Total discounts applied
    public AccountStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? LastInvoiceDate { get; set; }
    public bool IsVatExempt { get; set; } = false; // For Saudi citizens exempt from private education VAT
    public string? VatExemptionCode { get; set; } // VATEX-SA-EDU
    
    // Navigation properties
    public ICollection<StudentInvoice> Invoices { get; set; } = new List<StudentInvoice>();
    public ICollection<StudentPayment> Payments { get; set; } = new List<StudentPayment>();
}

public enum AccountStatus
{
    Active = 1,
    Inactive = 2,
    Suspended = 3,
    Graduated = 4,
    Withdrawn = 5
}
