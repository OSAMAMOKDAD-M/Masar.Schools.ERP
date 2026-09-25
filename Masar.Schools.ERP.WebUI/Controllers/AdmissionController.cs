using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities.Admissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class AdmissionController : Controller
{
    private readonly IAdmissionService _admissionService;

    public AdmissionController(IAdmissionService admissionService)
    {
        _admissionService = admissionService;
    }

    // ==================== Dashboard ====================
    public async Task<IActionResult> Dashboard()
    {
        // TODO: Get SchoolId from current user
        var schoolId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var academicYear = DateTime.UtcNow.Year.ToString();

        var statistics = await _admissionService.GetAdmissionStatisticsAsync(schoolId, academicYear);
        return View(statistics.Data);
    }

    // ==================== Applications ====================
    public async Task<IActionResult> Applications()
    {
        var search = new AdmissionSearchDto { Page = 1, PageSize = 20 };
        var result = await _admissionService.GetAdmissionApplicationsAsync(search);
        return View(result.Data);
    }

    public IActionResult CreateApplication()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateApplication(CreateAdmissionApplicationDto dto)
    {
        // TODO: Get SchoolId from current user
        dto.SchoolId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = await _admissionService.CreateAdmissionApplicationAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Applications");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> EditApplication(Guid id)
    {
        var result = await _admissionService.GetAdmissionApplicationAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> EditApplication(UpdateAdmissionApplicationDto dto)
    {
        var result = await _admissionService.UpdateAdmissionApplicationAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("Applications");
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> ApplicationDetails(Guid id)
    {
        var result = await _admissionService.GetAdmissionApplicationAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(UpdateApplicationStatusDto dto)
    {
        var result = await _admissionService.UpdateApplicationStatusAsync(dto);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteApplication(Guid id)
    {
        var result = await _admissionService.DeleteAdmissionApplicationAsync(id);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    // ==================== Exams & Interviews ====================
    public async Task<IActionResult> CreateExam(Guid applicationId)
    {
        ViewBag.ApplicationId = applicationId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateExam(CreateAdmissionExamDto dto)
    {
        var result = await _admissionService.CreateAdmissionExamAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("ApplicationDetails", new { id = dto.AdmissionApplicationId });
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    public async Task<IActionResult> EditExam(Guid id)
    {
        var result = await _admissionService.GetAdmissionExamAsync(id);
        if (!result.Success)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> EditExam(UpdateAdmissionExamDto dto)
    {
        var result = await _admissionService.UpdateAdmissionExamAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("ApplicationDetails", new { id = result.Data.AdmissionApplicationId });
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    // ==================== Documents ====================
    public IActionResult UploadDocument(Guid applicationId)
    {
        ViewBag.ApplicationId = applicationId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadDocument(CreateApplicationDocumentDto dto)
    {
        var result = await _admissionService.CreateApplicationDocumentAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("ApplicationDetails", new { id = dto.AdmissionApplicationId });
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyDocument(UpdateApplicationDocumentDto dto)
    {
        var result = await _admissionService.UpdateApplicationDocumentAsync(dto);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var result = await _admissionService.DeleteApplicationDocumentAsync(id);
        if (result.Success)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false, message = result.Message });
    }

    // ==================== Student Conversion ====================
    public IActionResult ConvertToStudent(Guid applicationId)
    {
        ViewBag.ApplicationId = applicationId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConvertToStudent(ConvertToStudentDto dto)
    {
        var result = await _admissionService.ConvertToStudentAsync(dto);
        if (result.Success)
        {
            return RedirectToAction("ApplicationDetails", new { id = dto.AdmissionApplicationId });
        }
        ModelState.AddModelError("", result.Message);
        return View(dto);
    }
}