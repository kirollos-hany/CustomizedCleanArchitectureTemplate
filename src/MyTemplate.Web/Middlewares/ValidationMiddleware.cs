using FluentValidation;
using MyTemplate.Core.Models;

namespace MyTemplate.Web.Middlewares;

public class ValidationMiddleware
{
  private readonly RequestDelegate _next;

  public ValidationMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (ValidationException exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      var response = new ActionResponse(exception.Errors.Select(failure => failure.ErrorMessage).ToList());
      await context.Response.WriteAsJsonAsync(response);
    }
  }
}
