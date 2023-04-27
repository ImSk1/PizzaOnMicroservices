using System.IdentityModel.Tokens.Jwt;
using Menu.API.Grpc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.BFF.Configuration;
using Web.BFF.Infrastructure;
using Web.BFF.Services;
using Web.BFF.Services.Contracts;

namespace Web.BFF.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<UrlsConfig>(configuration.GetSection("urls"));

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shopping Aggregator for Web Clients",
                    Version = "v1",
                    Description = "Shopping Aggregator for Web Clients"
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),

                            Scopes = new Dictionary<string, string>()
                            {
                                { "webbff", "BFF Layer for Web Clients" }
                            }
                        }
                    }
                });

            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "webbff";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            return services;
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMenuService, MenuService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<JwtClientInterceptor>();
            services.AddGrpcClient<PizzaService.PizzaServiceClient>((services, options) =>
            {
                var menuApi = "http://localhost:5101";
                options.Address = new Uri(menuApi);
            })
                .AddInterceptor<JwtClientInterceptor>();
            return services;
        }
    }
}
