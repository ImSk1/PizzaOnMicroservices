using Identity.API;
using Identity.API.Extensions;
using Serilog;
var appName = "Identity.API";
var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomApplicationServices();
builder.AddCustomAuthentication();
builder.AddCustomMvc();
builder.AddCustomMvc();
builder.AddCustomDatabase();
builder.AddCustomIdentity();
builder.AddCustomIdentityServer();

builder.Host.UseSerilog(ProgramExtensions.CreateSerilogLogger(builder.Configuration));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapDefaultControllerRoute();

try
{
    app.Logger.LogInformation("Seeding database ({ApplicationName})...", appName);

    using (var scope = app.Services.CreateScope())
    {
        await SeedData.SeedUsers(scope, app.Configuration, app.Logger);
    }
    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
    return 0;
}
catch (Exception e)
{
    app.Logger.LogCritical(e, "Host terminated unexpectedly ({ApplicationName})...", appName);
    return 1;
}
finally
{
    Serilog.Log.CloseAndFlush();
}
app.Run();