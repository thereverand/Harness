using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harness.Services
{
    public interface IProvider : IServiceProvider
    {
        IProvider RegisterAll(Type serviceType, Assembly[] assemblies, LifetimeScope scope = LifetimeScope.Default, Func<Type, bool> filter = null);

        IProvider RegisterAll<T>(Assembly[] assemblies, LifetimeScope scope = LifetimeScope.Default, Func<Type, bool> filter = null);

        IProvider Register(Type serviceType, Type implementation, LifetimeScope scope = LifetimeScope.Default, string key = null);

        IProvider Register(Type serviceType, object instance, string key = null);

        IProvider Register(Type serviceType, Type implementation, Func<object> handler, string key = null);

        IProvider Register<T>(Type implementation, LifetimeScope scope = LifetimeScope.Default, string key = null);

        IProvider Register<T>(object instance, string key = null);

        IProvider Register<T>(Type implementation, Func<T> handler, string key = null);

        IProvider Register<T, TY>(LifetimeScope scope = LifetimeScope.Default, string key = null);

        IProvider Register<T, TY>(Func<TY> handler, string key = null);

        IProvider AddRegistrar(Action<IProvider> registrar);

        object GetService(Type serviceType, string key);

        T GetService<T>(string key = default(string));

        IEnumerable<object> GetAllServices(Type serviceType);

        IEnumerable<T> GetAllServices<T>();
    }
}