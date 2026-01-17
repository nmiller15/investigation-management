namespace Investigations.Models;

public class CaseNote : BaseModelWithAudit
{
    public Guid CaseNoteId { get; set; } = Guid.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid CaseId { get; set; } = Guid.Empty;
    public string CaseNumber { get; set; } = string.Empty;
}
