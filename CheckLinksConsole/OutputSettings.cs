using System.IO;

namespace CheckLinksConsole
{
  public class OutputSettings
  {
      public OutputSettings()
      {
          File = "report.txt";
      }

      public string Folder { get; set; }
      public string File { get; set; }

      public string GetReportFilePath()
      {
          return Path.Combine(Directory.GetCurrentDirectory(), Folder, File);
      }

      public string GetReportDirectoryPath()
      {
          return Path.GetDirectoryName(GetReportFilePath());
      }
  }
}
