using Microsoft.OpenApi.Models;
using Serilog;

namespace Basket.API.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

            });
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PizzaOnContainers - Basket HTTP API",
                    Version = "v1"
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl =
                                new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                            TokenUrl =
                                new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                            Scopes = new Dictionary<string, string>() { { "basket", "Basket API" } }
                        }
                    }
                });
            });

            return services;
        }
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            //var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "Basket.API")
                .Enrich.FromLogContext()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication("Bearer").AddJwtBearer(options => {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "basket";
                options.TokenValidationParameters.ValidateAudience = false;
            });
            services.AddAuthorization(options => {
                options.AddPolicy("ApiScope", policy => {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "basket");
                });
            });

            return services;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        
            return services;
        }
    }
}
