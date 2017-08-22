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
    /// Represents education repository.
    /// </summary>
    public sealed class EducationRepository : RepositoryBase, IEducationRepository<EducationModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EducationRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EducationRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<EducationModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Educations
                        select new EducationModel
                        {
                            Id = c.ID,
                            ShortName = c.ShortName,
                            Name = c.Name,
                            IsDeleted = c.IsDeleted
                        };

                watch.Stop();

                Log.Debug(string.Format("All educations have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<EducationModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted);

                watch.Stop();

                Log.Debug(string.Format("All educations except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        public EducationModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Educations
                         where c.ID == id
                         select new EducationModel
                         {
                             Id = c.ID,
                             ShortName = c.ShortName,
                             Name = c.Name,
                             IsDeleted = c.IsDeleted
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The education with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new EducationModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(EducationModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var entity = new Education
                {
                    ShortName = tclass.ShortName,
                    Name = tclass.Name,
                    IsDeleted = tclass.IsDeleted
                };

                this.Entities.AddToEducations(entity);

                watch.Stop();

                Log.Debug(string.Format("A new education has been added. Took {0}", watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Update record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Update(EducationModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Educations
                             where c.ID == tclass.Id
                             select c).SingleOrDefault();

                if (query != null)
                {
                    query.Name = tclass.Name;
                    query.ShortName = tclass.ShortName;
                    query.IsDeleted = tclass.IsDeleted;
                }

                watch.Stop();

                Log.Debug(string.Format("The education with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete record.
        /// </summary>
        /// <param name="id">The identification.</param>
        public void Delete(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var staff = from c in this.Entities.Staffs
                            where c.EducationID == id
                            select c;

                if (staff.Any())
                {
                    throw new Exception("Selected education is used.");
                }

                var query = (from c in this.Entities.Educations
                             where c.ID == id
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The education with id {0} has been deleted. Took {1}", id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Try hide record.
        /// </summary>
        /// <param name="id">The identification.</param>
        /// <returns>Returns true if record was marked as deleted; otherwise, false.</returns>
        public bool TryHide(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var staff = from c in this.Entities.Staffs
                            where c.EducationID == id && !c.IsDeleted
                            select c;

                if (staff.Any())
                {
                    return false;
                }

                var query = (from c in this.Entities.Educations
                             where c.ID == id
                             select c).SingleOrDefault();

                if (query != null)
                {
                    query.IsDeleted = true;
                }

                watch.Stop();

                Log.Debug(string.Format("The education with id {0} has been hidden. Took {1}", id, watch.Elapsed));

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
    }
}
