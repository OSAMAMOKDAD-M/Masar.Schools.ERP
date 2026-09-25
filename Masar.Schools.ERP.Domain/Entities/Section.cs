using Masar.Schools.ERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.Domain.Entities;

public class Section : BaseEntity
{
    [Required(ErrorMessage = "اسم الشعبة مطلوب")]
    [StringLength(50, ErrorMessage = "اسم الشعبة يجب أن لا يتجاوز 50 حرف")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "اسم الشعبة بالعربي مطلوب")]
    [StringLength(50, ErrorMessage = "اسم الشعبة بالعربي يجب أن لا يتجاوز 50 حرف")]
    public string NameArabic { get; set; } = string.Empty;
    
    [StringLength(10, ErrorMessage = "كود الشعبة يجب أن لا يتجاوز 10 حرف")]
    [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "كود الشعبة يجب أن يحتوي على أحرف وأرقام فقط")]
    public string Code { get; set; } = string.Empty;
    
    [Range(1, 20, ErrorMessage = "الترتيب يجب أن يكون بين 1 و 20")]
    public int DisplayOrder { get; set; } = 1;
    
    public bool IsActive { get; set; } = true;
    
    // العلاقات
    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
}