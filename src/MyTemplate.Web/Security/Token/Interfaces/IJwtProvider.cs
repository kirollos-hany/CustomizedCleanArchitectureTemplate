using System.Security.Claims;

namespace MyTemplate.Web.Security.Token.Interfaces;

public interface IJwtProvider
{
  string Provide(IEnumerable<Claim> claims, DateTime expireDate);
}
