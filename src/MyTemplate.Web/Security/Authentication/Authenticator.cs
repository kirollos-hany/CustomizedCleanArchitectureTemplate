using Microsoft.AspNetCore.Identity;
using MyTemplate.Web.Security.Entities;
using MyTemplate.Web.Security.Interfaces;

namespace MyTemplate.Web.Security.Authentication;

public class Authenticator<TUser> : IAuthenticator<TUser> where TUser : User
{
  private readonly UserManager<TUser> _userManager;

  private readonly IClaimsProvider<TUser> _claimsProvider;

  public Authenticator(UserManager<TUser> userManager, IClaimsProvider<TUser> claimsProvider)
  {
    _userManager = userManager;
    _claimsProvider = claimsProvider;
  }

  public async Task<IAuthenticationResult> AuthenticateByEmailAsync(string email, string password)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == default)
    {
      return AuthenticationResult.Failed("Email is invalid or not registered");
    }
    var loginResult = await _userManager.CheckPasswordAsync(user, password);
    if (!loginResult)
    {
      return AuthenticationResult.Failed("Password is incorrect");
    }

    var claims = await _claimsProvider.ProvideAsync(user);

    return new AuthenticationResult(claims);
  }

  public async Task<IAuthenticationResult> AuthenticateByNameAsync(string name, string password)
  {
    var user = await _userManager.FindByNameAsync(name);
    if (user == default)
    {
      return AuthenticationResult.Failed("Username is invalid or not registered");
    }
    var loginResult = await _userManager.CheckPasswordAsync(user, password);
    if (!loginResult)
    {
      return AuthenticationResult.Failed("Password is incorrect");
    }

    var claims = await _claimsProvider.ProvideAsync(user);

    return new AuthenticationResult(claims);
  }
}