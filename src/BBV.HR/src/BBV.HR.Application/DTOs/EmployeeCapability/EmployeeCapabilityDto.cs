namespace BBV.HR.Application.DTOs.EmployeeCapability;

public class EmployeeCapabilityDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid CapabilityId { get; set; }
    public string CapabilityName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? CapabilityDescription { get; set; }
    public int? ProficiencyLevel { get; set; }
    public decimal? YearsExperience { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AddEmployeeCapabilityDto
{
    public Guid CapabilityId { get; set; }
    public int? ProficiencyLevel { get; set; }
    public decimal? YearsExperience { get; set; }
}

public class UpdateEmployeeCapabilityDto
{
    public int? ProficiencyLevel { get; set; }
    public decimal? YearsExperience { get; set; }
}
