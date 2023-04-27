using Menu.API.Infrastructure;
using Menu.API.Services;
using Menu.API.Services.Contracts;
using Menu.API.Settings;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace Menu.API.Extensions
{
    public static class CustomStartupExtensions
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

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var hcBuilder = services.AddHealthChecks();
            var mongoDbSettings = config.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            hcBuilder.AddMongoDb(mongoDbSettings.ConnectionString, mongoDatabaseName: mongoDbSettings.DatabaseName);

            services.AddHealthChecksUI().AddInMemoryStorage();
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration config)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            services.AddSingleton<IMongoDbContext>(serviceProvider =>
            {
                var mongoDbSettings = config.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                return new MenuAPIDbContext(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName);
            });
            services.AddSingleton<IBaseMongoRepository, MenuAPIRepository>();

            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PizzaOnContainers - Menu HTTP API",
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
                                new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
                            TokenUrl =
                                new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
                            Scopes = new Dictionary<string, string>() { { "menu", "Menu API" } }
                        }
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO: Implement Later
            throw new NotImplementedException();

        }
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            //var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, null)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMenuService, MenuService>();

            return services;
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication("Bearer").AddJwtBearer(options => {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "menu";
                options.TokenValidationParameters.ValidateAudience = false;
            });
            services.AddAuthorization(options => {
                options.AddPolicy("ApiScope", policy => {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "menu");
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomGrpc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            return services;
        }
        public static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 5101);
            var port = config.GetValue("PORT", 50001);
            return (port, grpcPort);
        }

    }
}
