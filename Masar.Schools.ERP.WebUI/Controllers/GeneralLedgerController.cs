using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class GeneralLedgerController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<GeneralLedgerController> _logger;

    public GeneralLedgerController(MasarDbContext context, ILogger<GeneralLedgerController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /GeneralLedger
    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, Guid? accountId)
    {
        var query = _context.JournalEntries
            .Include(j => j.EntryLines)
                .ThenInclude(l => l.Account)
            .Where(j => j.IsPosted);

        if (fromDate.HasValue)
        {
            query = query.Where(j => j.EntryDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(j => j.EntryDate <= toDate.Value);
        }

        if (accountId.HasValue)
        {
            query = query.Where(j => j.EntryLines.Any(l => l.AccountId == accountId.Value));
        }

        var journalEntries = await query
            .OrderByDescending(j => j.EntryDate)
            .ThenBy(j => j.EntryNumber)
            .ToListAsync();

        // Get accounts for filter dropdown
        var accounts = await _context.Accounts
            .Where(a => a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        ViewBag.Accounts = accounts;
        ViewBag.SelectedAccountId = accountId;
        ViewBag.FromDate = fromDate;
        ViewBag.ToDate = toDate;

        // Calculate totals
        var totalDebit = journalEntries.Sum(j => j.TotalDebit);
        var totalCredit = journalEntries.Sum(j => j.TotalCredit);
        ViewBag.TotalDebit = totalDebit;
        ViewBag.TotalCredit = totalCredit;

        return View(journalEntries);
    }

    // GET: /GeneralLedger/AccountBalance/5
    public async Task<IActionResult> AccountBalance(Guid id, DateTime? fromDate, DateTime? toDate)
    {
        var account = await _context.Accounts
            .Include(a => a.ParentAccount)
            .Include(a => a.ChildAccounts)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (account == null)
        {
            return NotFound();
        }

        var query = _context.JournalEntryLines
            .Include(l => l.JournalEntry)
            .Include(l => l.Student)
            .Where(l => l.AccountId == id && l.JournalEntry.IsPosted);

        if (fromDate.HasValue)
        {
            query = query.Where(l => l.JournalEntry.EntryDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(l => l.JournalEntry.EntryDate <= toDate.Value);
        }

        var entries = await query
            .OrderBy(l => l.JournalEntry.EntryDate)
            .ThenBy(l => l.JournalEntry.EntryNumber)
            .ToListAsync();

        // Calculate running balance
        decimal runningBalance = 0;
        var entriesWithBalance = new List<JournalEntryLineWithBalance>();
        
        foreach (var entry in entries)
        {
            runningBalance += entry.Debit - entry.Credit;
            entriesWithBalance.Add(new JournalEntryLineWithBalance
            {
                JournalEntryLine = entry,
                RunningBalance = runningBalance
            });
        }

        ViewBag.Account = account;
        ViewBag.FromDate = fromDate;
        ViewBag.ToDate = toDate;
        ViewBag.StartingBalance = account.Balance - runningBalance;
        ViewBag.EndingBalance = account.Balance;

        return View(entriesWithBalance);
    }

    // GET: /GeneralLedger/TrialBalance
    public async Task<IActionResult> TrialBalance(DateTime? asOfDate)
    {
        var date = asOfDate ?? DateTime.Now;

        var accounts = await _context.Accounts
            .Where(a => a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var trialBalance = new List<TrialBalanceEntry>();

        foreach (var account in accounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate <= date)
                .ToListAsync();

            var totalDebit = entries.Sum(e => e.Debit);
            var totalCredit = entries.Sum(e => e.Credit);
            var balance = totalDebit - totalCredit;

            trialBalance.Add(new TrialBalanceEntry
            {
                AccountCode = account.AccountCode,
                AccountNameAr = account.AccountNameAr,
                AccountNameEn = account.AccountNameEn,
                AccountType = account.AccountType,
                Debit = account.AccountType == AccountType.Asset || account.AccountType == AccountType.Expense ? balance : 0,
                Credit = account.AccountType == AccountType.Liability || account.AccountType == AccountType.Equity || account.AccountType == AccountType.Revenue ? balance : 0
            });
        }

        ViewBag.AsOfDate = date;
        ViewBag.TotalDebit = trialBalance.Sum(t => t.Debit);
        ViewBag.TotalCredit = trialBalance.Sum(t => t.Credit);

        return View(trialBalance);
    }

    // GET: /GeneralLedger/IncomeStatement
    public async Task<IActionResult> IncomeStatement(DateTime? fromDate, DateTime? toDate)
    {
        var fromDateValue = fromDate ?? new DateTime(DateTime.Now.Year, 1, 1);
        var toDateValue = toDate ?? DateTime.Now;

        var revenueAccounts = await _context.Accounts
            .Where(a => a.AccountType == AccountType.Revenue && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var expenseAccounts = await _context.Accounts
            .Where(a => a.AccountType == AccountType.Expense && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var revenues = new List<AccountBalance>();
        var expenses = new List<AccountBalance>();

        foreach (var account in revenueAccounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate >= fromDateValue && 
                           l.JournalEntry.EntryDate <= toDateValue)
                .ToListAsync();

            var balance = entries.Sum(e => e.Credit - e.Debit);
            revenues.Add(new AccountBalance
            {
                Account = account,
                Balance = balance
            });
        }

        foreach (var account in expenseAccounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate >= fromDateValue && 
                           l.JournalEntry.EntryDate <= toDateValue)
                .ToListAsync();

            var balance = entries.Sum(e => e.Debit - e.Credit);
            expenses.Add(new AccountBalance
            {
                Account = account,
                Balance = balance
            });
        }

        var totalRevenue = revenues.Sum(r => r.Balance);
        var totalExpenses = expenses.Sum(e => e.Balance);
        var netIncome = totalRevenue - totalExpenses;

        ViewBag.FromDate = fromDateValue;
        ViewBag.ToDate = toDateValue;
        ViewBag.TotalRevenue = totalRevenue;
        ViewBag.TotalExpenses = totalExpenses;
        ViewBag.NetIncome = netIncome;

        var viewModel = new IncomeStatementViewModel
        {
            Revenues = revenues,
            Expenses = expenses
        };

        return View(viewModel);
    }

    // GET: /GeneralLedger/BalanceSheet
    public async Task<IActionResult> BalanceSheet(DateTime? asOfDate)
    {
        var date = asOfDate ?? DateTime.Now;

        var assetAccounts = await _context.Accounts
            .Where(a => a.AccountType == AccountType.Asset && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var liabilityAccounts = await _context.Accounts
            .Where(a => a.AccountType == AccountType.Liability && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var equityAccounts = await _context.Accounts
            .Where(a => a.AccountType == AccountType.Equity && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        var assets = new List<AccountBalance>();
        var liabilities = new List<AccountBalance>();
        var equity = new List<AccountBalance>();

        foreach (var account in assetAccounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate <= date)
                .ToListAsync();

            var balance = entries.Sum(e => e.Debit - e.Credit);
            assets.Add(new AccountBalance
            {
                Account = account,
                Balance = balance
            });
        }

        foreach (var account in liabilityAccounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate <= date)
                .ToListAsync();

            var balance = entries.Sum(e => e.Credit - e.Debit);
            liabilities.Add(new AccountBalance
            {
                Account = account,
                Balance = balance
            });
        }

        foreach (var account in equityAccounts)
        {
            var entries = await _context.JournalEntryLines
                .Include(l => l.JournalEntry)
                .Where(l => l.AccountId == account.Id && 
                           l.JournalEntry.IsPosted && 
                           l.JournalEntry.EntryDate <= date)
                .ToListAsync();

            var balance = entries.Sum(e => e.Credit - e.Debit);
            equity.Add(new AccountBalance
            {
                Account = account,
                Balance = balance
            });
        }

        var totalAssets = assets.Sum(a => a.Balance);
        var totalLiabilities = liabilities.Sum(l => l.Balance);
        var totalEquity = equity.Sum(e => e.Balance);

        ViewBag.AsOfDate = date;
        ViewBag.TotalAssets = totalAssets;
        ViewBag.TotalLiabilities = totalLiabilities;
        ViewBag.TotalEquity = totalEquity;

        var viewModel = new BalanceSheetViewModel
        {
            Assets = assets,
            Liabilities = liabilities,
            Equity = equity
        };

        return View(viewModel);
    }

    // GET: /GeneralLedger/JournalEntry/5
    public async Task<IActionResult> JournalEntry(Guid id)
    {
        var journalEntry = await _context.JournalEntries
            .Include(j => j.EntryLines)
                .ThenInclude(l => l.Account)
            .Include(j => j.EntryLines)
                .ThenInclude(l => l.Student)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journalEntry == null)
        {
            return NotFound();
        }

        return View(journalEntry);
    }

    // GET: /GeneralLedger/CreateManualEntry
    public async Task<IActionResult> CreateManualEntry()
    {
        var accounts = await _context.Accounts
            .Where(a => a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();

        ViewBag.Accounts = accounts;
        return View();
    }

    // POST: /GeneralLedger/CreateManualEntry
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateManualEntry(ManualJournalEntryDto model)
    {
        if (!ModelState.IsValid)
        {
            var accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .OrderBy(a => a.AccountCode)
                .ToListAsync();
            ViewBag.Accounts = accounts;
            return View(model);
        }

        var journalEntry = new JournalEntry
        {
            EntryId = Guid.NewGuid(),
            EntryNumber = await GenerateJournalEntryNumber(),
            EntryDate = model.EntryDate,
            Description = model.Description,
            IsPosted = model.PostImmediately,
            PostedDate = model.PostImmediately ? DateTime.Now : null,
            PostedBy = model.PostImmediately ? User.Identity?.Name : null,
            EntryType = EntryType.Manual,
            TotalDebit = model.EntryLines.Sum(l => l.Debit),
            TotalCredit = model.EntryLines.Sum(l => l.Credit),
            Notes = model.Notes,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        foreach (var line in model.EntryLines)
        {
            var account = await _context.Accounts.FindAsync(line.AccountId);
            if (account == null)
            {
                ModelState.AddModelError("", $"الحساب غير موجود");
                var accounts = await _context.Accounts
                    .Where(a => a.IsActive)
                    .OrderBy(a => a.AccountCode)
                    .ToListAsync();
                ViewBag.Accounts = accounts;
                return View(model);
            }

            journalEntry.EntryLines.Add(new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = line.AccountId,
                Debit = line.Debit,
                Credit = line.Credit,
                Description = line.Description,
                StudentId = line.StudentId,
                LineNumber = line.LineNumber
            });

            // Update account balance if posted immediately
            if (model.PostImmediately)
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

        _context.JournalEntries.Add(journalEntry);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إنشاء القيد المحاسبي بنجاح";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> GenerateJournalEntryNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.JournalEntries
            .Where(j => j.EntryNumber != null && j.EntryNumber.StartsWith("JE" + year))
            .OrderByDescending(j => j.EntryNumber)
            .Select(j => j.EntryNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"JE{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(6));
        return $"JE{year}{(lastNum + 1).ToString("D4")}";
    }
}

// DTOs and View Models
public class TrialBalanceEntry
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string AccountNameEn { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}

public class AccountBalance
{
    public Account Account { get; set; } = null!;
    public decimal Balance { get; set; }
}

public class JournalEntryLineWithBalance
{
    public JournalEntryLine JournalEntryLine { get; set; } = null!;
    public decimal RunningBalance { get; set; }
}

public class IncomeStatementViewModel
{
    public List<AccountBalance> Revenues { get; set; } = new List<AccountBalance>();
    public List<AccountBalance> Expenses { get; set; } = new List<AccountBalance>();
}

public class BalanceSheetViewModel
{
    public List<AccountBalance> Assets { get; set; } = new List<AccountBalance>();
    public List<AccountBalance> Liabilities { get; set; } = new List<AccountBalance>();
    public List<AccountBalance> Equity { get; set; } = new List<AccountBalance>();
}

public class ManualJournalEntryDto
{
    public DateTime EntryDate { get; set; } = DateTime.Now;
    public string Description { get; set; } = string.Empty;
    public bool PostImmediately { get; set; } = true;
    public string? Notes { get; set; }
    public List<JournalEntryLineDto> EntryLines { get; set; } = new List<JournalEntryLineDto>();
}

public class JournalEntryLineDto
{
    public Guid AccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
    public Guid? StudentId { get; set; }
    public int LineNumber { get; set; }
}