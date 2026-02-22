using System.ComponentModel.DataAnnotations;

namespace Investigations.Models;

public class User() : BaseAuditModel
{
    // Code Type ROLE
    public enum Roles
    {
        Undefined = 0,
        [Display(Name = "System Administrator")]
        SystemAdministrator = 166,
        [Display(Name = "Account Owner")]
        AccountOwner = 167,
        [Display(Name = "Investigator")]
        Investigator = 168
    }

    public int UserKey { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? Birthdate { get; set; }
    public Roles Role { get; set; } = Roles.Undefined;
    public bool IsNew => UserKey == 0;
}
