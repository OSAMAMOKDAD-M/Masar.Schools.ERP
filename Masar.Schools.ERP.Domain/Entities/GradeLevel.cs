using Masar.Schools.ERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.Domain.Entities;

public class GradeLevel : BaseEntity
{
    [Required(ErrorMessage = "اسم المرحلة مطلوب")]
    [StringLength(100, ErrorMessage = "اسم المرحلة يجب أن لا يتجاوز 100 حرف")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "اسم المرحلة بالعربي مطلوب")]
    [StringLength(100, ErrorMessage = "اسم المرحلة بالعربي يجب أن لا يتجاوز 100 حرف")]
    public string NameArabic { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "كود المرحلة يجب أن لا يتجاوز 20 حرف")]
    [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "كود المرحلة يجب أن يحتوي على أحرف وأرقام فقط")]
    public string Code { get; set; } = string.Empty;
    
    [Range(1, 12, ErrorMessage = "الترتيب يجب أن يكون بين 1 و 12")]
    public int DisplayOrder { get; set; } = 1;
    
    public bool IsActive { get; set; } = true;
    
    // العلاقات
    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
}