using Common.Cache;
using Models;

namespace Client.Cache.Interface
{
    /// <summary>
    /// Represents cultures cache.
    /// </summary>
    public interface ICultureCache : ICache<CultureModel>
    {
    }
}
