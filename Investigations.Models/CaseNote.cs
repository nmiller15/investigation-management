using Investigations.Models.Shared;

namespace Investigations.Models;

public class CaseNote : BaseAuditModel
{
    public Guid CaseNoteId { get; set; } = Guid.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid CaseId { get; set; } = Guid.Empty;
    public string CaseNumber { get; set; } = string.Empty;
}
