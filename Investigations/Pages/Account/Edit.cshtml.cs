using System.ComponentModel.DataAnnotations;
using Investigations.Features.Account;
using Investigations.Features.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Pages.Account;

public class EditModel(EditAccount.Handler editAccount, ChangePassword.Handler changePassword, CurrentUser currentUser) : PageModel, IValidatableObject
{
    private readonly EditAccount.Handler _editAccount = editAccount;
    private readonly ChangePassword.Handler _changePassword = changePassword;
    private readonly CurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int UserKey { get; set; }

    [Required]
    [BindProperty]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [BindProperty]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public DateTime? Birthdate { get; set; }

    [BindProperty]
    public string? CurrentPassword { get; set; }

    [BindProperty]
    public string? NewPassword { get; set; }

    [BindProperty]
    public string? ConfirmPassword { get; set; }

    public bool AnyPasswordFieldsFilled => !string.IsNullOrEmpty(CurrentPassword) ||
                                                !string.IsNullOrEmpty(NewPassword);

    public bool AllPasswordFieldsFilled => !string.IsNullOrEmpty(CurrentPassword) &&
                                            !string.IsNullOrEmpty(NewPassword);

    public string Role { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanEditAccount(UserKey))
        {
            TempData["Error"] = "You can only edit your own account information.";
            return RedirectToPage("/Account/Profile", new { userKey = UserKey });
        }

        var response = await _editAccount.Handle(new EditAccount.Query
        {
            UserKey = UserKey
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "An error occurred while fetching user data.";
            return RedirectToPage("/Account/Index");
        }

        var result = response.Payload;

        FirstName = result.FirstName;
        LastName = result.LastName;
        Email = result.Email;
        Birthdate = result.Birthdate;
        Role = result.Role;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!_currentUser.CanEditAccount(UserKey))
        {
            Log.Warning("Unauthorized account edit attempt by user {UserKey} on account {TargetUserKey}", _currentUser.UserKey, UserKey);
            TempData["Error"] = "You can only edit your own account information.";
            return RedirectToPage("/Account/Profile", new { userKey = UserKey });
        }

        var response = await _editAccount.Handle(new EditAccount.Command
        {
            UserKey = UserKey,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Birthdate = Birthdate.GetValueOrDefault()
        });

        var result = response.Payload;

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "An error occurred while updating user data. Please try again later.";
            return RedirectToPage("/Account", new { userKey = UserKey });
        }

        TempData["Success"] = response.Message ?? "Account information updated successfully.";

        if (AnyPasswordFieldsFilled && !AllPasswordFieldsFilled)
        {
            TempData["Error"] = "To change your password, please fill in all password fields.";
            return Page();
        }

        if (AnyPasswordFieldsFilled && AllPasswordFieldsFilled)
        {
            var passwordResponse = await changePassword.Handle(new ChangePassword.Command
            {
                UserKey = UserKey,
                CurrentPassword = CurrentPassword!,
                NewPassword = NewPassword!
            });

            if (!passwordResponse.WasSuccessful)
            {
                TempData["Error"] = passwordResponse.Message ?? "An error occurred while updating the password. Please try again later.";
                return Page();
            }

            TempData["Success"] = passwordResponse.Message ?? "Password updated successfully.";
        }

        return RedirectToPage("/Account/Index", new { userKey = _currentUser.UserKey.GetValueOrDefault() });
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (AnyPasswordFieldsFilled)
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                results.Add(new ValidationResult("Current password is required when changing password.", ["CurrentPassword"]));
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                results.Add(new ValidationResult("New password is required when changing password.", ["NewPassword"]));
            }
            else
            {
                if (NewPassword.Length < 6)
                {
                    results.Add(new ValidationResult("Password must be at least 6 characters long.", ["NewPassword"]));
                }
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                results.Add(new ValidationResult("Please confirm your new password.", ["ConfirmPassword"]));
            }
            else if (!string.IsNullOrEmpty(NewPassword) && NewPassword != ConfirmPassword)
            {
                results.Add(new ValidationResult("Passwords do not match.", ["ConfirmPassword"]));
            }
        }

        return results;
    }
}
