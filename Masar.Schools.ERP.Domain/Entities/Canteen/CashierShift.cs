using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// ورديات الكاشير في الكانتين
/// </summary>
public class CashierShift : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    /// <summary>
    /// وقت بدء الوردية
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// وقت انتهاء الوردية
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// الرصيد الافتتاحي للنقدية
    /// </summary>
    public decimal OpeningBalance { get; set; } = 0;

    /// <summary>
    /// الرصيد الافتتاحي لبطاقة الموظف
    /// </summary>
    public decimal OpeningCardBalance { get; set; } = 0;

    /// <summary>
    /// المبلغ المتوقع في الخزنة
    /// </summary>
    public decimal ExpectedCash { get; set; } = 0;

    /// <summary>
    /// المبلغ الفعلي في الخزنة
    /// </summary>
    public decimal ActualCash { get; set; } = 0;

    /// <summary>
    /// المدفوعات بالبطاقة
    /// </summary>
    public decimal CardPayments { get; set; } = 0;

    /// <summary>
    /// المدفوعات من محفظة الطالب
    /// </summary>
    public decimal StudentBalancePayments { get; set; } = 0;

    /// <summary>
    /// المدفوعات من محفظة الموظف
    /// </summary>
    public decimal EmployeeBalancePayments { get; set; } = 0;

    /// <summary>
    /// إجمالي المبيعات
    /// </summary>
    public decimal TotalSales { get; set; } = 0;

    /// <summary>
    /// عدد الطلبات
    /// </summary>
    public int OrderCount { get; set; } = 0;

    /// <summary>
    /// عدد الفواتير
    /// </summary>
    public int InvoiceCount { get; set; } = 0;

    /// <summary>
    /// العجز أو الزيادة
    /// </summary>
    public decimal ShortageOverage { get; set; } = 0;

    /// <summary>
    /// هل الوردية مغلقة
    /// </summary>
    public bool IsClosed { get; set; } = false;

    /// <summary>
    /// ملاحظات الوردية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// الموظف الذي أغلق الوردية
    /// </summary>
    public string? ClosedBy { get; set; }

    /// <summary>
    /// تفسير العجز أو الزيادة
    /// </summary>
    public string? ShortageOverageReason { get; set; }

    /// <summary>
    /// الطلبات في هذه الوردية
    /// </summary>
    public ICollection<CanteenOrder> Orders { get; set; } = new List<CanteenOrder>();
}