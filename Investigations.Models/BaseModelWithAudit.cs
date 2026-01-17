namespace Investigations.Models;

public class BaseModelWithAudit
{
    public Guid InsertedByUserId { get; set; } = Guid.Empty;
    public string InsertedByFirstName { get; set; } = string.Empty;
    public string InsertedByLastName { get; set; } = string.Empty;
    public DateTime InsertedDateTime { get; set; }
    public Guid UpdatedByUserId { get; set; } = Guid.Empty;
    public string UpdatedByFirstName { get; set; } = string.Empty;
    public string UpdatedByLastName { get; set; } = string.Empty;
    public DateTime? UpdatedDateTime { get; set; }
}
