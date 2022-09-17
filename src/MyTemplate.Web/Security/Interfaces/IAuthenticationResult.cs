using System.Security.Claims;
namespace MyTemplate.Web.Security.Interfaces;

public interface IAuthenticationResult
{
  string Message { get; }
  bool IsAuthenticated { get; }

  IEnumerable<Claim> Claims { get; }
}