using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Models.Attribute;
using Models.Enumeration;

namespace Models
{
    /// <summary>
    /// Represents model class for staff.
    /// </summary>
    [Table("Staff")]
    public sealed class StaffModel : PersonModel<StaffModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the staff.
        /// </summary>
        private int id;

        /// <summary>
        /// Education identification.
        /// </summary>
        private int educationId;

        /// <summary>
        /// Education name.
        /// </summary>
        private string educationName;

        /// <summary>
        /// Position identification.
        /// </summary>
        private int positionId;

        /// <summary>
        /// Position name.
        /// </summary>
        private string positionName;

        /// <summary>
        /// Is taking staff.
        /// </summary>
        private bool isTaking;

        #endregion

        /// <summary>
        /// Gets or sets identification of the staff.
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
        /// Gets or sets education identification.
        /// </summary>
        [Validatable]
        [Required]
        public int EducationId
        {
            get
            {
                return this.educationId;
            }

            set
            {
                this.educationId = value;
                this.OnPropertyChanged(() => this.EducationId);
            }
        }

        /// <summary>
        /// Gets or sets education name.
        /// </summary>
        [NotMapped]
        public string EducationName
        {
            get
            {
                return this.educationName;
            }

            set
            {
                this.educationName = value;
                this.OnPropertyChanged(() => this.EducationName);
            }
        }

        /// <summary>
        /// Gets or sets position identification.
        /// </summary>
        [Validatable]
        [Required]
        public int PositionId
        {
            get
            {
                return this.positionId;
            }

            set
            {
                this.positionId = value;
                this.OnPropertyChanged(() => this.PositionId);
            }
        }

        /// <summary>
        /// Gets or sets position name.
        /// </summary>
        [NotMapped]
        public string PositionName
        {
            get
            {
                return this.positionName;
            }

            set
            {
                this.positionName = value;
                this.OnPropertyChanged(() => this.PositionName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether staff is taking.
        /// </summary>
        [Required]
        public bool IsTaking
        {
            get
            {
                return this.isTaking;
            }

            set
            {
                this.isTaking = value;
                this.OnPropertyChanged(() => this.IsTaking);
            }
        }

        /// <summary>
        /// Create a new staff model.
        /// </summary>
        /// <returns>Returns created staff model</returns>
        public static StaffModel Create()
        {
            return new StaffModel
            {
                Gender = (int)GenderType.Male
            };
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(StaffModel a, StaffModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<StaffModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(StaffModel a, StaffModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as StaffModel);
        }

        /// <summary>
        /// Map staff information.
        /// </summary>
        /// <param name="copy">Target staff.</param>
        /// <returns>Returns a new staff.</returns>
        public StaffModel Map(StaffModel copy = null)
        {
            if (copy == null)
            {
                copy = new StaffModel();
            }

            copy.Id = this.Id;
            copy.EducationId = this.EducationId;
            copy.EducationName = this.EducationName;
            copy.PositionId = this.PositionId;
            copy.PositionName = this.PositionName;
            copy.IsTaking = this.IsTaking;

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

                case "PositionId":
                    if (this.PositionId == 0)
                    {
                        return "Position is required";
                    }

                    break;

                case "EducationId":
                    if (this.EducationId == 0)
                    {
                        return "Education is required";
                    }

                    break;

                case "Email":
                    if (!this.Email.IsNullOrEmpty())
                    {
                        var expression = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", RegexOptions.IgnoreCase);

                        if (!expression.IsMatch(this.Email))
                        {
                            return "E-mail address is invalid";
                        }
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
        private static bool CompareObjects(StaffModel a, StaffModel b)
        {
            return a.Id == b.Id;
        }
    }
}
