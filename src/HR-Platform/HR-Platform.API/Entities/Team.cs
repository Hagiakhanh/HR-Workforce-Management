namespace HR_Platform.API.Entities;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DepartmentId { get; set; }
    public string TeamCode { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public Guid? TeamLeadId { get; set; }
    public string? Location { get; set; }
    public int TargetHeadcount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateOnly? EffectiveDate { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual Employee? TeamLead { get; set; }
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public virtual ICollection<Position> DefaultPositions { get; set; } = new List<Position>();
    public virtual ICollection<EmploymentRecord> EmploymentRecords { get; set; } = new List<EmploymentRecord>();
}
