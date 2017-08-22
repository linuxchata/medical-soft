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
    /// Represents culture repository.
    /// </summary>
    public sealed class UiCultureRepository : RepositoryBase, IUiCultureRepository<CultureModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UiCultureRepository"/> class
        /// </summary>
        /// <param name="entity">The entity.</param>
        public UiCultureRepository(dentistEntities entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>Returns list of all records.</returns>
        public IEnumerable<CultureModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Cultures
                        select new CultureModel
                        {
                            Id = c.ID,
                            Name = c.Name,
                            Description = c.Description
                        };

                watch.Stop();

                Log.Debug(string.Format("All UI cultures have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        /// <returns>Returns list of all non-deleted records.</returns>
        public IEnumerable<CultureModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }
    }
}
