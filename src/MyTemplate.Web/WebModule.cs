using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace MyTemplate.Web;

public class WebModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
        var mediatrOpenTypes = new[]
    {
      typeof(IRequestHandler<,>),
      typeof(IRequestExceptionHandler<,,>),
      typeof(IRequestExceptionAction<,>),
      typeof(INotificationHandler<>),
    };
    
    foreach (var mediatrOpenType in mediatrOpenTypes)
    {
      builder
        .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(WebModule)))
        .AsClosedTypesOf(mediatrOpenType)
        .AsImplementedInterfaces();
    }
  }
}