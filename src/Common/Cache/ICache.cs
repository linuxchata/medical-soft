using System.Collections.Generic;

namespace Common.Cache
{
    /// <summary>
    /// Represents base class for caching.
    /// </summary>
    /// <typeparam name="T">Item model.</typeparam>
    public interface ICache<T>
    {
        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        List<T> Get();

        /// <summary>
        /// Clear cache item.
        /// </summary>
        void Clear();
    }
}
