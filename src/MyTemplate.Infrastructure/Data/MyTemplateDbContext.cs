using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Common.Entities;
using MyTemplate.Domain.Entities.Security;

namespace MyTemplate.Infrastructure.Data;

public class MyTemplateDbContext : IdentityDbContext<User,
                                              Role,
                                              Guid,
                                              UserClaim,
                                              UserRole,
                                              UserLogin,
                                              RoleClaim,
                                              UserToken>
{
  public MyTemplateDbContext(DbContextOptions<MyTemplateDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
