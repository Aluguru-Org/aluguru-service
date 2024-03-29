using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aluguru.Marketplace.API.Middleware;
using Aluguru.Marketplace.Crosscutting.IoC;
using Aluguru.Marketplace.Register.Data.Seed;
using Aluguru.Marketplace.Security;
using Newtonsoft.Json;

namespace Aluguru.Marketplace.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {               
            if (Env.IsDevelopment())
            {
                services.AddAluguruSwagger();
            }

            services.AddCryptography(Configuration);
            services.AddJwtAuthentication(Configuration);

            services.AddDataComponents(Configuration);

            services.AddConfigurationSettings(Configuration);
            services.AddServiceComponents(typeof(Startup).Assembly);

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseAluguruSwagger();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(Policies.AllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.SeedRegisterContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}