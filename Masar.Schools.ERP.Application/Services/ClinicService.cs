using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.Clinic;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Application.Services;

public class ClinicService : IClinicService
{
    private readonly MasarDbContext _context;

    public ClinicService(MasarDbContext context)
    {
        _context = context;
    }

    // StudentHealthProfile operations
    public async Task<StudentHealthProfileDto?> GetStudentHealthProfileAsync(Guid studentId)
    {
        var profile = await _context.StudentHealthProfiles
            .Include(s => s.Student)
            .Include(s => s.CreatedByEmployee)
            .Include(s => s.ClinicVisits)
            .FirstOrDefaultAsync(s => s.StudentId == studentId);

        if (profile == null) return null;

        return new StudentHealthProfileDto
        {
            Id = profile.Id,
            StudentId = profile.StudentId,
            StudentName = profile.Student.FullName,
            StudentNameArabic = profile.Student.FullNameArabic,
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
            InsuranceCompany = profile.InsuranceCompany,
            ProfileCreatedDate = profile.ProfileCreatedDate,
            LastUpdated = profile.LastUpdated,
            CreatedByEmployeeName = profile.CreatedByEmployee?.FullName,
            VisitCount = profile.ClinicVisits.Count
        };
    }

    public async Task<StudentHealthProfileDto> CreateStudentHealthProfileAsync(CreateStudentHealthProfileDto dto)
    {
        var profile = new StudentHealthProfile
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            BloodType = dto.BloodType,
            RhFactor = dto.RhFactor,
            Allergies = dto.Allergies,
            AllergiesArabic = dto.AllergiesArabic,
            ProhibitedFoods = dto.ProhibitedFoods,
            ProhibitedFoodsArabic = dto.ProhibitedFoodsArabic,
            ChronicDiseases = dto.ChronicDiseases,
            ChronicDiseasesArabic = dto.ChronicDiseasesArabic,
            PhysicalDisabilities = dto.PhysicalDisabilities,
            PhysicalDisabilitiesArabic = dto.PhysicalDisabilitiesArabic,
            Vaccinations = dto.Vaccinations,
            VaccinationsArabic = dto.VaccinationsArabic,
            LastComprehensiveCheckup = dto.LastComprehensiveCheckup,
            DietaryRestrictions = dto.DietaryRestrictions,
            DietaryRestrictionsArabic = dto.DietaryRestrictionsArabic,
            AdditionalNotes = dto.AdditionalNotes,
            AdditionalNotesArabic = dto.AdditionalNotesArabic,
            RequiresSpecialCare = dto.RequiresSpecialCare,
            SpecialCareType = dto.SpecialCareType,
            SpecialCareTypeArabic = dto.SpecialCareTypeArabic,
            TreatingPhysician = dto.TreatingPhysician,
            TreatingPhysicianArabic = dto.TreatingPhysicianArabic,
            PhysicianPhone = dto.PhysicianPhone,
            PreferredHospital = dto.PreferredHospital,
            InsuranceNumber = dto.InsuranceNumber,
            InsuranceCompany = dto.InsuranceCompany,
            ProfileCreatedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.StudentHealthProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return await GetStudentHealthProfileAsync(profile.StudentId);
    }

    public async Task<StudentHealthProfileDto> UpdateStudentHealthProfileAsync(UpdateStudentHealthProfileDto dto)
    {
        var profile = await _context.StudentHealthProfiles.FindAsync(dto.Id);
        if (profile == null)
            throw new Exception("Health profile not found");

        profile.BloodType = dto.BloodType;
        profile.RhFactor = dto.RhFactor;
        profile.Allergies = dto.Allergies;
        profile.AllergiesArabic = dto.AllergiesArabic;
        profile.ProhibitedFoods = dto.ProhibitedFoods;
        profile.ProhibitedFoodsArabic = dto.ProhibitedFoodsArabic;
        profile.ChronicDiseases = dto.ChronicDiseases;
        profile.ChronicDiseasesArabic = dto.ChronicDiseasesArabic;
        profile.PhysicalDisabilities = dto.PhysicalDisabilities;
        profile.PhysicalDisabilitiesArabic = dto.PhysicalDisabilitiesArabic;
        profile.Vaccinations = dto.Vaccinations;
        profile.VaccinationsArabic = dto.VaccinationsArabic;
        profile.LastComprehensiveCheckup = dto.LastComprehensiveCheckup;
        profile.DietaryRestrictions = dto.DietaryRestrictions;
        profile.DietaryRestrictionsArabic = dto.DietaryRestrictionsArabic;
        profile.AdditionalNotes = dto.AdditionalNotes;
        profile.AdditionalNotesArabic = dto.AdditionalNotesArabic;
        profile.RequiresSpecialCare = dto.RequiresSpecialCare;
        profile.SpecialCareType = dto.SpecialCareType;
        profile.SpecialCareTypeArabic = dto.SpecialCareTypeArabic;
        profile.TreatingPhysician = dto.TreatingPhysician;
        profile.TreatingPhysicianArabic = dto.TreatingPhysicianArabic;
        profile.PhysicianPhone = dto.PhysicianPhone;
        profile.PreferredHospital = dto.PreferredHospital;
        profile.InsuranceNumber = dto.InsuranceNumber;
        profile.InsuranceCompany = dto.InsuranceCompany;
        profile.LastUpdated = DateTime.UtcNow;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetStudentHealthProfileAsync(profile.StudentId);
    }

    public async Task<bool> DeleteStudentHealthProfileAsync(Guid id)
    {
        var profile = await _context.StudentHealthProfiles.FindAsync(id);
        if (profile == null) return false;

        profile.IsDeleted = true;
        profile.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    // ClinicVisit operations
    public async Task<List<ClinicVisitDto>> GetClinicVisitsAsync(Guid schoolId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.ClinicVisits
            .Include(c => c.Student)
            .Include(c => c.AttendingEmployee)
            .Where(c => c.SchoolId == schoolId);

        if (startDate.HasValue)
            query = query.Where(c => c.VisitDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.VisitDate <= endDate.Value);

        var visits = await query.OrderByDescending(c => c.VisitDate).ToListAsync();

        return visits.Select(v => new ClinicVisitDto
        {
            Id = v.Id,
            SchoolId = v.SchoolId,
            StudentId = v.StudentId,
            StudentName = v.Student.FullName,
            StudentNameArabic = v.Student.FullNameArabic,
            StudentNumber = v.Student.StudentNumber ?? "",
            AttendingEmployeeName = v.AttendingEmployee?.FullName,
            VisitDate = v.VisitDate,
            StartTime = v.StartTime,
            EndTime = v.EndTime,
            VisitReason = v.VisitReason,
            VisitReasonArabic = v.VisitReasonArabic,
            VisitType = v.VisitType,
            VisitTypeArabic = v.VisitTypeArabic,
            Symptoms = v.Symptoms,
            SymptomsArabic = v.SymptomsArabic,
            Temperature = v.Temperature,
            BloodPressureSystolic = v.BloodPressureSystolic,
            BloodPressureDiastolic = v.BloodPressureDiastolic,
            HeartRate = v.HeartRate,
            RespiratoryRate = v.RespiratoryRate,
            OxygenSaturation = v.OxygenSaturation,
            Weight = v.Weight,
            Height = v.Height,
            PreliminaryDiagnosis = v.PreliminaryDiagnosis,
            PreliminaryDiagnosisArabic = v.PreliminaryDiagnosisArabic,
            Treatment = v.Treatment,
            TreatmentArabic = v.TreatmentArabic,
            MedicationPrescribed = v.MedicationPrescribed,
            MedicationPrescribedArabic = v.MedicationPrescribedArabic,
            Dosage = v.Dosage,
            DosageArabic = v.DosageArabic,
            Duration = v.Duration,
            DurationArabic = v.DurationArabic,
            PatientCondition = v.PatientCondition,
            PatientConditionArabic = v.PatientConditionArabic,
            ReturnedToClass = v.ReturnedToClass,
            ReferredToHospital = v.ReferredToHospital,
            SentHome = v.SentHome,
            ReferredHospital = v.ReferredHospital,
            ReferredHospitalArabic = v.ReferredHospitalArabic,
            ReferralReason = v.ReferralReason,
            ReferralReasonArabic = v.ReferralReasonArabic,
            IsEmergency = v.IsEmergency,
            Notes = v.Notes,
            NotesArabic = v.NotesArabic,
            CreatedAt = v.CreatedAt,
            WhatsAppNotificationSent = v.WhatsAppNotificationSent,
            WhatsAppNotificationSentAt = v.WhatsAppNotificationSentAt
        }).ToList();
    }

    public async Task<ClinicVisitDto?> GetClinicVisitAsync(Guid id)
    {
        var visit = await _context.ClinicVisits
            .Include(c => c.Student)
            .Include(c => c.AttendingEmployee)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (visit == null) return null;

        return new ClinicVisitDto
        {
            Id = visit.Id,
            SchoolId = visit.SchoolId,
            StudentId = visit.StudentId,
            StudentName = visit.Student.FullName,
            StudentNameArabic = visit.Student.FullNameArabic,
            StudentNumber = visit.Student.StudentNumber ?? "",
            AttendingEmployeeName = visit.AttendingEmployee?.FullName,
            VisitDate = visit.VisitDate,
            StartTime = visit.StartTime,
            EndTime = visit.EndTime,
            VisitReason = visit.VisitReason,
            VisitReasonArabic = visit.VisitReasonArabic,
            VisitType = visit.VisitType,
            VisitTypeArabic = visit.VisitTypeArabic,
            Symptoms = visit.Symptoms,
            SymptomsArabic = visit.SymptomsArabic,
            Temperature = visit.Temperature,
            BloodPressureSystolic = visit.BloodPressureSystolic,
            BloodPressureDiastolic = visit.BloodPressureDiastolic,
            HeartRate = visit.HeartRate,
            RespiratoryRate = visit.RespiratoryRate,
            OxygenSaturation = visit.OxygenSaturation,
            Weight = visit.Weight,
            Height = visit.Height,
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
            IsEmergency = visit.IsEmergency,
            Notes = visit.Notes,
            NotesArabic = visit.NotesArabic,
            CreatedAt = visit.CreatedAt,
            WhatsAppNotificationSent = visit.WhatsAppNotificationSent,
            WhatsAppNotificationSentAt = visit.WhatsAppNotificationSentAt
        };
    }

    public async Task<List<ClinicVisitDto>> GetStudentClinicVisitsAsync(Guid studentId)
    {
        var visits = await _context.ClinicVisits
            .Include(c => c.Student)
            .Include(c => c.AttendingEmployee)
            .Where(c => c.StudentId == studentId)
            .OrderByDescending(c => c.VisitDate)
            .ToListAsync();

        return visits.Select(v => new ClinicVisitDto
        {
            Id = v.Id,
            SchoolId = v.SchoolId,
            StudentId = v.StudentId,
            StudentName = v.Student.FullName,
            StudentNameArabic = v.Student.FullNameArabic,
            StudentNumber = v.Student.StudentNumber ?? "",
            AttendingEmployeeName = v.AttendingEmployee?.FullName,
            VisitDate = v.VisitDate,
            StartTime = v.StartTime,
            EndTime = v.EndTime,
            VisitReason = v.VisitReason,
            VisitReasonArabic = v.VisitReasonArabic,
            VisitType = v.VisitType,
            VisitTypeArabic = v.VisitTypeArabic,
            Symptoms = v.Symptoms,
            SymptomsArabic = v.SymptomsArabic,
            Temperature = v.Temperature,
            BloodPressureSystolic = v.BloodPressureSystolic,
            BloodPressureDiastolic = v.BloodPressureDiastolic,
            HeartRate = v.HeartRate,
            RespiratoryRate = v.RespiratoryRate,
            OxygenSaturation = v.OxygenSaturation,
            Weight = v.Weight,
            Height = v.Height,
            PreliminaryDiagnosis = v.PreliminaryDiagnosis,
            PreliminaryDiagnosisArabic = v.PreliminaryDiagnosisArabic,
            Treatment = v.Treatment,
            TreatmentArabic = v.TreatmentArabic,
            MedicationPrescribed = v.MedicationPrescribed,
            MedicationPrescribedArabic = v.MedicationPrescribedArabic,
            Dosage = v.Dosage,
            DosageArabic = v.DosageArabic,
            Duration = v.Duration,
            DurationArabic = v.DurationArabic,
            PatientCondition = v.PatientCondition,
            PatientConditionArabic = v.PatientConditionArabic,
            ReturnedToClass = v.ReturnedToClass,
            ReferredToHospital = v.ReferredToHospital,
            SentHome = v.SentHome,
            ReferredHospital = v.ReferredHospital,
            ReferredHospitalArabic = v.ReferredHospitalArabic,
            ReferralReason = v.ReferralReason,
            ReferralReasonArabic = v.ReferralReasonArabic,
            IsEmergency = v.IsEmergency,
            Notes = v.Notes,
            NotesArabic = v.NotesArabic,
            CreatedAt = v.CreatedAt,
            WhatsAppNotificationSent = v.WhatsAppNotificationSent,
            WhatsAppNotificationSentAt = v.WhatsAppNotificationSentAt
        }).ToList();
    }

    public async Task<ClinicVisitDto> CreateClinicVisitAsync(CreateClinicVisitDto dto)
    {
        var visit = new ClinicVisit
        {
            Id = Guid.NewGuid(),
            SchoolId = dto.SchoolId,
            StudentId = dto.StudentId,
            AttendingEmployeeId = dto.AttendingEmployeeId,
            VisitDate = dto.VisitDate,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            VisitReason = dto.VisitReason,
            VisitReasonArabic = dto.VisitReasonArabic,
            VisitType = dto.VisitType,
            VisitTypeArabic = dto.VisitTypeArabic,
            Symptoms = dto.Symptoms,
            SymptomsArabic = dto.SymptomsArabic,
            Temperature = dto.Temperature,
            BloodPressureSystolic = dto.BloodPressureSystolic,
            BloodPressureDiastolic = dto.BloodPressureDiastolic,
            HeartRate = dto.HeartRate,
            RespiratoryRate = dto.RespiratoryRate,
            OxygenSaturation = dto.OxygenSaturation,
            Weight = dto.Weight,
            Height = dto.Height,
            PreliminaryDiagnosis = dto.PreliminaryDiagnosis,
            PreliminaryDiagnosisArabic = dto.PreliminaryDiagnosisArabic,
            Treatment = dto.Treatment,
            TreatmentArabic = dto.TreatmentArabic,
            MedicationPrescribed = dto.MedicationPrescribed,
            MedicationPrescribedArabic = dto.MedicationPrescribedArabic,
            Dosage = dto.Dosage,
            DosageArabic = dto.DosageArabic,
            Duration = dto.Duration,
            DurationArabic = dto.DurationArabic,
            PatientCondition = dto.PatientCondition,
            PatientConditionArabic = dto.PatientConditionArabic,
            ReturnedToClass = dto.ReturnedToClass,
            ReferredToHospital = dto.ReferredToHospital,
            SentHome = dto.SentHome,
            ReferredHospital = dto.ReferredHospital,
            ReferredHospitalArabic = dto.ReferredHospitalArabic,
            ReferralReason = dto.ReferralReason,
            ReferralReasonArabic = dto.ReferralReasonArabic,
            IsEmergency = dto.IsEmergency,
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic,
            CreatedAt = DateTime.UtcNow,
            WhatsAppNotificationSent = false
        };

        _context.ClinicVisits.Add(visit);
        await _context.SaveChangesAsync();

        return await GetClinicVisitAsync(visit.Id);
    }

    public async Task<ClinicVisitDto> UpdateClinicVisitAsync(UpdateClinicVisitDto dto)
    {
        var visit = await _context.ClinicVisits.FindAsync(dto.Id);
        if (visit == null)
            throw new Exception("Clinic visit not found");

        visit.EndTime = dto.EndTime;
        visit.PreliminaryDiagnosis = dto.PreliminaryDiagnosis;
        visit.PreliminaryDiagnosisArabic = dto.PreliminaryDiagnosisArabic;
        visit.Treatment = dto.Treatment;
        visit.TreatmentArabic = dto.TreatmentArabic;
        visit.MedicationPrescribed = dto.MedicationPrescribed;
        visit.MedicationPrescribedArabic = dto.MedicationPrescribedArabic;
        visit.Dosage = dto.Dosage;
        visit.DosageArabic = dto.DosageArabic;
        visit.Duration = dto.Duration;
        visit.DurationArabic = dto.DurationArabic;
        visit.PatientCondition = dto.PatientCondition;
        visit.PatientConditionArabic = dto.PatientConditionArabic;
        visit.ReturnedToClass = dto.ReturnedToClass;
        visit.ReferredToHospital = dto.ReferredToHospital;
        visit.SentHome = dto.SentHome;
        visit.ReferredHospital = dto.ReferredHospital;
        visit.ReferredHospitalArabic = dto.ReferredHospitalArabic;
        visit.ReferralReason = dto.ReferralReason;
        visit.ReferralReasonArabic = dto.ReferralReasonArabic;
        visit.Notes = dto.Notes;
        visit.NotesArabic = dto.NotesArabic;
        visit.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetClinicVisitAsync(visit.Id);
    }

    public async Task<bool> DeleteClinicVisitAsync(Guid id)
    {
        var visit = await _context.ClinicVisits.FindAsync(id);
        if (visit == null) return false;

        visit.IsDeleted = true;
        visit.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SendWhatsAppNotificationAsync(Guid clinicVisitId)
    {
        var visit = await _context.ClinicVisits
            .Include(c => c.Student)
            .ThenInclude(s => s.Guardian)
            .FirstOrDefaultAsync(c => c.Id == clinicVisitId);

        if (visit == null) return false;

        // TODO: Implement WhatsApp notification via Node.js/Baileys service
        // This should integrate with the existing WhatsApp notification system

        visit.WhatsAppNotificationSent = true;
        visit.WhatsAppNotificationSentAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    // MedicalItem operations
    public async Task<List<MedicalItemDto>> GetMedicalItemsAsync(Guid schoolId)
    {
        var items = await _context.MedicalItems
            .Include(m => m.School)
            .Include(m => m.UpdatedByEmployee)
            .Include(m => m.CreatedByEmployee)
            .Where(m => m.SchoolId == schoolId)
            .ToListAsync();

        return items.Select(i => new MedicalItemDto
        {
            Id = i.Id,
            SchoolId = i.SchoolId,
            ItemName = i.ItemName,
            ItemNameArabic = i.ItemNameArabic,
            ItemType = i.ItemType,
            ItemTypeArabic = i.ItemTypeArabic,
            Manufacturer = i.Manufacturer,
            ManufacturerArabic = i.ManufacturerArabic,
            DrugCode = i.DrugCode,
            BatchNumber = i.BatchNumber,
            ManufacturingDate = i.ManufacturingDate,
            ExpiryDate = i.ExpiryDate,
            AvailableQuantity = i.AvailableQuantity,
            Unit = i.Unit,
            UnitArabic = i.UnitArabic,
            UnitPrice = i.UnitPrice,
            MinimumStockLevel = i.MinimumStockLevel,
            ConsumedQuantity = i.ConsumedQuantity,
            StorageLocation = i.StorageLocation,
            StorageLocationArabic = i.StorageLocationArabic,
            RequiredTemperature = i.RequiredTemperature,
            RequiresSpecialStorage = i.RequiresSpecialStorage,
            StorageType = i.StorageType,
            StorageTypeArabic = i.StorageTypeArabic,
            RecommendedDosage = i.RecommendedDosage,
            RecommendedDosageArabic = i.RecommendedDosageArabic,
            SideEffects = i.SideEffects,
            SideEffectsArabic = i.SideEffectsArabic,
            DrugInteractions = i.DrugInteractions,
            DrugInteractionsArabic = i.DrugInteractionsArabic,
            Contraindications = i.Contraindications,
            ContraindicationsArabic = i.ContraindicationsArabic,
            IsExpired = i.IsExpired,
            NearExpiry = i.NearExpiry,
            Notes = i.Notes,
            NotesArabic = i.NotesArabic,
            IsActive = i.IsActive,
            MaxDispenseQuantity = i.MaxDispenseQuantity,
            LastUpdated = i.LastUpdated,
            UpdatedByEmployeeName = i.UpdatedByEmployee?.FullName,
            CreatedAt = i.CreatedAt,
            CreatedByEmployeeName = i.CreatedByEmployee?.FullName
        }).ToList();
    }

    public async Task<MedicalItemDto?> GetMedicalItemAsync(Guid id)
    {
        var item = await _context.MedicalItems
            .Include(m => m.School)
            .Include(m => m.UpdatedByEmployee)
            .Include(m => m.CreatedByEmployee)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (item == null) return null;

        return new MedicalItemDto
        {
            Id = item.Id,
            SchoolId = item.SchoolId,
            ItemName = item.ItemName,
            ItemNameArabic = item.ItemNameArabic,
            ItemType = item.ItemType,
            ItemTypeArabic = item.ItemTypeArabic,
            Manufacturer = item.Manufacturer,
            ManufacturerArabic = item.ManufacturerArabic,
            DrugCode = item.DrugCode,
            BatchNumber = item.BatchNumber,
            ManufacturingDate = item.ManufacturingDate,
            ExpiryDate = item.ExpiryDate,
            AvailableQuantity = item.AvailableQuantity,
            Unit = item.Unit,
            UnitArabic = item.UnitArabic,
            UnitPrice = item.UnitPrice,
            MinimumStockLevel = item.MinimumStockLevel,
            ConsumedQuantity = item.ConsumedQuantity,
            StorageLocation = item.StorageLocation,
            StorageLocationArabic = item.StorageLocationArabic,
            RequiredTemperature = item.RequiredTemperature,
            RequiresSpecialStorage = item.RequiresSpecialStorage,
            StorageType = item.StorageType,
            StorageTypeArabic = item.StorageTypeArabic,
            RecommendedDosage = item.RecommendedDosage,
            RecommendedDosageArabic = item.RecommendedDosageArabic,
            SideEffects = item.SideEffects,
            SideEffectsArabic = item.SideEffectsArabic,
            DrugInteractions = item.DrugInteractions,
            DrugInteractionsArabic = item.DrugInteractionsArabic,
            Contraindications = item.Contraindications,
            ContraindicationsArabic = item.ContraindicationsArabic,
            IsExpired = item.IsExpired,
            NearExpiry = item.NearExpiry,
            Notes = item.Notes,
            NotesArabic = item.NotesArabic,
            IsActive = item.IsActive,
            MaxDispenseQuantity = item.MaxDispenseQuantity,
            LastUpdated = item.LastUpdated,
            UpdatedByEmployeeName = item.UpdatedByEmployee?.FullName,
            CreatedAt = item.CreatedAt,
            CreatedByEmployeeName = item.CreatedByEmployee?.FullName
        };
    }

    public async Task<List<MedicalItemDto>> GetLowStockItemsAsync(Guid schoolId)
    {
        var items = await _context.MedicalItems
            .Where(m => m.SchoolId == schoolId && m.AvailableQuantity <= m.MinimumStockLevel)
            .ToListAsync();

        return items.Select(i => new MedicalItemDto
        {
            Id = i.Id,
            SchoolId = i.SchoolId,
            ItemName = i.ItemName,
            ItemNameArabic = i.ItemNameArabic,
            ItemType = i.ItemType,
            ItemTypeArabic = i.ItemTypeArabic,
            AvailableQuantity = i.AvailableQuantity,
            MinimumStockLevel = i.MinimumStockLevel,
            Unit = i.Unit,
            UnitArabic = i.UnitArabic,
            IsExpired = i.IsExpired,
            NearExpiry = i.NearExpiry,
            IsActive = i.IsActive
        }).ToList();
    }

    public async Task<List<MedicalItemDto>> GetExpiredItemsAsync(Guid schoolId)
    {
        var items = await _context.MedicalItems
            .Where(m => m.SchoolId == schoolId && m.ExpiryDate.HasValue && m.ExpiryDate < DateTime.UtcNow)
            .ToListAsync();

        return items.Select(i => new MedicalItemDto
        {
            Id = i.Id,
            SchoolId = i.SchoolId,
            ItemName = i.ItemName,
            ItemNameArabic = i.ItemNameArabic,
            ItemType = i.ItemType,
            ItemTypeArabic = i.ItemTypeArabic,
            AvailableQuantity = i.AvailableQuantity,
            ExpiryDate = i.ExpiryDate,
            IsExpired = true,
            NearExpiry = i.NearExpiry,
            IsActive = i.IsActive
        }).ToList();
    }

    public async Task<List<MedicalItemDto>> GetNearExpiryItemsAsync(Guid schoolId)
    {
        var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);
        var items = await _context.MedicalItems
            .Where(m => m.SchoolId == schoolId && m.ExpiryDate.HasValue && m.ExpiryDate >= DateTime.UtcNow && m.ExpiryDate <= thirtyDaysFromNow)
            .ToListAsync();

        return items.Select(i => new MedicalItemDto
        {
            Id = i.Id,
            SchoolId = i.SchoolId,
            ItemName = i.ItemName,
            ItemNameArabic = i.ItemNameArabic,
            ItemType = i.ItemType,
            ItemTypeArabic = i.ItemTypeArabic,
            AvailableQuantity = i.AvailableQuantity,
            ExpiryDate = i.ExpiryDate,
            IsExpired = i.IsExpired,
            NearExpiry = true,
            IsActive = i.IsActive
        }).ToList();
    }

    public async Task<MedicalItemDto> CreateMedicalItemAsync(CreateMedicalItemDto dto)
    {
        var item = new MedicalItem
        {
            Id = Guid.NewGuid(),
            SchoolId = dto.SchoolId,
            ItemName = dto.ItemName,
            ItemNameArabic = dto.ItemNameArabic,
            ItemType = dto.ItemType,
            ItemTypeArabic = dto.ItemTypeArabic,
            Manufacturer = dto.Manufacturer,
            ManufacturerArabic = dto.ManufacturerArabic,
            DrugCode = dto.DrugCode,
            BatchNumber = dto.BatchNumber,
            ManufacturingDate = dto.ManufacturingDate,
            ExpiryDate = dto.ExpiryDate,
            AvailableQuantity = dto.AvailableQuantity,
            Unit = dto.Unit,
            UnitArabic = dto.UnitArabic,
            UnitPrice = dto.UnitPrice,
            MinimumStockLevel = dto.MinimumStockLevel,
            ConsumedQuantity = 0,
            StorageLocation = dto.StorageLocation,
            StorageLocationArabic = dto.StorageLocationArabic,
            RequiredTemperature = dto.RequiredTemperature,
            RequiresSpecialStorage = dto.RequiresSpecialStorage,
            StorageType = dto.StorageType,
            StorageTypeArabic = dto.StorageTypeArabic,
            RecommendedDosage = dto.RecommendedDosage,
            RecommendedDosageArabic = dto.RecommendedDosageArabic,
            SideEffects = dto.SideEffects,
            SideEffectsArabic = dto.SideEffectsArabic,
            DrugInteractions = dto.DrugInteractions,
            DrugInteractionsArabic = dto.DrugInteractionsArabic,
            Contraindications = dto.Contraindications,
            ContraindicationsArabic = dto.ContraindicationsArabic,
            IsExpired = dto.ExpiryDate.HasValue && dto.ExpiryDate < DateTime.UtcNow,
            NearExpiry = dto.ExpiryDate.HasValue && dto.ExpiryDate <= DateTime.UtcNow.AddDays(30),
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic,
            IsActive = true,
            MaxDispenseQuantity = dto.MaxDispenseQuantity,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.MedicalItems.Add(item);
        await _context.SaveChangesAsync();

        return await GetMedicalItemAsync(item.Id);
    }

    public async Task<MedicalItemDto> UpdateMedicalItemAsync(UpdateMedicalItemDto dto)
    {
        var item = await _context.MedicalItems.FindAsync(dto.Id);
        if (item == null)
            throw new Exception("Medical item not found");

        item.AvailableQuantity = dto.AvailableQuantity;
        item.ConsumedQuantity = dto.ConsumedQuantity;
        item.IsExpired = dto.IsExpired;
        item.NearExpiry = dto.NearExpiry;
        item.IsActive = dto.IsActive;
        item.Notes = dto.Notes;
        item.NotesArabic = dto.NotesArabic;
        item.LastUpdated = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetMedicalItemAsync(item.Id);
    }

    public async Task<bool> DeleteMedicalItemAsync(Guid id)
    {
        var item = await _context.MedicalItems.FindAsync(id);
        if (item == null) return false;

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DispenseMedicalItemAsync(DispenseMedicalItemDto dto)
    {
        var item = await _context.MedicalItems.FindAsync(dto.MedicalItemId);
        if (item == null) return false;

        if (item.AvailableQuantity < dto.Quantity)
            throw new Exception("Insufficient quantity available");

        item.AvailableQuantity -= dto.Quantity;
        item.ConsumedQuantity += dto.Quantity;
        item.LastUpdated = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    // MedicalExcuse operations
    public async Task<List<MedicalExcuseDto>> GetMedicalExcusesAsync(Guid schoolId)
    {
        var excuses = await _context.MedicalExcuses
            .Include(m => m.Student)
            .Include(m => m.ClinicVisit)
            .Include(m => m.IssuedByEmployee)
            .Where(m => m.SchoolId == schoolId)
            .OrderByDescending(m => m.IssuedAt)
            .ToListAsync();

        return excuses.Select(e => new MedicalExcuseDto
        {
            Id = e.Id,
            SchoolId = e.SchoolId,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            StudentNameArabic = e.Student.FullNameArabic,
            StudentNumber = e.Student.StudentNumber ?? "",
            ClinicVisitId = e.ClinicVisitId,
            ExcuseType = e.ExcuseType,
            ExcuseTypeArabic = e.ExcuseTypeArabic,
            Reason = e.Reason,
            ReasonArabic = e.ReasonArabic,
            Description = e.Description,
            DescriptionArabic = e.DescriptionArabic,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            NumberOfDays = e.NumberOfDays,
            IsDeductible = e.IsDeductible,
            PhysicalActivityRecommendations = e.PhysicalActivityRecommendations,
            PhysicalActivityRecommendationsArabic = e.PhysicalActivityRecommendationsArabic,
            DietaryRecommendations = e.DietaryRecommendations,
            DietaryRecommendationsArabic = e.DietaryRecommendationsArabic,
            IsPhysicalActivityRestricted = e.IsPhysicalActivityRestricted,
            PhysicalActivityRestrictionDuration = e.PhysicalActivityRestrictionDuration,
            PhysicalActivityRestrictionDurationArabic = e.PhysicalActivityRestrictionDurationArabic,
            IsActivityRestricted = e.IsActivityRestricted,
            RestrictedActivities = e.RestrictedActivities,
            RestrictedActivitiesArabic = e.RestrictedActivitiesArabic,
            RequiresFollowUp = e.RequiresFollowUp,
            FollowUpDate = e.FollowUpDate,
            FollowUpNotes = e.FollowUpNotes,
            FollowUpNotesArabic = e.FollowUpNotesArabic,
            PhysicianName = e.PhysicianName,
            PhysicianNameArabic = e.PhysicianNameArabic,
            PhysicianLicenseNumber = e.PhysicianLicenseNumber,
            HospitalName = e.HospitalName,
            HospitalNameArabic = e.HospitalNameArabic,
            IsExternalReport = e.IsExternalReport,
            ExternalReportDate = e.ExternalReportDate,
            IsActive = e.IsActive,
            SentToTeachers = e.SentToTeachers,
            SentToTeachersAt = e.SentToTeachersAt,
            IssuedByEmployeeName = e.IssuedByEmployee?.FullName,
            IssuedAt = e.IssuedAt,
            Notes = e.Notes,
            NotesArabic = e.NotesArabic
        }).ToList();
    }

    public async Task<MedicalExcuseDto?> GetMedicalExcuseAsync(Guid id)
    {
        var excuse = await _context.MedicalExcuses
            .Include(m => m.Student)
            .Include(m => m.ClinicVisit)
            .Include(m => m.IssuedByEmployee)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (excuse == null) return null;

        return new MedicalExcuseDto
        {
            Id = excuse.Id,
            SchoolId = excuse.SchoolId,
            StudentId = excuse.StudentId,
            StudentName = excuse.Student.FullName,
            StudentNameArabic = excuse.Student.FullNameArabic,
            StudentNumber = excuse.Student.StudentNumber ?? "",
            ClinicVisitId = excuse.ClinicVisitId,
            ExcuseType = excuse.ExcuseType,
            ExcuseTypeArabic = excuse.ExcuseTypeArabic,
            Reason = excuse.Reason,
            ReasonArabic = excuse.ReasonArabic,
            Description = excuse.Description,
            DescriptionArabic = excuse.DescriptionArabic,
            StartDate = excuse.StartDate,
            EndDate = excuse.EndDate,
            NumberOfDays = excuse.NumberOfDays,
            IsDeductible = excuse.IsDeductible,
            PhysicalActivityRecommendations = excuse.PhysicalActivityRecommendations,
            PhysicalActivityRecommendationsArabic = excuse.PhysicalActivityRecommendationsArabic,
            DietaryRecommendations = excuse.DietaryRecommendations,
            DietaryRecommendationsArabic = excuse.DietaryRecommendationsArabic,
            IsPhysicalActivityRestricted = excuse.IsPhysicalActivityRestricted,
            PhysicalActivityRestrictionDuration = excuse.PhysicalActivityRestrictionDuration,
            PhysicalActivityRestrictionDurationArabic = excuse.PhysicalActivityRestrictionDurationArabic,
            IsActivityRestricted = excuse.IsActivityRestricted,
            RestrictedActivities = excuse.RestrictedActivities,
            RestrictedActivitiesArabic = excuse.RestrictedActivitiesArabic,
            RequiresFollowUp = excuse.RequiresFollowUp,
            FollowUpDate = excuse.FollowUpDate,
            FollowUpNotes = excuse.FollowUpNotes,
            FollowUpNotesArabic = excuse.FollowUpNotesArabic,
            PhysicianName = excuse.PhysicianName,
            PhysicianNameArabic = excuse.PhysicianNameArabic,
            PhysicianLicenseNumber = excuse.PhysicianLicenseNumber,
            HospitalName = excuse.HospitalName,
            HospitalNameArabic = excuse.HospitalNameArabic,
            IsExternalReport = excuse.IsExternalReport,
            ExternalReportDate = excuse.ExternalReportDate,
            IsActive = excuse.IsActive,
            SentToTeachers = excuse.SentToTeachers,
            SentToTeachersAt = excuse.SentToTeachersAt,
            IssuedByEmployeeName = excuse.IssuedByEmployee?.FullName,
            IssuedAt = excuse.IssuedAt,
            Notes = excuse.Notes,
            NotesArabic = excuse.NotesArabic
        };
    }

    public async Task<List<MedicalExcuseDto>> GetStudentMedicalExcusesAsync(Guid studentId)
    {
        var excuses = await _context.MedicalExcuses
            .Include(m => m.Student)
            .Include(m => m.ClinicVisit)
            .Include(m => m.IssuedByEmployee)
            .Where(m => m.StudentId == studentId)
            .OrderByDescending(m => m.IssuedAt)
            .ToListAsync();

        return excuses.Select(e => new MedicalExcuseDto
        {
            Id = e.Id,
            SchoolId = e.SchoolId,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            StudentNameArabic = e.Student.FullNameArabic,
            StudentNumber = e.Student.StudentNumber ?? "",
            ClinicVisitId = e.ClinicVisitId,
            ExcuseType = e.ExcuseType,
            ExcuseTypeArabic = e.ExcuseTypeArabic,
            Reason = e.Reason,
            ReasonArabic = e.ReasonArabic,
            Description = e.Description,
            DescriptionArabic = e.DescriptionArabic,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            NumberOfDays = e.NumberOfDays,
            IsDeductible = e.IsDeductible,
            PhysicalActivityRecommendations = e.PhysicalActivityRecommendations,
            PhysicalActivityRecommendationsArabic = e.PhysicalActivityRecommendationsArabic,
            DietaryRecommendations = e.DietaryRecommendations,
            DietaryRecommendationsArabic = e.DietaryRecommendationsArabic,
            IsPhysicalActivityRestricted = e.IsPhysicalActivityRestricted,
            PhysicalActivityRestrictionDuration = e.PhysicalActivityRestrictionDuration,
            PhysicalActivityRestrictionDurationArabic = e.PhysicalActivityRestrictionDurationArabic,
            IsActivityRestricted = e.IsActivityRestricted,
            RestrictedActivities = e.RestrictedActivities,
            RestrictedActivitiesArabic = e.RestrictedActivitiesArabic,
            RequiresFollowUp = e.RequiresFollowUp,
            FollowUpDate = e.FollowUpDate,
            FollowUpNotes = e.FollowUpNotes,
            FollowUpNotesArabic = e.FollowUpNotesArabic,
            PhysicianName = e.PhysicianName,
            PhysicianNameArabic = e.PhysicianNameArabic,
            PhysicianLicenseNumber = e.PhysicianLicenseNumber,
            HospitalName = e.HospitalName,
            HospitalNameArabic = e.HospitalNameArabic,
            IsExternalReport = e.IsExternalReport,
            ExternalReportDate = e.ExternalReportDate,
            IsActive = e.IsActive,
            SentToTeachers = e.SentToTeachers,
            SentToTeachersAt = e.SentToTeachersAt,
            IssuedByEmployeeName = e.IssuedByEmployee?.FullName,
            IssuedAt = e.IssuedAt,
            Notes = e.Notes,
            NotesArabic = e.NotesArabic
        }).ToList();
    }

    public async Task<List<MedicalExcuseDto>> GetActiveMedicalExcusesAsync(Guid schoolId)
    {
        var excuses = await _context.MedicalExcuses
            .Include(m => m.Student)
            .Include(m => m.ClinicVisit)
            .Include(m => m.IssuedByEmployee)
            .Where(m => m.SchoolId == schoolId && m.IsActive && m.EndDate >= DateTime.UtcNow)
            .OrderByDescending(m => m.IssuedAt)
            .ToListAsync();

        return excuses.Select(e => new MedicalExcuseDto
        {
            Id = e.Id,
            SchoolId = e.SchoolId,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            StudentNameArabic = e.Student.FullNameArabic,
            StudentNumber = e.Student.StudentNumber ?? "",
            ClinicVisitId = e.ClinicVisitId,
            ExcuseType = e.ExcuseType,
            ExcuseTypeArabic = e.ExcuseTypeArabic,
            Reason = e.Reason,
            ReasonArabic = e.ReasonArabic,
            Description = e.Description,
            DescriptionArabic = e.DescriptionArabic,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            NumberOfDays = e.NumberOfDays,
            IsDeductible = e.IsDeductible,
            PhysicalActivityRecommendations = e.PhysicalActivityRecommendations,
            PhysicalActivityRecommendationsArabic = e.PhysicalActivityRecommendationsArabic,
            DietaryRecommendations = e.DietaryRecommendations,
            DietaryRecommendationsArabic = e.DietaryRecommendationsArabic,
            IsPhysicalActivityRestricted = e.IsPhysicalActivityRestricted,
            PhysicalActivityRestrictionDuration = e.PhysicalActivityRestrictionDuration,
            PhysicalActivityRestrictionDurationArabic = e.PhysicalActivityRestrictionDurationArabic,
            IsActivityRestricted = e.IsActivityRestricted,
            RestrictedActivities = e.RestrictedActivities,
            RestrictedActivitiesArabic = e.RestrictedActivitiesArabic,
            RequiresFollowUp = e.RequiresFollowUp,
            FollowUpDate = e.FollowUpDate,
            FollowUpNotes = e.FollowUpNotes,
            FollowUpNotesArabic = e.FollowUpNotesArabic,
            PhysicianName = e.PhysicianName,
            PhysicianNameArabic = e.PhysicianNameArabic,
            PhysicianLicenseNumber = e.PhysicianLicenseNumber,
            HospitalName = e.HospitalName,
            HospitalNameArabic = e.HospitalNameArabic,
            IsExternalReport = e.IsExternalReport,
            ExternalReportDate = e.ExternalReportDate,
            IsActive = e.IsActive,
            SentToTeachers = e.SentToTeachers,
            SentToTeachersAt = e.SentToTeachersAt,
            IssuedByEmployeeName = e.IssuedByEmployee?.FullName,
            IssuedAt = e.IssuedAt,
            Notes = e.Notes,
            NotesArabic = e.NotesArabic
        }).ToList();
    }

    public async Task<MedicalExcuseDto> CreateMedicalExcuseAsync(CreateMedicalExcuseDto dto)
    {
        var excuse = new MedicalExcuse
        {
            Id = Guid.NewGuid(),
            SchoolId = dto.SchoolId,
            StudentId = dto.StudentId,
            ClinicVisitId = dto.ClinicVisitId,
            ExcuseType = dto.ExcuseType,
            ExcuseTypeArabic = dto.ExcuseTypeArabic,
            Reason = dto.Reason,
            ReasonArabic = dto.ReasonArabic,
            Description = dto.Description,
            DescriptionArabic = dto.DescriptionArabic,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            NumberOfDays = (int)(dto.EndDate - dto.StartDate).TotalDays + 1,
            IsDeductible = dto.IsDeductible,
            PhysicalActivityRecommendations = dto.PhysicalActivityRecommendations,
            PhysicalActivityRecommendationsArabic = dto.PhysicalActivityRecommendationsArabic,
            DietaryRecommendations = dto.DietaryRecommendations,
            DietaryRecommendationsArabic = dto.DietaryRecommendationsArabic,
            IsPhysicalActivityRestricted = dto.IsPhysicalActivityRestricted,
            PhysicalActivityRestrictionDuration = dto.PhysicalActivityRestrictionDuration,
            PhysicalActivityRestrictionDurationArabic = dto.PhysicalActivityRestrictionDurationArabic,
            IsActivityRestricted = dto.IsActivityRestricted,
            RestrictedActivities = dto.RestrictedActivities,
            RestrictedActivitiesArabic = dto.RestrictedActivitiesArabic,
            RequiresFollowUp = dto.RequiresFollowUp,
            FollowUpDate = dto.FollowUpDate,
            FollowUpNotes = dto.FollowUpNotes,
            FollowUpNotesArabic = dto.FollowUpNotesArabic,
            PhysicianName = dto.PhysicianName,
            PhysicianNameArabic = dto.PhysicianNameArabic,
            PhysicianLicenseNumber = dto.PhysicianLicenseNumber,
            HospitalName = dto.HospitalName,
            HospitalNameArabic = dto.HospitalNameArabic,
            IsExternalReport = dto.IsExternalReport,
            ExternalReportDate = dto.ExternalReportDate,
            IsActive = true,
            SentToTeachers = false,
            IssuedAt = DateTime.UtcNow,
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic
        };

        _context.MedicalExcuses.Add(excuse);
        await _context.SaveChangesAsync();

        return await GetMedicalExcuseAsync(excuse.Id);
    }

    public async Task<MedicalExcuseDto> UpdateMedicalExcuseAsync(UpdateMedicalExcuseDto dto)
    {
        var excuse = await _context.MedicalExcuses.FindAsync(dto.Id);
        if (excuse == null)
            throw new Exception("Medical excuse not found");

        excuse.EndDate = dto.EndDate;
        excuse.NumberOfDays = (int)(dto.EndDate - excuse.StartDate).TotalDays + 1;
        excuse.IsActive = dto.IsActive;
        excuse.SentToTeachers = dto.SentToTeachers;
        excuse.Notes = dto.Notes;
        excuse.NotesArabic = dto.NotesArabic;
        excuse.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetMedicalExcuseAsync(excuse.Id);
    }

    public async Task<bool> DeleteMedicalExcuseAsync(Guid id)
    {
        var excuse = await _context.MedicalExcuses.FindAsync(id);
        if (excuse == null) return false;

        excuse.IsDeleted = true;
        excuse.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SendExcuseToTeachersAsync(Guid medicalExcuseId)
    {
        var excuse = await _context.MedicalExcuses.FindAsync(medicalExcuseId);
        if (excuse == null) return false;

        // TODO: Implement notification to teachers
        // This should integrate with the existing notification system

        excuse.SentToTeachers = true;
        excuse.SentToTeachersAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    // Dashboard statistics
    public async Task<ClinicDashboardDto> GetClinicDashboardAsync(Guid schoolId)
    {
        var today = DateTime.UtcNow.Date;
        var thisMonth = new DateTime(today.Year, today.Month, 1);

        var totalVisitsToday = await _context.ClinicVisits
            .CountAsync(v => v.SchoolId == schoolId && v.VisitDate.Date == today);

        var totalVisitsThisMonth = await _context.ClinicVisits
            .CountAsync(v => v.SchoolId == schoolId && v.VisitDate >= thisMonth);

        var emergencyVisitsToday = await _context.ClinicVisits
            .CountAsync(v => v.SchoolId == schoolId && v.VisitDate.Date == today && v.IsEmergency);

        var patientsSentHomeToday = await _context.ClinicVisits
            .CountAsync(v => v.SchoolId == schoolId && v.VisitDate.Date == today && v.SentHome);

        var patientsReferredToHospitalToday = await _context.ClinicVisits
            .CountAsync(v => v.SchoolId == schoolId && v.VisitDate.Date == today && v.ReferredToHospital);

        var lowStockItems = await _context.MedicalItems
            .CountAsync(m => m.SchoolId == schoolId && m.AvailableQuantity <= m.MinimumStockLevel);

        var expiredItems = await _context.MedicalItems
            .CountAsync(m => m.SchoolId == schoolId && m.ExpiryDate.HasValue && m.ExpiryDate < DateTime.UtcNow);

        var nearExpiryItems = await _context.MedicalItems
            .CountAsync(m => m.SchoolId == schoolId && m.ExpiryDate.HasValue && m.ExpiryDate >= DateTime.UtcNow && m.ExpiryDate <= DateTime.UtcNow.AddDays(30));

        var activeMedicalExcuses = await _context.MedicalExcuses
            .CountAsync(m => m.SchoolId == schoolId && m.IsActive && m.EndDate >= DateTime.UtcNow);

        var chronicDiseaseStudents = await _context.StudentHealthProfiles
            .CountAsync(s => s.Student.SchoolId == schoolId && !string.IsNullOrEmpty(s.ChronicDiseases));

        var allergyStudents = await _context.StudentHealthProfiles
            .CountAsync(s => s.Student.SchoolId == schoolId && !string.IsNullOrEmpty(s.Allergies));

        var topVisitingStudents = await _context.ClinicVisits
            .Where(v => v.SchoolId == schoolId)
            .GroupBy(v => v.StudentId)
            .Select(g => new StudentVisitCountDto
            {
                StudentId = g.Key,
                StudentName = g.First().Student.FullName,
                StudentNameArabic = g.First().Student.FullNameArabic,
                StudentNumber = g.First().Student.StudentNumber ?? "",
                VisitCount = g.Count()
            })
            .OrderByDescending(s => s.VisitCount)
            .Take(10)
            .ToListAsync();

        var recentVisits = await _context.ClinicVisits
            .Include(v => v.Student)
            .Where(v => v.SchoolId == schoolId)
            .OrderByDescending(v => v.VisitDate)
            .Take(10)
            .Select(v => new RecentVisitDto
            {
                Id = v.Id,
                StudentId = v.StudentId,
                StudentName = v.Student.FullName,
                StudentNameArabic = v.Student.FullNameArabic,
                VisitDate = v.VisitDate,
                VisitReason = v.VisitReason,
                VisitReasonArabic = v.VisitReasonArabic,
                IsEmergency = v.IsEmergency,
                PatientCondition = v.PatientCondition
            })
            .ToListAsync();

        return new ClinicDashboardDto
        {
            TotalVisitsToday = totalVisitsToday,
            TotalVisitsThisMonth = totalVisitsThisMonth,
            EmergencyVisitsToday = emergencyVisitsToday,
            PatientsSentHomeToday = patientsSentHomeToday,
            PatientsReferredToHospitalToday = patientsReferredToHospitalToday,
            LowStockItems = lowStockItems,
            ExpiredItems = expiredItems,
            NearExpiryItems = nearExpiryItems,
            ActiveMedicalExcuses = activeMedicalExcuses,
            ChronicDiseaseStudents = chronicDiseaseStudents,
            AllergyStudents = allergyStudents,
            TopVisitingStudents = topVisitingStudents,
            RecentVisits = recentVisits
        };
    }
}