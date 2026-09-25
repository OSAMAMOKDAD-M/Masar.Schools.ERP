using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// مدفوعات طلبات الكانتين
/// </summary>
public class CanteenPayment : BaseEntity
{
    public Guid CanteenOrderId { get; set; }
    public CanteenOrder Order { get; set; } = null!;

    /// <summary>
    /// طريقة الدفع
    /// </summary>
    public PaymentMethod Method { get; set; }

    /// <summary>
    /// حالة الدفع
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// المبلغ المدفوع
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// مرجع المعاملة (للبطاقات)
    /// </summary>
    public string? TransactionRef { get; set; }

    /// <summary>
    /// وقت الدفع
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// من قام بالعملية
    /// </summary>
    public string? ProcessedBy { get; set; }

    /// <summary>
    /// ملاحظات الدفع
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// رقم الطالب (للدفع من محفظة الطالب)
    /// </summary>
    public string? StudentNumber { get; set; }

    /// <summary>
    /// رقم الموظف (للدفع من محفظة الموظف)
    /// </summary>
    public string? EmployeeNumber { get; set; }

    /// <summary>
    /// الرصيد قبل الدفع
    /// </summary>
    public decimal BalanceBefore { get; set; }

    /// <summary>
    /// الرصيد بعد الدفع
    /// </summary>
    public decimal BalanceAfter { get; set; }
}