using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Harness.Services
{
    /// <summary>
    /// Provides resources from a source.
    /// </summary>
    public interface IResourceProvider
    {
        bool IsReadOnly { get; }

        Stream GetResource(string name);

        //bool SetResource(string name, Stream resources);
    }

    /// <summary>
    /// Provides resources from one or more sources
    /// </summary>
    public interface IResourceService
    {
        Stream GetResource(string name);

        StreamReader GetResourceReader(string name);

        //bool SetResource(string name, Stream value);

        //StreamWriter GetResourceWriter(string name);
    }

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

        public bool IsReadOnly => false;

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

    public class ResourceProviderSet : HashSet<IResourceProvider>, IResourceService
    {
        public Stream GetResource(string name)
        {
            return this.Select(p => p.GetResource(name)).First(p => p != null);
        }

        public StreamReader GetResourceReader(string name)
        {
            var result = this.Select(p => p.GetResource(name));
            return result == null ? null : new StreamReader(result.First(p => p != null));
        }
    }
}