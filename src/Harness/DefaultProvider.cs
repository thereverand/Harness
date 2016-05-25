using Harness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harness
{
    public class DefaultProvider : IProvider
    {
        protected IEnumerable<Type> Types;
        protected readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();
        protected readonly IDictionary<Type, Func<object>> Handlers = new Dictionary<Type, Func<object>>();
        protected readonly Func<Type, object[], object> Activator = (type, args) => System.Activator.CreateInstance(type, args);

        public DefaultProvider(
            Assembly[] assemblies = default(Assembly[]),
            Type[] types = default(Type[]),
            Func<Type, object[], object> activator = default(Func<Type, object[], object>))
        {
            var ts = types != null ? types.ToList() : new List<Type>();
            var assembltTs = assemblies?.SelectMany(a => a.ExportedTypes);
            if (assembltTs != null)
                ts.AddRange(assembltTs);

            Types = ts;

            if (activator != default(Func<Type, object[], object>))
            {
                Activator = activator;
            }
        }

        protected object GetDefault(Type t)
        {
            return this.GetType().GetRuntimeMethod("GetDefaultGeneric", new Type[0]).MakeGenericMethod(t).Invoke(this, null);
        }

        protected T GetDefaultGeneric<T>()
        {
            return default(T);
        }

        protected Type FindImplementationType(Type serviceType)
        {
            return Types.FirstOrDefault(t => t == serviceType);
        }

        protected IEnumerable<Type> FindImplementationTypes(Type serviceType)
        {
            return Types.Where(t => serviceType.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));
        }

        public virtual IProvider RegisterAll(Type serviceType, Assembly[] assemblies, Func<Type, bool> filter = null)
        {
            throw new NotImplementedException("The provider does not support this kind of registration");
        }

        public virtual IProvider RegisterAll<T>(Assembly[] assemblies, Func<Type, bool> filter = null)
        {
            throw new NotImplementedException("The provider does not support this kind of registration");
        }

        public virtual IProvider Register(
            Type serviceType,
            Type implementation = null,
            object instance = null,
            string key = null,
            LifetimeScope scope = LifetimeScope.Default,
            Func<object> handler = default(Func<object>))
        {
            if (instance == default(object) && handler == default(Func<object>)) throw new NotImplementedException("The provider does not support this kind of registration");

            if (instance != default(object)) Instances[serviceType] = implementation;
            else
            {
                Handlers[serviceType] = handler;
            }
            return this;
        }

        public virtual IProvider Register<T>(
            Type implementation = null,
            T instance = default(T),
            string key = null,
            LifetimeScope scope = LifetimeScope.Default,
            Func<T> handler = null) where T : class
        {
            Register(typeof(T), implementation, instance, key, scope, handler);
            return this;
        }

        public IProvider Register<T, TY>(
            string key = null,
            LifetimeScope scope = LifetimeScope.Default)
            where T : class
            where TY : class
        {
            Register(typeof(T), implementation: typeof(TY), key: key, scope: scope);
            return this;
        }

        public IProvider AddRegistrar(Action<IProvider> registrar)
        {
            registrar(this);
            return this;
        }

        public virtual object GetService(Type serviceType)
        {
            if (Instances.ContainsKey(serviceType)) return Instances[serviceType];
            if (Handlers.ContainsKey(serviceType)) return Handlers[serviceType]();

            var t = FindImplementationType(serviceType);
            return t == default(Type) ? default(object) : Activator(t, null);
        }

        public virtual object GetService(Type serviceType, string key)
        {
            return GetService(serviceType);
        }

        public virtual T GetService<T>(string key = null)
        {
            return (T)GetService(typeof(T));
        }

        public virtual IEnumerable<object> GetAllServices(Type serviceType)
        {
            return FindImplementationTypes(serviceType).Select(t => Activator(t, null));
        }

        public virtual IEnumerable<T> GetAllServices<T>()
        {
            return GetAllServices(typeof(T)).Cast<T>();
        }
    }
}