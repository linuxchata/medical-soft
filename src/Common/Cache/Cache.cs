using System.Collections.Generic;
using System.Runtime.Caching;
using Logger;

namespace Common.Cache
{
    /// <summary>
    /// Represents base class for caching.
    /// </summary>
    /// <typeparam name="T">Item model.</typeparam>
    public abstract class Cache<T> : ICache<T>
    {
        /// <summary>
        /// Initializes a new instance of the Cache class.
        /// </summary>
        protected Cache()
        {
            this.Policy = new CacheItemPolicy();
            this.CacheObject = MemoryCache.Default;
        }

        /// <summary>
        /// Gets or sets memory cache.
        /// </summary>
        protected ObjectCache CacheObject { get; set; }

        /// <summary>
        /// Gets or sets cache policy.
        /// </summary>
        protected CacheItemPolicy Policy { get; set; }

        /// <summary>
        /// Gets or sets name of the cache key.
        /// </summary>
        protected string CacheKeyName { get; set; }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public abstract List<T> Get();

        /// <summary>
        /// Clear cached object from the Memory Cache.
        /// </summary>
        public void Clear()
        {
            Log.Debug("Cache with key {0} has been cleared.", Log.Args(this.CacheKeyName));
            this.CacheObject.Remove(this.CacheKeyName);
        }

        /// <summary>
        /// Add object to the Memory Cache.
        /// </summary>
        /// <param name="items">Items to add.</param>
        protected void AddObjectToCache(object items)
        {
            this.CacheObject.Set(this.CacheKeyName, items, this.Policy);
            Log.Debug("Cache item with the key {0} has been added.", Log.Args(this.CacheKeyName));
        }
    }
}
