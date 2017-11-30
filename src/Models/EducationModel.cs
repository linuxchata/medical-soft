using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    using System.Diagnostics;

    /// <summary>
    /// Represents model class for education.
    /// </summary>
    [Table("Education")]
    [DebuggerDisplay("{Name}")]
    public sealed class EducationModel : ModelBase2<EducationModel>
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets identification of the education.
        /// </summary>
        private int id;

        /// <summary>
        /// Name of the university.
        /// </summary>
        private string name;

        /// <summary>
        /// Short name of the university.
        /// </summary>
        private string shortName;

        /// <summary>
        /// Value indicating whether education was deleted.
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Gets or sets identification of the education.
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
        /// Gets or sets name of the university.
        /// </summary>
        [MaxLength(200)]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.OnPropertyChanged(() => this.Name);
            }
        }

        /// <summary>
        /// Gets or sets short name of the university.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(50)]
        public string ShortName
        {
            get
            {
                return this.shortName;
            }

            set
            {
                this.shortName = value;
                this.OnPropertyChanged(() => this.ShortName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether education was deleted.
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
        public static bool operator ==(EducationModel a, EducationModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<EducationModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(EducationModel a, EducationModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as EducationModel);
        }

        /// <summary>
        /// Map education information.
        /// </summary>
        /// <param name="copy">Target education.</param>
        /// <returns>Returns a new education.</returns>
        public EducationModel Map(EducationModel copy = null)
        {
            if (copy == null)
            {
                copy = new EducationModel();
            }

            copy.Id = this.Id;
            copy.Name = this.Name;
            copy.ShortName = this.ShortName;
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
                case "ShortName":
                    if (this.ShortName.IsNullOrEmpty())
                    {
                        return "Short name is required";
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
        private static bool CompareObjects(EducationModel a, EducationModel b)
        {
            return a.Id == b.Id;
        }
    }
}
