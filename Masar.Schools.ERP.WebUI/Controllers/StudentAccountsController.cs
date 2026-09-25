using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class StudentAccountsController : Controller
{
    private readonly MasarDbContext _context;
    private readonly ILogger<StudentAccountsController> _logger;

    public StudentAccountsController(MasarDbContext context, ILogger<StudentAccountsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /StudentAccounts
    public async Task<IActionResult> Index()
    {
        var studentAccounts = await _context.StudentAccounts
            .Include(sa => sa.Student)
                .ThenInclude(s => s.ClassRoom)
            .Include(sa => sa.Student)
                .ThenInclude(s => s.School)
            .ToListAsync();

        // Calculate summary statistics
        var totalBalance = studentAccounts.Sum(sa => sa.TotalBalance);
        var totalPaid = studentAccounts.Sum(sa => sa.PaidBalance);
        var totalOutstanding = studentAccounts.Sum(sa => sa.OutstandingBalance);

        ViewBag.TotalBalance = totalBalance;
        ViewBag.TotalPaid = totalPaid;
        ViewBag.TotalOutstanding = totalOutstanding;
        ViewBag.TotalStudents = studentAccounts.Count;

        return View(studentAccounts);
    }

    // GET: /StudentAccounts/Statement/5
    public async Task<IActionResult> Statement(Guid? id)
    {
        if (id == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var studentAccount = await _context.StudentAccounts
            .Include(sa => sa.Student)
                .ThenInclude(s => s.ClassRoom)
            .Include(sa => sa.Student)
                .ThenInclude(s => s.School)
            .Include(sa => sa.Invoices)
                .ThenInclude(i => i.LineItems)
            .Include(sa => sa.Payments)
                .ThenInclude(p => p.Allocations)
            .FirstOrDefaultAsync(sa => sa.AccountId == id.Value);

        if (studentAccount == null)
        {
            return NotFound();
        }

        // Generate statement entries (invoices and payments)
        var statementEntries = new List<StatementEntry>();

        // Add invoices
        foreach (var invoice in studentAccount.Invoices)
        {
            statementEntries.Add(new StatementEntry
            {
                Date = invoice.IssueDate,
                ReferenceNumber = invoice.InvoiceNumber,
                Description = $"فاتورة - {invoice.LineItems.FirstOrDefault()?.DescriptionAr ?? "رسوم دراسية"}",
                Debit = invoice.TotalAmount,
                Credit = 0,
                Type = "Invoice",
                Status = invoice.Status.ToString()
            });
        }

        // Add payments
        foreach (var payment in studentAccount.Payments)
        {
            statementEntries.Add(new StatementEntry
            {
                Date = payment.PaymentDate,
                ReferenceNumber = payment.PaymentNumber,
                Description = $"سداد - {payment.PaymentMethod}",
                Debit = 0,
                Credit = payment.Amount,
                Type = "Payment",
                Status = payment.Status.ToString()
            });
        }

        // Sort by date and calculate running balance
        statementEntries = statementEntries.OrderBy(e => e.Date).ToList();
        decimal runningBalance = 0;
        foreach (var entry in statementEntries)
        {
            runningBalance += entry.Debit - entry.Credit;
            entry.RunningBalance = runningBalance;
        }

        ViewBag.StatementEntries = statementEntries;
        ViewBag.SchoolName = studentAccount.Student.School?.NameArabic ?? "مَسَار للمدارس";

        return View(studentAccount);
    }

    // POST: /StudentAccounts/GenerateBulkInvoices
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateBulkInvoices(Guid? classId, DateTime issueDate)
    {
        if (classId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var students = await _context.Students
            .Include(s => s.ClassRoom)
            .Where(s => s.ClassRoomId == classId.Value && !s.IsDeleted)
            .ToListAsync();

        if (students.Count == 0)
        {
            TempData["Error"] = "No students found in the selected class";
            return RedirectToAction(nameof(Index));
        }

        var successCount = 0;
        var errorCount = 0;

        foreach (var student in students)
        {
            try
            {
                // Check if student has an account
                var studentAccount = await _context.StudentAccounts
                    .FirstOrDefaultAsync(sa => sa.StudentId == student.Id);

                if (studentAccount == null)
                {
                    // Create student account
                    studentAccount = new StudentAccount
                    {
                        AccountId = Guid.NewGuid(),
                        StudentId = student.Id,
                        AccountNumber = $"SA-{DateTime.Now.Year}-{students.IndexOf(student) + 1:D4}",
                        TotalBalance = 0,
                        OutstandingBalance = 0,
                        PaidBalance = 0,
                        DiscountBalance = 0,
                        Status = AccountStatus.Active,
                        CreatedAt = DateTime.Now,
                        CreatedBy = User.Identity?.Name
                    };
                    _context.StudentAccounts.Add(studentAccount);
                    await _context.SaveChangesAsync();
                }

                // Generate invoice (simplified - in real implementation, this would calculate actual fees)
                var invoice = new StudentInvoice
                {
                    InvoiceId = Guid.NewGuid(),
                    InvoiceNumber = await GenerateInvoiceNumber(),
                    UUID = Guid.NewGuid().ToString(),
                    StudentAccountId = studentAccount.AccountId,
                    IssueDate = issueDate,
                    DueDate = issueDate.AddMonths(1),
                    TotalAmount = 15000, // Example amount
                    TaxAmount = 0, // Will be calculated based on VAT exemption
                    DiscountAmount = 0,
                    NetAmount = 15000,
                    PaidAmount = 0,
                    OutstandingAmount = 15000,
                    Status = InvoiceStatus.Issued,
                    InvoiceType = InvoiceType.B2C,
                    ZatcaStatus = ZatcaStatus.Pending,
                    IsVatExempt = studentAccount.IsVatExempt,
                    VatExemptionCode = studentAccount.VatExemptionCode,
                    CreatedAt = DateTime.Now,
                    CreatedBy = User.Identity?.Name
                };

                // Add line item
                invoice.LineItems.Add(new InvoiceLineItem
                {
                    ItemId = Guid.NewGuid(),
                    InvoiceId = invoice.InvoiceId,
                    Description = "الرسوم الدراسية - الفصل الأول",
                    DescriptionAr = "الرسوم الدراسية - الفصل الأول",
                    UnitPrice = 15000,
                    Quantity = 1,
                    TaxRate = studentAccount.IsVatExempt ? 0 : 0.15m,
                    TaxAmount = studentAccount.IsVatExempt ? 0 : 2250,
                    LineTotal = studentAccount.IsVatExempt ? 15000 : 17250,
                    LineNet = 15000,
                    LineNumber = 1
                });

                // Calculate VAT if not exempt
                if (!studentAccount.IsVatExempt)
                {
                    invoice.TaxAmount = 2250;
                    invoice.TotalAmount = 17250;
                    invoice.OutstandingAmount = 17250;
                }

                _context.StudentInvoices.Add(invoice);

                // Update student account balance
                studentAccount.TotalBalance += invoice.TotalAmount;
                studentAccount.OutstandingBalance += invoice.OutstandingAmount;
                studentAccount.LastInvoiceDate = issueDate;

                // Create journal entry for the invoice
                await CreateInvoiceJournalEntry(invoice, studentAccount);

                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating invoice for student {StudentId}", student.Id);
                errorCount++;
            }
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Generated {successCount} invoices successfully. {errorCount} errors occurred.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /StudentAccounts/ApplyDiscount/5
    public async Task<IActionResult> ApplyDiscount(Guid? studentAccountId)
    {
        // If no studentAccountId provided, redirect to Index
        if (!studentAccountId.HasValue)
        {
            return RedirectToAction(nameof(Index));
        }

        var studentAccount = await _context.StudentAccounts
            .Include(sa => sa.Student)
            .FirstOrDefaultAsync(sa => sa.AccountId == studentAccountId);

        if (studentAccount == null)
        {
            return NotFound();
        }

        var discounts = await _context.Discounts
            .Where(d => d.IsActive)
            .ToListAsync();

        var unpaidInvoices = await _context.StudentInvoices
            .Where(i => i.StudentAccountId == studentAccountId && 
                       i.Status != InvoiceStatus.Paid)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();

        ViewBag.Discounts = discounts;
        ViewBag.UnpaidInvoices = unpaidInvoices;

        return View(studentAccount);
    }

    // POST: /StudentAccounts/ApplyDiscount
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyDiscount(Guid? studentAccountId, Guid? discountId, Guid? invoiceId)
    {
        if (studentAccountId == null || discountId == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var studentAccount = await _context.StudentAccounts
            .Include(sa => sa.Student)
            .FirstOrDefaultAsync(sa => sa.AccountId == studentAccountId.Value);

        if (studentAccount == null)
        {
            return NotFound();
        }

        var discount = await _context.Discounts
            .FirstOrDefaultAsync(d => d.DiscountId == discountId.Value && d.IsActive);

        if (discount == null)
        {
            TempData["Error"] = "Discount not found or inactive";
            return RedirectToAction(nameof(Statement), new { id = studentAccountId.Value });
        }

        // Apply discount to the latest unpaid invoice if no specific invoice selected
        StudentInvoice? targetInvoice = null;
        if (invoiceId.HasValue)
        {
            targetInvoice = await _context.StudentInvoices
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId.Value);
        }
        else
        {
            targetInvoice = await _context.StudentInvoices
                .Where(i => i.StudentAccountId == studentAccountId && 
                           i.Status != InvoiceStatus.Paid)
                .OrderByDescending(i => i.IssueDate)
                .FirstOrDefaultAsync();
        }

        if (targetInvoice == null)
        {
            TempData["Error"] = "No unpaid invoice found";
            return RedirectToAction(nameof(Statement), new { id = studentAccountId });
        }

        // Calculate discount amount
        decimal discountAmount = 0;
        if (discount.DiscountType == DiscountType.Percentage)
        {
            discountAmount = targetInvoice.NetAmount * (discount.DiscountPercentage / 100);
            if (discount.MaxDiscountAmount > 0 && discountAmount > discount.MaxDiscountAmount)
            {
                discountAmount = discount.MaxDiscountAmount;
            }
        }
        else if (discount.DiscountType == DiscountType.FixedAmount)
        {
            discountAmount = discount.FixedAmount;
        }

        // Create student discount record
        var studentDiscount = new StudentDiscount
        {
            StudentDiscountId = Guid.NewGuid(),
            StudentAccountId = studentAccountId.Value,
            DiscountId = discountId.Value,
            InvoiceId = targetInvoice.InvoiceId,
            DiscountAmount = discountAmount,
            AppliedDate = DateTime.Now,
            IsApproved = true,
            ApprovedBy = User.Identity?.Name,
            ApprovedDate = DateTime.Now,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.StudentDiscounts.Add(studentDiscount);

        // Update invoice
        targetInvoice.DiscountAmount += discountAmount;
        targetInvoice.OutstandingAmount -= discountAmount;
        targetInvoice.TotalAmount -= discountAmount;

        // Update student account
        studentAccount.DiscountBalance += discountAmount;
        studentAccount.TotalBalance -= discountAmount;
        studentAccount.OutstandingBalance -= discountAmount;

        // Create journal entry for the discount
        await CreateDiscountJournalEntry(studentDiscount, studentAccount, targetInvoice);

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Discount of {discountAmount:C} applied successfully";
        return RedirectToAction(nameof(Statement), new { id = studentAccountId });
    }

    // GET: /StudentAccounts/CreateAccount
    public IActionResult CreateAccount()
    {
        var students = _context.Students
            .Include(s => s.ClassRoom)
            .Where(s => !s.IsDeleted && s.IsActive)
            .OrderBy(s => s.FullNameArabic)
            .ToList();

        ViewBag.Students = students;
        return View();
    }

    // POST: /StudentAccounts/CreateAccount
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount(Guid studentId, decimal initialBalance, string status, string notes, bool isVatExempt, string? vatExemptionCode)
    {
        if (studentId == Guid.Empty)
        {
            TempData["Error"] = "يرجى اختيار طالب";
            return RedirectToAction(nameof(CreateAccount));
        }

        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return NotFound();
        }

        // Check if account already exists
        var existingAccount = await _context.StudentAccounts
            .FirstOrDefaultAsync(sa => sa.StudentId == studentId);

        if (existingAccount != null)
        {
            TempData["Info"] = "الطالب لديه حساب بالفعل";
            return RedirectToAction(nameof(Statement), new { id = existingAccount.AccountId });
        }

        var studentAccount = new StudentAccount
        {
            AccountId = Guid.NewGuid(),
            StudentId = studentId,
            AccountNumber = await GenerateAccountNumber(),
            TotalBalance = initialBalance,
            OutstandingBalance = initialBalance,
            PaidBalance = 0,
            DiscountBalance = 0,
            Status = Enum.Parse<AccountStatus>(status),
            IsVatExempt = isVatExempt,
            VatExemptionCode = vatExemptionCode,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        _context.StudentAccounts.Add(studentAccount);
        await _context.SaveChangesAsync();

        TempData["Success"] = "تم إنشاء حساب الطالب بنجاح";
        return RedirectToAction(nameof(Statement), new { id = studentAccount.AccountId });
    }

    private async Task<string> GenerateInvoiceNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.StudentInvoices
            .Where(i => i.InvoiceNumber != null && i.InvoiceNumber.StartsWith("INV" + year))
            .OrderByDescending(i => i.InvoiceNumber)
            .Select(i => i.InvoiceNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"INV{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(7));
        return $"INV{year}{(lastNum + 1).ToString("D4")}";
    }

    private async Task<string> GenerateAccountNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.StudentAccounts
            .Where(sa => sa.AccountNumber != null && sa.AccountNumber.StartsWith("SA" + year))
            .OrderByDescending(sa => sa.AccountNumber)
            .Select(sa => sa.AccountNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"SA{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(6));
        return $"SA{year}{(lastNum + 1).ToString("D4")}";
    }

    private async Task CreateInvoiceJournalEntry(StudentInvoice invoice, StudentAccount studentAccount)
    {
        var journalEntry = new JournalEntry
        {
            EntryId = Guid.NewGuid(),
            EntryNumber = await GenerateJournalEntryNumber(),
            EntryDate = invoice.IssueDate,
            Description = $"فاتورة رقم {invoice.InvoiceNumber} - الطالب {studentAccount.Student.FullNameArabic}",
            IsPosted = true,
            PostedDate = DateTime.Now,
            PostedBy = User.Identity?.Name,
            EntryType = EntryType.Invoice,
            ReferenceId = invoice.InvoiceId,
            ReferenceType = "Invoice",
            TotalDebit = invoice.TotalAmount,
            TotalCredit = invoice.TotalAmount,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        // Get accounts
        var receivableAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "1201"); // Accounts Receivable
        var tuitionRevenueAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "4101"); // Tuition Revenue
        var vatPayableAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "2101"); // VAT Payable

        if (receivableAccount == null || tuitionRevenueAccount == null)
        {
            _logger.LogError("Required accounts not found for journal entry");
            return;
        }

        // Debit: Accounts Receivable (Student)
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = receivableAccount.Id,
            Debit = invoice.TotalAmount,
            Credit = 0,
            Description = $"مدينو الطلاب - {studentAccount.Student.FullNameArabic}",
            StudentId = studentAccount.StudentId,
            LineNumber = 1
        });

        // Credit: Tuition Revenue (Net amount)
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = tuitionRevenueAccount.Id,
            Debit = 0,
            Credit = invoice.NetAmount,
            Description = "إيرادات الرسوم الدراسية",
            LineNumber = 2
        });

        // Credit: VAT Payable (if applicable)
        if (invoice.TaxAmount > 0 && vatPayableAccount != null)
        {
            journalEntry.EntryLines.Add(new JournalEntryLine
            {
                Id = Guid.NewGuid(),
                EntryId = journalEntry.EntryId,
                AccountId = vatPayableAccount.Id,
                Debit = 0,
                Credit = invoice.TaxAmount,
                Description = "ضريبة القيمة المضافة المستحقة 15%",
                LineNumber = 3
            });
        }

        _context.JournalEntries.Add(journalEntry);

        // Update account balances
        receivableAccount.Balance += invoice.TotalAmount;
        tuitionRevenueAccount.Balance += invoice.NetAmount;
        if (vatPayableAccount != null && invoice.TaxAmount > 0)
        {
            vatPayableAccount.Balance += invoice.TaxAmount;
        }
    }

    private async Task CreateDiscountJournalEntry(StudentDiscount studentDiscount, StudentAccount studentAccount, StudentInvoice invoice)
    {
        var journalEntry = new JournalEntry
        {
            EntryId = Guid.NewGuid(),
            EntryNumber = await GenerateJournalEntryNumber(),
            EntryDate = studentDiscount.AppliedDate,
            Description = $"خصم - {studentDiscount.Discount.DiscountNameAr}",
            IsPosted = true,
            PostedDate = DateTime.Now,
            PostedBy = User.Identity?.Name,
            EntryType = EntryType.Discount,
            ReferenceId = studentDiscount.StudentDiscountId,
            ReferenceType = "Discount",
            TotalDebit = studentDiscount.DiscountAmount,
            TotalCredit = studentDiscount.DiscountAmount,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        // Get accounts
        var discountAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "4201"); // Discounts & Scholarships
        var receivableAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "1201"); // Accounts Receivable

        if (discountAccount == null || receivableAccount == null)
        {
            _logger.LogError("Required accounts not found for discount journal entry");
            return;
        }

        // Debit: Discounts & Scholarships
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = discountAccount.Id,
            Debit = studentDiscount.DiscountAmount,
            Credit = 0,
            Description = $"الخصومات والمنح - {studentDiscount.Discount.DiscountNameAr}",
            LineNumber = 1
        });

        // Credit: Accounts Receivable
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = receivableAccount.Id,
            Debit = 0,
            Credit = studentDiscount.DiscountAmount,
            Description = $"تخفيض مدينو الطلاب - {studentAccount.Student.FullNameArabic}",
            StudentId = studentAccount.StudentId,
            LineNumber = 2
        });

        _context.JournalEntries.Add(journalEntry);

        // Update account balances
        discountAccount.Balance += studentDiscount.DiscountAmount;
        receivableAccount.Balance -= studentDiscount.DiscountAmount;
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

// DTOs
public class StatementEntry
{
    public DateTime Date { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
