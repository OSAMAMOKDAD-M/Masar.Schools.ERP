using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Application.Interfaces;

public interface IClinicService
{
    // StudentHealthProfile operations
    Task<StudentHealthProfileDto?> GetStudentHealthProfileAsync(Guid studentId);
    Task<StudentHealthProfileDto> CreateStudentHealthProfileAsync(CreateStudentHealthProfileDto dto);
    Task<StudentHealthProfileDto> UpdateStudentHealthProfileAsync(UpdateStudentHealthProfileDto dto);
    Task<bool> DeleteStudentHealthProfileAsync(Guid id);

    // ClinicVisit operations
    Task<List<ClinicVisitDto>> GetClinicVisitsAsync(Guid schoolId, DateTime? startDate = null, DateTime? endDate = null);
    Task<ClinicVisitDto?> GetClinicVisitAsync(Guid id);
    Task<List<ClinicVisitDto>> GetStudentClinicVisitsAsync(Guid studentId);
    Task<ClinicVisitDto> CreateClinicVisitAsync(CreateClinicVisitDto dto);
    Task<ClinicVisitDto> UpdateClinicVisitAsync(UpdateClinicVisitDto dto);
    Task<bool> DeleteClinicVisitAsync(Guid id);
    Task<bool> SendWhatsAppNotificationAsync(Guid clinicVisitId);

    // MedicalItem operations
    Task<List<MedicalItemDto>> GetMedicalItemsAsync(Guid schoolId);
    Task<MedicalItemDto?> GetMedicalItemAsync(Guid id);
    Task<List<MedicalItemDto>> GetLowStockItemsAsync(Guid schoolId);
    Task<List<MedicalItemDto>> GetExpiredItemsAsync(Guid schoolId);
    Task<List<MedicalItemDto>> GetNearExpiryItemsAsync(Guid schoolId);
    Task<MedicalItemDto> CreateMedicalItemAsync(CreateMedicalItemDto dto);
    Task<MedicalItemDto> UpdateMedicalItemAsync(UpdateMedicalItemDto dto);
    Task<bool> DeleteMedicalItemAsync(Guid id);
    Task<bool> DispenseMedicalItemAsync(DispenseMedicalItemDto dto);

    // MedicalExcuse operations
    Task<List<MedicalExcuseDto>> GetMedicalExcusesAsync(Guid schoolId);
    Task<MedicalExcuseDto?> GetMedicalExcuseAsync(Guid id);
    Task<List<MedicalExcuseDto>> GetStudentMedicalExcusesAsync(Guid studentId);
    Task<List<MedicalExcuseDto>> GetActiveMedicalExcusesAsync(Guid schoolId);
    Task<MedicalExcuseDto> CreateMedicalExcuseAsync(CreateMedicalExcuseDto dto);
    Task<MedicalExcuseDto> UpdateMedicalExcuseAsync(UpdateMedicalExcuseDto dto);
    Task<bool> DeleteMedicalExcuseAsync(Guid id);
    Task<bool> SendExcuseToTeachersAsync(Guid medicalExcuseId);

    // Dashboard statistics
    Task<ClinicDashboardDto> GetClinicDashboardAsync(Guid schoolId);
}