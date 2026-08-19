namespace BBV.HR.Application.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
    public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    public ICollection<EmployeeCapability> EmployeeCapabilities { get; set; } = new List<EmployeeCapability>();
    public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
    public ICollection<Timesheet> ReviewedTimesheets { get; set; } = new List<Timesheet>();
    public ICollection<BudgetAdjustment> RequestedBudgetAdjustments { get; set; } = new List<BudgetAdjustment>();
    public ICollection<BudgetAdjustment> ApprovedBudgetAdjustments { get; set; } = new List<BudgetAdjustment>();
    public ICollection<ProductivitySession> ProductivitySessions { get; set; } = new List<ProductivitySession>();
}
