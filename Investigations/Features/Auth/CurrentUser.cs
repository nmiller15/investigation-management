using Investigations.Models;
using Investigations.Models.Auth;

namespace Investigations.Features.Auth;

public sealed class CurrentUser(IHttpContextAccessor http)
{
    private readonly IHttpContextAccessor _http = http;

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.IsAuthenticated() ?? false;

    public int? UserKey =>
        _http.HttpContext?.User?.GetUserKey();

    public string? Email =>
        _http.HttpContext?.User?.GetEmail();

    public User.Roles? Role =>
        Enum.TryParse<User.Roles>(
            _http.HttpContext?.User?.GetRole(),
            out var role) ? role : null;

    public UserIdentity? AsUserIdentity()
    {
        var principal = _http.HttpContext?.User;
        return principal != null ? UserIdentity.FromClaimsPrincipal(principal) : null;
    }

    // Explicit role checking - no hierarchy assumptions
    public bool IsSystemAdministrator() => Role == User.Roles.SystemAdministrator;
    public bool IsAccountOwner() => Role == User.Roles.AccountOwner;
    public bool IsInvestigator() => Role == User.Roles.Investigator;
}
