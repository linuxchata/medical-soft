using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for notification template.
    /// </summary>
    [Table("NotificationTemplate")]
    public sealed class NotificationTemplateModel : ModelBase2<NotificationTemplateModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the template.
        /// </summary>
        private int id;

        /// <summary>
        /// Template's description.
        /// </summary>
        private string description;

        /// <summary>
        /// Message's title.
        /// </summary>
        private string title;

        /// <summary>
        /// Message's body.
        /// </summary>
        private string body;

        /// <summary>
        /// Is reminder was deleted.
        /// </summary>
        private bool isDeleted;

        #endregion

        /// <summary>
        /// Gets or sets identification of the template.
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
        /// Gets or sets template's description.
        /// </summary>
        [Validatable]
        [MaxLength(1024)]
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
        /// Gets or sets message's title.
        /// </summary>
        [Validatable]
        [MaxLength(1024)]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.OnPropertyChanged(() => this.Title);
            }
        }

        /// <summary>
        /// Gets or sets message's body.
        /// </summary>
        [MaxLength(4096)]
        public string Body
        {
            get
            {
                return this.body;
            }

            set
            {
                this.body = value;
                this.OnPropertyChanged(() => this.Body);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether template is deleted.
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
        public static bool operator ==(NotificationTemplateModel a, NotificationTemplateModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<NotificationTemplateModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(NotificationTemplateModel a, NotificationTemplateModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as NotificationTemplateModel);
        }

        /// <summary>
        /// Map template information.
        /// </summary>
        /// <param name="copy">Target template.</param>
        /// <returns>Returns a new template.</returns>
        public NotificationTemplateModel Map(NotificationTemplateModel copy = null)
        {
            if (copy == null)
            {
                copy = new NotificationTemplateModel();
            }

            copy.Id = this.Id;
            copy.Description = this.Description;
            copy.Title = this.Title;
            copy.Body = this.Body;
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
                case "Description":
                    if (this.Description.IsNullOrEmpty())
                    {
                        return "Description is required";
                    }

                    break;

                case "Title":
                    if (this.Title.IsNullOrEmpty())
                    {
                        return "Title is required";
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
        private static bool CompareObjects(NotificationTemplateModel a, NotificationTemplateModel b)
        {
            return a.Id == b.Id;
        }
    }
}