using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for notification list.
    /// </summary>
    [Table("NotificationList")]
    public sealed class NotificationListModel : ModelBase<NotificationListModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the group.
        /// </summary>
        private int id;

        /// <summary>
        /// Identification of the reminder.
        /// </summary>
        private int patientId;

        /// <summary>
        /// Name of the patient.
        /// </summary>
        private string patientName;

        /// <summary>
        /// Email of the patient.
        /// </summary>
        private string patientEmail;

        /// <summary>
        /// Group id.
        /// </summary>
        private int groupId;

        /// <summary>
        /// Name of the group.
        /// </summary>
        private string group;

        /// <summary>
        /// Start date.
        /// </summary>
        private DateTime? startDate;

        /// <summary>
        /// Send date.
        /// </summary>
        private DateTime? sendDate;

        /// <summary>
        /// The status.
        /// </summary>
        private int status;

        /// <summary>
        /// Status name.
        /// </summary>
        private string statusName;

        /// <summary>
        /// Error description.
        /// </summary>
        private string errorDescription;

        #endregion

        /// <summary>
        /// Gets or sets identification of the group.
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
        /// Gets or sets description of the group.
        /// </summary>
        [Validatable]
        [Required]
        public int PatientId
        {
            get
            {
                return this.patientId;
            }

            set
            {
                this.patientId = value;
                this.OnPropertyChanged(() => this.PatientId);
            }
        }

        /// <summary>
        /// Gets or sets name of the patient.
        /// </summary>
        [NotMapped]
        public string PatientName
        {
            get
            {
                return this.patientName;
            }

            set
            {
                this.patientName = value;
                this.OnPropertyChanged(() => this.PatientName);
            }
        }

        /// <summary>
        /// Gets or sets email of the patient.
        /// </summary>
        [NotMapped]
        public string PatientEmail
        {
            get
            {
                return this.patientEmail;
            }

            set
            {
                this.patientEmail = value;
                this.OnPropertyChanged(() => this.PatientEmail);
            }
        }

        /// <summary>
        /// Gets or sets group id.
        /// </summary>
        [Validatable]
        [Required]
        public int GroupId
        {
            get
            {
                return this.groupId;
            }

            set
            {
                this.groupId = value;
                this.OnPropertyChanged(() => this.GroupId);
            }
        }

        /// <summary>
        /// Gets or sets name of the group.
        /// </summary>
        [NotMapped]
        public string Group
        {
            get
            {
                return this.group;
            }

            set
            {
                this.group = value;
                this.OnPropertyChanged(() => this.Group);
            }
        }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                return this.startDate;
            }

            set
            {
                this.startDate = value;
                this.OnPropertyChanged(() => this.StartDate);
            }
        }

        /// <summary>
        /// Gets or sets send date.
        /// </summary>
        public DateTime? SendDate
        {
            get
            {
                return this.sendDate;
            }

            set
            {
                this.sendDate = value;
                this.OnPropertyChanged(() => this.SendDate);
            }
        }
        
        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [Required]
        public int Status
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
        /// Gets or sets status name.
        /// </summary>
        [NotMapped]
        public string StatusName
        {
            get
            {
                return this.statusName;
            }

            set
            {
                this.statusName = value;
                this.OnPropertyChanged(() => this.StatusName);
            }
        }
        
        /// <summary>
        /// Gets or sets error description.
        /// </summary>
        public string ErrorDescription
        {
            get
            {
                return this.errorDescription;
            }

            set
            {
                this.errorDescription = value;
                this.OnPropertyChanged(() => this.ErrorDescription);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(NotificationListModel a, NotificationListModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<NotificationListModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(NotificationListModel a, NotificationListModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as NotificationListModel);
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
                case "PatientId":
                    if (this.PatientId == 0)
                    {
                        return "Patient is required";
                    }

                    break;

                case "GroupId":
                    if (this.GroupId == 0)
                    {
                        return "Group is required";
                    }

                    break;

                default:
                    throw new ArgumentException("Unexpected property being validated " + columnName);
            }

            return null;
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(NotificationListModel a, NotificationListModel b)
        {
            return a.Id == b.Id;
        }
    }
}