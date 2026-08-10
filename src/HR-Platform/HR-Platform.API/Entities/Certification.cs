namespace HR_Platform.API.Entities;

public class Certification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string CertificationName { get; set; } = string.Empty;
    public string? CredentialId { get; set; }
    public string? IssuingOrganization { get; set; }
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}
