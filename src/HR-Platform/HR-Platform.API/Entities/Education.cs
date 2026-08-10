namespace HR_Platform.API.Entities;

public class Education
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string? DegreeType { get; set; }
    public string? Major { get; set; }
    public string? InstitutionName { get; set; }
    public string? Location { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
