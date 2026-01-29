namespace Investigations.Models;

public class _Task : BaseAuditModel
{
    public Guid TaskId { get; set; } = Guid.Empty;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public Guid CaseId { get; set; } = Guid.Empty;
    public string CaseNumber { get; set; } = string.Empty;
    public string CaseTypeShortDescription { get; set; } = string.Empty;
    public string CaseTypeDescription { get; set; } = string.Empty;
    public string SubjectFirstName { get; set; } = string.Empty;
    public string SubjectLastName { get; set; } = string.Empty;
    public Guid AssignedToUserId { get; set; } = Guid.Empty;
    public string AssignedToFirstName { get; set; } = string.Empty;
    public string AssignedToLastName { get; set; } = string.Empty;
    public DateTime? ReminderDate { get; set; }
    public DateTime? DueDate { get; set; }
}
