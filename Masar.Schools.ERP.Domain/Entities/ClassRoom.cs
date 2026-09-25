using Masar.Schools.ERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.Domain.Entities;

public class ClassRoom : BaseEntity
{
    [Required(ErrorMessage = "اسم الفصل مطلوب")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "اسم الفصل بالعربي مطلوب")]
    public string NameArabic { get; set; } = string.Empty;
    
    public string Code { get; set; } = string.Empty;
    
    // الحقول القديمة للتوافق (سيتم حذفها مستقبلاً)
    public string? GradeLevelLegacy { get; set; }
    public string? SectionLegacy { get; set; }
    
    // العلاقات الجديدة مع المرحلة والشعبة
    public Guid? GradeLevelId { get; set; }
    public GradeLevel? GradeLevelEntity { get; set; }
    
    public Guid? SectionId { get; set; }
    public Section? SectionEntity { get; set; }
    
    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100")]
    public int Capacity { get; set; } = 30;
    
    public int CurrentCount { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    
    // العلاقات مع المؤسسة والمدرسة
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;
    
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    
    // معلم الفصل
    public Guid? ClassTeacherId { get; set; }
    public Employee? ClassTeacher { get; set; }
    
    // العلاقات مع الطلاب
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    public ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
}