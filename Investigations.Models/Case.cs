namespace Investigations.Models;

public class Case : BaseModelWithAudit
{
    public Guid CaseId { get; set; } = Guid.Empty;
    public string CaseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid SubjectId { get; set; } = Guid.Empty;
    public string SubjectFirstName { get; set; } = string.Empty;
    public string SubjectLastName { get; set; } = string.Empty;
    public Guid ClientId { get; set; } = Guid.Empty;
    public string ClientName { get; set; } = string.Empty;
    public DateTime? DateOfReferral { get; set; }
    public int CaseTypeCodeKey { get; set; } = 0;
    public string CaseTypeCode { get; set; } = string.Empty;
    public string CaseTypeShortDescription { get; set; } = string.Empty;
    public string CaseTypeDescription { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
}
