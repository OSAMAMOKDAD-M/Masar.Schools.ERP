namespace Masar.Schools.ERP.Domain.Entities.Canteen;

/// <summary>
/// أنواع الطلبات في الكانتين
/// </summary>
public enum OrderType
{
    /// <summary>
    /// طلب صالة - طاولة
    /// </summary>
    DiningIn = 1,

    /// <summary>
    /// طلب سفري - نقطة البيع السريع
    /// </summary>
    Takeaway = 2,

    /// <summary>
    /// طلب دليفري - توصيل
    /// </summary>
    Delivery = 3
}

/// <summary>
/// حالة الطلب
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// معلق - في انتظار المعالجة
    /// </summary>
    Pending = 1,

    /// <summary>
    /// قيد التحضير
    /// </summary>
    Preparing = 2,

    /// <summary>
    /// جاهز للتقديم
    /// </summary>
    Ready = 3,

    /// <summary>
    /// قيد التوصيل
    /// </summary>
    Delivering = 4,

    /// <summary>
    /// تم التسليم
    /// </summary>
    Delivered = 5,

    /// <summary>
    /// تم الإلغاء
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// مكتمل
    /// </summary>
    Completed = 7
}

/// <summary>
/// طرق الدفع
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// نقد
    /// </summary>
    Cash = 1,

    /// <summary>
    /// بطاقة الطالب
    /// </summary>
    StudentBalance = 2,

    /// <summary>
    /// بطاقة الموظف
    /// </summary>
    EmployeeBalance = 3,

    /// <summary>
    /// بطاقة ائتمانية
    /// </summary>
    Card = 4,

    /// <summary>
    /// تحويل بنكي
    /// </summary>
    BankTransfer = 5
}

/// <summary>
/// حالة الدفع
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// معلق
    /// </summary>
    Pending = 1,

    /// <summary>
    /// تم الدفع
    /// </summary>
    Paid = 2,

    /// <summary>
    /// فشل الدفع
    /// </summary>
    Failed = 3,

    /// <summary>
    /// مسترد
    /// </summary>
    Refunded = 4,

    /// <summary>
    /// مسترد جزئياً
    /// </summary>
    PartiallyRefunded = 5
}