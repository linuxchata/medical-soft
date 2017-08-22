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
    /// Represents notification template repository.
    /// </summary>
    public sealed class NotificationTemplateRepository : RepositoryBase, INotificationTemplateRepository<NotificationTemplateModel, ItemModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param> 
        public NotificationTemplateRepository(dentistEntities entity, IAuthenticationSession authenticationSession) :
            base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all records include deleted.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<NotificationTemplateModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.NotificationTemplates
                        select new NotificationTemplateModel
                        {
                            Id = c.ID,
                            Description = c.Description,
                            Title = c.Title,
                            Body = c.Body,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All notification templates have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted.
        /// </summary> 
        /// <returns>List of all non-deleted records.</returns>
        public IEnumerable<NotificationTemplateModel> GetAllExceptDeleted()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = this.GetAll().Where(a => !a.IsDeleted).ToList();

                watch.Stop();

                Log.Debug(string.Format("All notification templates except deleted have been received. Took {0}", watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Get all records except deleted for drop down list.
        /// </summary>
        /// <returns>List of all records except deleted.</returns>
        public List<ItemModel> GetAllForList()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.NotificationTemplates
                        where !c.IsDeleted
                        select new ItemModel
                        {
                            Id = c.ID,
                            Name = c.Description,
                        };

                watch.Stop();

                Log.Debug(string.Format("All notification templates except deleted for dropdownlist have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public NotificationTemplateModel GetById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.NotificationTemplates
                        where c.ID == id
                        select new NotificationTemplateModel
                        {
                            Id = c.ID,
                            Description = c.Description,
                            Title = c.Title,
                            Body = c.Body,
                            IsDeleted = c.IsDeleted,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("The notification template with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q.SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationTemplateModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(NotificationTemplateModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new NotificationTemplate
                {
                    Description = tclass.Description,
                    Title = tclass.Title,
                    Body = tclass.Body,
                    IsDeleted = tclass.IsDeleted,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToNotificationTemplates(entity);

                watch.Stop();

                Log.Debug(string.Format("A new notification template has been added. Took {0}", watch.Elapsed));
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
        public void Update(NotificationTemplateModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var query = this.GetPocoById(tclass.Id);

                query.Description = tclass.Description;
                query.Title = tclass.Title;
                query.Body = tclass.Body;
                query.ChangedBy = userId;
                query.Changed = DateTime.Now;

                watch.Stop();

                Log.Debug(string.Format("The notification template with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
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

                var groups = from c in this.Entities.NotificationGroups
                             where c.TemplateId == id && !c.IsDeleted
                             select c;

                if (groups.Any())
                {
                    throw new Exception("Selected template is used.");
                }

                var query = this.GetPocoById(id);

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The notification template with id {0} has been deleted. Took {1}", id, watch.Elapsed));
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

                var groups = from c in this.Entities.NotificationGroups
                             where c.TemplateId == id && !c.IsDeleted
                             select c;

                if (groups.Any())
                {
                    return false;
                }

                var userId = this.AuthenticationSession.GetUserId();

                var query = this.GetPocoById(id);

                query.IsDeleted = true;
                query.ChangedBy = userId;
                query.Changed = DateTime.Now;

                watch.Stop();

                Log.Debug(string.Format("The notification template with id {0} has been hided. Took {1}", id, watch.Elapsed));

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
        private NotificationTemplate GetPocoById(int id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.NotificationTemplates
                         where c.ID == id
                         select c).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The notification template with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new NotificationTemplate();
            }
        }
    }
}
