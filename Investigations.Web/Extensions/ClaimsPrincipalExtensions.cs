using System.Security.Claims;

namespace Investigations.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserKey(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        var claim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim, out var result) ? result : null;
    }

    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        var claim = principal.FindFirstValue(ClaimTypes.Email);
        return claim;
    }

    public static string? GetRole(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        var claim = principal.FindFirstValue(ClaimTypes.Role);
        return claim;
    }

    public static bool IsInRole(this ClaimsPrincipal principal, string role)
    {
        ArgumentNullException.ThrowIfNull(principal);
        return principal.GetRole() == role;
    }

    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        var isAuthed = principal?.Identity?.IsAuthenticated ?? false;
        return isAuthed;
    }
}
