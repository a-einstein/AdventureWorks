using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RCS.AdventureWorks.Api.Products
{
    public class Program
    {
        // TODO
        // Creeren RCS.AdventureWorks.Products voor gemeenschappelijke acties? (edmx, private delen ProductsService.svc.cs)
        // Hernoemen RCS.AdventureWorks.Common?

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}