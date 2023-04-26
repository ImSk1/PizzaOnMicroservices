using System.Reflection;
using Basket.API.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;

var appName = "Basket.API";
var builder = WebApplication.CreateBuilder(args);
builder.AddCustomConfiguration();

builder.Services
    .AddValidatorsFromAssembly(Assembly.Load(Namespace), ServiceLifetime.Scoped)
    .AddCustomMVC(builder.Configuration)
    .AddSwagger(builder.Configuration)
    .AddCustomServices(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddRedisCache(builder.Configuration);

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

app.MapControllers();

app.UseHttpsRedirection();


app.Run();
public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}