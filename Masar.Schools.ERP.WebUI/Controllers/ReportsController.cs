using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(MasarDbContext context, ILogger<ReportsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Reports/Index
    [HttpGet]
    public IActionResult Index()
    {
        var reports = new List<ReportDefinition>
        {
            new ReportDefinition
            {
                Id = "financial",
                Name = "التقرير المالي",
                NameArabic = "التقرير المالي",
                Description = "تقرير شامل عن الإيرادات، المصروفات، والتحصيلات",
                Category = "Financial"
            },
            new ReportDefinition
            {
                Id = "academic",
                Name = "التقرير الأكاديمي",
                NameArabic = "التقرير الأكاديمي",
                Description = "تقرير عن الدرجات، الامتحانات، والكنترول",
                Category = "Academic"
            },
            new ReportDefinition
            {
                Id = "attendance",
                Name = "تقرير الحضور والغياب",
                NameArabic = "تقرير الحضور والغياب",
                Description = "إحصائيات الحضور والغياب للطلاب والمعلمين",
                Category = "Attendance"
            },
            new ReportDefinition
            {
                Id = "hr",
                Name = "تقرير الموارد البشرية",
                NameArabic = "تقرير الموارد البشرية",
                Description = "تقرير عن الموظفين، المرتبات، والإجازات",
                Category = "HR"
            },
            new ReportDefinition
            {
                Id = "students",
                Name = "تقرير الطلاب",
                NameArabic = "تقرير الطلاب",
                Description = "إحصائيات شاملة عن الطلاب وأولياء الأمور",
                Category = "Students"
            }
        };

        return View(reports);
    }

    // GET: /Reports/ExportEngine
    [HttpGet]
    public IActionResult ExportEngine()
    {
        return View();
    }

    // GET: /Reports/ExportPdf
    [HttpGet]
    public async Task<IActionResult> ExportPdf(string reportType, string filters)
    {
        // TODO: Implement PDF export using QuestPDF or Rotativa
        var reportData = await GetReportData(reportType, filters);
        
        // Placeholder for PDF generation
        return File(new byte[0], "application/pdf", $"{reportType}_report.pdf");
    }

    // GET: /Reports/ExportExcel
    [HttpGet]
    public async Task<IActionResult> ExportExcel(string reportType, string filters)
    {
        // TODO: Implement Excel export using ClosedXML
        var reportData = await GetReportData(reportType, filters);
        
        // Placeholder for Excel generation
        return File(new byte[0], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{reportType}_report.xlsx");
    }

    private async Task<object> GetReportData(string reportType, string filters)
    {
        switch (reportType.ToLower())
        {
            case "financial":
                return await GetFinancialReport(filters);
            case "academic":
                return await GetAcademicReport(filters);
            case "attendance":
                return await GetAttendanceReport(filters);
            case "hr":
                return await GetHrReport(filters);
            case "students":
                return await GetStudentsReport(filters);
            default:
                throw new ArgumentException("Invalid report type");
        }
    }

    private async Task<object> GetFinancialReport(string filters)
    {
        var invoices = await _context.StudentInvoices
            .Include(i => i.StudentAccount)
                .ThenInclude(sa => sa.Student)
            .Include(i => i.LineItems)
            .Where(i => !i.IsDeleted)
            .ToListAsync();

        return new
        {
            TotalInvoices = invoices.Count,
            TotalAmount = invoices.Sum(i => i.TotalAmount),
            TotalCollected = invoices.Sum(i => i.PaidAmount),
            OutstandingBalance = invoices.Sum(i => i.OutstandingAmount),
            Invoices = invoices
        };
    }

    private async Task<object> GetAcademicReport(string filters)
    {
        var grades = await _context.Grades
            .Include(g => g.Student)
            .Where(g => !g.IsDeleted)
            .ToListAsync();

        return new
        {
            TotalGrades = grades.Count,
            AverageScore = grades.Average(g => g.TotalScore ?? 0),
            Grades = grades
        };
    }

    private async Task<object> GetAttendanceReport(string filters)
    {
        var attendance = await _context.AttendanceRecords
            .Include(a => a.Student)
            .Include(a => a.ClassRoom)
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        var presentCount = attendance.Count(a => a.Status == "Present");
        var absentCount = attendance.Count(a => a.Status == "Absent");

        return new
        {
            TotalRecords = attendance.Count,
            PresentCount = presentCount,
            AbsentCount = absentCount,
            AttendanceRate = attendance.Count > 0 ? (double)presentCount / attendance.Count * 100 : 0,
            Records = attendance
        };
    }

    private async Task<object> GetHrReport(string filters)
    {
        var employees = await _context.Employees
            .Include(e => e.School)
            .Where(e => !e.IsDeleted)
            .ToListAsync();

        return new
        {
            TotalEmployees = employees.Count,
            ActiveEmployees = employees.Count(e => e.IsActive),
            TotalSalary = employees.Sum(e => e.Salary ?? 0),
            Employees = employees
        };
    }

    private async Task<object> GetStudentsReport(string filters)
    {
        var students = await _context.Students
            .Include(s => s.School)
            .Include(s => s.ClassRoom)
            .Include(s => s.Guardian)
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return new
        {
            TotalStudents = students.Count,
            ActiveStudents = students.Count(s => s.IsActive),
            Students = students
        };
    }

    // GET: /Reports/CustomReportBuilder
    [HttpGet]
    public IActionResult CustomReportBuilder()
    {
        var availableTables = GetAvailableTables();
        return View(availableTables);
    }

    // POST: /Reports/GetTableColumns
    [HttpPost]
    public async Task<IActionResult> GetTableColumns([FromBody] TableRequest request)
    {
        try
        {
            var tableName = request?.TableName;
            _logger.LogInformation("GetTableColumns called for table: {TableName}", tableName);
            
            var columns = await GetTableColumnsAsync(tableName);
            _logger.LogInformation("Returning {Count} columns for table {TableName}", columns.Count, tableName);
            
            return Json(columns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTableColumns");
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: /Reports/GenerateCustomReport
    [HttpPost]
    public async Task<IActionResult> GenerateCustomReport([FromBody] CustomReportRequest request)
    {
        try
        {
            var data = await GenerateDynamicReport(request);
            return Json(new { success = true, data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating custom report");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /Reports/SaveCustomReport
    [HttpPost]
    public async Task<IActionResult> SaveCustomReport([FromBody] SavedReportTemplate template)
    {
        try
        {
            // Save to a simple JSON file for now (in production, this should use database)
            var reportsDirectory = Path.Combine(Environment.CurrentDirectory, "SavedReports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }

            template.Id = Guid.NewGuid().ToString();
            template.CreatedAt = DateTime.UtcNow;
            template.CreatedBy = User.Identity?.Name ?? "System";

            var filePath = Path.Combine(reportsDirectory, $"{template.Id}.json");
            var json = System.Text.Json.JsonSerializer.Serialize(template);
            await System.IO.File.WriteAllTextAsync(filePath, json);

            _logger.LogInformation("Saved report template: {ReportName}", template.Name);
            return Json(new { success = true, message = "تم حفظ نموذج التقرير بنجاح", reportId = template.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving custom report template");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // GET: /Reports/SavedReports
    [HttpGet]
    public IActionResult SavedReports()
    {
        var savedReports = new List<SavedReportTemplate>();

        try
        {
            var reportsDirectory = Path.Combine(Environment.CurrentDirectory, "SavedReports");
            if (Directory.Exists(reportsDirectory))
            {
                var files = Directory.GetFiles(reportsDirectory, "*.json");
                foreach (var file in files)
                {
                    try
                    {
                        var json = System.IO.File.ReadAllText(file);
                        var template = System.Text.Json.JsonSerializer.Deserialize<SavedReportTemplate>(json);
                        if (template != null)
                        {
                            savedReports.Add(template);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error loading saved report file: {FilePath}", file);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading saved reports");
        }

        return View(savedReports);
    }

    // POST: /Reports/LoadSavedReport
    [HttpPost]
    public async Task<IActionResult> LoadSavedReport(string reportId)
    {
        try
        {
            var reportsDirectory = Path.Combine(Environment.CurrentDirectory, "SavedReports");
            var filePath = Path.Combine(reportsDirectory, $"{reportId}.json");

            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { success = false, message = "النموذج غير موجود" });
            }

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var template = System.Text.Json.JsonSerializer.Deserialize<SavedReportTemplate>(json);

            _logger.LogInformation("Loaded saved report: {ReportName}", template?.Name);
            return Json(new { success = true, template });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading saved report");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /Reports/DeleteSavedReport
    [HttpPost]
    public async Task<IActionResult> DeleteSavedReport(string reportId)
    {
        try
        {
            var reportsDirectory = Path.Combine(Environment.CurrentDirectory, "SavedReports");
            var filePath = Path.Combine(reportsDirectory, $"{reportId}.json");

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                _logger.LogInformation("Deleted saved report: {ReportId}", reportId);
                return Json(new { success = true, message = "تم حذف النموذج بنجاح" });
            }

            return Json(new { success = false, message = "النموذج غير موجود" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting saved report");
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /Reports/ExportCustomReport
    [HttpPost]
    public async Task<IActionResult> ExportCustomReport([FromBody] CustomReportRequest request, string format)
    {
        try
        {
            var data = await GenerateDynamicReport(request);

            switch (format.ToLower())
            {
                case "pdf":
                    // TODO: Implement PDF export using QuestPDF or similar library
                    return File(new byte[0], "application/pdf", "custom_report.pdf");
                case "excel":
                    // TODO: Implement Excel export using ClosedXML or similar library
                    return File(new byte[0], "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "custom_report.xlsx");
                case "print":
                    return View("PrintReport", data);
                default:
                    return BadRequest("Invalid format");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting custom report");
            return BadRequest(ex.Message);
        }
    }

    private List<TableDefinition> GetAvailableTables()
    {
        return new List<TableDefinition>
        {
            // الطلاب والفصول
            new TableDefinition { Name = "Students", DisplayName = "الطلاب", DisplayNameArabic = "الطلاب" },
            new TableDefinition { Name = "Guardians", DisplayName = "أولياء الأمور", DisplayNameArabic = "أولياء الأمور" },
            new TableDefinition { Name = "ClassRooms", DisplayName = "الفصول", DisplayNameArabic = "الفصول" },
            new TableDefinition { Name = "GradeLevels", DisplayName = "المراحل الدراسية", DisplayNameArabic = "المراحل الدراسية" },
            new TableDefinition { Name = "Sections", DisplayName = "الأقسام", DisplayNameArabic = "الأقسام" },
            
            // الموظفين والموارد البشرية
            new TableDefinition { Name = "Employees", DisplayName = "الموظفين", DisplayNameArabic = "الموظفين" },
            new TableDefinition { Name = "LeaveRequests", DisplayName = "طلبات الإجازة", DisplayNameArabic = "طلبات الإجازة" },
            new TableDefinition { Name = "LeaveBalances", DisplayName = "رصيد الإجازات", DisplayNameArabic = "رصيد الإجازات" },
            new TableDefinition { Name = "Contracts", DisplayName = "العقود", DisplayNameArabic = "العقود" },
            
            // المالية
            new TableDefinition { Name = "Invoices", DisplayName = "الفواتير", DisplayNameArabic = "الفواتير" },
            new TableDefinition { Name = "InvoicePayments", DisplayName = "مدفوعات الفواتير", DisplayNameArabic = "مدفوعات الفواتير" },
            new TableDefinition { Name = "StudentPayments", DisplayName = "مدفوعات الطلاب", DisplayNameArabic = "مدفوعات الطلاب" },
            new TableDefinition { Name = "Accounts", DisplayName = "الحسابات", DisplayNameArabic = "الحسابات" },
            new TableDefinition { Name = "JournalEntries", DisplayName = "القيود المحاسبية", DisplayNameArabic = "القيود المحاسبية" },
            new TableDefinition { Name = "Discounts", DisplayName = "الخصومات", DisplayNameArabic = "الخصومات" },
            
            // الأكاديمي
            new TableDefinition { Name = "Grades", DisplayName = "الدرجات", DisplayNameArabic = "الدرجات" },
            new TableDefinition { Name = "AttendanceRecords", DisplayName = "سجلات الحضور", DisplayNameArabic = "سجلات الحضور" },
            new TableDefinition { Name = "AcademicTerms", DisplayName = "الفصول الدراسية", DisplayNameArabic = "الفصول الدراسية" },
            new TableDefinition { Name = "AcademicRecords", DisplayName = "السجلات الأكاديمية", DisplayNameArabic = "السجلات الأكاديمية" },
            
            // المخزون
            new TableDefinition { Name = "InventoryItems", DisplayName = "الأصناف", DisplayNameArabic = "الأصناف" },
            new TableDefinition { Name = "StockTransactions", DisplayName = "حركات المخزون", DisplayNameArabic = "حركات المخزون" },
            new TableDefinition { Name = "Warehouses", DisplayName = "المستودعات", DisplayNameArabic = "المستودعات" },
            new TableDefinition { Name = "Suppliers", DisplayName = "الموردين", DisplayNameArabic = "الموردين" },
            new TableDefinition { Name = "PurchaseOrders", DisplayName = "أوامر الشراء", DisplayNameArabic = "أوامر الشراء" },
            new TableDefinition { Name = "PurchaseInvoices", DisplayName = "فواتير الشراء", DisplayNameArabic = "فواتير الشراء" },
            
            // الكافيتريا
            new TableDefinition { Name = "CanteenProducts", DisplayName = "منتجات الكافيتريا", DisplayNameArabic = "منتجات الكافيتريا" },
            new TableDefinition { Name = "CanteenOrders", DisplayName = "طلبات الكافيتريا", DisplayNameArabic = "طلبات الكافيتريا" },
            new TableDefinition { Name = "CashierShifts", DisplayName = "ورديات الكاشير", DisplayNameArabic = "ورديات الكاشير" },
            new TableDefinition { Name = "CanteenTables", DisplayName = "طاولات الكافيتريا", DisplayNameArabic = "طاولات الكافيتريا" },
            
            // النقل
            new TableDefinition { Name = "Buses", DisplayName = "الحافلات", DisplayNameArabic = "الحافلات" },
            new TableDefinition { Name = "BusRoutes", DisplayName = "مسارات الحافلات", DisplayNameArabic = "مسارات الحافلات" },
            new TableDefinition { Name = "Drivers", DisplayName = "السائقين", DisplayNameArabic = "السائقين" },
            new TableDefinition { Name = "StudentTransportSubscriptions", DisplayName = "اشتراكات النقل", DisplayNameArabic = "اشتراكات النقل" },
            new TableDefinition { Name = "BusAttendances", DisplayName = "حضور الحافلات", DisplayNameArabic = "حضور الحافلات" },
            
            // الطب والعيادة
            new TableDefinition { Name = "StudentHealthProfiles", DisplayName = "الملفات الصحية", DisplayNameArabic = "الملفات الصحية" },
            new TableDefinition { Name = "ClinicVisits", DisplayName = "زيارات العيادة", DisplayNameArabic = "زيارات العيادة" },
            new TableDefinition { Name = "MedicalItems", DisplayName = "الأدوية والمستلزمات", DisplayNameArabic = "الأدوية والمستلزمات" },
            
            // التسجيل والقبول
            new TableDefinition { Name = "AdmissionApplications", DisplayName = "طلبات القبول", DisplayNameArabic = "طلبات القبول" },
            new TableDefinition { Name = "AdmissionExams", DisplayName = "امتحانات القبول", DisplayNameArabic = "امتحانات القبول" },
            
            // الخريجين
            new TableDefinition { Name = "AlumniRecords", DisplayName = "سجلات الخريجين", DisplayNameArabic = "سجلات الخريجين" },
            
            // النظام والإدارة
            new TableDefinition { Name = "Schools", DisplayName = "المدارس", DisplayNameArabic = "المدارس" },
            new TableDefinition { Name = "Branches", DisplayName = "الفروع", DisplayNameArabic = "الفروع" },
            new TableDefinition { Name = "MasarUsers", DisplayName = "المستخدمين", DisplayNameArabic = "المستخدمين" },
            new TableDefinition { Name = "MasarRoles", DisplayName = "الأدوار", DisplayNameArabic = "الأدوار" },
            new TableDefinition { Name = "AuditLogs", DisplayName = "سجلات التدقيق", DisplayNameArabic = "سجلات التدقيق" }
        };
    }

    private async Task<List<ColumnDefinition>> GetTableColumnsAsync(string tableName)
    {
        try
        {
            _logger.LogInformation("Getting columns for table: {TableName}", tableName);
            
            // استخدام Reflection للحصول على خصائص الـ Entity تلقائياً
            var entityType = _context.Model.FindEntityType(tableName);
            
            // إذا لم يتم العثور، حاول البحث حسب CLR type
            if (entityType == null)
            {
                entityType = _context.Model.GetEntityTypes()
                    .FirstOrDefault(e => e.ClrType.Name == tableName || e.ClrType.Name == tableName + "Entity");
            }
            
            if (entityType == null)
            {
                _logger.LogWarning("Entity type not found for table: {TableName}. Available entities: {Entities}", 
                    tableName, string.Join(", ", _context.Model.GetEntityTypes().Select(e => e.ClrType.Name)));
                return GetDefaultColumns();
            }

            var columns = new List<ColumnDefinition>();
            
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.Name;
                var columnType = GetPropertyType(property.ClrType);
                
                _logger.LogInformation("Found property: {Name} of type {Type}", columnName, columnType);
                
                columns.Add(new ColumnDefinition
                {
                    Name = columnName,
                    DisplayName = columnName,
                    DisplayNameArabic = columnName,
                    DataType = columnType
                });
            }

            _logger.LogInformation("Returning {Count} columns for table {TableName}", columns.Count, tableName);
            return await Task.FromResult(columns);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting columns for table: {TableName}", tableName);
            return GetDefaultColumns();
        }
    }

    private List<ColumnDefinition> GetDefaultColumns()
    {
        return new List<ColumnDefinition>
        {
            new ColumnDefinition { Name = "Id", DisplayName = "ID", DisplayNameArabic = "المعرف", DataType = "Guid" },
            new ColumnDefinition { Name = "CreatedAt", DisplayName = "Created At", DisplayNameArabic = "تاريخ الإنشاء", DataType = "DateTime" },
            new ColumnDefinition { Name = "UpdatedAt", DisplayName = "Updated At", DisplayNameArabic = "تاريخ التحديث", DataType = "DateTime" }
        };
    }

    private string GetPropertyType(Type type)
    {
        if (type == typeof(Guid) || type == typeof(Guid?))
            return "Guid";
        if (type == typeof(int) || type == typeof(int?) || type == typeof(long) || type == typeof(long?))
            return "Int";
        if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(double) || type == typeof(double?))
            return "Decimal";
        if (type == typeof(bool) || type == typeof(bool?))
            return "Boolean";
        if (type == typeof(DateTime) || type == typeof(DateTime?))
            return "DateTime";
        if (type == typeof(string))
            return "String";
        return "String";
    }

    private async Task<object> GenerateDynamicReport(CustomReportRequest request)
    {
        var query = BuildDynamicQuery(request);
        var results = await query.ToListAsync();

        return new
        {
            TableName = request.TableName,
            SelectedColumns = request.SelectedColumns,
            Filters = request.Filters,
            Results = results,
            TotalRecords = results.Count
        };
    }

    private IQueryable<object> BuildDynamicQuery(CustomReportRequest request)
    {
        try
        {
            _logger.LogInformation("Building dynamic query for table: {TableName}", request.TableName);
            
            // الحصول على الـ DbSet ديناميكياً باستخدام Reflection
            var dbSetProperty = _context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType && 
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    (p.PropertyType.GetGenericArguments()[0].Name == request.TableName || 
                     p.PropertyType.GetGenericArguments()[0].Name == request.TableName + "Entity"));

            if (dbSetProperty == null)
            {
                _logger.LogWarning("DbSet not found for table: {TableName}. Available DbSets: {DbSets}", 
                    request.TableName, string.Join(", ", _context.GetType().GetProperties()
                        .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                        .Select(p => p.PropertyType.GetGenericArguments()[0].Name)));
                throw new ArgumentException($"Table '{request.TableName}' not found in context");
            }

            var dbSet = dbSetProperty.GetValue(_context) as IQueryable;
            if (dbSet == null)
            {
                throw new ArgumentException($"Could not get DbSet for table '{request.TableName}'");
            }

            // تطبيق فلتر IsDeleted إذا وجد
            var entityType = dbSet.ElementType;
            var isDeletedProperty = entityType.GetProperty("IsDeleted");
            if (isDeletedProperty != null)
            {
                // بناء expression للـ Where
                var parameter = Expression.Parameter(entityType, "x");
                var isDeletedAccess = Expression.Property(parameter, isDeletedProperty);
                var isDeletedFalse = Expression.Constant(false);
                var comparison = Expression.Equal(isDeletedAccess, isDeletedFalse);
                var whereLambda = Expression.Lambda(comparison, parameter);
                var whereMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(entityType);
                dbSet = whereMethod.Invoke(null, new object[] { dbSet, whereLambda }) as IQueryable;
            }

            // تطبيق الفلاتر
            if (request.Filters != null && request.Filters.Any())
            {
                var dbSetObject = dbSet.Cast<object>();
                dbSetObject = ApplyDynamicFilters(dbSetObject, request);
                return dbSetObject;
            }

            return dbSet.Cast<object>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error building dynamic query for table: {TableName}", request.TableName);
            throw;
        }
    }

    private IQueryable<object> ApplyDynamicFilters(IQueryable<object> query, CustomReportRequest request)
    {
        if (request.Filters == null || !request.Filters.Any())
            return query;

        // Convert to dynamic query with filters
        var parameter = Expression.Parameter(typeof(object), "x");
        var filterExpression = BuildFilterExpression(parameter, request);

        if (filterExpression != null)
        {
            var lambda = Expression.Lambda<Func<object, bool>>(filterExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private Expression BuildFilterExpression(ParameterExpression parameter, CustomReportRequest request)
    {
        Expression? combinedExpression = null;

        foreach (var filter in request.Filters)
        {
            var propertyExpression = Expression.PropertyOrField(parameter, filter.ColumnName);
            var constantExpression = Expression.Constant(ConvertValue(filter.Value, GetPropertyType(filter.ColumnName)));

            Expression? filterExpression = null;

            switch (filter.Operator.ToLower())
            {
                case "equals":
                    filterExpression = Expression.Equal(propertyExpression, constantExpression);
                    break;
                case "notequals":
                    filterExpression = Expression.NotEqual(propertyExpression, constantExpression);
                    break;
                case "contains":
                    filterExpression = Expression.Call(
                        propertyExpression,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        constantExpression);
                    break;
                case "notcontains":
                    var containsExpression = Expression.Call(
                        propertyExpression,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        constantExpression);
                    filterExpression = Expression.Not(containsExpression);
                    break;
                case "startswith":
                    filterExpression = Expression.Call(
                        propertyExpression,
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        constantExpression);
                    break;
                case "endswith":
                    filterExpression = Expression.Call(
                        propertyExpression,
                        typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        constantExpression);
                    break;
                case "greaterthan":
                    filterExpression = Expression.GreaterThan(propertyExpression, constantExpression);
                    break;
                case "lessthan":
                    filterExpression = Expression.LessThan(propertyExpression, constantExpression);
                    break;
                case "between":
                    var constantExpression2 = Expression.Constant(ConvertValue(filter.Value2, GetPropertyType(filter.ColumnName)));
                    var greaterThanExpression = Expression.GreaterThanOrEqual(propertyExpression, constantExpression);
                    var lessThanExpression = Expression.LessThanOrEqual(propertyExpression, constantExpression2);
                    filterExpression = Expression.AndAlso(greaterThanExpression, lessThanExpression);
                    break;
            }

            if (filterExpression != null)
            {
                if (combinedExpression == null)
                {
                    combinedExpression = filterExpression;
                }
                else
                {
                    combinedExpression = Expression.AndAlso(combinedExpression, filterExpression);
                }
            }
        }

        return combinedExpression;
    }

    private Type GetPropertyType(string propertyName)
    {
        // Simplified type mapping - in production, this should use reflection
        var typeMappings = new Dictionary<string, Type>
        {
            { "Id", typeof(Guid) },
            { "FirstName", typeof(string) },
            { "LastName", typeof(string) },
            { "FirstNameArabic", typeof(string) },
            { "LastNameArabic", typeof(string) },
            { "NationalId", typeof(string) },
            { "PhoneNumber", typeof(string) },
            { "WhatsAppNumber", typeof(string) },
            { "Gender", typeof(string) },
            { "BirthDate", typeof(DateTime) },
            { "EnrollmentDate", typeof(DateTime) },
            { "HireDate", typeof(DateTime) },
            { "Salary", typeof(decimal) },
            { "TotalAmount", typeof(decimal) },
            { "PaidAmount", typeof(decimal) },
            { "OutstandingAmount", typeof(decimal) },
            { "Quantity", typeof(int) },
            { "UnitPrice", typeof(decimal) },
            { "TotalScore", typeof(decimal) },
            { "Percentage", typeof(decimal) },
            { "IsActive", typeof(bool) },
            { "IsLowStock", typeof(bool) },
            { "Status", typeof(string) },
            { "Position", typeof(string) },
            { "Relationship", typeof(string) },
            { "Category", typeof(string) },
            { "Grade", typeof(string) },
            { "Term", typeof(string) }
        };

        return typeMappings.TryGetValue(propertyName, out var type) ? type : typeof(string);
    }

    private object ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        try
        {
            if (targetType == typeof(Guid))
                return Guid.Parse(value);
            if (targetType == typeof(int) || targetType == typeof(long))
                return int.Parse(value);
            if (targetType == typeof(decimal) || targetType == typeof(double))
                return decimal.Parse(value);
            if (targetType == typeof(bool))
                return bool.Parse(value);
            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);
            return value;
        }
        catch
        {
            return value; // Return as string if conversion fails
        }
    }
}

public class TableDefinition
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DisplayNameArabic { get; set; } = string.Empty;
}

public class ColumnDefinition
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DisplayNameArabic { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
}

public class CustomReportRequest
{
    public string TableName { get; set; } = string.Empty;
    public List<string> SelectedColumns { get; set; } = new();
    public List<ReportFilter> Filters { get; set; } = new();
    public string ReportName { get; set; } = string.Empty;
}

public class TableRequest
{
    public string TableName { get; set; } = string.Empty;
}

public class ReportFilter
{
    public string ColumnName { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty; // Equals, GreaterThan, LessThan, Contains, Between, etc.
    public string Value { get; set; } = string.Empty;
    public string Value2 { get; set; } = string.Empty; // For Between operator
}

public class SavedReportTemplate
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public List<string> SelectedColumns { get; set; } = new();
    public List<ReportFilter> Filters { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class ReportDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameArabic { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
