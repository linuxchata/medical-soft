using System;
using System.IO;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents collection model for setting.
    /// </summary>
    public sealed class SettingCollectionModel : ModelBase<SettingCollectionModel>
    {
        #region Private Fields

        /// <summary>
        /// Represented model for language setting.
        /// </summary>
        private SettingModel language;

        /// <summary>
        /// Represented model for location of the backup setting.
        /// </summary>
        private SettingModel backupLocation;

        /// <summary>
        /// Represented model for name of the backup database setting.
        /// </summary>
        private SettingModel backupDatabaseName;

        /// <summary>
        /// Represented model for name of the backup file setting.
        /// </summary>
        private SettingModel backupFileName;

        /// <summary>
        /// Represented model for backup hours.
        /// </summary>
        private SettingModel backupHour;

        /// <summary>
        /// Represented model for backup minutes.
        /// </summary>
        private SettingModel backupMinute;

        /// <summary>
        /// Represented model for duration between showing the same notification on UI in minutes setting.
        /// </summary>
        private SettingModel reminderCheckDelay;

        /// <summary>
        /// Represented model for Secure Sockets Layer setting.
        /// </summary>
        private SettingModel smtpEnableSsl;

        /// <summary>
        /// Represented model for address setting.
        /// </summary>
        private SettingModel smtpFromAddress;

        /// <summary>
        /// Represented model for host setting.
        /// </summary>
        private SettingModel smtpHost;

        /// <summary>
        /// Represented model for password setting.
        /// </summary>
        private SettingModel smtpPassword;

        /// <summary>
        /// Represented model for port setting.
        /// </summary>
        private SettingModel smtpPort;

        /// <summary>
        /// Represented model for user name setting.
        /// </summary>
        private SettingModel smtpUserName;

        /// <summary>
        /// Represented model for video device.
        /// </summary>
        private string videoDevice;

        #endregion

        /// <summary>
        /// Gets or sets model for language setting.
        /// </summary>
        public SettingModel Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value;
                this.OnPropertyChanged(() => this.Language);
            }
        }

        /// <summary>
        /// Gets or sets model for location of the backup setting.
        /// </summary>
        [Validatable]
        public SettingModel BackupLocation
        {
            get
            {
                return this.backupLocation;
            }

            set
            {
                this.backupLocation = value;
                this.OnPropertyChanged(() => this.BackupLocation);
            }
        }

        /// <summary>
        /// Gets or sets model for name of the backup database setting.
        /// </summary>
        public SettingModel BackupDatabaseName
        {
            get
            {
                return this.backupDatabaseName;
            }

            set
            {
                this.backupDatabaseName = value;
                this.OnPropertyChanged(() => this.BackupDatabaseName);
            }
        }

        /// <summary>
        /// Gets or sets model for name of the backup file setting.
        /// </summary>
        public SettingModel BackupFileName
        {
            get
            {
                return this.backupFileName;
            }

            set
            {
                this.backupFileName = value;
                this.OnPropertyChanged(() => this.BackupFileName);
            }
        }

        /// <summary>
        /// Gets or sets model for backup hours.
        /// </summary>
        public SettingModel BackupHour
        {
            get
            {
                return this.backupHour;
            }

            set
            {
                this.backupHour = value;
                this.OnPropertyChanged(() => this.BackupHour);
            }
        }

        /// <summary>
        /// Gets or sets model for backup minutes.
        /// </summary>
        public SettingModel BackupMinute
        {
            get
            {
                return this.backupMinute;
            }

            set
            {
                this.backupMinute = value;
                this.OnPropertyChanged(() => this.BackupMinute);
            }
        }

        /// <summary>
        /// Gets or sets model for duration between showing the same notification on UI in minutes setting.
        /// </summary>
        public SettingModel ReminderCheckDelay
        {
            get
            {
                return this.reminderCheckDelay;
            }

            set
            {
                this.reminderCheckDelay = value;
                this.OnPropertyChanged(() => this.ReminderCheckDelay);
            }
        }

        /// <summary>
        /// Gets or sets model for Secure Sockets Layer setting.
        /// </summary>
        public SettingModel SmtpEnableSsl
        {
            get
            {
                return this.smtpEnableSsl;
            }

            set
            {
                this.smtpEnableSsl = value;
                this.OnPropertyChanged(() => this.SmtpEnableSsl);
            }
        }

        /// <summary>
        /// Gets or sets model for address setting.
        /// </summary>
        public SettingModel SmtpFromAddress
        {
            get
            {
                return this.smtpFromAddress;
            }

            set
            {
                this.smtpFromAddress = value;
                this.OnPropertyChanged(() => this.SmtpFromAddress);
            }
        }

        /// <summary>
        /// Gets or sets model for host setting.
        /// </summary>
        public SettingModel SmtpHost
        {
            get
            {
                return this.smtpHost;
            }

            set
            {
                this.smtpHost = value;
                this.OnPropertyChanged(() => this.SmtpHost);
            }
        }

        /// <summary>
        /// Gets or sets model for password setting.
        /// </summary>
        public SettingModel SmtpPassword
        {
            get
            {
                return this.smtpPassword;
            }

            set
            {
                this.smtpPassword = value;
                this.OnPropertyChanged(() => this.SmtpPassword);
            }
        }

        /// <summary>
        /// Gets or sets model for port setting.
        /// </summary>
        public SettingModel SmtpPort
        {
            get
            {
                return this.smtpPort;
            }

            set
            {
                this.smtpPort = value;
                this.OnPropertyChanged(() => this.SmtpPort);
            }
        }

        /// <summary>
        /// Gets or sets model for user name setting.
        /// </summary>
        public SettingModel SmtpUserName
        {
            get
            {
                return this.smtpUserName;
            }

            set
            {
                this.smtpUserName = value;
                this.OnPropertyChanged(() => this.SmtpUserName);
            }
        }

        /// <summary>
        /// Gets or sets model for video device.
        /// </summary>
        public string VideoDevice
        {
            get
            {
                return this.videoDevice;
            }

            set
            {
                this.videoDevice = value;
                this.OnPropertyChanged(() => this.VideoDevice);
            }
        }

        /// <summary>
        /// Get validated error for current model.
        /// </summary>
        /// <param name="columnName">Validated property.</param>
        /// <returns>Returns validation error if any, otherwise, null.</returns>
        protected override string GetValidationError(string columnName)
        {
            switch (columnName)
            {
                case "BackupLocation":
                    if (this.BackupLocation != null && !Directory.Exists(this.BackupLocation.NvValue))
                    {
                        return "Wrong backup location";
                    }

                    break;
                default:
                    throw new ArgumentException("Unexpected property being validated " + columnName);
            }

            return null;
        }
    }
}
