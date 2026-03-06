namespace Investigations.Models;

public class Client : BaseAuditModel
{
    public int ClientKey { get; set; } = 0;
    public string ClientName { get; set; } = string.Empty;
    public Contact PrimaryContact { get; set; } = new();

    public bool IsNew => ClientKey == 0;
}
