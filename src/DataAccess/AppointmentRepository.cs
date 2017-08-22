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
    /// Represents appointment repository.
    /// </summary>
    public sealed class AppointmentRepository : RepositoryBase, IAppointmentRepository<AppointmentModel, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        public AppointmentRepository(dentistEntities entity, IAuthenticationSession authenticationSession)
            : base(entity, authenticationSession)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<AppointmentModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Appointments
                        join s in this.Entities.Staffs on c.DoctorId equals s.ID
                        join p in this.Entities.Patients on c.PatientId equals p.ID
                        where !s.IsDeleted
                        where !p.IsDeleted
                        select new AppointmentModel
                        {
                            Id = c.ID,
                            StartTime = c.StartTime,
                            EndTime = c.EndTime,
                            Item1 = (s.SurName ?? string.Empty) + " " + (s.FirstName ?? string.Empty) + " " + (s.MiddleName ?? string.Empty),
                            Item1Id = c.DoctorId,
                            Item2 = (p.SurName ?? string.Empty) + " " + (p.FirstName ?? string.Empty) + " " + (p.MiddleName ?? string.Empty),
                            Item2Id = c.PatientId,
                            Comment = c.Comment,
                            Changed = c.Changed,
                            ChangedBy = c.ChangedBy,
                            Created = c.Created,
                            CreatedBy = c.CreatedBy
                        };

                watch.Stop();

                Log.Debug(string.Format("All appointments have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<AppointmentModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        public AppointmentModel GetById(Guid id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Appointments
                         join s in this.Entities.Staffs on c.DoctorId equals s.ID
                         join p in this.Entities.Patients on c.PatientId equals p.ID
                         where c.ID == id
                         select new AppointmentModel
                         {
                             Id = c.ID,
                             StartTime = c.StartTime,
                             EndTime = c.EndTime,
                             Item1 = (s.SurName ?? string.Empty) + " " + (s.FirstName ?? string.Empty) + " " + (s.MiddleName ?? string.Empty),
                             Item1Id = c.DoctorId,
                             Item2 = (p.SurName ?? string.Empty) + " " + (p.FirstName ?? string.Empty) + " " + (p.MiddleName ?? string.Empty),
                             Item2Id = c.PatientId,
                             Comment = c.Comment,
                             Changed = c.Changed,
                             ChangedBy = c.ChangedBy,
                             Created = c.Created,
                             CreatedBy = c.CreatedBy
                         }).SingleOrDefault();

                watch.Stop();

                Log.Debug(string.Format("The appointment with id {0} has been received. Took {1}", id, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new AppointmentModel();
            }
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(AppointmentModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var userId = this.AuthenticationSession.GetUserId();

                var entity = new Appointment
                {
                    ID = Guid.NewGuid(),
                    StartTime = tclass.StartTime,
                    EndTime = tclass.EndTime,
                    DoctorId = tclass.Item1Id,
                    PatientId = tclass.Item2Id,
                    Comment = tclass.Comment,
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    ChangedBy = userId,
                    Changed = DateTime.Now
                };

                this.Entities.AddToAppointments(entity);

                watch.Stop();

                Log.Debug(string.Format("A new record has been added. Took {0}", watch.Elapsed));
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
        public void Update(AppointmentModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Appointments
                             where c.ID == tclass.Id
                             select c).SingleOrDefault();

                var userId = this.AuthenticationSession.GetUserId();

                if (query != null)
                {
                    query.StartTime = tclass.StartTime;
                    query.EndTime = tclass.EndTime;
                    query.DoctorId = tclass.Item1Id;
                    query.PatientId = tclass.Item2Id;
                    query.Comment = tclass.Comment;
                    query.ChangedBy = userId;
                    query.Changed = DateTime.Now;
                }

                watch.Stop();

                Log.Debug(string.Format("The appointment with id {0} has been updated. Took {1}", tclass.Id, watch.Elapsed));
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
        public void Delete(Guid id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var query = (from c in this.Entities.Appointments
                             where c.ID == id
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The appointment with id {0} has been deleted. Took {1}", id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
    }
}
