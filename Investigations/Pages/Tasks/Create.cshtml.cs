using System.ComponentModel.DataAnnotations;
using Investigations.Features.Auth;
using Investigations.Features.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace Investigations.Pages.Tasks;

public class TaskCreateModel(CreateTask.Handler createTask, CurrentUser currentUser) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int? SourcedCaseKey { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Task name is required.")]
    [Display(Name = "Enter a short description of the task")]
    public string TaskName { get; set; } = string.Empty;

    [BindProperty]
    [Display(Name = "Enter a more detailed description of the task (optional)")]
    public string TaskDescription { get; set; } = string.Empty;

    [BindProperty]
    [Display(Name = "Select a due date for the task (optional)")]
    public DateTime? DueDate { get; set; }

    [BindProperty]
    [Display(Name = "Assign to case (optional)")]
    public int? SelectedCaseKey { get; set; }

    [BindProperty]
    [Display(Name = "Assign to user (optional)")]
    public int? SelectedUserKey { get; set; }

    public List<SelectListItem> AssignableUsers { get; set; } = [];

    public List<SelectListItem> AssignableCases { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        Log.Debug("OnGetAsync called with SourcedCaseKey: {SourcedCaseKey}", SourcedCaseKey);
        if (!currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login", new
            {
                returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString
            });

        SelectedCaseKey = SourcedCaseKey;
        SelectedUserKey = currentUser.UserKey;

        var response = await createTask.Handle(new CreateTask.Query
        {
            CanAssignToOtherUsers = currentUser.CanAssignTasksToOtherUsers(),
            CanAssignToAnyCase = currentUser.CanAssignTasksToAnyCase(),
            CurrentUserKey = currentUser.UserKey.GetValueOrDefault(),
            SourcedCaseKey = SourcedCaseKey > 0 ? SourcedCaseKey : null
        });

        if (!response.WasSuccessful)
        {
            TempData["ErrorMessage"] = response.Message ?? "An error occurred while preparing the task creation form.";
            return SourcedCaseKey.HasValue
                ? RedirectToPage("/Cases/Case", new { CaseKey = SourcedCaseKey })
                : RedirectToPage("/Index");
        }

        AssignableUsers = response.Payload.AssignableUsers.Select(u => new SelectListItem(ToSelectListItemText(u), u.UserKey.ToString())).ToList();
        AssignableCases = response.Payload.AssignableCases.Select(c => new SelectListItem($"{c.CaseNumber} {c.SubjectFirstName} {c.SubjectLastName}", c.CaseKey.ToString())).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login", new
            {
                returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString
            });

        var response = await createTask.Handle(new CreateTask.Command
        {
            CaseKey = SelectedCaseKey ?? SourcedCaseKey.GetValueOrDefault(),
            TaskName = TaskName,
            TaskDescription = TaskDescription,
            AssignedToUserKey = SelectedUserKey,
            DueDate = DueDate,
            InsertedByUserKey = currentUser.UserKey.GetValueOrDefault()
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "An error occurred while creating the task. Please try again.";
            return Page();
        }

        TempData["Success"] = "Task created successfully.";
        return RedirectToPage("/Cases/Case", new { CaseKey = SelectedCaseKey ?? SourcedCaseKey });
    }

    private static string ToSelectListItemText(CreateTask.AssignableUser user)
    {
        var fullName = $"{user.FirstName} {user.LastName}".Trim();
        return string.IsNullOrWhiteSpace(fullName) ? $"User {user.UserKey} - {user.Email}" : fullName;
    }
}
