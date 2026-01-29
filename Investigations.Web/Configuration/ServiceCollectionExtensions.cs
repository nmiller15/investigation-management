
using Investigations.App.Users;
using Investigations.Infrastructure.Data;
using Investigations.Models.Interfaces;
using Investigations.Models.Interfaces.Repositories;
using Investigations.Models.Interfaces.Services;

namespace Investigations.Web.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtilities(this IServiceCollection services, IConfiguration configuration)
    {
        var connStrings = new ConnectionStrings(configuration);
        services.AddSingleton<IConnectionStrings, ConnectionStrings>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        return services;
    }
}
