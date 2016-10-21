using Caliburn.Micro;
using Harness.Services;

namespace Harness
{
    public class CaliburCoreRegistrar
    {
        public void Register(IProvider provider)
        {
            provider.Register<IEventAggregator, EventAggregator>();
        }
    }
}