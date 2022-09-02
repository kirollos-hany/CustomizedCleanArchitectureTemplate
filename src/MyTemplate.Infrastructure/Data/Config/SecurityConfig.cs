using MyTemplate.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyTemplate.Infrastructure.Data.Config;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder.HasMany(e => e.UserRoles)
        .WithOne()
        .HasForeignKey(uc => uc.RoleId)
        .IsRequired();
    builder.ToTable(nameof(Role));
  }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

    builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

    builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

    builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    builder.ToTable(nameof(User));
  }
}

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    builder.ToTable(nameof(UserRole));
  }
}

public class RoleClaimConfig : IEntityTypeConfiguration<RoleClaim>
{
  public void Configure(EntityTypeBuilder<RoleClaim> builder)
  {
    builder.ToTable(nameof(RoleClaim));
  }
}

public class UserClaimConfig : IEntityTypeConfiguration<UserClaim>
{
  public void Configure(EntityTypeBuilder<UserClaim> builder)
  {
    builder.ToTable(nameof(UserClaim));
  }
}

public class UserLoginConfig : IEntityTypeConfiguration<UserLogin>
{
  public void Configure(EntityTypeBuilder<UserLogin> builder)
  {
    builder.ToTable(nameof(UserLogin));
  }
}

public class UserToken : IEntityTypeConfiguration<Core.Security.Entities.UserToken>
{
  public void Configure(EntityTypeBuilder<Core.Security.Entities.UserToken> builder)
  {
    builder.ToTable(nameof(UserToken));
  }
}
