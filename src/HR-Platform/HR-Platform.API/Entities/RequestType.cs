namespace HR_Platform.API.Entities;

public class RequestType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? DefaultSlaHours { get; set; }

    // Navigation properties
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
