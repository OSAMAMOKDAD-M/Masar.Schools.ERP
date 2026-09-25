using Masar.Schools.ERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.Domain.Entities;

public class School : BaseEntity
{
    [Required(ErrorMessage = "اسم المدرسة (إنجليزي) مطلوب")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "اسم المدرسة (عربي) مطلوب")]
    public string NameArabic { get; set; } = string.Empty;
    
    public string? Code { get; set; }
    public string? NoorSchoolId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; }
    
    public Guid? TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
}
