namespace Models
{
    /// <summary>
    /// Represents model class for about program information.
    /// </summary>
    public sealed class AboutModel : ModelBase<AboutModel>
    {
        #region Private Fields

        private string productVersion;

        private string databaseVersion;

        #endregion

        /// <summary>
        /// Gets or sets product version.
        /// </summary>
        public string ProductVersion
        {
            get
            {
                return this.productVersion;
            }

            set
            {
                this.productVersion = value;
                this.OnPropertyChanged(() => this.ProductVersion);
            }
        }

        /// <summary>
        /// Gets or sets database version.
        /// </summary>
        public string DatabaseVersion
        {
            get
            {
                return this.databaseVersion;
            }

            set
            {
                this.databaseVersion = value;
                this.OnPropertyChanged(() => this.DatabaseVersion);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(AboutModel a, AboutModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<AboutModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(AboutModel a, AboutModel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.ProductVersion.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as AboutModel);
        }

        private static bool CompareObjects(AboutModel a, AboutModel b)
        {
            return a.ProductVersion == b.ProductVersion &&
                   a.DatabaseVersion == b.DatabaseVersion;
        }
    }
}
