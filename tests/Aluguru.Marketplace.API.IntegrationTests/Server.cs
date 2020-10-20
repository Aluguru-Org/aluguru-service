using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;

namespace Aluguru.Marketplace.API.IntegrationTests
{
    public static class Server
    {
        static Server()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile($"appsettings.Test.json", optional: true);
                })
                .UseStartup<Startup>()); ;

            Instance = server;
        }

        public static TestServer Instance { get; }
        public static HttpClient NewClient()
        {
            return Instance.CreateClient();
        }
    }
}
