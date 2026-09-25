using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Infrastructure.Interfaces;

/// <summary>
/// واجهة خدمة الإجازات
/// </summary>
public interface ILeaveService
{
    // Leave Request Operations
    Task<List<LeaveRequestDto>> GetAllLeaveRequestsAsync();
    Task<LeaveRequestDto?> GetLeaveRequestByIdAsync(Guid id);
    Task<LeaveRequestDto> CreateLeaveRequestAsync(LeaveRequestCreateDto dto);
    Task<LeaveRequestDto?> UpdateLeaveRequestAsync(Guid id, LeaveRequestCreateDto dto);
    Task<bool> DeleteLeaveRequestAsync(Guid id);
    Task<List<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId);
    Task<List<LeaveRequestDto>> GetLeaveRequestsBySchoolIdAsync(Guid schoolId);
    Task<List<LeaveRequestDto>> GetPendingLeaveRequestsAsync();
    Task<LeaveRequestDto?> ApproveLeaveRequestAsync(Guid id, LeaveRequestActionDto actionDto);
    Task<LeaveRequestDto?> RejectLeaveRequestAsync(Guid id, LeaveRequestActionDto actionDto);

    // Leave Balance Operations
    Task<List<LeaveBalanceDto>> GetAllLeaveBalancesAsync();
    Task<LeaveBalanceDto?> GetLeaveBalanceByIdAsync(Guid id);
    Task<LeaveBalanceDto> CreateLeaveBalanceAsync(LeaveBalanceCreateDto dto);
    Task<LeaveBalanceDto?> UpdateLeaveBalanceAsync(Guid id, LeaveBalanceCreateDto dto);
    Task<bool> DeleteLeaveBalanceAsync(Guid id);
    Task<List<LeaveBalanceDto>> GetLeaveBalancesByEmployeeIdAsync(Guid employeeId);
    Task<LeaveBalanceDto?> GetLeaveBalanceByEmployeeAndTypeAsync(Guid employeeId, string leaveType);
    Task<bool> UpdateLeaveBalanceAsync(Guid employeeId, string leaveType, decimal days);

    // HR Extended Leave Operations (21/30 graduated system)
    Task<decimal> CalculateAnnualEntitlementAsync(Guid employeeId, int yearNumber);
    Task<LeaveBalanceCalculation> CalculateLeaveBalanceAsync(Guid employeeId, string calculationMode);
    Task<decimal> CalculateCarryoverAsync(Guid employeeId);
}

/// <summary>
/// حساب رصيد الإجازة
/// </summary>
public class LeaveBalanceCalculation
{
    public decimal ThisYearEntitlement { get; set; }
    public decimal CarryoverRemaining { get; set; }
    public decimal Used { get; set; }
    public decimal TotalRemaining { get; set; }
    public int ServiceYears { get; set; }
}
