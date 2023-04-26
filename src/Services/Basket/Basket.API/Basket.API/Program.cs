using Basket.API.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;

var appName = "Basket.API";
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCustomMVC(builder.Configuration)
    .AddSwagger(builder.Configuration)
    .AddCustomServices(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration);

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
