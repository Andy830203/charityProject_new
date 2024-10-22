using Microsoft.EntityFrameworkCore;

namespace WebAPI_for_frondEnd.Models;

public partial class charityContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(config.GetConnectionString("Charity"));
        }
    }
}
