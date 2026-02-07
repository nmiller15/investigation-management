using Investigations.Models.Users;
using static Investigations.Models.Users.User;

namespace Investigations.Models.Auth;

public interface ICurrentUser
{
    public int? UserKey { get; }
    bool IsAuthenticated { get; }
    string? Email { get; }
    Roles? Role { get; }
    UserIdentity? AsUserIdentity();

    // Explicit role checking methods
    bool IsSystemAdministrator();
    bool IsAccountOwner();
    bool IsInvestigator();
}
