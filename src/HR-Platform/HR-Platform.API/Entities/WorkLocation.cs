namespace HR_Platform.API.Entities;

public class WorkLocation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    public virtual ICollection<EmploymentRecord> EmploymentRecords { get; set; } = new List<EmploymentRecord>();
}
