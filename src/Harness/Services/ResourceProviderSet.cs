using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Harness.Services
{
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