using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents gender cache.
    /// </summary>
    public class GenderCache : Common.Cache.Cache<GenderModel>, IGenderCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenderCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public GenderCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "Genders";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.AddItemToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<GenderModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<GenderModel>;
        }

        private void AddItemToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.GenderRepository.GetAll().ToList();
            this.AddObjectToCache(items);
        }
    }
}
