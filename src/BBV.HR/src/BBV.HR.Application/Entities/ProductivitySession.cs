namespace BBV.HR.Application.Entities;

public class ProductivitySession
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public int? ActivityPct { get; set; }
    public string? SessionType { get; set; }

    public DateTime? CreatedAt { get; set; }

    // Navigation properties
    public ICollection<ProductivityAppLog> AppLogs { get; set; } = new List<ProductivityAppLog>();
    public ICollection<ProductivityWebsiteLog> WebsiteLogs { get; set; } = new List<ProductivityWebsiteLog>();
    public ICollection<ProductivityScreenshot> Screenshots { get; set; } = new List<ProductivityScreenshot>();
}
