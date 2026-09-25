using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class InvoiceItem : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
    public string? ItemCode { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal TaxPercentage { get; set; }
    public decimal LineTotal { get; set; }
    public string? ZatcaItemCode { get; set; }
    
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}
