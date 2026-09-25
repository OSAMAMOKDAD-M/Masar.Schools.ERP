using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class StudentInvoice : BaseEntity
{
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty; // INV-2026-0001
    public string UUID { get; set; } = string.Empty; // ZATCA UUID
    public Guid StudentAccountId { get; set; }
    public StudentAccount StudentAccount { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal TotalAmount { get; set; } // Including VAT
    public decimal TaxAmount { get; set; } // 15% VAT
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; } // After discount, before VAT
    public decimal PaidAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public InvoiceStatus Status { get; set; }
    public InvoiceType InvoiceType { get; set; } // B2C (Simplified) or B2B (Standard)
    public ZatcaStatus ZatcaStatus { get; set; }
    public string? XmlPath { get; set; } // Path to generated XML file
    public string? QrCodeBase64 { get; set; } // Base64 encoded QR code
    public string? CryptographicStamp { get; set; } // ZATCA digital signature
    public string? Hash { get; set; } // SHA-256 hash
    public bool IsVatExempt { get; set; } = false;
    public string? VatExemptionCode { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public ICollection<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();
    public ICollection<StudentPayment> Payments { get; set; } = new List<StudentPayment>();
}

public class InvoiceLineItem : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid InvoiceId { get; set; }
    public StudentInvoice Invoice { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal TaxRate { get; set; } // 15% or 0% for exempt
    public decimal TaxAmount { get; set; }
    public decimal LineTotal { get; set; } // Including tax
    public decimal LineNet { get; set; } // Before tax
    public string? ItemCode { get; set; }
    public int LineNumber { get; set; }
}

public enum InvoiceStatus
{
    Draft = 1,
    Issued = 2,
    PartiallyPaid = 3,
    Paid = 4,
    Overdue = 5,
    Cancelled = 6,
    Void = 7
}

public enum InvoiceType
{
    B2C = 1, // Simplified Tax Invoice
    B2B = 2  // Standard Tax Invoice
}

public enum ZatcaStatus
{
    Pending = 1,
    Reported = 2,        // Submitted to ZATCA (for B2C)
    Cleared = 3,        // Cleared by ZATCA (for B2B)
    Failed = 4,
    ComplianceCheck = 5
}
