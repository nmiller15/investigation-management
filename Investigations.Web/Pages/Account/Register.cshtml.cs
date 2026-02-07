using System.ComponentModel.DataAnnotations;
using Investigations.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    [Required]
    [EmailAddress]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    public void OnGetAsync()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _authService.Register(Email, Password);
        if (!result.WasSuccessful)
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
            return Page();
        }

        Log.Information("User registered successfully with email: {Email}", Email);

        var loginResult = await _authService.Login(Email, Password);
        if (!loginResult.WasSuccessful)
        {
            TempData["Success"] = loginResult.Message ?? "Login after registration failed.";
        }

        return RedirectToPage("/Account/Login");
    }
}
