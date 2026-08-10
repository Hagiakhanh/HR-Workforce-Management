namespace HR_Platform.API.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    public virtual ICollection<ExportLog> ExportLogs { get; set; } = new List<ExportLog>();
    public virtual ICollection<EmploymentHistory> PerformedEmploymentHistories { get; set; } = new List<EmploymentHistory>();
    public virtual ICollection<SalaryHistory> ApprovedSalaryHistories { get; set; } = new List<SalaryHistory>();
    public virtual ICollection<EmployeeDocument> UploadedDocuments { get; set; } = new List<EmployeeDocument>();
}
