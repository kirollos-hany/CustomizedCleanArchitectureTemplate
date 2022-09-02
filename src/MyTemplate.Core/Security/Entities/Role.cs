using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Core.Security.Entities;

public class Role : IdentityRole<Guid>
{
  public Role() : base()
  {
    UserRoles = Array.Empty<UserRole>();
  }
  public Role(string roleName) : this()
  {
    Name = roleName;
  }

  public ICollection<UserRole> UserRoles { get; set; }
}
