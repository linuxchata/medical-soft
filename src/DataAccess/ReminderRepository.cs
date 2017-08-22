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
    /// Represents reminder repository.
    /// </summary>
    public sealed class ReminderRepository : RepositoryBase, IReminderRepository<ReminderModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public ReminderRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all records include deleted.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<ReminderModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Reminders
                        join p in this.Entities.Patients on c.PatientId equals p.ID
                        join s in this.Entities.Staffs on c.DoctorId equals s.ID into doctors
                        join a in this.Entities.ReminderAlerts on c.AlertId equals a.ID
                        from d in doctors.DefaultIfEmpty()
                        where !p.IsDeleted && (d == null || (!d.IsDeleted && d.IsTaking))
                        orderby c.Date descending
                        select new ReminderModel
                        {
                            Id = c.ID,
                            Date = c.Date,
                            DoctorId = c.DoctorId,
                            Doctor = (d.SurName ?? string.Empty) + " " + (d.FirstName ?? string.Empty) + " " + (d.MiddleName ?? string.Empty),
                            PatientId = c.PatientId,
                            Patient = (p.SurName ?? string.Empty) + " " + (p.FirstName ?? string.Empty) + " " + (p.MiddleName ?? string.Empty),
                            PatientPhoneNumberWork = p.PhoneNumberWork,
                            PatientPhoneNumberCell = p.PhoneNumberCell,
                            PatientPhoneNumberHome = p.PhoneNumberHome,
                            Message = c.Message,
                            AlertId = c.AlertId,
                            AlertDays = a.Days,
                            IsCompleted = c.IsCompleted,
                            Comment = c.Comment,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All reminders have been received. Took {0}", watch.Elapsed));

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
        public IEnumerable<ReminderModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted);

                watch.Stop();

                Log.Debug(string.Format("All reminders except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get today's reminders for notification.
        /// </summary>
        /// <returns>List of today's reminders for notification.</returns>
        public IEnumerable<ReminderModel> GetActiveReminders()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted && !a.IsCompleted);
                var result = new List<ReminderModel>();

                var today = DateTime.Now;

                foreach (var reminder in q)
                {
                    var date = today.AddDays(reminder.AlertDays);

                    var dateToCompare = new DateTime(reminder.Date.Year, reminder.Date.Month, reminder.Date.Day, reminder.Date.Hour, reminder.Date.Minute, 0);

                    if (date >= dateToCompare)
                    {
                        result.Add(reminder);
                    }
                }

                watch.Stop();

                Log.Debug(string.Format("Todays reminders have been received. Took {0}", watch.Elapsed));

                return result;
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
        public ReminderModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Reminders
                         where c.ID == id
                         select new ReminderModel
                         {
                             Id = c.ID,
                             Date = c.Date,
                             DoctorId = c.DoctorId,
                             PatientId = c.PatientId,
                             Message = c.Message,
                             AlertId = c.AlertId,
                             IsCompleted = c.IsCompleted,
                             Comment = c.Comment,
                             IsDeleted = c.IsDeleted,
                             Changed = c.Changed,
                             ChangedBy = c.ChangedBy,
                             Created = c.Created,
                             CreatedBy = c.CreatedBy
                         }).ToList();

                watch.Stop();

                Log.Debug(string.Format("The reminder with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new ReminderModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(ReminderModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new Reminder
                {
                    Date = tclass.Date,
                    DoctorId = tclass.DoctorId != -1 ? tclass.DoctorId : null,
                    PatientId = tclass.PatientId ?? 0,
                    Message = tclass.Message,
                    AlertId = tclass.AlertId,
                    Comment = tclass.Comment,
                    IsDeleted = tclass.IsDeleted,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToReminders(entity);

                watch.Stop();

                Log.Debug(string.Format("A new reminder have been added. Took {0}", watch.Elapsed));
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
        public void Update(ReminderModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var query = this.GetPocoById(tclass.Id);

                query.Date = tclass.Date;
                query.DoctorId = tclass.DoctorId != -1 ? tclass.DoctorId : null;
                query.PatientId = tclass.PatientId ?? 0;
                query.Message = tclass.Message;
                query.AlertId = tclass.AlertId;
                query.IsCompleted = tclass.IsCompleted;
                query.Comment = tclass.Comment;
                query.IsDeleted = tclass.IsDeleted;
                query.ChangedBy = userId;
                query.Changed = DateTime.Now;

                watch.Stop();

                Log.Debug(string.Format("The reminder with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
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

                var query = this.GetPocoById(id);

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The reminder with id {0} has been deleted. Took {1}", id, watch.Elapsed));
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

                var userId = this.AuthenticationSession.GetUserId();

                var query = this.GetPocoById(id);

                query.IsDeleted = true;
                query.ChangedBy = userId;
                query.Changed = DateTime.Now;

                watch.Stop();

                Log.Debug(string.Format("The reminder with id {0} has been hided. Took {1}", id, watch.Elapsed));

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        private Reminder GetPocoById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Reminders
                         where c.ID == id
                         select c).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The reminder with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new Reminder();
            }
        }
    }
}
