using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Investigations.Features.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Pages.Account;

public class RegisterModel(Register.Handler register) : PageModel
{
    private readonly Register.Handler _register = register;

    [Required]
    [EmailAddress]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public void OnGetAsync()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var response = await _register.Handle(new Register.Command
        {
            Email = Email,
            Password = Password
        });

        if (!response.WasSuccessful)
        {
            ModelState.AddModelError(string.Empty, response.Message ?? "Registration failed.");
            return Page();
        }

        var result = response.Payload;

        Log.Information("User registered successfully with email: {Email}", Email);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.NameIdentifierClaimValue),
            new(ClaimTypes.Email, result.EmailClaimValue),
            new(ClaimTypes.Role, result.RoleClaimValue),
            new(ClaimTypes.Name, result.NameClaimValue)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);

        return RedirectToPage("/Account/Edit", new { userKey = result.UserKey });
    }
}
