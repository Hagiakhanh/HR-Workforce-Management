namespace BBV.HR.Application.Entities;

public class ProjectRequiredCapability
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid CapabilityId { get; set; }
    public Capability Capability { get; set; } = null!;

    public int? RequiredLevel { get; set; }
    public int? RequiredPeople { get; set; }
}
