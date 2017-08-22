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
    /// Represents patient repository.
    /// </summary>
    public sealed class PatientRepository : RepositoryBase, IPatientRepository<PatientModel, ItemModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public PatientRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all patients include deleted.
        /// </summary>
        /// <returns>Returns list of all the patients.</returns>
        public IEnumerable<PatientModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Patients
                        orderby c.SurName ascending
                        select new PatientModel
                        {
                            Id = c.ID,
                            FullName = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                            SurName = c.SurName ?? string.Empty,
                            FirstName = c.FirstName ?? string.Empty,
                            MiddleName = c.MiddleName ?? string.Empty,
                            Photo = c.Photo,
                            Gender = c.Sex,
                            Birthday = c.Birthday,
                            Address = c.Address,
                            Job = c.Job,
                            Profession = c.Profession,
                            PhoneNumberCell = c.PhoneNumberCell,
                            PhoneNumberHome = c.PhoneNumberHome,
                            PhoneNumberWork = c.PhoneNumberWork,
                            Email = c.Email,
                            IsEmailNotificationAllowed = c.IsEmailNotificationAllowed,
                            IsEmailChecked = c.IsEmailChecked,
                            Comments = c.Comments,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All patients have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all patients except deleted.
        /// </summary> 
        /// <returns>Returns list of all non-deleted patients.</returns>
        public IEnumerable<PatientModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted);

                watch.Stop();

                Log.Debug(string.Format("All patients except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get patients for drop down list.
        /// </summary>
        /// <returns>Returns list of all patients for drop down list.</returns>
        public List<ItemModel> GetAllForList()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Patients
                        orderby c.SurName ascending
                        where !c.IsDeleted
                        select new ItemModel
                        {
                            Id = c.ID,
                            Name = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                        };

                watch.Stop();

                Log.Debug(string.Format("All patients for list have been received. Took {0}", watch.Elapsed));

                return q.ToList();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Search patients for drop down list.
        /// </summary>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns>Returns IEnumerable of patients for drop down list.</returns>
        public List<ItemModel> SearchForList(string searchPattern)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Patients
                         orderby c.SurName ascending
                         where c.SurName.Contains(searchPattern)
                         where !c.IsDeleted
                         select new ItemModel
                         {
                             Id = c.ID,
                             Name = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                         }).ToList();

                q = q.Where(c => c.Name.Contains(searchPattern)).ToList();

                watch.Stop();

                Log.Debug(string.Format("All patients for list have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get patient by id.
        /// </summary>
        /// <param name="id">Id of the patient.</param>
        /// <returns>Returns patient with id.</returns>
        public PatientModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Patients
                         where c.ID == id
                         select new PatientModel
                         {
                             Id = c.ID,
                             FullName = (c.SurName ?? string.Empty) + " " + (c.FirstName ?? string.Empty) + " " + (c.MiddleName ?? string.Empty),
                             SurName = c.SurName ?? string.Empty,
                             FirstName = c.FirstName ?? string.Empty,
                             MiddleName = c.MiddleName ?? string.Empty,
                             Photo = c.Photo,
                             Gender = c.Sex,
                             Birthday = c.Birthday,
                             Address = c.Address,
                             Job = c.Job,
                             Profession = c.Profession,
                             PhoneNumberCell = c.PhoneNumberCell,
                             PhoneNumberHome = c.PhoneNumberHome,
                             PhoneNumberWork = c.PhoneNumberWork,
                             Email = c.Email,
                             IsEmailNotificationAllowed = c.IsEmailNotificationAllowed,
                             IsEmailChecked = c.IsEmailChecked,
                             Comments = c.Comments,
                             IsDeleted = c.IsDeleted,
                             Changed = c.Changed,
                             ChangedBy = c.ChangedBy,
                             Created = c.Created,
                             CreatedBy = c.CreatedBy
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("Patient with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new PatientModel();
            }
        }

        /// <summary>
        /// Add a new patient.
        /// </summary>
        /// <param name="tclass">Patient common entity.</param>
        public void Add(PatientModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new Patient
                {
                    RegistrationDate = DateTime.Now,
                    CardNumber = 1,
                    SurName = tclass.SurName,
                    FirstName = tclass.FirstName,
                    MiddleName = tclass.MiddleName,
                    Photo = tclass.Photo,
                    Sex = tclass.Gender,
                    Birthday = tclass.Birthday,
                    Address = tclass.Address,
                    Job = tclass.Job,
                    Profession = tclass.Profession,
                    PhoneNumberHome = tclass.PhoneNumberHome,
                    PhoneNumberWork = tclass.PhoneNumberWork,
                    PhoneNumberCell = tclass.PhoneNumberCell,
                    Email = tclass.Email,
                    IsEmailNotificationAllowed = tclass.IsEmailNotificationAllowed,
                    IsEmailChecked = tclass.IsEmailChecked,
                    Comments = tclass.Comments,
                    IsDeleted = tclass.IsDeleted,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToPatients(entity);

                watch.Stop();

                Log.Debug(string.Format("A new patient has been added. Took {0}", watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Update patient.
        /// </summary>
        /// <param name="tclass">Patient common entity.</param>
        public void Update(PatientModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Patients
                             where c.ID == tclass.Id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.CardNumber = tclass.CardNumber;
                    query.SurName = tclass.SurName;
                    query.FirstName = tclass.FirstName;
                    query.MiddleName = tclass.MiddleName;
                    query.Photo = tclass.Photo;
                    query.Sex = tclass.Gender;
                    query.Birthday = tclass.Birthday;
                    query.Address = tclass.Address;
                    query.Job = tclass.Job;
                    query.Profession = tclass.Profession;
                    query.PhoneNumberHome = tclass.PhoneNumberHome;
                    query.PhoneNumberWork = tclass.PhoneNumberWork;
                    query.PhoneNumberCell = tclass.PhoneNumberCell;
                    query.Email = tclass.Email;
                    query.IsEmailNotificationAllowed = tclass.IsEmailNotificationAllowed;
                    query.IsEmailChecked = tclass.IsEmailChecked;
                    query.Comments = tclass.Comments;
                    query.IsDeleted = tclass.IsDeleted;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The patient with id {0} have been updated. Took {1}", tclass.Id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Delete patient.
        /// </summary>
        /// <param name="id">Id of the patient.</param>
        public void Delete(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Patients
                             where c.ID == id
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The patient with id {0} has been deleted. Took {1}", id, watch.Elapsed));
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

                var query = (from c in this.Entities.Patients
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

                Log.Debug(string.Format("The patient with id {0} has been hidden. Took {1}", id, watch.Elapsed));

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
