namespace HR_Platform.API.Entities;

public class TeamMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeamId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? RoleInTeam { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Team Team { get; set; } = null!;
    public virtual Employee Employee { get; set; } = null!;
}
