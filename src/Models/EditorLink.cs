using System;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for editor link.
    /// </summary>
    public sealed class EditorLink : ModelBase<EditorLink>
    {
        #region Private Fields

        /// <summary>
        /// The name of the link.
        /// </summary>
        private string name;

        /// <summary>
        /// The location of the link.
        /// </summary>
        private string locaion;

        #endregion

        /// <summary>
        /// Gets or sets the name of the link.
        /// </summary>
        [Validatable]
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
        /// Gets or sets the location of the link.
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
        public static bool operator ==(EditorLink a, EditorLink b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<EditorLink>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(EditorLink a, EditorLink b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as EditorLink);
        }

        /// <summary>
        /// Map image information.
        /// </summary>
        /// <param name="copy">Target image.</param>
        /// <returns>Returns a new image.</returns>
        public EditorLink Map(EditorLink copy = null)
        {
            if (copy == null)
            {
                copy = new EditorLink();
            }

            copy.Name = this.Name;
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
                case "Name":
                    if (this.Name.IsNullOrEmpty() || this.Name.Trim().IsNullOrEmpty())
                    {
                        return "Link name is required";
                    }

                    break;
                case "Location":
                    Uri uriResult;
                    var result = Uri.TryCreate(this.Location, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (!result)
                    {
                        return "Link URL is required";
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
        private static bool CompareObjects(EditorLink a, EditorLink b)
        {
            return a.Location == b.Location;
        }
    }
}
