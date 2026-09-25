using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class AlumniController : Controller
{
    private readonly IAlumniService _alumniService;

    public AlumniController(IAlumniService alumniService)
    {
        _alumniService = alumniService;
    }

    // Dashboard
    public async Task<IActionResult> Dashboard()
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var dashboard = await _alumniService.GetDashboardAsync(tenantId, schoolId);
        return View(dashboard);
    }

    // Alumni Records
    public async Task<IActionResult> Index()
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var alumniRecords = await _alumniService.GetAllAlumniRecordsAsync(tenantId, schoolId);
        return View(alumniRecords);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var alumniRecord = await _alumniService.GetAlumniRecordByIdAsync(id, tenantId, schoolId);
        if (alumniRecord == null)
            return NotFound();
            
        return View(alumniRecord);
    }

    public async Task<IActionResult> Create()
    {
        // Get list of eligible students for graduation
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        ViewBag.GraduationYear = DateTime.Now.Year;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAlumniRecordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var alumniRecord = await _alumniService.CreateAlumniRecordAsync(dto, tenantId, schoolId, employeeId);
            return RedirectToAction(nameof(Details), new { id = alumniRecord.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var alumniRecord = await _alumniService.GetAlumniRecordByIdAsync(id, tenantId, schoolId);
        if (alumniRecord == null)
            return NotFound();

        var updateDto = new UpdateAlumniRecordDto
        {
            Id = alumniRecord.Id,
            StudentId = alumniRecord.StudentId,
            University = alumniRecord.University,
            UniversityArabic = alumniRecord.UniversityArabic,
            Major = alumniRecord.Major,
            MajorArabic = alumniRecord.MajorArabic,
            UniversityEnrollmentDate = alumniRecord.UniversityEnrollmentDate,
            UniversityGraduationDate = alumniRecord.UniversityGraduationDate,
            EmploymentStatus = alumniRecord.EmploymentStatus,
            EmploymentStatusArabic = alumniRecord.EmploymentStatusArabic,
            CompanyName = alumniRecord.CompanyName,
            CompanyNameArabic = alumniRecord.CompanyNameArabic,
            JobTitle = alumniRecord.JobTitle,
            JobTitleArabic = alumniRecord.JobTitleArabic,
            Industry = alumniRecord.Industry,
            IndustryArabic = alumniRecord.IndustryArabic,
            EmploymentStartDate = alumniRecord.EmploymentStartDate,
            PersonalEmail = alumniRecord.PersonalEmail,
            PersonalPhone = alumniRecord.PersonalPhone,
            LinkedInProfile = alumniRecord.LinkedInProfile,
            Website = alumniRecord.Website,
            CurrentAddress = alumniRecord.CurrentAddress,
            CurrentAddressArabic = alumniRecord.CurrentAddressArabic,
            Achievements = alumniRecord.Achievements,
            AchievementsArabic = alumniRecord.AchievementsArabic,
            Notes = alumniRecord.Notes,
            NotesArabic = alumniRecord.NotesArabic,
            IsActive = alumniRecord.IsActive,
            IsVerified = alumniRecord.IsVerified,
            LastContactDate = alumniRecord.LastContactDate
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateAlumniRecordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var alumniRecord = await _alumniService.UpdateAlumniRecordAsync(dto, tenantId, schoolId);
            return RedirectToAction(nameof(Details), new { id = alumniRecord.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var alumniRecord = await _alumniService.GetAlumniRecordByIdAsync(id, tenantId, schoolId);
        if (alumniRecord == null)
            return NotFound();

        return View(alumniRecord);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        await _alumniService.DeleteAlumniRecordAsync(id, tenantId, schoolId);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var alumniRecords = await _alumniService.SearchAlumniRecordsAsync(searchTerm, tenantId, schoolId);
        return View("Index", alumniRecords);
    }

    // Graduation Documents
    public async Task<IActionResult> Documents(Guid alumniRecordId)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var documents = await _alumniService.GetAlumniDocumentsAsync(alumniRecordId, tenantId, schoolId);
        ViewBag.AlumniRecordId = alumniRecordId;
        return View(documents);
    }

    public async Task<IActionResult> CreateDocument(Guid alumniRecordId)
    {
        ViewBag.AlumniRecordId = alumniRecordId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument(CreateGraduationDocumentDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var document = await _alumniService.CreateGraduationDocumentAsync(dto, tenantId, schoolId, employeeId);
            return RedirectToAction(nameof(Documents), new { alumniRecordId = dto.AlumniRecordId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> EditDocument(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var document = await _alumniService.GetGraduationDocumentByIdAsync(id, tenantId, schoolId);
        if (document == null)
            return NotFound();

        var updateDto = new UpdateGraduationDocumentDto
        {
            Id = document.Id,
            IsIssued = document.IsIssued,
            IsDelivered = document.IsDelivered,
            DeliveryDate = document.DeliveryDate,
            DeliveryMethod = document.DeliveryMethod,
            ReceivedBy = document.ReceivedBy,
            ReceivedByArabic = document.ReceivedByArabic,
            FinancialCleared = document.FinancialCleared,
            FinancialClearanceDate = document.FinancialClearanceDate,
            FinancialClearanceNotes = document.FinancialClearanceNotes,
            AdministrativeCleared = document.AdministrativeCleared,
            AdministrativeClearanceDate = document.AdministrativeClearanceDate,
            AdministrativeClearanceNotes = document.AdministrativeClearanceNotes,
            Notes = document.Notes,
            NotesArabic = document.NotesArabic
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> EditDocument(UpdateGraduationDocumentDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var document = await _alumniService.UpdateGraduationDocumentAsync(dto, tenantId, schoolId);
            return RedirectToAction(nameof(Documents), new { alumniRecordId = document.AlumniRecordId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var document = await _alumniService.GetGraduationDocumentByIdAsync(id, tenantId, schoolId);
        if (document == null)
            return NotFound();

        return View(document);
    }

    [HttpPost, ActionName("DeleteDocument")]
    public async Task<IActionResult> DeleteDocumentConfirmed(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var document = await _alumniService.GetGraduationDocumentByIdAsync(id, tenantId, schoolId);
        if (document != null)
        {
            await _alumniService.DeleteGraduationDocumentAsync(id, tenantId, schoolId);
            return RedirectToAction(nameof(Documents), new { alumniRecordId = document.AlumniRecordId });
        }
        
        return RedirectToAction(nameof(Index));
    }

    // Graduation Clearance
    public async Task<IActionResult> Clearance()
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var clearanceRecords = await _alumniService.GetAllClearanceRecordsAsync(tenantId, schoolId);
        return View(clearanceRecords);
    }

    public async Task<IActionResult> PendingClearance()
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var clearanceRecords = await _alumniService.GetPendingClearanceRecordsAsync(tenantId, schoolId);
        return View("Clearance", clearanceRecords);
    }

    public async Task<IActionResult> ClearanceDetails(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var clearanceRecord = await _alumniService.GetClearanceRecordByIdAsync(id, tenantId, schoolId);
        if (clearanceRecord == null)
            return NotFound();

        return View(clearanceRecord);
    }

    public async Task<IActionResult> CreateClearance(Guid studentId)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        ViewBag.StudentId = studentId;
        ViewBag.GraduationYear = DateTime.Now.Year;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateClearance(CreateGraduationClearanceRecordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var clearanceRecord = await _alumniService.CreateClearanceRecordAsync(dto, tenantId, schoolId, employeeId);
            return RedirectToAction(nameof(ClearanceDetails), new { id = clearanceRecord.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> EditClearance(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        
        var clearanceRecord = await _alumniService.GetClearanceRecordByIdAsync(id, tenantId, schoolId);
        if (clearanceRecord == null)
            return NotFound();

        var updateDto = new UpdateGraduationClearanceRecordDto
        {
            Id = clearanceRecord.Id,
            StudentId = clearanceRecord.StudentId,
            ClearanceStatus = clearanceRecord.ClearanceStatus,
            ClearanceStatusArabic = clearanceRecord.ClearanceStatusArabic,
            FinancialCleared = clearanceRecord.FinancialCleared,
            FinancialClearanceDate = clearanceRecord.FinancialClearanceDate,
            FinancialClearanceNotes = clearanceRecord.FinancialClearanceNotes,
            FinancialClearanceNotesArabic = clearanceRecord.FinancialClearanceNotesArabic,
            OutstandingBalance = clearanceRecord.OutstandingBalance,
            AdministrativeCleared = clearanceRecord.AdministrativeCleared,
            AdministrativeClearanceDate = clearanceRecord.AdministrativeClearanceDate,
            AdministrativeClearanceNotes = clearanceRecord.AdministrativeClearanceNotes,
            AdministrativeClearanceNotesArabic = clearanceRecord.AdministrativeClearanceNotesArabic,
            ReturnedLibraryBooks = clearanceRecord.ReturnedLibraryBooks,
            ReturnedEquipment = clearanceRecord.ReturnedEquipment,
            AcademicCleared = clearanceRecord.AcademicCleared,
            AcademicClearanceDate = clearanceRecord.AcademicClearanceDate,
            AcademicClearanceNotes = clearanceRecord.AcademicClearanceNotes,
            AcademicClearanceNotesArabic = clearanceRecord.AcademicClearanceNotesArabic,
            AllGradesRecorded = clearanceRecord.AllGradesRecorded,
            AllRequirementsMet = clearanceRecord.AllRequirementsMet,
            ClinicCleared = clearanceRecord.ClinicCleared,
            ClinicClearanceDate = clearanceRecord.ClinicClearanceDate,
            ClinicClearanceNotes = clearanceRecord.ClinicClearanceNotes,
            ClinicClearanceNotesArabic = clearanceRecord.ClinicClearanceNotesArabic,
            FinalApproval = clearanceRecord.FinalApproval,
            FinalApprovalDate = clearanceRecord.FinalApprovalDate,
            FinalApprovalNotes = clearanceRecord.FinalApprovalNotes,
            FinalApprovalNotesArabic = clearanceRecord.FinalApprovalNotesArabic,
            Notes = clearanceRecord.Notes,
            NotesArabic = clearanceRecord.NotesArabic
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> EditClearance(UpdateGraduationClearanceRecordDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var clearanceRecord = await _alumniService.UpdateClearanceRecordAsync(dto, tenantId, schoolId, employeeId);
            return RedirectToAction(nameof(ClearanceDetails), new { id = clearanceRecord.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    // Graduation Workflow
    public async Task<IActionResult> GraduateStudent(Guid studentId)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());
        
        ViewBag.StudentId = studentId;
        ViewBag.GraduationYear = DateTime.Now.Year;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GraduateStudent(Guid studentId, int graduationYear, decimal? finalGPA)
    {
        var tenantId = Guid.Parse(User.FindFirst("TenantId")?.Value ?? Guid.Empty.ToString());
        var schoolId = Guid.Parse(User.FindFirst("SchoolId")?.Value ?? Guid.Empty.ToString());
        var employeeId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());

        try
        {
            var success = await _alumniService.ProcessGraduationWorkflowAsync(studentId, graduationYear, finalGPA, tenantId, schoolId, employeeId);
            
            if (success)
            {
                TempData["SuccessMessage"] = "تم تخرج الطالب بنجاح وإنشاء سجل الخريج";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "فشل عملية التخرج. يرجى التحقق من إبراء الذمة المالي";
                return RedirectToAction(nameof(GraduateStudent), new { studentId });
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(GraduateStudent), new { studentId });
        }
    }
}