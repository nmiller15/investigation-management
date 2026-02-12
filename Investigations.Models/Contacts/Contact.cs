using Investigations.Models.Shared;

namespace Investigations.Models.Contacts;

public class Contact : BaseAuditModel
{
    public int ContactKey { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string WorkPhone { get; set; } = string.Empty;
    public string HomePhone { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public bool IsNew => ContactKey == 0;
}
