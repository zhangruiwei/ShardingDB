using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Warren.OrderService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

            .ConfigureAppConfiguration((context, config) =>
            {

                var rootConfig = config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", false, true)
                .AddJsonFile("Config/mysql.json", false, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args).Build();

            })
            .ConfigureServices(services => { })
            .ConfigureLogging((hostingContext, logging) =>
            {

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

#if DEBUG
                logging.AddConsole();
                logging.AddDebug();
#endif
            })
            .UseStartup<Startup>();
    }
}
