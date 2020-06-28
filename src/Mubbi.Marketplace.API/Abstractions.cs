using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API
{
    public static class Abstractions
    {
        public static IServiceCollection AddMubbiSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(ConfigureSwaggerOptions);
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

            options.EnableAnnotations();
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
    }
}