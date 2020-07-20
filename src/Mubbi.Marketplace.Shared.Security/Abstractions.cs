using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mubbi.Marketplace.Security.User;
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

            services.AddCors(setup =>
            {

            });

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

            services.AddAuthorization(PoliciesConfiguration.Configure);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }
    }
}
