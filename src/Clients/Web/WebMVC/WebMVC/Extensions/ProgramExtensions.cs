using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Serilog;
using WebMVC.Configuration;
using WebMVC.Infrastructure;
using WebMVC.Services;
using WebMVC.Services.Contracts;

namespace WebMVC.Extensions
{
    public static class ProgramExtensions
    {
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration config)
        {
            var appName = "Identity.API";

            var seqServerUrl = config["SeqServerUrl"];
            var logstashUrl = config["LogstashgUrl"];

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq/" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://localhost:8080/" : logstashUrl, null)
                .ReadFrom.Configuration(config)
                .CreateLogger();

            return loggerConfig;
        }

        public static void AddCustomMvc(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions()
                .Configure<AppSettings>(builder.Configuration)
                .AddSession()
                .AddDistributedMemoryCache();

            builder.Services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = "eshop.webmvc";
            });
        }

        public static void AddHttpClientServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            builder.Services.AddHttpClient<IMenuService, MenuService>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        }

        public static void AddCustomAuthentication(this WebApplicationBuilder builder)
        {
            var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = builder.Configuration.GetValue<string>("CallBackUrl");
            var sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

            // Add Authentication services          

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = identityUrl.ToString();
                options.SignedOutRedirectUri = callBackUrl.ToString();
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("menu");
            });
        }
    }
}
