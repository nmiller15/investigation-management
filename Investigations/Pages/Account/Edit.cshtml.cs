using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using Investigations.Models;
using Investigations.Features.Account;

namespace Investigations.Pages.Account;

public class EditModel(Edit.Handler edit) : PageModel, IValidatableObject
{
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
        var response = await edit.Handle(new Edit.Query
        {
            UserKey = UserKey
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "An error occurred while fetching user data.";
            return RedirectToPage("/Account/Index");
        }

        var result = response.Payload;

        if (!result.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        if (!result.CanEdit)
        {
            TempData["Error"] = "You can only edit your own account information.";
            return RedirectToPage("/Account/Index");
        }

        FirstName = result.FirstName;
        LastName = result.LastName;
        Email = result.Email;
        Birthdate = result.Birthdate;
        Role = result.Role;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }
    // {
    //     if (!_currentUser.IsAuthenticated)
    //         return RedirectToPage("/Account/Login");
    //
    //     if (_currentUser.UserKey != UserKey)
    //     {
    //         TempData["Error"] = "You can only edit your own account information.";
    //         Log.Warning("User {CurrentUserKey} attempted to edit account information for User {TargetUserKey}", _currentUser.UserKey, UserKey);
    //         return RedirectToPage("/Account/Index", new { userKey = _currentUser.UserKey });
    //     }
    //
    //     if (AnyPasswordFieldsFilled && !AllPasswordFieldsFilled)
    //     {
    //         TempData["Error"] = "To change your password, please fill in all password fields.";
    //         return Page();
    //     }
    //
    //     var user = await _userService.GetUser(UserKey);
    //     if (!user.WasSuccessful)
    //     {
    //         TempData["Error"] = user.Message ?? "An error occurred while fetching user data.";
    //         return Page();
    //     }
    //
    //     user.Payload.FirstName = FirstName;
    //     user.Payload.LastName = LastName;
    //     user.Payload.Email = Email;
    //     user.Payload.Birthdate = Birthdate;
    //
    //     var updateResult = await _userService.Save(user.Payload);
    //     if (!updateResult.WasSuccessful)
    //     {
    //         TempData["Error"] = "An error occurred while updating user data. Please try again later.";
    //         return Page();
    //     }
    //     else
    //     {
    //         TempData["Success"] = "Account information updated successfully.";
    //     }
    //
    //     if (!AllPasswordFieldsFilled)
    //         return RedirectToPage("/Account/Index");
    //
    //     var passwordCheck = await _authService.CheckPassword(UserKey, CurrentPassword!);
    //     if (!passwordCheck.WasSuccessful)
    //     {
    //         TempData["Error"] = "Current password is incorrect. Please try again.";
    //         return Page();
    //     }
    //
    //     var passwordUpdate = await _authService.UpdatePassword(UserKey, NewPassword!);
    //     if (!passwordUpdate.WasSuccessful)
    //     {
    //         TempData["Error"] = "An error occurred while updating the password. Please try again later.";
    //         return Page();
    //     }
    //
    //     TempData["Success"] = "Password updated successfully.";
    //     return RedirectToPage("/Account/Index");

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
