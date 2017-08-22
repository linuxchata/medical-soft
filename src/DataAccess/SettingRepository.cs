using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Contracts;
using Logger;
using Models;
using Models.Enumeration;

namespace DataAccess
{
    /// <summary>
    /// Represents setting repository.
    /// </summary>
    public sealed class SettingRepository : RepositoryBase, ISettingRepository<SettingModel, AvailableSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingRepository"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public SettingRepository(dentistEntities entity) :
            base(entity)
        {
        }

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>List of all records.</returns>
        public IEnumerable<SettingModel> GetAll()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = from c in this.Entities.Settings
                        select new SettingModel
                        {
                            NvKey = c.nvKey,
                            NvValue = c.nvValue,
                            IntValue = c.intValue,
                            BitValue = c.bitValue
                        };

                watch.Stop();

                Log.Debug(string.Format("All settings have been received. Count is {0}. Took {1}", q.Count(), watch.Elapsed));

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
        public IEnumerable<SettingModel> GetAllExceptDeleted()
        {
            return this.GetAll();
        }

        /// <summary>
        /// Get record by id.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <returns>T class.</returns>
        public SettingModel GetById(AvailableSettings id)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var textKey = id.ToString();

                var q = (from c in this.Entities.Settings
                         where c.nvKey.Equals(textKey, StringComparison.InvariantCultureIgnoreCase)
                         select new SettingModel
                         {
                             NvKey = c.nvKey,
                             NvValue = c.nvValue,
                             IntValue = c.intValue,
                             BitValue = c.bitValue
                         }).FirstOrDefault();

                watch.Stop();

                var result = string.Empty;

                if (!q.NvValue.IsNullOrEmpty())
                {
                    result = q.NvValue;
                }
                else if (q.IntValue.HasValue)
                {
                    result = q.IntValue.Value.ToString(CultureInfo.InvariantCulture);
                }
                else if (q.BitValue.HasValue)
                {
                    result = q.BitValue.Value.ToString();
                }

                Log.Debug(string.Format("The setting with id {0} and value {1} has been received. Took {2}", id, result, watch.Elapsed));

                return q;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return new SettingModel();
            }
        }

        /// <summary>
        /// Update record.
        /// </summary>
        /// <param name="tclass">Model class.</param>
        public void Update(SettingModel tclass)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                var q = (from c in this.Entities.Settings
                         where c.nvKey == tclass.NvKey
                         select c).SingleOrDefault();

                if (q != null)
                {
                    q.nvKey = tclass.NvKey;
                    q.nvValue = tclass.NvValue;
                    q.intValue = tclass.IntValue;
                    q.bitValue = tclass.BitValue;
                }

                watch.Stop();

                Log.Debug(string.Format("The setting with nvKey {0} have been updated. Took {1}", tclass.NvKey, watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
    }
}
