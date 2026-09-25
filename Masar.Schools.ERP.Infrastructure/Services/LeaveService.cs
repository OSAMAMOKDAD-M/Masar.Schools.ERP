using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة الإجازات
/// </summary>
public class LeaveService : ILeaveService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<LeaveService> _logger;
    private readonly HRLeaveServiceExtension _hrLeaveExtension;

    public LeaveService(MasarDbContext context, ILogger<LeaveService> logger)
    {
        _context = context;
        _logger = logger;
        _hrLeaveExtension = new HRLeaveServiceExtension(context);
    }

    // Leave Request Operations
    public async Task<List<LeaveRequestDto>> GetAllLeaveRequestsAsync()
    {
        var requests = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.School)
            .ToListAsync();

        return requests.Select(lr => new LeaveRequestDto
        {
            Id = lr.Id,
            EmployeeId = lr.EmployeeId,
            SchoolId = lr.SchoolId,
            LeaveType = lr.LeaveType,
            LeaveTypeArabic = lr.LeaveTypeArabic,
            StartDate = lr.StartDate,
            EndDate = lr.EndDate,
            TotalDays = lr.DaysCount,
            Status = lr.Status,
            StatusArabic = lr.StatusArabic,
            IsPaid = lr.IsPaid,
            LeaveNumber = lr.LeaveNumber,
            Reason = lr.Reason,
            ReasonArabic = lr.ReasonArabic,
            RejectionReason = lr.ManagerNotes,
            RejectionReasonArabic = lr.ManagerNotes,
            ApprovedAt = lr.ActionDate,
            RejectedAt = lr.ActionDate,
            ApprovedBy = lr.ActionBy,
            RejectedBy = lr.ActionBy,
            TenantId = null,
            CreatedAt = lr.CreatedAt,
            UpdatedAt = lr.UpdatedAt,
            EmployeeName = lr.Employee?.FirstName + " " + lr.Employee?.LastName,
            SchoolName = lr.School?.Name
        }).ToList();
    }

    public async Task<LeaveRequestDto?> GetLeaveRequestByIdAsync(Guid id)
    {
        var request = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.School)
            .FirstOrDefaultAsync(lr => lr.Id == id);

        if (request == null) return null;

        return new LeaveRequestDto
        {
            Id = request.Id,
            EmployeeId = request.EmployeeId,
            SchoolId = request.SchoolId,
            LeaveType = request.LeaveType,
            LeaveTypeArabic = request.LeaveTypeArabic,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalDays = request.DaysCount,
            Status = request.Status,
            StatusArabic = request.StatusArabic,
            IsPaid = request.IsPaid,
            LeaveNumber = request.LeaveNumber,
            Reason = request.Reason,
            ReasonArabic = request.ReasonArabic,
            RejectionReason = request.ManagerNotes,
            RejectionReasonArabic = request.ManagerNotes,
            ApprovedAt = request.ActionDate,
            RejectedAt = request.ActionDate,
            ApprovedBy = request.ActionBy,
            RejectedBy = request.ActionBy,
            TenantId = null,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            EmployeeName = request.Employee?.FirstName + " " + request.Employee?.LastName,
            SchoolName = request.School?.Name
        };
    }

    public async Task<LeaveRequestDto> CreateLeaveRequestAsync(LeaveRequestCreateDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var daysCount = (int)(dto.EndDate - dto.StartDate).TotalDays + 1;
        var leaveNumber = $"LV-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

        var leaveRequest = new LeaveRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            SchoolId = dto.SchoolId ?? employee.SchoolId,
            LeaveType = dto.LeaveType,
            LeaveTypeArabic = dto.LeaveTypeArabic,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DaysCount = daysCount,
            Status = "Pending",
            StatusArabic = "قيد الانتظار",
            IsPaid = dto.IsPaid,
            LeaveNumber = leaveNumber,
            Reason = dto.Reason,
            ReasonArabic = dto.ReasonArabic,
            CreatedAt = DateTime.UtcNow
        };

        _context.LeaveRequests.Add(leaveRequest);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Created leave request {leaveNumber} for employee {dto.EmployeeId}");

        return await GetLeaveRequestByIdAsync(leaveRequest.Id);
    }

    public async Task<LeaveRequestDto?> UpdateLeaveRequestAsync(Guid id, LeaveRequestCreateDto dto)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null) return null;

        // Only allow updating if status is Pending
        if (request.Status != "Pending")
            throw new Exception("Cannot update a leave request that is not pending");

        var daysCount = (int)(dto.EndDate - dto.StartDate).TotalDays + 1;

        request.LeaveType = dto.LeaveType;
        request.LeaveTypeArabic = dto.LeaveTypeArabic;
        request.StartDate = dto.StartDate;
        request.EndDate = dto.EndDate;
        request.DaysCount = daysCount;
        request.IsPaid = dto.IsPaid;
        request.Reason = dto.Reason;
        request.ReasonArabic = dto.ReasonArabic;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Updated leave request {request.LeaveNumber}");

        return await GetLeaveRequestByIdAsync(id);
    }

    public async Task<bool> DeleteLeaveRequestAsync(Guid id)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null) return false;

        // Only allow deleting if status is Pending
        if (request.Status != "Pending")
            throw new Exception("Cannot delete a leave request that is not pending");

        request.IsDeleted = true;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Deleted leave request {request.LeaveNumber}");

        return true;
    }

    public async Task<List<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId)
    {
        var requests = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.School)
            .Where(lr => lr.EmployeeId == employeeId)
            .ToListAsync();

        return requests.Select(lr => new LeaveRequestDto
        {
            Id = lr.Id,
            EmployeeId = lr.EmployeeId,
            SchoolId = lr.SchoolId,
            LeaveType = lr.LeaveType,
            LeaveTypeArabic = lr.LeaveTypeArabic,
            StartDate = lr.StartDate,
            EndDate = lr.EndDate,
            TotalDays = lr.DaysCount,
            Status = lr.Status,
            StatusArabic = lr.StatusArabic,
            IsPaid = lr.IsPaid,
            LeaveNumber = lr.LeaveNumber,
            Reason = lr.Reason,
            ReasonArabic = lr.ReasonArabic,
            RejectionReason = lr.ManagerNotes,
            RejectionReasonArabic = lr.ManagerNotes,
            ApprovedAt = lr.ActionDate,
            RejectedAt = lr.ActionDate,
            ApprovedBy = lr.ActionBy,
            RejectedBy = lr.ActionBy,
            TenantId = null,
            CreatedAt = lr.CreatedAt,
            UpdatedAt = lr.UpdatedAt,
            EmployeeName = lr.Employee?.FirstName + " " + lr.Employee?.LastName,
            SchoolName = lr.School?.Name
        }).ToList();
    }

    public async Task<List<LeaveRequestDto>> GetLeaveRequestsBySchoolIdAsync(Guid schoolId)
    {
        var requests = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.School)
            .Where(lr => lr.SchoolId == schoolId)
            .ToListAsync();

        return requests.Select(lr => new LeaveRequestDto
        {
            Id = lr.Id,
            EmployeeId = lr.EmployeeId,
            SchoolId = lr.SchoolId,
            LeaveType = lr.LeaveType,
            LeaveTypeArabic = lr.LeaveTypeArabic,
            StartDate = lr.StartDate,
            EndDate = lr.EndDate,
            TotalDays = lr.DaysCount,
            Status = lr.Status,
            StatusArabic = lr.StatusArabic,
            IsPaid = lr.IsPaid,
            LeaveNumber = lr.LeaveNumber,
            Reason = lr.Reason,
            ReasonArabic = lr.ReasonArabic,
            RejectionReason = lr.ManagerNotes,
            RejectionReasonArabic = lr.ManagerNotes,
            ApprovedAt = lr.ActionDate,
            RejectedAt = lr.ActionDate,
            ApprovedBy = lr.ActionBy,
            RejectedBy = lr.ActionBy,
            TenantId = null,
            CreatedAt = lr.CreatedAt,
            UpdatedAt = lr.UpdatedAt,
            EmployeeName = lr.Employee?.FirstName + " " + lr.Employee?.LastName,
            SchoolName = lr.School?.Name
        }).ToList();
    }

    public async Task<List<LeaveRequestDto>> GetPendingLeaveRequestsAsync()
    {
        var requests = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr => lr.School)
            .Where(lr => lr.Status == "Pending")
            .ToListAsync();

        return requests.Select(lr => new LeaveRequestDto
        {
            Id = lr.Id,
            EmployeeId = lr.EmployeeId,
            SchoolId = lr.SchoolId,
            LeaveType = lr.LeaveType,
            LeaveTypeArabic = lr.LeaveTypeArabic,
            StartDate = lr.StartDate,
            EndDate = lr.EndDate,
            TotalDays = lr.DaysCount,
            Status = lr.Status,
            StatusArabic = lr.StatusArabic,
            IsPaid = lr.IsPaid,
            LeaveNumber = lr.LeaveNumber,
            Reason = lr.Reason,
            ReasonArabic = lr.ReasonArabic,
            RejectionReason = lr.ManagerNotes,
            RejectionReasonArabic = lr.ManagerNotes,
            ApprovedAt = lr.ActionDate,
            RejectedAt = lr.ActionDate,
            ApprovedBy = lr.ActionBy,
            RejectedBy = lr.ActionBy,
            TenantId = null,
            CreatedAt = lr.CreatedAt,
            UpdatedAt = lr.UpdatedAt,
            EmployeeName = lr.Employee?.FirstName + " " + lr.Employee?.LastName,
            SchoolName = lr.School?.Name
        }).ToList();
    }

    public async Task<LeaveRequestDto?> ApproveLeaveRequestAsync(Guid id, LeaveRequestActionDto actionDto)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null) return null;

        if (request.Status != "Pending")
            throw new Exception("Cannot approve a leave request that is not pending");

        // Check leave balance
        var currentYear = DateTime.Now.Year;
        var balance = await _context.LeaveBalances
            .FirstOrDefaultAsync(lb => lb.EmployeeId == request.EmployeeId && lb.LeaveType == request.LeaveType && lb.FiscalYear == currentYear);

        if (balance == null)
            throw new Exception("Leave balance not found for this employee and leave type");

        if (balance.RemainingDays < request.DaysCount)
            throw new Exception("Insufficient leave balance");

        // Update request
        request.Status = "Approved";
        request.StatusArabic = "موافق عليه";
        request.ActionDate = DateTime.UtcNow;
        request.ActionBy = actionDto.Status; // Should be user name in real implementation
        request.UpdatedAt = DateTime.UtcNow;

        // Update balance
        balance.UsedDays += request.DaysCount;
        balance.RemainingDays = balance.AnnualAllowance - balance.UsedDays;
        balance.LastUpdated = DateTime.UtcNow;
        balance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Approved leave request {request.LeaveNumber}");

        return await GetLeaveRequestByIdAsync(id);
    }

    public async Task<LeaveRequestDto?> RejectLeaveRequestAsync(Guid id, LeaveRequestActionDto actionDto)
    {
        var request = await _context.LeaveRequests.FindAsync(id);
        if (request == null) return null;

        if (request.Status != "Pending")
            throw new Exception("Cannot reject a leave request that is not pending");

        request.Status = "Rejected";
        request.StatusArabic = "مرفوض";
        request.ActionDate = DateTime.UtcNow;
        request.ActionBy = actionDto.Status; // Should be user name in real implementation
        request.ManagerNotes = actionDto.RejectionReason;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Rejected leave request {request.LeaveNumber}");

        return await GetLeaveRequestByIdAsync(id);
    }

    // Leave Balance Operations
    public async Task<List<LeaveBalanceDto>> GetAllLeaveBalancesAsync()
    {
        var balances = await _context.LeaveBalances
            .Include(lb => lb.Employee)
            .Include(lb => lb.School)
            .ToListAsync();

        return balances.Select(lb => new LeaveBalanceDto
        {
            Id = lb.Id,
            EmployeeId = lb.EmployeeId,
            SchoolId = lb.SchoolId,
            LeaveType = lb.LeaveType,
            LeaveTypeArabic = lb.LeaveTypeArabic,
            TotalDays = lb.AnnualAllowance,
            UsedDays = lb.UsedDays,
            RemainingDays = lb.RemainingDays,
            FiscalYear = lb.FiscalYear,
            IsActive = lb.IsActive,
            TenantId = null,
            CreatedAt = lb.CreatedAt,
            UpdatedAt = lb.UpdatedAt,
            EmployeeName = lb.Employee?.FirstName + " " + lb.Employee?.LastName,
            SchoolName = lb.School?.Name
        }).ToList();
    }

    public async Task<LeaveBalanceDto?> GetLeaveBalanceByIdAsync(Guid id)
    {
        var balance = await _context.LeaveBalances
            .Include(lb => lb.Employee)
            .Include(lb => lb.School)
            .FirstOrDefaultAsync(lb => lb.Id == id);

        if (balance == null) return null;

        return new LeaveBalanceDto
        {
            Id = balance.Id,
            EmployeeId = balance.EmployeeId,
            SchoolId = balance.SchoolId,
            LeaveType = balance.LeaveType,
            LeaveTypeArabic = balance.LeaveTypeArabic,
            TotalDays = balance.AnnualAllowance,
            UsedDays = balance.UsedDays,
            RemainingDays = balance.RemainingDays,
            FiscalYear = balance.FiscalYear,
            IsActive = balance.IsActive,
            TenantId = null,
            CreatedAt = balance.CreatedAt,
            UpdatedAt = balance.UpdatedAt,
            EmployeeName = balance.Employee?.FirstName + " " + balance.Employee?.LastName,
            SchoolName = balance.School?.Name
        };
    }

    public async Task<LeaveBalanceDto> CreateLeaveBalanceAsync(LeaveBalanceCreateDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.EmployeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var currentYear = DateTime.Now.Year;

        var leaveBalance = new LeaveBalance
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            SchoolId = dto.SchoolId ?? employee.SchoolId,
            LeaveType = dto.LeaveType,
            LeaveTypeArabic = dto.LeaveTypeArabic,
            FiscalYear = currentYear,
            AnnualAllowance = dto.TotalDays,
            UsedDays = 0,
            RemainingDays = dto.TotalDays,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.LeaveBalances.Add(leaveBalance);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Created leave balance for employee {dto.EmployeeId}, type {dto.LeaveType}");

        return await GetLeaveBalanceByIdAsync(leaveBalance.Id);
    }

    public async Task<LeaveBalanceDto?> UpdateLeaveBalanceAsync(Guid id, LeaveBalanceCreateDto dto)
    {
        var balance = await _context.LeaveBalances.FindAsync(id);
        if (balance == null) return null;

        balance.LeaveType = dto.LeaveType;
        balance.LeaveTypeArabic = dto.LeaveTypeArabic;
        balance.AnnualAllowance = dto.TotalDays;
        balance.RemainingDays = dto.TotalDays - balance.UsedDays;
        balance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Updated leave balance for employee {dto.EmployeeId}, type {dto.LeaveType}");

        return await GetLeaveBalanceByIdAsync(id);
    }

    public async Task<bool> DeleteLeaveBalanceAsync(Guid id)
    {
        var balance = await _context.LeaveBalances.FindAsync(id);
        if (balance == null) return false;

        balance.IsActive = false;
        balance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Deactivated leave balance for employee {balance.EmployeeId}");

        return true;
    }

    public async Task<List<LeaveBalanceDto>> GetLeaveBalancesByEmployeeIdAsync(Guid employeeId)
    {
        var balances = await _context.LeaveBalances
            .Include(lb => lb.Employee)
            .Include(lb => lb.School)
            .Where(lb => lb.EmployeeId == employeeId && lb.IsActive)
            .ToListAsync();

        return balances.Select(lb => new LeaveBalanceDto
        {
            Id = lb.Id,
            EmployeeId = lb.EmployeeId,
            SchoolId = lb.SchoolId,
            LeaveType = lb.LeaveType,
            LeaveTypeArabic = lb.LeaveTypeArabic,
            TotalDays = lb.AnnualAllowance,
            UsedDays = lb.UsedDays,
            RemainingDays = lb.RemainingDays,
            FiscalYear = lb.FiscalYear,
            IsActive = lb.IsActive,
            TenantId = null,
            CreatedAt = lb.CreatedAt,
            UpdatedAt = lb.UpdatedAt,
            EmployeeName = lb.Employee?.FirstName + " " + lb.Employee?.LastName,
            SchoolName = lb.School?.Name
        }).ToList();
    }

    public async Task<LeaveBalanceDto?> GetLeaveBalanceByEmployeeAndTypeAsync(Guid employeeId, string leaveType)
    {
        var balance = await _context.LeaveBalances
            .Include(lb => lb.Employee)
            .Include(lb => lb.School)
            .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.LeaveType == leaveType && lb.IsActive);

        if (balance == null) return null;

        return new LeaveBalanceDto
        {
            Id = balance.Id,
            EmployeeId = balance.EmployeeId,
            SchoolId = balance.SchoolId,
            LeaveType = balance.LeaveType,
            LeaveTypeArabic = balance.LeaveTypeArabic,
            TotalDays = balance.AnnualAllowance,
            UsedDays = balance.UsedDays,
            RemainingDays = balance.RemainingDays,
            FiscalYear = balance.FiscalYear,
            IsActive = balance.IsActive,
            TenantId = null,
            CreatedAt = balance.CreatedAt,
            UpdatedAt = balance.UpdatedAt,
            EmployeeName = balance.Employee?.FirstName + " " + balance.Employee?.LastName,
            SchoolName = balance.School?.Name
        };
    }

    public async Task<bool> UpdateLeaveBalanceAsync(Guid employeeId, string leaveType, decimal days)
    {
        var currentYear = DateTime.Now.Year;
        var balance = await _context.LeaveBalances
            .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId && lb.LeaveType == leaveType && lb.FiscalYear == currentYear && lb.IsActive);

        if (balance == null) return false;

        balance.UsedDays += days;
        balance.RemainingDays = balance.AnnualAllowance - balance.UsedDays;
        balance.LastUpdated = DateTime.UtcNow;
        balance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Updated leave balance for employee {employeeId}, type {leaveType} by {days} days");

        return true;
    }

    // HR Extended Leave Operations (21/30 graduated system)
    public async Task<decimal> CalculateAnnualEntitlementAsync(Guid employeeId, int yearNumber)
    {
        return await _hrLeaveExtension.CalculateAnnualEntitlementAsync(employeeId, yearNumber);
    }

    public async Task<LeaveBalanceCalculation> CalculateLeaveBalanceAsync(Guid employeeId, string calculationMode)
    {
        return await _hrLeaveExtension.CalculateLeaveBalanceAsync(employeeId, calculationMode);
    }

    public async Task<decimal> CalculateCarryoverAsync(Guid employeeId)
    {
        return await _hrLeaveExtension.CalculateCarryoverAsync(employeeId);
    }
}
