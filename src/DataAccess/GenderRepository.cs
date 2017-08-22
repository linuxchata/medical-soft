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
    /// Represents gender repository.
    /// </summary>
    public sealed class GenderRepository : RepositoryBase, IGenderRepository<GenderModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenderRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public GenderRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<GenderModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.Sexs
                        join ld in this.Entities.LanguageDatas on c.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        select new GenderModel
                        {
                            Id = c.ID,
                            Name = ld.Value
                        };

                watch.Stop();

                Log.Debug(string.Format("All sexs have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q;
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
        public IEnumerable<GenderModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }
    }
}
