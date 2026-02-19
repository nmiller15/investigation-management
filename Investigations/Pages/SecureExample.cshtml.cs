using Investigations.Features.Auth;
using Investigations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Pages;

public class SecureExampleModel(CurrentUser currentUser) : PageModel
{
    private readonly CurrentUser _currentUser = currentUser;

    // Page data
    public string CurrentUserInfo { get; set; } = string.Empty;
    public List<User> Users { get; set; } = new();
    public List<string> Messages { get; set; } = new();

    // UI Role Properties for conditional rendering
    public bool CanViewAdminPanel => _currentUser.IsSystemAdministrator();
    public bool CanManageUsers => _currentUser.IsAccountOwner() || _currentUser.IsSystemAdministrator();
    public bool CanViewCases => _currentUser.IsInvestigator() ||
                                _currentUser.IsAccountOwner() ||
                                _currentUser.IsSystemAdministrator();

    public async Task<IActionResult> OnGetAsync()
    {
        // Set current user info for display
        CurrentUserInfo = $"User: {_currentUser.Email} (ID: {_currentUser.UserKey}, Role: {_currentUser.Role})";

        // Example 1: Admin-only functionality
        await DemonstrateAdminOnlyAccess();

        // Example 2: Multi-role access
        await DemonstrateMultiRoleAccess();

        // Example 3: Role-based UI properties
        DemonstrateRoleBasedUI();

        return Page();
    }

    private async Task DemonstrateAdminOnlyAccess()
    {
        // Explicit role check - only System Administrators
        if (!_currentUser.IsSystemAdministrator())
        {
            Messages.Add("Admin access denied: Only System Administrators can view all users");
            return;
        }

        Messages.Add($"Admin access granted:");
    }

    private async Task DemonstrateMultiRoleAccess()
    {
        // Explicit role combination checking - no hierarchy assumptions
        if (!_currentUser.IsInvestigator() &&
            !_currentUser.IsAccountOwner() &&
            !_currentUser.IsSystemAdministrator())
        {
            Messages.Add("Multi-role access denied: Requires Investigator, AccountOwner, or SystemAdministrator role");
            return;
        }

        // Example of accessing case-related data (service would be ICasesService in real implementation)
        Messages.Add("Multi-role access granted: Can access investigation data");

        // Service call example (using UsersService as example)
        Messages.Add($"Current user profile: {_currentUser.UserKey} - {_currentUser.Email}");
    }

    private void DemonstrateRoleBasedUI()
    {
        // Demonstrate role-based UI properties
        Messages.Add($"UI Features Available:");
        Messages.Add($"- Can View Admin Panel: {CanViewAdminPanel}");
        Messages.Add($"- Can Manage Users: {CanManageUsers}");
        Messages.Add($"- Can View Cases: {CanViewCases}");
    }

    public async Task<IActionResult> OnPostAdminActionAsync()
    {
        // Example POST handler with explicit role checking
        if (!_currentUser.IsSystemAdministrator())
            return Forbid();

        // Admin action logic here
        Messages.Add($"Admin action performed by {_currentUser.Email}");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCaseActionAsync()
    {
        // Multi-role POST handler
        if (!_currentUser.IsInvestigator() &&
            !_currentUser.IsAccountOwner() &&
            !_currentUser.IsSystemAdministrator())
            return Forbid();

        // Case-related action logic here
        Messages.Add($"Case action performed by {_currentUser.Email}");
        return RedirectToPage();
    }
}
