namespace Investigations.Models;

public class Notification : BaseAuditModel
{
    public int NotificationKey { get; set; } = 0;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AssignedToUserKey { get; set; } = -1;
    public string AssignedToUserFirstName { get; set; } = string.Empty;
    public string AssignedToUserLastName { get; set; } = string.Empty;
    public bool IsDelayed { get; set; }
    public DateTime? DelayedUntilDatetime { get; set; }
}
