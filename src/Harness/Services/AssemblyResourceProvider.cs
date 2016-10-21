using System.IO;
using System.Reflection;

namespace Harness.Services
{
    /// <summary>
    /// Provides Resources embedded in an assembly.
    /// </summary>
    public class AssemblyResourceProvider : IResourceProvider
    {
        private readonly Assembly _source;
        private readonly string _ns;

        public AssemblyResourceProvider(Assembly source, string ns)
        {
            _source = source;
            _ns = ns;
        }

        public string GetResourceName(string name)
        {
            return $"{_ns}.Resources.{name}";
        }

        public Stream GetResource(string name)
        {
            var resourceName = GetResourceName(name);
            return _source.GetManifestResourceStream(resourceName);
        }
    }
}