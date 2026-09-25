using System.ComponentModel.DataAnnotations;

namespace Masar.Schools.ERP.WebUI.ViewModels;

public class ClassroomListVM
{
    public Guid Id { get; set; }
    
    [Display(Name = "اسم الفصل")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "اسم الفصل بالعربي")]
    public string NameArabic { get; set; } = string.Empty;
    
    [Display(Name = "كود الفصل")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "المرحلة")]
    public string? GradeLevel { get; set; }
    
    [Display(Name = "الشعبة")]
    public string? Section { get; set; }
    
    [Display(Name = "المرحلة الدراسية")]
    public string? GradeLevelName { get; set; }
    
    [Display(Name = "الشعبة الدراسية")]
    public string? SectionName { get; set; }
    
    [Display(Name = "السعة")]
    public int Capacity { get; set; }
    
    [Display(Name = "عدد الطلاب")]
    public int CurrentCount { get; set; }
    
    [Display(Name = "المتاح")]
    public int AvailableCapacity => Capacity - CurrentCount;
    
    [Display(Name = "الحالة")]
    public bool IsActive { get; set; }
    
    [Display(Name = "المدرسة")]
    public string? SchoolName { get; set; }
    
    [Display(Name = "المؤسسة")]
    public string? TenantName { get; set; }
    
    [Display(Name = "معلم الفصل")]
    public string? ClassTeacherName { get; set; }
    
    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "تم الإنشاء بواسطة")]
    public string? CreatedBy { get; set; }
}