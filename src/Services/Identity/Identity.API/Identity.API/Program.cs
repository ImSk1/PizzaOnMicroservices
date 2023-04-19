
using Identity.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



builder.AddCustomConfiguration();
builder.AddCustomMvc();
builder.AddCustomDatabase();
builder.AddCustomIdentity();
builder.AddCustomIdentityServer();
builder.AddCustomAuthentication();
builder.AddCustomApplicationServices();

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

app.UseAuthentication();

app.MapDefaultControllerRoute();


app.Run();
