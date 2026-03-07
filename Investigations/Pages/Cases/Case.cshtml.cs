using Investigations.Features.Auth;
using Investigations.Features.Cases;
using Investigations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Pages.Cases;

public class CaseModel(ViewCase.Handler viewCase, CurrentUser currentUser) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int CaseKey { get; set; }

    public CaseViewModel Case { get; set; } = new();
    public List<TaskViewModel> Tasks { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        if (!currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        var response = await viewCase.Handle(new ViewCase.Query { CaseKey = CaseKey });
        if (!response.WasSuccessful)
        {
            TempData["ErrorMessage"] = response.Message ?? "An error occurred while loading the case.";
            return RedirectToPage("/Cases/Index");
        }

        Case = CaseViewModel.From(response.Payload.Case);
        Tasks = TaskViewModel.From(response.Payload.Tasks)
            .OrderByDescending(t => DateTime.TryParse(t.DueDate, out var dueDate) ? dueDate : DateTime.MinValue)
            .ToList();

        return Page();
    }

    public class CaseViewModel
    {
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Status => IsActive ? "Active" : "Closed";
        public int SubjectKey { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int ClientKey { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CaseTypeCode { get; set; } = string.Empty;
        public string CaseTypeDescription { get; set; } = string.Empty;
        public string DateOfReferral { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public int UpdatedByUserKey { get; set; }
        public string UpdatedByUser { get; set; } = string.Empty;
        public string UpdatedOnDateTime { get; set; } = string.Empty;
        public int InsertedByUserKey { get; set; }
        public string InsertedByUser { get; set; } = string.Empty;
        public string InsertedOnDateTime { get; set; } = string.Empty;

        public static CaseViewModel From(ViewCase.CaseRow caseRow)
        {
            return new CaseViewModel
            {
                CaseKey = caseRow.CaseKey,
                CaseNumber = caseRow.CaseNumber,
                IsActive = caseRow.IsActive,
                SubjectKey = caseRow.SubjectKey,
                SubjectName = caseRow.SubjectFirstName + " " + caseRow.SubjectLastName,
                ClientKey = caseRow.ClientKey,
                ClientName = caseRow.ClientName,
                CaseTypeCode = caseRow.CaseTypeCode,
                CaseTypeDescription = caseRow.CaseTypeDescription,
                DateOfReferral = caseRow.DateOfReferral.ToString("MMMM dd, yyyy"),
                Synopsis = caseRow.Synopsis,
                UpdatedByUserKey = caseRow.UpdatedByUserKey,
                UpdatedByUser = caseRow.UpdatedByFirstName + " " + caseRow.UpdatedByLastName,
                UpdatedOnDateTime = caseRow.UpdatedOnDateTime > DateTime.MinValue
                    ? caseRow.UpdatedOnDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    : string.Empty,
                InsertedByUserKey = caseRow.InsertedByUserKey,
                InsertedByUser = caseRow.InsertedByFirstName + " " + caseRow.InsertedByLastName,
                InsertedOnDateTime = caseRow.InsertedOnDateTime > DateTime.MinValue
                    ? caseRow.InsertedOnDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    : string.Empty,
            };
        }
    }

    public class TaskViewModel
    {
        public int TaskKey { get; set; }
        public bool IsCompleted { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public int AssignedToUserKey { get; set; }
        public string AssignedToUser { get; set; } = string.Empty;

        public static List<TaskViewModel> From(List<ViewCase.TaskRow> taskRows)
        {
            return taskRows.Select(taskRow => new TaskViewModel
            {
                TaskKey = taskRow.TaskKey,
                IsCompleted = taskRow.IsCompleted,
                TaskName = taskRow.TaskName,
                TaskDescription = taskRow.TaskDescription,
                DueDate = taskRow.DueDate.HasValue
                    ? taskRow.DueDate.Value.ToString("MMMM dd, yyyy")
                    : "No due date",
                AssignedToUserKey = taskRow.AssignedToUserKey,
                AssignedToUser = taskRow.AssignedToFirstName + " " + taskRow.AssignedToLastName
            }).ToList();
        }
    }
}
