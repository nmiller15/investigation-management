using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Investigations.Models;
using Investigations.Features.Auth;
using Investigations.Features.Cases;

namespace Investigations.Pages.Cases;

public class CasesIndexModel(ListCases.Handler cases, CurrentUser currentUser) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SortColumn { get; set; } = "CaseNumber";

    [BindProperty(SupportsGet = true)]
    public string SortDirection { get; set; } = "ASC";

    [BindProperty(SupportsGet = true)]
    public bool ShowClosedCases { get; set; }

    public IList<CaseViewModel> Cases { get; set; } = [];

    public IList<CaseViewModel> ClosedCases { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        if (!currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        var response = await cases.Handle(new ListCases.Query()
        {
            SortColumn = SortColumn,
            SortDirection = SortDirection,
            ShowClosedCases = ShowClosedCases
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message ?? "An unknown error occurred, try again later.";
            return Page();
        }

        Cases = response.Payload.Cases
            .Where(c => c.IsActive)
            .Select(CaseViewModel.From).ToList();
        ClosedCases = response.Payload.Cases
            .Where(c => c.IsActive == false)
            .Select(CaseViewModel.From).ToList();

        return Page();
    }
}

public class CaseViewModel
{
    public int CaseKey { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int SubjectKey { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int ClientKey { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string DateOfReferral { get; set; } = string.Empty;
    public string CaseTypeCode { get; set; } = string.Empty;

    public static CaseViewModel From(ListCases.CaseRow c)
    {
        return new CaseViewModel
        {
            CaseKey = c.CaseKey,
            CaseNumber = c.CaseNumber,
            Status = c.IsActive ? "Active" : "Closed",
            SubjectKey = c.SubjectKey,
            SubjectName = c.SubjectFirstName + " " + c.SubjectLastName,
            ClientKey = c.ClientKey,
            ClientName = c.ClientName,
            DateOfReferral = c.DateOfReferral.ToString("MMMM dd, yyyy"),
            CaseTypeCode = c.CaseTypeCode.ToUpper(),
        };
    }
}
