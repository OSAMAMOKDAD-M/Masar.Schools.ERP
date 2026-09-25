using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Masar.Schools.ERP.Domain.Entities;
using Masar.Schools.ERP.Infrastructure.Data;

namespace Masar.Schools.ERP.Infrastructure.Services;

public class ClassroomLookupService : IClassroomLookupService
{
    private readonly MasarDbContext _context;
    private readonly ILogger<ClassroomLookupService> _logger;

    public ClassroomLookupService(MasarDbContext context, ILogger<ClassroomLookupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ClassRoom>> GetClassroomsBySchoolAsync(Guid schoolId)
    {
        try
        {
            return await _context.ClassRooms
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .Where(c => c.SchoolId == schoolId && !c.IsDeleted)
                .OrderBy(c => c.GradeLevelLegacy)
                .ThenBy(c => c.Code)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classrooms by school: {SchoolId}", schoolId);
            throw;
        }
    }

    public async Task<List<ClassRoom>> GetClassroomsByGradeAsync(Guid schoolId, string gradeLevel)
    {
        try
        {
            return await _context.ClassRooms
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .Where(c => c.SchoolId == schoolId &&
                           c.GradeLevelLegacy == gradeLevel &&
                           !c.IsDeleted)
                .OrderBy(c => c.Code)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classrooms by grade: {SchoolId}, {GradeLevel}", schoolId, gradeLevel);
            throw;
        }
    }

    public async Task<int> GetClassroomAvailableCapacityAsync(Guid classroomId)
    {
        try
        {
            var classroom = await _context.ClassRooms
                .FirstOrDefaultAsync(c => c.Id == classroomId && !c.IsDeleted);
            
            if (classroom == null)
                return 0;
                
            return classroom.Capacity - classroom.CurrentCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classroom available capacity: {ClassroomId}", classroomId);
            throw;
        }
    }

    public async Task<List<ClassRoom>> GetAvailableClassroomsAsync(Guid schoolId)
    {
        try
        {
            return await _context.ClassRooms
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .Where(c => c.SchoolId == schoolId &&
                           !c.IsDeleted &&
                           c.IsActive &&
                           c.CurrentCount < c.Capacity)
                .OrderBy(c => c.GradeLevelLegacy)
                .ThenBy(c => c.Code)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available classrooms: {SchoolId}", schoolId);
            throw;
        }
    }

    public async Task<ClassRoom?> GetClassroomByIdAsync(Guid classroomId)
    {
        try
        {
            return await _context.ClassRooms
                .Include(c => c.School)
                .Include(c => c.Branch)
                .Include(c => c.ClassTeacher)
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .FirstOrDefaultAsync(c => c.Id == classroomId && !c.IsDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting classroom by id: {ClassroomId}", classroomId);
            throw;
        }
    }

    public async Task<List<ClassRoom>> GetActiveClassroomsAsync()
    {
        try
        {
            return await _context.ClassRooms
                .Where(c => c.IsActive && !c.IsDeleted)
                .Include(c => c.School)
                .Include(c => c.GradeLevelEntity)
                .Include(c => c.SectionEntity)
                .OrderBy(c => c.NameArabic)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active classrooms");
            throw;
        }
    }

    public async Task<bool> IsClassroomCodeUniqueAsync(Guid schoolId, string code, Guid? excludeClassroomId = null)
    {
        try
        {
            var query = _context.ClassRooms
                .Where(c => c.SchoolId == schoolId && 
                           c.Code == code && 
                           !c.IsDeleted);
            
            if (excludeClassroomId.HasValue)
            {
                query = query.Where(c => c.Id != excludeClassroomId.Value);
            }
            
            return !await query.AnyAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking classroom code uniqueness: {SchoolId}, {Code}", schoolId, code);
            throw;
        }
    }
}