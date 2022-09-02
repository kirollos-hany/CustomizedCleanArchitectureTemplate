using System.Security.Claims;

namespace MyTemplate.Core.Security.Interfaces;

public interface IClaimsProvider<T>
{
  Task<IEnumerable<Claim>> ProvideAsync(T user);
}
