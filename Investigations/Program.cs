using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Investigations.Configuration;
using Investigations.Web;
using Investigations.Features.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.LoadConfigurationFiles();
builder.ConfigureLogging();

builder.Services.AddUtilities();
builder.Services.AddCookieAuthentication(builder.Environment.IsDevelopment());
builder.Services.AddUserContext();

builder.Services.AddRepositories();
builder.Services.AddFeatureHandlers();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseRouting();

Log.Information("Configuring authentication middleware...");
Log.Information("Default authentication scheme: {Scheme}", CookieAuthenticationDefaults.AuthenticationScheme);
app.UseAuthentication();
Log.Information("Authentication middleware configured");
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

Log.Information("Starting web application...");

app.Run();

Log.Information("Closing sesison...");

Log.CloseAndFlush();
