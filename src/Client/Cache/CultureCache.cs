using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents cultures cache.
    /// </summary>
    public class CultureCache : Common.Cache.Cache<CultureModel>, ICultureCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public CultureCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "Cultures";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.AddItemToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<CultureModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<CultureModel>;
        }

        private void AddItemToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.UiCultureRepository.GetAll().ToList();
            this.AddObjectToCache(items);
        }
    }
}
