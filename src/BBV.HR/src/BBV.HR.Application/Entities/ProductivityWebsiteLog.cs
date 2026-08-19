namespace BBV.HR.Application.Entities;

public class ProductivityWebsiteLog
{
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }
    public ProductivitySession Session { get; set; } = null!;

    public string? Domain { get; set; }
    public string? Url { get; set; }
    public int? DurationMins { get; set; }
    public string? Category { get; set; }
}
