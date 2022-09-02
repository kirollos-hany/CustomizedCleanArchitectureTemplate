using System.Security.Claims;
using MyTemplate.Core.Security.Interfaces;

namespace MyTemplate.Web.Security.Authentication;

public class AuthenticationResult : IAuthenticationResult
{
  private readonly IEnumerable<Claim> _claims;

  private readonly bool _isAuthenticated;

  private string _message;
  public AuthenticationResult(IEnumerable<Claim> claims)
  {
    _claims = claims;
    _isAuthenticated = true;
    _message = string.Empty;
  }

  private AuthenticationResult(string message)
  {
    _claims = Enumerable.Empty<Claim>();
    _isAuthenticated = false;
    _message = message;
  }

  public static AuthenticationResult Failed(string message)
  {
    return new AuthenticationResult(message);
  }
  public bool IsAuthenticated => _isAuthenticated;

  public IEnumerable<Claim> Claims => _claims;

  public string Message => _message;
}

