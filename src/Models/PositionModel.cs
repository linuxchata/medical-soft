using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for position.
    /// </summary>
    [Table("Position")]
    public sealed class PositionModel : ModelBase2<PositionModel>
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets identification of the position.
        /// </summary>
        private int id;

        /// <summary>
        /// Gets or sets name of the position.
        /// </summary>
        private string name;

        /// <summary>
        /// Value indicating whether position was deleted.
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Gets or sets identification of the position.
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
        /// Gets or sets the name.
        /// </summary>
        [Validatable]
        [Required]
        [MaxLength(100)]
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
        /// Gets or sets a value indicating whether position is deleted.
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
        public static bool operator ==(PositionModel a, PositionModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<PositionModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(PositionModel a, PositionModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as PositionModel);
        }

        /// <summary>
        /// Map position information.
        /// </summary>
        /// <param name="copy">Target position.</param>
        /// <returns>Returns a new position.</returns>
        public PositionModel Map(PositionModel copy = null)
        {
            if (copy == null)
            {
                copy = new PositionModel();
            }

            copy.Id = this.Id;
            copy.Name = this.Name;
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
                case "Name":
                    if (this.Name.IsNullOrEmpty())
                    {
                        return "Name is required";
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
        private static bool CompareObjects(PositionModel a, PositionModel b)
        {
            return a.Id == b.Id;
        }
    }
}
