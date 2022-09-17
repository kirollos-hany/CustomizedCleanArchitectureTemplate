using System.Security.Claims;
using MyTemplate.Web.Security.Entities;
using MyTemplate.Web.Security.Enums;
using MyTemplate.Web.Security.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Providers;

public class ClaimsProvider<TUser> : IClaimsProvider<TUser> where TUser : User
{
  private readonly UserManager<TUser> _userManager;

  public ClaimsProvider(UserManager<TUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task<IEnumerable<Claim>> ProvideAsync(TUser user)
  {
    var userClaims = await _userManager.GetClaimsAsync(user);
    var roles = await _userManager.GetRolesAsync(user);
    var roleClaims = roles.Select(role => new Claim(nameof(ClaimsTypes.Roles), role));

    var idClaim = new Claim(nameof(ClaimsTypes.UserId), user.Id.ToString());

    var claims = userClaims.Concat(roleClaims).Concat(new List<Claim> { idClaim });

    return claims;
  }
}
