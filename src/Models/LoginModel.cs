using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents login model class.
    /// </summary>
    [Table("Login")]
    public sealed class LoginModel : ModelBase2<LoginModel>
    {
        /// <summary>
        /// Validated properties.
        /// </summary>
        private static readonly string[] ShortValidatableProperties =
        {
            "LoginName"
        };

        #region Private Fields

        /// <summary>
        /// Identification of the login.
        /// </summary>
        private int id;

        /// <summary>
        /// Full validation.
        /// </summary>
        private bool isFullValidation;

        /// <summary>
        /// Login name.
        /// </summary>
        private string loginName;

        /// <summary>
        /// The password.
        /// </summary>
        private string password;

        /// <summary>
        /// Confirm password.
        /// </summary>
        private string confirmPassword;

        /// <summary>
        /// Can login to the program.
        /// </summary>
        private bool isCanLogin;

        /// <summary>
        /// User have to change password.
        /// </summary>
        private bool isHaveToChangePass;

        /// <summary>
        /// Role in the system.
        /// </summary>
        private int roleInSystem;

        /// <summary>
        /// Role in the system.
        /// </summary>
        private string roleNameInSystem;

        /// <summary>
        /// Staff id.
        /// </summary>
        private int staffId;

        /// <summary>
        /// Staff full name.
        /// </summary>
        private string staffName;

        /// <summary>
        /// Is login is super administrator.
        /// </summary>
        private bool isSa;

        /// <summary>
        /// Is login was deleted.
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public LoginModel()
        {
            this.isFullValidation = true;
        }

        /// <summary>
        /// Gets or sets full validation.
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
        /// Gets or sets a value indicating whether model has to be full validated.
        /// </summary>
        [NotMapped]
        public bool IsFullValidation
        {
            get
            {
                return this.isFullValidation;
            }

            set
            {
                this.isFullValidation = value;
            }
        }

        /// <summary>
        /// Gets or sets login name.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(50)]
        public string LoginName
        {
            get
            {
                return this.loginName;
            }

            set
            {
                this.loginName = value;
                this.OnPropertyChanged(() => this.LoginName);
            }
        }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(200)]
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;

                this.OnPropertyChanged(() => this.Password);
                this.OnPropertyChanged(() => this.ConfirmPassword);
            }
        }

        /// <summary>
        /// Gets or sets confirm password.
        /// </summary>
        [Validatable]
        [NotMapped]
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                if (this.confirmPassword == value)
                {
                    return;
                }

                this.confirmPassword = value;
                this.OnPropertyChanged(() => this.ConfirmPassword);
                this.OnPropertyChanged(() => this.Password);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether staff can login to the system.
        /// </summary>
        public bool IsCanLogin
        {
            get
            {
                return this.isCanLogin;
            }

            set
            {
                this.isCanLogin = value;
                this.OnPropertyChanged(() => this.IsCanLogin);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether user have to change password.
        /// </summary>
        public bool IsHaveToChangePass
        {
            get
            {
                return this.isHaveToChangePass;
            }

            set
            {
                this.isHaveToChangePass = value;
                this.OnPropertyChanged(() => this.IsHaveToChangePass);
            }
        }

        /// <summary>
        /// Gets or sets role in the system.
        /// </summary>
        [Validatable]
        [Required]
        public int RoleInSystem
        {
            get
            {
                return this.roleInSystem;
            }

            set
            {
                this.roleInSystem = value;
                this.OnPropertyChanged(() => this.RoleInSystem);
            }
        }

        /// <summary>
        /// Gets or sets role in the system.
        /// </summary>
        [NotMapped]
        public string RoleNameInSystem
        {
            get
            {
                return this.roleNameInSystem;
            }

            set
            {
                this.roleNameInSystem = value;
                this.OnPropertyChanged(() => this.RoleNameInSystem);
            }
        }

        /// <summary>
        /// Gets or sets staff id.
        /// </summary>
        [Validatable]
        [Required]
        public int StaffId
        {
            get
            {
                return this.staffId;
            }

            set
            {
                this.staffId = value;
                this.OnPropertyChanged(() => this.StaffId);
            }
        }

        /// <summary>
        /// Gets or sets staff full name.
        /// </summary>
        [NotMapped]
        public string StaffName
        {
            get
            {
                return this.staffName;
            }

            set
            {
                this.staffName = value;
                this.OnPropertyChanged(() => this.StaffName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether login is super administrator.
        /// </summary>
        public bool IsSa
        {
            get
            {
                return this.isSa;
            }

            set
            {
                this.isSa = value;
                this.OnPropertyChanged(() => this.IsSa);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether login was deleted.
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
        /// Gets a value indicating whether the object hasn't validation errors.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (this.isFullValidation)
                {
                    return ValidatableProperties.All(p => this.GetValidationError(p.Name) == null);
                }

                return ShortValidatableProperties.All(p => this.GetValidationError(p) == null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether password and confirmation password are equal.
        /// </summary>
        private bool ConfirmPasswordDoesnotMatchPassword
        {
            get
            {
                return string.Equals(this.Password, this.ConfirmPassword, StringComparison.InvariantCulture);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Returns true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(LoginModel a, LoginModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<LoginModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(LoginModel a, LoginModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as LoginModel);
        }

        /// <summary>
        /// Map login information.
        /// </summary>
        /// <param name="copy">Target login.</param>
        /// <returns>Returns a new login.</returns>
        public LoginModel Map(LoginModel copy = null)
        {
            if (copy == null)
            {
                copy = new LoginModel();
            }

            copy.Id = this.Id;
            copy.LoginName = this.LoginName;
            copy.Password = this.Password;
            copy.ConfirmPassword = this.ConfirmPassword;
            copy.IsCanLogin = this.IsCanLogin;
            copy.IsHaveToChangePass = this.IsHaveToChangePass;
            copy.RoleInSystem = this.RoleInSystem;
            copy.RoleNameInSystem = this.RoleNameInSystem;
            copy.StaffId = this.StaffId;
            copy.StaffName = this.StaffName;
            copy.IsSa = this.IsSa;
            copy.IsDeleted = this.IsDeleted;

            base.Map(copy);

            return copy;
        }

        /// <summary>
        /// Get validated error for current model
        /// </summary>
        /// <param name="columnName">Validated property</param>
        /// <returns>Returns validation error.</returns>
        protected override string GetValidationError(string columnName)
        {
            if (this.isFullValidation)
            {
                switch (columnName)
                {
                    case "LoginName":
                        if (this.LoginName.IsNullOrEmpty())
                        {
                            return "Name is required";
                        }

                        break;

                    case "Password":
                        if (this.Password.IsNullOrEmpty())
                        {
                            return "Password is required";
                        }

                        break;

                    case "ConfirmPassword":
                        if (this.ConfirmPassword.IsNullOrEmpty() || !this.ConfirmPasswordDoesnotMatchPassword)
                        {
                            return "Password confirmation is required";
                        }

                        break;

                    case "RoleInSystem":
                        if (this.RoleInSystem < 1)
                        {
                            return "Role is required";
                        }

                        break;

                    case "StaffId":
                        if (this.StaffId < 1)
                        {
                            return "Staff is required";
                        }

                        break;

                    default:
                        throw new ArgumentException("Unexpected property being validated " + columnName);
                }
            }
            else
            {
                switch (columnName)
                {
                    case "LoginName":
                        if (this.LoginName.IsNullOrEmpty())
                        {
                            return "Name is required";
                        }

                        break;

                    default:
                        throw new ArgumentException("Unexpected property being validated " + columnName);
                }
            }

            return null;
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(LoginModel a, LoginModel b)
        {
            return a.Id == b.Id;
        }
    }
}
