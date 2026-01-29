namespace Investigations.Models;

public class Notification : BaseAuditModel
{
    public Guid NotificationId { get; set; } = Guid.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignedToUserId { get; set; } = Guid.Empty;
    public string AssignedToUserFirstName { get; set; } = string.Empty;
    public string AssignedToUserLastName { get; set; } = string.Empty;
    public bool IsDelayed { get; set; }
    public DateTime? DelayedUntilDatetime { get; set; }
}
