using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// Represents model class for culture.
    /// </summary>
    [Table("Culture")]
    public sealed class CultureModel : ModelBase<CultureModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the culture.
        /// </summary>
        private int id;

        /// <summary>
        /// Name of the culture.
        /// </summary>
        private string name;

        /// <summary>
        /// Description of the culture.
        /// </summary>
        private string description;

        #endregion

        /// <summary>
        /// Gets or sets identification of the culture.
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
        /// Gets or sets name of the culture.
        /// </summary>
        [Required]
        [MaxLength(5)]
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
        /// Gets or sets description of the culture.
        /// </summary>
        [MaxLength(50)]
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                this.OnPropertyChanged(() => this.Description);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(CultureModel a, CultureModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<CultureModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(CultureModel a, CultureModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as CultureModel);
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(CultureModel a, CultureModel b)
        {
            return a.Id == b.Id;
        }
    }
}
