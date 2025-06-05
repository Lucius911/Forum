using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Forum.Data
{
  public class AppDbContextFactory : IDesignTimeDbContextFactory<ForumContext>
  {
    public ForumContext CreateDbContext(string[] args)
    {
      var optionBuilder = new DbContextOptionsBuilder<ForumContext>();

      // Load connection string
      var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .Build();

      var connectionString = config.GetConnectionString("DefaultConnection");

      optionBuilder.UseSqlServer(connectionString);

      return new ForumContext(optionBuilder.Options);
    }
  }
}
