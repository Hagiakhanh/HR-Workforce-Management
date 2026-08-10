namespace HR_Platform.API.Entities;

public class Position
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DepartmentId { get; set; }
    public string PositionCode { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public Guid? DefaultTeamId { get; set; }
    public Guid? ReportsToPositionId { get; set; }
    public string? JobLevel { get; set; }
    public int ApprovedHeadcount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual Team? DefaultTeam { get; set; }
    public virtual Position? ReportsToPosition { get; set; }
    public virtual ICollection<Position> SubordinatePositions { get; set; } = new List<Position>();
    public virtual ICollection<EmploymentRecord> EmploymentRecords { get; set; } = new List<EmploymentRecord>();
}
