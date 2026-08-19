namespace BBV.HR.Application.Entities;

public class ProductivityScreenshot
{
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }
    public ProductivitySession Session { get; set; } = null!;

    public DateTime CapturedAt { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public int? ActivityPct { get; set; }
}
