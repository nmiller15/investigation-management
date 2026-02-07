using System.Security.Claims;
using Investigations.Models.Users;
using static Investigations.Models.Users.User;

namespace Investigations.Models.Auth;

public class UserIdentity
{
    public string Email { get; set; }
    public int UserKey { get; set; }
    public Roles Role { get; set; }

    public UserIdentity(User user)
    {
        Email = user.Email;
        UserKey = user.UserKey;
        Role = user.Role;
    }

    public UserIdentity(ClaimsPrincipal principal)
    {
        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        Email = emailClaim?.Value ?? string.Empty;

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        UserKey = int.TryParse(userIdClaim?.Value, out var userId) ? userId : -1;

        var roleClaim = principal.FindFirst(ClaimTypes.Role);
        Role = Enum.TryParse<Roles>(roleClaim?.Value, out var role) ? role : Roles.Undefined;
    }

    public static UserIdentity? FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
            return null;

        return new UserIdentity(principal);
    }
}
