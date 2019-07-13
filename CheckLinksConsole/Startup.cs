using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CheckLinksConsole
{
  public class Startup
  {
    private IConfigurationRoot _config;

     // Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        _config = Config.Build();
        services.AddHangfire(c => c.UseMemoryStorage());
        services.AddTransient<CheckLinksJob>();
        services.AddTransient<LinkChecker>();
        services.Configure<OutputSettings>(_config.GetSection("output"));
        services.Configure<SiteSettings>(_config.GetSection("site"));
    }

    // Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        Logs.Init(loggerFactory, _config);
        app.UseHangfireServer();
        app.UseHangfireDashboard();
        RecurringJob.AddOrUpdate<CheckLinksJob>(j => j.Execute(), Cron.Minutely);
    }
  }
}