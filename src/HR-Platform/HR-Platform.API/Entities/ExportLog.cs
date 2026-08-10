namespace HR_Platform.API.Entities;

public class ExportLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string ExportFormat { get; set; } = string.Empty;
    public string? SelectedFields { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
}
