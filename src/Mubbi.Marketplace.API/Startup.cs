using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mubbi.Marketplace.Data;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;

namespace Mubbi.Marketplace.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MubbiContext>(options => options.UseSqlServer(connectionString));

            //services.AddDbContext<MubbiContext>(options =>
            //{
            //    options.UseInMemoryDatabase("Database");
            //});

            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddSwagger();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
