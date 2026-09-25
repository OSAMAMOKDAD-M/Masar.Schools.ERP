using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class StudentPayment : BaseEntity
{
    public string PaymentNumber { get; set; } = string.Empty; // PAY-2026-0001
    public Guid StudentAccountId { get; set; }
    public StudentAccount StudentAccount { get; set; } = null!;
    public Guid? InvoiceId { get; set; }
    public StudentInvoice? Invoice { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; } // Bank reference, Mada transaction ID, etc.
    public string? BankName { get; set; }
    public string? Notes { get; set; }
    public PaymentStatus Status { get; set; }
    public string? ReceiptNumber { get; set; } // Receipt number for printing
    public string? ReceiptPath { get; set; } // Path to generated receipt PDF
    public string? QrCodeBase64 { get; set; } // QR code for receipt
    public Guid? CreatedByUserId { get; set; }
    public string? ReceivedBy { get; set; }
    
    // Navigation properties
    public ICollection<PaymentAllocation> Allocations { get; set; } = new List<PaymentAllocation>();
}

public class PaymentAllocation : BaseEntity
{
    public Guid PaymentId { get; set; }
    public StudentPayment Payment { get; set; } = null!;
    public Guid InvoiceId { get; set; }
    public StudentInvoice Invoice { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}

public enum PaymentMethod
{
    Cash = 1,
    BankTransfer = 2,
    Mada = 3,
    Sadad = 4,
    ApplePay = 5,
    CreditCard = 6,
    Cheque = 7
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4,
    PartiallyRefunded = 5
}
