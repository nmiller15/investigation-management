using Investigations.Models.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Investigations.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ITestService _testService;

    public string Text { get; set; } = string.Empty;

    public IndexModel(ITestService testService)
    {
        _testService = testService;
    }

    public void OnGet()
    {
        Log.Information("Handling Index page GET request.");
        var text = _testService.HelloWorld();
        Text = text;
    }
}
