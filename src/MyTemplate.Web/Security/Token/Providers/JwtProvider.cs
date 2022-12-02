using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyTemplate.Domain.Common.Entities;
using MyTemplate.Domain.Enums.Security;
using MyTemplate.Web.Security.Token.Configuration;

namespace MyTemplate.Web.Security.Token.Providers;

public class JwtProvider : IUserTwoFactorTokenProvider<User>
{
  private readonly IJwtConfig _config;

  public JwtProvider(IJwtConfig config)
  {
    _config = config;
  }

  private string ProvideAccessToken(IEnumerable<Claim> claims)
  {
    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secrets));
    var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

    var jwtSecurityToken = new JwtSecurityToken(
      issuer: _config.Issuer,
    audience: _config.Audience,
    claims: claims,
    expires: DateTime.Now.AddMonths(_config.Duration),
    signingCredentials: signingCredentials
    );

    return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
  }

  private static string ProvideRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public async Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
  {
    switch (purpose)
    {
      case nameof(TokenNames.AccessToken):
      {
        var claims = await manager.GetClaimsAsync(user);
        var roles = await manager.GetRolesAsync(user);
        var userIdClaim = new Claim(nameof(ClaimsTypes.UserId), user.Id.ToString());
        var jtiClaim = new Claim(nameof(JwtRegisteredClaimNames.Jti),Guid.NewGuid().ToString());
        var emailClaim = new Claim(nameof(JwtRegisteredClaimNames.Email), user.Email);
        var tokenClaims = claims.Concat(roles.Select(r => new Claim(nameof(ClaimsTypes.Roles), r))).ToList();
        tokenClaims.Add(userIdClaim);
        tokenClaims.Add(jtiClaim);
        tokenClaims.Add(emailClaim);
        var jwtToken =  ProvideAccessToken(tokenClaims);
        await manager.SetAuthenticationTokenAsync(user, nameof(LoginProviders.MyTemplate), purpose, jwtToken);
        return jwtToken;
      }
      case nameof(TokenNames.RefreshToken):
      {
        var refreshToken = ProvideRefreshToken();
        await manager.SetAuthenticationTokenAsync(user, nameof(LoginProviders.MyTemplate), purpose, refreshToken);
        return refreshToken;
      }
      default:
        return string.Empty;
    }
  }

  public async Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
  {
    switch (purpose)
    {
      case nameof(TokenNames.AccessToken):
        var accessToken = await manager.GetAuthenticationTokenAsync(user, nameof(LoginProviders.MyTemplate),
          nameof(TokenNames.AccessToken));
        return accessToken != default && accessToken == token;
      case nameof(TokenNames.RefreshToken):
        var refreshToken = await manager.GetAuthenticationTokenAsync(user, nameof(LoginProviders.MyTemplate),
          nameof(TokenNames.RefreshToken));
        return refreshToken != default && refreshToken == token;
      default:
        return false;
    }
  }

  public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
  {
    return Task.FromResult(user.Email != default);
  }
}
