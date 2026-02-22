using Investigations.Features.Account;
using Investigations.Features.Auth;
using Investigations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Pages.Account;

public class RoleModel(ChangeRole.Handler changeRole, CurrentUser currentUser) : PageModel
{
    private readonly ChangeRole.Handler _changeRole = changeRole;
    private readonly CurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    [BindProperty]
    public User.Roles Role { get; set; }

    public IActionResult OnGet()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanUpdateRole())
        {
            TempData["Error"] = "You do not have permission to update user roles.";
            Log.Warning("User with UserKey {UserKey} attempted to access role management without permission.", _currentUser.UserKey);
            return RedirectToPage("/Index");
        }

        Role = _currentUser.Role.GetValueOrDefault();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanUpdateRole())
        {
            TempData["Error"] = "You do not have permission to update user roles.";
            Log.Warning("User with UserKey {UserKey} attempted to update a role without permission.", _currentUser.UserKey);
            return RedirectToPage("/Index");
        }

        Log.Debug("Updating role for UserKey {UserKey} to {Role}", UserKey, Role);

        var result = await _changeRole.Handle(new ChangeRole.Command
        {
            UserKey = UserKey,
            Role = Role
        });

        if (!result.WasSuccessful)
        {
            TempData["Error"] = result.Message ?? "Unable to update user role. Please try again later.";
            return Page();
        }

        return RedirectToPage("/Account/Index");
    }
}
