using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mubbi.Marketplace.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Data
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
                    using (var context = services.GetRequiredService<MubbiContext>())
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
