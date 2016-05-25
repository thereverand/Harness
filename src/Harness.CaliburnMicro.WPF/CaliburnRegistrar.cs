using Caliburn.Micro;
using Harness.Services;

namespace Harness
{
    public static class CaliburnRegistrar
    {
        public static void Register(IProvider provider)
        {
            provider.Register<IWindowManager, WindowManager>();
        }
    }
}