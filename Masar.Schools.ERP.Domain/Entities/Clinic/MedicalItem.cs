using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities.Clinic;

/// <summary>
/// الأدوية والمستلزمات الطبية في العيادة
/// </summary>
public class MedicalItem : BaseEntity
{
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    /// <summary>
    /// اسم الدواء/المستلزم
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// اسم الدواء بالعربية
    /// </summary>
    public string? ItemNameArabic { get; set; }

    /// <summary>
    /// نوع الدواء (دواء، مطهر، إسعاف، مستلزم)
    /// </summary>
    public string ItemType { get; set; } = string.Empty;

    /// <summary>
    /// نوع الدواء بالعربية
    /// </summary>
    public string? ItemTypeArabic { get; set; }

    /// <summary>
    /// الشركة المصنعة
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// الشركة المصنعة بالعربية
    /// </summary>
    public string? ManufacturerArabic { get; set; }

    /// <summary>
    /// رقم الصنف الدوائي
    /// </summary>
    public string? DrugCode { get; set; }

    /// <summary>
    /// رقم الشحنة
    /// </summary>
    public string? BatchNumber { get; set; }

    /// <summary>
    /// تاريخ الإنتاج
    /// </summary>
    public DateTime? ManufacturingDate { get; set; }

    /// <summary>
    /// تاريخ الانتهاء
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// الكمية المتاحة
    /// </summary>
    public int AvailableQuantity { get; set; }

    /// <summary>
    /// الوحدة (عبوة، علبة، أنبوب)
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// الوحدة بالعربية
    /// </summary>
    public string? UnitArabic { get; set; }

    /// <summary>
    /// السعر للوحدة
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// الحد الأدنى للجرعة
    /// </summary>
    public int MinimumStockLevel { get; set; }

    /// <summary>
    /// الكمية المرصرفة حالياً
    /// </summary>
    public int ConsumedQuantity { get; set; }

    /// <summary>
    /// موضع التخزين
    /// </summary>
    public string? StorageLocation { get; set; }

    /// <summary>
    /// موضع التخزين بالعربية
    /// </summary>
    public string? StorageLocationArabic { get; set; }

    /// <summary>
    /// درجة الحرارة المطلوبة للتخزين
    /// </summary>
    public string? RequiredTemperature { get; set; }

    /// <summary>
    /// هل يحتاج تخزين خاص (مثلاج)
    /// </summary>
    public bool RequiresSpecialStorage { get; set; }

    /// <summary>
    /// نوع التخزين المطلوب
    /// </summary>
    public string? StorageType { get; set; }

    /// <summary>
    /// نوع التخزين بالعربية
    /// </summary>
    public string? StorageTypeArabic { get; set; }

    /// <summary>
    /// الجرعة اليومية الموصى بها
    /// </summary>
    public string? RecommendedDosage { get; set; }

    /// <summary>
    /// الجرعة اليومية بالعربية
    /// </summary>
    public string? RecommendedDosageArabic { get; set; }

    /// <summary>
    /// الآثار الجانبية
    /// </summary>
    public string? SideEffects { get; set; }

    /// <summary>
    /// الآثار الجانبية بالعربية
    /// </summary>
    public string? SideEffectsArabic { get; set; }

    /// <summary>
    /// التداخلات الدوائية
    /// </summary>
    public string? DrugInteractions { get; set; }

    /// <summary>
    /// التداخلات الدوائية بالعربية
    /// </summary>
    public string? DrugInteractionsArabic { get; set; }

    /// <summary>
    /// موانع الاستخدام
    /// </summary>
    public string? Contraindications { get; set; }

    /// <summary>
    /// موانع الاستخدام بالعربية
    /// </summary>
    public string? ContraindicationsArabic { get; set; }

    /// <summary>
    /// تاريخ الانتهاء
    /// </summary>
    public bool IsExpired { get; set; }

    /// <summary>
    /// هل قرب من انتهاء الصلاحية (أقل من 30 يوم)
    /// </summary>
    public bool NearExpiry { get; set; }

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ملاحظات إضافية بالعربية
    /// </summary>
    public string? NotesArabic { get; set; }

    /// <summary>
    /// هل الدواء نشط
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// الكمية المحجوزة للصرف
    /// </summary>
    public int MaxDispenseQuantity { get; set; } = 10;

    /// <summary>
    /// تاريخ آخر تحديث
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// الموظف الذي حدث الكمية
    /// </summary>
    public Guid? UpdatedByEmployeeId { get; set; }
    public Employee? UpdatedByEmployee { get; set; }

    /// <summary>
    /// تاريخ إنشاء المستلزم
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// الموظف الذي أنشأ المستلزم
    /// </summary>
    public Guid? CreatedByEmployeeId { get; set; }
    public Employee? CreatedByEmployee { get; set; }
}