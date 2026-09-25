using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.Canteen;
using Masar.Schools.ERP.Domain.Entities.Admissions;
using Masar.Schools.ERP.Domain.Entities.Clinic;
using Masar.Schools.ERP.Domain.Entities.Alumni;
using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace Masar.Schools.ERP.Infrastructure.Data;

public class MasarDbContext : IdentityDbContext<MasarUser, MasarRole, Guid>
{
    public MasarDbContext(DbContextOptions<MasarDbContext> options) : base(options)
    {
    }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<MasarUser> MasarUsers { get; set; }
    public DbSet<MasarRole> MasarRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<ClassRoom> ClassRooms { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<GradeLevel> GradeLevels { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<BackupRecord> BackupRecords { get; set; }
    public DbSet<BackupSettings> BackupSettings { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<InvoicePayment> InvoicePayments { get; set; }
    
    // Financial Accounting Entities
    public DbSet<Account> Accounts { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
    public DbSet<StudentAccount> StudentAccounts { get; set; }
    public DbSet<StudentInvoice> StudentInvoices { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    public DbSet<StudentPayment> StudentPayments { get; set; }
    public DbSet<PaymentAllocation> PaymentAllocations { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<StudentDiscount> StudentDiscounts { get; set; }

    // AI Engine Entities
    public DbSet<StudentRiskScore> StudentRiskScores { get; set; }
    public DbSet<RiskAlertHistory> RiskAlertHistories { get; set; }
    public DbSet<AiChatHistory> AiChatHistories { get; set; }

    // Financial Closing Entities
    public DbSet<FinancialPeriod> FinancialPeriods { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    // Student Records Entities
    public DbSet<AcademicRecord> AcademicRecords { get; set; }
    public DbSet<StudentDocument> StudentDocuments { get; set; }
    public DbSet<DocumentActivityLog> DocumentActivityLogs { get; set; }
    public DbSet<AcademicTerm> AcademicTerms { get; set; }
    public DbSet<TermArchive> TermArchives { get; set; }

    // Inventory Management Entities
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<StockAdjustment> StockAdjustments { get; set; }
    public DbSet<StockAdjustmentItem> StockAdjustmentItems { get; set; }

    // Procurement Management Entities
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
    public DbSet<InventoryTransfer> InventoryTransfers { get; set; }
    public DbSet<InventoryTransferItem> InventoryTransferItems { get; set; }

    // Canteen & POS Management Entities
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<CanteenProduct> CanteenProducts { get; set; }
    public DbSet<CanteenProductImage> CanteenProductImages { get; set; }
    public DbSet<CanteenProductVariant> CanteenProductVariants { get; set; }
    public DbSet<CanteenProductExtra> CanteenProductExtras { get; set; }
    public DbSet<CanteenTable> CanteenTables { get; set; }
    public DbSet<CashierShift> CashierShifts { get; set; }
    public DbSet<CanteenOrder> CanteenOrders { get; set; }
    public DbSet<CanteenOrderItem> CanteenOrderItems { get; set; }
    public DbSet<CanteenOrderItemExtra> CanteenOrderItemExtras { get; set; }
    public DbSet<DeliveryZone> DeliveryZones { get; set; }
    public DbSet<PickUpPoint> PickUpPoints { get; set; }
    public DbSet<CanteenPayment> CanteenPayments { get; set; }

    // Admissions & Registration Entities
    public DbSet<AdmissionApplication> AdmissionApplications { get; set; }
    public DbSet<AdmissionExam> AdmissionExams { get; set; }
    public DbSet<ApplicationDocument> ApplicationDocuments { get; set; }

    // School Clinic Entities
    public DbSet<StudentHealthProfile> StudentHealthProfiles { get; set; }
    public DbSet<ClinicVisit> ClinicVisits { get; set; }
    public DbSet<MedicalItem> MedicalItems { get; set; }
    public DbSet<MedicalExcuse> MedicalExcuses { get; set; }

    // Alumni Management Entities
    public DbSet<AlumniRecord> AlumniRecords { get; set; }
    public DbSet<GraduationDocument> GraduationDocuments { get; set; }
    public DbSet<GraduationClearanceRecord> GraduationClearanceRecords { get; set; }

    // Roll Call Entities
    public DbSet<RollCallSession> RollCallSessions { get; set; }
    public DbSet<RollCallRecord> RollCallRecords { get; set; }

    // Transport Management Entities
    public DbSet<Bus> Buses { get; set; }
    public DbSet<BusRoute> BusRoutes { get; set; }
    public DbSet<RouteStop> RouteStops { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<BusSupervisor> BusSupervisors { get; set; }
    public DbSet<BusSchedule> BusSchedules { get; set; }
    public DbSet<StudentTransportSubscription> StudentTransportSubscriptions { get; set; }
    public DbSet<BusTrackingLog> BusTrackingLogs { get; set; }
    public DbSet<BusAttendance> BusAttendances { get; set; }
    public DbSet<BusIncident> BusIncidents { get; set; }
    public DbSet<FuelRecord> FuelRecords { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

    // HR & Leave Management Entities
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }
    
    // Extended HR Entities
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<ContractArchive> ContractArchives { get; set; }
    public DbSet<PayrollProfile> PayrollProfiles { get; set; }
    public DbSet<PayrollArchive> PayrollArchives { get; set; }
    public DbSet<AttendanceLog> AttendanceLogs { get; set; }
    public DbSet<AttendancePermit> AttendancePermits { get; set; }
    public DbSet<ResignedEmployee> ResignedEmployees { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<Nationality> Nationalities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure multi-tenancy with global query filters
        ConfigureMultiTenancy(modelBuilder);
        
        // Configure entity relationships
        ConfigureRelationships(modelBuilder);
        
        // Configure indexes
        ConfigureIndexes(modelBuilder);

        // Configure AI Engine entities
        ConfigureAIEntities(modelBuilder);
        
        // Configure Classroom entities
        ConfigureClassroomEntities(modelBuilder);

        // Configure Student Records entities
        ConfigureStudentRecordsEntities(modelBuilder);

        // Configure Archive entities
        ConfigureArchiveEntities(modelBuilder);

        // Configure Inventory Management entities
        ConfigureInventoryEntities(modelBuilder);

        // Configure Procurement Management entities
        ConfigureProcurementEntities(modelBuilder);

        // Configure Roll Call entities
        ConfigureRollCallEntities(modelBuilder);

        // Configure Archive entities
        ConfigureArchiveEntities(modelBuilder);

        // Configure Transport Management entities
        ConfigureTransportEntities(modelBuilder);

        // Configure Canteen & POS Management entities
        ConfigureCanteenEntities(modelBuilder);

        // Configure Admissions & Registration entities
        ConfigureAdmissionsEntities(modelBuilder);

        // Configure School Clinic entities
        ConfigureClinicEntities(modelBuilder);

        // Configure Alumni Management entities
        ConfigureAlumniEntities(modelBuilder);

        // Configure HR & Leave Management entities
        ConfigureLeaveEntities(modelBuilder);

        // Configure Extended HR entities
        ConfigureHREntities(modelBuilder);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void ConfigureMultiTenancy(ModelBuilder modelBuilder)
    {
        // Apply global query filter for tenant isolation
        modelBuilder.Entity<Tenant>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<School>().HasQueryFilter(s => !s.IsDeleted && !s.Tenant.IsDeleted);
        modelBuilder.Entity<Branch>().HasQueryFilter(b => !b.IsDeleted && !b.School.IsDeleted);
        modelBuilder.Entity<MasarUser>().HasQueryFilter(u => u.IsActive && !u.Tenant.IsDeleted);
        modelBuilder.Entity<MasarRole>().HasQueryFilter(r => r.IsActive && !r.Tenant.IsDeleted);
        modelBuilder.Entity<Permission>().HasQueryFilter(p => p.IsActive);
        modelBuilder.Entity<UserPermission>().HasQueryFilter(up => true);
        modelBuilder.Entity<RolePermission>().HasQueryFilter(rp => true);
        modelBuilder.Entity<ChatRoom>().HasQueryFilter(c => !c.IsDeleted && !c.Tenant.IsDeleted);
        modelBuilder.Entity<ChatMessage>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<ChatRoomMember>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Student>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Guardian>().HasQueryFilter(g => !g.IsDeleted && !g.Tenant.IsDeleted);
        modelBuilder.Entity<ClassRoom>().HasQueryFilter(c => !c.IsDeleted && !c.School.IsDeleted);
        modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<LeaveRequest>().HasQueryFilter(l => !l.IsDeleted);
        modelBuilder.Entity<LeaveBalance>().HasQueryFilter(l => l.IsActive);
        modelBuilder.Entity<GradeLevel>().HasQueryFilter(g => !g.IsDeleted);
        
        // Alumni entities query filters
        modelBuilder.Entity<AlumniRecord>().HasQueryFilter(a => a.IsActive);
        modelBuilder.Entity<GraduationDocument>().HasQueryFilter(g => !g.IsDeleted);
        modelBuilder.Entity<GraduationClearanceRecord>().HasQueryFilter(g => !g.IsDeleted);
        modelBuilder.Entity<Section>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<AttendanceRecord>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Grade>().HasQueryFilter(g => !g.IsDeleted);
        modelBuilder.Entity<Invoice>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<InvoiceItem>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<InvoicePayment>().HasQueryFilter(p => !p.IsDeleted);
        
        // Financial entities query filters
        modelBuilder.Entity<Account>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<JournalEntry>().HasQueryFilter(j => !j.IsDeleted);
        modelBuilder.Entity<JournalEntryLine>().HasQueryFilter(j => !j.IsDeleted);
        modelBuilder.Entity<StudentAccount>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<StudentInvoice>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<InvoiceLineItem>().HasQueryFilter(i => !i.IsDeleted);
        modelBuilder.Entity<StudentPayment>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<PaymentAllocation>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Discount>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<StudentDiscount>().HasQueryFilter(s => !s.IsDeleted);

        // AI Engine entities query filters
        modelBuilder.Entity<StudentRiskScore>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<RiskAlertHistory>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<AiChatHistory>().HasQueryFilter(a => true);

        // Financial Closing entities query filters
        modelBuilder.Entity<FinancialPeriod>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<AuditLog>().HasQueryFilter(a => !a.IsDeleted);

        // Student Records entities query filters
        modelBuilder.Entity<AcademicRecord>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<StudentDocument>().HasQueryFilter(s => !s.IsDeleted);

        // Roll Call entities query filters
        modelBuilder.Entity<RollCallSession>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<RollCallRecord>().HasQueryFilter(r => !r.IsDeleted);

        // Transport Management entities query filters
        modelBuilder.Entity<Bus>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<BusRoute>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<RouteStop>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Driver>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<BusSupervisor>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<BusSchedule>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<StudentTransportSubscription>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<BusTrackingLog>().HasQueryFilter(b => true);
        modelBuilder.Entity<BusAttendance>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<BusIncident>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<FuelRecord>().HasQueryFilter(f => !f.IsDeleted);
        modelBuilder.Entity<MaintenanceRecord>().HasQueryFilter(m => !m.IsDeleted);

        // Canteen & POS Management entities query filters
        modelBuilder.Entity<ProductCategory>().HasQueryFilter(p => !p.School.IsDeleted);
        modelBuilder.Entity<CanteenProduct>().HasQueryFilter(p => !p.School.IsDeleted);
        modelBuilder.Entity<CanteenProductImage>().HasQueryFilter(p => true);
        modelBuilder.Entity<CanteenProductVariant>().HasQueryFilter(p => true);
        modelBuilder.Entity<CanteenProductExtra>().HasQueryFilter(p => !p.School.IsDeleted);
        modelBuilder.Entity<CanteenTable>().HasQueryFilter(c => !c.School.IsDeleted);
        modelBuilder.Entity<CashierShift>().HasQueryFilter(c => !c.School.IsDeleted);
        modelBuilder.Entity<CanteenOrder>().HasQueryFilter(c => !c.School.IsDeleted);
        modelBuilder.Entity<CanteenOrderItem>().HasQueryFilter(c => true);
        modelBuilder.Entity<CanteenOrderItemExtra>().HasQueryFilter(c => true);
        modelBuilder.Entity<DeliveryZone>().HasQueryFilter(d => !d.School.IsDeleted);
        modelBuilder.Entity<PickUpPoint>().HasQueryFilter(p => !p.School.IsDeleted);
        modelBuilder.Entity<CanteenPayment>().HasQueryFilter(c => true);

        // Admissions & Registration entities query filters
        modelBuilder.Entity<AdmissionApplication>().HasQueryFilter(a => !a.School.IsDeleted);
        modelBuilder.Entity<AdmissionExam>().HasQueryFilter(e => !e.School.IsDeleted);
        modelBuilder.Entity<ApplicationDocument>().HasQueryFilter(d => true);

        // School Clinic entities query filters
        modelBuilder.Entity<StudentHealthProfile>().HasQueryFilter(s => !s.Student.IsDeleted);
        modelBuilder.Entity<ClinicVisit>().HasQueryFilter(c => !c.School.IsDeleted);
        modelBuilder.Entity<MedicalItem>().HasQueryFilter(m => !m.School.IsDeleted);
        modelBuilder.Entity<MedicalExcuse>().HasQueryFilter(m => !m.School.IsDeleted);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Tenant relationships
        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.Schools)
            .WithOne(s => s.Tenant)
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.MasarUsers)
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tenant>()
            .HasMany(t => t.MasarRoles)
            .WithOne(r => r.Tenant)
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // School relationships
        modelBuilder.Entity<School>()
            .HasMany(s => s.Branches)
            .WithOne(b => b.School)
            .HasForeignKey(b => b.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<School>()
            .HasMany(s => s.Students)
            .WithOne(st => st.School)
            .HasForeignKey(st => st.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<School>()
            .HasMany(s => s.Employees)
            .WithOne(e => e.School)
            .HasForeignKey(e => e.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch relationships
        modelBuilder.Entity<Branch>()
            .HasMany(b => b.ClassRooms)
            .WithOne(c => c.Branch)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // MasarUser relationships with Identity
        modelBuilder.Entity<MasarUser>()
            .HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Chat relationships
        modelBuilder.Entity<ChatRoom>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.ChatRoom)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatRoom>()
            .HasMany(c => c.Members)
            .WithOne(m => m.ChatRoom)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatMessage relationships - fix cascade paths
        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student relationships
        modelBuilder.Entity<Student>()
            .HasMany(s => s.AttendanceRecords)
            .WithOne(a => a.Student)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Grades)
            .WithOne(g => g.Student)
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.AcademicRecords)
            .WithOne(a => a.Student)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Documents)
            .WithOne(d => d.Student)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guardian relationships
        modelBuilder.Entity<Guardian>()
            .HasMany(g => g.Students)
            .WithOne(s => s.Guardian)
            .HasForeignKey(s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);

        // ClassRoom relationships
        modelBuilder.Entity<ClassRoom>()
            .HasMany(c => c.Students)
            .WithOne(s => s.ClassRoom)
            .HasForeignKey(s => s.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ClassRoom>()
            .HasMany(c => c.AttendanceRecords)
            .WithOne(a => a.ClassRoom)
            .HasForeignKey(a => a.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Invoice relationships
        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Items)
            .WithOne(item => item.Invoice)
            .HasForeignKey(item => item.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Financial Accounting relationships
        // Account hierarchy
        modelBuilder.Entity<Account>()
            .HasMany(a => a.ChildAccounts)
            .WithOne(a => a.ParentAccount)
            .HasForeignKey(a => a.ParentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Account entity to use Id as primary key
        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.AccountId)
            .IsRequired();

        // Journal Entry relationships
        modelBuilder.Entity<JournalEntry>()
            .HasMany(j => j.EntryLines)
            .WithOne(l => l.JournalEntry)
            .HasForeignKey(l => l.EntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JournalEntryLine>()
            .HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student Account relationships
        modelBuilder.Entity<StudentAccount>()
            .HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentAccount>()
            .HasMany(s => s.Invoices)
            .WithOne(i => i.StudentAccount)
            .HasForeignKey(i => i.StudentAccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentAccount>()
            .HasMany(s => s.Payments)
            .WithOne(p => p.StudentAccount)
            .HasForeignKey(p => p.StudentAccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student Invoice relationships
        modelBuilder.Entity<StudentInvoice>()
            .HasMany(i => i.LineItems)
            .WithOne(l => l.Invoice)
            .HasForeignKey(l => l.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentInvoice>()
            .HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student Payment relationships
        modelBuilder.Entity<StudentPayment>()
            .HasMany(p => p.Allocations)
            .WithOne(a => a.Payment)
            .HasForeignKey(a => a.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Permission relationships
        modelBuilder.Entity<Permission>()
            .HasMany(p => p.UserPermissions)
            .WithOne(up => up.Permission)
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Permission>()
            .HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MasarUser>()
            .HasMany(u => u.UserPermissions)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MasarRole>()
            .HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentPayment>()
            .HasMany(p => p.Allocations)
            .WithOne(a => a.Payment)
            .HasForeignKey(a => a.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PaymentAllocation>()
            .HasOne(a => a.Invoice)
            .WithMany()
            .HasForeignKey(a => a.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Discount relationships
        modelBuilder.Entity<Discount>()
            .HasMany(d => d.StudentDiscounts)
            .WithOne(sd => sd.Discount)
            .HasForeignKey(sd => sd.DiscountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentDiscount>()
            .HasOne(sd => sd.StudentAccount)
            .WithMany()
            .HasForeignKey(sd => sd.StudentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentDiscount>()
            .HasOne(sd => sd.Invoice)
            .WithMany()
            .HasForeignKey(sd => sd.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // AI Engine relationships
        modelBuilder.Entity<StudentRiskScore>()
            .HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RiskAlertHistory>()
            .HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Financial Closing relationships
        modelBuilder.Entity<FinancialPeriod>()
            .HasMany<AuditLog>()
            .WithOne()
            .HasForeignKey(a => a.FinancialPeriodId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FinancialPeriod>()
            .HasOne(f => f.Tenant)
            .WithMany()
            .HasForeignKey(f => f.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FinancialPeriod>()
            .HasOne(f => f.School)
            .WithMany()
            .HasForeignKey(f => f.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        // Roll Call relationships
        modelBuilder.Entity<RollCallSession>()
            .HasMany(r => r.Records)
            .WithOne(r => r.Session)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RollCallSession>()
            .HasOne(r => r.ClassRoom)
            .WithMany()
            .HasForeignKey(r => r.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RollCallSession>()
            .HasOne(r => r.Teacher)
            .WithMany()
            .HasForeignKey(r => r.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RollCallRecord>()
            .HasOne(r => r.Student)
            .WithMany()
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Student Document relationships
        modelBuilder.Entity<StudentDocument>()
            .HasOne(d => d.PreviousDocument)
            .WithMany(d => d.NextDocuments)
            .HasForeignKey(d => d.PreviousDocumentId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureAIEntities(ModelBuilder modelBuilder)
    {
        // Apply AI entity configurations
        modelBuilder.ApplyConfiguration(new StudentRiskScoreConfiguration());
        modelBuilder.ApplyConfiguration(new RiskAlertHistoryConfiguration());

        // Configure AiChatHistory
        modelBuilder.Entity<AiChatHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserMessage).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.AssistantResponse).IsRequired().HasMaxLength(8000);
            entity.Property(e => e.PageContext).HasMaxLength(500);
            entity.Property(e => e.UserRole).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(200);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => e.CreatedAt);
        });
    }

    private void ConfigureClassroomEntities(ModelBuilder modelBuilder)
    {
        // Apply Classroom entity configuration
        modelBuilder.ApplyConfiguration(new ClassroomConfiguration());
        
        // Configure GradeLevel entity
        modelBuilder.Entity<GradeLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Code).IsUnique().HasFilter("IsDeleted = 0");
        });
        
        // Configure Section entity
        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Code).IsUnique().HasFilter("IsDeleted = 0");
        });
        
        // Configure ClassRoom relationships with GradeLevel and Section
        modelBuilder.Entity<ClassRoom>()
            .HasOne(c => c.GradeLevelEntity)
            .WithMany(g => g.ClassRooms)
            .HasForeignKey(c => c.GradeLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<ClassRoom>()
            .HasOne(c => c.SectionEntity)
            .WithMany(s => s.ClassRooms)
            .HasForeignKey(c => c.SectionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }

    private void ConfigureStudentRecordsEntities(ModelBuilder modelBuilder)
    {
        // Configure AcademicRecord entity
        modelBuilder.Entity<AcademicRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GradeLevel).HasMaxLength(50);
            entity.Property(e => e.GradeLevelArabic).HasMaxLength(50);
            entity.Property(e => e.Section).HasMaxLength(10);
            entity.Property(e => e.SectionArabic).HasMaxLength(10);
            entity.Property(e => e.AcademicStanding).HasMaxLength(50);
            entity.Property(e => e.AcademicStandingArabic).HasMaxLength(50);
            entity.Property(e => e.CurrentTerm).HasMaxLength(50);
            entity.Property(e => e.CurrentTermArabic).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure StudentDocument entity
        modelBuilder.Entity<StudentDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DocumentTypeArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.MimeType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CategoryArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.IsConfidential).HasDefaultValue(false);
            entity.Property(e => e.Version).HasDefaultValue(1);
            entity.Property(e => e.IsRequired).HasDefaultValue(false);
        });

        // Configure AcademicTerm entity
        modelBuilder.Entity<AcademicTerm>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TermName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TermNameArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("قيد الانتظار");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.BiometricAttendanceEnabled).HasDefaultValue(false);
            entity.Property(e => e.RiskPredictionEnabled).HasDefaultValue(false);
            entity.Property(e => e.EnrolledStudentsCount).HasDefaultValue(0);
            entity.Property(e => e.ActiveSectionsCount).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureRollCallEntities(ModelBuilder modelBuilder)
    {
        // Configure RollCallSession entity
        modelBuilder.Entity<RollCallSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Period).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PeriodArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.SubjectArabic).HasMaxLength(100);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.SessionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SessionTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.WhatsAppNotificationsSent).HasDefaultValue(false);
        });

        // Configure RollCallRecord entity
        modelBuilder.Entity<RollCallRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.GuardianNotificationSent).HasDefaultValue(false);
            entity.Property(e => e.IsModified).HasDefaultValue(false);
            entity.Property(e => e.RecordingMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AttendedFullPeriod).HasDefaultValue(true);
            entity.Property(e => e.IsExcused).HasDefaultValue(false);
        });
    }

    private void ConfigureArchiveEntities(ModelBuilder modelBuilder)
    {
        // Configure TermArchive entity
        modelBuilder.Entity<TermArchive>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TermName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TermNameArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ClosingDate).IsRequired();
            entity.Property(e => e.ClosedBy).HasMaxLength(100);
            entity.Property(e => e.ArchiveData).HasMaxLength(5000);
            entity.Property(e => e.ArchiveFilePath).HasMaxLength(500);
            entity.Property(e => e.ArchiveSizeBytes).HasDefaultValue(0);
            entity.Property(e => e.IsArchiveComplete).HasDefaultValue(false);
            entity.Property(e => e.RestoredBy).HasMaxLength(100);

            entity.HasOne(e => e.Term)
                  .WithMany()
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureInventoryEntities(ModelBuilder modelBuilder)
    {
        // Configure Warehouse entity
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.WarehouseType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.WarehouseTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Manager).HasMaxLength(100);
            entity.Property(e => e.ManagerPhone).HasMaxLength(20);
            entity.Property(e => e.CapacityUnit).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Branch)
                  .WithMany()
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure InventoryItem entity
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CategoryArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UnitOfMeasure).IsRequired().HasMaxLength(20);
            entity.Property(e => e.UnitOfMeasureArabic).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsStockItem).HasDefaultValue(true);

            entity.HasOne(e => e.PreferredSupplier)
                  .WithMany()
                  .HasForeignKey(e => e.PreferredSupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure StockTransaction entity
        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransactionNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.PerformedBy).HasMaxLength(100);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany()
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Warehouse)
                  .WithMany()
                  .HasForeignKey(e => e.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure StockAdjustment entity
        modelBuilder.Entity<StockAdjustment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AdjustmentNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AdjustmentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AdjustmentTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ReasonArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("قيد الانتظار");
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.ApprovalNotes).HasMaxLength(500);

            entity.HasOne(e => e.Warehouse)
                  .WithMany()
                  .HasForeignKey(e => e.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure StockAdjustmentItem entity
        modelBuilder.Entity<StockAdjustmentItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.StockAdjustment)
                  .WithMany(e => e.AdjustmentItems)
                  .HasForeignKey(e => e.StockAdjustmentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany()
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureProcurementEntities(ModelBuilder modelBuilder)
    {
        // Configure Supplier entity
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CommercialRegistration).HasMaxLength(50);
            entity.Property(e => e.TaxNumber).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.SupplierCategory).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SupplierCategoryArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PaymentTerms).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure PurchaseOrder entity
        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Draft");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("مسودة");
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.PaymentTerms).HasMaxLength(50);
            entity.Property(e => e.ApprovedByName).HasMaxLength(100);

            entity.HasOne(e => e.Supplier)
                  .WithMany(e => e.PurchaseOrders)
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Warehouse)
                  .WithMany()
                  .HasForeignKey(e => e.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PurchaseOrderItem entity
        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.PurchaseOrder)
                  .WithMany(e => e.OrderItems)
                  .HasForeignKey(e => e.PurchaseOrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany(e => e.PurchaseOrderItems)
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PurchaseInvoice entity
        modelBuilder.Entity<PurchaseInvoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TaxInvoiceNumber).HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("قيد الانتظار");
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.ZatcaId).HasMaxLength(100);

            entity.HasOne(e => e.PurchaseOrder)
                  .WithMany(e => e.PurchaseInvoices)
                  .HasForeignKey(e => e.PurchaseOrderId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Supplier)
                  .WithMany()
                  .HasForeignKey(e => e.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PurchaseInvoiceItem entity
        modelBuilder.Entity<PurchaseInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.PurchaseInvoice)
                  .WithMany(e => e.InvoiceItems)
                  .HasForeignKey(e => e.PurchaseInvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany()
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PurchaseRequest entity
        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.RequestedBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Purpose).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PurposeArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Draft");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("مسودة");
            entity.Property(e => e.ApprovalNotes).HasMaxLength(500);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PurchaseRequestItem entity
        modelBuilder.Entity<PurchaseRequestItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.PurchaseRequest)
                  .WithMany(e => e.RequestItems)
                  .HasForeignKey(e => e.PurchaseRequestId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany()
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure InventoryTransfer entity
        modelBuilder.Entity<InventoryTransfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransferNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Draft");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50).HasDefaultValue("مسودة");
            entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("SAR");
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ReasonArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PerformedBy).HasMaxLength(100);

            entity.HasOne(e => e.FromWarehouse)
                  .WithMany()
                  .HasForeignKey(e => e.FromWarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ToWarehouse)
                  .WithMany()
                  .HasForeignKey(e => e.ToWarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure InventoryTransferItem entity
        modelBuilder.Entity<InventoryTransferItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.InventoryTransfer)
                  .WithMany(e => e.TransferItems)
                  .HasForeignKey(e => e.InventoryTransferId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.InventoryItem)
                  .WithMany()
                  .HasForeignKey(e => e.InventoryItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureTransportEntities(ModelBuilder modelBuilder)
    {
        // Configure Bus entity
        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BusNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PlateNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Active");
            entity.Property(e => e.VehicleIdentificationNumber).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure BusRoute entity
        modelBuilder.Entity<BusRoute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RouteName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RouteCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.StartLocation).IsRequired().HasMaxLength(200);
            entity.Property(e => e.EndLocation).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Direction).HasMaxLength(20).HasDefaultValue("Outbound");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure RouteStop entity
        modelBuilder.Entity<RouteStop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StopName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Landmark).HasMaxLength(200);
        });

        // Configure Driver entity
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LicenseType).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure BusSupervisor entity
        modelBuilder.Entity<BusSupervisor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.EmergencyContact).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure BusSchedule entity
        modelBuilder.Entity<BusSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TripType).HasMaxLength(20).HasDefaultValue("Morning");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure StudentTransportSubscription entity
        modelBuilder.Entity<StudentTransportSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Configure BusTrackingLog entity
        modelBuilder.Entity<BusTrackingLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure BusAttendance entity
        modelBuilder.Entity<BusAttendance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AttendanceType).HasMaxLength(20).HasDefaultValue("Pickup");
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Present");
        });

        // Configure BusIncident entity
        modelBuilder.Entity<BusIncident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IncidentType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Severity).HasMaxLength(20).HasDefaultValue("Medium");
        });

        // Configure FuelRecord entity
        modelBuilder.Entity<FuelRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FuelStation).HasMaxLength(100);
        });

        // Configure MaintenanceRecord entity
        modelBuilder.Entity<MaintenanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MaintenanceType).HasMaxLength(50).HasDefaultValue("Routine");
            entity.Property(e => e.Workshop).HasMaxLength(100);
        });
    }

    private void ConfigureCanteenEntities(ModelBuilder modelBuilder)
    {
        // Configure ProductCategory entity
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CanteenProduct entity
        modelBuilder.Entity<CanteenProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NameEn).HasMaxLength(200);
            entity.Property(e => e.DescriptionAr).HasMaxLength(1000);
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.AllowStudentBalance).HasDefaultValue(true);
            entity.Property(e => e.AllowEmployeeBalance).HasDefaultValue(true);
            entity.Property(e => e.MaxOrderQuantity).HasDefaultValue(10);
            entity.Property(e => e.MinOrderQuantity).HasDefaultValue(1);
            entity.Property(e => e.IsFavorite).HasDefaultValue(false);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.ProductCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CanteenProductImage entity
        modelBuilder.Entity<CanteenProductImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.AltText).HasMaxLength(200);

            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Images)
                  .HasForeignKey(e => e.CanteenProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CanteenProductVariant entity
        modelBuilder.Entity<CanteenProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(e => e.CanteenProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CanteenProductExtra entity
        modelBuilder.Entity<CanteenProductExtra>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Products)
                  .WithMany(p => p.Extras)
                  .UsingEntity(j => j.ToTable("CanteenProductExtrasMapping"));
        });

        // Configure CanteenTable entity
        modelBuilder.Entity<CanteenTable>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TableNumber).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Branch)
                  .WithMany()
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CashierShift entity
        modelBuilder.Entity<CashierShift>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OpeningBalance).HasDefaultValue(0);
            entity.Property(e => e.OpeningCardBalance).HasDefaultValue(0);
            entity.Property(e => e.ExpectedCash).HasDefaultValue(0);
            entity.Property(e => e.ActualCash).HasDefaultValue(0);
            entity.Property(e => e.CardPayments).HasDefaultValue(0);
            entity.Property(e => e.StudentBalancePayments).HasDefaultValue(0);
            entity.Property(e => e.EmployeeBalancePayments).HasDefaultValue(0);
            entity.Property(e => e.TotalSales).HasDefaultValue(0);
            entity.Property(e => e.OrderCount).HasDefaultValue(0);
            entity.Property(e => e.InvoiceCount).HasDefaultValue(0);
            entity.Property(e => e.ShortageOverage).HasDefaultValue(0);
            entity.Property(e => e.IsClosed).HasDefaultValue(false);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.ClosedBy).HasMaxLength(200);
            entity.Property(e => e.ShortageOverageReason).HasMaxLength(500);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Branch)
                  .WithMany()
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CanteenOrder entity
        modelBuilder.Entity<CanteenOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);
            entity.Property(e => e.DeliveryAddress).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.StudentBalanceDeduction).HasDefaultValue(0);
            entity.Property(e => e.DeliveryFee).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Branch)
                  .WithMany()
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CashierShift)
                  .WithMany(s => s.Orders)
                  .HasForeignKey(e => e.CashierShiftId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CanteenTable)
                  .WithMany()
                  .HasForeignKey(e => e.CanteenTableId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DeliveryZone)
                  .WithMany()
                  .HasForeignKey(e => e.DeliveryZoneId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PickUpPoint)
                  .WithMany()
                  .HasForeignKey(e => e.PickUpPointId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DeliveryEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.DeliveryEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Student)
                  .WithMany()
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CanteenOrderItem entity
        modelBuilder.Entity<CanteenOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.Order)
                  .WithMany(o => o.Items)
                  .HasForeignKey(e => e.CanteenOrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey(e => e.CanteenProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Variant)
                  .WithMany()
                  .HasForeignKey(e => e.VariantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CanteenOrderItemExtra entity
        modelBuilder.Entity<CanteenOrderItemExtra>(entity =>
        {
            entity.HasKey(e => new { e.CanteenOrderItemId, e.CanteenProductExtraId });
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(e => e.OrderItem)
                  .WithMany(oi => oi.Extras)
                  .HasForeignKey(e => e.CanteenOrderItemId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Extra)
                  .WithMany()
                  .HasForeignKey(e => e.CanteenProductExtraId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure DeliveryZone entity
        modelBuilder.Entity<DeliveryZone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinimumOrderAmount).HasDefaultValue(0);
            entity.Property(e => e.EstimatedDeliveryTime).HasDefaultValue(30);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Branch)
                  .WithMany()
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PickUpPoint entity
        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NameAr).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DeliveryZone)
                  .WithMany(dz => dz.PickUpPoints)
                  .HasForeignKey(e => e.DeliveryZoneId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CanteenPayment entity
        modelBuilder.Entity<CanteenPayment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransactionRef).HasMaxLength(100);
            entity.Property(e => e.ProcessedBy).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.StudentNumber).HasMaxLength(50);
            entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

            entity.HasOne(e => e.Order)
                  .WithOne(o => o.Payment)
                  .HasForeignKey<CanteenPayment>(e => e.CanteenOrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureAdmissionsEntities(ModelBuilder modelBuilder)
    {
        // Configure AdmissionApplication entity
        modelBuilder.Entity<AdmissionApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApplicationNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AcademicYear).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FullNameArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FullNameEnglish).HasMaxLength(200);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.PassportNumber).HasMaxLength(50);
            entity.Property(e => e.Religion).HasMaxLength(50);
            entity.Property(e => e.CivilId).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.GuardianFirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.GuardianLastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.GuardianFullNameArabic).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Relationship).HasMaxLength(50);
            entity.Property(e => e.RelationshipArabic).HasMaxLength(50);
            entity.Property(e => e.GuardianNationalId).HasMaxLength(50);
            entity.Property(e => e.GuardianPhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.GuardianWhatsAppNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.GuardianEmail).HasMaxLength(200);
            entity.Property(e => e.GuardianOccupation).HasMaxLength(100);
            entity.Property(e => e.GuardianOccupationArabic).HasMaxLength(100);
            entity.Property(e => e.GuardianWorkplace).HasMaxLength(200);
            entity.Property(e => e.GuardianWorkAddress).HasMaxLength(500);
            entity.Property(e => e.GuardianAddress).HasMaxLength(500);
            entity.Property(e => e.PreviousSchool).HasMaxLength(200);
            entity.Property(e => e.PreviousSchoolArabic).HasMaxLength(200);
            entity.Property(e => e.PreviousSchoolCity).HasMaxLength(100);
            entity.Property(e => e.TransferReason).HasMaxLength(500);
            entity.Property(e => e.TransferReasonArabic).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.RejectionReasonArabic).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Priority).HasDefaultValue(1);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.GradeLevel)
                  .WithMany()
                  .HasForeignKey(e => e.GradeLevelId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Section)
                  .WithMany()
                  .HasForeignKey(e => e.SectionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SubmittedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.SubmittedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ReviewedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.ReviewedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ConvertedStudent)
                  .WithOne()
                  .HasForeignKey<AdmissionApplication>(e => e.ConvertedStudentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure AdmissionExam entity
        modelBuilder.Entity<AdmissionExam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InterviewLocation).HasMaxLength(200);
            entity.Property(e => e.InterviewNotes).HasMaxLength(1000);
            entity.Property(e => e.ExamLocation).HasMaxLength(200);
            entity.Property(e => e.ExamNotes).HasMaxLength(1000);
            entity.Property(e => e.MedicalNotes).HasMaxLength(1000);
            entity.Property(e => e.BehavioralNotes).HasMaxLength(1000);
            entity.Property(e => e.FinalRecommendation).HasMaxLength(1000);
            entity.Property(e => e.FinalRecommendationArabic).HasMaxLength(1000);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AdmissionApplication)
                  .WithOne(a => a.Exam)
                  .HasForeignKey<AdmissionExam>(e => e.AdmissionApplicationId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Interviewer)
                  .WithMany()
                  .HasForeignKey(e => e.InterviewerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Examiner)
                  .WithMany()
                  .HasForeignKey(e => e.ExaminerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EvaluatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.EvaluatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ApplicationDocument entity
        modelBuilder.Entity<ApplicationDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileType).HasMaxLength(100);
            entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.VerificationNotes).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsRequired).HasDefaultValue(true);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);

            entity.HasOne(e => e.AdmissionApplication)
                  .WithMany(a => a.Documents)
                  .HasForeignKey(e => e.AdmissionApplicationId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.VerifiedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.VerifiedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UploadedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.UploadedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureClinicEntities(ModelBuilder modelBuilder)
    {
        // Configure StudentHealthProfile entity
        modelBuilder.Entity<StudentHealthProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BloodType).HasMaxLength(10);
            entity.Property(e => e.RhFactor).HasMaxLength(10);
            entity.Property(e => e.Allergies).HasMaxLength(1000);
            entity.Property(e => e.AllergiesArabic).HasMaxLength(1000);
            entity.Property(e => e.ProhibitedFoods).HasMaxLength(1000);
            entity.Property(e => e.ProhibitedFoodsArabic).HasMaxLength(1000);
            entity.Property(e => e.ChronicDiseases).HasMaxLength(1000);
            entity.Property(e => e.ChronicDiseasesArabic).HasMaxLength(1000);
            entity.Property(e => e.PhysicalDisabilities).HasMaxLength(1000);
            entity.Property(e => e.PhysicalDisabilitiesArabic).HasMaxLength(1000);
            entity.Property(e => e.Vaccinations).HasMaxLength(2000);
            entity.Property(e => e.VaccinationsArabic).HasMaxLength(2000);
            entity.Property(e => e.DietaryRestrictions).HasMaxLength(1000);
            entity.Property(e => e.DietaryRestrictionsArabic).HasMaxLength(1000);
            entity.Property(e => e.AdditionalNotes).HasMaxLength(2000);
            entity.Property(e => e.AdditionalNotesArabic).HasMaxLength(2000);
            entity.Property(e => e.SpecialCareType).HasMaxLength(200);
            entity.Property(e => e.SpecialCareTypeArabic).HasMaxLength(200);
            entity.Property(e => e.TreatingPhysician).HasMaxLength(200);
            entity.Property(e => e.TreatingPhysicianArabic).HasMaxLength(200);
            entity.Property(e => e.PhysicianPhone).HasMaxLength(20);
            entity.Property(e => e.PreferredHospital).HasMaxLength(200);
            entity.Property(e => e.InsuranceNumber).HasMaxLength(50);
            entity.Property(e => e.InsuranceCompany).HasMaxLength(200);

            entity.HasOne(e => e.Student)
                  .WithOne(s => s.HealthProfile)
                  .HasForeignKey<StudentHealthProfile>(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ClinicVisit entity
        modelBuilder.Entity<ClinicVisit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VisitReason).HasMaxLength(500);
            entity.Property(e => e.VisitReasonArabic).HasMaxLength(500);
            entity.Property(e => e.VisitType).HasMaxLength(50);
            entity.Property(e => e.VisitTypeArabic).HasMaxLength(50);
            entity.Property(e => e.Symptoms).HasMaxLength(1000);
            entity.Property(e => e.SymptomsArabic).HasMaxLength(1000);
            entity.Property(e => e.PreliminaryDiagnosis).HasMaxLength(1000);
            entity.Property(e => e.PreliminaryDiagnosisArabic).HasMaxLength(1000);
            entity.Property(e => e.Treatment).HasMaxLength(2000);
            entity.Property(e => e.TreatmentArabic).HasMaxLength(2000);
            entity.Property(e => e.MedicationPrescribed).HasMaxLength(1000);
            entity.Property(e => e.MedicationPrescribedArabic).HasMaxLength(1000);
            entity.Property(e => e.Dosage).HasMaxLength(200);
            entity.Property(e => e.DosageArabic).HasMaxLength(200);
            entity.Property(e => e.Duration).HasMaxLength(200);
            entity.Property(e => e.DurationArabic).HasMaxLength(200);
            entity.Property(e => e.PatientCondition).HasMaxLength(200);
            entity.Property(e => e.PatientConditionArabic).HasMaxLength(200);
            entity.Property(e => e.ReferredHospital).HasMaxLength(200);
            entity.Property(e => e.ReferredHospitalArabic).HasMaxLength(200);
            entity.Property(e => e.ReferralReason).HasMaxLength(500);
            entity.Property(e => e.ReferralReasonArabic).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.NotesArabic).HasMaxLength(2000);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Student)
                  .WithMany()
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AttendingEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.AttendingEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure MedicalItem entity
        modelBuilder.Entity<MedicalItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ItemName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ItemNameArabic).HasMaxLength(200);
            entity.Property(e => e.ItemType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ItemTypeArabic).HasMaxLength(50);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.ManufacturerArabic).HasMaxLength(200);
            entity.Property(e => e.DrugCode).HasMaxLength(50);
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
            entity.Property(e => e.UnitArabic).HasMaxLength(20);
            entity.Property(e => e.StorageLocation).HasMaxLength(200);
            entity.Property(e => e.StorageLocationArabic).HasMaxLength(200);
            entity.Property(e => e.RequiredTemperature).HasMaxLength(50);
            entity.Property(e => e.StorageType).HasMaxLength(50);
            entity.Property(e => e.StorageTypeArabic).HasMaxLength(50);
            entity.Property(e => e.RecommendedDosage).HasMaxLength(500);
            entity.Property(e => e.RecommendedDosageArabic).HasMaxLength(500);
            entity.Property(e => e.SideEffects).HasMaxLength(1000);
            entity.Property(e => e.SideEffectsArabic).HasMaxLength(1000);
            entity.Property(e => e.DrugInteractions).HasMaxLength(1000);
            entity.Property(e => e.DrugInteractionsArabic).HasMaxLength(1000);
            entity.Property(e => e.Contraindications).HasMaxLength(1000);
            entity.Property(e => e.ContraindicationsArabic).HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.NotesArabic).HasMaxLength(1000);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UpdatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.UpdatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure MedicalExcuse entity
        modelBuilder.Entity<MedicalExcuse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExcuseType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ExcuseTypeArabic).HasMaxLength(50);
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ReasonArabic).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.DescriptionArabic).HasMaxLength(2000);
            entity.Property(e => e.PhysicalActivityRecommendations).HasMaxLength(1000);
            entity.Property(e => e.PhysicalActivityRecommendationsArabic).HasMaxLength(1000);
            entity.Property(e => e.DietaryRecommendations).HasMaxLength(1000);
            entity.Property(e => e.DietaryRecommendationsArabic).HasMaxLength(1000);
            entity.Property(e => e.PhysicalActivityRestrictionDuration).HasMaxLength(200);
            entity.Property(e => e.PhysicalActivityRestrictionDurationArabic).HasMaxLength(200);
            entity.Property(e => e.RestrictedActivities).HasMaxLength(1000);
            entity.Property(e => e.RestrictedActivitiesArabic).HasMaxLength(1000);
            entity.Property(e => e.FollowUpNotes).HasMaxLength(1000);
            entity.Property(e => e.FollowUpNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.PhysicianName).HasMaxLength(200);
            entity.Property(e => e.PhysicianNameArabic).HasMaxLength(200);
            entity.Property(e => e.PhysicianLicenseNumber).HasMaxLength(50);
            entity.Property(e => e.HospitalName).HasMaxLength(200);
            entity.Property(e => e.HospitalNameArabic).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.NotesArabic).HasMaxLength(1000);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Student)
                  .WithMany()
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ClinicVisit)
                  .WithMany()
                  .HasForeignKey(e => e.ClinicVisitId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.IssuedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.IssuedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureAlumniEntities(ModelBuilder modelBuilder)
    {
        // Configure AlumniRecord entity
        modelBuilder.Entity<AlumniRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GradeLevel).HasMaxLength(100);
            entity.Property(e => e.GradeLevelArabic).HasMaxLength(100);
            entity.Property(e => e.Section).HasMaxLength(50);
            entity.Property(e => e.SectionArabic).HasMaxLength(50);
            entity.Property(e => e.University).HasMaxLength(200);
            entity.Property(e => e.UniversityArabic).HasMaxLength(200);
            entity.Property(e => e.Major).HasMaxLength(200);
            entity.Property(e => e.MajorArabic).HasMaxLength(200);
            entity.Property(e => e.EmploymentStatus).HasMaxLength(50);
            entity.Property(e => e.EmploymentStatusArabic).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.CompanyNameArabic).HasMaxLength(200);
            entity.Property(e => e.JobTitle).HasMaxLength(200);
            entity.Property(e => e.JobTitleArabic).HasMaxLength(200);
            entity.Property(e => e.Industry).HasMaxLength(100);
            entity.Property(e => e.IndustryArabic).HasMaxLength(100);
            entity.Property(e => e.PersonalEmail).HasMaxLength(200);
            entity.Property(e => e.PersonalPhone).HasMaxLength(20);
            entity.Property(e => e.LinkedInProfile).HasMaxLength(500);
            entity.Property(e => e.Website).HasMaxLength(500);
            entity.Property(e => e.CurrentAddress).HasMaxLength(500);
            entity.Property(e => e.CurrentAddressArabic).HasMaxLength(500);
            entity.Property(e => e.Achievements).HasMaxLength(2000);
            entity.Property(e => e.AchievementsArabic).HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.NotesArabic).HasMaxLength(2000);

            entity.HasOne(e => e.Student)
                  .WithOne()
                  .HasForeignKey<AlumniRecord>(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => !e.School.IsDeleted);
        });

        // Configure GraduationDocument entity
        modelBuilder.Entity<GraduationDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DocumentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DocumentTypeArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DescriptionArabic).HasMaxLength(500);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(50);
            entity.Property(e => e.DeliveryMethod).HasMaxLength(50);
            entity.Property(e => e.ReceivedBy).HasMaxLength(200);
            entity.Property(e => e.ReceivedByArabic).HasMaxLength(200);
            entity.Property(e => e.FinancialClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.AdministrativeClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.NotesArabic).HasMaxLength(1000);

            entity.HasOne(e => e.AlumniRecord)
                  .WithMany(a => a.GraduationDocuments)
                  .HasForeignKey(e => e.AlumniRecordId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure GraduationClearanceRecord entity
        modelBuilder.Entity<GraduationClearanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GradeLevel).HasMaxLength(100);
            entity.Property(e => e.GradeLevelArabic).HasMaxLength(100);
            entity.Property(e => e.ClearanceStatus).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ClearanceStatusArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FinancialClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.FinancialClearanceNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.AdministrativeClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.AdministrativeClearanceNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.AcademicClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.AcademicClearanceNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.ClinicClearanceNotes).HasMaxLength(1000);
            entity.Property(e => e.ClinicClearanceNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.ApprovedByEmployeeName).HasMaxLength(200);
            entity.Property(e => e.FinalApprovalNotes).HasMaxLength(1000);
            entity.Property(e => e.FinalApprovalNotesArabic).HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.NotesArabic).HasMaxLength(1000);

            entity.HasOne(e => e.Student)
                  .WithOne()
                  .HasForeignKey<GraduationClearanceRecord>(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByEmployee)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Query filter handled through student relationship
        });
    }

    private void ConfigureLeaveEntities(ModelBuilder modelBuilder)
    {
        // Configure LeaveRequest entity
        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LeaveType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LeaveTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsPaid).HasDefaultValue(true);
            entity.Property(e => e.LeaveNumber).HasMaxLength(20);
            
            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.LeaveRequests)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure LeaveBalance entity
        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LeaveType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LeaveTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.LeaveBalances)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure LeaveRequest entity
        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LeaveType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LeaveTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.StatusArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsPaid).HasDefaultValue(true);
            entity.Property(e => e.LeaveNumber).HasMaxLength(20);
            
            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.LeaveRequests)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure LeaveBalance entity
        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LeaveType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LeaveTypeArabic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.LeaveBalances)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Transport relationships
        modelBuilder.Entity<BusRoute>()
            .HasMany(r => r.Stops)
            .WithOne(s => s.Route)
            .HasForeignKey(s => s.RouteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BusSchedule>()
            .HasOne(s => s.Bus)
            .WithMany(b => b.Schedules)
            .HasForeignKey(s => s.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusSchedule>()
            .HasOne(s => s.Route)
            .WithMany(r => r.Schedules)
            .HasForeignKey(s => s.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Driver>()
            .HasOne(d => d.AssignedBus)
            .WithMany(b => b.Drivers)
            .HasForeignKey(d => d.AssignedBusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusSupervisor>()
            .HasOne(s => s.AssignedBus)
            .WithMany(b => b.Supervisors)
            .HasForeignKey(s => s.AssignedBusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentTransportSubscription>()
            .HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentTransportSubscription>()
            .HasOne(s => s.BusSchedule)
            .WithMany(b => b.Subscriptions)
            .HasForeignKey(s => s.BusScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentTransportSubscription>()
            .HasOne(s => s.RouteStop)
            .WithMany(r => r.Subscriptions)
            .HasForeignKey(s => s.RouteStopId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusTrackingLog>()
            .HasOne(l => l.Bus)
            .WithMany(b => b.TrackingLogs)
            .HasForeignKey(l => l.BusId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BusAttendance>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusAttendance>()
            .HasOne(a => a.BusSchedule)
            .WithMany(b => b.AttendanceRecords)
            .HasForeignKey(a => a.BusScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BusIncident>()
            .HasOne(i => i.Bus)
            .WithMany(b => b.Incidents)
            .HasForeignKey(i => i.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FuelRecord>()
            .HasOne(f => f.Bus)
            .WithMany(b => b.FuelRecords)
            .HasForeignKey(f => f.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.Bus)
            .WithMany(b => b.MaintenanceRecords)
            .HasForeignKey(m => m.BusId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // MasarUser indexes
        modelBuilder.Entity<MasarUser>()
            .HasIndex(u => u.UserName)
            .IsUnique()
            .HasFilter("IsActive = 1");

        modelBuilder.Entity<MasarUser>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("IsActive = 1");

        modelBuilder.Entity<MasarUser>()
            .HasIndex(u => u.NationalId)
            .HasFilter("IsActive = 1");

        // Tenant indexes
        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.LicenseNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        // School indexes
        modelBuilder.Entity<School>()
            .HasIndex(s => s.NoorSchoolId)
            .HasFilter("IsDeleted = 0");

        // Student indexes
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.NationalId)
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentNumber)
            .HasFilter("IsDeleted = 0");

        // Invoice indexes
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.ZatcaInvoiceUuid)
            .HasFilter("IsDeleted = 0");

        // Financial Accounting indexes
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountCode)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<JournalEntry>()
            .HasIndex(j => j.EntryNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<JournalEntry>()
            .HasIndex(j => j.EntryDate)
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<StudentAccount>()
            .HasIndex(s => s.AccountNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<StudentInvoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<StudentInvoice>()
            .HasIndex(i => i.UUID)
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<StudentPayment>()
            .HasIndex(p => p.PaymentNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<Discount>()
            .HasIndex(d => d.DiscountCode)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        // AI Engine indexes
        modelBuilder.Entity<StudentRiskScore>()
            .HasIndex(r => r.StudentId);

        modelBuilder.Entity<StudentRiskScore>()
            .HasIndex(r => r.CalculatedAt);

        modelBuilder.Entity<StudentRiskScore>()
            .HasIndex(r => r.RiskLevel);

        modelBuilder.Entity<StudentRiskScore>()
            .HasIndex(r => new { r.StudentId, r.CalculatedAt });

        modelBuilder.Entity<RiskAlertHistory>()
            .HasIndex(r => r.StudentId);

        modelBuilder.Entity<RiskAlertHistory>()
            .HasIndex(r => r.AlertType);

        modelBuilder.Entity<RiskAlertHistory>()
            .HasIndex(r => r.Status);

        // Student Records indexes
        modelBuilder.Entity<AcademicRecord>()
            .HasIndex(a => a.StudentId);

        modelBuilder.Entity<AcademicRecord>()
            .HasIndex(a => a.AcademicYear);

        modelBuilder.Entity<AcademicRecord>()
            .HasIndex(a => new { a.StudentId, a.AcademicYear });

        modelBuilder.Entity<StudentDocument>()
            .HasIndex(d => d.StudentId);

        modelBuilder.Entity<StudentDocument>()
            .HasIndex(d => d.DocumentType);

        modelBuilder.Entity<StudentDocument>()
            .HasIndex(d => d.Category);

        modelBuilder.Entity<StudentDocument>()
            .HasIndex(d => d.ExpiryDate);

        // Roll Call indexes
        modelBuilder.Entity<RollCallSession>()
            .HasIndex(r => r.ClassRoomId);

        modelBuilder.Entity<RollCallSession>()
            .HasIndex(r => r.TeacherId);

        modelBuilder.Entity<RollCallSession>()
            .HasIndex(r => r.SessionDate);

        modelBuilder.Entity<RollCallSession>()
            .HasIndex(r => new { r.ClassRoomId, r.SessionDate });

        modelBuilder.Entity<RollCallRecord>()
            .HasIndex(r => r.SessionId);

        modelBuilder.Entity<RollCallRecord>()
            .HasIndex(r => r.StudentId);

        modelBuilder.Entity<RollCallRecord>()
            .HasIndex(r => r.Status);

        // Transport Management indexes
        modelBuilder.Entity<Bus>()
            .HasIndex(b => b.BusNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<Bus>()
            .HasIndex(b => b.PlateNumber)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<BusRoute>()
            .HasIndex(r => r.RouteCode)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<Driver>()
            .HasIndex(d => d.LicenseNumber)
            .HasFilter("IsDeleted = 0");

        modelBuilder.Entity<BusSchedule>()
            .HasIndex(s => s.BusId);

        modelBuilder.Entity<BusSchedule>()
            .HasIndex(s => s.RouteId);

        modelBuilder.Entity<BusSchedule>()
            .HasIndex(s => s.DepartureTime);

        modelBuilder.Entity<StudentTransportSubscription>()
            .HasIndex(s => s.StudentId);

        modelBuilder.Entity<StudentTransportSubscription>()
            .HasIndex(s => s.BusScheduleId);

        modelBuilder.Entity<BusTrackingLog>()
            .HasIndex(l => l.BusId);

        modelBuilder.Entity<BusTrackingLog>()
            .HasIndex(l => l.Timestamp);

        modelBuilder.Entity<BusAttendance>()
            .HasIndex(a => a.StudentId);

        modelBuilder.Entity<BusAttendance>()
            .HasIndex(a => a.AttendanceDate);

        modelBuilder.Entity<BusIncident>()
            .HasIndex(i => i.BusId);

        modelBuilder.Entity<BusIncident>()
            .HasIndex(i => i.IncidentDate);

        modelBuilder.Entity<FuelRecord>()
            .HasIndex(f => f.BusId);

        modelBuilder.Entity<FuelRecord>()
            .HasIndex(f => f.RefuelDate);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasIndex(m => m.BusId);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasIndex(m => m.MaintenanceDate);
    }

    private void SeedGradeLevels(ModelBuilder modelBuilder)
    {
        var seedDate = DateTime.Now;
        
        var gradeLevels = new[]
        {
            new GradeLevel { Id = Guid.NewGuid(), Name = "First Grade", NameArabic = "الصف الأول", Code = "G1", DisplayOrder = 1, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Second Grade", NameArabic = "الصف الثاني", Code = "G2", DisplayOrder = 2, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Third Grade", NameArabic = "الصف الثالث", Code = "G3", DisplayOrder = 3, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Fourth Grade", NameArabic = "الصف الرابع", Code = "G4", DisplayOrder = 4, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Fifth Grade", NameArabic = "الصف الخامس", Code = "G5", DisplayOrder = 5, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Sixth Grade", NameArabic = "الصف السادس", Code = "G6", DisplayOrder = 6, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "First Middle", NameArabic = "الصف الأول المتوسط", Code = "M1", DisplayOrder = 7, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Second Middle", NameArabic = "الصف الثاني المتوسط", Code = "M2", DisplayOrder = 8, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Third Middle", NameArabic = "الصف الثالث المتوسط", Code = "M3", DisplayOrder = 9, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "First Secondary", NameArabic = "الصف الأول الثانوي", Code = "H1", DisplayOrder = 10, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Second Secondary", NameArabic = "الصف الثاني الثانوي", Code = "H2", DisplayOrder = 11, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new GradeLevel { Id = Guid.NewGuid(), Name = "Third Secondary", NameArabic = "الصف الثالث الثانوي", Code = "H3", DisplayOrder = 12, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" }
        };
        
        modelBuilder.Entity<GradeLevel>().HasData(gradeLevels);
    }
    
    private void SeedSections(ModelBuilder modelBuilder)
    {
        var seedDate = DateTime.Now;
        
        var sections = new[]
        {
            new Section { Id = Guid.NewGuid(), Name = "Section A", NameArabic = "شعبة أ", Code = "A", DisplayOrder = 1, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new Section { Id = Guid.NewGuid(), Name = "Section B", NameArabic = "شعبة ب", Code = "B", DisplayOrder = 2, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new Section { Id = Guid.NewGuid(), Name = "Section C", NameArabic = "شعبة ج", Code = "C", DisplayOrder = 3, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new Section { Id = Guid.NewGuid(), Name = "Section D", NameArabic = "شعبة د", Code = "D", DisplayOrder = 4, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" },
            new Section { Id = Guid.NewGuid(), Name = "Section E", NameArabic = "شعبة هـ", Code = "E", DisplayOrder = 5, IsActive = true, CreatedAt = seedDate, CreatedBy = "System" }
        };
        
        modelBuilder.Entity<Section>().HasData(sections);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Super Admin
        var superAdminId = new Guid("00000000-0000-0000-0000-000000000001");
        var superAdminRoleId = new Guid("00000000-0000-0000-0000-000000000002");
        var masarTenantId = new Guid("00000000-0000-0000-0000-000000000003");

        // Seed Masar Tenant
        modelBuilder.Entity<Tenant>().HasData(new Tenant
        {
            Id = masarTenantId,
            Name = "Masar Schools System",
            NameArabic = "نظام مَسَار للمدارس",
            LicenseNumber = "MASAR-SYSTEM-001",
            IsActive = true,
            SubscriptionStartDate = new DateTime(2026, 1, 1),
            SubscriptionEndDate = new DateTime(2036, 1, 1),
            CreatedAt = new DateTime(2026, 1, 1),
            CreatedBy = "System"
        });

        // Seed Super Admin Role
        modelBuilder.Entity<MasarRole>().HasData(new MasarRole
        {
            Id = superAdminRoleId,
            Name = "Super Admin",
            NormalizedName = "SUPER ADMIN",
            Description = "System-wide administrator with full access",
            DescriptionArabic = "مشرف النظام بصلاحيات كاملة",
            IsActive = true,
            TenantId = masarTenantId,
            ConcurrencyStamp = "INITIAL_SEED"
        });

        // Seed Super Admin User
        // Password: Admin@123 (properly hashed using ASP.NET Core Identity)
        var passwordHash = "AQAAAAIAAYagAAAAEAz7yeP4lkZ0E5qgnAXGXtIwsK3YN6MG7vfmIfmVt1IF5+n8+WTWo3Tbdr1EfVbjNg=="; 
        modelBuilder.Entity<MasarUser>().HasData(new MasarUser
        {
            Id = superAdminId,
            UserName = "superadmin",
            NormalizedUserName = "SUPERADMIN",
            Email = "superadmin@masar.sa",
            NormalizedEmail = "SUPERADMIN@MASAR.SA",
            PasswordHash = passwordHash,
            FirstName = "Super",
            FirstNameArabic = "مشرف",
            LastName = "Admin",
            LastNameArabic = "عام",
            FullName = "Super Admin",
            FullNameArabic = "المشرف العام",
            IsActive = true,
            IsSuperAdmin = true,
            TenantId = masarTenantId,
            SecurityStamp = "INITIAL_SECURITY_STAMP",
            ConcurrencyStamp = "INITIAL_CONCURRENCY_STAMP"
        });

        // Seed User-Role relationship
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            UserId = superAdminId,
            RoleId = superAdminRoleId
        });

        // Seed Chart of Accounts (دليل الحسابات المالي)
        SeedChartOfAccounts(modelBuilder);
    }

    private void SeedChartOfAccounts(ModelBuilder modelBuilder)
    {
        // Create GUIDs for all accounts
        var assetsId = new Guid("10000000-0000-0000-0000-000000000001");
        var cashId = new Guid("10000000-0000-0000-0000-000000000002");
        var banksId = new Guid("10000000-0000-0000-0000-000000000003");
        var receivableId = new Guid("10000000-0000-0000-0000-000000000004");
        var fixedAssetsId = new Guid("10000000-0000-0000-0000-000000000005");
        var accumulatedDepreciationId = new Guid("10000000-0000-0000-0000-000000000006");
        var liabilitiesId = new Guid("20000000-0000-0000-0000-000000000001");
        var vatId = new Guid("20000000-0000-0000-0000-000000000002");
        var unearnedId = new Guid("20000000-0000-0000-0000-000000000003");
        var payrollPayableId = new Guid("20000000-0000-0000-0000-000000000004");
        var socialSecurityId = new Guid("20000000-0000-0000-0000-000000000005");
        var equityId = new Guid("30000000-0000-0000-0000-000000000001");
        var capitalId = new Guid("30000000-0000-0000-0000-000000000002");
        var retainedEarningsId = new Guid("30000000-0000-0000-0000-000000000003");
        var revenueId = new Guid("40000000-0000-0000-0000-000000000001");
        var tuitionId = new Guid("40000000-0000-0000-0000-000000000002");
        var transportId = new Guid("40000000-0000-0000-0000-000000000003");
        var booksId = new Guid("40000000-0000-0000-0000-000000000004");
        var activitiesId = new Guid("40000000-0000-0000-0000-000000000005");
        var discountsId = new Guid("40000000-0000-0000-0000-000000000006");
        var expensesId = new Guid("50000000-0000-0000-0000-000000000001");
        var adminId = new Guid("50000000-0000-0000-0000-000000000002");
        var operatingId = new Guid("50000000-0000-0000-0000-000000000003");
        var badDebtId = new Guid("50000000-0000-0000-0000-000000000004");
        var salariesId = new Guid("50000000-0000-0000-0000-000000000005");
        
        var seedDate = new DateTime(2026, 1, 1);

        var accounts = new List<Account>
        {
            // الأصول (Assets - 1000)
            new Account
            {
                Id = assetsId,
                AccountId = assetsId,
                AccountCode = "1000",
                AccountNameAr = "الأصول",
                AccountNameEn = "Assets",
                AccountType = AccountType.Asset,
                Level = 1,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = cashId,
                AccountId = cashId,
                AccountCode = "1101",
                AccountNameAr = "الصندوق والخصوم النقدية",
                AccountNameEn = "Cash & Banks",
                AccountType = AccountType.Asset,
                Level = 2,
                ParentAccountId = assetsId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = banksId,
                AccountId = banksId,
                AccountCode = "1102",
                AccountNameAr = "البنوك والمدفوعات الإلكترونية",
                AccountNameEn = "Banks & Electronic Payments",
                AccountType = AccountType.Asset,
                Level = 2,
                ParentAccountId = assetsId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = receivableId,
                AccountId = receivableId,
                AccountCode = "1201",
                AccountNameAr = "مدينو الطلاب / ذمم أولياء الأمور",
                AccountNameEn = "Accounts Receivable - Students",
                AccountType = AccountType.Asset,
                Level = 2,
                ParentAccountId = assetsId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = fixedAssetsId,
                AccountId = fixedAssetsId,
                AccountCode = "1301",
                AccountNameAr = "الأصول الثابتة",
                AccountNameEn = "Fixed Assets",
                AccountType = AccountType.Asset,
                Level = 2,
                ParentAccountId = assetsId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = accumulatedDepreciationId,
                AccountId = accumulatedDepreciationId,
                AccountCode = "1302",
                AccountNameAr = "مجمع الإهلاك",
                AccountNameEn = "Accumulated Depreciation",
                AccountType = AccountType.Asset,
                Level = 2,
                ParentAccountId = assetsId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },

            // الالتزامات (Liabilities - 2000)
            new Account
            {
                Id = liabilitiesId,
                AccountId = liabilitiesId,
                AccountCode = "2000",
                AccountNameAr = "الالتزامات",
                AccountNameEn = "Liabilities",
                AccountType = AccountType.Liability,
                Level = 1,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = vatId,
                AccountId = vatId,
                AccountCode = "2101",
                AccountNameAr = "ضريبة القيمة المضافة المستحقة",
                AccountNameEn = "VAT Payable 15%",
                AccountType = AccountType.Liability,
                Level = 2,
                ParentAccountId = liabilitiesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = unearnedId,
                AccountId = unearnedId,
                AccountCode = "2201",
                AccountNameAr = "الإيرادات غير المحققة / رسوم مقدماً",
                AccountNameEn = "Unearned Tuition Fees",
                AccountType = AccountType.Liability,
                Level = 2,
                ParentAccountId = liabilitiesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = payrollPayableId,
                AccountId = payrollPayableId,
                AccountCode = "2301",
                AccountNameAr = "المرتبات المستحقة",
                AccountNameEn = "Payroll Payable",
                AccountType = AccountType.Liability,
                Level = 2,
                ParentAccountId = liabilitiesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = socialSecurityId,
                AccountId = socialSecurityId,
                AccountCode = "2302",
                AccountNameAr = "التأمينات الاجتماعية المستحقة",
                AccountNameEn = "Social Security Payable",
                AccountType = AccountType.Liability,
                Level = 2,
                ParentAccountId = liabilitiesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },

            // حقوق الملكية (Equity - 3000)
            new Account
            {
                Id = equityId,
                AccountId = equityId,
                AccountCode = "3000",
                AccountNameAr = "حقوق الملكية",
                AccountNameEn = "Equity",
                AccountType = AccountType.Equity,
                Level = 1,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = capitalId,
                AccountId = capitalId,
                AccountCode = "3101",
                AccountNameAr = "رأس المال",
                AccountNameEn = "Capital",
                AccountType = AccountType.Equity,
                Level = 2,
                ParentAccountId = equityId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = retainedEarningsId,
                AccountId = retainedEarningsId,
                AccountCode = "3201",
                AccountNameAr = "الأرباح المحتجزة",
                AccountNameEn = "Retained Earnings",
                AccountType = AccountType.Equity,
                Level = 2,
                ParentAccountId = equityId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },

            // الإيرادات (Revenue - 4000)
            new Account
            {
                Id = revenueId,
                AccountId = revenueId,
                AccountCode = "4000",
                AccountNameAr = "الإيرادات",
                AccountNameEn = "Revenue",
                AccountType = AccountType.Revenue,
                Level = 1,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = tuitionId,
                AccountId = tuitionId,
                AccountCode = "4101",
                AccountNameAr = "إيرادات الرسوم الدراسية",
                AccountNameEn = "Tuition Revenue",
                AccountType = AccountType.Revenue,
                Level = 2,
                ParentAccountId = revenueId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = transportId,
                AccountId = transportId,
                AccountCode = "4102",
                AccountNameAr = "إيرادات النقل والمواصلات",
                AccountNameEn = "Transportation Revenue",
                AccountType = AccountType.Revenue,
                Level = 2,
                ParentAccountId = revenueId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = booksId,
                AccountId = booksId,
                AccountCode = "4103",
                AccountNameAr = "إيرادات الكتب والزي المدرسي",
                AccountNameEn = "Books & Uniforms Revenue",
                AccountType = AccountType.Revenue,
                Level = 2,
                ParentAccountId = revenueId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = activitiesId,
                AccountId = activitiesId,
                AccountCode = "4104",
                AccountNameAr = "إيرادات الأنشطة والاختبارات",
                AccountNameEn = "Activities Revenue",
                AccountType = AccountType.Revenue,
                Level = 2,
                ParentAccountId = revenueId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = discountsId,
                AccountId = discountsId,
                AccountCode = "4201",
                AccountNameAr = "الخصومات والمنح الممنوحة",
                AccountNameEn = "Discounts & Scholarships",
                AccountType = AccountType.Revenue,
                Level = 2,
                ParentAccountId = revenueId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },

            // المصروفات (Expenses - 5000)
            new Account
            {
                Id = expensesId,
                AccountId = expensesId,
                AccountCode = "5000",
                AccountNameAr = "المصروفات",
                AccountNameEn = "Expenses",
                AccountType = AccountType.Expense,
                Level = 1,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = adminId,
                AccountId = adminId,
                AccountCode = "5101",
                AccountNameAr = "المصروفات الإدارية",
                AccountNameEn = "Administrative Expenses",
                AccountType = AccountType.Expense,
                Level = 2,
                ParentAccountId = expensesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = operatingId,
                AccountId = operatingId,
                AccountCode = "5102",
                AccountNameAr = "المصروفات التشغيلية",
                AccountNameEn = "Operating Expenses",
                AccountType = AccountType.Expense,
                Level = 2,
                ParentAccountId = expensesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = badDebtId,
                AccountId = badDebtId,
                AccountCode = "5103",
                AccountNameAr = "مصروفات الديون المعدومة",
                AccountNameEn = "Bad Debt Expenses",
                AccountType = AccountType.Expense,
                Level = 2,
                ParentAccountId = expensesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            },
            new Account
            {
                Id = salariesId,
                AccountId = salariesId,
                AccountCode = "5201",
                AccountNameAr = "مرتبات الموظفين",
                AccountNameEn = "Salaries & Wages",
                AccountType = AccountType.Expense,
                Level = 2,
                ParentAccountId = expensesId,
                Balance = 0,
                IsActive = true,
                CreatedAt = seedDate,
                CreatedBy = "System"
            }
        };

        modelBuilder.Entity<Account>().HasData(accounts);
    }

    private void ConfigureHREntities(ModelBuilder modelBuilder)
    {
        // Configure Contract entity
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContractNumber).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("ساري");
            entity.Property(e => e.StatusArabic).HasMaxLength(50).HasDefaultValue("ساري");
            entity.Property(e => e.JobTitle).HasMaxLength(150);
            entity.Property(e => e.JobTitleArabic).HasMaxLength(150);
            entity.Property(e => e.Department).HasMaxLength(150);
            entity.Property(e => e.DepartmentArabic).HasMaxLength(150);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.Contracts)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EmployeeId, e.ContractNumber }).IsUnique();
        });

        // Configure ContractArchive entity
        modelBuilder.Entity<ContractArchive>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.JobTitle).HasMaxLength(150);
            entity.Property(e => e.JobTitleArabic).HasMaxLength(150);
            entity.Property(e => e.Department).HasMaxLength(150);
            entity.Property(e => e.DepartmentArabic).HasMaxLength(150);
            entity.Property(e => e.ArchiveReason).HasMaxLength(500);
            entity.Property(e => e.ArchiveReasonArabic).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PayrollProfile entity
        modelBuilder.Entity<PayrollProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GosiScheme).HasMaxLength(20).HasDefaultValue("auto");
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.PayrollProfiles)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PayrollArchive entity
        modelBuilder.Entity<PayrollArchive>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmployeeName).HasMaxLength(200);
            entity.Property(e => e.EmployeeNameArabic).HasMaxLength(200);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.GosiScheme).HasMaxLength(20).HasDefaultValue("auto");
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure AttendanceLog entity
        modelBuilder.Entity<AttendanceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BiometricId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Direction).HasMaxLength(20).HasDefaultValue("غير مصنف");
            entity.Property(e => e.Source).HasMaxLength(50).HasDefaultValue("يدوي");
            entity.Property(e => e.DeviceName).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.AttendanceLogs)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.BiometricId, e.EventAt, e.Direction, e.Source }).IsUnique();
        });

        // Configure AttendancePermit entity
        modelBuilder.Entity<AttendancePermit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasOne(e => e.Employee)
                  .WithMany(e => e.AttendancePermits)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ResignedEmployee entity
        modelBuilder.Entity<ResignedEmployee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.FullNameArabic).HasMaxLength(200);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.JobTitle).HasMaxLength(150);
            entity.Property(e => e.JobTitleArabic).HasMaxLength(150);
            entity.Property(e => e.Department).HasMaxLength(150);
            entity.Property(e => e.DepartmentArabic).HasMaxLength(150);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.ReasonArabic).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Employee)
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure LeaveType entity
        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Nationality entity
        modelBuilder.Entity<Nationality>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.NameArabic).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.School)
                  .WithMany()
                  .HasForeignKey(e => e.SchoolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Employee extended relationships
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.MasarUser)
            .WithMany()
            .HasForeignKey(e => e.MasarUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Add query filters for HR entities
        modelBuilder.Entity<Contract>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<ContractArchive>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<PayrollProfile>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<PayrollArchive>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<AttendanceLog>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<AttendancePermit>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<ResignedEmployee>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<LeaveType>().HasQueryFilter(l => l.IsActive);
        modelBuilder.Entity<Nationality>().HasQueryFilter(n => n.IsActive);
    }
}
