using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Investigations.Models.Auth;
using Investigations.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    [Required]
    [EmailAddress]
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; set; }


    public LoginModel(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

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

        var result = await _authService.Login(Email, Password);
        if (!result.WasSuccessful)
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Invalid login attempt.");
            return Page();
        }

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) // Match the 8-hour sliding expiration
        };

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Payload.UserKey.ToString()),
            new(ClaimTypes.Email, Email),
            new(ClaimTypes.Role, result.Payload.Role.ToString()),
            new(ClaimTypes.Name, Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);

        var userReq = await _userService.GetUser(result.Payload.UserKey);
        if (userReq.WasSuccessful &&
            (string.IsNullOrEmpty(userReq.Payload.FirstName) ||
             string.IsNullOrEmpty(userReq.Payload.LastName)))
        {
            TempData["Info"] = "Please complete your profile information.";
            return RedirectToPage("/Account/Edit", new { userKey = userReq.Payload.UserKey });
        }

        return RedirectToPage(returnUrl ?? "/Index");
    }
}
