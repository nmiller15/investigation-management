using Investigations.Features.Account;
using Investigations.Features.Auth;
using Investigations.Models;
using Investigations.Models.Users;
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

    public string Name { get; set; } = string.Empty;

    [BindProperty]
    public User.Roles Role { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanUpdateRole())
        {
            TempData["Error"] = "You do not have permission to update user roles.";
            Log.Warning("User with UserKey {UserKey} attempted to access role management without permission.", _currentUser.UserKey);
            return RedirectToPage("/Index");
        }

        var response = await _changeRole.Handle(new ChangeRole.Query { UserKey = UserKey });
        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "Unable to retrieve user to edit. Please try again later.";
            return RedirectToPage("/Account/Index");
        }

        Role = response.Payload.Role;
        Name = response.Payload.Name;

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


        var result = await _changeRole.Handle(new ChangeRole.Command { UserKey = UserKey, Role = Role });

        if (!result.WasSuccessful)
        {
            TempData["Error"] = result.Message ?? "Unable to update user role. Please try again later.";
            return Page();
        }

        TempData["Success"] = $"You have successfully updated the user role to {Role.ToDisplayString()}.";

        return RedirectToPage("/Account/Index");
    }
}
