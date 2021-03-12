using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using MicrosoftHost = Microsoft.Extensions.Hosting.Host;

namespace AspireAdmin.Host
{
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return MicrosoftHost.CreateDefaultBuilder(args)
                .ConfigureLogging(configureLogging =>
                {
                    configureLogging.AddSimpleConsole(options =>
                    {
                        options.TimestampFormat = "[HH:mm:ss.fff] ";
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
