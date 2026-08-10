namespace HR_Platform.API.Entities;

public class Company
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? RegistrationNumber { get; set; }
    public string? Country { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
