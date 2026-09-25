using Masar.Schools.ERP.Domain.Common;

namespace Masar.Schools.ERP.Domain.Entities;

public class Grade : BaseEntity
{
    public string Subject { get; set; } = string.Empty;
    public string SubjectArabic { get; set; } = string.Empty;
    public decimal? MidtermScore { get; set; }
    public decimal? FinalScore { get; set; }
    public decimal? TotalScore { get; set; }
    public decimal? MaxScore { get; set; }
    public string? GradeLetter { get; set; }
    public string? Term { get; set; }
    public string? AcademicYear { get; set; }
    public string? Notes { get; set; }
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public Guid? TeacherId { get; set; }
    public Employee? Teacher { get; set; }
}
