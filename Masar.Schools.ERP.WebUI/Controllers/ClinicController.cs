using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Masar.Schools.ERP.WebUI.Controllers;

[Authorize]
public class ClinicController : Controller
{
    private readonly IClinicService _clinicService;

    public ClinicController(IClinicService clinicService)
    {
        _clinicService = clinicService;
    }

    // Dashboard
    public async Task<IActionResult> Dashboard()
    {
        // Get current school ID from session or user context
        var schoolId = GetCurrentSchoolId();
        var dashboard = await _clinicService.GetClinicDashboardAsync(schoolId);
        return View(dashboard);
    }

    // Quick Visit (ClinicPOS)
    public async Task<IActionResult> QuickVisit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> QuickVisit(CreateClinicVisitDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        dto.SchoolId = GetCurrentSchoolId();
        dto.VisitDate = DateTime.UtcNow;
        dto.StartTime = DateTime.UtcNow.TimeOfDay;

        var visit = await _clinicService.CreateClinicVisitAsync(dto);

        // Send WhatsApp notification if emergency
        if (dto.IsEmergency)
        {
            await _clinicService.SendWhatsAppNotificationAsync(visit.Id);
        }

        return RedirectToAction(nameof(VisitDetails), new { id = visit.Id });
    }

    public async Task<IActionResult> VisitDetails(Guid id)
    {
        var visit = await _clinicService.GetClinicVisitAsync(id);
        if (visit == null)
            return NotFound();

        return View(visit);
    }

    // Health Profiles
    public async Task<IActionResult> HealthProfiles()
    {
        var schoolId = GetCurrentSchoolId();
        // Get all students with health profiles
        // This is a simplified version - in production, you'd filter by school
        return View();
    }

    public async Task<IActionResult> StudentHealthProfile(Guid studentId)
    {
        var profile = await _clinicService.GetStudentHealthProfileAsync(studentId);
        if (profile == null)
        {
            // Create a new profile if it doesn't exist
            return RedirectToAction(nameof(CreateHealthProfile), new { studentId });
        }

        return View(profile);
    }

    public IActionResult CreateHealthProfile(Guid studentId)
    {
        var dto = new CreateStudentHealthProfileDto { StudentId = studentId };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHealthProfile(CreateStudentHealthProfileDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var profile = await _clinicService.CreateStudentHealthProfileAsync(dto);
        return RedirectToAction(nameof(StudentHealthProfile), new { studentId = profile.StudentId });
    }

    public async Task<IActionResult> EditHealthProfile(Guid id)
    {
        var profile = await _clinicService.GetStudentHealthProfileAsync(id);
        if (profile == null)
            return NotFound();

        var dto = new UpdateStudentHealthProfileDto
        {
            Id = profile.Id,
            StudentId = profile.StudentId,
            BloodType = profile.BloodType,
            RhFactor = profile.RhFactor,
            Allergies = profile.Allergies,
            AllergiesArabic = profile.AllergiesArabic,
            ProhibitedFoods = profile.ProhibitedFoods,
            ProhibitedFoodsArabic = profile.ProhibitedFoodsArabic,
            ChronicDiseases = profile.ChronicDiseases,
            ChronicDiseasesArabic = profile.ChronicDiseasesArabic,
            PhysicalDisabilities = profile.PhysicalDisabilities,
            PhysicalDisabilitiesArabic = profile.PhysicalDisabilitiesArabic,
            Vaccinations = profile.Vaccinations,
            VaccinationsArabic = profile.VaccinationsArabic,
            LastComprehensiveCheckup = profile.LastComprehensiveCheckup,
            DietaryRestrictions = profile.DietaryRestrictions,
            DietaryRestrictionsArabic = profile.DietaryRestrictionsArabic,
            AdditionalNotes = profile.AdditionalNotes,
            AdditionalNotesArabic = profile.AdditionalNotesArabic,
            RequiresSpecialCare = profile.RequiresSpecialCare,
            SpecialCareType = profile.SpecialCareType,
            SpecialCareTypeArabic = profile.SpecialCareTypeArabic,
            TreatingPhysician = profile.TreatingPhysician,
            TreatingPhysicianArabic = profile.TreatingPhysicianArabic,
            PhysicianPhone = profile.PhysicianPhone,
            PreferredHospital = profile.PreferredHospital,
            InsuranceNumber = profile.InsuranceNumber,
            InsuranceCompany = profile.InsuranceCompany
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> EditHealthProfile(UpdateStudentHealthProfileDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var profile = await _clinicService.UpdateStudentHealthProfileAsync(dto);
        return RedirectToAction(nameof(StudentHealthProfile), new { studentId = profile.StudentId });
    }

    // Visits
    public async Task<IActionResult> Visits(DateTime? startDate = null, DateTime? endDate = null)
    {
        var schoolId = GetCurrentSchoolId();
        var visits = await _clinicService.GetClinicVisitsAsync(schoolId, startDate, endDate);
        return View(visits);
    }

    public async Task<IActionResult> StudentVisits(Guid studentId)
    {
        var visits = await _clinicService.GetStudentClinicVisitsAsync(studentId);
        return View(visits);
    }

    public async Task<IActionResult> EditVisit(Guid id)
    {
        var visit = await _clinicService.GetClinicVisitAsync(id);
        if (visit == null)
            return NotFound();

        var dto = new UpdateClinicVisitDto
        {
            Id = visit.Id,
            EndTime = visit.EndTime,
            PreliminaryDiagnosis = visit.PreliminaryDiagnosis,
            PreliminaryDiagnosisArabic = visit.PreliminaryDiagnosisArabic,
            Treatment = visit.Treatment,
            TreatmentArabic = visit.TreatmentArabic,
            MedicationPrescribed = visit.MedicationPrescribed,
            MedicationPrescribedArabic = visit.MedicationPrescribedArabic,
            Dosage = visit.Dosage,
            DosageArabic = visit.DosageArabic,
            Duration = visit.Duration,
            DurationArabic = visit.DurationArabic,
            PatientCondition = visit.PatientCondition,
            PatientConditionArabic = visit.PatientConditionArabic,
            ReturnedToClass = visit.ReturnedToClass,
            ReferredToHospital = visit.ReferredToHospital,
            SentHome = visit.SentHome,
            ReferredHospital = visit.ReferredHospital,
            ReferredHospitalArabic = visit.ReferredHospitalArabic,
            ReferralReason = visit.ReferralReason,
            ReferralReasonArabic = visit.ReferralReasonArabic,
            Notes = visit.Notes,
            NotesArabic = visit.NotesArabic
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> EditVisit(UpdateClinicVisitDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var visit = await _clinicService.UpdateClinicVisitAsync(dto);
        return RedirectToAction(nameof(VisitDetails), new { id = visit.Id });
    }

    // Medical Inventory
    public async Task<IActionResult> Inventory()
    {
        var schoolId = GetCurrentSchoolId();
        var items = await _clinicService.GetMedicalItemsAsync(schoolId);
        return View(items);
    }

    public IActionResult CreateMedicalItem()
    {
        var dto = new CreateMedicalItemDto { SchoolId = GetCurrentSchoolId() };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalItem(CreateMedicalItemDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var item = await _clinicService.CreateMedicalItemAsync(dto);
        return RedirectToAction(nameof(Inventory));
    }

    public async Task<IActionResult> EditMedicalItem(Guid id)
    {
        var item = await _clinicService.GetMedicalItemAsync(id);
        if (item == null)
            return NotFound();

        var dto = new UpdateMedicalItemDto
        {
            Id = item.Id,
            AvailableQuantity = item.AvailableQuantity,
            ConsumedQuantity = item.ConsumedQuantity,
            IsExpired = item.IsExpired,
            NearExpiry = item.NearExpiry,
            IsActive = item.IsActive,
            Notes = item.Notes,
            NotesArabic = item.NotesArabic
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> EditMedicalItem(UpdateMedicalItemDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var item = await _clinicService.UpdateMedicalItemAsync(dto);
        return RedirectToAction(nameof(Inventory));
    }

    public async Task<IActionResult> DispenseItem(Guid id)
    {
        var item = await _clinicService.GetMedicalItemAsync(id);
        if (item == null)
            return NotFound();

        var dto = new DispenseMedicalItemDto { MedicalItemId = id };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> DispenseItem(DispenseMedicalItemDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _clinicService.DispenseMedicalItemAsync(dto);
        return RedirectToAction(nameof(Inventory));
    }

    public async Task<IActionResult> LowStockItems()
    {
        var schoolId = GetCurrentSchoolId();
        var items = await _clinicService.GetLowStockItemsAsync(schoolId);
        return View(items);
    }

    public async Task<IActionResult> ExpiredItems()
    {
        var schoolId = GetCurrentSchoolId();
        var items = await _clinicService.GetExpiredItemsAsync(schoolId);
        return View(items);
    }

    public async Task<IActionResult> NearExpiryItems()
    {
        var schoolId = GetCurrentSchoolId();
        var items = await _clinicService.GetNearExpiryItemsAsync(schoolId);
        return View(items);
    }

    // Medical Excuses
    public async Task<IActionResult> MedicalExcuses()
    {
        var schoolId = GetCurrentSchoolId();
        var excuses = await _clinicService.GetMedicalExcusesAsync(schoolId);
        return View(excuses);
    }

    public async Task<IActionResult> StudentMedicalExcuses(Guid studentId)
    {
        var excuses = await _clinicService.GetStudentMedicalExcusesAsync(studentId);
        return View(excuses);
    }

    public async Task<IActionResult> ActiveMedicalExcuses()
    {
        var schoolId = GetCurrentSchoolId();
        var excuses = await _clinicService.GetActiveMedicalExcusesAsync(schoolId);
        return View(excuses);
    }

    public IActionResult CreateMedicalExcuse(Guid? studentId = null, Guid? clinicVisitId = null)
    {
        var dto = new CreateMedicalExcuseDto
        {
            SchoolId = GetCurrentSchoolId(),
            StudentId = studentId ?? Guid.Empty,
            ClinicVisitId = clinicVisitId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalExcuse(CreateMedicalExcuseDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var excuse = await _clinicService.CreateMedicalExcuseAsync(dto);
        return RedirectToAction(nameof(MedicalExcuseDetails), new { id = excuse.Id });
    }

    public async Task<IActionResult> MedicalExcuseDetails(Guid id)
    {
        var excuse = await _clinicService.GetMedicalExcuseAsync(id);
        if (excuse == null)
            return NotFound();

        return View(excuse);
    }

    public async Task<IActionResult> EditMedicalExcuse(Guid id)
    {
        var excuse = await _clinicService.GetMedicalExcuseAsync(id);
        if (excuse == null)
            return NotFound();

        var dto = new UpdateMedicalExcuseDto
        {
            Id = excuse.Id,
            EndDate = excuse.EndDate,
            IsActive = excuse.IsActive,
            SentToTeachers = excuse.SentToTeachers,
            Notes = excuse.Notes,
            NotesArabic = excuse.NotesArabic
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> EditMedicalExcuse(UpdateMedicalExcuseDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var excuse = await _clinicService.UpdateMedicalExcuseAsync(dto);
        return RedirectToAction(nameof(MedicalExcuseDetails), new { id = excuse.Id });
    }

    public async Task<IActionResult> SendToTeachers(Guid id)
    {
        await _clinicService.SendExcuseToTeachersAsync(id);
        return RedirectToAction(nameof(MedicalExcuseDetails), new { id });
    }

    private Guid GetCurrentSchoolId()
    {
        // In production, this would come from the current user's context
        // For now, return a default GUID or get it from session
        var schoolIdStr = HttpContext.Session.GetString("CurrentSchoolId");
        if (Guid.TryParse(schoolIdStr, out var schoolId))
            return schoolId;

        // Default fallback - should be replaced with proper logic
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}