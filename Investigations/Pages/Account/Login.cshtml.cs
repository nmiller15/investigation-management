using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Investigations.Features.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Pages.Account;

public class LoginModel(Login.Handler login) : PageModel
{
    private readonly Login.Handler _login = login;

    [Required]
    [EmailAddress]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; set; }

    public void OnGetAsync()
    {
        // If already authenticated, redirect to return URL or home
        if (User.Identity?.IsAuthenticated == true)
        {
            RedirectToPage("/Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return Page();

        var response = await _login.Handle(new Login.Command
        {
            Email = Email,
            Password = Password
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "Login failed.";
            return Page();
        }

        var result = response.Payload;

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) // Match the 8-hour sliding expiration
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

        if (result.UserInformationIsComplete)
        {
            TempData["Info"] = "Please complete your profile information.";
            return RedirectToPage("/Account/Edit", new { userKey = result.UserKey });
        }

        return RedirectToPage(returnUrl ?? "/Index");
    }
}
