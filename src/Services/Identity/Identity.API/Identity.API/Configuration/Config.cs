using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API.Configuration
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("menu", "Menu Service"),
                new ApiResource("basket", "Basket Service"),
                new ApiResource("webbff", "Web Bff"),


            };
        }

        // ApiScope is used to protect the API 
        //The effect is the same as that of API resources in IdentityServer 3.x
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("menu", "Menu Service"),
                new ApiScope("basket", "Basket Service"),
                new ApiScope("webbff", "Web Bff"),
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var redirectUri = configuration["MVCClient"];
            return new List<Client>
            {
                // MVC Client
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Code, // This is the line that should include the "authorization_code" grant type

                    RedirectUris = { "http://localhost:5220/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5220/signout-callback-oidc" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "menu",
                        "basket",
                        "webbff"
                    },

                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                new Client
                {
                    ClientId = "webbff",
                    ClientName = "bff Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["WebBffUrl"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["WebBffUrl"]}/swagger/" },

                    AllowedScopes =
                    {
                        "webbff",
                        "basket"
                    }
                },
            };
        }
    }
}