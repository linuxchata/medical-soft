﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// Represents model class for reminder filter.
    /// </summary>
    [Table("ReminderFilter")]
    public sealed class ReminderFilterModel : ModelBase<ReminderFilterModel>
    {
        #region Private Fields

        /// <summary>
        /// Gets or sets identification of the filter.
        /// </summary>
        private int id;

        /// <summary>
        /// Gets or sets name of the filter.
        /// </summary>
        private string name;

        /// <summary>
        /// Gets or sets filter's culture identification.
        /// </summary>
        private int cultureId;

        #endregion

        /// <summary>
        /// Gets or sets identification of the filter.
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
        /// Gets or sets name of the filter.
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
        /// Gets or sets filter's culture identification.
        /// </summary>
        public int CultureId
        {
            get
            {
                return this.cultureId;
            }

            set
            {
                this.cultureId = value;
                this.OnPropertyChanged(() => this.CultureId);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(ReminderFilterModel a, ReminderFilterModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<ReminderFilterModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(ReminderFilterModel a, ReminderFilterModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as ReminderFilterModel);
        }

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        private static bool CompareObjects(ReminderFilterModel a, ReminderFilterModel b)
        {
            return a.Id == b.Id;
        }
    }
}
