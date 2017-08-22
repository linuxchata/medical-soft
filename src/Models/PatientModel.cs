using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Models
{
    /// <summary>
    /// Represents model class for patient.
    /// </summary>
    [Table("Patient")]
    [Serializable]
    public sealed class PatientModel : PersonModel<PatientModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the patient
        /// </summary>
        private int id;

        /// <summary>
        /// The photo.
        /// </summary>
        private byte[] photo;

        /// <summary>
        /// Maximum size of the photo in pixels.
        /// </summary>
        private int maximumSizeOfPhotoInPixels;

        /// <summary>
        /// Maximum size of the photo in bytes.
        /// </summary>
        private int maximumSizeOfPhotoInBytes;

        /// <summary>
        /// Ambulance card
        /// </summary>
        private int cardNumber;

        /// <summary>
        /// Registration date
        /// </summary>
        private DateTime registrationDate;

        /// <summary>
        /// The job
        /// </summary>
        private string job;

        /// <summary>
        /// The profession
        /// </summary>
        private string profession;

        /// <summary>
        /// Is email notifications allowed.
        /// </summary>
        private bool isEmailNotificationAllowed;

        /// <summary>
        /// Is email checked.
        /// </summary>
        private bool isEmailChecked;

        /// <summary>
        /// Is email checked enabled.
        /// </summary>
        private bool isEmailCheckedEnabled;

        /// <summary>
        /// Is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Is locked.
        /// </summary>
        private bool isLocked;

        #endregion

        /// <summary>
        /// Gets or sets identification of the patient.
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
        /// Gets or sets the photo.
        /// </summary>
        public byte[] Photo
        {
            get
            {
                return this.photo;
            }

            set
            {
                this.photo = value;
                this.OnPropertyChanged(() => this.Photo);
            }
        }

        /// <summary>
        /// Gets or sets maximum size of the photo in pixels.
        /// </summary>
        public int MaximumSizeOfPhotoInPixels
        {
            get
            {
                return this.maximumSizeOfPhotoInPixels;
            }

            set
            {
                this.maximumSizeOfPhotoInPixels = value;
                this.OnPropertyChanged(() => this.MaximumSizeOfPhotoInPixels);
            }
        }
        
        /// <summary>
        /// Gets or sets maximum size of the photo in bytes.
        /// </summary>
        public int MaximumSizeOfPhotoInBytes
        {
            get
            {
                return this.maximumSizeOfPhotoInBytes;
            }

            set
            {
                this.maximumSizeOfPhotoInBytes = value;
                this.OnPropertyChanged(() => this.MaximumSizeOfPhotoInBytes);
            }
        }

        /// <summary>
        /// Gets or sets ambulance card.
        /// </summary>
        public int CardNumber
        {
            get
            {
                return this.cardNumber;
            }

            set
            {
                this.cardNumber = value;
                this.OnPropertyChanged(() => this.CardNumber);
            }
        }

        /// <summary>
        /// Gets or sets registration date.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public DateTime RegistrationDate
        {
            get
            {
                return this.registrationDate;
            }

            set
            {
                this.registrationDate = value;
                this.OnPropertyChanged(() => this.RegistrationDate);
            }
        }

        /// <summary>
        /// Gets or sets job.
        /// </summary>
        [MaxLength(200)]
        public string Job
        {
            get
            {
                return this.job;
            }

            set
            {
                this.job = value;
                this.OnPropertyChanged(() => this.Job);
            }
        }

        /// <summary>
        /// Gets or sets profession.
        /// </summary>
        [MaxLength(100)]
        public string Profession
        {
            get
            {
                return this.profession;
            }

            set
            {
                this.profession = value;
                this.OnPropertyChanged(() => this.Profession);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether email notifications allowed.
        /// </summary>
        [Required]
        public bool IsEmailNotificationAllowed
        {
            get
            {
                return this.isEmailNotificationAllowed;
            }

            set
            {
                this.isEmailNotificationAllowed = value;
                this.OnPropertyChanged(() => this.IsEmailNotificationAllowed);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether email checked.
        /// </summary>
        [Required]
        public bool IsEmailChecked
        {
            get
            {
                return this.isEmailChecked;
            }

            set
            {
                this.isEmailChecked = value;
                this.OnPropertyChanged(() => this.IsEmailChecked);
                this.OnPropertyChanged(() => this.IsEmailValid);
            }
        }

        /// <summary>
        /// Gets a value indicating whether email is valid.
        /// </summary>
        [NotMapped]
        public bool IsEmailValid
        {
            get
            {
                return this.Email.IsNullOrEmpty() || this.IsEmailChecked;
            }
        }

        /// <summary>
        /// Gets a value indicating whether email checked enabled.
        /// </summary>
        [NotMapped]
        public bool IsEmailCheckedEnabled
        {
            get
            {
                return this.isEmailCheckedEnabled;
            }

            private set
            {
                this.isEmailCheckedEnabled = value;
                this.OnPropertyChanged(() => this.IsEmailCheckedEnabled);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether patient selected.
        /// </summary>
        [NotMapped]
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                // Patient that has already received notification in some group
                // cannot be deleted from notification list for that group.
                this.isSelected = this.IsLocked || value;

                this.OnPropertyChanged(() => this.IsSelected);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether patient locked.
        /// This property is related to the e-mail notification module.
        /// Patient that has already received notification in some group 
        /// cannot be deleted from notification list for that group.
        /// </summary>
        [NotMapped]
        public bool IsLocked
        {
            get
            {
                return this.isLocked;
            }

            set
            {
                this.isLocked = value;
                this.OnPropertyChanged(() => this.IsLocked);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(PatientModel a, PatientModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<PatientModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(PatientModel a, PatientModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as PatientModel);
        }

        /// <summary>
        /// Map patient information.
        /// </summary>
        /// <param name="copy">Target appointment.</param>
        /// <returns>Returns a new patient.</returns>
        public PatientModel Map(PatientModel copy = null)
        {
            if (copy == null)
            {
                copy = new PatientModel();
            }

            copy.Id = this.Id;
            copy.Photo = this.Photo;
            copy.CardNumber = this.CardNumber;
            copy.RegistrationDate = this.RegistrationDate;
            copy.Job = this.Job;
            copy.Profession = this.Profession;
            copy.IsEmailNotificationAllowed = this.IsEmailNotificationAllowed;
            copy.IsEmailChecked = this.IsEmailChecked;
            copy.IsEmailCheckedEnabled = this.IsEmailCheckedEnabled;
            copy.IsSelected = this.IsSelected;
            copy.IsLocked = this.IsLocked;

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
                case "SurName":
                    if (this.SurName.IsNullOrEmpty())
                    {
                        return "SurName is required";
                    }

                    break;

                case "FirstName":
                    if (this.FirstName.IsNullOrEmpty())
                    {
                        return "FirstName is required";
                    }

                    break;

                case "Email":
                    if (!this.Email.IsNullOrEmpty())
                    {
                        var expression = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.IgnoreCase);

                        if (!expression.IsMatch(this.Email))
                        {
                            this.IsEmailCheckedEnabled = false;
                            return "E-mail address is invalid";
                        }

                        this.IsEmailCheckedEnabled = true;
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
        private static bool CompareObjects(PatientModel a, PatientModel b)
        {
            return a.Id == b.Id;
        }
    }
}
