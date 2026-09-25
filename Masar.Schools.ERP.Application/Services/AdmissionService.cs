using Masar.Schools.ERP.Application.Interfaces;
using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Domain.Entities.Admissions;
using Masar.Schools.ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Masar.Schools.ERP.Application.Services;

/// <summary>
/// خدمة القبول والتسجيل
/// </summary>
public class AdmissionService : IAdmissionService
{
    private readonly MasarDbContext _context;

    public AdmissionService(MasarDbContext context)
    {
        _context = context;
    }

    // ==================== Admission Applications ====================
    public async Task<AdmissionResponseDto<AdmissionApplicationDto>> CreateAdmissionApplicationAsync(CreateAdmissionApplicationDto dto)
    {
        // Generate application number
        var applicationNumber = await GenerateApplicationNumberAsync(dto.SchoolId);

        var application = new AdmissionApplication
        {
            Id = Guid.NewGuid(),
            SchoolId = dto.SchoolId,
            ApplicationNumber = applicationNumber,
            ApplicationType = dto.ApplicationType,
            Status = AdmissionStatus.New,
            AcademicYear = dto.AcademicYear,
            GradeLevelId = dto.GradeLevelId,
            SectionId = dto.SectionId,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            FullNameArabic = dto.FullNameArabic,
            FullNameEnglish = dto.FullNameEnglish,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Nationality = dto.Nationality,
            NationalId = dto.NationalId,
            PassportNumber = dto.PassportNumber,
            Religion = dto.Religion,
            CivilId = dto.CivilId,
            IsSaudiCitizen = dto.IsSaudiCitizen,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            HasSpecialNeeds = dto.HasSpecialNeeds,
            SpecialNeedsDescription = dto.SpecialNeedsDescription,
            IsGifted = dto.IsGifted,
            GiftedDescription = dto.GiftedDescription,
            GuardianFirstName = dto.GuardianFirstName,
            GuardianLastName = dto.GuardianLastName,
            GuardianFullNameArabic = dto.GuardianFullNameArabic,
            Relationship = dto.Relationship,
            RelationshipArabic = dto.RelationshipArabic,
            GuardianNationalId = dto.GuardianNationalId,
            GuardianPhoneNumber = dto.GuardianPhoneNumber,
            GuardianWhatsAppNumber = dto.GuardianWhatsAppNumber,
            GuardianEmail = dto.GuardianEmail,
            GuardianOccupation = dto.GuardianOccupation,
            GuardianOccupationArabic = dto.GuardianOccupationArabic,
            GuardianWorkplace = dto.GuardianWorkplace,
            GuardianWorkAddress = dto.GuardianWorkAddress,
            GuardianAddress = dto.GuardianAddress,
            PreviousSchool = dto.PreviousSchool,
            PreviousSchoolArabic = dto.PreviousSchoolArabic,
            PreviousSchoolCity = dto.PreviousSchoolCity,
            TransferReason = dto.TransferReason,
            TransferReasonArabic = dto.TransferReasonArabic,
            Notes = dto.Notes,
            Priority = dto.Priority,
            ApplicationDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.AdmissionApplications.Add(application);
        await _context.SaveChangesAsync();

        var applicationDto = new AdmissionApplicationDto
        {
            Id = application.Id,
            ApplicationNumber = application.ApplicationNumber,
            ApplicationType = application.ApplicationType,
            Status = application.Status,
            AcademicYear = application.AcademicYear,
            FirstName = application.FirstName,
            MiddleName = application.MiddleName,
            LastName = application.LastName,
            FullNameArabic = application.FullNameArabic,
            FullNameEnglish = application.FullNameEnglish,
            BirthDate = application.BirthDate,
            Gender = application.Gender,
            Nationality = application.Nationality,
            NationalId = application.NationalId,
            GuardianFullNameArabic = application.GuardianFullNameArabic,
            GuardianPhoneNumber = application.GuardianPhoneNumber,
            GuardianWhatsAppNumber = application.GuardianWhatsAppNumber,
            GuardianEmail = application.GuardianEmail,
            ApplicationDate = application.ApplicationDate,
            Priority = application.Priority
        };

        return new AdmissionResponseDto<AdmissionApplicationDto>
        {
            Success = true,
            Message = "تم إنشاء طلب الالتحاق بنجاح",
            Data = applicationDto
        };
    }

    public async Task<AdmissionResponseDto<AdmissionApplicationDto>> UpdateAdmissionApplicationAsync(UpdateAdmissionApplicationDto dto)
    {
        var application = await _context.AdmissionApplications.FindAsync(dto.Id);
        if (application == null)
        {
            return new AdmissionResponseDto<AdmissionApplicationDto>
            {
                Success = false,
                Message = "طلب الالتحاق غير موجود"
            };
        }

        application.ApplicationType = dto.ApplicationType;
        application.Status = dto.Status;
        application.AcademicYear = dto.AcademicYear;
        application.GradeLevelId = dto.GradeLevelId;
        application.SectionId = dto.SectionId;
        application.FirstName = dto.FirstName;
        application.MiddleName = dto.MiddleName;
        application.LastName = dto.LastName;
        application.FullNameArabic = dto.FullNameArabic;
        application.FullNameEnglish = dto.FullNameEnglish;
        application.BirthDate = dto.BirthDate;
        application.Gender = dto.Gender;
        application.Nationality = dto.Nationality;
        application.NationalId = dto.NationalId;
        application.PassportNumber = dto.PassportNumber;
        application.Religion = dto.Religion;
        application.CivilId = dto.CivilId;
        application.IsSaudiCitizen = dto.IsSaudiCitizen;
        application.Address = dto.Address;
        application.City = dto.City;
        application.PostalCode = dto.PostalCode;
        application.PhoneNumber = dto.PhoneNumber;
        application.Email = dto.Email;
        application.HasSpecialNeeds = dto.HasSpecialNeeds;
        application.SpecialNeedsDescription = dto.SpecialNeedsDescription;
        application.IsGifted = dto.IsGifted;
        application.GiftedDescription = dto.GiftedDescription;
        application.GuardianFirstName = dto.GuardianFirstName;
        application.GuardianLastName = dto.GuardianLastName;
        application.GuardianFullNameArabic = dto.GuardianFullNameArabic;
        application.Relationship = dto.Relationship;
        application.RelationshipArabic = dto.RelationshipArabic;
        application.GuardianNationalId = dto.GuardianNationalId;
        application.GuardianPhoneNumber = dto.GuardianPhoneNumber;
        application.GuardianWhatsAppNumber = dto.GuardianWhatsAppNumber;
        application.GuardianEmail = dto.GuardianEmail;
        application.GuardianOccupation = dto.GuardianOccupation;
        application.GuardianOccupationArabic = dto.GuardianOccupationArabic;
        application.GuardianWorkplace = dto.GuardianWorkplace;
        application.GuardianWorkAddress = dto.GuardianWorkAddress;
        application.GuardianAddress = dto.GuardianAddress;
        application.PreviousSchool = dto.PreviousSchool;
        application.PreviousSchoolArabic = dto.PreviousSchoolArabic;
        application.PreviousSchoolCity = dto.PreviousSchoolCity;
        application.TransferReason = dto.TransferReason;
        application.TransferReasonArabic = dto.TransferReasonArabic;
        application.Notes = dto.Notes;
        application.Priority = dto.Priority;
        application.RejectionReason = dto.RejectionReason;
        application.RejectionReasonArabic = dto.RejectionReasonArabic;
        application.LastUpdated = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;
        application.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var applicationDto = new AdmissionApplicationDto
        {
            Id = application.Id,
            ApplicationNumber = application.ApplicationNumber,
            ApplicationType = application.ApplicationType,
            Status = application.Status,
            AcademicYear = application.AcademicYear,
            FirstName = application.FirstName,
            MiddleName = application.MiddleName,
            LastName = application.LastName,
            FullNameArabic = application.FullNameArabic,
            FullNameEnglish = application.FullNameEnglish,
            BirthDate = application.BirthDate,
            Gender = application.Gender,
            Nationality = application.Nationality,
            NationalId = application.NationalId,
            GuardianFullNameArabic = application.GuardianFullNameArabic,
            GuardianPhoneNumber = application.GuardianPhoneNumber,
            GuardianWhatsAppNumber = application.GuardianWhatsAppNumber,
            GuardianEmail = application.GuardianEmail,
            ApplicationDate = application.ApplicationDate,
            LastUpdated = application.LastUpdated,
            Priority = application.Priority,
            RejectionReason = application.RejectionReason,
            RejectionReasonArabic = application.RejectionReasonArabic
        };

        return new AdmissionResponseDto<AdmissionApplicationDto>
        {
            Success = true,
            Message = "تم تحديث طلب الالتحاق بنجاح",
            Data = applicationDto
        };
    }

    public async Task<AdmissionResponseDto<bool>> DeleteAdmissionApplicationAsync(Guid id)
    {
        var application = await _context.AdmissionApplications.FindAsync(id);
        if (application == null)
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "طلب الالتحاق غير موجود"
            };
        }

        if (application.Status == AdmissionStatus.Enrolled)
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "لا يمكن حذف طلب تم تحويله لطالب رسمي"
            };
        }

        _context.AdmissionApplications.Remove(application);
        await _context.SaveChangesAsync();

        return new AdmissionResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف طلب الالتحاق بنجاح",
            Data = true
        };
    }

    public async Task<AdmissionResponseDto<AdmissionApplicationDto>> GetAdmissionApplicationAsync(Guid id)
    {
        var application = await _context.AdmissionApplications
            .Include(a => a.Documents)
            .Include(a => a.Exam)
            .Include(a => a.GradeLevel)
            .Include(a => a.Section)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
        {
            return new AdmissionResponseDto<AdmissionApplicationDto>
            {
                Success = false,
                Message = "طلب الالتحاق غير موجود"
            };
        }

        var applicationDto = new AdmissionApplicationDto
        {
            Id = application.Id,
            ApplicationNumber = application.ApplicationNumber,
            ApplicationType = application.ApplicationType,
            Status = application.Status,
            AcademicYear = application.AcademicYear,
            GradeLevelName = application.GradeLevel?.NameArabic,
            SectionName = application.Section?.NameArabic,
            FirstName = application.FirstName,
            MiddleName = application.MiddleName,
            LastName = application.LastName,
            FullNameArabic = application.FullNameArabic,
            FullNameEnglish = application.FullNameEnglish,
            BirthDate = application.BirthDate,
            Gender = application.Gender,
            Nationality = application.Nationality,
            NationalId = application.NationalId,
            GuardianFullNameArabic = application.GuardianFullNameArabic,
            GuardianPhoneNumber = application.GuardianPhoneNumber,
            GuardianWhatsAppNumber = application.GuardianWhatsAppNumber,
            GuardianEmail = application.GuardianEmail,
            ApplicationDate = application.ApplicationDate,
            LastUpdated = application.LastUpdated,
            Priority = application.Priority,
            DecisionDate = application.DecisionDate,
            ConvertedStudentId = application.ConvertedStudentId,
            ConversionDate = application.ConversionDate,
            WhatsAppNotificationSent = application.WhatsAppNotificationSent,
            WhatsAppNotificationSentAt = application.WhatsAppNotificationSentAt,
            EmailNotificationSent = application.EmailNotificationSent,
            EmailNotificationSentAt = application.EmailNotificationSentAt,
            RejectionReason = application.RejectionReason,
            RejectionReasonArabic = application.RejectionReasonArabic,
            Notes = application.Notes
        };

        return new AdmissionResponseDto<AdmissionApplicationDto>
        {
            Success = true,
            Data = applicationDto
        };
    }

    public async Task<AdmissionPagedResponseDto<AdmissionApplicationDto>> GetAdmissionApplicationsAsync(AdmissionSearchDto search)
    {
        var query = _context.AdmissionApplications.AsQueryable();

        if (!string.IsNullOrEmpty(search.SearchTerm))
        {
            query = query.Where(a => a.ApplicationNumber.Contains(search.SearchTerm) ||
                                    a.FullNameArabic.Contains(search.SearchTerm) ||
                                    a.FullNameEnglish.Contains(search.SearchTerm) ||
                                    a.NationalId.Contains(search.SearchTerm));
        }

        if (search.SchoolId.HasValue)
        {
            query = query.Where(a => a.SchoolId == search.SchoolId.Value);
        }

        if (search.ApplicationType.HasValue)
        {
            query = query.Where(a => a.ApplicationType == search.ApplicationType.Value);
        }

        if (search.Status.HasValue)
        {
            query = query.Where(a => a.Status == search.Status.Value);
        }

        if (!string.IsNullOrEmpty(search.AcademicYear))
        {
            query = query.Where(a => a.AcademicYear == search.AcademicYear);
        }

        if (search.GradeLevelId.HasValue)
        {
            query = query.Where(a => a.GradeLevelId == search.GradeLevelId.Value);
        }

        if (search.StartDate.HasValue)
        {
            query = query.Where(a => a.ApplicationDate >= search.StartDate.Value);
        }

        if (search.EndDate.HasValue)
        {
            query = query.Where(a => a.ApplicationDate <= search.EndDate.Value);
        }

        var totalCount = await query.CountAsync();
        var applications = await query
            .OrderByDescending(a => a.ApplicationDate)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(a => new AdmissionApplicationDto
            {
                Id = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                ApplicationType = a.ApplicationType,
                Status = a.Status,
                AcademicYear = a.AcademicYear,
                GradeLevelName = a.GradeLevel != null ? a.GradeLevel.NameArabic : null,
                SectionName = a.Section != null ? a.Section.NameArabic : null,
                FirstName = a.FirstName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                FullNameArabic = a.FullNameArabic,
                FullNameEnglish = a.FullNameEnglish,
                BirthDate = a.BirthDate,
                Gender = a.Gender,
                Nationality = a.Nationality,
                NationalId = a.NationalId,
                GuardianFullNameArabic = a.GuardianFullNameArabic,
                GuardianPhoneNumber = a.GuardianPhoneNumber,
                GuardianWhatsAppNumber = a.GuardianWhatsAppNumber,
                GuardianEmail = a.GuardianEmail,
                ApplicationDate = a.ApplicationDate,
                LastUpdated = a.LastUpdated,
                Priority = a.Priority,
                DecisionDate = a.DecisionDate,
                ConvertedStudentId = a.ConvertedStudentId,
                ConversionDate = a.ConversionDate
            })
            .ToListAsync();

        return new AdmissionPagedResponseDto<AdmissionApplicationDto>
        {
            Success = true,
            Data = applications,
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / search.PageSize)
        };
    }

    public async Task<AdmissionResponseDto<bool>> UpdateApplicationStatusAsync(UpdateApplicationStatusDto dto)
    {
        var application = await _context.AdmissionApplications.FindAsync(dto.Id);
        if (application == null)
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "طلب الالتحاق غير موجود"
            };
        }

        // Validate status transitions
        if (!IsValidStatusTransition(application.Status, dto.Status))
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "الانتقال بين الحالات غير مسموح"
            };
        }

        application.Status = dto.Status;
        application.RejectionReason = dto.RejectionReason;
        application.RejectionReasonArabic = dto.RejectionReasonArabic;
        application.Notes = dto.Notes;
        application.LastUpdated = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;
        application.UpdatedBy = "System";

        if (dto.Status == AdmissionStatus.Accepted || dto.Status == AdmissionStatus.Rejected)
        {
            application.DecisionDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return new AdmissionResponseDto<bool>
        {
            Success = true,
            Message = "تم تحديث حالة الطلب بنجاح",
            Data = true
        };
    }

    public async Task<AdmissionResponseDto<AdmissionStatisticsDto>> GetAdmissionStatisticsAsync(Guid schoolId, string? academicYear)
    {
        var query = _context.AdmissionApplications.Where(a => a.SchoolId == schoolId);

        if (!string.IsNullOrEmpty(academicYear))
        {
            query = query.Where(a => a.AcademicYear == academicYear);
        }

        var applications = await query.ToListAsync();

        var statistics = new AdmissionStatisticsDto
        {
            TotalApplications = applications.Count,
            NewApplications = applications.Count(a => a.Status == AdmissionStatus.New),
            UnderReview = applications.Count(a => a.Status == AdmissionStatus.UnderReview),
            InterviewScheduled = applications.Count(a => a.Status == AdmissionStatus.InterviewScheduled),
            ExamScheduled = applications.Count(a => a.Status == AdmissionStatus.ExamScheduled),
            Accepted = applications.Count(a => a.Status == AdmissionStatus.Accepted),
            Rejected = applications.Count(a => a.Status == AdmissionStatus.Rejected),
            Waitlisted = applications.Count(a => a.Status == AdmissionStatus.Waitlisted),
            Withdrawn = applications.Count(a => a.Status == AdmissionStatus.Withdrawn),
            Enrolled = applications.Count(a => a.Status == AdmissionStatus.Enrolled)
        };

        // Calculate acceptance rate
        var totalDecisions = statistics.Accepted + statistics.Rejected;
        statistics.AcceptanceRate = totalDecisions > 0 ? (decimal)statistics.Accepted / totalDecisions * 100 : 0;

        // Get capacity info (this would be calculated based on school capacity settings)
        statistics.TotalCapacity = 1000; // Placeholder - should come from school settings
        statistics.AvailableSlots = statistics.TotalCapacity - statistics.Enrolled;

        return new AdmissionResponseDto<AdmissionStatisticsDto>
        {
            Success = true,
            Data = statistics
        };
    }

    // ==================== Admission Exams ====================
    public async Task<AdmissionResponseDto<AdmissionExamDto>> CreateAdmissionExamAsync(CreateAdmissionExamDto dto)
    {
        var exam = new AdmissionExam
        {
            Id = Guid.NewGuid(),
            AdmissionApplicationId = dto.AdmissionApplicationId,
            InterviewStatus = dto.InterviewDate.HasValue ? InterviewStatus.Scheduled : InterviewStatus.NotScheduled,
            InterviewDate = dto.InterviewDate,
            InterviewTime = dto.InterviewTime,
            InterviewLocation = dto.InterviewLocation,
            InterviewerId = dto.InterviewerId,
            ExamStatus = dto.ExamDate.HasValue ? ExamStatus.Scheduled : ExamStatus.NotScheduled,
            ExamDate = dto.ExamDate,
            ExamTime = dto.ExamTime,
            ExamLocation = dto.ExamLocation,
            ExaminerId = dto.ExaminerId,
            PassingScore = dto.PassingScore,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.AdmissionExams.Add(exam);
        await _context.SaveChangesAsync();

        // Update application status
        var application = await _context.AdmissionApplications.FindAsync(dto.AdmissionApplicationId);
        if (application != null)
        {
            if (dto.InterviewDate.HasValue)
            {
                application.Status = AdmissionStatus.InterviewScheduled;
            }
            else if (dto.ExamDate.HasValue)
            {
                application.Status = AdmissionStatus.ExamScheduled;
            }
            application.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        var examDto = new AdmissionExamDto
        {
            Id = exam.Id,
            AdmissionApplicationId = exam.AdmissionApplicationId,
            InterviewStatus = exam.InterviewStatus,
            InterviewDate = exam.InterviewDate,
            InterviewTime = exam.InterviewTime,
            InterviewLocation = exam.InterviewLocation,
            ExamStatus = exam.ExamStatus,
            ExamDate = exam.ExamDate,
            ExamTime = exam.ExamTime,
            ExamLocation = exam.ExamLocation,
            PassingScore = exam.PassingScore
        };

        return new AdmissionResponseDto<AdmissionExamDto>
        {
            Success = true,
            Message = "تم إنشاء جدول المقابلة والاختبار بنجاح",
            Data = examDto
        };
    }

    public async Task<AdmissionResponseDto<AdmissionExamDto>> UpdateAdmissionExamAsync(UpdateAdmissionExamDto dto)
    {
        var exam = await _context.AdmissionExams.FindAsync(dto.Id);
        if (exam == null)
        {
            return new AdmissionResponseDto<AdmissionExamDto>
            {
                Success = false,
                Message = "جدول المقابلة والاختبار غير موجود"
            };
        }

        exam.InterviewStatus = dto.InterviewStatus;
        exam.InterviewDate = dto.InterviewDate;
        exam.InterviewTime = dto.InterviewTime;
        exam.InterviewLocation = dto.InterviewLocation;
        exam.InterviewerId = dto.InterviewerId;
        exam.InterviewScore = dto.InterviewScore;
        exam.InterviewNotes = dto.InterviewNotes;
        exam.ExamStatus = dto.ExamStatus;
        exam.ExamDate = dto.ExamDate;
        exam.ExamTime = dto.ExamTime;
        exam.ExamLocation = dto.ExamLocation;
        exam.ExaminerId = dto.ExaminerId;
        exam.ArabicScore = dto.ArabicScore;
        exam.MathScore = dto.MathScore;
        exam.ScienceScore = dto.ScienceScore;
        exam.EnglishScore = dto.EnglishScore;
        exam.TotalScore = dto.TotalScore;
        exam.Percentage = dto.Percentage;
        exam.PassingScore = dto.PassingScore;
        exam.PassedExam = dto.PassedExam;
        exam.ExamNotes = dto.ExamNotes;
        exam.MedicalNotes = dto.MedicalNotes;
        exam.BehavioralNotes = dto.BehavioralNotes;
        exam.FinalRecommendation = dto.FinalRecommendation;
        exam.FinalRecommendationArabic = dto.FinalRecommendationArabic;
        exam.EvaluationDate = dto.EvaluationDate;
        exam.EvaluatedByEmployeeId = dto.EvaluatedByEmployeeId;
        exam.UpdatedAt = DateTime.UtcNow;
        exam.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var examDto = new AdmissionExamDto
        {
            Id = exam.Id,
            AdmissionApplicationId = exam.AdmissionApplicationId,
            InterviewStatus = exam.InterviewStatus,
            InterviewDate = exam.InterviewDate,
            InterviewTime = exam.InterviewTime,
            InterviewLocation = exam.InterviewLocation,
            InterviewScore = exam.InterviewScore,
            InterviewNotes = exam.InterviewNotes,
            ExamStatus = exam.ExamStatus,
            ExamDate = exam.ExamDate,
            ExamTime = exam.ExamTime,
            ExamLocation = exam.ExamLocation,
            ArabicScore = exam.ArabicScore,
            MathScore = exam.MathScore,
            ScienceScore = exam.ScienceScore,
            EnglishScore = exam.EnglishScore,
            TotalScore = exam.TotalScore,
            Percentage = exam.Percentage,
            PassingScore = exam.PassingScore,
            PassedExam = exam.PassedExam,
            ExamNotes = exam.ExamNotes,
            MedicalNotes = exam.MedicalNotes,
            BehavioralNotes = exam.BehavioralNotes,
            FinalRecommendation = exam.FinalRecommendation,
            FinalRecommendationArabic = exam.FinalRecommendationArabic,
            EvaluationDate = exam.EvaluationDate
        };

        return new AdmissionResponseDto<AdmissionExamDto>
        {
            Success = true,
            Message = "تم تحديث جدول المقابلة والاختبار بنجاح",
            Data = examDto
        };
    }

    public async Task<AdmissionResponseDto<AdmissionExamDto>> GetAdmissionExamAsync(Guid id)
    {
        var exam = await _context.AdmissionExams.FindAsync(id);
        if (exam == null)
        {
            return new AdmissionResponseDto<AdmissionExamDto>
            {
                Success = false,
                Message = "جدول المقابلة والاختبار غير موجود"
            };
        }

        var examDto = new AdmissionExamDto
        {
            Id = exam.Id,
            AdmissionApplicationId = exam.AdmissionApplicationId,
            InterviewStatus = exam.InterviewStatus,
            InterviewDate = exam.InterviewDate,
            InterviewTime = exam.InterviewTime,
            InterviewLocation = exam.InterviewLocation,
            InterviewScore = exam.InterviewScore,
            InterviewNotes = exam.InterviewNotes,
            ExamStatus = exam.ExamStatus,
            ExamDate = exam.ExamDate,
            ExamTime = exam.ExamTime,
            ExamLocation = exam.ExamLocation,
            ArabicScore = exam.ArabicScore,
            MathScore = exam.MathScore,
            ScienceScore = exam.ScienceScore,
            EnglishScore = exam.EnglishScore,
            TotalScore = exam.TotalScore,
            Percentage = exam.Percentage,
            PassingScore = exam.PassingScore,
            PassedExam = exam.PassedExam,
            ExamNotes = exam.ExamNotes,
            MedicalNotes = exam.MedicalNotes,
            BehavioralNotes = exam.BehavioralNotes,
            FinalRecommendation = exam.FinalRecommendation,
            FinalRecommendationArabic = exam.FinalRecommendationArabic,
            EvaluationDate = exam.EvaluationDate,
            InterviewWhatsAppSent = exam.InterviewWhatsAppSent,
            InterviewWhatsAppSentAt = exam.InterviewWhatsAppSentAt,
            ExamWhatsAppSent = exam.ExamWhatsAppSent,
            ExamWhatsAppSentAt = exam.ExamWhatsAppSentAt
        };

        return new AdmissionResponseDto<AdmissionExamDto>
        {
            Success = true,
            Data = examDto
        };
    }

    public async Task<AdmissionResponseDto<AdmissionExamDto>> GetExamByApplicationIdAsync(Guid applicationId)
    {
        var exam = await _context.AdmissionExams.FirstOrDefaultAsync(e => e.AdmissionApplicationId == applicationId);
        if (exam == null)
        {
            return new AdmissionResponseDto<AdmissionExamDto>
            {
                Success = false,
                Message = "لا يوجد جدول مقابلة واختبار لهذا الطلب"
            };
        }

        var examDto = new AdmissionExamDto
        {
            Id = exam.Id,
            AdmissionApplicationId = exam.AdmissionApplicationId,
            InterviewStatus = exam.InterviewStatus,
            InterviewDate = exam.InterviewDate,
            InterviewTime = exam.InterviewTime,
            InterviewLocation = exam.InterviewLocation,
            InterviewScore = exam.InterviewScore,
            InterviewNotes = exam.InterviewNotes,
            ExamStatus = exam.ExamStatus,
            ExamDate = exam.ExamDate,
            ExamTime = exam.ExamTime,
            ExamLocation = exam.ExamLocation,
            ArabicScore = exam.ArabicScore,
            MathScore = exam.MathScore,
            ScienceScore = exam.ScienceScore,
            EnglishScore = exam.EnglishScore,
            TotalScore = exam.TotalScore,
            Percentage = exam.Percentage,
            PassingScore = exam.PassingScore,
            PassedExam = exam.PassedExam,
            ExamNotes = exam.ExamNotes,
            MedicalNotes = exam.MedicalNotes,
            BehavioralNotes = exam.BehavioralNotes,
            FinalRecommendation = exam.FinalRecommendation,
            FinalRecommendationArabic = exam.FinalRecommendationArabic,
            EvaluationDate = exam.EvaluationDate
        };

        return new AdmissionResponseDto<AdmissionExamDto>
        {
            Success = true,
            Data = examDto
        };
    }

    // ==================== Application Documents ====================
    public async Task<AdmissionResponseDto<ApplicationDocumentDto>> CreateApplicationDocumentAsync(CreateApplicationDocumentDto dto)
    {
        var document = new ApplicationDocument
        {
            Id = Guid.NewGuid(),
            AdmissionApplicationId = dto.AdmissionApplicationId,
            DocumentType = dto.DocumentType,
            DocumentName = dto.DocumentName,
            FilePath = dto.FilePath,
            FileSize = dto.FileSize,
            FileType = dto.FileType,
            OriginalFileName = dto.OriginalFileName,
            IsRequired = dto.IsRequired,
            Description = dto.Description,
            UploadedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.ApplicationDocuments.Add(document);
        await _context.SaveChangesAsync();

        var documentDto = new ApplicationDocumentDto
        {
            Id = document.Id,
            AdmissionApplicationId = document.AdmissionApplicationId,
            DocumentType = document.DocumentType,
            DocumentName = document.DocumentName,
            FilePath = document.FilePath,
            FileSize = document.FileSize,
            FileType = document.FileType,
            OriginalFileName = document.OriginalFileName,
            IsRequired = document.IsRequired,
            IsVerified = document.IsVerified,
            UploadedAt = document.UploadedAt,
            Description = document.Description
        };

        return new AdmissionResponseDto<ApplicationDocumentDto>
        {
            Success = true,
            Message = "تم رفع المستند بنجاح",
            Data = documentDto
        };
    }

    public async Task<AdmissionResponseDto<bool>> UpdateApplicationDocumentAsync(UpdateApplicationDocumentDto dto)
    {
        var document = await _context.ApplicationDocuments.FindAsync(dto.Id);
        if (document == null)
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "المستند غير موجود"
            };
        }

        document.IsVerified = dto.IsVerified;
        document.VerificationNotes = dto.VerificationNotes;
        document.VerifiedDate = DateTime.UtcNow;
        document.UpdatedAt = DateTime.UtcNow;
        document.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        return new AdmissionResponseDto<bool>
        {
            Success = true,
            Message = "تم تحديث المستند بنجاح",
            Data = true
        };
    }

    public async Task<AdmissionResponseDto<bool>> DeleteApplicationDocumentAsync(Guid id)
    {
        var document = await _context.ApplicationDocuments.FindAsync(id);
        if (document == null)
        {
            return new AdmissionResponseDto<bool>
            {
                Success = false,
                Message = "المستند غير موجود"
            };
        }

        _context.ApplicationDocuments.Remove(document);
        await _context.SaveChangesAsync();

        return new AdmissionResponseDto<bool>
        {
            Success = true,
            Message = "تم حذف المستند بنجاح",
            Data = true
        };
    }

    public async Task<AdmissionPagedResponseDto<ApplicationDocumentDto>> GetApplicationDocumentsAsync(Guid applicationId)
    {
        var documents = await _context.ApplicationDocuments
            .Where(d => d.AdmissionApplicationId == applicationId)
            .Select(d => new ApplicationDocumentDto
            {
                Id = d.Id,
                AdmissionApplicationId = d.AdmissionApplicationId,
                DocumentType = d.DocumentType,
                DocumentName = d.DocumentName,
                FilePath = d.FilePath,
                FileSize = d.FileSize,
                FileType = d.FileType,
                OriginalFileName = d.OriginalFileName,
                IsRequired = d.IsRequired,
                IsVerified = d.IsVerified,
                VerifiedDate = d.VerifiedDate,
                VerificationNotes = d.VerificationNotes,
                UploadedAt = d.UploadedAt,
                Description = d.Description
            })
            .ToListAsync();

        return new AdmissionPagedResponseDto<ApplicationDocumentDto>
        {
            Success = true,
            Data = documents,
            TotalCount = documents.Count,
            Page = 1,
            PageSize = documents.Count,
            TotalPages = 1
        };
    }

    // ==================== Student Conversion ====================
    public async Task<AdmissionResponseDto<ConversionResultDto>> ConvertToStudentAsync(ConvertToStudentDto dto)
    {
        var application = await _context.AdmissionApplications
            .Include(a => a.School)
            .Include(a => a.GradeLevel)
            .Include(a => a.Section)
            .FirstOrDefaultAsync(a => a.Id == dto.AdmissionApplicationId);

        if (application == null)
        {
            return new AdmissionResponseDto<ConversionResultDto>
            {
                Success = false,
                Message = "طلب الالتحاق غير موجود"
            };
        }

        if (application.Status != AdmissionStatus.Accepted)
        {
            return new AdmissionResponseDto<ConversionResultDto>
            {
                Success = false,
                Message = "يمكن تحويل الطلبات المقبولة فقط"
            };
        }

        if (application.ConvertedStudentId.HasValue)
        {
            return new AdmissionResponseDto<ConversionResultDto>
            {
                Success = false,
                Message = "تم تحويل هذا الطلب لطالب رسمي بالفعل"
            };
        }

        // Create or find guardian
        var guardian = await _context.Guardians
            .FirstOrDefaultAsync(g => g.NationalId == application.GuardianNationalId);

        if (guardian == null)
        {
            guardian = new Guardian
            {
                Id = Guid.NewGuid(),
                FirstName = application.GuardianFirstName,
                LastName = application.GuardianLastName,
                FullNameArabic = application.GuardianFullNameArabic,
                NationalId = application.GuardianNationalId,
                PhoneNumber = application.GuardianPhoneNumber,
                WhatsAppNumber = application.GuardianWhatsAppNumber,
                Email = application.GuardianEmail,
                Occupation = application.GuardianOccupation,
                OccupationArabic = application.GuardianOccupationArabic,
                Relationship = application.Relationship,
                RelationshipArabic = application.RelationshipArabic,
                TenantId = application.School.TenantId ?? Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.Guardians.Add(guardian);
            await _context.SaveChangesAsync();
        }

        // Generate student number
        var studentNumber = await GenerateStudentNumberAsync(application.SchoolId);

        // Create student
        var student = new Student
        {
            Id = Guid.NewGuid(),
            StudentNumber = studentNumber,
            FirstName = application.FirstName,
            FirstNameArabic = application.FirstName,
            LastName = application.LastName,
            LastNameArabic = application.LastName,
            FullNameArabic = application.FullNameArabic,
            BirthDate = application.BirthDate,
            Gender = application.Gender,
            NationalId = application.NationalId,
            SchoolId = application.SchoolId,
            ClassRoomId = dto.ClassRoomId,
            GuardianId = guardian.Id,
            SpecialNeeds = application.HasSpecialNeeds ? application.SpecialNeedsDescription : null,
            SpecialNeedsArabic = application.HasSpecialNeeds ? application.SpecialNeedsDescription : null,
            IsGifted = application.IsGifted,
            GiftedProgram = application.GiftedDescription,
            Address = application.Address,
            PhoneNumber = application.PhoneNumber,
            Email = application.Email,
            NoorStudentId = dto.NoorStudentId,
            EnrollmentDate = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Create student account
        var studentAccount = new StudentAccount
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            StudentId = student.Id,
            AccountNumber = $"SA-{studentNumber}",
            TotalBalance = 0,
            OutstandingBalance = 0,
            PaidBalance = 0,
            DiscountBalance = 0,
            Status = AccountStatus.Active,
            IsVatExempt = student.IsSaudiCitizen,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.StudentAccounts.Add(studentAccount);
        await _context.SaveChangesAsync();

        // Update application
        application.ConvertedStudentId = student.Id;
        application.Status = AdmissionStatus.Enrolled;
        application.ConversionDate = DateTime.UtcNow;
        application.LastUpdated = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;
        application.UpdatedBy = "System";

        await _context.SaveChangesAsync();

        var result = new ConversionResultDto
        {
            StudentId = student.Id,
            StudentNumber = studentNumber,
            GuardianId = guardian.Id,
            AccountCreated = true,
            Message = "تم تحويل المتقدم لطالب رسمي بنجاح"
        };

        return new AdmissionResponseDto<ConversionResultDto>
        {
            Success = true,
            Message = "تم التحويل بنجاح",
            Data = result
        };
    }

    // ==================== Helper Methods ====================
    private async Task<string> GenerateApplicationNumberAsync(Guid schoolId)
    {
        var year = DateTime.UtcNow.Year.ToString();
        var prefix = $"ADM{year}";

        var lastNumber = await _context.AdmissionApplications
            .Where(a => a.SchoolId == schoolId && a.ApplicationNumber.StartsWith(prefix))
            .OrderByDescending(a => a.ApplicationNumber)
            .Select(a => a.ApplicationNumber)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (lastNumber != null)
        {
            var lastSequence = int.Parse(lastNumber.Substring(prefix.Length));
            sequence = lastSequence + 1;
        }

        return $"{prefix}{sequence:D5}";
    }

    private async Task<string> GenerateStudentNumberAsync(Guid schoolId)
    {
        var year = DateTime.UtcNow.Year.ToString();
        var prefix = $"STU{year}";

        var lastNumber = await _context.Students
            .Where(s => s.SchoolId == schoolId && s.StudentNumber.StartsWith(prefix))
            .OrderByDescending(s => s.StudentNumber)
            .Select(s => s.StudentNumber)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (lastNumber != null)
        {
            var lastSequence = int.Parse(lastNumber.Substring(prefix.Length));
            sequence = lastSequence + 1;
        }

        return $"{prefix}{sequence:D5}";
    }

    private bool IsValidStatusTransition(AdmissionStatus currentStatus, AdmissionStatus newStatus)
    {
        // Define valid status transitions
        var validTransitions = new Dictionary<AdmissionStatus, List<AdmissionStatus>>
        {
            [AdmissionStatus.New] = new List<AdmissionStatus> { AdmissionStatus.UnderReview, AdmissionStatus.InterviewScheduled, AdmissionStatus.ExamScheduled, AdmissionStatus.Withdrawn },
            [AdmissionStatus.UnderReview] = new List<AdmissionStatus> { AdmissionStatus.InterviewScheduled, AdmissionStatus.ExamScheduled, AdmissionStatus.UnderConsideration, AdmissionStatus.Rejected, AdmissionStatus.Withdrawn },
            [AdmissionStatus.InterviewScheduled] = new List<AdmissionStatus> { AdmissionStatus.ExamScheduled, AdmissionStatus.UnderConsideration, AdmissionStatus.Withdrawn },
            [AdmissionStatus.ExamScheduled] = new List<AdmissionStatus> { AdmissionStatus.UnderConsideration, AdmissionStatus.Withdrawn },
            [AdmissionStatus.UnderConsideration] = new List<AdmissionStatus> { AdmissionStatus.Accepted, AdmissionStatus.Rejected, AdmissionStatus.Waitlisted },
            [AdmissionStatus.Accepted] = new List<AdmissionStatus> { AdmissionStatus.Enrolled },
            [AdmissionStatus.Waitlisted] = new List<AdmissionStatus> { AdmissionStatus.Accepted, AdmissionStatus.Rejected, AdmissionStatus.Withdrawn },
            [AdmissionStatus.Rejected] = new List<AdmissionStatus>(), // No transitions from rejected
            [AdmissionStatus.Withdrawn] = new List<AdmissionStatus>(), // No transitions from withdrawn
            [AdmissionStatus.Enrolled] = new List<AdmissionStatus>() // No transitions from enrolled
        };

        return validTransitions.ContainsKey(currentStatus) && validTransitions[currentStatus].Contains(newStatus);
    }
}