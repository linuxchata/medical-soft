using Common.Cache;
using Models;

namespace Client.Cache.Interface
{
    /// <summary>
    /// Represents positions cache.
    /// </summary>
    public interface IPositionCache : ICache<PositionModel>
    {
    }
}
