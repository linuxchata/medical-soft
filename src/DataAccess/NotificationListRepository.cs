using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Contracts;
using Logger;
using Models;
using Models.Enumeration;

namespace DataAccess
{
    /// <summary>
    /// Represents notification list repository.
    /// </summary>
    public sealed class NotificationListRepository :
        RepositoryBase,
        INotificationListRepository<NotificationListModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationListRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public NotificationListRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records include deleted.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<NotificationListModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationLists
                        join p in this.Entities.Patients on c.PatientId equals p.ID
                        join g in this.Entities.NotificationGroups on c.GroupId equals g.ID
                        join s in this.Entities.NotificationListStatus on c.Status equals s.ID
                        join ld in this.Entities.LanguageDatas on s.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        where !p.IsDeleted && !g.IsDeleted
                        select new NotificationListModel
                        {
                            Id = c.ID,
                            PatientEmail = c.Patient.Email,
                            PatientId = c.PatientId,
                            PatientName = (p.SurName ?? string.Empty) + " " + (p.FirstName ?? string.Empty) + " " + (p.MiddleName ?? string.Empty),
                            GroupId = c.GroupId,
                            Group = g.Description,
                            StartDate = c.StartDate,
                            SendDate = c.SendDate,
                            Status = c.Status,
                            StatusName = ld.Value,
                            ErrorDescription = c.ErrorDescription
                        };

                watch.Stop();

                Log.Debug(string.Format("All notification lists have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<NotificationListModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll();

                watch.Stop();

                Log.Debug(string.Format("All notifications lists except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted record by notification group id.
        /// Ordered by patient name.
        /// </summary>
        /// <param name="groupId">Id of the group.</param>
        /// <returns>List of all non-deleted records.</returns>
        public IEnumerable<NotificationListModel> GetAllExceptDeletedByGroupId(int groupId)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => a.GroupId == groupId).OrderBy(a => a.PatientName).ToList();

                watch.Stop();

                Log.Debug(string.Format("All notification lists except deleted by notification group id have been received. Count is {0}. Took {1}", q.Count, watch.Elapsed));

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
        public NotificationListModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationLists
                        join p in this.Entities.Patients on c.PatientId equals p.ID
                        join g in this.Entities.NotificationGroups on c.GroupId equals g.ID
                        join s in this.Entities.NotificationListStatus on c.Status equals s.ID
                        join ld in this.Entities.LanguageDatas on s.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        where c.ID == id
                        select new NotificationListModel
                        {
                            Id = c.ID,
                            PatientId = c.PatientId,
                            PatientEmail = c.Patient.Email,
                            PatientName = (p.SurName ?? string.Empty) + " " + (p.FirstName ?? string.Empty) + " " + (p.MiddleName ?? string.Empty),
                            GroupId = c.GroupId,
                            Group = g.Description,
                            StartDate = c.StartDate,
                            SendDate = c.SendDate,
                            Status = c.Status,
                            StatusName = ld.Value,
                            ErrorDescription = c.ErrorDescription
                        };

                watch.Stop();

                Log.Debug(string.Format("Notification list with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationListModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(NotificationListModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var entity = new NotificationList
                {
                    PatientId = tclass.PatientId,
                    GroupId = tclass.GroupId,
                    Status = (int)NotificationListStatus.NotSent
                };

                this.Entities.AddToNotificationLists(entity);

                watch.Stop();

                Log.Debug(string.Format("A new notification list has been added. Took {0}", watch.Elapsed));
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
        public void Update(NotificationListModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = this.GetPocoById(tclass.Id);

                query.PatientId = tclass.PatientId;
                query.GroupId = tclass.GroupId;
                query.StartDate = tclass.StartDate;
                query.SendDate = tclass.SendDate;
                query.Status = tclass.Status;
                query.ErrorDescription = tclass.ErrorDescription;

                watch.Stop();

                Log.Debug(string.Format("The notification list with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
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

                Log.Debug(string.Format("The notification list with id {0} has been deleted. Took {1}", id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>Entity class.</returns>
        private NotificationList GetPocoById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.NotificationLists
                         where c.ID == id
                         select c).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("Notification list with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationList();
            }
        }
    }
}
