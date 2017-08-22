using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Common.Authentication;
using Contracts;
using Logger;
using Models;
using Models.Enumeration;

namespace DataAccess
{
    /// <summary>
    /// Represents notification group repository.
    /// </summary>
    public sealed class NotificationGroupRepository : RepositoryBase, INotificationGroupRepository<NotificationGroupModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationGroupRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public NotificationGroupRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all records include deleted.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<NotificationGroupModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationGroups
                        join t in this.Entities.NotificationTemplates on c.TemplateId equals t.ID
                        join s in this.Entities.NotificationGroupStatus on c.Status equals s.ID
                        join ld in this.Entities.LanguageDatas on s.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        where !t.IsDeleted
                        select new NotificationGroupModel
                        {
                            Id = c.ID,
                            Description = c.Description,
                            TemplateId = c.TemplateId,
                            Template = t.Title,
                            StartDate = c.StartDate,
                            Status = c.Status,
                            StatusName = ld.Value,
                            CompletedDate = c.CompletedDate,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All notification groups have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<NotificationGroupModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted)
                                     .OrderByDescending(a => a.StartDate);

                watch.Stop();

                Log.Debug(string.Format("All notification groups except deleted have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get active (not completed) records except deleted record.
        /// </summary> 
        /// <returns>List of active non-deleted records.</returns>
        public IEnumerable<NotificationGroupModel> GetActiveExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAllExceptDeleted()
                    .Where(a => a.StartDate <= DateTime.Now &&
                        (a.Status == (int)NotificationGroupStatus.NotProcessed || a.Status == (int)NotificationGroupStatus.Processing));

                watch.Stop();

                Log.Debug(string.Format("Active notification groups except deleted have been received. Took {0}", watch.Elapsed));

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
        public NotificationGroupModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationGroups
                        join t in this.Entities.NotificationTemplates on c.TemplateId equals t.ID
                        join s in this.Entities.NotificationGroupStatus on c.Status equals s.ID
                        join ld in this.Entities.LanguageDatas on s.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        where c.ID == id
                        select new NotificationGroupModel
                        {
                            Id = c.ID,
                            Description = c.Description,
                            TemplateId = c.TemplateId,
                            Template = t.Title,
                            StartDate = c.StartDate,
                            Status = c.Status,
                            StatusName = ld.Value,
                            CompletedDate = c.CompletedDate,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("Notification group with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationGroupModel();
            }
        }

        /// <summary>
        /// Get record by unique identifier.
        /// </summary>
        /// <param name="id">Unique identifier of the record.</param>
        /// <returns>T class.</returns>
        public NotificationGroupModel GetByGuidId(Guid id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.NotificationGroups
                        join t in this.Entities.NotificationTemplates on c.TemplateId equals t.ID
                        join s in this.Entities.NotificationGroupStatus on c.Status equals s.ID
                        join ld in this.Entities.LanguageDatas on s.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        where c.UniqueId == id
                        select new NotificationGroupModel
                        {
                            Id = c.ID,
                            Description = c.Description,
                            TemplateId = c.TemplateId,
                            Template = t.Title,
                            StartDate = c.StartDate,
                            Status = c.Status,
                            StatusName = ld.Value,
                            CompletedDate = c.CompletedDate,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("Notification group with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationGroupModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(NotificationGroupModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new NotificationGroup
                {
                    UniqueId = tclass.UniqueId,
                    Description = tclass.Description,
                    TemplateId = tclass.TemplateId,
                    StartDate = tclass.StartDate,
                    Status = (int)NotificationGroupStatus.NotProcessed,
                    CompletedDate = tclass.CompletedDate,
                    IsDeleted = tclass.IsDeleted,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToNotificationGroups(entity);

                watch.Stop();

                Log.Debug(string.Format("A new notification group has been added. Took {0}", watch.Elapsed));
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
        public void Update(NotificationGroupModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var query = this.GetPocoById(tclass.Id);

                query.Description = tclass.Description;
                query.TemplateId = tclass.TemplateId;
                query.StartDate = tclass.StartDate;
                query.Status = tclass.Status;
                query.CompletedDate = tclass.CompletedDate;
                query.ChangedBy = userId;
                query.Changed = DateTime.Now;

                watch.Stop();

                Log.Debug(string.Format("The notification group with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
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

                Log.Debug(string.Format("The notification group with id {0} has been deleted. Took {1}", id, watch.Elapsed));
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

                Log.Debug(string.Format("The notification group with id {0} has been hided. Took {1}", id, watch.Elapsed));

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>Entity class.</returns>
        private NotificationGroup GetPocoById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.NotificationGroups
                         where c.ID == id
                         select c).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("Notification group with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationGroup();
            }
        }
    }
}
