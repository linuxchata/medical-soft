using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enumeration;

namespace Models
{
    /// <summary>
    /// Represents model class for backup logs.
    /// </summary>
    [Table("BackupLogs")]
    public sealed class BackupLogsModel : ModelBase<BackupLogsModel>
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets identification of the backup.
        /// </summary>
        private int id;

        /// <summary>
        /// Gets or sets backup type.
        /// </summary>
        private BackuperType backupTypes;

        /// <summary>
        /// Gets or sets backup type name.
        /// </summary>
        private string backupTypesName;

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        private DateTime startDateTime;

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        private DateTime endDateTime;

        /// <summary>
        /// Gets or sets status of the backup.
        /// </summary>
        private string status;

        /// <summary>
        /// Gets or sets name of the backup file.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Gets or sets started by.
        /// </summary>
        private int startedBy;

        /// <summary>
        /// Gets or sets started by
        /// </summary>
        private string nameStartedBy;

        #endregion

        /// <summary>
        /// Gets or sets identification of the backup
        /// </summary>
        [Key]
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
                this.OnPropertyChanged(() => this.Id);
            }
        }

        /// <summary>
        /// Gets or sets backup type.
        /// </summary>
        [Required]
        public BackuperType BackupTypes
        {
            get
            {
                return this.backupTypes;
            }

            set
            {
                this.backupTypes = value;
                this.OnPropertyChanged(() => this.BackupTypes);
            }
        }

        /// <summary>
        /// Gets or sets backup type name.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string BackupTypesName
        {
            get
            {
                return this.backupTypesName;
            }

            set
            {
                this.backupTypesName = value;
                this.OnPropertyChanged(() => this.BackupTypesName);
            }
        }

        /// <summary>
        /// Gets or sets start time
        /// </summary>
        [Required]
        public DateTime StartDateTime
        {
            get
            {
                return this.startDateTime;
            }

            set
            {
                this.startDateTime = value;
                this.OnPropertyChanged(() => this.StartDateTime);
            }
        }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        [Required]
        public DateTime EndDateTime
        {
            get
            {
                return this.endDateTime;
            }

            set
            {
                this.endDateTime = value;
                this.OnPropertyChanged(() => this.EndDateTime);
            }
        }

        /// <summary>
        /// Gets or sets status of the backup.
        /// </summary>
        [MaxLength(4000)]
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.OnPropertyChanged(() => this.Status);
            }
        }

        /// <summary>
        /// Gets or sets name of the backup file.
        /// </summary>
        [MaxLength(200)]
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
                this.OnPropertyChanged(() => this.FileName);
            }
        }

        /// <summary>
        /// Gets or sets started by.
        /// </summary>
        [Required]
        public int StartedBy
        {
            get
            {
                return this.startedBy;
            }

            set
            {
                this.startedBy = value;
                this.OnPropertyChanged(() => this.StartedBy);
            }
        }

        /// <summary>
        /// Gets or sets started by.
        /// </summary>
        [Required]
        public string NameStartedBy
        {
            get
            {
                return this.nameStartedBy;
            }

            set
            {
                this.nameStartedBy = value;
                this.OnPropertyChanged(() => this.NameStartedBy);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(BackupLogsModel a, BackupLogsModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<BackupLogsModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(BackupLogsModel a, BackupLogsModel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Id;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as BackupLogsModel);
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(BackupLogsModel a, BackupLogsModel b)
        {
            return a.Id == b.Id;
        }
    }
}