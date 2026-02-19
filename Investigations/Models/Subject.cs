namespace Investigations.Models;

public class Subject : BaseAuditModel
{
    public Guid SubjectId { get; set; } = Guid.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int MaritalStatusCodeKey { get; set; } = 0;
    public string MaritalStatusCode { get; set; } = string.Empty;
    public string MaritalStatusDescription { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
}
