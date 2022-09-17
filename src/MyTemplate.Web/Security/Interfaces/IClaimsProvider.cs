using System.Security.Claims;

namespace MyTemplate.Web.Security.Interfaces;

public interface IClaimsProvider<T>
{
  Task<IEnumerable<Claim>> ProvideAsync(T user);
}
