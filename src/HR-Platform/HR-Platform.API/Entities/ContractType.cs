namespace HR_Platform.API.Entities;

public class ContractType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
