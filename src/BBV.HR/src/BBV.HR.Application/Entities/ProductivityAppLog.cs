namespace BBV.HR.Application.Entities;

public class ProductivityAppLog
{
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }
    public ProductivitySession Session { get; set; } = null!;

    public string AppName { get; set; } = string.Empty;
    public int? DurationMins { get; set; }
    public string? Category { get; set; }
}
