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
    /// Represents reminder filter repository.
    /// </summary>
    public sealed class ReminderFilterRepository : RepositoryBase, IReminderFilterRepository<ReminderFilterModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderFilterRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public ReminderFilterRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<ReminderFilterModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.ReminderFilters
                        join ld in this.Entities.LanguageDatas on c.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        select new ReminderFilterModel
                        {
                            Id = c.ID,
                            Name = ld.Value
                        };

                watch.Stop();

                Log.Debug(string.Format("All reminder's filters have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<ReminderFilterModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }
    }
}
