using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using MicrosoftHost = Microsoft.Extensions.Hosting.Host;

namespace AspireAdmin.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return MicrosoftHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
