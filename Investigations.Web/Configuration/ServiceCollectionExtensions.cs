using Investigations.App.Services;
using Investigations.Models.Interfaces.Services;

namespace Investigations.Web.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Add repository registrations here
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITestService, TestService>();
        return services;
    }
}
