using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.ViewModels;

public class ClassroomEditVM
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "اسم الفصل مطلوب")]
    [StringLength(100, ErrorMessage = "اسم الفصل يجب أن لا يتجاوز 100 حرف")]
    [Display(Name = "اسم الفصل")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "اسم الفصل بالعربي مطلوب")]
    [StringLength(100, ErrorMessage = "اسم الفصل بالعربي يجب أن لا يتجاوز 100 حرف")]
    [Display(Name = "اسم الفصل بالعربي")]
    public string NameArabic { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "كود الفصل يجب أن لا يتجاوز 20 حرف")]
    [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "كود الفصل يجب أن يحتوي على أحرف وأرقام فقط")]
    [Display(Name = "كود الفصل")]
    public string Code { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "المرحلة الدراسية مطلوبة")]
    [Display(Name = "المرحلة الدراسية")]
    public Guid GradeLevelId { get; set; }
    
    [Required(ErrorMessage = "الشعبة مطلوبة")]
    [Display(Name = "الشعبة")]
    public Guid SectionId { get; set; }
    
    [Required(ErrorMessage = "السعة مطلوبة")]
    [Range(1, 100, ErrorMessage = "السعة يجب أن تكون بين 1 و 100 طالب")]
    [Display(Name = "السعة")]
    public int Capacity { get; set; } = 30;
    
    [Required(ErrorMessage = "المدرسة مطلوبة")]
    [Display(Name = "المدرسة")]
    public Guid SchoolId { get; set; }
    
    [Display(Name = "الفرع")]
    public Guid? BranchId { get; set; }
    
    [Display(Name = "معلم الفصل")]
    public Guid? ClassTeacherId { get; set; }
    
    [Display(Name = "نشط")]
    public bool IsActive { get; set; } = true;
    
    [Display(Name = "عدد الطلاب الحالي")]
    public int CurrentCount { get; set; }
}