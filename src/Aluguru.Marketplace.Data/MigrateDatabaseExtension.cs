using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aluguru.Marketplace.Infrastructure;
using System;

namespace Aluguru.Marketplace.Data
{
    public static class MigrateDatabaseExtension
    {
        public static IHost MigrateDatabase(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    using (var context = services.GetRequiredService<AluguruContext>())
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.GetErrorMsg() + " | " + ex.GetErrorList());
                }
            }
            return webHost;
        }
    }

}
