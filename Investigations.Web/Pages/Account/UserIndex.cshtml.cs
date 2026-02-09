using Investigations.Models.Auth;
using Investigations.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Web.Pages.Account;

public class UserIndexModel(IUserService userService, ICurrentUser currentUser) : PageModel
{
    private readonly IUserService _userService = userService;
    private readonly ICurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    public new User User { get; set; } = new();
    public bool CanEdit => _currentUser.IsAuthenticated && _currentUser.UserKey == UserKey;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        var userResult = await _userService.GetUser(UserKey);
        if (!userResult.WasSuccessful)
        {
            TempData["Error"] = userResult.Message ?? "User not found.";
            return RedirectToPage("/Account/Index");
        }

        User = userResult.Payload;
        return Page();
    }
}
