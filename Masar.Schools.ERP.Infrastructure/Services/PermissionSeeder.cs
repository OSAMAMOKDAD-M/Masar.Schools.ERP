using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة مزامنة الصلاحيات التلقائية
/// تقوم بتحديث جدول Permissions في قاعدة البيانات تلقائياً من PermissionConstants
/// دون مسح الصلاحيات المحددة يدوياً
/// </summary>
public class PermissionSeeder
{
    private readonly MasarDbContext _context;
    private readonly ILogger<PermissionSeeder> _logger;

    public PermissionSeeder(
        MasarDbContext context,
        ILogger<PermissionSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// مزامنة الصلاحيات من PermissionConstants إلى قاعدة البيانات
    /// </summary>
    public async Task SyncPermissionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting permission synchronization...");

        try
        {
            var modulePermissions = PermissionConstants.GetPermissionsByModule();
            var existingPermissions = await _context.Permissions
                .ToListAsync(cancellationToken);

            var existingPermissionCodes = existingPermissions.Select(p => p.Code).ToHashSet();
            var permissionsToSync = new List<Permission>();

            foreach (var (moduleName, permissionCodes) in modulePermissions)
            {
                foreach (var permissionCode in permissionCodes)
                {
                    if (!existingPermissionCodes.Contains(permissionCode))
                    {
                        // الصلاحية جديدة - إضافتها
                        var permission = CreatePermissionFromCode(permissionCode, moduleName);
                        permissionsToSync.Add(permission);
                        _logger.LogInformation("Adding new permission: {PermissionCode} in module {Module}", permissionCode, moduleName);
                    }
                    else
                    {
                        // الصلاحية موجودة - التحقق من تحديث المعلومات
                        var existingPermission = existingPermissions.First(p => p.Code == permissionCode);
                        var updatedPermission = CreatePermissionFromCode(permissionCode, moduleName);

                        if (ShouldUpdatePermission(existingPermission, updatedPermission))
                        {
                            UpdatePermission(existingPermission, updatedPermission);
                            _logger.LogInformation("Updating permission: {PermissionCode}", permissionCode);
                        }
                    }
                }
            }

            // إضافة الصلاحيات الجديدة
            if (permissionsToSync.Any())
            {
                await _context.Permissions.AddRangeAsync(permissionsToSync, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Added {Count} new permissions", permissionsToSync.Count);
            }

            // تحديث الصلاحيات المعدلة
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Updated existing permissions");
            }

            _logger.LogInformation("Permission synchronization completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during permission synchronization");
            throw;
        }
    }

    /// <summary>
    /// إنشاء كيان Permission من كود الصلاحية
    /// </summary>
    private Permission CreatePermissionFromCode(string permissionCode, string moduleName)
    {
        var parts = permissionCode.Split('.');
        var action = parts.Length > 1 ? parts[1] : "View";
        var entity = parts[0];

        return new Permission
        {
            Id = Guid.NewGuid(),
            Code = permissionCode,
            NameArabic = GetArabicName(permissionCode, action, entity),
            NameEnglish = GetEnglishName(permissionCode, action, entity),
            Module = moduleName,
            Description = GetDescription(permissionCode, action, entity),
            DisplayOrder = GetDisplayOrder(moduleName, action),
            IsActive = true
        };
    }

    /// <summary>
    /// الحصول على الاسم العربي للصلاحية
    /// </summary>
    private string GetArabicName(string permissionCode, string action, string entity)
    {
        var entityArabic = GetEntityArabicName(entity);
        var actionArabic = GetActionArabicName(action);

        return $"{actionArabic} {entityArabic}";
    }

    /// <summary>
    /// الحصول على الاسم الإنجليزي للصلاحية
    /// </summary>
    private string GetEnglishName(string permissionCode, string action, string entity)
    {
        return $"{action} {entity}";
    }

    /// <summary>
    /// الحصول على وصف الصلاحية
    /// </summary>
    private string GetDescription(string permissionCode, string action, string entity)
    {
        var entityArabic = GetEntityArabicName(entity);
        var actionArabic = GetActionArabicName(action);

        return $"السماح بـ {actionArabic.ToLower()} للـ {entityArabic.ToLower()}";
    }

    /// <summary>
    /// الحصول على ترتيب العرض
    /// </summary>
    private int GetDisplayOrder(string module, string action)
    {
        var moduleOrder = GetModuleOrder(module);
        var actionOrder = GetActionOrder(action);

        return moduleOrder * 100 + actionOrder;
    }

    /// <summary>
    /// الحصول على ترتيب الموديول
    /// </summary>
    private int GetModuleOrder(string module)
    {
        return module switch
        {
            "Students" => 1,
            "Guardians" => 2,
            "Employees" => 3,
            "Attendance" => 4,
            "Grades" => 5,
            "Invoices" => 6,
            "Financial" => 7,
            "Transport" => 8,
            "Clinic" => 9,
            "Canteen" => 10,
            "Timetable" => 11,
            "Behavior" => 12,
            "Reports" => 13,
            "Settings" => 14,
            "Chat" => 15,
            "Admin" => 16,
            "Classrooms" => 17,
            "StudentAccounts" => 18,
            "GeneralLedger" => 19,
            "FinancialYearClosing" => 20,
            "EarlyWarning" => 21,
            "License" => 22,
            _ => 99
        };
    }

    /// <summary>
    /// الحصول على ترتيب الإجراء
    /// </summary>
    private int GetActionOrder(string action)
    {
        return action switch
        {
            "View" => 1,
            "Create" => 2,
            "Edit" => 3,
            "Delete" => 4,
            "Manage" => 5,
            "Export" => 6,
            "Import" => 7,
            "ViewDashboard" => 8,
            "ViewStudentDetails" => 9,
            "ViewRiskMatrix" => 10,
            "ViewRevenuePrediction" => 11,
            "TriggerNotification" => 12,
            "Activate" => 13,
            "Deactivate" => 14,
            "ViewStatus" => 15,
            "DownloadTemplate" => 16,
            "ViewStatement" => 17,
            "ApplyDiscount" => 18,
            "GenerateInvoices" => 19,
            "ManageBalance" => 20,
            "AccountBalance" => 21,
            "TrialBalance" => 22,
            "IncomeStatement" => 23,
            "BalanceSheet" => 24,
            "CreateManualEntry" => 25,
            "ViewJournalEntry" => 26,
            "CreateOpeningEntry" => 27,
            "ViewAuditLogs" => 28,
            "PostUnpostedEntries" => 29,
            "ManageCapacity" => 30,
            "AssignTeacher" => 31,
            _ => 99
        };
    }

    /// <summary>
    /// الحصول على الاسم العربي للكيان
    /// </summary>
    private string GetEntityArabicName(string entity)
    {
        return entity switch
        {
            "Students" => "الطلاب",
            "Guardians" => "أولياء الأمور",
            "Employees" => "الموظفين",
            "Attendance" => "الحضور",
            "Grades" => "الدرجات",
            "Invoices" => "الفواتير",
            "Financial" => "المالية",
            "Transport" => "النقل",
            "Clinic" => "العيادة",
            "Canteen" => "المقصف",
            "Timetable" => "الجدول الزمني",
            "Behavior" => "السلوك",
            "Reports" => "التقارير",
            "Settings" => "الإعدادات",
            "Chat" => "الدردشة",
            "Admin" => "الإدارة",
            "Classrooms" => "الفصول الدراسية",
            "StudentAccounts" => "حسابات الطلاب",
            "GeneralLedger" => "دفتر الأستاذ",
            "FinancialYearClosing" => "الإغلاق المالي",
            "EarlyWarning" => "الإنذار المبكر",
            "License" => "التراخيص",
            _ => entity
        };
    }

    /// <summary>
    /// الحصول على الاسم العربي للإجراء
    /// </summary>
    private string GetActionArabicName(string action)
    {
        return action switch
        {
            "View" => "عرض",
            "Create" => "إنشاء",
            "Edit" => "تعديل",
            "Delete" => "حذف",
            "Manage" => "إدارة",
            "Export" => "تصدير",
            "Import" => "استيراد",
            "TakeAttendance" => "رصد الحضور",
            "LiveLog" => "السجل الحي",
            "ManualEntry" => "إدخال يدوي",
            "ViewReports" => "عرض التقارير",
            "Promote" => "ترقية",
            "ViewDocuments" => "عرض الوثائق",
            "ManageDocuments" => "إدارة الوثائق",
            "ViewAcademic" => "عرض الأكاديمي",
            "ManageAcademic" => "إدارة الأكاديمي",
            "LinkStudent" => "ربط طالب",
            "UnlinkStudent" => "فك ربط طالب",
            "ViewContact" => "عرض الاتصال",
            "ManageContact" => "إدارة الاتصال",
            "ManageLeaves" => "إدارة الإجازات",
            "ViewSalary" => "عرض الراتب",
            "ManageSalary" => "إدارة الراتب",
            "ViewAttendance" => "عرض الحضور",
            "ManageAttendance" => "إدارة الحضور",
            "ManageLevels" => "إدارة المستويات",
            "AssignClasses" => "تعيين الفصول",
            "Pay" => "دفع",
            "Refund" => "استرداد",
            "ViewHistory" => "عرض السجل",
            "ViewLedger" => "عرض الدفتر",
            "ManageAccounts" => "إدارة الحسابات",
            "CreateJournal" => "إنشاء قيد",
            "EditJournal" => "تعديل قيد",
            "DeleteJournal" => "حذف قيد",
            "ZatcaSubmit" => "إرسال لـ ZATCA",
            "ExportReports" => "تصدير التقارير",
            "ManageBuses" => "إدارة الحافلات",
            "ManageRoutes" => "إدارة المسارات",
            "ManageDrivers" => "إدارة السائقين",
            "AssignStudents" => "تعيين الطلاب",
            "TrackLive" => "تتبع حي",
            "ViewSchedule" => "عرض الجدول",
            "ManageVisits" => "إدارة الزيارات",
            "ManageMedicine" => "إدارة الأدوية",
            "ViewRecords" => "عرض السجلات",
            "CreatePrescription" => "إنشاء وصفة",
            "ManageInsurance" => "إدارة التأمين",
            "ManageItems" => "إدارة العناصر",
            "ManageSales" => "إدارة المبيعات",
            "ManageInventory" => "إدارة المخزون",
            "ManageSuppliers" => "إدارة الموردين",
            "RecordIncident" => "تسجيل حادث",
            "ManageCategories" => "إدارة الفئات",
            "IssueWarning" => "إصدار تحذير",
            "IssueReward" => "إصدار مكافأة",
            "StudentReports" => "تقارير الطلاب",
            "FinancialReports" => "التقارير المالية",
            "AttendanceReports" => "تقارير الحضور",
            "AcademicReports" => "التقارير الأكاديمية",
            "CustomReports" => "تقارير مخصصة",
            "ManageSchool" => "إدارة المدرسة",
            "ManageBranches" => "إدارة الفروع",
            "ManageUsers" => "إدارة المستخدمين",
            "ManageRoles" => "إدارة الأدوار",
            "ManagePermissions" => "إدارة الصلاحيات",
            "SystemConfig" => "إعدادات النظام",
            "BackupRestore" => "النسخ الاحتياطي والاستعادة",
            "SendMessage" => "إرسال رسالة",
            "ManageRooms" => "إدارة الغرف",
            "ManageMembers" => "إدارة الأعضاء",
            "ViewChatHistory" => "عرض سجل الدردشة",
            "ManageTenants" => "إدارة المستأجرين",
            "SystemLogs" => "سجلات النظام",
            "ManageLicenses" => "إدارة التراخيص",
            "SuperAdmin" => "مدير النظام",
            // Classrooms
            "ManageCapacity" => "إدارة السعة",
            "AssignTeacher" => "تعيين معلم",
            // StudentAccounts
            "ViewStatement" => "عرض كشف الحساب",
            "ApplyDiscount" => "تطبيق الخصم",
            "GenerateInvoices" => "إنشاء الفواتير",
            "ManageBalance" => "إدارة الرصيد",
            // GeneralLedger
            "AccountBalance" => "رصيد الحساب",
            "TrialBalance" => "ميزان المراجعة",
            "IncomeStatement" => "قائمة الدخل",
            "BalanceSheet" => "الميزانية العمومية",
            "CreateManualEntry" => "إنشاء قيد يدوي",
            "ViewJournalEntry" => "عرض القيد",
            // FinancialYearClosing
            "CreateOpeningEntry" => "إنشاء قيد افتتاحي",
            "ViewAuditLogs" => "عرض سجلات التدقيق",
            "PostUnpostedEntries" => "ترحيل القيود",
            // EarlyWarning
            "ViewDashboard" => "عرض لوحة التحكم",
            "ViewStudentDetails" => "عرض تفاصيل الطالب",
            "ViewRiskMatrix" => "عرض مصفوفة المخاطر",
            "ViewRevenuePrediction" => "عرض توقعات الإيرادات",
            "TriggerNotification" => "إرسال تنبيه",
            // License
            "Activate" => "تفعيل",
            "Deactivate" => "إلغاء التفعيل",
            "ViewStatus" => "عرض الحالة",
            "DownloadTemplate" => "تنزيل نموذج",
            _ => action
        };
    }

    /// <summary>
    /// التحقق من الحاجة لتحديث الصلاحية
    /// </summary>
    private bool ShouldUpdatePermission(Permission existing, Permission updated)
    {
        return existing.NameArabic != updated.NameArabic ||
               existing.NameEnglish != updated.NameEnglish ||
               existing.Module != updated.Module ||
               existing.Description != updated.Description ||
               existing.DisplayOrder != updated.DisplayOrder;
    }

    /// <summary>
    /// تحديث الصلاحية الموجودة
    /// </summary>
    private void UpdatePermission(Permission existing, Permission updated)
    {
        existing.NameArabic = updated.NameArabic;
        existing.NameEnglish = updated.NameEnglish;
        existing.Module = updated.Module;
        existing.Description = updated.Description;
        existing.DisplayOrder = updated.DisplayOrder;
    }
}
