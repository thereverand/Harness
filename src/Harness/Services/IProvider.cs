using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harness.Services
{
    public enum LifetimeScope
    {
        Default,
        Instance,
        PerRequest,
        Singleton,
    }

    public interface IProvider : IServiceProvider
    {
        IProvider RegisterAll(Type serviceType, Assembly[] assemblies, Func<Type, bool> filter = null);

        IProvider RegisterAll<T>(Assembly[] assemblies, Func<Type, bool> filter = null);

        IProvider Register(
            Type serviceType,
            Type implementation = default(Type),
            object instance = default(object),
            string key = default(string),
            LifetimeScope scope = LifetimeScope.Default,
            Func<object> handler = default(Func<object>)
        );

        IProvider Register<T>(
            Type implementation = default(Type),
            T instance = default(T),
            string key = default(string),
            LifetimeScope scope = LifetimeScope.Default,
            Func<T> handler = default(Func<T>)
        ) where T : class;

        IProvider Register<T, TY>(
            string key = default(string),
            LifetimeScope scope = LifetimeScope.Default
        ) where T : class
          where TY : class;

        IProvider AddRegistrar(Action<IProvider> registrar);

        object GetService(Type serviceType, string key);

        T GetService<T>(string key = default(string));

        IEnumerable<object> GetAllServices(Type serviceType);

        IEnumerable<T> GetAllServices<T>();
    }
}