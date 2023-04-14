using Menu.API.Infrastructure;
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

        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration config, string appName)
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
    }
}
