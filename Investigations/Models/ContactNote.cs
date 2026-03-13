namespace Investigations.Models;

public class ContactNote : BaseAuditModel
{
    public int ContactKey { get; set; } = 0;
    public string Body { get; set; } = string.Empty;
    public bool IsPinned { get; set; } = false;
}
