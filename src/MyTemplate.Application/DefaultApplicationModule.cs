using Autofac;
using MediatR;
using MyTemplate.Application.Pipelines;

namespace MyTemplate.Application;

public class DefaultApplicationModule : Module
{
protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterGeneric(typeof(ValidationBehavior<,>))
    .As(typeof(IPipelineBehavior<,>))
    .InstancePerDependency();
  }
}