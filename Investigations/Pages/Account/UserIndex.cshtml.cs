using Investigations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Pages.Account;

public class UserIndexModel() : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    public new User User { get; set; } = new();

    public bool CanEdit => true;
    //_currentUser.IsAuthenticated && _currentUser.UserKey == UserKey;

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
        // if (!_currentUser.IsAuthenticated)
        //     return RedirectToPage("/Account/Login");
        //
        // var userResult = await _userService.GetUser(UserKey);
        // if (!userResult.WasSuccessful)
        // {
        //     TempData["Error"] = userResult.Message ?? "User not found.";
        //     return RedirectToPage("/Account/Index");
        // }
        //
        // User = userResult.Payload;
        // return Page();
    }
}
