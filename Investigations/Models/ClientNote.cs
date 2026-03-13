namespace Investigations.Models;

public class ClientNote : BaseAuditModel
{
    public int ClientKey { get; set; } = 0;
    public string Body { get; set; } = string.Empty;
    public bool IsPinned { get; set; } = false;
}
