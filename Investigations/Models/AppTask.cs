namespace Investigations.Models;

public class AppTask : BaseAuditModel
{
    public int TaskKey { get; set; } = 0;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public int CaseKey { get; set; } = -1;
    public string CaseNumber { get; set; } = string.Empty;
    public string CaseTypeShortDescription { get; set; } = string.Empty;
    public string CaseTypeDescription { get; set; } = string.Empty;
    public string SubjectFirstName { get; set; } = string.Empty;
    public string SubjectLastName { get; set; } = string.Empty;
    public int AssignedToUserKey { get; set; } = -1;
    public string AssignedToFirstName { get; set; } = string.Empty;
    public string AssignedToLastName { get; set; } = string.Empty;
    public DateTime? ReminderDate { get; set; }
    public DateTime? DueDate { get; set; }
}
