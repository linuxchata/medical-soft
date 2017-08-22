using System;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for editor image.
    /// </summary>
    public sealed class EditorImage : ModelBase<EditorImage>
    {
        #region Private Fields

        /// <summary>
        /// The description of the image.
        /// </summary>
        private string description;

        /// <summary>
        /// The location of the image.
        /// </summary>
        private string locaion;

        #endregion

        /// <summary>
        /// Gets or sets the description of the image.
        /// </summary>
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
        /// Gets or sets the location of the image.
        /// </summary>
        [Validatable]
        public string Location
        {
            get
            {
                return this.locaion;
            }

            set
            {
                this.locaion = value;
                this.OnPropertyChanged(() => this.Location);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(EditorImage a, EditorImage b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<EditorImage>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(EditorImage a, EditorImage b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Location.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as EditorImage);
        }

        /// <summary>
        /// Map image information.
        /// </summary>
        /// <param name="copy">Target image.</param>
        /// <returns>Returns a new image.</returns>
        public EditorImage Map(EditorImage copy = null)
        {
            if (copy == null)
            {
                copy = new EditorImage();
            }

            copy.Description = this.Description;
            copy.Location = this.Location;

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
                case "Location":
                    if (this.Location.IsNullOrEmpty() || this.Location.Trim().IsNullOrEmpty())
                    {
                        return "Location name is required";
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
        private static bool CompareObjects(EditorImage a, EditorImage b)
        {
            return a.Location == b.Location;
        }
    }
}
