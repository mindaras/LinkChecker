using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CheckLinksConsole
{
  public class Program
    { 
        static void Main(string[] args)
        {   
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
