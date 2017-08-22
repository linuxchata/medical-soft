using Common.Cache;
using Models;

namespace Client.Cache.Interface
{
    /// <summary>
    /// Represents gender cache.
    /// </summary>
    public interface IGenderCache : ICache<GenderModel>
    {
    }
}
