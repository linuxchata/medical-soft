using Models.Enumeration;

namespace Models
{
    /// <summary>
    /// Represents model class for navigation.
    /// </summary>
    public sealed class NavigationModel : ModelBase<NavigationModel>
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets identification of the left navigation item.
        /// </summary>
        private NavigationLeftItem navigationLeftItemId;

        /// <summary>
        /// Gets or sets identification of the right navigation item.
        /// </summary>
        private NavigationRightItem navigationRightItemId;

        /// <summary>
        /// Name of the navigation.
        /// </summary>
        private string name;

        /// <summary>
        /// Style of the navigation.
        /// </summary>
        private string style;

        #endregion

        /// <summary>
        /// Gets or sets identification of the left navigation item.
        /// </summary>
        public NavigationLeftItem NavigationLeftItemId
        {
            get
            {
                return this.navigationLeftItemId;
            }

            set
            {
                this.navigationLeftItemId = value;
                this.OnPropertyChanged(() => this.NavigationLeftItemId);
            }
        }

        /// <summary>
        /// Gets or sets identification of the right navigation item.
        /// </summary>
        public NavigationRightItem NavigationRightItemId
        {
            get
            {
                return this.navigationRightItemId;
            }

            set
            {
                this.navigationRightItemId = value;
                this.OnPropertyChanged(() => this.NavigationRightItemId);
            }
        }

        /// <summary>
        /// Gets or sets name of the navigation.
        /// </summary>
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
        /// Gets or sets style of the navigation.
        /// </summary>
        public string Style
        {
            get
            {
                return this.style;
            }

            set
            {
                this.style = value;
                this.OnPropertyChanged(() => this.Style);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(NavigationModel a, NavigationModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<NavigationModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(NavigationModel a, NavigationModel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as NavigationModel);
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(NavigationModel a, NavigationModel b)
        {
            return a.Name == b.Name;
        }
    }
}
