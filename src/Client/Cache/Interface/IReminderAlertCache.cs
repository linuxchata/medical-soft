using Common.Cache;
using Models;

namespace Client.Cache.Interface
{
    /// <summary>
    /// Represents reminder alerts cache.
    /// </summary>
    public interface IReminderAlertCache : ICache<ReminderAlertModel>
    {
    }
}
