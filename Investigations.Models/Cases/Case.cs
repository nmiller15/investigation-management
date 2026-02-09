using Investigations.Models.Shared;

namespace Investigations.Models.Cases;

public class Case : BaseAuditModel
{
    public int CaseKey { get; set; } = 0;
    public string CaseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SubjectKey { get; set; } = -1;
    public string SubjectFirstName { get; set; } = string.Empty;
    public string SubjectLastName { get; set; } = string.Empty;
    public int ClientKey { get; set; } = -1;
    public string ClientName { get; set; } = string.Empty;
    public DateTime? DateOfReferral { get; set; }
    public int CaseTypeCodeKey { get; set; } = 0;
    public string CaseTypeCode { get; set; } = string.Empty;
    public string CaseTypeShortDescription { get; set; } = string.Empty;
    public string CaseTypeDescription { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;

    public bool IsNew => CaseKey == 0;
}
