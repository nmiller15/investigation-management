using Investigations.Models;
using Investigations.Models.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IUsersService _usersService;

    public List<string> ErrorMessages { get; set; } = new();
    public List<string> UserEmails { get; set; } = new();
    public User KeyUser { get; set; } = new();
    public User EmailUser { get; set; } = new();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public IndexModel(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task OnGetAsync()
    {
        Log.Information("Handling Index page GET request.");
        var usersResponse = await _usersService.GetUsers();
        if (!usersResponse.WasSuccessful)
            ErrorMessages.Add(usersResponse.Message ?? "An error occurred while retrieving users.");
        else
            UserEmails = usersResponse.Payload.Select(u => u.Email).ToList();

        var userByKeyResponse = await _usersService.GetUser(100);
        if (!userByKeyResponse.WasSuccessful)
            ErrorMessages.Add(userByKeyResponse.Message ?? "An error occurred while retrieving user by key.");
        else
            KeyUser = userByKeyResponse.Payload;


        var userByEmailResponse = await _usersService.GetUserByEmail("admin@nolanmiller.me");
        if (!userByEmailResponse.WasSuccessful)
            ErrorMessages.Add(userByEmailResponse.Message ?? "An error occurred while retrieving user by email.");
        else
            EmailUser = userByEmailResponse.Payload;
    }
}
