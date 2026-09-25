using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Masar.Schools.ERP.Domain.Constants;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.WebUI.Filters
{
    /// <summary>
    /// Action Filter للتحقق من الفترات المالية المغلقة
    /// يمنع التعديل على الفترات المغلقة إلا للسوبر أدمن
    /// </summary>
    public class CheckClosedPeriodAttribute : ActionFilterAttribute
    {
        private readonly MasarDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckClosedPeriodAttribute(MasarDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = _httpContextAccessor.HttpContext?.User;

            // التحقق من وجود مستخدم
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // التحقق من صلاحية السوبر أدمن
            var hasOverridePermission = user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "SuperAdmin") ||
                                       user.HasClaim(c => c.Type == "Permission" && c.Value == PermissionConstants.Financial.OverrideClosedPeriods);

            // الحصول على التاريخ الحالي
            var currentDate = DateTime.Now;

            // البحث عن فترة مالية مغلقة تحتوي على التاريخ الحالي
            var closedPeriod = await _context.FinancialPeriods
                .FirstOrDefaultAsync(p => p.IsClosed && 
                                        p.StartDate <= currentDate && 
                                        p.EndDate >= currentDate);

            // إذا لم تكن هناك فترة مغلقة، استمر
            if (closedPeriod == null)
            {
                await next();
                return;
            }

            // إذا كان المستخدم لديه صلاحية السوبر أدمن، سجل العملية واستمر
            if (hasOverridePermission)
            {
                // تسجيل العملية في سجل المراجعة
                var auditLog = new Masar.Schools.ERP.Domain.Entities.AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserName = userName ?? "Unknown",
                    ActionDate = DateTime.Now,
                    ActionType = "Override",
                    EntityName = context.ActionDescriptor.DisplayName,
                    Description = $"تجاوز فترة مالية مغلقة: {closedPeriod.PeriodName}",
                    IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    FinancialPeriodId = closedPeriod.Id
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                await next();
                return;
            }

            // إذا لم يكن لديه صلاحية، منع العملية
            context.Result = new ContentResult
            {
                Content = "لا يمكن التعديل، تم إغلاق هذه الفترة المالية.",
                ContentType = "text/plain; charset=utf-8",
                StatusCode = 403
            };
        }
    }
}
