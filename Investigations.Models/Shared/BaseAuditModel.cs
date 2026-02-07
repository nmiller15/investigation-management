namespace Investigations.Models.Shared;

public class BaseAuditModel
{
    public int InsertedByUserKey { get; set; } = -1;
    public string InsertedByFirstName { get; set; } = string.Empty;
    public string InsertedByLastName { get; set; } = string.Empty;
    public DateTime InsertedDateTime { get; set; }
    public int UpdatedByUserKey { get; set; } = -1;
    public string UpdatedByFirstName { get; set; } = string.Empty;
    public string UpdatedByLastName { get; set; } = string.Empty;
    public DateTime? UpdatedDateTime { get; set; }
}
