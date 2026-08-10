namespace HR_Platform.API.Entities;

public class RequestActivityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RequestId { get; set; }
    public Guid? ActorId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Request Request { get; set; } = null!;
    public virtual Employee? Actor { get; set; }
}
