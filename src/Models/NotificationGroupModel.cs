using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;
using Models.Enumeration;

namespace Models
{
    /// <summary>
    /// Represents model class for notification group.
    /// </summary>
    [Table("NotificationGroup")]
    public sealed class NotificationGroupModel : ModelBase2<NotificationGroupModel>
    {
        #region Private Fields

        /// <summary>
        /// Identification of the group.
        /// </summary>
        private int id;

        /// <summary>
        /// Unique identifier of the group.
        /// </summary>
        private Guid uniqueId;

        /// <summary>
        /// Description of the group.
        /// </summary>
        private string description;

        /// <summary>
        /// Template id.
        /// </summary>
        private int templateId;

        /// <summary>
        /// Name of the template.
        /// </summary>
        private string template;

        /// <summary>
        /// Start date of the notification group.
        /// </summary>
        private DateTime startDate;

        /// <summary>
        /// The status.
        /// </summary>
        private int status;

        /// <summary>
        /// Status name.
        /// </summary>
        private string statusName;

        /// <summary>
        /// Completed date of the notification group.
        /// </summary>
        private DateTime? completedDate;

        /// <summary>
        /// Is notification group was deleted.
        /// </summary>
        private bool isDeleted;

        /// <summary>
        /// Gets or sets result of the group handling.
        /// </summary>
        private string result;

        #endregion

        /// <summary>
        /// Gets or sets identification of the group.
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
        /// Gets or sets unique identifier of the group.
        /// </summary>
        [Required]
        public Guid UniqueId
        {
            get
            {
                return this.uniqueId;
            }

            set
            {
                this.uniqueId = value;
                this.OnPropertyChanged(() => this.UniqueId);
            }
        }

        /// <summary>
        /// Gets or sets description of the group.
        /// </summary>
        [Validatable]
        [Required]
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
        /// Gets or sets template id.
        /// </summary>
        [Validatable]
        [Required]
        public int TemplateId
        {
            get
            {
                return this.templateId;
            }

            set
            {
                this.templateId = value;
                this.OnPropertyChanged(() => this.TemplateId);
            }
        }

        /// <summary>
        /// Gets or sets name of the template.
        /// </summary>
        [NotMapped]
        public string Template
        {
            get
            {
                return this.template;
            }

            set
            {
                this.template = value;
                this.OnPropertyChanged(() => this.Template);
            }
        }

        /// <summary>
        /// Gets or sets start date of the notification group.
        /// </summary>
        [Required]
        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }

            set
            {
                this.startDate = value;
                this.OnPropertyChanged(() => this.StartDate);
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [Required]
        public int Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.OnPropertyChanged(() => this.Status);
                this.OnPropertyChanged(() => this.IsNotProcessing);
                this.OnPropertyChanged(() => this.IsNotProcessed);
            }
        }

        /// <summary>
        /// Gets or sets status name.
        /// </summary>
        [NotMapped]
        public string StatusName
        {
            get
            {
                return this.statusName;
            }

            set
            {
                this.statusName = value;
                this.OnPropertyChanged(() => this.StatusName);
            }
        }

        /// <summary>
        /// Gets or sets completed date of the notification group.
        /// </summary>
        [Required]
        public DateTime? CompletedDate
        {
            get
            {
                return this.completedDate;
            }

            set
            {
                this.completedDate = value;
                this.OnPropertyChanged(() => this.CompletedDate);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether group is deleted.
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
        /// Gets or sets result of the group handling.
        /// </summary>
        [NotMapped]
        public string Result
        {
            get
            {
                return this.result;
            }

            set
            {
                this.result = value;
                this.OnPropertyChanged(() => this.Result);
            }
        }

        /// <summary>
        /// Gets a value indicating whether group not processing.
        /// </summary>
        [NotMapped]
        public bool IsNotProcessing
        {
            get
            {
                var resultedStatus = this.Status != (int)NotificationGroupStatus.Processing;
                return resultedStatus;
            }
        }

        /// <summary>
        /// Gets a value indicating whether group not processed.
        /// </summary>
        [NotMapped]
        public bool IsNotProcessed
        {
            get
            {
                var resultedStatus = this.Status == (int)NotificationGroupStatus.NotProcessed || this.Status == (int)NotificationGroupStatus.Cancelled;
                return resultedStatus;
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(NotificationGroupModel a, NotificationGroupModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<NotificationGroupModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(NotificationGroupModel a, NotificationGroupModel b)
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
            return this.ObjectEquals(obj) && CompareObjects(this, obj as NotificationGroupModel);
        }

        /// <summary>
        /// Map group information.
        /// </summary>
        /// <param name="copy">Target group.</param>
        /// <returns>Returns a new group.</returns>
        public NotificationGroupModel Map(NotificationGroupModel copy = null)
        {
            if (copy == null)
            {
                copy = new NotificationGroupModel();
            }

            copy.Id = this.Id;
            copy.UniqueId = this.UniqueId;
            copy.Description = this.Description;
            copy.TemplateId = this.TemplateId;
            copy.Template = this.Template;
            copy.StartDate = this.StartDate;
            copy.Status = this.Status;
            copy.StatusName = this.StatusName;
            copy.CompletedDate = this.CompletedDate;
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

                case "TemplateId":
                    if (this.TemplateId == 0)
                    {
                        return "Template is required";
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
        private static bool CompareObjects(NotificationGroupModel a, NotificationGroupModel b)
        {
            return a.Id == b.Id;
        }
    }
}