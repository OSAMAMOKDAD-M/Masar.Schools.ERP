using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.Infrastructure.Services
{
    /// <summary>
    /// خدمة الإغلاق المالي والترحيل للأستاذ العام
    /// </summary>
    public class FinancialClosingService
    {
        private readonly MasarDbContext _context;
        private readonly ILogger<FinancialClosingService> _logger;

        public FinancialClosingService(MasarDbContext context, ILogger<FinancialClosingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// إغلاق فترة مالية
        /// </summary>
        public async Task<bool> CloseFinancialPeriodAsync(Guid periodId, string closedBy, string? notes = null)
        {
            try
            {
                var period = await _context.FinancialPeriods.FindAsync(periodId);
                if (period == null)
                {
                    _logger.LogError($"الفترة المالية {periodId} غير موجودة");
                    return false;
                }

                if (period.IsClosed)
                {
                    _logger.LogWarning($"الفترة المالية {periodId} مغلقة بالفعل");
                    return false;
                }

                // إغلاق الفترة
                period.IsClosed = true;
                period.ClosedDate = DateTime.Now;
                period.ClosedBy = closedBy;
                period.ClosingNotes = notes;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"تم إغلاق الفترة المالية {periodId} بنجاح");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطأ في إغلاق الفترة المالية {periodId}");
                return false;
            }
        }

        /// <summary>
        /// إعادة فتح فترة مالية
        /// </summary>
        public async Task<bool> ReopenFinancialPeriodAsync(Guid periodId, string reopenedBy, string? notes = null)
        {
            try
            {
                var period = await _context.FinancialPeriods.FindAsync(periodId);
                if (period == null)
                {
                    _logger.LogError($"الفترة المالية {periodId} غير موجودة");
                    return false;
                }

                if (!period.IsClosed)
                {
                    _logger.LogWarning($"الفترة المالية {periodId} مفتوحة بالفعل");
                    return false;
                }

                // إعادة فتح الفترة
                period.IsClosed = false;
                period.ClosedDate = null;
                period.ClosedBy = null;
                period.ClosingNotes = notes;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"تم إعادة فتح الفترة المالية {periodId} بنجاح");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطأ في إعادة فتح الفترة المالية {periodId}");
                return false;
            }
        }

        /// <summary>
        /// إنشاء قيد إقفال قائمة الدخل (Closing Entry)
        /// </summary>
        public async Task<bool> CreateClosingEntryAsync(Guid periodId, string createdBy)
        {
            try
            {
                var period = await _context.FinancialPeriods.FindAsync(periodId);
                if (period == null)
                {
                    _logger.LogError($"الفترة المالية {periodId} غير موجودة");
                    return false;
                }

                // الحصول على حسابات الإيرادات والمصروفات
                var revenueAccounts = await _context.Accounts
                    .Where(a => a.AccountType == AccountType.Revenue)
                    .ToListAsync();

                var expenseAccounts = await _context.Accounts
                    .Where(a => a.AccountType == AccountType.Expense)
                    .ToListAsync();

                // حساب صافي الربح/الخسارة
                decimal totalRevenue = revenueAccounts.Sum(a => a.Balance);
                decimal totalExpense = expenseAccounts.Sum(a => a.Balance);
                decimal netProfit = totalRevenue - totalExpense;

                // إنشاء قيد الإقفال
                var closingEntry = new JournalEntry
                {
                    EntryId = Guid.NewGuid(),
                    EntryNumber = await GenerateJournalEntryNumber(),
                    EntryDate = period.EndDate,
                    Description = $"قيد إقفال السنة المالية: {period.PeriodName}",
                    IsPosted = true,
                    PostedDate = DateTime.Now,
                    PostedBy = createdBy,
                    EntryType = EntryType.Closing,
                    TotalDebit = totalExpense + (netProfit > 0 ? netProfit : 0),
                    TotalCredit = totalRevenue + (netProfit < 0 ? Math.Abs(netProfit) : 0),
                    Notes = "قيد إقفال تلقائي للإيرادات والمصروفات",
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy
                };

                // إضافة سطور القيد للإيرادات
                foreach (var account in revenueAccounts)
                {
                    if (account.Balance > 0)
                    {
                        closingEntry.EntryLines.Add(new JournalEntryLine
                        {
                            Id = Guid.NewGuid(),
                            EntryId = closingEntry.EntryId,
                            AccountId = account.Id,
                            Debit = account.Balance,
                            Credit = 0,
                            Description = $"إقفال حساب الإيرادات: {account.AccountNameAr}",
                            LineNumber = closingEntry.EntryLines.Count + 1
                        });

                        // تصفير رصيد حساب الإيرادات
                        account.Balance = 0;
                    }
                }

                // إضافة سطور القيد للمصروفات
                foreach (var account in expenseAccounts)
                {
                    if (account.Balance > 0)
                    {
                        closingEntry.EntryLines.Add(new JournalEntryLine
                        {
                            Id = Guid.NewGuid(),
                            EntryId = closingEntry.EntryId,
                            AccountId = account.Id,
                            Debit = 0,
                            Credit = account.Balance,
                            Description = $"إقفال حساب المصروفات: {account.AccountNameAr}",
                            LineNumber = closingEntry.EntryLines.Count + 1
                        });

                        // تصفير رصيد حساب المصروفات
                        account.Balance = 0;
                    }
                }

                // إضافة سطر صافي الربح/الخسارة إلى حساب الأرباح المحتجزة
                var retainedEarningsAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountCode == "3201");

                if (retainedEarningsAccount != null && netProfit != 0)
                {
                    closingEntry.EntryLines.Add(new JournalEntryLine
                    {
                        Id = Guid.NewGuid(),
                        EntryId = closingEntry.EntryId,
                        AccountId = retainedEarningsAccount.Id,
                        Debit = netProfit < 0 ? Math.Abs(netProfit) : 0,
                        Credit = netProfit > 0 ? netProfit : 0,
                        Description = $"صافي {(netProfit > 0 ? "الربح" : "الخسارة")}",
                        LineNumber = closingEntry.EntryLines.Count + 1
                    });

                    // تحديث رصيد الأرباح المحتجزة
                    retainedEarningsAccount.Balance += netProfit;
                }

                _context.JournalEntries.Add(closingEntry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"تم إنشاء قيد الإقفال للفترة {periodId} بنجاح");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطأ في إنشاء قيد الإقفال للفترة {periodId}");
                return false;
            }
        }

        /// <summary>
        /// إنشاء قيد افتتاحي للسنة الجديدة (Opening Entry)
        /// </summary>
        public async Task<bool> CreateOpeningEntryAsync(Guid newPeriodId, string createdBy)
        {
            try
            {
                var newPeriod = await _context.FinancialPeriods.FindAsync(newPeriodId);
                if (newPeriod == null)
                {
                    _logger.LogError($"الفترة المالية {newPeriodId} غير موجودة");
                    return false;
                }

                // الحصول على الحسابات ذات الأرصدة المتبقية (الأصول، الالتزامات، حقوق الملكية)
                var balanceSheetAccounts = await _context.Accounts
                    .Where(a => (a.AccountType == AccountType.Asset || 
                               a.AccountType == AccountType.Liability || 
                               a.AccountType == AccountType.Equity) && 
                               a.Balance != 0)
                    .ToListAsync();

                if (!balanceSheetAccounts.Any())
                {
                    _logger.LogWarning($"لا توجد أرصدة لترحيلها للفترة {newPeriodId}");
                    return true;
                }

                // إنشاء القيد الافتتاحي
                var openingEntry = new JournalEntry
                {
                    EntryId = Guid.NewGuid(),
                    EntryNumber = await GenerateJournalEntryNumber(),
                    EntryDate = newPeriod.StartDate,
                    Description = $"قيد افتتاحي للسنة المالية: {newPeriod.PeriodName}",
                    IsPosted = true,
                    PostedDate = DateTime.Now,
                    PostedBy = createdBy,
                    EntryType = EntryType.OpeningBalance,
                    TotalDebit = balanceSheetAccounts.Where(a => a.AccountType == AccountType.Asset).Sum(a => a.Balance),
                    TotalCredit = balanceSheetAccounts.Where(a => a.AccountType != AccountType.Asset).Sum(a => a.Balance),
                    Notes = "قيد افتتاحي تلقائي",
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy
                };

                // إضافة سطور القيد
                int lineNumber = 1;
                foreach (var account in balanceSheetAccounts)
                {
                    openingEntry.EntryLines.Add(new JournalEntryLine
                    {
                        Id = Guid.NewGuid(),
                        EntryId = openingEntry.EntryId,
                        AccountId = account.Id,
                        Debit = account.AccountType == AccountType.Asset ? account.Balance : 0,
                        Credit = account.AccountType != AccountType.Asset ? account.Balance : 0,
                        Description = $"رصيد افتتاحي: {account.AccountNameAr}",
                        LineNumber = lineNumber++
                    });
                }

                _context.JournalEntries.Add(openingEntry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"تم إنشاء القيد الافتتاحي للفترة {newPeriodId} بنجاح");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطأ في إنشاء القيد الافتتاحي للفترة {newPeriodId}");
                return false;
            }
        }

        /// <summary>
        /// ترحيل القيود غير المرحلة (Batch Posting)
        /// </summary>
        public async Task<bool> PostUnpostedEntriesAsync(string postedBy)
        {
            try
            {
                var unpostedEntries = await _context.JournalEntries
                    .Where(j => !j.IsPosted)
                    .Include(j => j.EntryLines)
                    .ToListAsync();

                if (!unpostedEntries.Any())
                {
                    _logger.LogInformation("لا توجد قيود غير مرحلة");
                    return true;
                }

                foreach (var entry in unpostedEntries)
                {
                    // التحقق من توازن القيد
                    if (Math.Abs(entry.TotalDebit - entry.TotalCredit) > 0.01m)
                    {
                        _logger.LogWarning($"القيد {entry.EntryNumber} غير متوازن، تم التخطي");
                        continue;
                    }

                    // ترحيل القيد
                    entry.IsPosted = true;
                    entry.PostedDate = DateTime.Now;
                    entry.PostedBy = postedBy;

                    // تحديث أرصدة الحسابات
                    foreach (var line in entry.EntryLines)
                    {
                        var account = await _context.Accounts.FindAsync(line.AccountId);
                        if (account != null)
                        {
                            if (account.AccountType == AccountType.Asset || account.AccountType == AccountType.Expense)
                            {
                                account.Balance += line.Debit - line.Credit;
                            }
                            else
                            {
                                account.Balance += line.Credit - line.Debit;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"تم ترحيل {unpostedEntries.Count} قيد بنجاح");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في ترحيل القيود");
                return false;
            }
        }

        /// <summary>
        /// توليد رقم قيد جديد
        /// </summary>
        private async Task<string> GenerateJournalEntryNumber()
        {
            var lastEntry = await _context.JournalEntries
                .OrderByDescending(j => j.EntryNumber)
                .FirstOrDefaultAsync();

            if (lastEntry == null)
            {
                return "JE-000001";
            }

            var lastNumber = lastEntry.EntryNumber.Split('-')[1];
            var newNumber = int.Parse(lastNumber) + 1;
            return $"JE-{newNumber:D6}";
        }

        /// <summary>
        /// التحقق من توازن القيد
        /// </summary>
        public bool IsEntryBalanced(decimal totalDebit, decimal totalCredit)
        {
            return Math.Abs(totalDebit - totalCredit) < 0.01m;
        }
    }
}
