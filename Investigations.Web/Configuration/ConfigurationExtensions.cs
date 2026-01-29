using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http;

namespace Investigations.Web.Configuration;

public static class ConfigurationExtensions
{
    public static void LoadConfigurationFiles(this WebApplicationBuilder builder)
    {
        var environment = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile(environment.IsDevelopment()
                ? $"investigations.{environment.EnvironmentName}.json"
                : "/configs/investigations.production.json",
                optional: false, reloadOnChange: true);
    }

    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var logLevel = builder.Environment.IsDevelopment()
            ? LogEventLevel.Debug
            : LogEventLevel.Information;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/investigations.log", rollingInterval: RollingInterval.Day)
            .WriteTo.Http(requestUri: config.GetSection("SerilogSettings")["Endpoint"],
                            queueLimitBytes: null,
                            httpClient: new CustomHttpClient(),
                            configuration: config,
                            restrictedToMinimumLevel: logLevel)
            .CreateLogger();
    }
}

public class CustomHttpClient : IHttpClient
{
    private readonly HttpClient httpClient;

    public CustomHttpClient() => httpClient = new HttpClient();

    public void Configure(IConfiguration configuration)
    {
        var key = configuration.GetSection("SerilogSettings")["ApiKey"];
        // Console.WriteLine($"Configuring CustomHttpClient with API Key. {key.Substring(0, 4)}****");
        httpClient.DefaultRequestHeaders.Add("X-Api-Key", key);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream, CancellationToken cancellationToken)
    {
        using var content = new StreamContent(contentStream);
        // Console.WriteLine(await content.ReadAsStringAsync());

        content.Headers.Add("Content-Type", "application/json");

        var response = await httpClient
            .PostAsync(requestUri, content, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }

    public void Dispose() => httpClient?.Dispose();
}
