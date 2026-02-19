using Microsoft.AspNetCore.Authentication.Cookies;

namespace Investigations.Features.Auth;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<CurrentUser>();

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

        return services;
    }
}

