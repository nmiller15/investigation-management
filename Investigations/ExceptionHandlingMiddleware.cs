using Serilog;

namespace Investigations.Web;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred while processing the request.");
            Console.Error.WriteLine($"Exception caught: {ex.Message}");
            await _next(context);
        }
    }
}
