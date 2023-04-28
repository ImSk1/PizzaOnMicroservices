using System.Net;
using System.Reflection;
using Basket.API.Extensions;
using Basket.API.Services;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var appName = "Basket.API";
var builder = WebApplication.CreateBuilder(args);
builder.AddCustomConfiguration();
builder.WebHost.UseKestrel(options => {
    var ports = ProgramExtensions.GetDefinedPorts(builder.Configuration);
    options.Listen(IPAddress.Any, ports.httpPort, listenOptions => {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions => {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

});
builder.Services
    .AddValidatorsFromAssembly(Assembly.Load(Namespace), ServiceLifetime.Scoped)
    .AddCustomMVC(builder.Configuration)
    .AddSwagger(builder.Configuration)
    .AddCustomServices(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddRedisCache(builder.Configuration)
    .AddCustomHealthCheck(builder.Configuration)
    .AddCustomGrpc(builder.Configuration);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger()
    .UseSwaggerUI(c => {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Menu.API V1");
    });
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapGrpcService<BasketGrpcService>();

app.MapControllers();

app.UseHttpsRedirection();


app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});
app.UseHealthChecksUI(config =>
{
    config.UIPath = "/hc-ui";

});





app.Run();
public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}