namespace HR_Platform.API.Entities;

public class EmployeeDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Guid DocumentTypeId { get; set; }
    public string? Category { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public Guid? UploadedByUserId { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string? Status { get; set; }
    public DateTime? VerifiedAt { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual DocumentType DocumentType { get; set; } = null!;
    public virtual User? UploadedByUser { get; set; }
}
