using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Common.Authentication;
using DataAccess;
using Logger;
using Models;
using Models.Enumeration;
using Services.Settings;

namespace Services.Backup
{
    /// <summary>
    /// Represents class to perform backup of the database.
    /// </summary>
    public class Backuper : IBackuper
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IAuthenticationSession authenticationSession;

        private readonly ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Backuper"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="authenticationSession">Authentication session.</param>
        /// <param name="settingsService">Setting service.</param>
        public Backuper(
            IUnitOfWork unitOfWork,
            IAuthenticationSession authenticationSession,
            ISettingsService settingsService)
        {
            this.authenticationSession = authenticationSession;
            this.unitOfWork = unitOfWork;
            this.settingsService = settingsService;
        }

        /// <summary>
        /// Perform backup of the database.
        /// </summary>
        /// <param name="backupType">Type of backup.</param>
        public void PerformBackup(BackuperType backupType)
        {
            try
            {
                Log.Debug("Start performing backup of the database.");

                var watch = new Stopwatch();
                watch.Start();

                var backupLog = new BackupLogsModel
                {
                    BackupTypes = backupType,
                    StartDateTime = DateTime.Now,
                    StartedBy = this.authenticationSession.GetUserId()
                };

                var nameOfTheBackupDatabase = this.settingsService.Get(AvailableSettings.BackupDatabaseName);
                Log.Debug("Name of the backuped database is {0}.", Log.Args(nameOfTheBackupDatabase));

                var locationOfTheBackupFolder = this.settingsService.Get(AvailableSettings.BackupLocation);
                var nameOfTheBackupFile = this.settingsService.Get(AvailableSettings.BackupFileName);
                var locationOfTheBackupFile = this.DefineNameOfTheBackupFile(locationOfTheBackupFolder, nameOfTheBackupFile);

                var resultMessage = this.unitOfWork.BackupRepository.PerformBackup(nameOfTheBackupDatabase, locationOfTheBackupFile);
                if (resultMessage.IsNullOrEmpty())
                {
                    Log.Error("Process of the backup failed.");
                }

                backupLog.Status = resultMessage;
                backupLog.FileName = nameOfTheBackupFile;
                backupLog.EndDateTime = DateTime.Now;

                this.unitOfWork.BackupRepository.Add(backupLog);
                this.unitOfWork.Save();

                Log.Debug("Process of the backup ends. Took {0}.", Log.Args(watch.Elapsed));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        private string DefineNameOfTheBackupFile(string locationOfTheBackupFolder, string nameOfTheBackupFile)
        {
            var suffix = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) + DateTime.Now.Day + DateTime.Now.Year
            + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;

            nameOfTheBackupFile = nameOfTheBackupFile + suffix + ".bak";

            var locationOfTheBackupFile = Path.Combine(locationOfTheBackupFolder, nameOfTheBackupFile);
            Log.Debug("Full location of the backup file is {0}.", Log.Args(locationOfTheBackupFile));

            return locationOfTheBackupFile;
        }
    }
}
