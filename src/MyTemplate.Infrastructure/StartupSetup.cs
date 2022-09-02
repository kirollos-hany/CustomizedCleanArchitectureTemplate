using MyTemplate.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MyTemplate.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection serviceCollection, string connectionString)
  {
    serviceCollection.AddDbContextPool<ApplicationDbContext>(options =>
    {
      options.UseSqlServer(connectionString);
    });
  }
}
