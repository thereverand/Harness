using Harness.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harness
{
    public static class X
    {
        private static readonly IDictionary<Type, object> Values = new Dictionary<Type, object>();
        private static IProvider ServiceProvider { get; set; }

        public static void BuildFrom(
            Assembly[] assemblies = null,
            Type[] types = null,
            Func<Type, object[], object> activator = null,
            IProvider provider = null
        )
        {
            ServiceProvider = provider ?? new DefaultProvider(assemblies, types, activator);
            foreach (var startup in ServiceProvider.GetAllServices<IStartup>())
            {
                startup.Startup();
            }
        }

        public static T Get<T>()
        {
            if (Values.ContainsKey(typeof(T))) return (T)Values[typeof(T)];
            try
            {
                return ServiceProvider.GetService<T>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Attempt to obtain service failed", ex);
            }
        }

        public static IEnumerable<T> GetAll<T>()
        {
            try
            {
                return ServiceProvider.GetAllServices<T>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Attempt to obtain service failed", ex);
            }
        }

        public static bool Set<T>(T value)
        {
            Values[typeof(T)] = value;

            return true;
        }
    }
}