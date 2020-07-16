using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.AccessControl
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources = new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };


        public static IEnumerable<ApiResource> Apis = new ApiResource[]
        {
            new ApiResource("mubbi-service", "Mubbi Marketplace API", new [] { JwtClaimTypes.Email, JwtClaimTypes.Name, JwtClaimTypes.Role }),
        };

        public static IEnumerable<Client> Clients = new Client[]
        {
            new Client
            {
                ClientId = "mubbi-web",
                ClientName = "Mubbi Web",
                RequirePkce = true,
                RequireConsent = false,
                RedirectUris =
                {

                },

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "mubbi-service" },
                AllowAccessTokensViaBrowser = true,
                PostLogoutRedirectUris = { },
                AllowedCorsOrigins = { }
            }
        };
    }
}
