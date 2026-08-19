using System.Reflection;
using BBV.HR.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace BBV.HR.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<Capability> Capabilities => Set<Capability>();
    public DbSet<EmployeeCapability> EmployeeCapabilities => Set<EmployeeCapability>();
    public DbSet<ProjectRequiredCapability> ProjectRequiredCapabilities => Set<ProjectRequiredCapability>();
    public DbSet<Timesheet> Timesheets => Set<Timesheet>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    public DbSet<BudgetAdjustment> BudgetAdjustments => Set<BudgetAdjustment>();
    public DbSet<ProductivitySession> ProductivitySessions => Set<ProductivitySession>();
    public DbSet<ProductivityAppLog> ProductivityAppLogs => Set<ProductivityAppLog>();
    public DbSet<ProductivityWebsiteLog> ProductivityWebsiteLogs => Set<ProductivityWebsiteLog>();
    public DbSet<ProductivityScreenshot> ProductivityScreenshots => Set<ProductivityScreenshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
