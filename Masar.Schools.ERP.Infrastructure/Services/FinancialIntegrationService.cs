using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Masar.Schools.ERP.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة التكامل بين المخزون/المشتريات والمالية
/// </summary>
public class FinancialIntegrationService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<FinancialIntegrationService> _logger;

    public FinancialIntegrationService(MasarDbContext context, ILogger<FinancialIntegrationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// إنشاء قيد دفتري عند استلام أمر الشراء
    /// </summary>
    public async Task CreatePurchaseOrderEntryAsync(PurchaseOrderDto purchaseOrder)
    {
        try
        {
            // إنشاء قيد دفتري للمشتريات
            var journalEntry = new JournalEntry
            {
                Id = Guid.NewGuid(),
                EntryId = Guid.NewGuid(),
                EntryNumber = $"PO-{purchaseOrder.OrderNumber}",
                EntryDate = DateTime.UtcNow,
                Description = $"استلام أمر شراء {purchaseOrder.OrderNumber} من {purchaseOrder.SupplierName}",
                ReferenceId = purchaseOrder.Id,
                ReferenceType = "PurchaseOrder",
                EntryType = EntryType.Invoice,
                CreatedAt = DateTime.UtcNow
            };

            // خط الدفتر (المشتريات)
            var purchaseLine = new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                LineId = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = GetAccountId("Purchases"), // حساب المشتريات
                Debit = purchaseOrder.TotalAmount,
                Credit = 0,
                Description = $"المشتريات - {purchaseOrder.SupplierName}",
                CreatedAt = DateTime.UtcNow
            };

            // خط الدفتر (المورد)
            var supplierLine = new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                LineId = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = GetAccountId("AccountsPayable"), // حساب الدائنين
                Debit = 0,
                Credit = purchaseOrder.TotalAmount,
                Description = $"الذمم الموردية - {purchaseOrder.SupplierName}",
                CreatedAt = DateTime.UtcNow
            };

            journalEntry.EntryLines = new List<JournalEntryLine> { purchaseLine, supplierLine };

            // Calculate totals
            journalEntry.TotalDebit = purchaseLine.Debit + supplierLine.Debit;
            journalEntry.TotalCredit = purchaseLine.Credit + supplierLine.Credit;

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created financial entry for purchase order {purchaseOrder.OrderNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating financial entry for purchase order {purchaseOrder.OrderNumber}");
        }
    }

    /// <summary>
    /// إنشاء قيد دفتري عند دفع فاتورة الشراء
    /// </summary>
    public async Task CreatePurchaseInvoicePaymentEntryAsync(PurchaseInvoiceDto invoice, decimal paymentAmount)
    {
        try
        {
            var journalEntry = new JournalEntry
            {
                Id = Guid.NewGuid(),
                EntryId = Guid.NewGuid(),
                EntryNumber = $"PINV-{invoice.InvoiceNumber}",
                EntryDate = DateTime.UtcNow,
                Description = $"دفع فاتورة شراء {invoice.InvoiceNumber}",
                ReferenceId = invoice.Id,
                ReferenceType = "PurchaseInvoice",
                EntryType = EntryType.Payment,
                CreatedAt = DateTime.UtcNow
            };

            // خط الدفتر (الذمم الموردية)
            var payableLine = new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                LineId = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = GetAccountId("AccountsPayable"),
                Debit = paymentAmount,
                Credit = 0,
                Description = $"تسديد الذمم الموردية - {invoice.SupplierName}",
                CreatedAt = DateTime.UtcNow
            };

            // خط الدفتر (البنك/الصندوق)
            var bankLine = new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                LineId = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = GetAccountId("Bank"), // حساب البنك
                Debit = 0,
                Credit = paymentAmount,
                Description = $"البنك - دفع فاتورة {invoice.InvoiceNumber}",
                CreatedAt = DateTime.UtcNow
            };

            journalEntry.EntryLines = new List<JournalEntryLine> { payableLine, bankLine };

            // Calculate totals
            journalEntry.TotalDebit = payableLine.Debit + bankLine.Debit;
            journalEntry.TotalCredit = payableLine.Credit + bankLine.Credit;

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created payment entry for invoice {invoice.InvoiceNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating payment entry for invoice {invoice.InvoiceNumber}");
        }
    }

    /// <summary>
    /// تحديث الميزانية عند إنشاء طلب شراء
    /// </summary>
    public async Task UpdateBudgetForPurchaseRequestAsync(PurchaseRequestDto request)
    {
        try
        {
            // يمكن إضافة منطق لتخصيص الميزانية للطلبات
            _logger.LogInformation($"Budget allocation check for purchase request {request.RequestNumber} - Amount: {request.BudgetAmount}");
            
            // هنا يمكن التحقق من توفر الميزانية وتخصيصها
            // وتحديث سجلات الميزانية إذا لزم الأمر
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating budget for purchase request {request.RequestNumber}");
        }
    }

    /// <summary>
    /// إنشاء قيد دفتري لتعديل المخزون
    /// </summary>
    public async Task CreateStockAdjustmentEntryAsync(StockAdjustmentDto adjustment)
    {
        try
        {
            if (adjustment.Status != "Approved")
                return;

            var journalEntry = new JournalEntry
            {
                Id = Guid.NewGuid(),
                EntryId = Guid.NewGuid(),
                EntryNumber = $"SAD-{adjustment.AdjustmentNumber}",
                EntryDate = DateTime.UtcNow,
                Description = $"تعديل مخزون {adjustment.AdjustmentNumber} - {adjustment.Reason}",
                ReferenceId = adjustment.Id,
                ReferenceType = "StockAdjustment",
                EntryType = EntryType.Adjustment,
                CreatedAt = DateTime.UtcNow
            };

            // قيد دفتري للخسائر/المكاسب
            var adjustmentLine = new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                LineId = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = GetAccountId("InventoryAdjustment"), // حساب تعديل المخزون
                Debit = adjustment.TotalValue,
                Credit = 0,
                Description = $"تعديل مخزون - {adjustment.Reason}",
                CreatedAt = DateTime.UtcNow
            };

            journalEntry.EntryLines = new List<JournalEntryLine> { adjustmentLine };

            // Calculate totals
            journalEntry.TotalDebit = adjustmentLine.Debit;
            journalEntry.TotalCredit = adjustmentLine.Credit;

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created adjustment entry for {adjustment.AdjustmentNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating adjustment entry for {adjustment.AdjustmentNumber}");
        }
    }

    /// <summary>
    /// الحصول على معرف الحساب المحاسبي
    /// </summary>
    private Guid GetAccountId(string accountType)
    {
        // هذه دالة مساعدة للحصول على معرف الحساب
        // في الواقع، يجب استرجاع الحسابات من جدول Chart of Accounts
        // هنا نرجع معرف افتراضي للمثال
        
        return Guid.NewGuid(); // في الواقع، استرجع من جدول الحسابات
    }

    /// <summary>
    /// الحصول على تقرير تكامل المالية للمخزون
    /// </summary>
    public async Task<FinancialIntegrationReportDto> GetFinancialIntegrationReportAsync(DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var query = _context.PurchaseOrders.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(p => p.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.OrderDate <= endDate.Value);

            var orders = await query.ToListAsync();

            return new FinancialIntegrationReportDto
            {
                TotalPurchaseOrders = orders.Count,
                TotalPurchaseValue = orders.Sum(p => p.TotalAmount),
                TotalPaidAmount = orders.Sum(p => p.PaidAmount),
                TotalPendingAmount = orders.Sum(p => p.RemainingAmount),
                Currency = "SAR",
                ReportDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting financial integration report");
            return new FinancialIntegrationReportDto();
        }
    }
}

/// <summary>
/// DTO لتقرير التكامل المالي
/// </summary>
public class FinancialIntegrationReportDto
{
    public int TotalPurchaseOrders { get; set; }
    public decimal TotalPurchaseValue { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public decimal TotalPendingAmount { get; set; }
    public string Currency { get; set; } = "SAR";
    public DateTime ReportDate { get; set; }
}
