using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents roles cache.
    /// </summary>
    public class RoleCache : Common.Cache.Cache<RoleModel>, IRoleCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public RoleCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "Roles";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.AddItemToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<RoleModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<RoleModel>;
        }

        private void AddItemToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.RoleRepository.GetAll().ToList();
            this.AddObjectToCache(items);
        }
    }
}
