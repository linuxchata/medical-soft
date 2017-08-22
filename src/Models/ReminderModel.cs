using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for reminder.
    /// </summary>
    [Table("Reminder")]
    public sealed class ReminderModel : ModelBase2<ReminderModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the reminder.
        /// </summary>
        private int id;

        /// <summary>
        /// Gets or sets date of the reminder.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Identification of the patient.
        /// </summary>
        private int? patientId;

        /// <summary>
        /// Patient's name.
        /// </summary>
        private string patient;

        /// <summary>
        /// Gets or sets patient's phone number.
        /// </summary>
        private string patientPhoneNumbers;

        /// <summary>
        /// Gets or sets patient's cell phone number.
        /// </summary>
        private string patientPhoneNumberCell;

        /// <summary>
        /// Gets or sets patient's home phone number.
        /// </summary>
        private string patientPhoneNumberHome;

        /// <summary>
        /// Gets or sets patient's work phone number.
        /// </summary>
        private string patientPhoneNumberWork;

        /// <summary>
        /// Identification of the doctor.
        /// </summary>
        private int? doctorId;

        /// <summary>
        /// Doctor's name.
        /// </summary>
        private string doctor;

        /// <summary>
        /// The message.
        /// </summary>
        private string message;

        /// <summary>
        /// Identification of the alert.
        /// </summary>
        private int alertId;

        /// <summary>
        /// Alert's days.
        /// </summary>
        private int alertDays;

        /// <summary>
        /// Gets or sets is reminder hidden.
        /// </summary>
        private bool isCompleted;

        /// <summary>
        /// The comment.
        /// </summary>
        private string comment;

        /// <summary>
        /// Is reminder was deleted.
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Gets or sets identification of the reminder.
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
        /// Gets or sets date of the reminder.
        /// </summary>
        [Required]
        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.date = value;
                this.OnPropertyChanged(() => this.Date);
            }
        }

        /// <summary>
        /// Gets formatted date of the reminder.
        /// </summary>
        [NotMapped]
        public string DateFormatted
        {
            get
            {
                return this.Date.ToShortDateString();
            }
        }

        /// <summary>
        /// Gets or sets identification of the patient.
        /// </summary>
        [Validatable]
        [Required]
        public int? PatientId
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
        /// Gets or sets patient's name.
        /// </summary>
        [NotMapped]
        public string Patient
        {
            get
            {
                return this.patient;
            }

            set
            {
                this.patient = value;
                this.OnPropertyChanged(() => this.Patient);
            }
        }

        /// <summary>
        /// Gets or sets patient's phone number.
        /// </summary>
        [NotMapped]
        public string PatientPhoneNumbers
        {
            get
            {
                return this.patientPhoneNumbers;
            }

            set
            {
                this.patientPhoneNumbers = value;
                this.OnPropertyChanged(() => this.PatientPhoneNumbers);
            }
        }

        /// <summary>
        /// Gets or sets patient's cell phone number.
        /// </summary>
        [NotMapped]
        public string PatientPhoneNumberCell
        {
            get
            {
                return this.patientPhoneNumberCell;
            }

            set
            {
                this.patientPhoneNumberCell = value;
                this.OnPropertyChanged(() => this.PatientPhoneNumberCell);
            }
        }

        /// <summary>
        /// Gets or sets patient's home phone number.
        /// </summary>
        [NotMapped]
        public string PatientPhoneNumberHome
        {
            get
            {
                return this.patientPhoneNumberHome;
            }

            set
            {
                this.patientPhoneNumberHome = value;
                this.OnPropertyChanged(() => this.PatientPhoneNumberHome);
            }
        }

        /// <summary>
        /// Gets or sets patient's work phone number.
        /// </summary>
        [NotMapped]
        public string PatientPhoneNumberWork
        {
            get
            {
                return this.patientPhoneNumberWork;
            }

            set
            {
                this.patientPhoneNumberWork = value;
                this.OnPropertyChanged(() => this.PatientPhoneNumberWork);
            }
        }

        /// <summary>
        /// Gets or sets identification of the doctor.
        /// </summary>
        [Required]
        public int? DoctorId
        {
            get
            {
                return this.doctorId;
            }

            set
            {
                this.doctorId = value;
                this.OnPropertyChanged(() => this.DoctorId);
            }
        }

        /// <summary>
        /// Gets or sets doctor's name.
        /// </summary>
        [NotMapped]
        public string Doctor
        {
            get
            {
                return this.doctor;
            }

            set
            {
                this.doctor = value;
                this.OnPropertyChanged(() => this.Doctor);
            }
        }

        /// <summary>
        /// Gets or sets message.
        /// </summary>
        [Validatable]
        [MaxLength(1024)]
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged(() => this.Message);
            }
        }

        /// <summary>
        /// Gets or sets identification of the alert.
        /// </summary>
        [Validatable]
        [Required]
        public int AlertId
        {
            get
            {
                return this.alertId;
            }

            set
            {
                this.alertId = value;
                this.OnPropertyChanged(() => this.AlertId);
            }
        }

        /// <summary>
        /// Gets or sets alert's days.
        /// </summary>
        [NotMapped]
        public int AlertDays
        {
            get
            {
                return this.alertDays;
            }

            set
            {
                this.alertDays = value;
                this.OnPropertyChanged(() => this.AlertDays);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether reminder is hidden.
        /// </summary>
        [Required]
        public bool IsCompleted
        {
            get
            {
                return this.isCompleted;
            }

            set
            {
                this.isCompleted = value;
                this.OnPropertyChanged(() => this.IsCompleted);
            }
        }

        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        [MaxLength(1024)]
        public string Comment
        {
            get
            {
                return this.comment;
            }

            set
            {
                this.comment = value;
                this.OnPropertyChanged(() => this.Comment);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether reminder is deleted.
        /// </summary>
        [Required]
        public bool IsDeleted
        {
            get
            {
                return this.isDeleted;
            }

            set
            {
                this.isDeleted = value;
                this.OnPropertyChanged(() => this.IsDeleted);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(ReminderModel a, ReminderModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<ReminderModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(ReminderModel a, ReminderModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as ReminderModel);
        }

        /// <summary>
        /// Map reminder information.
        /// </summary>
        /// <param name="copy">Target reminder.</param>
        /// <returns>Returns a new reminder.</returns>
        public ReminderModel Map(ReminderModel copy = null)
        {
            if (copy == null)
            {
                copy = new ReminderModel();
            }

            copy.Id = this.Id;
            copy.Date = this.Date;
            copy.PatientId = this.PatientId;
            copy.Patient = this.Patient;
            copy.PatientPhoneNumbers = this.PatientPhoneNumbers;
            copy.PatientPhoneNumberCell = this.PatientPhoneNumberCell;
            copy.PatientPhoneNumberHome = this.PatientPhoneNumberHome;
            copy.PatientPhoneNumberWork = this.PatientPhoneNumberWork;
            copy.DoctorId = this.DoctorId;
            copy.Doctor = this.Doctor;
            copy.Message = this.Message;
            copy.AlertId = this.AlertId;
            copy.AlertDays = this.AlertDays;
            copy.IsCompleted = this.IsCompleted;
            copy.Comment = this.Comment;
            copy.IsDeleted = this.IsDeleted;

            base.Map(copy);

            return copy;
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
                    if (!this.PatientId.HasValue || this.PatientId.Value == 0)
                    {
                        return "Patient is required";
                    }

                    break;

                case "DoctorId":
                    if (this.DoctorId == 0)
                    {
                        return "Doctor is required";
                    }

                    break;

                case "Message":
                    if (this.Message.IsNullOrEmpty())
                    {
                        return "Message is required";
                    }

                    break;

                case "AlertId":
                    if (this.AlertId == 0)
                    {
                        return "Alert is required";
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
        private static bool CompareObjects(ReminderModel a, ReminderModel b)
        {
            return a.Id == b.Id;
        }
    }
}