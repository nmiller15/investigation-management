using Investigations.Models.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Pages;

public class IndexModel : PageModel
{
    public async Task OnGetAsync()
    {
        Log.Information("Handling Index page GET request.");
    }
}
