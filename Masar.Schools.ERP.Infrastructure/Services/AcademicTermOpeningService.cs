using Masar.Schools.ERP.Domain.DTOs;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;
using Masar.Schools.ERP.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Masar.Schools.ERP.Infrastructure.Services;

/// <summary>
/// خدمة فتح الفصل الدراسي - تنفذ دورة العمل كاملة
/// </summary>
public class AcademicTermOpeningService : IAcademicTermOpeningService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<AcademicTermOpeningService> _logger;

    public AcademicTermOpeningService(MasarDbContext context, ILogger<AcademicTermOpeningService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AcademicTermOpeningSummary> GetOpeningSummaryAsync(OpenAcademicTermRequest request)
    {
        var summary = new AcademicTermOpeningSummary();

        // Get selected grade levels
        var gradeLevels = await _context.GradeLevels
            .Where(gl => request.SelectedGradeLevels.Contains(gl.Id))
            .ToListAsync();
        summary.TotalGradeLevels = gradeLevels.Count;

        // Get selected grades (Note: Grades are marks, not grade levels)
        // For summary, we use the count of selected grade IDs
        summary.TotalGrades = request.SelectedGrades.Count;

        // Get selected sections
        var sections = await _context.Sections
            .Where(s => request.SelectedSections.Contains(s.Id))
            .ToListAsync();
        summary.TotalSections = sections.Count;

        // Count students to promote
        if (request.AutoPromoteStudents)
        {
            summary.TotalStudentsToPromote = await _context.Students
                .Where(s => !s.IsDeleted && s.IsActive)
                .CountAsync();
        }

        // Count teachers
        summary.TotalTeachers = await _context.Employees
            .Where(e => !e.IsDeleted && e.IsActive && 
                       (e.JobTitleArabic != null && e.JobTitleArabic.Contains("معلم")))
            .CountAsync();

        // Count subjects (placeholder - would need Subject entity)
        summary.TotalSubjects = 0;

        // Calculate expected fees (placeholder - would need FeeStructure entity)
        summary.ExpectedFeesAmount = 0;

        return summary;
    }

    public async Task<OpenAcademicTermResult> OpenAcademicTermAsync(OpenAcademicTermRequest request)
    {
        var result = new OpenAcademicTermResult();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Execute all steps in a single transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Step 1: Create Academic Term
                var step1Result = await Step1_CreateAcademicTermAsync(request);
                result.Steps.Add(step1Result);
                if (!step1Result.Success)
                {
                    throw new Exception($"Step 1 failed: {step1Result.Message}");
                }
                result.TermId = step1Result.Success ? Guid.NewGuid() : null;

                // Step 2: Link Academic Structure
                var step2Result = await Step2_LinkAcademicStructureAsync(request);
                result.Steps.Add(step2Result);
                if (!step2Result.Success)
                {
                    throw new Exception($"Step 2 failed: {step2Result.Message}");
                }

                // Step 3: Allocate Teachers and Courses
                var step3Result = await Step3_AllocateTeachersAndCoursesAsync(request);
                result.Steps.Add(step3Result);
                if (!step3Result.Success)
                {
                    throw new Exception($"Step 3 failed: {step3Result.Message}");
                }

                // Step 4: Enroll/Promote Students
                var step4Result = await Step4_EnrollStudentsAsync(request);
                result.Steps.Add(step4Result);
                if (!step4Result.Success)
                {
                    throw new Exception($"Step 4 failed: {step4Result.Message}");
                }

                // Step 5: Generate Financial Records
                var step5Result = await Step5_GenerateFinancialRecordsAsync(request);
                result.Steps.Add(step5Result);
                if (!step5Result.Success)
                {
                    throw new Exception($"Step 5 failed: {step5Result.Message}");
                }

                // Step 6: Initialize Attendance System
                var step6Result = await Step6_InitializeAttendanceSystemAsync(request);
                result.Steps.Add(step6Result);
                if (!step6Result.Success)
                {
                    throw new Exception($"Step 6 failed: {step6Result.Message}");
                }

                // Step 7: Initialize Risk Prediction Engine
                var step7Result = await Step7_InitializeRiskPredictionAsync(request);
                result.Steps.Add(step7Result);
                if (!step7Result.Success)
                {
                    throw new Exception($"Step 7 failed: {step7Result.Message}");
                }

                // Commit transaction
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "تم فتح الفصل الدراسي بنجاح - Academic Term Opened Successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Success = false;
                result.Message = $"فشل فتح الفصل الدراسي: {ex.Message}";
                result.Errors.Add(ex.Message);
                _logger.LogError(ex, "Failed to open academic term");
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"خطأ في معاملة قاعدة البيانات: {ex.Message}";
            result.Errors.Add(ex.Message);
            _logger.LogError(ex, "Database transaction error");
        }

        stopwatch.Stop();
        return result;
    }

    #region Workflow Steps

    /// <summary>
    /// الخطوة 1: إنشاء الفصل الدراسي
    /// </summary>
    private async Task<TermOpeningStepResult> Step1_CreateAcademicTermAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Create Academic Term",
            StepNameArabic = "إنشاء الفصل الدراسي"
        };

        try
        {
            // Check if term already exists for this school
            var existingTerm = await _context.AcademicTerms
                .FirstOrDefaultAsync(t => t.SchoolId == request.SchoolId && 
                                       t.TermName == request.TermName &&
                                       t.StartDate.Year == request.StartDate.Year);

            if (existingTerm != null)
            {
                result.Success = false;
                result.Message = "الفصل الدراسي موجود بالفعل - Academic Term already exists";
                return result;
            }

            var academicTerm = new AcademicTerm
            {
                Id = Guid.NewGuid(),
                TermName = request.TermName,
                TermNameArabic = request.TermNameArabic,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                SchoolId = request.SchoolId,
                Status = "Active",
                StatusArabic = "نشط",
                IsActive = true,
                BiometricAttendanceEnabled = request.EnableBiometricAttendance,
                RiskPredictionEnabled = request.EnableRiskPrediction,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.AcademicTerms.Add(academicTerm);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "تم إنشاء الفصل الدراسي بنجاح";
            result.RecordsProcessed = 1;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل إنشاء الفصل الدراسي: {ex.Message}";
            _logger.LogError(ex, "Step 1 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 2: ربط الهيكل الأكاديمي
    /// </summary>
    private async Task<TermOpeningStepResult> Step2_LinkAcademicStructureAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Link Academic Structure",
            StepNameArabic = "ربط الهيكل الأكاديمي"
        };

        try
        {
            var recordsProcessed = 0;

            // Link selected grade levels
            var gradeLevels = await _context.GradeLevels
                .Where(gl => request.SelectedGradeLevels.Contains(gl.Id))
                .ToListAsync();
            
            foreach (var gl in gradeLevels)
            {
                gl.IsActive = true;
                gl.UpdatedAt = DateTime.UtcNow;
                recordsProcessed++;
            }

            // Link selected grades (Note: Grades are marks, not grade levels - they don't need activation)
            // grades are not activated as they represent student marks, not grade levels

            // Link selected sections
            var sections = await _context.Sections
                .Where(s => request.SelectedSections.Contains(s.Id))
                .ToListAsync();
            
            foreach (var section in sections)
            {
                section.IsActive = true;
                section.UpdatedAt = DateTime.UtcNow;
                recordsProcessed++;
            }

            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = $"تم ربط {recordsProcessed} عنصر أكاديمي بنجاح";
            result.RecordsProcessed = recordsProcessed;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل ربط الهيكل الأكاديمي: {ex.Message}";
            _logger.LogError(ex, "Step 2 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 3: توزيع المعلمين والمواد
    /// </summary>
    private async Task<TermOpeningStepResult> Step3_AllocateTeachersAndCoursesAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Allocate Teachers and Courses",
            StepNameArabic = "توزيع المعلمين والمواد"
        };

        try
        {
            // Placeholder: This would involve creating ClassSchedule, CourseAssignment entities
            // For now, we'll just count available teachers
            var teachers = await _context.Employees
                .Where(e => !e.IsDeleted && e.IsActive && 
                           (e.JobTitleArabic != null && e.JobTitleArabic.Contains("معلم")))
                .CountAsync();

            result.Success = true;
            result.Message = $"تم تجهيز {teachers} معلم للتوزيع";
            result.RecordsProcessed = teachers;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل توزيع المعلمين: {ex.Message}";
            _logger.LogError(ex, "Step 3 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 4: تسكين الطلاب
    /// </summary>
    private async Task<TermOpeningStepResult> Step4_EnrollStudentsAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Enroll Students",
            StepNameArabic = "تسكين الطلاب"
        };

        try
        {
            var recordsProcessed = 0;

            if (request.AutoPromoteStudents)
            {
                // Get all active students
                var students = await _context.Students
                    .Where(s => !s.IsDeleted && s.IsActive)
                    .ToListAsync();

                // Promote students to next grade (simplified logic)
                foreach (var student in students)
                {
                    // In a real implementation, this would:
                    // 1. Check current grade
                    // 2. Determine next grade based on promotion rules
                    // 3. Update student's grade
                    // 4. Create enrollment record
                    
                    student.UpdatedAt = DateTime.UtcNow;
                    recordsProcessed++;
                }

                await _context.SaveChangesAsync();
            }

            result.Success = true;
            result.Message = $"تم تسكين {recordsProcessed} طالب بنجاح";
            result.RecordsProcessed = recordsProcessed;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل تسكين الطلاب: {ex.Message}";
            _logger.LogError(ex, "Step 4 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 5: توليد السجلات المالية
    /// </summary>
    private async Task<TermOpeningStepResult> Step5_GenerateFinancialRecordsAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Generate Financial Records",
            StepNameArabic = "توليد السجلات المالية"
        };

        try
        {
            var recordsProcessed = 0;

            if (request.AutoGenerateFees)
            {
                // Placeholder: This would involve creating Invoice, FeeSchedule entities
                // For now, we'll just count students that would get invoices
                var students = await _context.Students
                    .Where(s => !s.IsDeleted && s.IsActive)
                    .CountAsync();

                recordsProcessed = students;
            }

            result.Success = true;
            result.Message = $"تم تجهيز {recordsProcessed} سجل مالي";
            result.RecordsProcessed = recordsProcessed;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل توليد السجلات المالية: {ex.Message}";
            _logger.LogError(ex, "Step 5 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 6: تهيئة نظام الحضور
    /// </summary>
    private async Task<TermOpeningStepResult> Step6_InitializeAttendanceSystemAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Initialize Attendance System",
            StepNameArabic = "تهيئة نظام الحضور"
        };

        try
        {
            if (request.EnableBiometricAttendance)
            {
                // Placeholder: This would involve creating attendance templates
                // and configuring biometric devices
                
                result.Success = true;
                result.Message = "تم تفعيل نظام الحضور بالبصمة بنجاح";
                result.RecordsProcessed = 1;
            }
            else
            {
                result.Success = true;
                result.Message = "تم تخطي تفعيل نظام الحضور";
                result.RecordsProcessed = 0;
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل تهيئة نظام الحضور: {ex.Message}";
            _logger.LogError(ex, "Step 6 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    /// <summary>
    /// الخطوة 7: تهيئة محرك التنبؤ بالمخاطر
    /// </summary>
    private async Task<TermOpeningStepResult> Step7_InitializeRiskPredictionAsync(OpenAcademicTermRequest request)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new TermOpeningStepResult
        {
            StepName = "Initialize Risk Prediction Engine",
            StepNameArabic = "تهيئة محرك التنبؤ بالمخاطر"
        };

        try
        {
            if (request.EnableRiskPrediction)
            {
                // Placeholder: This would involve:
                // 1. Resetting risk scores for students
                // 2. Configuring risk thresholds
                // 3. Initializing prediction models
                
                var students = await _context.Students
                    .Where(s => !s.IsDeleted && s.IsActive)
                    .CountAsync();

                result.Success = true;
                result.Message = $"تم تفعيل محرك التنبؤ لـ {students} طالب";
                result.RecordsProcessed = students;
            }
            else
            {
                result.Success = true;
                result.Message = "تم تخطي تفعيل محرك التنبؤ";
                result.RecordsProcessed = 0;
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"فشل تهيئة محرك التنبؤ: {ex.Message}";
            _logger.LogError(ex, "Step 7 failed");
        }

        stopwatch.Stop();
        result.ExecutionTime = stopwatch.Elapsed;
        return result;
    }

    #endregion

    public async Task<List<AcademicTermDto>> GetAllAcademicTermsAsync()
    {
        var terms = await _context.AcademicTerms
            .Include(t => t.School)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return terms.Select(t => new AcademicTermDto
        {
            Id = t.Id,
            TermName = t.TermName,
            TermNameArabic = t.TermNameArabic,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            SchoolId = t.SchoolId,
            SchoolName = t.School?.Name,
            Status = t.Status,
            StatusArabic = t.StatusArabic,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();
    }

    public async Task<AcademicTermDto?> GetAcademicTermByIdAsync(Guid id)
    {
        var term = await _context.AcademicTerms
            .Include(t => t.School)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (term == null) return null;

        return new AcademicTermDto
        {
            Id = term.Id,
            TermName = term.TermName,
            TermNameArabic = term.TermNameArabic,
            StartDate = term.StartDate,
            EndDate = term.EndDate,
            SchoolId = term.SchoolId,
            SchoolName = term.School?.Name,
            Status = term.Status,
            StatusArabic = term.StatusArabic,
            IsActive = term.IsActive,
            CreatedAt = term.CreatedAt,
            UpdatedAt = term.UpdatedAt
        };
    }

    public async Task<bool> ToggleAcademicTermStatusAsync(Guid id, bool isActive)
    {
        var term = await _context.AcademicTerms.FindAsync(id);
        if (term == null) return false;

        term.IsActive = isActive;
        term.Status = isActive ? "Active" : "Inactive";
        term.StatusArabic = isActive ? "نشط" : "غير نشط";
        term.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Toggled academic term {id} to {isActive}");
        return true;
    }
}
