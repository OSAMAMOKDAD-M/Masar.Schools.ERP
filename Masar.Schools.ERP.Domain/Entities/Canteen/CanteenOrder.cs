using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// طلبات الكانتين
/// </summary>
public class CanteenOrder : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public Guid BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public Guid CashierShiftId { get; set; }
    public CashierShift CashierShift { get; set; } = null!;

    /// <summary>
    /// نوع الطلب (صالة، سفري، دليفري)
    /// </summary>
    public OrderType OrderType { get; set; }

    /// <summary>
    /// حالة الطلب
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    /// <summary>
    /// للطلبات الصالة - الطاولة
    /// </summary>
    public Guid? CanteenTableId { get; set; }
    public CanteenTable? CanteenTable { get; set; }

    /// <summary>
    /// للطلبات الدليفري - منطقة التوصيل
    /// </summary>
    public Guid? DeliveryZoneId { get; set; }
    public DeliveryZone? DeliveryZone { get; set; }

    /// <summary>
    /// للطلبات الدليفري - نقطة الاستلام
    /// </summary>
    public Guid? PickUpPointId { get; set; }
    public PickUpPoint? PickUpPoint { get; set; }

    /// <summary>
    /// للطلبات الدليفري - الموصل
    /// </summary>
    public Guid? DeliveryEmployeeId { get; set; }
    public Employee? DeliveryEmployee { get; set; }

    /// <summary>
    /// رسوم التوصيل
    /// </summary>
    public decimal DeliveryFee { get; set; } = 0;

    /// <summary>
    /// للطلبات التي يدفعها الطالب
    /// </summary>
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }

    /// <summary>
    /// المبلغ المخصوم من محفظة الطالب
    /// </summary>
    public decimal StudentBalanceDeduction { get; set; } = 0;

    /// <summary>
    /// اسم العميل
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// رقم هاتف العميل
    /// </summary>
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// عنوان التوصيل (للدليفري)
    /// </summary>
    public string? DeliveryAddress { get; set; }

    /// <summary>
    /// المجموع الفرعي
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// مبلغ الخصم
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// مبلغ الضريبة
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// الإجمالي النهائي
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// ملاحظات عامة للطلب
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// عدد الأشخاص (لطلبات الصالة)
    /// </summary>
    public int? PartySize { get; set; }

    /// <summary>
    /// وقت التقديم المتوقع
    /// </summary>
    public DateTime? EstimatedDeliveryTime { get; set; }

    /// <summary>
    /// وقت التسليم الفعلي
    /// </summary>
    public DateTime? ActualDeliveryTime { get; set; }

    /// <summary>
    /// بنود الطلب
    /// </summary>
    public ICollection<CanteenOrderItem> Items { get; set; } = new List<CanteenOrderItem>();

    /// <summary>
    /// الدفع
    /// </summary>
    public CanteenPayment? Payment { get; set; }
}