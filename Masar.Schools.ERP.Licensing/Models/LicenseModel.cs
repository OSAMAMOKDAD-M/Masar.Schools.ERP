using System;

namespace Masar.Schools.ERP.Licensing.Models;

/// <summary>
/// نموذج بيانات الترخيص
/// يحتوي على جميع المعلومات المشفرة في ملف الترخيص
/// </summary>
public class LicenseModel
{
    /// <summary>
    /// معرف فريد للترخيص
    /// </summary>
    public Guid LicenseId { get; set; }

    /// <summary>
    /// اسم المدرسة/العميل
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// بصمة الجهاز الفريدة (Machine ID)
    /// </summary>
    public string MachineId { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ بدء الترخيص
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// تاريخ انتهاء الترخيص
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// الحد الأقصى لعدد الطلاب المسموح
    /// </summary>
    public int MaxStudents { get; set; }

    /// <summary>
    /// الحد الأقصى لعدد المستخدمين المسموح
    /// </summary>
    public int MaxUsers { get; set; }

    /// <summary>
    /// الحد الأقصى لعدد الموظفين المسموح
    /// </summary>
    public int MaxStaff { get; set; }

    /// <summary>
    /// الموديولات المفعلة
    /// </summary>
    public LicenseModules Modules { get; set; } = new LicenseModules();

    /// <summary>
    /// نوع الترخيص
    /// </summary>
    public LicenseType LicenseType { get; set; }

    /// <summary>
    /// تاريخ إنشاء الترخيص
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// معرف المستخدم الفائق الذي أصدر الترخيص
    /// </summary>
    public string IssuedBy { get; set; } = string.Empty;

    /// <summary>
    /// ملاحظات إضافية
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// الموديولات المسموح بها في الترخيص
/// </summary>
[Flags]
public enum LicenseModules
{
    None = 0,
    Core = 1,                    // الوحدات الأساسية
    ZATCA = 2,                   // نظام ZATCA للفواتير
    WhatsApp = 4,                // تكامل واتساب
    Transport = 8,               // نظام النقل
    Clinic = 16,                 // العيادة المدرسية
    Canteen = 32,                // المقصف المدرسي
    Timetable = 64,              // الجدول المدرسي
    Behavior = 128,              // السلوك والمواظبة
    AdvancedReporting = 256,     // التقارير المتقدمة
    HR = 512,                    // نظام الموارد البشرية
    FullEdition = Core | ZATCA | WhatsApp | Transport | Clinic | Canteen | Timetable | Behavior | AdvancedReporting | HR
}

/// <summary>
/// نوع الترخيص
/// </summary>
public enum LicenseType
{
    Trial,           // نسخة تجريبية
    Standard,        // نسخة قياسية
    Professional,    // نسخة احترافية
    Enterprise,      // نسخة مؤسسية
    Custom           // نسخة مخصصة
}