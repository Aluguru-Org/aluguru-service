using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Aluguru.Marketplace.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Aluguru.Marketplace.API
{
    public static class Abstractions
    {
        public static IServiceCollection AddAluguruSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(ConfigureSwaggerOptions);
        }

        public static IApplicationBuilder UseAluguruSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {                
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aluguru API V1");
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
                Title = "Aluguru API",                 
                Description = "This is the development aluguru-service iteractive documentation. Use this to correctly integrate your system to our backend.\r\n" +
                    "## Authorization to use Aluguru-Service\r\n" +
                    "Only authorized users can do some requests. In order to use them, authenticate your user as follows:\r\n" +
                    "1. Login using `auth/login` endpoint providing your email and password.\r\n" +
                    "2. Copy the token returned by our api.\r\n" +
                    "3. On **Authorize**, type `Bearer {YOUR_TOKEN}` and close it. For subsequent sessions, repeat the steps above to obtain and use a new user token.\r\n",
                Contact = new OpenApiContact
                {
                    Name = "Felipe Rodrigues de Almeida",
                    Email = "felipe.allmeida.dev@gmail.com",
                    Url = new Uri("https://github.com/felipe-allmeida")
                }
            });

            // options.AddServer(new OpenApiServer()
            // {
            //     Url = "http://localhost:5000",
            //     Description = "Local server with docker-compose"
            // });

            // options.AddServer(new OpenApiServer()
            // {
            //     Url = "https://localhost:44362",
            //     Description = "Local server with SQL Server Express"
            // });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Input the JWT like: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
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