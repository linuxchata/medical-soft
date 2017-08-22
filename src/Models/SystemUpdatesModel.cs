using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// Represents system updates model class.
    /// </summary>
    [Table("SystemUpdates")]
    public sealed class SystemUpdatesModel : ModelBase<SystemUpdatesModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the system update.
        /// </summary>
        private int id;

        /// <summary>
        /// System update information.
        /// </summary>
        private string updateInformation;

        /// <summary>
        /// System update version.
        /// </summary>
        private string updateVersion;

        /// <summary>
        /// System update version.
        /// </summary>
        private int updateVersionInt;

        /// <summary>
        /// System update installation.
        /// </summary>
        private DateTime updateDate;

        #endregion

        /// <summary>
        /// Gets or sets Identification of the system update.
        /// </summary>
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
        /// Gets or sets system update information.
        /// </summary>
        public string UpdateInformation
        {
            get
            {
                return this.updateInformation;
            }

            set
            {
                this.updateInformation = value;
                this.OnPropertyChanged(() => this.UpdateInformation);
            }
        }

        /// <summary>
        /// Gets or sets system update version.
        /// </summary>
        public string UpdateVersion
        {
            get
            {
                return this.updateVersion;
            }

            set
            {
                this.updateVersion = value;
                this.OnPropertyChanged(() => this.UpdateVersion);
            }
        }

        /// <summary>
        /// Gets or sets system update version.
        /// </summary>
        public int UpdateVersionInt
        {
            get
            {
                return this.updateVersionInt;
            }

            set
            {
                this.updateVersionInt = value;
                this.OnPropertyChanged(() => this.UpdateVersionInt);
            }
        }

        /// <summary>
        /// Gets or sets date of the system update installation.
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                return this.updateDate;
            }

            set
            {
                this.updateDate = value;
                this.OnPropertyChanged(() => this.UpdateDate);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Returns true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(SystemUpdatesModel a, SystemUpdatesModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<SystemUpdatesModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Returns true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(SystemUpdatesModel a, SystemUpdatesModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as SystemUpdatesModel);
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(SystemUpdatesModel a, SystemUpdatesModel b)
        {
            return a.Id == b.Id;
        }
    }
}
