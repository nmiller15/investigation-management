namespace Investigations.Models.Users;

public static class UserRolesExtensions
{
    public static string ToDisplayString(this User.Roles role)
    {
        return role switch
        {
            User.Roles.SystemAdministrator => "System Administrator",
            User.Roles.AccountOwner => "Account Owner",
            User.Roles.Investigator => "Investigator",
            User.Roles.Undefined => "Undefined",
            _ => "Unknown"
        };
    }
}
