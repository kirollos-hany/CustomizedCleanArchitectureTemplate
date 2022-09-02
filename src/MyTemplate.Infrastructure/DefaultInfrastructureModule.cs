﻿using System.Reflection;
using Autofac;
using MyTemplate.Core;
using MyTemplate.Core.Files.Interfaces;
using MyTemplate.Infrastructure.Data;
using MyTemplate.Infrastructure.IO;
using MyTemplate.Core.Persistence.Interfaces;
using ExtCore.FileStorage.Abstractions;
using ExtCore.FileStorage.FileSystem;
using MediatR;
using MediatR.Pipeline;
using Module = Autofac.Module;

namespace MyTemplate.Infrastructure;

public class DefaultInfrastructureModule : Module
{
  private readonly bool _isDevelopment = false;
  private readonly List<Assembly> _assemblies = new List<Assembly>();

  public DefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
  {
    _isDevelopment = isDevelopment;
    var coreAssembly = Assembly.GetAssembly(typeof(DefaultCoreModule));
    var infrastructureAssembly = Assembly.GetAssembly(typeof(DefaultInfrastructureModule));
    if (coreAssembly != null)
    {
      _assemblies.Add(coreAssembly);
    }
    if (infrastructureAssembly != null)
    {
      _assemblies.Add(infrastructureAssembly);
    }
    if (callingAssembly != null)
    {
      _assemblies.Add(callingAssembly);
    }
  }

  protected override void Load(ContainerBuilder builder)
  {
    if (_isDevelopment)
    {
      RegisterDevelopmentOnlyDependencies(builder);
    }
    else
    {
      RegisterProductionOnlyDependencies(builder);
    }
    RegisterCommonDependencies(builder);
  }

  private void RegisterCommonDependencies(ContainerBuilder builder)
  {
    builder.RegisterGeneric(typeof(EfRepository<>))
        .As(typeof(IRepository<>))
        .As(typeof(IReadRepository<>))
        .InstancePerLifetimeScope();

    builder
        .RegisterType<Mediator>()
        .As<IMediator>()
        .InstancePerLifetimeScope();

    builder.RegisterType<FileStorage>()
      .As<IFileStorage>()
      .InstancePerDependency();

    builder.RegisterType<FileExtensionContentTypeProvider>()
      .As<IContentTypeProvider>()
      .InstancePerDependency();

    builder.RegisterType<FileManager>()
      .As<IFileManager>()
      .InstancePerDependency();

    builder.Register<ServiceFactory>(context =>
    {
      var c = context.Resolve<IComponentContext>();
      return t => c.Resolve(t);
    });

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
      .RegisterAssemblyTypes(_assemblies.ToArray())
      .AsClosedTypesOf(mediatrOpenType)
      .AsImplementedInterfaces();
    }
  }

  private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
  {
    // TODO: Add development only services
  }

  private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
  {
    // TODO: Add production only services
  }

}
