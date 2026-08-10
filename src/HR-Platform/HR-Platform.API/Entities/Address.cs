namespace HR_Platform.API.Entities;

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string? AddressType { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
