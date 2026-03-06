namespace Investigations.Models;

public class Subject : BaseAuditModel
{
    public enum Genders
    {
        Male = 198,
        Female = 199,
        OtherNonBinary = 200,
        Undisclosed = 201,
    }

    public enum MaritalStatuses
    {
        Unknown = 192,
        Single = 193,
        Married = 194,
        Divorced = 195,
        Widowed = 196,
        Separated = 197
    }

    public int SubjectKey { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public MaritalStatuses MaritalStatus { get; set; } = MaritalStatuses.Unknown;
    public Genders Gender { get; set; } = Genders.Undisclosed;

    public bool IsNew => SubjectKey == 0;
}
