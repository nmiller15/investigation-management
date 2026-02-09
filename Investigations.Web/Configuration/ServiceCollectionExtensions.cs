using Investigations.App.Auth;
using Investigations.App.Cases;
using Investigations.App.Users;
using Investigations.Infrastructure.Auth;
using Investigations.Models.Auth;
using Investigations.Models.Cases;
using Investigations.Models.Configuration;
using Investigations.Models.Users;
using Investigations.Web.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Investigations.Web.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtilities(this IServiceCollection services, IConfiguration configuration)
    {
        var connStrings = new ConnectionStrings(configuration);
        services.AddSingleton<IConnectionStrings, ConnectionStrings>();

        services.AddSingleton<IEmailSettings, EmailSettings>();
        services.AddSingleton<IPasswordHasher, AspNetPasswordHasher>();
        return services;
    }

    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICaseRepository, CaseRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, bool isDevelopment)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                    {
                        o.Cookie.Name = "investigations_auth_cookie";
                        o.Cookie.HttpOnly = true;
                        o.Cookie.SecurePolicy = isDevelopment
                            ? CookieSecurePolicy.None
                            : CookieSecurePolicy.Always;
                        o.SlidingExpiration = true;
                        o.ExpireTimeSpan = TimeSpan.FromHours(8);

                        o.LoginPath = "/Account/Login";
                        o.LogoutPath = "/Account/Logout";
                        o.AccessDeniedPath = "/Error/AccessDenied";
                    });
        // services.AddAuthentication("Cookies")
        //     .AddCookie("CookieAuthentication", options =>
        //     {
        //         options.Cookie.HttpOnly = true;
        //         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //         options.Cookie.SameSite = SameSiteMode.Lax;
        //
        //         options.SlidingExpiration = true;
        //         options.ExpireTimeSpan = TimeSpan.FromHours(8);
        //
        //         options.LoginPath = "/Account/Login";
        //         options.LogoutPath = "/Account/Logout";
        //     });
        return services;
    }
}
