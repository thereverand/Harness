using Caliburn.Micro;
using Harness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harness
{
    public class SimpleServiceProvider : IProvider
    {
        private readonly SimpleContainer _container;

        public SimpleServiceProvider(SimpleContainer container)
        {
            _container = container;
        }

        public IProvider RegisterAll(Type serviceType, Assembly[] assemblies, LifetimeScope scope = LifetimeScope.Default, Func<Type, bool> filter = default(Func<Type, bool>))
        {
            foreach (var a in assemblies)
            {
                var types = a.ExportedTypes.Where(serviceType.IsAssignableFrom);

                // Filter Func handles it.
                // ReSharper disable once AssignNullToNotNullAttribute
                var validTypes = filter == default(Func<Type, bool>) ? types : types.Where(filter);
                foreach (var t in validTypes)
                {
                    Register(serviceType, t, scope);
                }
            }
            return this;
        }

        public IProvider RegisterAll<T>(Assembly[] assemblies, LifetimeScope scope = LifetimeScope.Default, Func<Type, bool> filter = null)
        {
            RegisterAll(typeof(T), assemblies, scope, filter);
            return this;
        }

        public IProvider Register(Type serviceType, Type implementation, LifetimeScope scope = LifetimeScope.Default, string key = null)
        {
            switch (scope)
            {
                case LifetimeScope.Default:
                case LifetimeScope.PerRequest:
                    _container.RegisterPerRequest(serviceType, key, implementation);
                    break;

                case LifetimeScope.Singleton:
                    _container.RegisterSingleton(serviceType, key, implementation);
                    break;

                case LifetimeScope.Instance:
                    break;

                default:
                    throw new ArgumentException("Lifetime Scope value is invalid", nameof(scope));
            }

            return this;
        }

        public IProvider Register(Type serviceType, object instance, string key = null)
        {
            _container.RegisterInstance(serviceType, key, instance);
            return this;
        }

        public IProvider Register(Type serviceType, Type implementation, Func<object> handler, string key = null)
        {
            _container.RegisterHandler(serviceType, key, c => handler());
            return this;
        }

        public IProvider Register<T>(Type implementation, LifetimeScope scope = LifetimeScope.Default, string key = null)
        {
            return Register(typeof(T), implementation, scope, key);
        }

        public IProvider Register<T>(object instance, string key = null)
        {
            return Register(typeof(T), instance, key);
        }

        public IProvider Register<T>(Type implementation, Func<T> handler, string key = null)
        {
            return Register(typeof(T), implementation, () => handler(), key);
        }

        public IProvider Register<T, TY>(LifetimeScope scope = LifetimeScope.Default, string key = null)
        {
            return Register(typeof(T), typeof(TY), scope, key);
        }

        public IProvider Register<T, TY>(Func<TY> handler, string key = null)
        {
            return Register(typeof(T), typeof(TY), () => handler(), key);
        }

        public IProvider AddRegistrar(Action<IProvider> registrar)
        {
            registrar(this);
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType, null);
        }

        public object GetService(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        public T GetService<T>(string key = null)
        {
            return _container.GetInstance<T>(key);
        }

        public IEnumerable<object> GetAllServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public IEnumerable<T> GetAllServices<T>()
        {
            return _container.GetAllInstances<T>();
        }
    }
}