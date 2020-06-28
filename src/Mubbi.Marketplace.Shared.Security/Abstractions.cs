using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PampaDevs.Utils;
using System;
using System.Text;

namespace Mubbi.Marketplace.Security
{
    public static class Abstractions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            Ensure.Argument.NotNull(services);
            Ensure.Argument.NotNull(configuration);

            var section = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(section);

            var appSettings = section.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddCors();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer,
                    ClockSkew = TimeSpan.FromSeconds(1)
                };
            });

            services.AddAuthorization(PoliciesConfiguration.Configure);

            return services;
        }
    }
}
