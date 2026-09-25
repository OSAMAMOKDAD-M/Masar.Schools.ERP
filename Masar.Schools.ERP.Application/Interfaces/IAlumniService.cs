using Masar.Schools.ERP.Domain.DTOs;

namespace Masar.Schools.ERP.Application.Interfaces;

public interface IAlumniService
{
    // Dashboard
    Task<AlumniDashboardDto> GetDashboardAsync(Guid tenantId, Guid schoolId);
    
    // Alumni Records
    Task<List<AlumniRecordDto>> GetAllAlumniRecordsAsync(Guid tenantId, Guid schoolId);
    Task<AlumniRecordDto?> GetAlumniRecordByIdAsync(Guid id, Guid tenantId, Guid schoolId);
    Task<AlumniRecordDto?> GetAlumniRecordByStudentIdAsync(Guid studentId, Guid tenantId, Guid schoolId);
    Task<AlumniRecordDto> CreateAlumniRecordAsync(CreateAlumniRecordDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId);
    Task<AlumniRecordDto> UpdateAlumniRecordAsync(UpdateAlumniRecordDto dto, Guid tenantId, Guid schoolId);
    Task<bool> DeleteAlumniRecordAsync(Guid id, Guid tenantId, Guid schoolId);
    Task<List<AlumniRecordDto>> SearchAlumniRecordsAsync(string searchTerm, Guid tenantId, Guid schoolId);
    
    // Graduation Documents
    Task<List<GraduationDocumentDto>> GetAlumniDocumentsAsync(Guid alumniRecordId, Guid tenantId, Guid schoolId);
    Task<GraduationDocumentDto?> GetGraduationDocumentByIdAsync(Guid id, Guid tenantId, Guid schoolId);
    Task<GraduationDocumentDto> CreateGraduationDocumentAsync(CreateGraduationDocumentDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId);
    Task<GraduationDocumentDto> UpdateGraduationDocumentAsync(UpdateGraduationDocumentDto dto, Guid tenantId, Guid schoolId);
    Task<bool> DeleteGraduationDocumentAsync(Guid id, Guid tenantId, Guid schoolId);
    
    // Graduation Clearance
    Task<List<GraduationClearanceRecordDto>> GetAllClearanceRecordsAsync(Guid tenantId, Guid schoolId);
    Task<GraduationClearanceRecordDto?> GetClearanceRecordByIdAsync(Guid id, Guid tenantId, Guid schoolId);
    Task<GraduationClearanceRecordDto?> GetClearanceRecordByStudentIdAsync(Guid studentId, Guid tenantId, Guid schoolId);
    Task<GraduationClearanceRecordDto> CreateClearanceRecordAsync(CreateGraduationClearanceRecordDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId);
    Task<GraduationClearanceRecordDto> UpdateClearanceRecordAsync(UpdateGraduationClearanceRecordDto dto, Guid tenantId, Guid schoolId, Guid approvedByEmployeeId);
    Task<bool> DeleteClearanceRecordAsync(Guid id, Guid tenantId, Guid schoolId);
    Task<List<GraduationClearanceRecordDto>> GetPendingClearanceRecordsAsync(Guid tenantId, Guid schoolId);
    
    // Graduation Workflow
    Task<bool> GraduateStudentAsync(Guid studentId, int graduationYear, decimal? finalGPA, Guid tenantId, Guid schoolId, Guid processedByEmployeeId);
    Task<bool> CheckFinancialClearanceAsync(Guid studentId, Guid tenantId, Guid schoolId);
    Task<bool> ProcessGraduationWorkflowAsync(Guid studentId, int graduationYear, decimal? finalGPA, Guid tenantId, Guid schoolId, Guid processedByEmployeeId);
}