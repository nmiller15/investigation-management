namespace Investigations.Models;

public class Case : BaseAuditModel
{
    public enum Types
    {
        Background = 169,
        Civil = 170,
        Collection = 171,
        Criminal = 172,
        Custody = 173,
        Death = 174,
        Divorce = 175,
        Domestic = 176,
        Employer = 177,
        Fraud = 178,
        Homicide = 179,
        Industrial = 180,
        Infidelity = 181,
        Insurance = 182,
        Locate = 183,
        MissingPerson = 184,
        OfficerInvolvedShooting = 185,
        Political = 186,
        ProcessService = 187,
        Protection = 188,
        Stalking = 189,
        Sweep = 190,
        TrafficCrash = 191,
        UseOfForce = 192,
    }

    public int CaseKey { get; set; } = 0;
    public string CaseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Subject Subject { get; set; } = new();
    public Client Client { get; set; } = new();
    public DateTime? DateOfReferral { get; set; }
    public Types Type { get; set; } = Types.Background;
    public string Synopsis { get; set; } = string.Empty;

    public bool IsNew => CaseKey == 0;
}
