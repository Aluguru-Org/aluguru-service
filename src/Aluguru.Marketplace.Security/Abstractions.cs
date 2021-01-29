using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Aluguru.Marketplace.Security.User;
using PampaDevs.Utils;
using System;
using System.Text;

namespace Aluguru.Marketplace.Security
{
    public static class Abstractions
    {
        public static IServiceCollection AddCryptography(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("SecuritySettings");
            services.Configure<SecuritySettings>(section);

            services.AddScoped<ICryptography, Cryptography>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            Ensure.Argument.NotNull(services);
            Ensure.Argument.NotNull(configuration);

            var section = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(section);

            var appSettings = section.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddCors(options => PoliciesConfiguration.ConfigureCors(options, appSettings.Audiences));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudiences = appSettings.Audiences,

                    ClockSkew = TimeSpan.FromSeconds(1)
                };
            });

            services.AddAuthorization(PoliciesConfiguration.ConfigureAuthorization);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }
    }
}
