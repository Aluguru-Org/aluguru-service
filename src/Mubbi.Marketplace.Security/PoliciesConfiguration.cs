using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

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
            options.AddPolicy(Policies.NotAnonymous, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
            });

            options.AddPolicy(Policies.CategoryReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Category, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.CategoryWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Category, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.ProductReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Product, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.ProductWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Product, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.RentPeriodReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.RentPeriod, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.RentPeriodWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.RentPeriod, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.UserReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.User, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.UserWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.User, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.UserRoleReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.UserRole, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.UserRoleWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.UserRole, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.OrderReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Order, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.OrderWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Order, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });

            options.AddPolicy(Policies.VoucherReader, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Voucher, ClaimValuesHelper.ToPolicy(ClaimValues.Read));
            });

            options.AddPolicy(Policies.VoucherWriter, policyBuilder =>
            {
                policyBuilder.AddDefaultAuth();
                policyBuilder.RequireClaim(Claims.Voucher, ClaimValuesHelper.ToPolicy(ClaimValues.Write));
            });
        }

        private static void AddDefaultAuth(this AuthorizationPolicyBuilder builder)
        {
            builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            builder.RequireAuthenticatedUser();
        }
    }
}
