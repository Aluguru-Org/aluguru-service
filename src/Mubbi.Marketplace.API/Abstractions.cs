using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mubbi.Marketplace.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mubbi.Marketplace.API
{
    public static class Abstractions
    {
        public static IServiceCollection AddMubbiSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(ConfigureSwaggerOptions);
        }

        public static IApplicationBuilder UseMubbiSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mubbi API V1");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }

        private static void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            //TODO: Swagger configuration should be loaded
            // by a configuration file like appsettings.json

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Mubbi API",
                Description = "Mubbi API",
                Contact = new OpenApiContact
                {
                    Name = "Mubbi"
                },
                License = new OpenApiLicense
                {
                    Name = "Lincense.."
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Input the JWT like: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

            options.EnableAnnotations();

            options.SchemaFilter<SwaggerExcludeFilter>();
        }
    }
}