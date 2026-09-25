using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.Alumni;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Application.Services;

public class AlumniService : IAlumniService
{
    private readonly MasarDbContext _context;

    public AlumniService(MasarDbContext context)
    {
        _context = context;
    }

    public async Task<AlumniDashboardDto> GetDashboardAsync(Guid tenantId, Guid schoolId)
    {
        var totalAlumni = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId)
            .CountAsync();

        var activeAlumni = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && a.IsActive)
            .CountAsync();

        var verifiedAlumni = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && a.IsVerified)
            .CountAsync();

        var pendingClearance = await _context.GraduationClearanceRecords
            .Where(g => g.TenantId == tenantId && g.SchoolId == schoolId && g.ClearanceStatus == "Pending")
            .CountAsync();

        var currentYear = DateTime.Now.Year;
        var clearedThisYear = await _context.GraduationClearanceRecords
            .Where(g => g.TenantId == tenantId && g.SchoolId == schoolId && 
                       g.FinalApproval == true && g.FinalApprovalDate.HasValue &&
                       g.FinalApprovalDate.Value.Year == currentYear)
            .CountAsync();

        var employedAlumni = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && 
                       a.EmploymentStatus == "Employed")
            .CountAsync();

        var unemployedAlumni = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && 
                       a.EmploymentStatus == "Unemployed")
            .CountAsync();

        var universityStudents = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && 
                       a.EmploymentStatus == "Student")
            .CountAsync();

        var recentGraduates = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId)
            .OrderByDescending(a => a.GraduationDate)
            .Take(10)
            .Select(a => new RecentGraduateDto
            {
                Id = a.Id,
                StudentName = a.Student.FullName,
                StudentNameArabic = a.Student.FullNameArabic,
                GraduationDate = a.GraduationDate,
                FinalGPA = a.FinalGPA,
                University = a.University,
                Major = a.Major,
                EmploymentStatus = a.EmploymentStatus
            })
            .ToListAsync();

        var employmentStats = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && a.EmploymentStatus != null)
            .GroupBy(a => a.EmploymentStatus)
            .Select(g => new AlumniStatDto
            {
                Category = g.Key,
                CategoryArabic = g.First().EmploymentStatusArabic,
                Count = g.Count(),
                Percentage = totalAlumni > 0 ? (decimal)g.Count() / totalAlumni * 100 : 0
            })
            .ToListAsync();

        var universityStats = await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId && a.University != null)
            .GroupBy(a => a.University)
            .Select(g => new AlumniStatDto
            {
                Category = g.Key,
                CategoryArabic = g.First().UniversityArabic,
                Count = g.Count(),
                Percentage = totalAlumni > 0 ? (decimal)g.Count() / totalAlumni * 100 : 0
            })
            .Take(10)
            .ToListAsync();

        return new AlumniDashboardDto
        {
            TotalAlumni = totalAlumni,
            ActiveAlumni = activeAlumni,
            VerifiedAlumni = verifiedAlumni,
            PendingClearance = pendingClearance,
            ClearedThisYear = clearedThisYear,
            EmployedAlumni = employedAlumni,
            UnemployedAlumni = unemployedAlumni,
            UniversityStudents = universityStudents,
            RecentGraduates = recentGraduates,
            EmploymentStats = employmentStats,
            UniversityStats = universityStats
        };
    }

    public async Task<List<AlumniRecordDto>> GetAllAlumniRecordsAsync(Guid tenantId, Guid schoolId)
    {
        return await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId)
            .Include(a => a.Student)
            .Select(a => new AlumniRecordDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FullName,
                StudentNameArabic = a.Student.FullNameArabic,
                StudentNumber = a.Student.StudentNumber ?? "",
                GraduationDate = a.GraduationDate,
                GraduationYear = a.GraduationYear,
                FinalGPA = a.FinalGPA,
                GradeLevel = a.GradeLevel,
                GradeLevelArabic = a.GradeLevelArabic,
                Section = a.Section,
                SectionArabic = a.SectionArabic,
                University = a.University,
                UniversityArabic = a.UniversityArabic,
                Major = a.Major,
                MajorArabic = a.MajorArabic,
                UniversityEnrollmentDate = a.UniversityEnrollmentDate,
                UniversityGraduationDate = a.UniversityGraduationDate,
                EmploymentStatus = a.EmploymentStatus,
                EmploymentStatusArabic = a.EmploymentStatusArabic,
                CompanyName = a.CompanyName,
                CompanyNameArabic = a.CompanyNameArabic,
                JobTitle = a.JobTitle,
                JobTitleArabic = a.JobTitleArabic,
                Industry = a.Industry,
                IndustryArabic = a.IndustryArabic,
                EmploymentStartDate = a.EmploymentStartDate,
                PersonalEmail = a.PersonalEmail,
                PersonalPhone = a.PersonalPhone,
                LinkedInProfile = a.LinkedInProfile,
                Website = a.Website,
                CurrentAddress = a.CurrentAddress,
                CurrentAddressArabic = a.CurrentAddressArabic,
                Achievements = a.Achievements,
                AchievementsArabic = a.AchievementsArabic,
                Notes = a.Notes,
                NotesArabic = a.NotesArabic,
                IsActive = a.IsActive,
                IsVerified = a.IsVerified,
                LastContactDate = a.LastContactDate,
                CreatedAt = a.CreatedAt,
                CreatedByEmployeeName = a.CreatedByEmployee != null ? a.CreatedByEmployee.FullName : "",
                LastUpdated = a.LastUpdated,
                DocumentCount = a.GraduationDocuments.Count
            })
            .OrderByDescending(a => a.GraduationDate)
            .ToListAsync();
    }

    public async Task<AlumniRecordDto?> GetAlumniRecordByIdAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        return await _context.AlumniRecords
            .Where(a => a.Id == id && a.TenantId == tenantId && a.SchoolId == schoolId)
            .Include(a => a.Student)
            .Include(a => a.GraduationDocuments)
            .Select(a => new AlumniRecordDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FullName,
                StudentNameArabic = a.Student.FullNameArabic,
                StudentNumber = a.Student.StudentNumber ?? "",
                GraduationDate = a.GraduationDate,
                GraduationYear = a.GraduationYear,
                FinalGPA = a.FinalGPA,
                GradeLevel = a.GradeLevel,
                GradeLevelArabic = a.GradeLevelArabic,
                Section = a.Section,
                SectionArabic = a.SectionArabic,
                University = a.University,
                UniversityArabic = a.UniversityArabic,
                Major = a.Major,
                MajorArabic = a.MajorArabic,
                UniversityEnrollmentDate = a.UniversityEnrollmentDate,
                UniversityGraduationDate = a.UniversityGraduationDate,
                EmploymentStatus = a.EmploymentStatus,
                EmploymentStatusArabic = a.EmploymentStatusArabic,
                CompanyName = a.CompanyName,
                CompanyNameArabic = a.CompanyNameArabic,
                JobTitle = a.JobTitle,
                JobTitleArabic = a.JobTitleArabic,
                Industry = a.Industry,
                IndustryArabic = a.IndustryArabic,
                EmploymentStartDate = a.EmploymentStartDate,
                PersonalEmail = a.PersonalEmail,
                PersonalPhone = a.PersonalPhone,
                LinkedInProfile = a.LinkedInProfile,
                Website = a.Website,
                CurrentAddress = a.CurrentAddress,
                CurrentAddressArabic = a.CurrentAddressArabic,
                Achievements = a.Achievements,
                AchievementsArabic = a.AchievementsArabic,
                Notes = a.Notes,
                NotesArabic = a.NotesArabic,
                IsActive = a.IsActive,
                IsVerified = a.IsVerified,
                LastContactDate = a.LastContactDate,
                CreatedAt = a.CreatedAt,
                CreatedByEmployeeName = a.CreatedByEmployee != null ? a.CreatedByEmployee.FullName : "",
                LastUpdated = a.LastUpdated,
                DocumentCount = a.GraduationDocuments.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AlumniRecordDto?> GetAlumniRecordByStudentIdAsync(Guid studentId, Guid tenantId, Guid schoolId)
    {
        return await _context.AlumniRecords
            .Where(a => a.StudentId == studentId && a.TenantId == tenantId && a.SchoolId == schoolId)
            .Include(a => a.Student)
            .Include(a => a.GraduationDocuments)
            .Select(a => new AlumniRecordDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FullName,
                StudentNameArabic = a.Student.FullNameArabic,
                StudentNumber = a.Student.StudentNumber ?? "",
                GraduationDate = a.GraduationDate,
                GraduationYear = a.GraduationYear,
                FinalGPA = a.FinalGPA,
                GradeLevel = a.GradeLevel,
                GradeLevelArabic = a.GradeLevelArabic,
                Section = a.Section,
                SectionArabic = a.SectionArabic,
                University = a.University,
                UniversityArabic = a.UniversityArabic,
                Major = a.Major,
                MajorArabic = a.MajorArabic,
                UniversityEnrollmentDate = a.UniversityEnrollmentDate,
                UniversityGraduationDate = a.UniversityGraduationDate,
                EmploymentStatus = a.EmploymentStatus,
                EmploymentStatusArabic = a.EmploymentStatusArabic,
                CompanyName = a.CompanyName,
                CompanyNameArabic = a.CompanyNameArabic,
                JobTitle = a.JobTitle,
                JobTitleArabic = a.JobTitleArabic,
                Industry = a.Industry,
                IndustryArabic = a.IndustryArabic,
                EmploymentStartDate = a.EmploymentStartDate,
                PersonalEmail = a.PersonalEmail,
                PersonalPhone = a.PersonalPhone,
                LinkedInProfile = a.LinkedInProfile,
                Website = a.Website,
                CurrentAddress = a.CurrentAddress,
                CurrentAddressArabic = a.CurrentAddressArabic,
                Achievements = a.Achievements,
                AchievementsArabic = a.AchievementsArabic,
                Notes = a.Notes,
                NotesArabic = a.NotesArabic,
                IsActive = a.IsActive,
                IsVerified = a.IsVerified,
                LastContactDate = a.LastContactDate,
                CreatedAt = a.CreatedAt,
                CreatedByEmployeeName = a.CreatedByEmployee != null ? a.CreatedByEmployee.FullName : "",
                LastUpdated = a.LastUpdated,
                DocumentCount = a.GraduationDocuments.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AlumniRecordDto> CreateAlumniRecordAsync(CreateAlumniRecordDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == dto.StudentId && s.SchoolId == schoolId);

        if (student == null)
            throw new Exception("Student not found");

        var alumniRecord = new AlumniRecord
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            GraduationDate = dto.GraduationDate,
            GraduationYear = dto.GraduationYear,
            FinalGPA = dto.FinalGPA,
            GradeLevel = dto.GradeLevel,
            GradeLevelArabic = dto.GradeLevelArabic,
            Section = dto.Section,
            SectionArabic = dto.SectionArabic,
            University = dto.University,
            UniversityArabic = dto.UniversityArabic,
            Major = dto.Major,
            MajorArabic = dto.MajorArabic,
            UniversityEnrollmentDate = dto.UniversityEnrollmentDate,
            UniversityGraduationDate = dto.UniversityGraduationDate,
            EmploymentStatus = dto.EmploymentStatus,
            EmploymentStatusArabic = dto.EmploymentStatusArabic,
            CompanyName = dto.CompanyName,
            CompanyNameArabic = dto.CompanyNameArabic,
            JobTitle = dto.JobTitle,
            JobTitleArabic = dto.JobTitleArabic,
            Industry = dto.Industry,
            IndustryArabic = dto.IndustryArabic,
            EmploymentStartDate = dto.EmploymentStartDate,
            PersonalEmail = dto.PersonalEmail,
            PersonalPhone = dto.PersonalPhone,
            LinkedInProfile = dto.LinkedInProfile,
            Website = dto.Website,
            CurrentAddress = dto.CurrentAddress,
            CurrentAddressArabic = dto.CurrentAddressArabic,
            Achievements = dto.Achievements,
            AchievementsArabic = dto.AchievementsArabic,
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic,
            IsActive = true,
            IsVerified = false,
            TenantId = tenantId,
            SchoolId = schoolId,
            CreatedByEmployeeId = createdByEmployeeId,
            CreatedAt = DateTime.UtcNow
        };

        _context.AlumniRecords.Add(alumniRecord);
        
        // Update student graduation status
        student.IsGraduated = true;
        student.GraduationYear = dto.GraduationYear;
        student.FinalGPA = dto.FinalGPA;
        student.GraduationDate = dto.GraduationDate;
        student.IsActive = false; // Deactivate student record after graduation

        await _context.SaveChangesAsync();

        return await GetAlumniRecordByIdAsync(alumniRecord.Id, tenantId, schoolId);
    }

    public async Task<AlumniRecordDto> UpdateAlumniRecordAsync(UpdateAlumniRecordDto dto, Guid tenantId, Guid schoolId)
    {
        var alumniRecord = await _context.AlumniRecords
            .FirstOrDefaultAsync(a => a.Id == dto.Id && a.TenantId == tenantId && a.SchoolId == schoolId);

        if (alumniRecord == null)
            throw new Exception("Alumni record not found");

        alumniRecord.University = dto.University;
        alumniRecord.UniversityArabic = dto.UniversityArabic;
        alumniRecord.Major = dto.Major;
        alumniRecord.MajorArabic = dto.MajorArabic;
        alumniRecord.UniversityEnrollmentDate = dto.UniversityEnrollmentDate;
        alumniRecord.UniversityGraduationDate = dto.UniversityGraduationDate;
        alumniRecord.EmploymentStatus = dto.EmploymentStatus;
        alumniRecord.EmploymentStatusArabic = dto.EmploymentStatusArabic;
        alumniRecord.CompanyName = dto.CompanyName;
        alumniRecord.CompanyNameArabic = dto.CompanyNameArabic;
        alumniRecord.JobTitle = dto.JobTitle;
        alumniRecord.JobTitleArabic = dto.JobTitleArabic;
        alumniRecord.Industry = dto.Industry;
        alumniRecord.IndustryArabic = dto.IndustryArabic;
        alumniRecord.EmploymentStartDate = dto.EmploymentStartDate;
        alumniRecord.PersonalEmail = dto.PersonalEmail;
        alumniRecord.PersonalPhone = dto.PersonalPhone;
        alumniRecord.LinkedInProfile = dto.LinkedInProfile;
        alumniRecord.Website = dto.Website;
        alumniRecord.CurrentAddress = dto.CurrentAddress;
        alumniRecord.CurrentAddressArabic = dto.CurrentAddressArabic;
        alumniRecord.Achievements = dto.Achievements;
        alumniRecord.AchievementsArabic = dto.AchievementsArabic;
        alumniRecord.Notes = dto.Notes;
        alumniRecord.NotesArabic = dto.NotesArabic;
        alumniRecord.IsActive = dto.IsActive;
        alumniRecord.IsVerified = dto.IsVerified;
        alumniRecord.LastContactDate = dto.LastContactDate;
        alumniRecord.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetAlumniRecordByIdAsync(alumniRecord.Id, tenantId, schoolId);
    }

    public async Task<bool> DeleteAlumniRecordAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        var alumniRecord = await _context.AlumniRecords
            .FirstOrDefaultAsync(a => a.Id == id && a.TenantId == tenantId && a.SchoolId == schoolId);

        if (alumniRecord == null)
            return false;

        alumniRecord.IsActive = false;
        alumniRecord.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<AlumniRecordDto>> SearchAlumniRecordsAsync(string searchTerm, Guid tenantId, Guid schoolId)
    {
        return await _context.AlumniRecords
            .Where(a => a.TenantId == tenantId && a.SchoolId == schoolId &&
                       (a.Student.FullName.Contains(searchTerm) ||
                        a.Student.FullNameArabic.Contains(searchTerm) ||
                        a.Student.StudentNumber.Contains(searchTerm) ||
                        a.University.Contains(searchTerm) ||
                        a.CompanyName.Contains(searchTerm)))
            .Include(a => a.Student)
            .Select(a => new AlumniRecordDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FullName,
                StudentNameArabic = a.Student.FullNameArabic,
                StudentNumber = a.Student.StudentNumber ?? "",
                GraduationDate = a.GraduationDate,
                GraduationYear = a.GraduationYear,
                FinalGPA = a.FinalGPA,
                GradeLevel = a.GradeLevel,
                GradeLevelArabic = a.GradeLevelArabic,
                Section = a.Section,
                SectionArabic = a.SectionArabic,
                University = a.University,
                UniversityArabic = a.UniversityArabic,
                Major = a.Major,
                MajorArabic = a.MajorArabic,
                UniversityEnrollmentDate = a.UniversityEnrollmentDate,
                UniversityGraduationDate = a.UniversityGraduationDate,
                EmploymentStatus = a.EmploymentStatus,
                EmploymentStatusArabic = a.EmploymentStatusArabic,
                CompanyName = a.CompanyName,
                CompanyNameArabic = a.CompanyNameArabic,
                JobTitle = a.JobTitle,
                JobTitleArabic = a.JobTitleArabic,
                Industry = a.Industry,
                IndustryArabic = a.IndustryArabic,
                EmploymentStartDate = a.EmploymentStartDate,
                PersonalEmail = a.PersonalEmail,
                PersonalPhone = a.PersonalPhone,
                LinkedInProfile = a.LinkedInProfile,
                Website = a.Website,
                CurrentAddress = a.CurrentAddress,
                CurrentAddressArabic = a.CurrentAddressArabic,
                Achievements = a.Achievements,
                AchievementsArabic = a.AchievementsArabic,
                Notes = a.Notes,
                NotesArabic = a.NotesArabic,
                IsActive = a.IsActive,
                IsVerified = a.IsVerified,
                LastContactDate = a.LastContactDate,
                CreatedAt = a.CreatedAt,
                CreatedByEmployeeName = a.CreatedByEmployee != null ? a.CreatedByEmployee.FullName : "",
                LastUpdated = a.LastUpdated,
                DocumentCount = a.GraduationDocuments.Count
            })
            .OrderByDescending(a => a.GraduationDate)
            .ToListAsync();
    }

    public async Task<List<GraduationDocumentDto>> GetAlumniDocumentsAsync(Guid alumniRecordId, Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationDocuments
            .Where(g => g.AlumniRecordId == alumniRecordId && g.TenantId == tenantId && g.SchoolId == schoolId)
            .Include(g => g.AlumniRecord)
            .ThenInclude(a => a.Student)
            .Select(g => new GraduationDocumentDto
            {
                Id = g.Id,
                AlumniRecordId = g.AlumniRecordId,
                StudentName = g.AlumniRecord.Student.FullName,
                DocumentType = g.DocumentType,
                DocumentTypeArabic = g.DocumentTypeArabic,
                DocumentNumber = g.DocumentNumber,
                IssueDate = g.IssueDate,
                ExpiryDate = g.ExpiryDate,
                Description = g.Description,
                DescriptionArabic = g.DescriptionArabic,
                FilePath = g.FilePath,
                FileName = g.FileName,
                FileType = g.FileType,
                FileSize = g.FileSize,
                IsIssued = g.IsIssued,
                IsDelivered = g.IsDelivered,
                DeliveryDate = g.DeliveryDate,
                DeliveryMethod = g.DeliveryMethod,
                ReceivedBy = g.ReceivedBy,
                ReceivedByArabic = g.ReceivedByArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : ""
            })
            .OrderByDescending(g => g.IssueDate)
            .ToListAsync();
    }

    public async Task<GraduationDocumentDto?> GetGraduationDocumentByIdAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationDocuments
            .Where(g => g.Id == id && g.TenantId == tenantId && g.SchoolId == schoolId)
            .Include(g => g.AlumniRecord)
            .ThenInclude(a => a.Student)
            .Select(g => new GraduationDocumentDto
            {
                Id = g.Id,
                AlumniRecordId = g.AlumniRecordId,
                StudentName = g.AlumniRecord.Student.FullName,
                DocumentType = g.DocumentType,
                DocumentTypeArabic = g.DocumentTypeArabic,
                DocumentNumber = g.DocumentNumber,
                IssueDate = g.IssueDate,
                ExpiryDate = g.ExpiryDate,
                Description = g.Description,
                DescriptionArabic = g.DescriptionArabic,
                FilePath = g.FilePath,
                FileName = g.FileName,
                FileType = g.FileType,
                FileSize = g.FileSize,
                IsIssued = g.IsIssued,
                IsDelivered = g.IsDelivered,
                DeliveryDate = g.DeliveryDate,
                DeliveryMethod = g.DeliveryMethod,
                ReceivedBy = g.ReceivedBy,
                ReceivedByArabic = g.ReceivedByArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : ""
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GraduationDocumentDto> CreateGraduationDocumentAsync(CreateGraduationDocumentDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId)
    {
        var graduationDocument = new GraduationDocument
        {
            Id = Guid.NewGuid(),
            AlumniRecordId = dto.AlumniRecordId,
            DocumentType = dto.DocumentType,
            DocumentTypeArabic = dto.DocumentTypeArabic,
            DocumentNumber = dto.DocumentNumber,
            IssueDate = dto.IssueDate,
            ExpiryDate = dto.ExpiryDate,
            Description = dto.Description,
            DescriptionArabic = dto.DescriptionArabic,
            FilePath = dto.FilePath,
            FileName = dto.FileName,
            FileType = dto.FileType,
            FileSize = dto.FileSize,
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic,
            TenantId = tenantId,
            SchoolId = schoolId,
            CreatedByEmployeeId = createdByEmployeeId,
            CreatedAt = DateTime.UtcNow
        };

        _context.GraduationDocuments.Add(graduationDocument);
        await _context.SaveChangesAsync();

        return await GetGraduationDocumentByIdAsync(graduationDocument.Id, tenantId, schoolId);
    }

    public async Task<GraduationDocumentDto> UpdateGraduationDocumentAsync(UpdateGraduationDocumentDto dto, Guid tenantId, Guid schoolId)
    {
        var graduationDocument = await _context.GraduationDocuments
            .FirstOrDefaultAsync(g => g.Id == dto.Id && g.TenantId == tenantId && g.SchoolId == schoolId);

        if (graduationDocument == null)
            throw new Exception("Graduation document not found");

        graduationDocument.IsIssued = dto.IsIssued;
        graduationDocument.IsDelivered = dto.IsDelivered;
        graduationDocument.DeliveryDate = dto.DeliveryDate;
        graduationDocument.DeliveryMethod = dto.DeliveryMethod;
        graduationDocument.ReceivedBy = dto.ReceivedBy;
        graduationDocument.ReceivedByArabic = dto.ReceivedByArabic;
        graduationDocument.FinancialCleared = dto.FinancialCleared;
        graduationDocument.FinancialClearanceDate = dto.FinancialClearanceDate;
        graduationDocument.FinancialClearanceNotes = dto.FinancialClearanceNotes;
        graduationDocument.AdministrativeCleared = dto.AdministrativeCleared;
        graduationDocument.AdministrativeClearanceDate = dto.AdministrativeClearanceDate;
        graduationDocument.AdministrativeClearanceNotes = dto.AdministrativeClearanceNotes;
        graduationDocument.Notes = dto.Notes;
        graduationDocument.NotesArabic = dto.NotesArabic;
        graduationDocument.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetGraduationDocumentByIdAsync(graduationDocument.Id, tenantId, schoolId);
    }

    public async Task<bool> DeleteGraduationDocumentAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        var graduationDocument = await _context.GraduationDocuments
            .FirstOrDefaultAsync(g => g.Id == id && g.TenantId == tenantId && g.SchoolId == schoolId);

        if (graduationDocument == null)
            return false;

        _context.GraduationDocuments.Remove(graduationDocument);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<GraduationClearanceRecordDto>> GetAllClearanceRecordsAsync(Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationClearanceRecords
            .Where(g => g.TenantId == tenantId && g.SchoolId == schoolId)
            .Include(g => g.Student)
            .Select(g => new GraduationClearanceRecordDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = g.Student.FullName,
                StudentNumber = g.Student.StudentNumber ?? "",
                GraduationYear = g.GraduationYear,
                GradeLevel = g.GradeLevel,
                GradeLevelArabic = g.GradeLevelArabic,
                ClearanceStatus = g.ClearanceStatus,
                ClearanceStatusArabic = g.ClearanceStatusArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                FinancialClearanceNotesArabic = g.FinancialClearanceNotesArabic,
                OutstandingBalance = g.OutstandingBalance,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                AdministrativeClearanceNotesArabic = g.AdministrativeClearanceNotesArabic,
                ReturnedLibraryBooks = g.ReturnedLibraryBooks,
                ReturnedEquipment = g.ReturnedEquipment,
                AcademicCleared = g.AcademicCleared,
                AcademicClearanceDate = g.AcademicClearanceDate,
                AcademicClearanceNotes = g.AcademicClearanceNotes,
                AcademicClearanceNotesArabic = g.AcademicClearanceNotesArabic,
                AllGradesRecorded = g.AllGradesRecorded,
                AllRequirementsMet = g.AllRequirementsMet,
                ClinicCleared = g.ClinicCleared,
                ClinicClearanceDate = g.ClinicClearanceDate,
                ClinicClearanceNotes = g.ClinicClearanceNotes,
                ClinicClearanceNotesArabic = g.ClinicClearanceNotesArabic,
                FinalApproval = g.FinalApproval,
                FinalApprovalDate = g.FinalApprovalDate,
                ApprovedByEmployeeName = g.ApprovedByEmployeeName,
                FinalApprovalNotes = g.FinalApprovalNotes,
                FinalApprovalNotesArabic = g.FinalApprovalNotesArabic,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : "",
                LastUpdated = g.LastUpdated
            })
            .OrderByDescending(g => g.GraduationYear)
            .ToListAsync();
    }

    public async Task<GraduationClearanceRecordDto?> GetClearanceRecordByIdAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationClearanceRecords
            .Where(g => g.Id == id && g.TenantId == tenantId && g.SchoolId == schoolId)
            .Include(g => g.Student)
            .Select(g => new GraduationClearanceRecordDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = g.Student.FullName,
                StudentNumber = g.Student.StudentNumber ?? "",
                GraduationYear = g.GraduationYear,
                GradeLevel = g.GradeLevel,
                GradeLevelArabic = g.GradeLevelArabic,
                ClearanceStatus = g.ClearanceStatus,
                ClearanceStatusArabic = g.ClearanceStatusArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                FinancialClearanceNotesArabic = g.FinancialClearanceNotesArabic,
                OutstandingBalance = g.OutstandingBalance,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                AdministrativeClearanceNotesArabic = g.AdministrativeClearanceNotesArabic,
                ReturnedLibraryBooks = g.ReturnedLibraryBooks,
                ReturnedEquipment = g.ReturnedEquipment,
                AcademicCleared = g.AcademicCleared,
                AcademicClearanceDate = g.AcademicClearanceDate,
                AcademicClearanceNotes = g.AcademicClearanceNotes,
                AcademicClearanceNotesArabic = g.AcademicClearanceNotesArabic,
                AllGradesRecorded = g.AllGradesRecorded,
                AllRequirementsMet = g.AllRequirementsMet,
                ClinicCleared = g.ClinicCleared,
                ClinicClearanceDate = g.ClinicClearanceDate,
                ClinicClearanceNotes = g.ClinicClearanceNotes,
                ClinicClearanceNotesArabic = g.ClinicClearanceNotesArabic,
                FinalApproval = g.FinalApproval,
                FinalApprovalDate = g.FinalApprovalDate,
                ApprovedByEmployeeName = g.ApprovedByEmployeeName,
                FinalApprovalNotes = g.FinalApprovalNotes,
                FinalApprovalNotesArabic = g.FinalApprovalNotesArabic,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : "",
                LastUpdated = g.LastUpdated
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GraduationClearanceRecordDto?> GetClearanceRecordByStudentIdAsync(Guid studentId, Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationClearanceRecords
            .Where(g => g.StudentId == studentId && g.TenantId == tenantId && g.SchoolId == schoolId)
            .Include(g => g.Student)
            .Select(g => new GraduationClearanceRecordDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = g.Student.FullName,
                StudentNumber = g.Student.StudentNumber ?? "",
                GraduationYear = g.GraduationYear,
                GradeLevel = g.GradeLevel,
                GradeLevelArabic = g.GradeLevelArabic,
                ClearanceStatus = g.ClearanceStatus,
                ClearanceStatusArabic = g.ClearanceStatusArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                FinancialClearanceNotesArabic = g.FinancialClearanceNotesArabic,
                OutstandingBalance = g.OutstandingBalance,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                AdministrativeClearanceNotesArabic = g.AdministrativeClearanceNotesArabic,
                ReturnedLibraryBooks = g.ReturnedLibraryBooks,
                ReturnedEquipment = g.ReturnedEquipment,
                AcademicCleared = g.AcademicCleared,
                AcademicClearanceDate = g.AcademicClearanceDate,
                AcademicClearanceNotes = g.AcademicClearanceNotes,
                AcademicClearanceNotesArabic = g.AcademicClearanceNotesArabic,
                AllGradesRecorded = g.AllGradesRecorded,
                AllRequirementsMet = g.AllRequirementsMet,
                ClinicCleared = g.ClinicCleared,
                ClinicClearanceDate = g.ClinicClearanceDate,
                ClinicClearanceNotes = g.ClinicClearanceNotes,
                ClinicClearanceNotesArabic = g.ClinicClearanceNotesArabic,
                FinalApproval = g.FinalApproval,
                FinalApprovalDate = g.FinalApprovalDate,
                ApprovedByEmployeeName = g.ApprovedByEmployeeName,
                FinalApprovalNotes = g.FinalApprovalNotes,
                FinalApprovalNotesArabic = g.FinalApprovalNotesArabic,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : "",
                LastUpdated = g.LastUpdated
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GraduationClearanceRecordDto> CreateClearanceRecordAsync(CreateGraduationClearanceRecordDto dto, Guid tenantId, Guid schoolId, Guid createdByEmployeeId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == dto.StudentId && s.SchoolId == schoolId);

        if (student == null)
            throw new Exception("Student not found");

        var clearanceRecord = new GraduationClearanceRecord
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            GraduationYear = dto.GraduationYear,
            GradeLevel = dto.GradeLevel,
            GradeLevelArabic = dto.GradeLevelArabic,
            ClearanceStatus = "Pending",
            ClearanceStatusArabic = "قيد الانتظار",
            Notes = dto.Notes,
            NotesArabic = dto.NotesArabic,
            TenantId = tenantId,
            SchoolId = schoolId,
            CreatedByEmployeeId = createdByEmployeeId,
            CreatedAt = DateTime.UtcNow
        };

        _context.GraduationClearanceRecords.Add(clearanceRecord);
        await _context.SaveChangesAsync();

        return await GetClearanceRecordByIdAsync(clearanceRecord.Id, tenantId, schoolId);
    }

    public async Task<GraduationClearanceRecordDto> UpdateClearanceRecordAsync(UpdateGraduationClearanceRecordDto dto, Guid tenantId, Guid schoolId, Guid approvedByEmployeeId)
    {
        var clearanceRecord = await _context.GraduationClearanceRecords
            .FirstOrDefaultAsync(g => g.Id == dto.Id && g.TenantId == tenantId && g.SchoolId == schoolId);

        if (clearanceRecord == null)
            throw new Exception("Clearance record not found");

        clearanceRecord.ClearanceStatus = dto.ClearanceStatus;
        clearanceRecord.ClearanceStatusArabic = dto.ClearanceStatusArabic;
        clearanceRecord.FinancialCleared = dto.FinancialCleared;
        clearanceRecord.FinancialClearanceDate = dto.FinancialClearanceDate;
        clearanceRecord.FinancialClearanceNotes = dto.FinancialClearanceNotes;
        clearanceRecord.FinancialClearanceNotesArabic = dto.FinancialClearanceNotesArabic;
        clearanceRecord.OutstandingBalance = dto.OutstandingBalance;
        clearanceRecord.AdministrativeCleared = dto.AdministrativeCleared;
        clearanceRecord.AdministrativeClearanceDate = dto.AdministrativeClearanceDate;
        clearanceRecord.AdministrativeClearanceNotes = dto.AdministrativeClearanceNotes;
        clearanceRecord.AdministrativeClearanceNotesArabic = dto.AdministrativeClearanceNotesArabic;
        clearanceRecord.ReturnedLibraryBooks = dto.ReturnedLibraryBooks;
        clearanceRecord.ReturnedEquipment = dto.ReturnedEquipment;
        clearanceRecord.AcademicCleared = dto.AcademicCleared;
        clearanceRecord.AcademicClearanceDate = dto.AcademicClearanceDate;
        clearanceRecord.AcademicClearanceNotes = dto.AcademicClearanceNotes;
        clearanceRecord.AcademicClearanceNotesArabic = dto.AcademicClearanceNotesArabic;
        clearanceRecord.AllGradesRecorded = dto.AllGradesRecorded;
        clearanceRecord.AllRequirementsMet = dto.AllRequirementsMet;
        clearanceRecord.ClinicCleared = dto.ClinicCleared;
        clearanceRecord.ClinicClearanceDate = dto.ClinicClearanceDate;
        clearanceRecord.ClinicClearanceNotes = dto.ClinicClearanceNotes;
        clearanceRecord.ClinicClearanceNotesArabic = dto.ClinicClearanceNotesArabic;
        clearanceRecord.FinalApproval = dto.FinalApproval;
        clearanceRecord.FinalApprovalDate = dto.FinalApprovalDate;
        clearanceRecord.ApprovedByEmployeeId = approvedByEmployeeId;
        clearanceRecord.FinalApprovalNotes = dto.FinalApprovalNotes;
        clearanceRecord.FinalApprovalNotesArabic = dto.FinalApprovalNotesArabic;
        clearanceRecord.Notes = dto.Notes;
        clearanceRecord.NotesArabic = dto.NotesArabic;
        clearanceRecord.LastUpdated = DateTime.UtcNow;

        if (dto.FinalApproval)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == approvedByEmployeeId);
            clearanceRecord.ApprovedByEmployeeName = employee?.FullName;
        }

        await _context.SaveChangesAsync();

        return await GetClearanceRecordByIdAsync(clearanceRecord.Id, tenantId, schoolId);
    }

    public async Task<bool> DeleteClearanceRecordAsync(Guid id, Guid tenantId, Guid schoolId)
    {
        var clearanceRecord = await _context.GraduationClearanceRecords
            .FirstOrDefaultAsync(g => g.Id == id && g.TenantId == tenantId && g.SchoolId == schoolId);

        if (clearanceRecord == null)
            return false;

        _context.GraduationClearanceRecords.Remove(clearanceRecord);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<GraduationClearanceRecordDto>> GetPendingClearanceRecordsAsync(Guid tenantId, Guid schoolId)
    {
        return await _context.GraduationClearanceRecords
            .Where(g => g.TenantId == tenantId && g.SchoolId == schoolId && g.ClearanceStatus == "Pending")
            .Include(g => g.Student)
            .Select(g => new GraduationClearanceRecordDto
            {
                Id = g.Id,
                StudentId = g.StudentId,
                StudentName = g.Student.FullName,
                StudentNumber = g.Student.StudentNumber ?? "",
                GraduationYear = g.GraduationYear,
                GradeLevel = g.GradeLevel,
                GradeLevelArabic = g.GradeLevelArabic,
                ClearanceStatus = g.ClearanceStatus,
                ClearanceStatusArabic = g.ClearanceStatusArabic,
                FinancialCleared = g.FinancialCleared,
                FinancialClearanceDate = g.FinancialClearanceDate,
                FinancialClearanceNotes = g.FinancialClearanceNotes,
                FinancialClearanceNotesArabic = g.FinancialClearanceNotesArabic,
                OutstandingBalance = g.OutstandingBalance,
                AdministrativeCleared = g.AdministrativeCleared,
                AdministrativeClearanceDate = g.AdministrativeClearanceDate,
                AdministrativeClearanceNotes = g.AdministrativeClearanceNotes,
                AdministrativeClearanceNotesArabic = g.AdministrativeClearanceNotesArabic,
                ReturnedLibraryBooks = g.ReturnedLibraryBooks,
                ReturnedEquipment = g.ReturnedEquipment,
                AcademicCleared = g.AcademicCleared,
                AcademicClearanceDate = g.AcademicClearanceDate,
                AcademicClearanceNotes = g.AcademicClearanceNotes,
                AcademicClearanceNotesArabic = g.AcademicClearanceNotesArabic,
                AllGradesRecorded = g.AllGradesRecorded,
                AllRequirementsMet = g.AllRequirementsMet,
                ClinicCleared = g.ClinicCleared,
                ClinicClearanceDate = g.ClinicClearanceDate,
                ClinicClearanceNotes = g.ClinicClearanceNotes,
                ClinicClearanceNotesArabic = g.ClinicClearanceNotesArabic,
                FinalApproval = g.FinalApproval,
                FinalApprovalDate = g.FinalApprovalDate,
                ApprovedByEmployeeName = g.ApprovedByEmployeeName,
                FinalApprovalNotes = g.FinalApprovalNotes,
                FinalApprovalNotesArabic = g.FinalApprovalNotesArabic,
                Notes = g.Notes,
                NotesArabic = g.NotesArabic,
                CreatedAt = g.CreatedAt,
                CreatedByEmployeeName = g.CreatedByEmployee != null ? g.CreatedByEmployee.FullName : "",
                LastUpdated = g.LastUpdated
            })
            .OrderBy(g => g.GraduationYear)
            .ToListAsync();
    }

    public async Task<bool> GraduateStudentAsync(Guid studentId, int graduationYear, decimal? finalGPA, Guid tenantId, Guid schoolId, Guid processedByEmployeeId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolId == schoolId);

        if (student == null)
            return false;

        student.IsGraduated = true;
        student.GraduationYear = graduationYear;
        student.FinalGPA = finalGPA;
        student.GraduationDate = DateTime.UtcNow;
        student.IsActive = false;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckFinancialClearanceAsync(Guid studentId, Guid tenantId, Guid schoolId)
    {
        // Check if there are any outstanding balances using StudentInvoices through StudentAccount
        var student = await _context.Students
            .Include(s => s.StudentAccount)
            .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolId == schoolId);

        if (student == null || student.StudentAccount == null)
            return false;

        var hasOutstandingBalance = await _context.StudentInvoices
            .Where(i => i.StudentAccountId == student.StudentAccount.AccountId && i.OutstandingAmount > 0)
            .AnyAsync();

        return !hasOutstandingBalance;
    }

    public async Task<bool> ProcessGraduationWorkflowAsync(Guid studentId, int graduationYear, decimal? finalGPA, Guid tenantId, Guid schoolId, Guid processedByEmployeeId)
    {
        // Step 1: Check financial clearance
        var financiallyCleared = await CheckFinancialClearanceAsync(studentId, tenantId, schoolId);
        
        if (!financiallyCleared)
            return false;

        // Step 2: Graduate the student
        var graduated = await GraduateStudentAsync(studentId, graduationYear, finalGPA, tenantId, schoolId, processedByEmployeeId);
        
        if (!graduated)
            return false;

        // Step 3: Create alumni record
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId && s.SchoolId == schoolId);

        if (student == null)
            return false;

        var alumniRecord = new AlumniRecord
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            GraduationDate = DateTime.UtcNow,
            GraduationYear = graduationYear,
            FinalGPA = finalGPA,
            GradeLevel = student.ClassRoom?.GradeLevelId.ToString(),
            GradeLevelArabic = student.ClassRoom?.GradeLevelId.ToString(),
            Section = student.ClassRoom?.SectionId.ToString(),
            SectionArabic = student.ClassRoom?.SectionId.ToString(),
            IsActive = true,
            IsVerified = false,
            TenantId = tenantId,
            SchoolId = schoolId,
            CreatedByEmployeeId = processedByEmployeeId,
            CreatedAt = DateTime.UtcNow
        };

        _context.AlumniRecords.Add(alumniRecord);
        await _context.SaveChangesAsync();

        // Step 4: Create graduation documents
        var certificateDocument = new GraduationDocument
        {
            Id = Guid.NewGuid(),
            AlumniRecordId = alumniRecord.Id,
            DocumentType = "Certificate",
            DocumentTypeArabic = "شهادة التخرج",
            DocumentNumber = $"CERT-{graduationYear}-{student.StudentNumber}",
            IssueDate = DateTime.UtcNow,
            IsIssued = false,
            IsDelivered = false,
            FinancialCleared = financiallyCleared,
            FinancialClearanceDate = DateTime.UtcNow,
            AdministrativeCleared = false,
            TenantId = tenantId,
            SchoolId = schoolId,
            CreatedByEmployeeId = processedByEmployeeId,
            CreatedAt = DateTime.UtcNow
        };

        _context.GraduationDocuments.Add(certificateDocument);
        await _context.SaveChangesAsync();

        return true;
    }
}