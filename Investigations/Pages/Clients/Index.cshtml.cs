using Investigations.Features.Auth;
using Investigations.Features.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Investigations.Pages.Clients;

public class ClientsIndexModel(ListClients.Handler listClients, CurrentUser currentUser) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SortColumn { get; set; } = "ClientName";

    [BindProperty(SupportsGet = true)]
    public string SortDirection { get; set; } = "ASC";

    public List<ClientViewModel> Clients { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        if (!currentUser.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login");
        }

        var response = await listClients.Handle(new ListClients.Query
        {
            SortColumn = SortColumn,
            SortDirection = SortDirection
        });

        if (!response.WasSuccessful)
        {
            TempData["Error"] = response.Message;
            return Page();
        }

        Clients.AddRange(response.Payload.Clients.Select(c => new ClientViewModel().From(c)));

        return Page();
    }
}

public class ClientViewModel
{
    public int ClientKey { get; set; } = 0;
    public string ClientName { get; set; } = string.Empty;
    public int PrimaryContactKey { get; set; } = 0;
    public string PrimaryContactName { get; set; } = string.Empty;

    public ClientViewModel From(ListClients.ClientRow clientRow)
    {
        return new ClientViewModel
        {
            ClientKey = clientRow.ClientKey,
            ClientName = clientRow.ClientName,
            PrimaryContactKey = clientRow.PrimaryContactKey,
            PrimaryContactName = $"{clientRow.PrimaryContactFirstName} {clientRow.PrimaryContactLastName}".Trim()
        };
    }
}

