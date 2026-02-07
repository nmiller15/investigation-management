using Investigations.Models.Shared;

namespace Investigations.Models;

public class Contact : BaseAuditModel
{
    public Guid ContactId { get; set; } = Guid.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string WorkPhone { get; set; } = string.Empty;
    public string HomePhone { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
