using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Security
{
    public static class PoliciesConfiguration
    {
        public static void ConfigureCors(CorsOptions options, string[] origins)
        {
            options.AddPolicy(Policies.AllowSpecificOrigins,
                    builder => builder
                            .WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod());
        }

        public static void ConfigureAuthorization(AuthorizationOptions options)
        {
            options.AddPolicy(Policies.NotAnonymous, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        }
    }
}
