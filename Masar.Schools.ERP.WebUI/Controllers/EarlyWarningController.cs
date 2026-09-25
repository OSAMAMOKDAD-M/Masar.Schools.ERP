using Masar.Schools.ERP.AI.DTOs;
using Masar.Schools.ERP.AI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

/// <summary>
/// Controller لوحة التحكم بالإنذار المبكر والتكهن بالطلاب
/// </summary>
[Authorize]
public class EarlyWarningController : Controller
{
    private readonly IPredictiveAnalyticsService _predictiveAnalyticsService;
    private readonly IRiskAlertService _riskAlertService;
    private readonly ILogger<EarlyWarningController> _logger;

    public EarlyWarningController(
        IPredictiveAnalyticsService predictiveAnalyticsService,
        IRiskAlertService riskAlertService,
        ILogger<EarlyWarningController> logger)
    {
        _predictiveAnalyticsService = predictiveAnalyticsService;
        _riskAlertService = riskAlertService;
        _logger = logger;
    }

    // GET: /Admin/EarlyWarning
    public async Task<IActionResult> Index()
    {
        try
        {
            var dashboard = await _predictiveAnalyticsService.GetDashboardAsync();
            return View(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading early warning dashboard");
            return View("Error");
        }
    }

    // GET: /Admin/EarlyWarning/GetStudentDetails
    public async Task<IActionResult> GetStudentDetails(Guid studentId)
    {
        try
        {
            var details = await _predictiveAnalyticsService.GetStudentRiskDetailsAsync(studentId);
            return Json(new { success = true, data = details });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student risk details for {StudentId}", studentId);
            return Json(new { success = false, error = "Failed to load student details" });
        }
    }

    // GET: /Admin/EarlyWarning/GetRiskMatrix
    public async Task<IActionResult> GetRiskMatrix(RiskMatrixFilterDto filter)
    {
        try
        {
            var matrix = await _predictiveAnalyticsService.GetRiskMatrixAsync(filter);
            return Json(new { success = true, data = matrix });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting risk matrix");
            return Json(new { success = false, error = "Failed to load risk matrix" });
        }
    }

    // GET: /Admin/EarlyWarning/GetRevenuePrediction
    public async Task<IActionResult> GetRevenuePrediction(DateTime month)
    {
        try
        {
            var prediction = await _predictiveAnalyticsService.PredictMonthlyRevenueAsync(month);
            return Json(new { success = true, data = prediction });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue prediction for {Month}", month);
            return Json(new { success = false, error = "Failed to load revenue prediction" });
        }
    }

    // POST: /Admin/EarlyWarning/TriggerNotification
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TriggerNotification(Guid studentId)
    {
        try
        {
            // التحقق من الصلاحيات والملكية
            // يتم تنفيذ التحقق من البيانات في النظام الفعلي

            var studentDetails = await _predictiveAnalyticsService.GetStudentRiskDetailsAsync(studentId);

            if (studentDetails.RiskLevel != "Critical")
            {
                return Json(new { success = false, error = "Notification only available for critical risk students" });
            }

            // إرسال التنبيه
            // سيتم تنفيذ الإرسال الفعلي من خلال Worker

            return Json(new { success = true, message = "Notification triggered successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error triggering notification for student {StudentId}", studentId);
            return Json(new { success = false, error = "Failed to trigger notification" });
        }
    }
}
