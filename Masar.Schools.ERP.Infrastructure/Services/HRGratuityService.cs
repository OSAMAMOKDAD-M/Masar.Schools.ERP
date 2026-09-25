using Masar.Schools.ERP.Domain.Entities.HR;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة مكافأة نهاية الخدمة
/// </summary>
public class HRGratuityService : IHRGratuityService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<HRGratuityService> _logger;

    public HRGratuityService(MasarDbContext context, ILogger<HRGratuityService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GratuityCalculation> CalculateGratuityAsync(Guid employeeId, DateTime endDate)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var startDate = employee.ContractStart ?? employee.HireDate ?? DateTime.Now;
        var salary = employee.Salary ?? 0;

        var totalDuration = endDate - startDate;
        var totalYears = (int)(totalDuration.TotalDays / 365);
        var totalMonths = (int)((totalDuration.TotalDays % 365) / 30);
        var totalDays = (int)(totalDuration.TotalDays % 30);

        decimal gratuity = 0;
        var calculationDetails = new List<string>();

        // Years 1-5: Half salary per year
        for (int year = 1; year <= Math.Min(5, totalYears); year++)
        {
            gratuity += salary / 2;
            calculationDetails.Add($"السنة {year}: نصف راتب = {salary / 2:C}");
        }

        // Year 6+: Full salary per year
        for (int year = 6; year <= totalYears; year++)
        {
            gratuity += salary;
            calculationDetails.Add($"السنة {year}: راتب كامل = {salary:C}");
        }

        // Fractions
        if (totalMonths > 0 || totalDays > 0)
        {
            var fractionRate = totalYears >= 5 ? salary : salary / 2;
            var fractionYear = (totalMonths * 30 + totalDays) / 365.0m;
            var fractionGratuity = fractionRate * fractionYear;
            gratuity += fractionGratuity;
            calculationDetails.Add($"الكسور ({totalMonths} شهر و {totalDays} يوم): {fractionGratuity:C}");
        }

        return new GratuityCalculation
        {
            EmployeeName = employee.FullName,
            EmployeeNameArabic = employee.FullNameArabic,
            EmployeeCode = employee.EmployeeNumber,
            Salary = salary,
            StartDate = startDate,
            EndDate = endDate,
            ServiceYears = totalYears,
            ServiceMonths = totalMonths,
            ServiceDays = totalDays,
            GratuityAmount = gratuity,
            CalculationDetails = string.Join("\n", calculationDetails)
        };
    }

    public async Task<ResignedEmployee> CreateResignedEmployeeAsync(ResignedEmployeeCreateDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.School)
            .FirstOrDefaultAsync(e => e.Id == dto.EmployeeId);

        if (employee == null)
            throw new Exception("Employee not found");

        // Calculate gratuity
        var gratuityCalculation = await CalculateGratuityAsync(dto.EmployeeId, dto.ResignDate);

        var resignedEmployee = new ResignedEmployee
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            Code = employee.EmployeeNumber,
            FullName = employee.FullName,
            FullNameArabic = employee.FullNameArabic,
            NationalId = employee.NationalId,
            JobTitle = employee.JobTitle,
            JobTitleArabic = employee.JobTitleArabic,
            Department = employee.Department,
            DepartmentArabic = employee.DepartmentArabic,
            Salary = employee.Salary ?? 0,
            ContractStart = employee.ContractStart,
            ResignDate = dto.ResignDate,
            Reason = dto.Reason,
            ReasonArabic = dto.ReasonArabic,
            ServiceYears = gratuityCalculation.ServiceYears,
            ServiceMonths = gratuityCalculation.ServiceMonths,
            ServiceDays = gratuityCalculation.ServiceDays,
            GratuityAmount = gratuityCalculation.GratuityAmount,
            Notes = dto.Notes,
            SchoolId = dto.SchoolId,
            CreatedAt = DateTime.Now,
            CreatedBy = "System"
        };

        _context.ResignedEmployees.Add(resignedEmployee);

        // Mark employee as resigned
        employee.IsResigned = true;
        employee.ResignDate = dto.ResignDate;
        employee.ResignReason = dto.Reason;
        employee.ResignReasonArabic = dto.ReasonArabic;
        employee.IsActive = false;
        employee.TerminationDate = dto.ResignDate;

        await _context.SaveChangesAsync();

        return resignedEmployee;
    }

    public async Task<List<ResignedEmployee>> GetResignedEmployeesAsync()
    {
        return await _context.ResignedEmployees
            .Include(r => r.Employee)
            .Include(r => r.School)
            .OrderByDescending(r => r.ResignDate)
            .ToListAsync();
    }

    public async Task<ResignedEmployee?> GetResignedEmployeeByIdAsync(Guid id)
    {
        return await _context.ResignedEmployees
            .Include(r => r.Employee)
            .Include(r => r.School)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<byte[]> GenerateGratuityLetterAsync(Guid employeeId, DateTime endDate, LetterType type)
    {
        var employee = await _context.Employees
            .Include(e => e.School)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            throw new Exception("Employee not found");

        var gratuityCalculation = await CalculateGratuityAsync(employeeId, endDate);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Gratuity Letter");

        // Set up RTL for Arabic
        worksheet.RightToLeft = true;

        // Header
        worksheet.Cell("A1").Value = employee.School?.NameArabic ?? "المدرسة";
        worksheet.Cell("A2").Value = "شهادة خبرة / مكافأة نهاية الخدمة";
        worksheet.Cell("A3").Value = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-SA"));

        // Employee Information
        worksheet.Cell("A5").Value = "بيانات الموظف:";
        worksheet.Cell("A6").Value = $"الاسم: {employee.FullNameArabic}";
        worksheet.Cell("A7").Value = $"الرقم الوظيفي: {employee.EmployeeNumber}";
        worksheet.Cell("A8").Value = $"الرقم الوطني: {employee.NationalId}";
        worksheet.Cell("A9").Value = $"المسمى الوظيفي: {employee.JobTitleArabic}";
        worksheet.Cell("A10").Value = $"القسم: {employee.DepartmentArabic}";

        // Service Information
        worksheet.Cell("A12").Value = "بيانات الخدمة:";
        worksheet.Cell("A13").Value = $"تاريخ بدء الخدمة: {employee.ContractStart?.ToString("yyyy-MM-dd")}";
        worksheet.Cell("A14").Value = $"تاريخ انتهاء الخدمة: {endDate:yyyy-MM-dd}";
        worksheet.Cell("A15").Value = $"مدة الخدمة: {gratuityCalculation.ServiceYears} سنة، {gratuityCalculation.ServiceMonths} شهر، {gratuityCalculation.ServiceDays} يوم";

        // Gratuity Calculation
        worksheet.Cell("A17").Value = "حساب مكافأة نهاية الخدمة:";
        worksheet.Cell("A18").Value = $"الراتب الأساسي: {employee.Salary:C}";
        worksheet.Cell("A19").Value = $"مبلغ المكافأة: {gratuityCalculation.GratuityAmount:C}";

        // Calculation Details
        worksheet.Cell("A21").Value = "تفاصيل الحساب:";
        var details = gratuityCalculation.CalculationDetails.Split('\n');
        for (int i = 0; i < details.Length; i++)
        {
            worksheet.Cell($"A{22 + i}").Value = details[i];
        }

        // Formatting
        worksheet.Range("A1:A3").Style.Font.Bold = true;
        worksheet.Range("A5").Style.Font.Bold = true;
        worksheet.Range("A12").Style.Font.Bold = true;
        worksheet.Range("A17").Style.Font.Bold = true;
        worksheet.Range("A21").Style.Font.Bold = true;

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}