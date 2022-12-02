using System.Text;
using MyTemplate.Application.Interfaces.Web;

namespace MyTemplate.Web.Services;

public class BaseUrlProvider : IBaseUrlProvider
{
  private readonly HttpContext _httpContext;

  public BaseUrlProvider(IHttpContextAccessor accessor)
  {
    _httpContext = accessor.HttpContext!;
  }

  public string GetBaseUrl()
  {
    var request = _httpContext.Request;
    return new StringBuilder(request.Scheme).Append("://").Append(request.Host).Append(request.PathBase.ToString()).ToString();
  }
}