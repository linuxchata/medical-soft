using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents base model class for person.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public abstract class PersonModel<TModel> : ModelBase2<TModel>
        where TModel : class
    {
        #region Private Fields

        /// <summary>
        /// Full name of the person
        /// </summary>
        private string fullName;

        /// <summary>
        /// Surname of the person
        /// </summary>
        private string surName;

        /// <summary>
        /// First name of the person
        /// </summary>
        private string firstName;

        /// <summary>
        /// Middle name of the person
        /// </summary>
        private string middleName;

        /// <summary>
        /// The gender
        /// </summary>
        private int gender;

        /// <summary>
        /// The address
        /// </summary>
        private string address;

        /// <summary>
        /// Home phone number
        /// </summary>
        private string phoneNumberHome;

        /// <summary>
        /// Work phone number
        /// </summary>
        private string phoneNumberWork;

        /// <summary>
        /// Cell phone number
        /// </summary>
        private string phoneNumberCell;

        /// <summary>
        /// Email address
        /// </summary>
        private string email;

        /// <summary>
        /// The birthday
        /// </summary>
        private DateTime? birthday;

        /// <summary>
        /// The comments
        /// </summary>
        private string comments;

        /// <summary>
        /// Is patient was deleted
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Gets or sets full name of the person.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return this.fullName;
            }

            set
            {
                this.fullName = value;
                this.OnPropertyChanged(() => this.FullName);
            }
        }

        /// <summary>
        /// Gets or sets surname of the person.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(50)]
        public string SurName
        {
            get
            {
                return this.surName;
            }

            set
            {
                this.surName = value;
                this.OnPropertyChanged(() => this.SurName);
            }
        }

        /// <summary>
        /// Gets or sets first name of the person.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(50)]
        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                this.firstName = value;
                this.OnPropertyChanged(() => this.FirstName);
            }
        }

        /// <summary>
        /// Gets or sets middle name of the person.
        /// </summary>
        [MaxLength(50)]
        public string MiddleName
        {
            get
            {
                return this.middleName;
            }

            set
            {
                this.middleName = value;
                this.OnPropertyChanged(() => this.MiddleName);
            }
        }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public int Gender
        {
            get
            {
                return this.gender;
            }

            set
            {
                this.gender = value;
                this.OnPropertyChanged(() => this.Gender);
            }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [MaxLength(200)]
        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
                this.OnPropertyChanged(() => this.Address);
            }
        }

        /// <summary>
        /// Gets or sets home phone number.
        /// </summary>
        [MaxLength(50)]
        public string PhoneNumberHome
        {
            get
            {
                return this.phoneNumberHome;
            }

            set
            {
                this.phoneNumberHome = value;
                this.OnPropertyChanged(() => this.PhoneNumberHome);
            }
        }

        /// <summary>
        /// Gets or sets work phone number.
        /// </summary>
        [MaxLength(50)]
        public string PhoneNumberWork
        {
            get
            {
                return this.phoneNumberWork;
            }

            set
            {
                this.phoneNumberWork = value;
                this.OnPropertyChanged(() => this.PhoneNumberWork);
            }
        }

        /// <summary>
        /// Gets or sets cell phone number.
        /// </summary>
        [MaxLength(50)]
        public string PhoneNumberCell
        {
            get
            {
                return this.phoneNumberCell;
            }

            set
            {
                this.phoneNumberCell = value;
                this.OnPropertyChanged(() => this.PhoneNumberCell);
            }
        }

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Validatable]
        [MaxLength(50)]
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                this.email = value;
                this.OnPropertyChanged(() => this.Email);
            }
        }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        [MaxLength(10)]
        public DateTime? Birthday
        {
            get
            {
                return this.birthday;
            }

            set
            {
                this.birthday = value;
                this.OnPropertyChanged(() => this.Birthday);
            }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        [MaxLength(300)]
        public string Comments
        {
            get
            {
                return this.comments;
            }

            set
            {
                this.comments = value;
                this.OnPropertyChanged(() => this.Comments);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether patient is deleted.
        /// </summary>
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
        /// Map person information.
        /// </summary>
        /// <param name="copy">Copy of the object.</param>
        /// <returns>Returns copied object.</returns>
        protected PersonModel<TModel> Map(PersonModel<TModel> copy)
        {
            copy.FullName = this.FullName;
            copy.SurName = this.SurName;
            copy.FirstName = this.FirstName;
            copy.MiddleName = this.MiddleName;
            copy.gender = this.gender;
            copy.Address = this.Address;
            copy.PhoneNumberHome = this.PhoneNumberHome;
            copy.PhoneNumberWork = this.PhoneNumberWork;
            copy.PhoneNumberCell = this.PhoneNumberCell;
            copy.Email = this.Email;
            copy.Birthday = this.Birthday;
            copy.Comments = this.Comments;
            copy.IsDeleted = this.IsDeleted;

            base.Map(copy);

            return copy;
        }
    }
}