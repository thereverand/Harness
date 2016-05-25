using Caliburn.Micro;
using Harness.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Harness
{
    public abstract class Startup<TRootModel> : BootstrapperBase
    {
        private SimpleContainer _container;

        protected Startup(bool useApplication = true) : base(useApplication)
        {
            Initialize();
        }

        protected abstract void Register(IProvider provider);

        protected override void Configure()
        {
            _container = new SimpleContainer();
            var provider = new SimpleServiceProvider(_container);

            provider
                .Register<IServiceProvider>(instance: provider)
                .AddRegistrar(CaliburCoreRegistrar.Register)
                .AddRegistrar(CaliburnRegistrar.Register);
            Register(provider);

            X.BuildFrom(provider: provider);
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<TRootModel>();
        }
    }
}