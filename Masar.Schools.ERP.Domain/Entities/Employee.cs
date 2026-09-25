using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameArabic { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameArabic { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string FullNameArabic { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? JobTitle { get; set; }
    public string? JobTitleArabic { get; set; }
    public string? Department { get; set; }
    public string? DepartmentArabic { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public decimal? Salary { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public bool IsActive { get; set; }
    public string? Address { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? FingerprintTemplate { get; set; }
    
    // Additional HR fields
    public string? Gender { get; set; }
    public string? GenderArabic { get; set; }
    public string? Nationality { get; set; }
    public string? NationalityArabic { get; set; }
    public DateTime? ContractStart { get; set; }
    public DateTime? ContractEnd { get; set; }
    public bool IsResigned { get; set; }
    public DateTime? ResignDate { get; set; }
    public string? ResignReason { get; set; }
    public string? ResignReasonArabic { get; set; }
    public string? MaritalStatus { get; set; }
    public string? MaritalStatusArabic { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Education { get; set; }
    public string? EducationArabic { get; set; }
    public string? Specialty { get; set; }
    public string? SpecialtyArabic { get; set; }
    public string? BiometricId { get; set; }
    
    // Integration with Masar User System
    public Guid? MasarUserId { get; set; }
    public MasarUser? MasarUser { get; set; }
    
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
    
    // HR navigation properties
    public ICollection<HR.Contract> Contracts { get; set; } = new List<HR.Contract>();
    public ICollection<HR.PayrollProfile> PayrollProfiles { get; set; } = new List<HR.PayrollProfile>();
    public ICollection<HR.AttendanceLog> AttendanceLogs { get; set; } = new List<HR.AttendanceLog>();
    public ICollection<HR.AttendancePermit> AttendancePermits { get; set; } = new List<HR.AttendancePermit>();
}
