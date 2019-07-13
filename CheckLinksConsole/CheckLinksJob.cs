using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CheckLinksConsole
{
  public class CheckLinksJob
    {
        private ILogger _logger;
        private OutputSettings _output;
        private SiteSettings _siteSettings;
        private LinkChecker _linkChecker;

        public CheckLinksJob(
            ILogger<CheckLinksJob> logger, 
            IOptions<OutputSettings> output, 
            IOptions<SiteSettings> siteSettings, 
            LinkChecker linkChecker)
        {
            _logger = logger;
            _output = output.Value;
            _siteSettings = siteSettings.Value;
            _linkChecker = linkChecker;
        }

        public void Execute()
        {
            Console.WriteLine("CheckLinksJob executed");
            Directory.CreateDirectory(_output.GetReportDirectoryPath());
            var client = new HttpClient();
            var body = client.GetStringAsync(_siteSettings.Url);
            var links = _linkChecker.GetLinks(_siteSettings.Url, body.Result);
            _logger.LogInformation(100, $"Saving report to {_output.GetReportFilePath()}");
            var checkedLinks = _linkChecker.CheckLinks(links);
  
            using (var file = File.CreateText(_output.GetReportFilePath()))
            using (var linksDb = new LinksDb())
            {
                foreach (var link in checkedLinks.OrderBy(l => l.Exists))
                {  
                    var status = link.IsMissing ? "missing" : "OK";
                    file.WriteLine($"{status} - {link.Link}");
                    linksDb.Links.Add(link);
                }

                linksDb.SaveChanges();
            }
        }
    }
}
