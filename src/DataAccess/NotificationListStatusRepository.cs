using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Contracts;
using Logger;
using Models;

namespace DataAccess
{
    /// <summary>
    /// Represents notification list status repository.
    /// </summary>
    public sealed class NotificationListStatusRepository : RepositoryBase, INotificationListStatusRepository<NotificationListStatusModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationListStatusRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public NotificationListStatusRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<NotificationListStatusModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationListStatus
                        join ld in this.Entities.LanguageDatas on c.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        select new NotificationListStatusModel
                        {
                            Id = c.ID,
                            Name = ld.Value
                        };

                watch.Stop();

                Log.Debug(string.Format("All notification list statuses have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q.ToList();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted record.
        /// </summary> 
        /// <returns>List of all non-deleted records.</returns>
        public IEnumerable<NotificationListStatusModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }
    }
}
