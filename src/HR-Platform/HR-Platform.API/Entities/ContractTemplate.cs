namespace HR_Platform.API.Entities;

public class ContractTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TemplateName { get; set; } = string.Empty;
    public string? TemplateType { get; set; }
    public string? FileUrl { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
