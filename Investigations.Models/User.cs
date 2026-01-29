namespace Investigations.Models;

public class User : BaseAuditModel
{
    public int UserKey { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public int RoleCodeKey { get; set; } = 0;
    public string RoleDescription { get; set; } = string.Empty;
}
