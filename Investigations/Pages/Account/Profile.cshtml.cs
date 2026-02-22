using Investigations.Features.Account;
using Investigations.Features.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Pages.Account;

public class ProfileModel(ViewAccount.Handler viewAccount, CurrentUser currentUser) : PageModel
{
    private readonly ViewAccount.Handler _viewAccount = viewAccount;
    private readonly CurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Birthdate { get; set; } = string.Empty;
    public string AccountCreatedOn { get; set; } = string.Empty;
    public string LastUpdatedOn { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanViewAccount(UserKey))
        {
            Log.Debug("Cannot view this account information for UserKey {UserKey}", UserKey);
            TempData["Error"] = "You cannot view this account information.";
            return RedirectToPage("/Index");
        }

        var response = await _viewAccount.Handle(new ViewAccount.Query { UserKey = UserKey });
        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "Unable to load account information. Please try again later.";
            return RedirectToPage("/Index");
        }

        var result = response.Payload;

        FirstName = result.FirstName;
        LastName = result.LastName;
        Email = result.Email;
        Birthdate = result.Birthdate;
        AccountCreatedOn = result.AccountCreatedOn;
        LastUpdatedOn = result.LastUpdatedOn;
        Role = result.Role;

        return Page();
    }

}
