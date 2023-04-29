using System.Net;
using System.Reflection;
using FluentValidation;
using HealthChecks.UI.Client;
using Menu.API.Extensions;
using Menu.API.Grpc;
using Menu.API.Services;
using Menu.API.Services.Contracts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var appName = "Menu.API";


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.WebHost.UseKestrel(options => {
    var ports = CustomStartupExtensions.GetDefinedPorts(builder.Configuration);
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
    .AddMongo(builder.Configuration)
    .AddCustomHealthChecks(builder.Configuration)
    .AddCustomServices(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomGrpc(builder.Configuration)
    .AddCustomMessaging(builder.Configuration);

builder.Host.UseSerilog(CustomStartupExtensions.CreateSerilogLogger(builder.Configuration));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    //TODO: Exception Handler
}
var pathBase = app.Configuration["PATH_BASE"];
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseSwagger()
    .UseSwaggerUI(c => {
        c.SwaggerEndpoint($"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json", "Menu.API V1");
    });
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapGrpcService<PizzaGrpcService>();
app.MapControllers();
app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

app.UseHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

//app.UseAuthorization();

app.Run();

public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}