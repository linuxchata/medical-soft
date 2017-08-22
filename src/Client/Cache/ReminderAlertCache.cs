using System.Collections.Generic;
using System.Linq;
using Client.Cache.Interface;
using DataAccess;
using Models;

namespace Client.Cache
{
    /// <summary>
    /// Represents reminder alerts cache.
    /// </summary>
    public class ReminderAlertCache : Common.Cache.Cache<ReminderAlertModel>, IReminderAlertCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderAlertCache"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        public ReminderAlertCache(IUnitOfWork unitOfWork)
        {
            this.CacheKeyName = "ReminderAlerts";

            if (!this.CacheObject.Contains(this.CacheKeyName))
            {
                this.AddItemToCache(unitOfWork);
            }
        }

        /// <summary>
        /// Get cached object.
        /// </summary>
        /// <returns>Returns cached object.</returns>
        public override List<ReminderAlertModel> Get()
        {
            return this.CacheObject.Get(this.CacheKeyName) as List<ReminderAlertModel>;
        }

        private void AddItemToCache(IUnitOfWork unitOfWork)
        {
            var items = unitOfWork.ReminderAlertRepository.GetAll().ToList();
            this.AddObjectToCache(items);
        }
    }
}
