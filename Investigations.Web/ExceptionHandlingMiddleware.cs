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
            Log.Error(ex.Message);
            Console.Error.WriteLine($"Exception caught: {ex.Message}");
            await _next(context);
        }
    }
}
