using Investigations.Models.Users;
using Investigations.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Investigations.Models.Auth;

namespace Investigations.Web.Pages.Account;

public class UserIndexModel(IUsersService usersService, ICurrentUser currentUser) : PageModel
{
    private readonly IUsersService _usersService = usersService;
    private readonly ICurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    public new User User { get; set; } = new();
    public bool CanEdit => _currentUser.IsAuthenticated && _currentUser.UserKey == UserKey;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        var userResult = await _usersService.GetUser(UserKey);
        if (!userResult.WasSuccessful)
        {
            TempData["Error"] = userResult.Message ?? "User not found.";
            return RedirectToPage("/Account/Index");
        }

        User = userResult.Payload;
        return Page();
    }
}