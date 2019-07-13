using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CheckLinksConsole
{
  public class Config
  {
    public static IConfigurationRoot Build()
    {
        var inMemory = new Dictionary<string, string>
        {
            {"site:url", "https://google.com"},
            {"output:folder", "reports"}
        };
        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemory)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("checksettings.json", true)
            .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray());

        return configBuilder.Build();
    }
  }
}