using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Contracts;
using Logger;
using Models;

namespace DataAccess
{
    /// <summary>
    /// Represents backup repository.
    /// </summary>
    public sealed class BackupRepository : RepositoryBase, IBackupRepository<BackupLogsModel, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public BackupRepository(dentistEntities entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Perform backup of the database.
        /// </summary>
        /// <param name="nameOfDb">Name of the database.</param>
        /// <param name="locationOfTheBackup">Location of the backup file.</param>
        /// <returns>Returns result of the backup.</returns>
        public string PerformBackup(string nameOfDb, string locationOfTheBackup)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var returnValue = this.Entities.sp_BackUpOfTheDataBase(nameOfDb, locationOfTheBackup).ToList();
                var result = returnValue.SingleOrDefault();

                Log.Debug(string.Format(result + ". Took {0}", watch.Elapsed));

                return result;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<BackupLogsModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var culture = Thread.CurrentThread.CurrentCulture.ToString();

                var q = from c in this.Entities.BackupLogs
                        join l in this.Entities.Logins on c.StartedBy equals l.ID
                        join b in this.Entities.BackupTypes on c.BackupTypesId equals b.ID
                        join ld in this.Entities.LanguageDatas on b.KeyId equals ld.KeyId
                        join cl in this.Entities.Cultures on ld.CultureId equals cl.ID
                        where cl.Name == culture
                        orderby c.StartDateTime descending
                        select new BackupLogsModel
                        {
                            Id = c.Id,
                            BackupTypesName = ld.Value,
                            EndDateTime = c.EndDateTime,
                            StartDateTime = c.StartDateTime,
                            Status = c.Status,
                            FileName = c.FileName,
                            StartedBy = l.ID,
                            NameStartedBy = l.LoginName
                        };

                watch.Stop();

                Log.Debug(string.Format("All backups have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<BackupLogsModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }

        /// <summary>
        /// Add a new record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Add(BackupLogsModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var entity = new BackupLog
                {
                    BackupTypesId = Convert.ToInt32(tclass.BackupTypes),
                    StartDateTime = tclass.StartDateTime,
                    EndDateTime = tclass.EndDateTime,
                    FileName = tclass.FileName,
                    Status = tclass.Status,
                    StartedBy = tclass.StartedBy
                };

                this.Entities.AddToBackupLogs(entity);

                watch.Stop();

                Log.Debug(string.Format("A new backup has been added. Took {0}", watch.Elapsed));
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

                var query = (from c in this.Entities.BackupLogs
                             where c.Id == id
                             select c).SingleOrDefault();

                this.Entities.DeleteObject(query);

                watch.Stop();

                Log.Debug(string.Format("The backup with id {0} has been deleted. Took {1}", id, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
    }
}
