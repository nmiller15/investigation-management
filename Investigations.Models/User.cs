namespace Investigations.Models;

public class User : BaseModelWithAudit
{
    public Guid UserId { get; set; } = Guid.Empty;
    public int UserNo { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public int RoleCodeKey { get; set; } = 0;
    public string RoleDescription { get; set; } = string.Empty;
}
