using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Investigations.Models;
namespace Investigations.Pages.Account;

public class AccountIndexModel() : PageModel
{
    public bool IsAuthenticated => true; //_currentUser.IsAuthenticated;
    public bool CanViewAccountList => true; //_currentUser.IsAccountOwner() ||
                                            // _currentUser.IsSystemAdministrator();

    [BindProperty(SupportsGet = true)]
    public string SortColumn { get; set; } = "LastName";

    [BindProperty(SupportsGet = true)]
    public string SortDirection { get; set; } = "ASC";

    public List<User> Users = new();

    public async Task<IActionResult> OnGetAsync()
    {
        // if (!CanViewAccountList)
        //     return RedirectToPage("/Account/{userKey}/", new { userKey = _currentUser.UserKey });
        //
        // var usersResult = await _userService.GetUsers();
        // if (!usersResult.WasSuccessful)
        // {
        //     TempData["Error"] = "Unable to load accounts. Please try again later.";
        //     return Page();
        // }
        //
        // var users = usersResult.Payload;
        //
        // Users = SortColumn.ToLower() switch
        // {
        //     "lastname" => SortDirection == "ASC"
        //         ? users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList()
        //         : users.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName).ToList(),
        //     "email" => SortDirection == "ASC"
        //         ? users.OrderBy(u => u.Email).ToList()
        //         : users.OrderByDescending(u => u.Email).ToList(),
        //     "role" => SortDirection == "ASC"
        //         ? users.OrderBy(u => u.Role).ToList()
        //         : users.OrderByDescending(u => u.Role).ToList(),
        //     _ => users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList()
        // };

        return Page();
    }
}
