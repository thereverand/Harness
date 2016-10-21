using System.IO;

namespace Harness.Services
{
    /// <summary>
    /// Provides resources from a source.
    /// </summary>
    public interface IResourceProvider
    {
        Stream GetResource(string name);
    }
}