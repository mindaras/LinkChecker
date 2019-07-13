using Microsoft.EntityFrameworkCore;

namespace CheckLinksConsole
{
    public class LinksDb : DbContext
    {
      public DbSet<LinkCheckResult> Links { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        // MSSQL:
        // var connection = @"Server=localhost;Database=Links;User Id=sa;Password=whatever12!";
        // optionsBuilder.UseSqlServer(connection);

        // MySQL:
        var connection = "server=localhost;userid=root;pwd=password;database=Links;sslmode=none;AllowPublicKeyRetrieval=True;";
        optionsBuilder.UseMySql(connection);
      }
    }
}