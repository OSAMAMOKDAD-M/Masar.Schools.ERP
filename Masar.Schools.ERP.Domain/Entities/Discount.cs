using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Discount : BaseEntity
{
    public Guid DiscountId { get; set; }
    public string DiscountCode { get; set; } = string.Empty; // BROTHER10, EXCELLENCE20
    public string DiscountNameAr { get; set; } = string.Empty;
    public string DiscountNameEn { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal FixedAmount { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
    public int? MaxUsagePerStudent { get; set; }
    public int CurrentUsageCount { get; set; }
    
    // Navigation properties
    public ICollection<StudentDiscount> StudentDiscounts { get; set; } = new List<StudentDiscount>();
}

public class StudentDiscount : BaseEntity
{
    public Guid StudentDiscountId { get; set; }
    public Guid StudentAccountId { get; set; }
    public StudentAccount StudentAccount { get; set; } = null!;
    public Guid DiscountId { get; set; }
    public Discount Discount { get; set; } = null!;
    public Guid? InvoiceId { get; set; }
    public StudentInvoice? Invoice { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime AppliedDate { get; set; }
    public string? Notes { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
}

public enum DiscountType
{
    Percentage = 1,      // خصم نسبة مئوية
    FixedAmount = 2,     // خصم مبلغ ثابت
    BrotherDiscount = 3,  // خصم الأخوة
    Scholarship = 4,     // منحة دراسية
    EarlyPayment = 5,    // خصم الدفع المبكر
    StaffDiscount = 6    // خصم الموظفين
}
