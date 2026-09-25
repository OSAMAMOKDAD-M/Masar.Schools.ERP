using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "SAR";
    public string Status { get; set; } = string.Empty; // Draft, Sent, Paid, Cancelled
    public string? Notes { get; set; }
    public string? ZatcaInvoiceUuid { get; set; }
    public string? ZatcaQrCode { get; set; }
    public string? ZatcaHash { get; set; }
    public DateTime? ZatcaSubmittedAt { get; set; }
    public bool ZatcaCompliance { get; set; }
    public string? ZatcaStatus { get; set; }
    public string? ZatcaErrorMessage { get; set; }
    
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();
}
