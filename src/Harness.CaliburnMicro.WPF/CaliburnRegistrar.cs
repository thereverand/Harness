using Caliburn.Micro;
using Harness.Services;

namespace Harness
{
    public class CaliburnRegistrar
    {
        public void Register(IProvider provider)
        {
            provider.Register<IWindowManager, WindowManager>();
        }
    }
}