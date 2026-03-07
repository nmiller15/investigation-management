using Investigations.Infrastructure.Data;
using Serilog;

namespace Investigations.Tests.Integration.Utilities;

public static class TestSetup
{
    public static void RegisterRepositories(IServiceCollection services)
    {
        var handlerTypes = typeof(TestEntry).Assembly.GetTypes()
            .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace != null
                    && t.Namespace.Contains(".Data.Repositories")
                    && t.Name.EndsWith("Repository"));

        foreach (var type in handlerTypes)
        {
            services.AddScoped(type);
        }
    }

    public static void RegisterHandlers(IServiceCollection services)
    {
        var handlerTypes = typeof(TestEntry).Assembly.GetTypes()
            .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace != null
                    && t.Namespace.Contains(".Features")
                    && t.Name.EndsWith("Handler"));

        foreach (var type in handlerTypes)
        {
            Console.WriteLine($"Registering handler: {type.FullName}");
            services.AddScoped(type);
        }
    }
}
