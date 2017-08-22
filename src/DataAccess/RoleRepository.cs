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
    /// Represents role repository.
    /// </summary>
    public sealed class RoleRepository : RepositoryBase, IRoleRepository<RoleModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public RoleRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<RoleModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Roles
                        select new RoleModel
                        {
                            Id = c.ID,
                            Name = c.Name
                        };

                watch.Stop();

                Log.Debug(string.Format("All roles have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<RoleModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        public RoleModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Roles
                         where c.ID == id
                         select new RoleModel
                         {
                             Id = c.ID,
                             Name = c.Name
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The role with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new RoleModel();
            }
        }
    }
}
