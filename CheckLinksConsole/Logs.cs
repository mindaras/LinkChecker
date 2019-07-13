using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CheckLinksConsole
{
    public static class Logs
    {
      public static void Init(ILoggerFactory loggerFactory, IConfiguration configuration)
      {
          loggerFactory.AddFile(configuration.GetSection("Logging"));
      }
    }
}