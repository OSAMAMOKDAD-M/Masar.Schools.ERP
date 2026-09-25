using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Services;

namespace Masar.Schools.ERP.WebUI.Controllers
{
    [Authorize]
    public class FinancialYearClosingController : Controller
    {
        private readonly MasarDbContext _context;
        private readonly FinancialClosingService _closingService;

        public FinancialYearClosingController(MasarDbContext context, FinancialClosingService closingService)
        {
            _context = context;
            _closingService = closingService;
        }

        // GET: /FinancialYearClosing/Index
        public async Task<IActionResult> Index()
        {
            var periods = await _context.FinancialPeriods
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();

            return View(periods);
        }

        // GET: /FinancialYearClosing/Create
        [Authorize(Policy = PermissionConstants.Financial.ClosePeriod)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /FinancialYearClosing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PermissionConstants.Financial.ClosePeriod)]
        public async Task<IActionResult> Create(FinancialPeriod period)
        {
            if (!ModelState.IsValid)
            {
                return View(period);
            }

            period.Id = Guid.NewGuid();
            period.CreatedAt = DateTime.Now;
            period.CreatedBy = User.Identity?.Name;
            period.IsClosed = false;

            _context.FinancialPeriods.Add(period);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إنشاء الفترة المالية بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: /FinancialYearClosing/Close/{id}
        [Authorize(Policy = PermissionConstants.Financial.ClosePeriod)]
        public async Task<IActionResult> Close(Guid id)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: /FinancialYearClosing/Close/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PermissionConstants.Financial.ClosePeriod)]
        public async Task<IActionResult> Close(Guid id, string notes)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            // إنشاء قيد الإقفال
            var closingResult = await _closingService.CreateClosingEntryAsync(id, User.Identity?.Name ?? "System");
            if (!closingResult)
            {
                TempData["Error"] = "فشل إنشاء قيد الإقفال";
                return View(period);
            }

            // إغلاق الفترة
            var closeResult = await _closingService.CloseFinancialPeriodAsync(id, User.Identity?.Name ?? "System", notes);
            if (!closeResult)
            {
                TempData["Error"] = "فشل إغلاق الفترة المالية";
                return View(period);
            }

            TempData["Success"] = "تم إغلاق الفترة المالية بنجاح مع إنشاء قيد الإقفال";
            return RedirectToAction(nameof(Index));
        }

        // GET: /FinancialYearClosing/Reopen/{id}
        [Authorize(Policy = PermissionConstants.Financial.ReopenPeriod)]
        public async Task<IActionResult> Reopen(Guid id)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: /FinancialYearClosing/Reopen/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PermissionConstants.Financial.ReopenPeriod)]
        public async Task<IActionResult> Reopen(Guid id, string notes)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            var reopenResult = await _closingService.ReopenFinancialPeriodAsync(id, User.Identity?.Name ?? "System", notes);
            if (!reopenResult)
            {
                TempData["Error"] = "فشل إعادة فتح الفترة المالية";
                return View(period);
            }

            TempData["Success"] = "تم إعادة فتح الفترة المالية بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: /FinancialYearClosing/CreateOpeningEntry/{id}
        [Authorize(Policy = PermissionConstants.Financial.CreateJournal)]
        public async Task<IActionResult> CreateOpeningEntry(Guid id)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: /FinancialYearClosing/CreateOpeningEntry/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PermissionConstants.Financial.CreateJournal)]
        public async Task<IActionResult> CreateOpeningEntryPost(Guid id)
        {
            var period = await _context.FinancialPeriods.FindAsync(id);
            if (period == null)
            {
                return NotFound();
            }

            var openingResult = await _closingService.CreateOpeningEntryAsync(id, User.Identity?.Name ?? "System");
            if (!openingResult)
            {
                TempData["Error"] = "فشل إنشاء القيد الافتتاحي";
                return View(period);
            }

            TempData["Success"] = "تم إنشاء القيد الافتتاحي بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // POST: /FinancialYearClosing/PostUnpostedEntries
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = PermissionConstants.Financial.CreateJournal)]
        public async Task<IActionResult> PostUnpostedEntries()
        {
            var result = await _closingService.PostUnpostedEntriesAsync(User.Identity?.Name ?? "System");
            if (!result)
            {
                TempData["Error"] = "فشل ترحيل القيود";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "تم ترحيل القيود بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // GET: /FinancialYearClosing/AuditLogs/{id}
        public async Task<IActionResult> AuditLogs(Guid id)
        {
            var logs = await _context.AuditLogs
                .Where(a => a.FinancialPeriodId == id)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            var period = await _context.FinancialPeriods.FindAsync(id);
            ViewBag.PeriodName = period?.PeriodName;

            return View(logs);
        }
    }
}
