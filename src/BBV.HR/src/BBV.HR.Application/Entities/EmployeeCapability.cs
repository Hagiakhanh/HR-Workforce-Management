namespace BBV.HR.Application.Entities;

public class EmployeeCapability
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid CapabilityId { get; set; }
    public Capability Capability { get; set; } = null!;

    public int? ProficiencyLevel { get; set; }
    public decimal? YearsExperience { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
