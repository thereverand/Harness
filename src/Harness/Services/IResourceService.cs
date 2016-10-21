using System.IO;

namespace Harness.Services
{
    /// <summary>
    /// Provides resources from one or more sources
    /// </summary>
    public interface IResourceService
    {
        Stream GetResource(string name);

        StreamReader GetResourceReader(string name);
    }
}