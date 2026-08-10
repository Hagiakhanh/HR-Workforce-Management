namespace HR_Platform.API.Entities;

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PreferredName { get; set; }
    public string? PhotoUrl { get; set; }
    public string WorkEmail { get; set; } = string.Empty;
    public string? PersonalEmail { get; set; }
    public string? WorkPhone { get; set; }
    public string? PersonalPhone { get; set; }
    public string? Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Nationality { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateOnly? HireDate { get; set; }
    public Guid? UserId { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
    public virtual ICollection<EmploymentRecord> EmploymentRecords { get; set; } = new List<EmploymentRecord>();
    public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
    public virtual ICollection<SalaryHistory> SalaryHistories { get; set; } = new List<SalaryHistory>();
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();
    public virtual ICollection<TrainingRecord> TrainingRecords { get; set; } = new List<TrainingRecord>();
    public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();
    public virtual ICollection<Request> SubmittedRequests { get; set; } = new List<Request>();
    public virtual ICollection<Request> HandoverRequests { get; set; } = new List<Request>();
    public virtual ICollection<ApprovalStep> AssignedApprovalSteps { get; set; } = new List<ApprovalStep>();
    public virtual ICollection<RequestActivityLog> RequestActivityLogs { get; set; } = new List<RequestActivityLog>();
    public virtual ICollection<LeaveQuota> LeaveQuotas { get; set; } = new List<LeaveQuota>();
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public virtual ICollection<ReportingLine> ReportingLinesAsEmployee { get; set; } = new List<ReportingLine>();
    public virtual ICollection<ReportingLine> ReportingLinesAsManager { get; set; } = new List<ReportingLine>();
    public virtual ICollection<Department> ManagedDepartments { get; set; } = new List<Department>();
    public virtual ICollection<Team> LedTeams { get; set; } = new List<Team>();
    public virtual ICollection<EmploymentRecord> ManagedEmploymentRecords { get; set; } = new List<EmploymentRecord>();
}
