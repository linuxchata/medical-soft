using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// Base class for model classes that add service fields.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public abstract class ModelBase2<TModel> : ModelBase<TModel>
        where TModel : class
    {
        #region Private Fields

        /// <summary>
        /// Date of creating an entry
        /// </summary>
        private DateTime created;

        /// <summary>
        /// Date of changing an entry
        /// </summary>
        private DateTime changed;

        /// <summary>
        /// User which create an entry
        /// </summary>
        private int createdBy;

        /// <summary>
        /// User which changed an entry
        /// </summary>
        private int changedBy;

        #endregion

        /// <summary>
        /// Gets or sets date of creating an entry
        /// </summary>
        [Required]
        public DateTime Created
        {
            get
            {
                return this.created;
            }

            set
            {
                this.created = value;
                this.OnPropertyChanged(() => this.Created);
            }
        }

        /// <summary>
        /// Gets or sets date of changing an entry
        /// </summary>
        [Required]
        public DateTime Changed
        {
            get
            {
                return this.changed;
            }

            set
            {
                this.changed = value;
                this.OnPropertyChanged(() => this.Changed);
            }
        }

        /// <summary>
        /// Gets or sets user which create an entry
        /// </summary>
        [Required]
        public int CreatedBy
        {
            get
            {
                return this.createdBy;
            }

            set
            {
                this.createdBy = value;
                this.OnPropertyChanged(() => this.CreatedBy);
            }
        }

        /// <summary>
        /// Gets or sets user which changed an entry
        /// </summary>
        [Required]
        public int ChangedBy
        {
            get
            {
                return this.changedBy;
            }

            set
            {
                this.changedBy = value;
                this.OnPropertyChanged(() => this.ChangedBy);
            }
        }

        /// <summary>
        /// Map service fields information.
        /// </summary>
        /// <param name="copy">Copy of the object.</param>
        /// <returns>Returns copied object.</returns>
        protected ModelBase2<TModel> Map(ModelBase2<TModel> copy)
        {
            copy.Created = this.Created;
            copy.Changed = this.Changed;
            copy.CreatedBy = this.CreatedBy;
            copy.ChangedBy = this.ChangedBy;

            return copy;
        }
    }
}
