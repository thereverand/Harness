using Caliburn.Micro;
using Harness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harness
{
    public class SimpleServiceProvider : DefaultProvider
    {
        private readonly SimpleContainer _container;

        public SimpleServiceProvider(SimpleContainer container)
        {
            _container = container;
        }

        public override IProvider RegisterAll(Type serviceType, Assembly[] assemblies, Func<Type, bool> filter = null)
        {
            if (filter == default(Func<Type, bool>)) filter = t => true;
            foreach (var a in assemblies)
            {
                var ts = a.ExportedTypes.Where(filter);
                foreach (var t in ts)
                {
                    _container.RegisterPerRequest(serviceType, null, t);
                }
            }
            return this;
        }

        public override IProvider RegisterAll<T>(Assembly[] assemblies, Func<Type, bool> filter = null)
        {
            RegisterAll(typeof(T), assemblies, filter);
            return this;
        }

        public override IProvider Register(
            Type serviceType,
            Type implementation = default(Type),
            object instance = default(object),
            string key = default(string),
            LifetimeScope scope = LifetimeScope.Default,
            Func<object> handler = default(Func<object>))
        {
            if (instance != default(object))
            {
                _container.RegisterInstance(serviceType, key, instance);
                return this;
            }
            if (handler != default(Func<object>))
            {
                _container.RegisterHandler(serviceType, key, c => handler());
                return this;
            }

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

        public override IProvider Register<T>(
            Type implementation = default(Type),
            T instance = default(T),
            string key = default(string),
            LifetimeScope scope = LifetimeScope.Default,
            Func<T> handler = default(Func<T>))
        {
            Register(typeof(T), implementation, instance, key, scope);
            return this;
        }

        public override object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType, null);
        }

        public override object GetService(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, key);
        }

        public override T GetService<T>(string key = null)
        {
            return _container.GetInstance<T>(key);
        }

        public override IEnumerable<object> GetAllServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public override IEnumerable<T> GetAllServices<T>()
        {
            return _container.GetAllInstances<T>();
        }
    }
}