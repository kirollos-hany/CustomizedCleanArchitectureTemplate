using System.Security.Claims;
namespace MyTemplate.Core.Security.Interfaces;

public interface IAuthenticationResult
{
  string Message { get; }
  bool IsAuthenticated { get; }

  IEnumerable<Claim> Claims { get; }
}