namespace HR_Platform.API.Entities;

public class Department
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public string DeptCode { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
    public Guid? ParentDeptId { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? WorkLocationId { get; set; }
    public string? CostCenter { get; set; }
    public string? Location { get; set; }
    public DateOnly? EffectiveDate { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual Department? ParentDept { get; set; }
    public virtual Employee? Manager { get; set; }
    public virtual WorkLocation? WorkLocation { get; set; }
    public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    public virtual ICollection<EmploymentRecord> EmploymentRecords { get; set; } = new List<EmploymentRecord>();
}
