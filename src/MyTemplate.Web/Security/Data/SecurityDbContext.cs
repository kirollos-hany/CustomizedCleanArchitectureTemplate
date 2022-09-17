using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Web.Security.Entities;

namespace MyTemplate.Web.Security.Data;

public class SecurityDbContext : IdentityDbContext<User,
                                              Role,
                                              Guid,
                                              UserClaim,
                                              UserRole,
                                              UserLogin,
                                              RoleClaim,
                                              UserToken>
{
  public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}