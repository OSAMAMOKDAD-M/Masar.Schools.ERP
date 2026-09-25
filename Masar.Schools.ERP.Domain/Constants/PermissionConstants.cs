namespace Masar.Schools.ERP.Domain.Constants;

/// <summary>
/// ثوابت الصلاحيات الشجري لنظام مَسَار للمدارس
/// تحتوي على جميع الموديولات والشاشات والإجراءات المسموح بها
/// </summary>
public static class PermissionConstants
{
    // Students Module
    public static class Students
    {
        public const string View = "Students.View";
        public const string Create = "Students.Create";
        public const string Edit = "Students.Edit";
        public const string Delete = "Students.Delete";
        public const string Promote = "Students.Promote";
        public const string ViewDocuments = "Students.ViewDocuments";
        public const string ManageDocuments = "Students.ManageDocuments";
        public const string ViewAcademic = "Students.ViewAcademic";
        public const string ManageAcademic = "Students.ManageAcademic";
        public const string Export = "Students.Export";
        public const string Import = "Students.Import";
    }

    // Guardians Module
    public static class Guardians
    {
        public const string View = "Guardians.View";
        public const string Create = "Guardians.Create";
        public const string Edit = "Guardians.Edit";
        public const string Delete = "Guardians.Delete";
        public const string LinkStudent = "Guardians.LinkStudent";
        public const string UnlinkStudent = "Guardians.UnlinkStudent";
        public const string ViewContact = "Guardians.ViewContact";
        public const string ManageContact = "Guardians.ManageContact";
    }

    // Employees Module
    public static class Employees
    {
        public const string View = "Employees.View";
        public const string Create = "Employees.Create";
        public const string Edit = "Employees.Edit";
        public const string Delete = "Employees.Delete";
        public const string ManageLeaves = "Employees.ManageLeaves";
        public const string ViewSalary = "Employees.ViewSalary";
        public const string ManageSalary = "Employees.ManageSalary";
        public const string ViewAttendance = "Employees.ViewAttendance";
        public const string ManageAttendance = "Employees.ManageAttendance";
        public const string Export = "Employees.Export";
    }

    // HR Module (Advanced Human Resources)
    public static class HR
    {
        public const string Dashboard = "HR.Dashboard";
        public const string EmployeesView = "HR.EmployeesView";
        public const string EmployeesCreate = "HR.EmployeesCreate";
        public const string EmployeesEdit = "HR.EmployeesEdit";
        public const string EmployeesDelete = "HR.EmployeesDelete";
        public const string ContractsView = "HR.ContractsView";
        public const string ContractsCreate = "HR.ContractsCreate";
        public const string ContractsEdit = "HR.ContractsEdit";
        public const string ContractsDelete = "HR.ContractsDelete";
        public const string ContractsRenew = "HR.ContractsRenew";
        public const string ContractsArchive = "HR.ContractsArchive";
        public const string LeavesView = "HR.LeavesView";
        public const string LeavesCreate = "HR.LeavesCreate";
        public const string LeavesEdit = "HR.LeavesEdit";
        public const string LeavesDelete = "HR.LeavesDelete";
        public const string LeavesApprove = "HR.LeavesApprove";
        public const string LeavesReject = "HR.LeavesReject";
        public const string LeaveBalanceView = "HR.LeaveBalanceView";
        public const string LeaveLedger = "HR.LeaveLedger";
        public const string PayrollView = "HR.PayrollView";
        public const string PayrollCreate = "HR.PayrollCreate";
        public const string PayrollEdit = "HR.PayrollEdit";
        public const string PayrollDelete = "HR.PayrollDelete";
        public const string PayrollApprove = "HR.PayrollApprove";
        public const string PayrollArchive = "HR.PayrollArchive";
        public const string PayrollExport = "HR.PayrollExport";
        public const string AttendanceView = "HR.AttendanceView";
        public const string AttendanceImport = "HR.AttendanceImport";
        public const string AttendanceEdit = "HR.AttendanceEdit";
        public const string AttendanceDelete = "HR.AttendanceDelete";
        public const string PermitsView = "HR.PermitsView";
        public const string PermitsCreate = "HR.PermitsCreate";
        public const string PermitsEdit = "HR.PermitsEdit";
        public const string PermitsDelete = "HR.PermitsDelete";
        public const string GratuityView = "HR.GratuityView";
        public const string GratuityCalculate = "HR.GratuityCalculate";
        public const string GratuityGenerateLetter = "HR.GratuityGenerateLetter";
        public const string ResignedView = "HR.ResignedView";
        public const string ResignedCreate = "HR.ResignedCreate";
        public const string ReportsView = "HR.ReportsView";
        public const string ReportsExport = "HR.ReportsExport";
        public const string SettingsManage = "HR.SettingsManage";
        public const string NationalitiesManage = "HR.NationalitiesManage";
        public const string LeaveTypesManage = "HR.LeaveTypesManage";
    }

    // Attendance Module
    public static class Attendance
    {
        public const string View = "Attendance.View";
        public const string TakeAttendance = "Attendance.TakeAttendance";
        public const string LiveLog = "Attendance.LiveLog";
        public const string ManualEntry = "Attendance.ManualEntry";
        public const string EditAttendance = "Attendance.EditAttendance";
        public const string Export = "Attendance.Export";
        public const string ViewReports = "Attendance.ViewReports";
        public const string ManageNotifications = "Attendance.ManageNotifications";
    }

    // Grades Module
    public static class Grades
    {
        public const string View = "Grades.View";
        public const string Create = "Grades.Create";
        public const string Edit = "Grades.Edit";
        public const string Delete = "Grades.Delete";
        public const string ManageLevels = "Grades.ManageLevels";
        public const string AssignClasses = "Grades.AssignClasses";
    }

    // Invoices Module
    public static class Invoices
    {
        public const string View = "Invoices.View";
        public const string Create = "Invoices.Create";
        public const string Edit = "Invoices.Edit";
        public const string Delete = "Invoices.Delete";
        public const string Pay = "Invoices.Pay";
        public const string Refund = "Invoices.Refund";
        public const string Export = "Invoices.Export";
        public const string ViewHistory = "Invoices.ViewHistory";
    }

    // Financial Module
    public static class Financial
    {
        public const string View = "Financial.View";
        public const string ViewLedger = "Financial.ViewLedger";
        public const string ManageAccounts = "Financial.ManageAccounts";
        public const string CreateJournal = "Financial.CreateJournal";
        public const string EditJournal = "Financial.EditJournal";
        public const string DeleteJournal = "Financial.DeleteJournal";
        public const string ViewReports = "Financial.ViewReports";
        public const string ZatcaSubmit = "Financial.ZatcaSubmit";
        public const string ExportReports = "Financial.ExportReports";
        public const string OverrideClosedPeriods = "Financial.OverrideClosedPeriods";
        public const string ClosePeriod = "Financial.ClosePeriod";
        public const string ReopenPeriod = "Financial.ReopenPeriod";
    }

    // Transport Module
    public static class Transport
    {
        public const string View = "Transport.View";
        public const string ManageBuses = "Transport.ManageBuses";
        public const string ManageRoutes = "Transport.ManageRoutes";
        public const string ManageDrivers = "Transport.ManageDrivers";
        public const string AssignStudents = "Transport.AssignStudents";
        public const string TrackLive = "Transport.TrackLive";
        public const string ViewSchedule = "Transport.ViewSchedule";
    }

    // Clinic Module
    public static class Clinic
    {
        public const string View = "Clinic.View";
        public const string Dashboard = "Clinic.Dashboard";
        public const string QuickVisit = "Clinic.QuickVisit";
        public const string ManageVisits = "Clinic.ManageVisits";
        public const string ViewVisits = "Clinic.ViewVisits";
        public const string EditVisit = "Clinic.EditVisit";
        public const string DeleteVisit = "Clinic.DeleteVisit";
        public const string HealthProfiles = "Clinic.HealthProfiles";
        public const string ViewHealthProfile = "Clinic.ViewHealthProfile";
        public const string EditHealthProfile = "Clinic.EditHealthProfile";
        public const string MedicalInventory = "Clinic.MedicalInventory";
        public const string ManageMedicine = "Clinic.ManageMedicine";
        public const string DispenseMedicine = "Clinic.DispenseMedicine";
        public const string ViewRecords = "Clinic.ViewRecords";
        public const string CreatePrescription = "Clinic.CreatePrescription";
        public const string ManageInsurance = "Clinic.ManageInsurance";
        public const string MedicalExcuses = "Clinic.MedicalExcuses";
        public const string CreateExcuse = "Clinic.CreateExcuse";
        public const string EditExcuse = "Clinic.EditExcuse";
        public const string SendExcuseToTeachers = "Clinic.SendExcuseToTeachers";
        public const string ViewReports = "Clinic.ViewReports";
    }

    // Alumni Management Module
    public static class Alumni
    {
        public const string View = "Alumni.View";
        public const string Dashboard = "Alumni.Dashboard";
        public const string ViewDirectory = "Alumni.ViewDirectory";
        public const string ManageRecords = "Alumni.ManageRecords";
        public const string EditRecord = "Alumni.EditRecord";
        public const string DeleteRecord = "Alumni.DeleteRecord";
        public const string GraduateStudent = "Alumni.GraduateStudent";
        public const string ProcessGraduation = "Alumni.ProcessGraduation";
        public const string ManageClearance = "Alumni.ManageClearance";
        public const string ApproveClearance = "Alumni.ApproveClearance";
        public const string ViewClearance = "Alumni.ViewClearance";
        public const string ManageDocuments = "Alumni.ManageDocuments";
        public const string IssueCertificates = "Alumni.IssueCertificates";
        public const string ViewReports = "Alumni.ViewReports";
        public const string TrackCareer = "Alumni.TrackCareer";
        public const string Networking = "Alumni.Networking";
    }

    // Admissions Module
    public static class Admissions
    {
        public const string View = "Admissions.View";
        public const string Dashboard = "Admissions.Dashboard";
        public const string ViewApplications = "Admissions.ViewApplications";
        public const string CreateApplication = "Admissions.CreateApplication";
        public const string EditApplication = "Admissions.EditApplication";
        public const string DeleteApplication = "Admissions.DeleteApplication";
        public const string ReviewApplication = "Admissions.ReviewApplication";
        public const string ApproveApplication = "Admissions.ApproveApplication";
        public const string RejectApplication = "Admissions.RejectApplication";
        public const string CreateExam = "Admissions.CreateExam";
        public const string EditExam = "Admissions.EditExam";
        public const string ViewExam = "Admissions.ViewExam";
        public const string ConvertToStudent = "Admissions.ConvertToStudent";
        public const string ViewDocuments = "Admissions.ViewDocuments";
        public const string ManageDocuments = "Admissions.ManageDocuments";
    }

    // Canteen Module
    public static class Canteen
    {
        public const string View = "Canteen.View";
        public const string ManageItems = "Canteen.ManageItems";
        public const string ManageSales = "Canteen.ManageSales";
        public const string ManageInventory = "Canteen.ManageInventory";
        public const string ViewReports = "Canteen.ViewReports";
        public const string ManageSuppliers = "Canteen.ManageSuppliers";
    }

    // Inventory Management Module
    public static class Inventory
    {
        public const string View = "Inventory.View";
        public const string ManageWarehouses = "Inventory.ManageWarehouses";
        public const string ManageItems = "Inventory.ManageItems";
        public const string ManageTransactions = "Inventory.ManageTransactions";
        public const string ManageAdjustments = "Inventory.ManageAdjustments";
        public const string ViewReports = "Inventory.ViewReports";
        public const string ManageTransfers = "Inventory.ManageTransfers";
    }

    // Procurement Management Module
    public static class Procurement
    {
        public const string View = "Procurement.View";
        public const string ManageSuppliers = "Procurement.ManageSuppliers";
        public const string ManagePurchaseOrders = "Procurement.ManagePurchaseOrders";
        public const string ManagePurchaseInvoices = "Procurement.ManagePurchaseInvoices";
        public const string ManagePurchaseRequests = "Procurement.ManagePurchaseRequests";
        public const string ApproveOrders = "Procurement.ApproveOrders";
        public const string ViewReports = "Procurement.ViewReports";
    }

    // Timetable Module
    public static class Timetable
    {
        public const string View = "Timetable.View";
        public const string Create = "Timetable.Create";
        public const string Edit = "Timetable.Edit";
        public const string Delete = "Timetable.Delete";
        public const string ManageClasses = "Timetable.ManageClasses";
        public const string ManageSubjects = "Timetable.ManageSubjects";
        public const string AssignTeachers = "Timetable.AssignTeachers";
        public const string ViewSchedule = "Timetable.ViewSchedule";
    }

    // Behavior Module
    public static class Behavior
    {
        public const string View = "Behavior.View";
        public const string RecordIncident = "Behavior.RecordIncident";
        public const string ViewRecords = "Behavior.ViewRecords";
        public const string ManageCategories = "Behavior.ManageCategories";
        public const string IssueWarning = "Behavior.IssueWarning";
        public const string IssueReward = "Behavior.IssueReward";
        public const string ViewReports = "Behavior.ViewReports";
    }

    // Reports Module
    public static class Reports
    {
        public const string View = "Reports.View";
        public const string StudentReports = "Reports.StudentReports";
        public const string FinancialReports = "Reports.FinancialReports";
        public const string AttendanceReports = "Reports.AttendanceReports";
        public const string AcademicReports = "Reports.AcademicReports";
        public const string CustomReports = "Reports.CustomReports";
        public const string ExportReports = "Reports.ExportReports";
    }

    // Settings Module
    public static class Settings
    {
        public const string View = "Settings.View";
        public const string ManageSchool = "Settings.ManageSchool";
        public const string ManageBranches = "Settings.ManageBranches";
        public const string ManageUsers = "Settings.ManageUsers";
        public const string ManageRoles = "Settings.ManageRoles";
        public const string ManagePermissions = "Settings.ManagePermissions";
        public const string SystemConfig = "Settings.SystemConfig";
        public const string BackupRestore = "Settings.BackupRestore";
    }

    // Chat Module
    public static class Chat
    {
        public const string View = "Chat.View";
        public const string SendMessage = "Chat.SendMessage";
        public const string ManageRooms = "Chat.ManageRooms";
        public const string ManageMembers = "Chat.ManageMembers";
        public const string ViewHistory = "Chat.ViewHistory";
    }

    // Admin Module
    public static class Admin
    {
        public const string View = "Admin.View";
        public const string ManageTenants = "Admin.ManageTenants";
        public const string SystemLogs = "Admin.SystemLogs";
        public const string ManageLicenses = "Admin.ManageLicenses";
        public const string SuperAdmin = "Admin.SuperAdmin";
    }

    // Classrooms Module
    public static class Classrooms
    {
        public const string View = "Classrooms.View";
        public const string Create = "Classrooms.Create";
        public const string Edit = "Classrooms.Edit";
        public const string Delete = "Classrooms.Delete";
        public const string ManageCapacity = "Classrooms.ManageCapacity";
        public const string AssignTeacher = "Classrooms.AssignTeacher";
    }

    // StudentAccounts Module
    public static class StudentAccounts
    {
        public const string View = "StudentAccounts.View";
        public const string Create = "StudentAccounts.Create";
        public const string Edit = "StudentAccounts.Edit";
        public const string ViewStatement = "StudentAccounts.ViewStatement";
        public const string ApplyDiscount = "StudentAccounts.ApplyDiscount";
        public const string GenerateInvoices = "StudentAccounts.GenerateInvoices";
        public const string ManageBalance = "StudentAccounts.ManageBalance";
    }

    // GeneralLedger Module
    public static class GeneralLedger
    {
        public const string View = "GeneralLedger.View";
        public const string ViewLedger = "GeneralLedger.ViewLedger";
        public const string AccountBalance = "GeneralLedger.AccountBalance";
        public const string TrialBalance = "GeneralLedger.TrialBalance";
        public const string IncomeStatement = "GeneralLedger.IncomeStatement";
        public const string BalanceSheet = "GeneralLedger.BalanceSheet";
        public const string CreateManualEntry = "GeneralLedger.CreateManualEntry";
        public const string ViewJournalEntry = "GeneralLedger.ViewJournalEntry";
    }

    // FinancialYearClosing Module
    public static class FinancialYearClosing
    {
        public const string View = "FinancialYearClosing.View";
        public const string ClosePeriod = "Financial.ClosePeriod";
        public const string ReopenPeriod = "Financial.ReopenPeriod";
        public const string CreateOpeningEntry = "FinancialYearClosing.CreateOpeningEntry";
        public const string ViewAuditLogs = "FinancialYearClosing.ViewAuditLogs";
        public const string PostUnpostedEntries = "FinancialYearClosing.PostUnpostedEntries";
    }

    // EarlyWarning Module (AI-Powered)
    public static class EarlyWarning
    {
        public const string View = "EarlyWarning.View";
        public const string ViewDashboard = "EarlyWarning.ViewDashboard";
        public const string ViewStudentDetails = "EarlyWarning.ViewStudentDetails";
        public const string ViewRiskMatrix = "EarlyWarning.ViewRiskMatrix";
        public const string ViewRevenuePrediction = "EarlyWarning.ViewRevenuePrediction";
        public const string TriggerNotification = "EarlyWarning.TriggerNotification";
    }

    // License Module (Client-Facing)
    public static class License
    {
        public const string View = "License.View";
        public const string Activate = "License.Activate";
        public const string Deactivate = "License.Deactivate";
        public const string ViewStatus = "License.ViewStatus";
        public const string DownloadTemplate = "License.DownloadTemplate";
    }

    /// <summary>
    /// الحصول على جميع الصلاحيات في النظام
    /// </summary>
    public static readonly string[] AllPermissions = GetAllPermissions();

    /// <summary>
    /// الحصول على جميع الصلاحيات مرتبة حسب الموديول
    /// </summary>
    public static Dictionary<string, string[]> GetPermissionsByModule()
    {
        return new Dictionary<string, string[]>
        {
            ["Students"] = new[] { Students.View, Students.Create, Students.Edit, Students.Delete, Students.Promote, Students.ViewDocuments, Students.ManageDocuments, Students.ViewAcademic, Students.ManageAcademic, Students.Export, Students.Import },
            ["Guardians"] = new[] { Guardians.View, Guardians.Create, Guardians.Edit, Guardians.Delete, Guardians.LinkStudent, Guardians.UnlinkStudent, Guardians.ViewContact, Guardians.ManageContact },
            ["Employees"] = new[] { Employees.View, Employees.Create, Employees.Edit, Employees.Delete, Employees.ManageLeaves, Employees.ViewSalary, Employees.ManageSalary, Employees.ViewAttendance, Employees.ManageAttendance, Employees.Export },
            ["Attendance"] = new[] { Attendance.View, Attendance.TakeAttendance, Attendance.LiveLog, Attendance.ManualEntry, Attendance.EditAttendance, Attendance.Export, Attendance.ViewReports, Attendance.ManageNotifications },
            ["Grades"] = new[] { Grades.View, Grades.Create, Grades.Edit, Grades.Delete, Grades.ManageLevels, Grades.AssignClasses },
            ["Invoices"] = new[] { Invoices.View, Invoices.Create, Invoices.Edit, Invoices.Delete, Invoices.Pay, Invoices.Refund, Invoices.Export, Invoices.ViewHistory },
            ["Financial"] = new[] { Financial.View, Financial.ViewLedger, Financial.ManageAccounts, Financial.CreateJournal, Financial.EditJournal, Financial.DeleteJournal, Financial.ViewReports, Financial.ZatcaSubmit, Financial.ExportReports, Financial.OverrideClosedPeriods, Financial.ClosePeriod, Financial.ReopenPeriod },
            ["Transport"] = new[] { Transport.View, Transport.ManageBuses, Transport.ManageRoutes, Transport.ManageDrivers, Transport.AssignStudents, Transport.TrackLive, Transport.ViewSchedule },
            ["Clinic"] = new[] { Clinic.View, Clinic.Dashboard, Clinic.QuickVisit, Clinic.ManageVisits, Clinic.ViewVisits, Clinic.EditVisit, Clinic.DeleteVisit, Clinic.HealthProfiles, Clinic.ViewHealthProfile, Clinic.EditHealthProfile, Clinic.MedicalInventory, Clinic.ManageMedicine, Clinic.DispenseMedicine, Clinic.ViewRecords, Clinic.CreatePrescription, Clinic.ManageInsurance, Clinic.MedicalExcuses, Clinic.CreateExcuse, Clinic.EditExcuse, Clinic.SendExcuseToTeachers, Clinic.ViewReports },
            ["Alumni"] = new[] { Alumni.View, Alumni.Dashboard, Alumni.ViewDirectory, Alumni.ManageRecords, Alumni.EditRecord, Alumni.DeleteRecord, Alumni.GraduateStudent, Alumni.ProcessGraduation, Alumni.ManageClearance, Alumni.ApproveClearance, Alumni.ViewClearance, Alumni.ManageDocuments, Alumni.IssueCertificates, Alumni.ViewReports, Alumni.TrackCareer, Alumni.Networking },
            ["Admissions"] = new[] { Admissions.View, Admissions.Dashboard, Admissions.ViewApplications, Admissions.CreateApplication, Admissions.EditApplication, Admissions.DeleteApplication, Admissions.ReviewApplication, Admissions.ApproveApplication, Admissions.RejectApplication, Admissions.CreateExam, Admissions.EditExam, Admissions.ViewExam, Admissions.ConvertToStudent, Admissions.ViewDocuments, Admissions.ManageDocuments },
            ["Canteen"] = new[] { Canteen.View, Canteen.ManageItems, Canteen.ManageSales, Canteen.ManageInventory, Canteen.ViewReports, Canteen.ManageSuppliers },
            ["Inventory"] = new[] { Inventory.View, Inventory.ManageWarehouses, Inventory.ManageItems, Inventory.ManageTransactions, Inventory.ManageAdjustments, Inventory.ViewReports, Inventory.ManageTransfers },
            ["Procurement"] = new[] { Procurement.View, Procurement.ManageSuppliers, Procurement.ManagePurchaseOrders, Procurement.ManagePurchaseInvoices, Procurement.ManagePurchaseRequests, Procurement.ApproveOrders, Procurement.ViewReports },
            ["Timetable"] = new[] { Timetable.View, Timetable.Create, Timetable.Edit, Timetable.Delete, Timetable.ManageClasses, Timetable.ManageSubjects, Timetable.AssignTeachers, Timetable.ViewSchedule },
            ["Behavior"] = new[] { Behavior.View, Behavior.RecordIncident, Behavior.ViewRecords, Behavior.ManageCategories, Behavior.IssueWarning, Behavior.IssueReward, Behavior.ViewReports },
            ["Reports"] = new[] { Reports.View, Reports.StudentReports, Reports.FinancialReports, Reports.AttendanceReports, Reports.AcademicReports, Reports.CustomReports, Reports.ExportReports },
            ["Settings"] = new[] { Settings.View, Settings.ManageSchool, Settings.ManageBranches, Settings.ManageUsers, Settings.ManageRoles, Settings.ManagePermissions, Settings.SystemConfig, Settings.BackupRestore },
            ["Chat"] = new[] { Chat.View, Chat.SendMessage, Chat.ManageRooms, Chat.ManageMembers, Chat.ViewHistory },
            ["Admin"] = new[] { Admin.View, Admin.ManageTenants, Admin.SystemLogs, Admin.ManageLicenses, Admin.SuperAdmin },
            ["Classrooms"] = new[] { Classrooms.View, Classrooms.Create, Classrooms.Edit, Classrooms.Delete, Classrooms.ManageCapacity, Classrooms.AssignTeacher },
            ["StudentAccounts"] = new[] { StudentAccounts.View, StudentAccounts.Create, StudentAccounts.Edit, StudentAccounts.ViewStatement, StudentAccounts.ApplyDiscount, StudentAccounts.GenerateInvoices, StudentAccounts.ManageBalance },
            ["GeneralLedger"] = new[] { GeneralLedger.View, GeneralLedger.ViewLedger, GeneralLedger.AccountBalance, GeneralLedger.TrialBalance, GeneralLedger.IncomeStatement, GeneralLedger.BalanceSheet, GeneralLedger.CreateManualEntry, GeneralLedger.ViewJournalEntry },
            ["FinancialYearClosing"] = new[] { FinancialYearClosing.View, Financial.ClosePeriod, Financial.ReopenPeriod, FinancialYearClosing.CreateOpeningEntry, FinancialYearClosing.ViewAuditLogs, FinancialYearClosing.PostUnpostedEntries },
            ["EarlyWarning"] = new[] { EarlyWarning.View, EarlyWarning.ViewDashboard, EarlyWarning.ViewStudentDetails, EarlyWarning.ViewRiskMatrix, EarlyWarning.ViewRevenuePrediction, EarlyWarning.TriggerNotification },
            ["License"] = new[] { License.View, License.Activate, License.Deactivate, License.ViewStatus, License.DownloadTemplate },
            ["HR"] = new[] { 
                HR.Dashboard, HR.EmployeesView, HR.EmployeesCreate, HR.EmployeesEdit, HR.EmployeesDelete,
                HR.ContractsView, HR.ContractsCreate, HR.ContractsEdit, HR.ContractsDelete, HR.ContractsRenew, HR.ContractsArchive,
                HR.LeavesView, HR.LeavesCreate, HR.LeavesEdit, HR.LeavesDelete, HR.LeavesApprove, HR.LeavesReject, HR.LeaveBalanceView, HR.LeaveLedger,
                HR.PayrollView, HR.PayrollCreate, HR.PayrollEdit, HR.PayrollDelete, HR.PayrollApprove, HR.PayrollArchive, HR.PayrollExport,
                HR.AttendanceView, HR.AttendanceImport, HR.AttendanceEdit, HR.AttendanceDelete, HR.PermitsView, HR.PermitsCreate, HR.PermitsEdit, HR.PermitsDelete,
                HR.GratuityView, HR.GratuityCalculate, HR.GratuityGenerateLetter, HR.ResignedView, HR.ResignedCreate,
                HR.ReportsView, HR.ReportsExport, HR.SettingsManage, HR.NationalitiesManage, HR.LeaveTypesManage
            }
        };
    }

    /// <summary>
    /// الحصول على جميع الصلاحيات كقائمة مسطحة
    /// </summary>
    private static string[] GetAllPermissions()
    {
        var permissions = new List<string>();
        var modulePermissions = GetPermissionsByModule();
        
        foreach (var module in modulePermissions.Values)
        {
            permissions.AddRange(module);
        }
        
        return permissions.ToArray();
    }

    /// <summary>
    /// قوالب الأدوار الجاهزة (Role Presets)
    /// </summary>
    public static class RolePresets
    {
        public static readonly Dictionary<string, string[]> Teacher = new()
        {
            ["Students"] = new[] { Students.View, Students.ViewAcademic, Students.ViewDocuments },
            ["Attendance"] = new[] { Attendance.View, Attendance.TakeAttendance },
            ["Grades"] = new[] { Grades.View, Grades.Create, Grades.Edit },
            ["Classrooms"] = new[] { Classrooms.View },
            ["Timetable"] = new[] { Timetable.View, Timetable.ViewSchedule },
            ["Behavior"] = new[] { Behavior.View, Behavior.RecordIncident, Behavior.IssueReward },
            ["Clinic"] = new[] { Clinic.View, Clinic.HealthProfiles, Clinic.ViewHealthProfile },
            ["Chat"] = new[] { Chat.View, Chat.SendMessage, Chat.ViewHistory }
        };

        public static readonly Dictionary<string, string[]> FinancialManager = new()
        {
            ["Financial"] = new[] { Financial.View, Financial.ViewLedger, Financial.ManageAccounts, Financial.CreateJournal, Financial.EditJournal, Financial.DeleteJournal, Financial.ViewReports, Financial.ZatcaSubmit, Financial.ExportReports, Financial.OverrideClosedPeriods, Financial.ClosePeriod, Financial.ReopenPeriod },
            ["StudentAccounts"] = new[] { StudentAccounts.View, StudentAccounts.ViewStatement, StudentAccounts.ApplyDiscount, StudentAccounts.GenerateInvoices, StudentAccounts.ManageBalance },
            ["GeneralLedger"] = new[] { GeneralLedger.View, GeneralLedger.ViewLedger, GeneralLedger.AccountBalance, GeneralLedger.TrialBalance, GeneralLedger.IncomeStatement, GeneralLedger.BalanceSheet, GeneralLedger.CreateManualEntry, GeneralLedger.ViewJournalEntry },
            ["FinancialYearClosing"] = new[] { FinancialYearClosing.View, Financial.ClosePeriod, Financial.ReopenPeriod, FinancialYearClosing.CreateOpeningEntry, FinancialYearClosing.ViewAuditLogs, FinancialYearClosing.PostUnpostedEntries },
            ["Invoices"] = new[] { Invoices.View, Invoices.Create, Invoices.Edit, Invoices.Delete, Invoices.Pay, Invoices.Refund, Invoices.Export, Invoices.ViewHistory },
            ["Inventory"] = new[] { Inventory.View, Inventory.ManageItems, Inventory.ManageTransactions, Inventory.ManageAdjustments, Inventory.ViewReports },
            ["Procurement"] = new[] { Procurement.View, Procurement.ManageSuppliers, Procurement.ManagePurchaseOrders, Procurement.ManagePurchaseInvoices, Procurement.ApproveOrders, Procurement.ViewReports },
            ["Students"] = new[] { Students.View },
            ["Guardians"] = new[] { Guardians.View },
            ["HR"] = new[] { HR.Dashboard, HR.PayrollView, HR.PayrollCreate, HR.PayrollEdit, HR.PayrollApprove, HR.PayrollArchive, HR.PayrollExport, HR.ReportsView, HR.ReportsExport },
            ["Reports"] = new[] { Reports.View, Reports.FinancialReports, Reports.ExportReports }
        };

        public static readonly Dictionary<string, string[]> Accountant = new()
        {
            ["Invoices"] = new[] { Invoices.View, Invoices.Create, Invoices.Edit, Invoices.Pay, Invoices.ViewHistory, Invoices.Export },
            ["Financial"] = new[] { Financial.View, Financial.ViewLedger, Financial.CreateJournal, Financial.EditJournal, Financial.ViewReports, Financial.ZatcaSubmit, Financial.ExportReports },
            ["Students"] = new[] { Students.View },
            ["Guardians"] = new[] { Guardians.View },
            ["StudentAccounts"] = new[] { StudentAccounts.View, StudentAccounts.ViewStatement, StudentAccounts.ApplyDiscount, StudentAccounts.GenerateInvoices },
            ["Inventory"] = new[] { Inventory.View, Inventory.ManageItems, Inventory.ManageTransactions, Inventory.ViewReports },
            ["Procurement"] = new[] { Procurement.View, Procurement.ManageSuppliers, Procurement.ManagePurchaseInvoices, Procurement.ViewReports },
            ["GeneralLedger"] = new[] { GeneralLedger.View, GeneralLedger.ViewLedger, GeneralLedger.AccountBalance, GeneralLedger.TrialBalance, GeneralLedger.IncomeStatement, GeneralLedger.BalanceSheet },
            ["FinancialYearClosing"] = new[] { FinancialYearClosing.View, Financial.ClosePeriod, Financial.ReopenPeriod },
            ["Reports"] = new[] { Reports.View, Reports.FinancialReports, Reports.ExportReports }
        };

        public static readonly Dictionary<string, string[]> Receptionist = new()
        {
            ["Students"] = new[] { Students.View, Students.Create, Students.Edit, Students.ViewDocuments },
            ["Guardians"] = new[] { Guardians.View, Guardians.Create, Guardians.Edit, Guardians.LinkStudent },
            ["Attendance"] = new[] { Attendance.View, Attendance.ManualEntry },
            ["Invoices"] = new[] { Invoices.View, Invoices.Create },
            ["StudentAccounts"] = new[] { StudentAccounts.View, StudentAccounts.ViewStatement },
            ["Clinic"] = new[] { Clinic.View, Clinic.QuickVisit, Clinic.ManageVisits, Clinic.ViewVisits, Clinic.EditVisit },
            ["Admissions"] = new[] { Admissions.View, Admissions.Dashboard, Admissions.ViewApplications, Admissions.CreateApplication },
            ["Chat"] = new[] { Chat.View, Chat.SendMessage }
        };

        public static readonly Dictionary<string, string[]> BusSupervisor = new()
        {
            ["Transport"] = new[] { Transport.View, Transport.ManageBuses, Transport.ManageRoutes, Transport.AssignStudents, Transport.TrackLive, Transport.ViewSchedule },
            ["Students"] = new[] { Students.View },
            ["Classrooms"] = new[] { Classrooms.View },
            ["Attendance"] = new[] { Attendance.View }
        };

        public static readonly Dictionary<string, string[]> InventoryManager = new()
        {
            ["Inventory"] = new[] { Inventory.View, Inventory.ManageWarehouses, Inventory.ManageItems, Inventory.ManageTransactions, Inventory.ManageAdjustments, Inventory.ViewReports, Inventory.ManageTransfers },
            ["Procurement"] = new[] { Procurement.View, Procurement.ManageSuppliers, Procurement.ManagePurchaseOrders, Procurement.ManagePurchaseInvoices, Procurement.ManagePurchaseRequests, Procurement.ApproveOrders, Procurement.ViewReports },
            ["Reports"] = new[] { Reports.View, Reports.ExportReports }
        };

        public static readonly Dictionary<string, string[]> SchoolNurse = new()
        {
            ["Clinic"] = new[] { Clinic.View, Clinic.Dashboard, Clinic.QuickVisit, Clinic.ManageVisits, Clinic.ViewVisits, Clinic.EditVisit, Clinic.DeleteVisit, Clinic.HealthProfiles, Clinic.ViewHealthProfile, Clinic.EditHealthProfile, Clinic.MedicalInventory, Clinic.ManageMedicine, Clinic.DispenseMedicine, Clinic.ViewRecords, Clinic.CreatePrescription, Clinic.ManageInsurance, Clinic.MedicalExcuses, Clinic.CreateExcuse, Clinic.EditExcuse, Clinic.SendExcuseToTeachers, Clinic.ViewReports },
            ["Students"] = new[] { Students.View, Students.ViewDocuments },
            ["Guardians"] = new[] { Guardians.View },
            ["Chat"] = new[] { Chat.View, Chat.SendMessage }
        };

        public static readonly Dictionary<string, string[]> HRManager = new()
        {
            ["HR"] = new[] { 
                HR.Dashboard, HR.EmployeesView, HR.EmployeesCreate, HR.EmployeesEdit, HR.EmployeesDelete,
                HR.ContractsView, HR.ContractsCreate, HR.ContractsEdit, HR.ContractsDelete, HR.ContractsRenew, HR.ContractsArchive,
                HR.LeavesView, HR.LeavesCreate, HR.LeavesEdit, HR.LeavesDelete, HR.LeavesApprove, HR.LeavesReject, HR.LeaveBalanceView, HR.LeaveLedger,
                HR.PayrollView, HR.PayrollCreate, HR.PayrollEdit, HR.PayrollDelete, HR.PayrollApprove, HR.PayrollArchive, HR.PayrollExport,
                HR.AttendanceView, HR.AttendanceImport, HR.AttendanceEdit, HR.AttendanceDelete, HR.PermitsView, HR.PermitsCreate, HR.PermitsEdit, HR.PermitsDelete,
                HR.GratuityView, HR.GratuityCalculate, HR.GratuityGenerateLetter, HR.ResignedView, HR.ResignedCreate,
                HR.ReportsView, HR.ReportsExport, HR.SettingsManage, HR.NationalitiesManage, HR.LeaveTypesManage
            },
            ["Employees"] = new[] { Employees.View, Employees.Create, Employees.Edit, Employees.ManageLeaves, Employees.ViewSalary, Employees.ManageSalary, Employees.ViewAttendance, Employees.ManageAttendance },
            ["Students"] = new[] { Students.View },
            ["Reports"] = new[] { Reports.View, Reports.ExportReports }
        };

        public static readonly Dictionary<string, string[]> SchoolAdmin = new()
        {
            ["Students"] = new[] { Students.View, Students.Create, Students.Edit, Students.Delete, Students.Promote, Students.ManageDocuments, Students.ManageAcademic, Students.Export, Students.Import },
            ["Guardians"] = new[] { Guardians.View, Guardians.Create, Guardians.Edit, Guardians.Delete, Guardians.LinkStudent, Guardians.UnlinkStudent },
            ["Employees"] = new[] { Employees.View, Employees.Create, Employees.Edit, Employees.Delete, Employees.ManageLeaves, Employees.ViewSalary, Employees.ManageSalary, Employees.Export },
            ["Attendance"] = new[] { Attendance.View, Attendance.TakeAttendance, Attendance.LiveLog, Attendance.ManualEntry, Attendance.EditAttendance, Attendance.Export, Attendance.ViewReports },
            ["Grades"] = new[] { Grades.View, Grades.Create, Grades.Edit, Grades.Delete, Grades.ManageLevels, Grades.AssignClasses },
            ["Invoices"] = new[] { Invoices.View, Invoices.Create, Invoices.Edit, Invoices.Delete, Invoices.Pay, Invoices.Refund, Invoices.Export, Invoices.ViewHistory },
            ["Classrooms"] = new[] { Classrooms.View, Classrooms.Create, Classrooms.Edit, Classrooms.Delete, Classrooms.ManageCapacity, Classrooms.AssignTeacher },
            ["StudentAccounts"] = new[] { StudentAccounts.View, StudentAccounts.Create, StudentAccounts.Edit, StudentAccounts.ViewStatement, StudentAccounts.ApplyDiscount, StudentAccounts.GenerateInvoices, StudentAccounts.ManageBalance },
            ["Inventory"] = new[] { Inventory.View, Inventory.ManageWarehouses, Inventory.ManageItems, Inventory.ManageTransactions, Inventory.ManageAdjustments, Inventory.ViewReports, Inventory.ManageTransfers },
            ["Procurement"] = new[] { Procurement.View, Procurement.ManageSuppliers, Procurement.ManagePurchaseOrders, Procurement.ManagePurchaseInvoices, Procurement.ManagePurchaseRequests, Procurement.ApproveOrders, Procurement.ViewReports },
            ["Clinic"] = new[] { Clinic.View, Clinic.Dashboard, Clinic.QuickVisit, Clinic.ManageVisits, Clinic.ViewVisits, Clinic.EditVisit, Clinic.DeleteVisit, Clinic.HealthProfiles, Clinic.ViewHealthProfile, Clinic.EditHealthProfile, Clinic.MedicalInventory, Clinic.ManageMedicine, Clinic.DispenseMedicine, Clinic.ViewRecords, Clinic.CreatePrescription, Clinic.ManageInsurance, Clinic.MedicalExcuses, Clinic.CreateExcuse, Clinic.EditExcuse, Clinic.SendExcuseToTeachers, Clinic.ViewReports },
            ["Alumni"] = new[] { Alumni.View, Alumni.Dashboard, Alumni.ViewDirectory, Alumni.ManageRecords, Alumni.EditRecord, Alumni.DeleteRecord, Alumni.GraduateStudent, Alumni.ProcessGraduation, Alumni.ManageClearance, Alumni.ApproveClearance, Alumni.ViewClearance, Alumni.ManageDocuments, Alumni.IssueCertificates, Alumni.ViewReports, Alumni.TrackCareer, Alumni.Networking },
            ["Admissions"] = new[] { Admissions.View, Admissions.Dashboard, Admissions.ViewApplications, Admissions.CreateApplication, Admissions.EditApplication, Admissions.DeleteApplication, Admissions.ReviewApplication, Admissions.ApproveApplication, Admissions.RejectApplication, Admissions.CreateExam, Admissions.EditExam, Admissions.ViewExam, Admissions.ConvertToStudent, Admissions.ViewDocuments, Admissions.ManageDocuments },
            ["Settings"] = new[] { Settings.View, Settings.ManageSchool, Settings.ManageBranches, Settings.ManageUsers, Settings.ManageRoles, Settings.ManagePermissions },
            ["Reports"] = new[] { Reports.View, Reports.StudentReports, Reports.FinancialReports, Reports.AttendanceReports, Reports.AcademicReports, Reports.ExportReports },
            ["Chat"] = new[] { Chat.View, Chat.SendMessage, Chat.ManageRooms, Chat.ManageMembers, Chat.ViewHistory },
            ["HR"] = new[] { 
                HR.Dashboard, HR.EmployeesView, HR.EmployeesCreate, HR.EmployeesEdit, HR.EmployeesDelete,
                HR.ContractsView, HR.ContractsCreate, HR.ContractsEdit, HR.ContractsDelete, HR.ContractsRenew, HR.ContractsArchive,
                HR.LeavesView, HR.LeavesCreate, HR.LeavesEdit, HR.LeavesDelete, HR.LeavesApprove, HR.LeavesReject, HR.LeaveBalanceView, HR.LeaveLedger,
                HR.PayrollView, HR.PayrollCreate, HR.PayrollEdit, HR.PayrollDelete, HR.PayrollApprove, HR.PayrollArchive, HR.PayrollExport,
                HR.AttendanceView, HR.AttendanceImport, HR.AttendanceEdit, HR.AttendanceDelete, HR.PermitsView, HR.PermitsCreate, HR.PermitsEdit, HR.PermitsDelete,
                HR.GratuityView, HR.GratuityCalculate, HR.GratuityGenerateLetter, HR.ResignedView, HR.ResignedCreate,
                HR.ReportsView, HR.ReportsExport, HR.SettingsManage, HR.NationalitiesManage, HR.LeaveTypesManage
            }
        };

        public static readonly Dictionary<string, string[]> SuperAdmin = new()
        {
            ["All"] = AllPermissions
        };

        public static Dictionary<string, Dictionary<string, string[]>> AllPresets = new()
        {
            ["Teacher"] = Teacher,
            ["Accountant"] = Accountant,
            ["FinancialManager"] = FinancialManager,
            ["Receptionist"] = Receptionist,
            ["BusSupervisor"] = BusSupervisor,
            ["InventoryManager"] = InventoryManager,
            ["SchoolNurse"] = SchoolNurse,
            ["HRManager"] = HRManager,
            ["SchoolAdmin"] = SchoolAdmin,
            ["SuperAdmin"] = SuperAdmin
        };
    }
}