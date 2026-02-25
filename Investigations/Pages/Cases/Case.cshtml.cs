using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Investigations.Models;
using Investigations.Features.Auth;
using Investigations.Features.Cases;

namespace Investigations.Pages.Cases;

public class CaseModel(ViewCase.Handler viewCase, CurrentUser currentUser) : PageModel
{
    private readonly ViewCase.Handler _viewCase = viewCase;
    private readonly CurrentUser _currentUser = currentUser;

    [BindProperty(SupportsGet = true)]
    public int CaseKey { get; set; }

    public Case Case { get; set; } = new Case();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!currentUser.IsAuthenticated)
            return RedirectToPage("/Account/Login");

        var result = await _viewCase.Handle(new ViewCase.Query { CaseKey = CaseKey });

        if (!result.WasSuccessful)
        {
            TempData["Error"] = result.Message ?? "An unknown error occurred, try again later.";
            return RedirectToPage("/Cases/Index");
        }

        Case = result.Payload.Case;

        return Page();
    }
}
