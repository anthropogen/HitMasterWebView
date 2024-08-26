using System;
using System.Collections.Generic;

namespace Bootstrapper.Patterns
{
  public class ServiceLocator
  {
    private static readonly Dictionary<Type, IService> services = new();


    public static void Register<TService>(TService service) where TService : IService
    {
      var type = service.GetType();

      services[type] = service;
    }

    public static TService Get<TService>() where TService : IService
    {
      var type = typeof(TService);
      if (!services.ContainsKey(type))
        throw new InvalidOperationException("Doesn't contains so type");
      return (TService)services[type];
    }
  }
}