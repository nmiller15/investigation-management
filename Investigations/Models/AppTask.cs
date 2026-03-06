namespace Investigations.Models;

public class AppTask : BaseAuditModel
{
    public int TaskKey { get; set; } = 0;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public Case Case { get; set; } = new();
    public User AssignedToUser { get; set; } = new();
    public User AssignedByUser { get; set; } = new();
    public DateTime? ReminderDate { get; set; }
    public DateTime? DueDate { get; set; }
}
