using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents positions cache.
    /// </summary>
    public class PositionCache : Common.Cache.Cache<PositionModel>, IPositionCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public PositionCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "Positions";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.AddItemToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<PositionModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<PositionModel>;
        }

        private void AddItemToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.PositionRepository.GetAllExceptDeleted().ToList();
            this.AddObjectToCache(items);
        }
    }
}
