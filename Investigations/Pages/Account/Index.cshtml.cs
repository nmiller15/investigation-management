using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Investigations.Features.Account;
using Investigations.Features.Auth;
namespace Investigations.Pages.Account;

public class AccountIndexModel(ListAccounts.Handler listAccounts, CurrentUser currentUser) : PageModel
{
    private readonly ListAccounts.Handler _listAccounts = listAccounts;
    private readonly CurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public string SortColumn { get; set; } = "LastName";

    [BindProperty(SupportsGet = true)]
    public string SortDirection { get; set; } = "ASC";

    public List<ListAccounts.Account> Accounts = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanViewAccountList())
            return RedirectToPage("/Account/Profile", new { userKey = _currentUser.UserKey.GetValueOrDefault() });

        var response = await _listAccounts.Handle(new ListAccounts.Query());

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "Unable to load account information. Please try again later.";
            return RedirectToPage("/");
        }

        var result = response.Payload;

        Accounts = result.Accounts;

        return Page();
    }
}
