using System.Security.Claims;
using MyTemplate.Core.Security.Entities;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Middlewares;

public class LoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly Serilog.ILogger _logger;
  private readonly UserManager<User> _userManager;

  public LoggingMiddleware(RequestDelegate next, Serilog.ILogger logger, UserManager<User> userManager)
  {
    _next = next;
    _logger = logger;
    _userManager = userManager;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var endpoint = context.Request.Path;
    var httpMethod = context.Request.Method;
    try
    {
      await _next(context);
      await LogSuccess(context.User, endpoint, httpMethod);
    }
    catch (Exception ex)
    {
      await LogFailure(ex, context.User, endpoint, httpMethod);
      throw;
    }
  }

  private async Task LogSuccess(ClaimsPrincipal claims, string endpoint, string httpMethod)
  {
    var user = await _userManager.GetUserAsync(claims);
    if (user is not null)
    {
      var userId = user.Id;
      _logger.Information("UserId:{userId} invoked endpoint: {endpoint}, with action:{httpMethod} successfully", userId, endpoint, httpMethod);
    }
    else
    {
      var displayName = "anonymous";
      _logger.Information("{displayName} invoked endpoint: {endpoint}, with action:{httpMethod} successfully", displayName, endpoint, httpMethod);
    }
  }

  private async Task LogFailure(Exception ex, ClaimsPrincipal claims, string endpoint, string httpMethod)
  {
    var user = await _userManager.GetUserAsync(claims);
    if (user is not null)
    {
      var userId = user.Id;
      _logger.Warning("UserId:{userId} invoked endpoint: {endpoint}, with action:{httpMethod} with failure", userId, endpoint, httpMethod);
      _logger.Error(ex, ex.Message);
    }
    else
    {
      var displayName = "anonymous";
      _logger.Warning("{displayName} invoked endpoint: {endpoint}, with action:{httpMethod} with failure", displayName, endpoint, httpMethod);
      _logger.Error(ex, ex.Message);
    }
  }

}
