using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents educations cache.
    /// </summary>
    public class EducationCache : Common.Cache.Cache<EducationModel>, IEducationCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EducationCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public EducationCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "Educations";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.GetAddItemsToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<EducationModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<EducationModel>;
        }

        private void GetAddItemsToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.EducationRepository.GetAllExceptDeleted().ToList();
            this.AddObjectToCache(items);
        }
    }
}
