using System.Reflection;
using Investigations.Features.Auth;
using Serilog;

namespace Investigations.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtilities(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionStrings, ConnectionStrings>();
        services.AddSingleton<IEmailSettings, EmailSettings>();
        services.AddSingleton(new PasswordHasher());
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace != null
                    && t.Namespace.Contains(".Data.Repositories")
                    && t.Name.EndsWith("Repository"));

        foreach (var type in handlerTypes)
        {
            Log.Debug("Registering repository {HandlerType}", type.Name);
            services.AddScoped(type);
        }

        return services;
    }

    public static IServiceCollection AddFeatureHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace != null
                    && t.Namespace.Contains(".Features.")
                    && t.Name.EndsWith("Handler"));

        foreach (var type in handlerTypes)
        {
            Log.Debug("Registering feature handler {HandlerType}", type.FullName);
            services.AddScoped(type);
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        return services;
    }
}
