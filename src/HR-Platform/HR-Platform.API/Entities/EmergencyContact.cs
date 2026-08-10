namespace HR_Platform.API.Entities;

public class EmergencyContact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Relationship { get; set; }
    public string? PrimaryPhone { get; set; }
    public string? SecondaryPhone { get; set; }
    public string? Email { get; set; }
    public string? PreferredContactMethod { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
