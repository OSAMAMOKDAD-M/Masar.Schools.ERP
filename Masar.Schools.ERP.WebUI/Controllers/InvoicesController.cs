using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Services;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class InvoicesController : Controller
{
    private readonly MasarDbContext _context;
    private readonly IZatcaService _zatcaService;
    private readonly ILogger<InvoicesController> _logger;

    public InvoicesController(
        MasarDbContext context,
        IZatcaService zatcaService,
        ILogger<InvoicesController> logger)
    {
        _context = context;
        _zatcaService = zatcaService;
        _logger = logger;
    }

    // GET: /Invoices/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var invoices = await _context.StudentInvoices
            .Include(i => i.StudentAccount)
                .ThenInclude(sa => sa.Student)
                    .ThenInclude(s => s.Guardian)
            .Include(i => i.LineItems)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();

        return View(invoices);
    }

    // GET: /Invoices/CreatePayment
    [HttpGet]
    public async Task<IActionResult> CreatePayment(Guid? studentId)
    {
        ViewBag.Students = new SelectList(
            await _context.Students
                .Include(s => s.Guardian)
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.FullNameArabic)
                .ToListAsync(),
            "Id", "FullNameArabic");

        if (studentId.HasValue)
        {
            var student = await _context.Students.FindAsync(studentId);
            ViewBag.SelectedStudent = student;
        }

        return View();
    }

    // POST: /Invoices/CreatePayment
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("CreatePayment")]
    public async Task<IActionResult> CreatePaymentPost(CreatePaymentDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Students = new SelectList(
                await _context.Students
                    .Include(s => s.Guardian)
                    .Where(s => s.IsActive && !s.IsDeleted)
                    .OrderBy(s => s.FullNameArabic)
                    .ToListAsync(),
                "Id", "FullNameArabic");
            return View(model);
        }

        var student = await _context.Students.FindAsync(model.StudentId);
        if (student == null)
        {
            ModelState.AddModelError("", "الطالب غير موجود");
            return View(model);
        }

        // Get or create student account
        var studentAccount = await _context.StudentAccounts
            .FirstOrDefaultAsync(sa => sa.StudentId == student.Id);

        if (studentAccount == null)
        {
            studentAccount = new StudentAccount
            {
                AccountId = Guid.NewGuid(),
                StudentId = student.Id,
                AccountNumber = await GenerateStudentAccountNumber(),
                TotalBalance = 0,
                OutstandingBalance = 0,
                PaidBalance = 0,
                DiscountBalance = 0,
                Status = AccountStatus.Active,
                IsVatExempt = false,
                VatExemptionCode = null,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity?.Name
            };
            _context.StudentAccounts.Add(studentAccount);
            await _context.SaveChangesAsync();
        }

        // Create StudentInvoice (new financial system)
        var invoiceNumber = await GenerateInvoiceNumber();
        var studentInvoice = new StudentInvoice
        {
            InvoiceId = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            UUID = Guid.NewGuid().ToString(),
            StudentAccountId = studentAccount.AccountId,
            IssueDate = DateTime.Now,
            DueDate = model.DueDate,
            TotalAmount = model.Amount + model.TaxAmount - model.DiscountAmount,
            TaxAmount = model.TaxAmount,
            DiscountAmount = model.DiscountAmount,
            NetAmount = model.Amount - model.DiscountAmount,
            PaidAmount = 0,
            OutstandingAmount = model.Amount + model.TaxAmount - model.DiscountAmount,
            Status = InvoiceStatus.Issued,
            InvoiceType = model.InvoiceType == "B2B" ? InvoiceType.B2B : InvoiceType.B2C,
            ZatcaStatus = ZatcaStatus.Pending,
            IsVatExempt = studentAccount.IsVatExempt,
            VatExemptionCode = studentAccount.VatExemptionCode,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        // Add line item
        studentInvoice.LineItems.Add(new InvoiceLineItem
        {
            ItemId = Guid.NewGuid(),
            InvoiceId = studentInvoice.InvoiceId,
            Description = model.Description,
            DescriptionAr = model.DescriptionArabic,
            UnitPrice = model.Amount,
            Quantity = 1,
            TaxRate = studentAccount.IsVatExempt ? 0 : (model.TaxPercentage / 100),
            TaxAmount = model.TaxAmount,
            LineTotal = model.Amount + model.TaxAmount,
            LineNet = model.Amount,
            LineNumber = 1
        });

        _context.StudentInvoices.Add(studentInvoice);

        // Update student account balance
        studentAccount.TotalBalance += studentInvoice.TotalAmount;
        studentAccount.OutstandingBalance += studentInvoice.OutstandingAmount;
        studentAccount.LastInvoiceDate = DateTime.Now;

        // Create journal entry for the invoice
        await CreateInvoiceJournalEntry(studentInvoice, studentAccount);

        await _context.SaveChangesAsync();

        // Submit to ZATCA if required (simplified for now)
        if (model.SubmitToZatca)
        {
            try
            {
                // ZATCA integration will be completed in a separate task
                _logger.LogInformation("ZATCA submission requested for invoice {InvoiceNumber}", studentInvoice.InvoiceNumber);
                studentInvoice.ZatcaStatus = ZatcaStatus.Pending;
                _context.Update(studentInvoice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ZATCA request");
            }
        }

        TempData["Success"] = "تم إنشاء الفاتورة بنجاح";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Invoices/GetZatcaStatus/5
    // GET: /Invoices/GetZatcaStatus
    [HttpGet]
    public async Task<IActionResult> GetZatcaStatus(Guid? id)
    {
        // If no id provided, redirect to Index
        if (!id.HasValue)
        {
            return RedirectToAction(nameof(Index));
        }

        var invoice = await _context.StudentInvoices
            .Include(i => i.StudentAccount)
                .ThenInclude(sa => sa.Student)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (invoice == null)
        {
            return NotFound();
        }

        var zatcaStatus = new ZatcaStatusViewModel
        {
            Invoice = invoice,
            XmlContent = invoice.XmlPath != null ? $"XML saved at: {invoice.XmlPath}" : "No XML generated",
            QrCode = invoice.QrCodeBase64 != null ? "QR Code generated" : "No QR Code"
        };

        return View(zatcaStatus);
    }

    // GET: /Invoices/StudentStatement/5
    // GET: /Invoices/StudentStatement
    [HttpGet]
    public async Task<IActionResult> StudentStatement(Guid? id)
    {
        // If no id provided, redirect to Index
        if (!id.HasValue)
        {
            return RedirectToAction(nameof(Index));
        }

        var student = await _context.Students
            .Include(s => s.Guardian)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (student == null)
        {
            return NotFound();
        }

        var studentAccount = await _context.StudentAccounts
            .Include(sa => sa.Invoices)
                .ThenInclude(i => i.Payments)
            .FirstOrDefaultAsync(sa => sa.StudentId == id);

        if (studentAccount == null)
        {
            return NotFound();
        }

        var statement = new StudentStatementViewModel
        {
            Student = student,
            StudentAccount = studentAccount,
            TotalBilled = studentAccount.TotalBalance,
            TotalPaid = studentAccount.PaidBalance,
            BalanceDue = studentAccount.OutstandingBalance
        };

        return View(statement);
    }

    // GET: /Invoices/RecordPayment/5
    // GET: /Invoices/RecordPayment
    [HttpGet]
    public async Task<IActionResult> RecordPayment(Guid? studentAccountId)
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

        ViewBag.StudentAccount = studentAccount;
        ViewBag.OutstandingBalance = studentAccount.OutstandingBalance;

        return View();
    }

    // POST: /Invoices/RecordPayment
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("RecordPayment")]
    public async Task<IActionResult> RecordPaymentPost(RecordPaymentDto model)
    {
        if (!ModelState.IsValid)
        {
            var accountForView = await _context.StudentAccounts
                .Include(sa => sa.Student)
                .FirstOrDefaultAsync(sa => sa.AccountId == model.StudentAccountId);
            ViewBag.StudentAccount = accountForView;
            ViewBag.OutstandingBalance = accountForView?.OutstandingBalance ?? 0;
            return View(model);
        }

        var studentAccount = await _context.StudentAccounts
            .Include(sa => sa.Student)
            .Include(sa => sa.Invoices)
            .FirstOrDefaultAsync(sa => sa.AccountId == model.StudentAccountId);

        if (studentAccount == null)
        {
            ModelState.AddModelError("", "حساب الطالب غير موجود");
            return View(model);
        }

        // Create payment
        var paymentNumber = await GeneratePaymentNumber();
        var payment = new StudentPayment
        {
            Id = Guid.NewGuid(),
            PaymentNumber = paymentNumber,
            StudentAccountId = studentAccount.AccountId,
            PaymentDate = DateTime.Now,
            Amount = model.Amount,
            PaymentMethod = model.PaymentMethod,
            ReferenceNumber = model.ReferenceNumber,
            BankName = model.BankName,
            Notes = model.Notes,
            Status = PaymentStatus.Completed,
            ReceiptNumber = $"REC{DateTime.Now:yyyyMMddHHmmss}",
            ReceivedBy = User.Identity?.Name,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        // Allocate payment to invoices (FIFO or specified invoice)
        if (model.InvoiceId.HasValue)
        {
            var invoice = studentAccount.Invoices.FirstOrDefault(i => i.InvoiceId == model.InvoiceId.Value);
            if (invoice != null)
            {
                payment.InvoiceId = invoice.InvoiceId;
                payment.Allocations.Add(new PaymentAllocation
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    InvoiceId = invoice.InvoiceId,
                    Amount = Math.Min(model.Amount, invoice.OutstandingAmount),
                    Notes = "تخصيص دفعة مباشر"
                });

                // Update invoice
                invoice.PaidAmount += payment.Allocations.First().Amount;
                invoice.OutstandingAmount -= payment.Allocations.First().Amount;
                if (invoice.OutstandingAmount <= 0)
                {
                    invoice.Status = InvoiceStatus.Paid;
                }
            }
        }
        else
        {
            // Allocate to oldest invoices (FIFO)
            var remainingAmount = model.Amount;
            foreach (var invoice in studentAccount.Invoices.Where(i => i.OutstandingAmount > 0).OrderBy(i => i.IssueDate))
            {
                if (remainingAmount <= 0) break;

                var allocationAmount = Math.Min(remainingAmount, invoice.OutstandingAmount);
                payment.Allocations.Add(new PaymentAllocation
                {
                    Id = Guid.NewGuid(),
                    PaymentId = payment.Id,
                    InvoiceId = invoice.InvoiceId,
                    Amount = allocationAmount,
                    Notes = "تخصيص تلقائي FIFO"
                });

                invoice.PaidAmount += allocationAmount;
                invoice.OutstandingAmount -= allocationAmount;
                if (invoice.OutstandingAmount <= 0)
                {
                    invoice.Status = InvoiceStatus.Paid;
                }

                remainingAmount -= allocationAmount;
            }
        }

        _context.StudentPayments.Add(payment);

        // Update student account balance
        studentAccount.PaidBalance += model.Amount;
        studentAccount.OutstandingBalance -= model.Amount;
        studentAccount.LastPaymentDate = DateTime.Now;

        // Create journal entry for the payment
        await CreatePaymentJournalEntry(payment, studentAccount);

        await _context.SaveChangesAsync();

        TempData["Success"] = "تم تسجيل الدفعة بنجاح";
        return RedirectToAction(nameof(StudentStatement), new { id = studentAccount.StudentId });
    }

    private async Task<string> GeneratePaymentNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.StudentPayments
            .Where(p => p.PaymentNumber != null && p.PaymentNumber.StartsWith("PAY" + year))
            .OrderByDescending(p => p.PaymentNumber)
            .Select(p => p.PaymentNumber)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastNumber))
        {
            return $"PAY{year}0001";
        }

        var lastNum = int.Parse(lastNumber.Substring(7));
        return $"PAY{year}{(lastNum + 1).ToString("D4")}";
    }

    private async Task<string> GenerateInvoiceNumber()
    {
        var year = DateTime.Now.Year;
        var lastNumber = await _context.Invoices
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

    private async Task<string> GenerateStudentAccountNumber()
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

    private async Task CreatePaymentJournalEntry(StudentPayment payment, StudentAccount studentAccount)
    {
        var journalEntry = new JournalEntry
        {
            EntryId = Guid.NewGuid(),
            EntryNumber = await GenerateJournalEntryNumber(),
            EntryDate = payment.PaymentDate,
            Description = $"دفعة رقم {payment.PaymentNumber} - الطالب {studentAccount.Student.FullNameArabic}",
            IsPosted = true,
            PostedDate = DateTime.Now,
            PostedBy = User.Identity?.Name,
            EntryType = EntryType.Payment,
            ReferenceId = payment.Id,
            ReferenceType = "Payment",
            TotalDebit = payment.Amount,
            TotalCredit = payment.Amount,
            CreatedAt = DateTime.Now,
            CreatedBy = User.Identity?.Name
        };

        // Get accounts
        var cashAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "1101"); // Cash
        var bankAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "1102"); // Banks
        var receivableAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountCode == "1201"); // Accounts Receivable

        if (receivableAccount == null)
        {
            _logger.LogError("Accounts Receivable account not found for payment journal entry");
            return;
        }

        // Select appropriate debit account based on payment method
        var debitAccount = payment.PaymentMethod == PaymentMethod.Cash ? cashAccount : bankAccount;

        if (debitAccount == null)
        {
            _logger.LogError("Debit account not found for payment journal entry");
            return;
        }

        // Debit: Cash or Bank
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = debitAccount.Id,
            Debit = payment.Amount,
            Credit = 0,
            Description = payment.PaymentMethod == PaymentMethod.Cash ? "الصندوق" : "البنوك",
            StudentId = studentAccount.StudentId,
            LineNumber = 1
        });

        // Credit: Accounts Receivable
        journalEntry.EntryLines.Add(new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            EntryId = journalEntry.EntryId,
            AccountId = receivableAccount.Id,
            Debit = 0,
            Credit = payment.Amount,
            Description = $"مدينو الطلاب - {studentAccount.Student.FullNameArabic}",
            StudentId = studentAccount.StudentId,
            LineNumber = 2
        });

        _context.JournalEntries.Add(journalEntry);

        // Update account balances
        debitAccount.Balance += payment.Amount;
        receivableAccount.Balance -= payment.Amount;
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
public class CreatePaymentDto
{
    public Guid StudentId { get; set; }
    public Guid SchoolId { get; set; }
    public string InvoiceType { get; set; } = "B2C";
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TaxPercentage { get; set; } = 15;
    public decimal DiscountAmount { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime DueDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DescriptionArabic { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool SubmitToZatca { get; set; }
}

public class ZatcaStatusViewModel
{
    public StudentInvoice Invoice { get; set; } = null!;
    public string? XmlContent { get; set; }
    public string? QrCode { get; set; }
}

public class StudentStatementViewModel
{
    public Student Student { get; set; } = null!;
    public StudentAccount? StudentAccount { get; set; }
    public decimal TotalBilled { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal BalanceDue { get; set; }
}

public class RecordPaymentDto
{
    public Guid StudentAccountId { get; set; }
    public Guid? InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? BankName { get; set; }
    public string? Notes { get; set; }
}
