using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CheckLinksConsole
{
  public class LinkChecker
  {
    private ILogger _logger;

    public LinkChecker(ILogger logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> GetLinks(string link, string page)
    {
        var htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(page);

        var originalLinks = htmlDocument.DocumentNode.SelectNodes("//a[@href]")
          .Select(n => n.GetAttributeValue("href", string.Empty))
          .ToList();

        using (_logger.BeginScope($"Getting links from {link}"))
        {
          originalLinks.ForEach(l => _logger.LogTrace(200, "Getting original link {link}", l));
        }

        var links = originalLinks
          .Where(l => !String.IsNullOrEmpty(l))
          .Where(l => l.StartsWith("http"));

        return links;
    }

    public IEnumerable<LinkCheckResult> CheckLinks(IEnumerable<string> links)
    {
      var all = Task.WhenAll(links.Select(CheckLink));
      return all.Result;
    }

    public async Task<LinkCheckResult> CheckLink(string link)
    {
      var result = new LinkCheckResult();

      result.Link = link;

      using (var client = new HttpClient())
      {
        var request = new HttpRequestMessage(HttpMethod.Head, link);

        try
        {
          var response = await client.SendAsync(request);
          result.Problem = response.IsSuccessStatusCode ? null : response.StatusCode.ToString();
          return result;
        }
        catch (HttpRequestException exception)
        {
          _logger.LogTrace(100, exception, "Failed to retrieve {link}", link);
          result.Problem = exception.Message;
          return result;
        }
      }
    }
  }
}

public class LinkCheckResult
{
  public int Id { get; set; }
  public bool Exists => String.IsNullOrEmpty(Problem);
  public bool IsMissing => !Exists;
  public string Problem { get; set; }
  public string Link { get; set; }
  public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}