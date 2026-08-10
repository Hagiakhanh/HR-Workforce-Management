namespace HR_Platform.API.Entities;

public class Contract
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public Guid ContractTypeId { get; set; }
    public Guid? TemplateId { get; set; }
    public string? DocumentTitle { get; set; }
    public DateOnly? EffectiveDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? BaseSalary { get; set; }
    public string? DocumentUrl { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual ContractType ContractType { get; set; } = null!;
    public virtual ContractTemplate? Template { get; set; }
    public virtual ICollection<ContractAllowance> ContractAllowances { get; set; } = new List<ContractAllowance>();
}
