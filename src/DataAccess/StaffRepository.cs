using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Authentication;
using Contracts;
using Logger;
using Models;

namespace DataAccess
{
    /// <summary>
    /// Represents staff repository.
    /// </summary>
    public sealed class StaffRepository : RepositoryBase, IStaffRepository<StaffModel, ItemModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public StaffRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<StaffModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Staffs
                        orderby c.SurName ascending
                        join p in this.Entities.Positions on c.PositionID equals p.ID
                        select new StaffModel
                        {
                            Id = c.ID,
                            FullName = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                            SurName = c.SurName ?? string.Empty,
                            FirstName = c.FirstName ?? string.Empty,
                            MiddleName = c.MiddleName ?? string.Empty,
                            Gender = c.Sex,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            PositionId = c.PositionID,
                            PositionName = p.Name,
                            EducationId = c.EducationID,
                            IsTaking = c.IsTaking,
                            PhoneNumberCell = c.PhoneNumberCell,
                            PhoneNumberHome = c.PhoneNumberHome,
                            PhoneNumberWork = c.PhoneNumberWork,
                            Email = c.Email,
                            Comments = c.Comments,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All staff have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<StaffModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted);

                watch.Stop();

                Log.Debug(string.Format("All staff except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get only taking staff for drop down list.
        /// </summary>
        /// <returns>List of all records.</returns>
        public List<ItemModel> GetAllIsTakingForList()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Staffs
                        where c.IsTaking
                        where !c.IsDeleted
                        orderby c.SurName ascending
                        join p in this.Entities.Positions on c.PositionID equals p.ID
                        select new ItemModel
                        {
                            Id = c.ID,
                            Name = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                        };

                watch.Stop();

                Log.Debug(string.Format("All staff for dropdown list have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q.ToList();
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
        public StaffModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Staffs
                         join p in this.Entities.Positions on c.PositionID equals p.ID
                         where c.ID == id
                         select new StaffModel
                         {
                             Id = c.ID,
                             FullName = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                             SurName = c.SurName ?? string.Empty,
                             FirstName = c.FirstName ?? string.Empty,
                             MiddleName = c.MiddleName ?? string.Empty,
                             Gender = c.Sex,
                             Birthday = c.Birthday,
                             Address = c.Address,
                             PositionId = c.PositionID,
                             PositionName = p.Name,
                             EducationId = c.EducationID,
                             IsTaking = c.IsTaking,
                             PhoneNumberCell = c.PhoneNumberCell,
                             PhoneNumberHome = c.PhoneNumberHome,
                             PhoneNumberWork = c.PhoneNumberWork,
                             Email = c.Email,
                             Comments = c.Comments,
                             IsDeleted = c.IsDeleted,
                             Changed = c.Changed,
                             ChangedBy = c.ChangedBy,
                             Created = c.Created,
                             CreatedBy = c.CreatedBy
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("Staff with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new StaffModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(StaffModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new Staff
                {
                    SurName = tclass.SurName,
                    FirstName = tclass.FirstName,
                    MiddleName = tclass.MiddleName,
                    Sex = tclass.Gender,
                    Birthday = tclass.Birthday,
                    Address = tclass.Address,
                    PositionID = tclass.PositionId,
                    EducationID = tclass.EducationId,
                    PhoneNumberHome = tclass.PhoneNumberHome,
                    PhoneNumberWork = tclass.PhoneNumberWork,
                    PhoneNumberCell = tclass.PhoneNumberCell,
                    Email = tclass.Email,
                    IsTaking = tclass.IsTaking,
                    Comments = tclass.Comments,
                    IsDeleted = tclass.IsDeleted,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToStaffs(entity);

                watch.Stop();

                Log.Debug(string.Format("A new staff has been added. Took {0}", watch.Elapsed));
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
        public void Update(StaffModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Staffs
                             where c.ID == tclass.Id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.SurName = tclass.SurName;
                    query.FirstName = tclass.FirstName;
                    query.MiddleName = tclass.MiddleName;
                    query.Sex = tclass.Gender;
                    query.Birthday = tclass.Birthday;
                    query.Address = tclass.Address;
                    query.PositionID = tclass.PositionId;
                    query.EducationID = tclass.EducationId;
                    query.PhoneNumberHome = tclass.PhoneNumberHome;
                    query.PhoneNumberWork = tclass.PhoneNumberWork;
                    query.PhoneNumberCell = tclass.PhoneNumberCell;
                    query.Email = tclass.Email;
                    query.IsTaking = tclass.IsTaking;
                    query.Comments = tclass.Comments;
                    query.IsDeleted = tclass.IsDeleted;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The staff with id {0} have been updated. Took {1}", tclass.Id, watch.Elapsed));
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

                var query = (from c in this.Entities.Staffs
                             where c.ID == id
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The staff with id {0} has been deleted. Took {1}", id, watch.Elapsed));
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

                var query = (from c in this.Entities.Staffs
                             where c.ID == id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.IsDeleted = true;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The staff with id {0} has been hidden. Took {1}", id, watch.Elapsed));
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
