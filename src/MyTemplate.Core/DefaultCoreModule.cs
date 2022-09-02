using System.Reflection;
using Autofac;
using MyTemplate.Core.Pipelines;
using Mapster;
using MediatR;
using Module = Autofac.Module;

namespace MyTemplate.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    builder.RegisterGeneric(typeof(ValidationBehavior<,>))
    .As(typeof(IPipelineBehavior<,>))
    .InstancePerDependency();
  }
}
