using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Core.Security.Entities;

namespace MyTemplate.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User,
                                              Role,
                                              Guid,
                                              UserClaim,
                                              UserRole,
                                              UserLogin,
                                              RoleClaim,
                                              UserToken>
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
