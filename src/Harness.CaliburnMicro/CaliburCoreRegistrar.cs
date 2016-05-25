using Caliburn.Micro;
using Harness.Services;

namespace Harness
{
    public static class CaliburCoreRegistrar
    {
        public static void Register(IProvider provider)
        {
            provider.Register<IEventAggregator, EventAggregator>();
        }
    }
}