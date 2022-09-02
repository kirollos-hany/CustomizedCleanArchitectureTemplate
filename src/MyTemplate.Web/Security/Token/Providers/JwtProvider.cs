using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyTemplate.Web.Security.Token.Configuration;
using MyTemplate.Web.Security.Token.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MyTemplate.Web.Security.Token.Providers;

public class JwtProvider : IJwtProvider
{

  private readonly IJwtConfig _config;

  public JwtProvider(IJwtConfig config)
  {
    _config = config;
  }

  public string Provide(IEnumerable<Claim> claims, DateTime expireDate)
  {
    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secrets));
    var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

    var jwtSecurityToken = new JwtSecurityToken(
    issuer: _config.Issuer,
    audience: _config.Audience,
    claims: claims,
    expires: expireDate,
    signingCredentials: signingCredentials
    );

    return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
  }
}
