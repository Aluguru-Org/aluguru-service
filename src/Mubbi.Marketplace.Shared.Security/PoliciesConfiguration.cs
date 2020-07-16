using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Security
{
    public static class PoliciesConfiguration
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyDefaults.NotAnonymous, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        }
    }
}
