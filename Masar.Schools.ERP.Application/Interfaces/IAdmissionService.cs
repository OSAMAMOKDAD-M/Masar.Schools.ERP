using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities.Admissions;

namespace Masar.Schools.ERP.Application.Interfaces;

/// <summary>
/// واجهة خدمة القبول والتسجيل
/// </summary>
public interface IAdmissionService
{
    // ==================== Admission Applications ====================
    Task<AdmissionResponseDto<AdmissionApplicationDto>> CreateAdmissionApplicationAsync(CreateAdmissionApplicationDto dto);
    Task<AdmissionResponseDto<AdmissionApplicationDto>> UpdateAdmissionApplicationAsync(UpdateAdmissionApplicationDto dto);
    Task<AdmissionResponseDto<bool>> DeleteAdmissionApplicationAsync(Guid id);
    Task<AdmissionResponseDto<AdmissionApplicationDto>> GetAdmissionApplicationAsync(Guid id);
    Task<AdmissionPagedResponseDto<AdmissionApplicationDto>> GetAdmissionApplicationsAsync(AdmissionSearchDto search);
    Task<AdmissionResponseDto<bool>> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto);
    Task<AdmissionResponseDto<AdmissionStatisticsDto>> GetAdmissionStatisticsAsync(Guid schoolId, string? academicYear);

    // ==================== Admission Exams ====================
    Task<AdmissionResponseDto<AdmissionExamDto>> CreateAdmissionExamAsync(CreateAdmissionExamDto dto);
    Task<AdmissionResponseDto<AdmissionExamDto>> UpdateAdmissionExamAsync(UpdateAdmissionExamDto dto);
    Task<AdmissionResponseDto<AdmissionExamDto>> GetAdmissionExamAsync(Guid id);
    Task<AdmissionResponseDto<AdmissionExamDto>> GetExamByApplicationIdAsync(Guid applicationId);

    // ==================== Application Documents ====================
    Task<AdmissionResponseDto<ApplicationDocumentDto>> CreateApplicationDocumentAsync(CreateApplicationDocumentDto dto);
    Task<AdmissionResponseDto<bool>> UpdateApplicationDocumentAsync(UpdateApplicationDocumentDto dto);
    Task<AdmissionResponseDto<bool>> DeleteApplicationDocumentAsync(Guid id);
    Task<AdmissionPagedResponseDto<ApplicationDocumentDto>> GetApplicationDocumentsAsync(Guid applicationId);

    // ==================== Student Conversion ====================
    Task<AdmissionResponseDto<ConversionResultDto>> ConvertToStudentAsync(ConvertToStudentDto dto);
}