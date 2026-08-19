namespace BBV.HR.Application.Entities;

public class Capability
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<EmployeeCapability> EmployeeCapabilities { get; set; } = new List<EmployeeCapability>();
    public ICollection<ProjectRequiredCapability> ProjectRequiredCapabilities { get; set; } = new List<ProjectRequiredCapability>();
}
