using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PFStudio.PFSign
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            SeedData.EnsureMigrate(host.Services);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
