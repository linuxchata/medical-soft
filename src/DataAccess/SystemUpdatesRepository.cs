using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contracts;
using Logger;
using Models;

namespace DataAccess
{
    /// <summary>
    /// Represents system updates repository.
    /// </summary>
    public sealed class SystemUpdatesRepository : RepositoryBase, ISystemUpdatesRepository<SystemUpdatesModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUpdatesRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public SystemUpdatesRepository(dentistEntities entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<SystemUpdatesModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.SystemUpdates
                        select new SystemUpdatesModel
                        {
                            Id = c.ID,
                            UpdateDate = c.UpdateDate,
                            UpdateVersion = c.UpdateVersion,
                            UpdateVersionInt = c.UpdateVersionInt,
                            UpdateInformation = c.UpdateInformation
                        };

                watch.Stop();

                Log.Debug(string.Format("All system updates have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q.ToList();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get current database version. 
        /// </summary>
        /// <returns>T class.</returns>
        public SystemUpdatesModel GetDatabaseVersion()
        {
            return this.GetAll().OrderByDescending(a => a.UpdateVersionInt).First();
        }
    }
}
