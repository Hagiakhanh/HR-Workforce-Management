namespace HR_Platform.API.Entities;

public class ContractAllowance
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ContractId { get; set; }
    public string AllowanceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    // Navigation properties
    public virtual Contract Contract { get; set; } = null!;
}
