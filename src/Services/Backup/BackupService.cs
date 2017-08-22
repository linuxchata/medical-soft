using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Common.Services;
using DataAccess;
using Models;
using Models.Enumeration;
using Services.Settings;

namespace Services.Backup
{
    /// <summary>
    /// Represents backup logic.
    /// </summary>
    public sealed class BackupService : ServiceBase, IBackupService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ISettingsService settingsService;

        private readonly IBackuper backuper;

        private int hours;

        private int minutes;

        private int delayInDays;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="settingsService">Settings service.</param>
        /// <param name="backuper">The object to perform backup of the database.</param>
        public BackupService(IUnitOfWork unitOfWork, ISettingsService settingsService, IBackuper backuper)
        {
            this.unitOfWork = unitOfWork;
            this.settingsService = settingsService;
            this.backuper = backuper;
        }

        /// <summary>
        /// Represents backup logic.
        /// </summary>
        protected override void Do()
        {
            this.GetBackupSettings();

            while (true)
            {
                var backups = this.unitOfWork.BackupRepository.GetAllExceptDeleted();
                if (backups != null)
                {
                    var nextBackupDate = this.GetNextBackupDate(backups);
                    if (this.IsBackupNeeded(nextBackupDate))
                    {
                        this.PerformBackup();
                    }
                }

                Thread.Sleep(TimeSpan.FromMinutes(30));
            }
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose();
        }

        private void GetBackupSettings()
        {
            this.hours = Convert.ToInt32(this.settingsService.Get(AvailableSettings.BackupHour));
            this.minutes = Convert.ToInt32(this.settingsService.Get(AvailableSettings.BackupMinute));
            this.delayInDays = this.settingsService.GetInt(AvailableSettings.BackupDelayInDays);
        }

        private DateTime GetNextBackupDate(IEnumerable<BackupLogsModel> backups)
        {
            var nextBackupDate = DateTime.MinValue;

            var lastBackupDate = backups.OrderBy(a => a.StartDateTime).LastOrDefault();
            if (lastBackupDate != null)
            {
                var year = lastBackupDate.StartDateTime.Year;
                var month = lastBackupDate.StartDateTime.Month;
                var day = lastBackupDate.StartDateTime.Day;
                nextBackupDate = new DateTime(year, month, day, this.hours, this.minutes, 0).AddDays(this.delayInDays);
            }

            return nextBackupDate;
        }

        private bool IsBackupNeeded(DateTime nextBackupDate)
        {
            return DateTime.Now >= nextBackupDate;
        }

        private void PerformBackup()
        {
            this.backuper.PerformBackup(BackuperType.Scheduled);
        }
    }
}
