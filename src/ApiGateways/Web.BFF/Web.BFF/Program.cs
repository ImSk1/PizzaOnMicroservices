using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Web.BFF.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddCustomConfiguration();
builder.WebHost.UseKestrel(options => {
    var port = ProgramExtensions.GetDefinedPort(builder.Configuration);
    options.Listen(IPAddress.Any, port, listenOptions => {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
    
});
// Add services to the container.
builder.Services
    .AddCustomMvc(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddApplicationServices()
    .AddGrpcServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Purchase BFF V1");

    c.OAuthClientId("webbff");
    c.OAuthClientSecret(string.Empty);
    c.OAuthRealm(string.Empty);
    c.OAuthAppName("bff Swagger UI");
});

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapControllers();




app.Run();
