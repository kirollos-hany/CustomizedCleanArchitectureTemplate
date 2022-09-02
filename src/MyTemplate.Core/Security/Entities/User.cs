using Microsoft.AspNetCore.Identity;
using MyTemplate.Core.Persistence.Interfaces;
namespace MyTemplate.Core.Security.Entities;

public class User : IdentityUser<Guid>, IAggregateRoot
{
  public User()
  {
    Claims = Enumerable.Empty<UserClaim>();
    Logins = Enumerable.Empty<UserLogin>();
    Tokens = Enumerable.Empty<UserToken>();
    UserRoles = Enumerable.Empty<UserRole>();
  }
  public IEnumerable<UserClaim> Claims { get; set; }
  public IEnumerable<UserLogin> Logins { get; set; }
  public IEnumerable<UserToken> Tokens { get; set; }
  public IEnumerable<UserRole> UserRoles { get; set; }
}
