namespace Investigations.Models;

public class Client : BaseAuditModel
{
    public int ClientKey { get; set; } = 0;
    public string ClientName { get; set; } = string.Empty;
    public int PrimaryContactKey { get; set; } = -1;
    public string PrimaryContactFirstName { get; set; } = string.Empty;
    public string PrimaryContactLastName { get; set; } = string.Empty;

    public bool IsNew => ClientKey == 0;
}
