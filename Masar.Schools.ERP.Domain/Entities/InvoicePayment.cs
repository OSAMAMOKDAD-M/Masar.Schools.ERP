using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class InvoicePayment : BaseEntity
{
    public string PaymentNumber { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // Cash, BankTransfer, Mada, Sadad, ApplePay
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, Completed, Failed, Refunded
    
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}
