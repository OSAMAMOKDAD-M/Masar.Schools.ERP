namespace Masar.Schools.ERP.Domain.DTOs;

// StudentHealthProfile DTOs
public class StudentHealthProfileDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string? BloodType { get; set; }
    public string? RhFactor { get; set; }
    public string? Allergies { get; set; }
    public string? AllergiesArabic { get; set; }
    public string? ProhibitedFoods { get; set; }
    public string? ProhibitedFoodsArabic { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? ChronicDiseasesArabic { get; set; }
    public string? PhysicalDisabilities { get; set; }
    public string? PhysicalDisabilitiesArabic { get; set; }
    public string? Vaccinations { get; set; }
    public string? VaccinationsArabic { get; set; }
    public DateTime? LastComprehensiveCheckup { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string? DietaryRestrictionsArabic { get; set; }
    public string? AdditionalNotes { get; set; }
    public string? AdditionalNotesArabic { get; set; }
    public bool RequiresSpecialCare { get; set; }
    public string? SpecialCareType { get; set; }
    public string? SpecialCareTypeArabic { get; set; }
    public string? TreatingPhysician { get; set; }
    public string? TreatingPhysicianArabic { get; set; }
    public string? PhysicianPhone { get; set; }
    public string? PreferredHospital { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? InsuranceCompany { get; set; }
    public DateTime ProfileCreatedDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? CreatedByEmployeeName { get; set; }
    public int VisitCount { get; set; }
}

public class CreateStudentHealthProfileDto
{
    public Guid StudentId { get; set; }
    public string? BloodType { get; set; }
    public string? RhFactor { get; set; }
    public string? Allergies { get; set; }
    public string? AllergiesArabic { get; set; }
    public string? ProhibitedFoods { get; set; }
    public string? ProhibitedFoodsArabic { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? ChronicDiseasesArabic { get; set; }
    public string? PhysicalDisabilities { get; set; }
    public string? PhysicalDisabilitiesArabic { get; set; }
    public string? Vaccinations { get; set; }
    public string? VaccinationsArabic { get; set; }
    public DateTime? LastComprehensiveCheckup { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string? DietaryRestrictionsArabic { get; set; }
    public string? AdditionalNotes { get; set; }
    public string? AdditionalNotesArabic { get; set; }
    public bool RequiresSpecialCare { get; set; }
    public string? SpecialCareType { get; set; }
    public string? SpecialCareTypeArabic { get; set; }
    public string? TreatingPhysician { get; set; }
    public string? TreatingPhysicianArabic { get; set; }
    public string? PhysicianPhone { get; set; }
    public string? PreferredHospital { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? InsuranceCompany { get; set; }
}

public class UpdateStudentHealthProfileDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string? BloodType { get; set; }
    public string? RhFactor { get; set; }
    public string? Allergies { get; set; }
    public string? AllergiesArabic { get; set; }
    public string? ProhibitedFoods { get; set; }
    public string? ProhibitedFoodsArabic { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? ChronicDiseasesArabic { get; set; }
    public string? PhysicalDisabilities { get; set; }
    public string? PhysicalDisabilitiesArabic { get; set; }
    public string? Vaccinations { get; set; }
    public string? VaccinationsArabic { get; set; }
    public DateTime? LastComprehensiveCheckup { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string? DietaryRestrictionsArabic { get; set; }
    public string? AdditionalNotes { get; set; }
    public string? AdditionalNotesArabic { get; set; }
    public bool RequiresSpecialCare { get; set; }
    public string? SpecialCareType { get; set; }
    public string? SpecialCareTypeArabic { get; set; }
    public string? TreatingPhysician { get; set; }
    public string? TreatingPhysicianArabic { get; set; }
    public string? PhysicianPhone { get; set; }
    public string? PreferredHospital { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? InsuranceCompany { get; set; }
}

// ClinicVisit DTOs
public class ClinicVisitDto
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string? AttendingEmployeeName { get; set; }
    public DateTime VisitDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? VisitReason { get; set; }
    public string? VisitReasonArabic { get; set; }
    public string? VisitType { get; set; }
    public string? VisitTypeArabic { get; set; }
    public string? Symptoms { get; set; }
    public string? SymptomsArabic { get; set; }
    public decimal? Temperature { get; set; }
    public int? BloodPressureSystolic { get; set; }
    public int? BloodPressureDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryRate { get; set; }
    public int? OxygenSaturation { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public string? PreliminaryDiagnosis { get; set; }
    public string? PreliminaryDiagnosisArabic { get; set; }
    public string? Treatment { get; set; }
    public string? TreatmentArabic { get; set; }
    public string? MedicationPrescribed { get; set; }
    public string? MedicationPrescribedArabic { get; set; }
    public string? Dosage { get; set; }
    public string? DosageArabic { get; set; }
    public string? Duration { get; set; }
    public string? DurationArabic { get; set; }
    public string? PatientCondition { get; set; }
    public string? PatientConditionArabic { get; set; }
    public bool ReturnedToClass { get; set; }
    public bool ReferredToHospital { get; set; }
    public bool SentHome { get; set; }
    public string? ReferredHospital { get; set; }
    public string? ReferredHospitalArabic { get; set; }
    public string? ReferralReason { get; set; }
    public string? ReferralReasonArabic { get; set; }
    public bool IsEmergency { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool WhatsAppNotificationSent { get; set; }
    public DateTime? WhatsAppNotificationSentAt { get; set; }
}

public class CreateClinicVisitDto
{
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? AttendingEmployeeId { get; set; }
    public DateTime VisitDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? VisitReason { get; set; }
    public string? VisitReasonArabic { get; set; }
    public string? VisitType { get; set; }
    public string? VisitTypeArabic { get; set; }
    public string? Symptoms { get; set; }
    public string? SymptomsArabic { get; set; }
    public decimal? Temperature { get; set; }
    public int? BloodPressureSystolic { get; set; }
    public int? BloodPressureDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public int? RespiratoryRate { get; set; }
    public int? OxygenSaturation { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public string? PreliminaryDiagnosis { get; set; }
    public string? PreliminaryDiagnosisArabic { get; set; }
    public string? Treatment { get; set; }
    public string? TreatmentArabic { get; set; }
    public string? MedicationPrescribed { get; set; }
    public string? MedicationPrescribedArabic { get; set; }
    public string? Dosage { get; set; }
    public string? DosageArabic { get; set; }
    public string? Duration { get; set; }
    public string? DurationArabic { get; set; }
    public string? PatientCondition { get; set; }
    public string? PatientConditionArabic { get; set; }
    public bool ReturnedToClass { get; set; }
    public bool ReferredToHospital { get; set; }
    public bool SentHome { get; set; }
    public string? ReferredHospital { get; set; }
    public string? ReferredHospitalArabic { get; set; }
    public string? ReferralReason { get; set; }
    public string? ReferralReasonArabic { get; set; }
    public bool IsEmergency { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateClinicVisitDto
{
    public Guid Id { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? PreliminaryDiagnosis { get; set; }
    public string? PreliminaryDiagnosisArabic { get; set; }
    public string? Treatment { get; set; }
    public string? TreatmentArabic { get; set; }
    public string? MedicationPrescribed { get; set; }
    public string? MedicationPrescribedArabic { get; set; }
    public string? Dosage { get; set; }
    public string? DosageArabic { get; set; }
    public string? Duration { get; set; }
    public string? DurationArabic { get; set; }
    public string? PatientCondition { get; set; }
    public string? PatientConditionArabic { get; set; }
    public bool ReturnedToClass { get; set; }
    public bool ReferredToHospital { get; set; }
    public bool SentHome { get; set; }
    public string? ReferredHospital { get; set; }
    public string? ReferredHospitalArabic { get; set; }
    public string? ReferralReason { get; set; }
    public string? ReferralReasonArabic { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

// MedicalItem DTOs
public class MedicalItemDto
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? ItemNameArabic { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string? ItemTypeArabic { get; set; }
    public string? Manufacturer { get; set; }
    public string? ManufacturerArabic { get; set; }
    public string? DrugCode { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int AvailableQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? UnitArabic { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinimumStockLevel { get; set; }
    public int ConsumedQuantity { get; set; }
    public string? StorageLocation { get; set; }
    public string? StorageLocationArabic { get; set; }
    public string? RequiredTemperature { get; set; }
    public bool RequiresSpecialStorage { get; set; }
    public string? StorageType { get; set; }
    public string? StorageTypeArabic { get; set; }
    public string? RecommendedDosage { get; set; }
    public string? RecommendedDosageArabic { get; set; }
    public string? SideEffects { get; set; }
    public string? SideEffectsArabic { get; set; }
    public string? DrugInteractions { get; set; }
    public string? DrugInteractionsArabic { get; set; }
    public string? Contraindications { get; set; }
    public string? ContraindicationsArabic { get; set; }
    public bool IsExpired { get; set; }
    public bool NearExpiry { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
    public bool IsActive { get; set; }
    public int MaxDispenseQuantity { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? UpdatedByEmployeeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedByEmployeeName { get; set; }
}

public class CreateMedicalItemDto
{
    public Guid SchoolId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? ItemNameArabic { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string? ItemTypeArabic { get; set; }
    public string? Manufacturer { get; set; }
    public string? ManufacturerArabic { get; set; }
    public string? DrugCode { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int AvailableQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? UnitArabic { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinimumStockLevel { get; set; }
    public string? StorageLocation { get; set; }
    public string? StorageLocationArabic { get; set; }
    public string? RequiredTemperature { get; set; }
    public bool RequiresSpecialStorage { get; set; }
    public string? StorageType { get; set; }
    public string? StorageTypeArabic { get; set; }
    public string? RecommendedDosage { get; set; }
    public string? RecommendedDosageArabic { get; set; }
    public string? SideEffects { get; set; }
    public string? SideEffectsArabic { get; set; }
    public string? DrugInteractions { get; set; }
    public string? DrugInteractionsArabic { get; set; }
    public string? Contraindications { get; set; }
    public string? ContraindicationsArabic { get; set; }
    public int MaxDispenseQuantity { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateMedicalItemDto
{
    public Guid Id { get; set; }
    public int AvailableQuantity { get; set; }
    public int ConsumedQuantity { get; set; }
    public bool IsExpired { get; set; }
    public bool NearExpiry { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class DispenseMedicalItemDto
{
    public Guid MedicalItemId { get; set; }
    public int Quantity { get; set; }
    public Guid? DispensedByEmployeeId { get; set; }
    public string? Notes { get; set; }
}

// MedicalExcuse DTOs
public class MedicalExcuseDto
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public Guid? ClinicVisitId { get; set; }
    public string ExcuseType { get; set; } = string.Empty;
    public string? ExcuseTypeArabic { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReasonArabic { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfDays { get; set; }
    public bool IsDeductible { get; set; }
    public string? PhysicalActivityRecommendations { get; set; }
    public string? PhysicalActivityRecommendationsArabic { get; set; }
    public string? DietaryRecommendations { get; set; }
    public string? DietaryRecommendationsArabic { get; set; }
    public bool IsPhysicalActivityRestricted { get; set; }
    public string? PhysicalActivityRestrictionDuration { get; set; }
    public string? PhysicalActivityRestrictionDurationArabic { get; set; }
    public bool IsActivityRestricted { get; set; }
    public string? RestrictedActivities { get; set; }
    public string? RestrictedActivitiesArabic { get; set; }
    public bool RequiresFollowUp { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? FollowUpNotes { get; set; }
    public string? FollowUpNotesArabic { get; set; }
    public string? PhysicianName { get; set; }
    public string? PhysicianNameArabic { get; set; }
    public string? PhysicianLicenseNumber { get; set; }
    public string? HospitalName { get; set; }
    public string? HospitalNameArabic { get; set; }
    public bool IsExternalReport { get; set; }
    public DateTime? ExternalReportDate { get; set; }
    public bool IsActive { get; set; }
    public bool SentToTeachers { get; set; }
    public DateTime? SentToTeachersAt { get; set; }
    public string? IssuedByEmployeeName { get; set; }
    public DateTime IssuedAt { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class CreateMedicalExcuseDto
{
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? ClinicVisitId { get; set; }
    public string ExcuseType { get; set; } = string.Empty;
    public string? ExcuseTypeArabic { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReasonArabic { get; set; }
    public string? Description { get; set; }
    public string? DescriptionArabic { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDeductible { get; set; }
    public string? PhysicalActivityRecommendations { get; set; }
    public string? PhysicalActivityRecommendationsArabic { get; set; }
    public string? DietaryRecommendations { get; set; }
    public string? DietaryRecommendationsArabic { get; set; }
    public bool IsPhysicalActivityRestricted { get; set; }
    public string? PhysicalActivityRestrictionDuration { get; set; }
    public string? PhysicalActivityRestrictionDurationArabic { get; set; }
    public bool IsActivityRestricted { get; set; }
    public string? RestrictedActivities { get; set; }
    public string? RestrictedActivitiesArabic { get; set; }
    public bool RequiresFollowUp { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? FollowUpNotes { get; set; }
    public string? FollowUpNotesArabic { get; set; }
    public string? PhysicianName { get; set; }
    public string? PhysicianNameArabic { get; set; }
    public string? PhysicianLicenseNumber { get; set; }
    public string? HospitalName { get; set; }
    public string? HospitalNameArabic { get; set; }
    public bool IsExternalReport { get; set; }
    public DateTime? ExternalReportDate { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

public class UpdateMedicalExcuseDto
{
    public Guid Id { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public bool SentToTeachers { get; set; }
    public string? Notes { get; set; }
    public string? NotesArabic { get; set; }
}

// Dashboard DTOs
public class ClinicDashboardDto
{
    public int TotalVisitsToday { get; set; }
    public int TotalVisitsThisMonth { get; set; }
    public int EmergencyVisitsToday { get; set; }
    public int PatientsSentHomeToday { get; set; }
    public int PatientsReferredToHospitalToday { get; set; }
    public int LowStockItems { get; set; }
    public int ExpiredItems { get; set; }
    public int NearExpiryItems { get; set; }
    public int ActiveMedicalExcuses { get; set; }
    public int ChronicDiseaseStudents { get; set; }
    public int AllergyStudents { get; set; }
    public List<StudentVisitCountDto> TopVisitingStudents { get; set; } = new();
    public List<RecentVisitDto> RecentVisits { get; set; } = new();
}

public class StudentVisitCountDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public int VisitCount { get; set; }
}

public class RecentVisitDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNameArabic { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? VisitReason { get; set; }
    public string? VisitReasonArabic { get; set; }
    public bool IsEmergency { get; set; }
    public string? PatientCondition { get; set; }
    public bool ReferredToHospital { get; set; }
    public bool SentHome { get; set; }
}