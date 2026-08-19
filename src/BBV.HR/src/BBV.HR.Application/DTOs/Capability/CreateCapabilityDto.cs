namespace BBV.HR.Application.DTOs.Capability;

public class CreateCapabilityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }
}