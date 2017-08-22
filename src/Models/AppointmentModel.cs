using System;
using System.ComponentModel.DataAnnotations;
using Contracts;

namespace Models
{
    /// <summary>
    /// Represents model class for appointment.
    /// </summary>
    public sealed class AppointmentModel : ModelBase2<AppointmentModel>, IModel
    {
        #region Private Fields

        private Guid id;

        private DateTime startTime;

        private DateTime endTime;

        private int item1Id;

        private string item1;

        private int item2Id;

        private string item2;

        private string comment;

        #endregion

        /// <summary>
        /// Gets or sets identification of the appointment.
        /// </summary>
        [Key]
        public Guid Id
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
        /// Gets or sets start time.
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }

            set
            {
                this.startTime = value;
                this.OnPropertyChanged(() => this.StartTime);
            }
        }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return this.endTime;
            }

            set
            {
                this.endTime = value;
                this.OnPropertyChanged(() => this.EndTime);
            }
        }

        /// <summary>
        /// Gets or sets identification of the doctor.
        /// </summary>
        public int Item1Id
        {
            get
            {
                return this.item1Id;
            }

            set
            {
                this.item1Id = value;
                this.OnPropertyChanged(() => this.Item1Id);
            }
        }

        /// <summary>
        /// Gets or sets the doctor.
        /// </summary>
        public string Item1
        {
            get
            {
                return this.item1;
            }

            set
            {
                this.item1 = value;
                this.OnPropertyChanged(() => this.Item1);
            }
        }

        /// <summary>
        /// Gets or sets identification of the patient.
        /// </summary>
        public int Item2Id
        {
            get
            {
                return this.item2Id;
            }

            set
            {
                this.item2Id = value;
                this.OnPropertyChanged(() => this.Item2Id);
            }
        }

        /// <summary>
        /// Gets or sets the patient.
        /// </summary>
        public string Item2
        {
            get
            {
                return this.item2;
            }

            set
            {
                this.item2 = value;
                this.OnPropertyChanged(() => this.Item2);
            }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [MaxLength(200)]
        public string Comment
        {
            get
            {
                return this.comment;
            }

            set
            {
                this.comment = value;
                this.OnPropertyChanged(() => this.Comment);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(AppointmentModel a, AppointmentModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<AppointmentModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(AppointmentModel a, AppointmentModel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Explicit casts object that implement IModel interface to the AppointmentCommon.
        /// </summary>
        /// <param name="model">IModel interface.</param>
        /// <returns>Returns the appointment.</returns>
        public static AppointmentModel FromModel(IModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var appointmentCommon = new AppointmentModel
            {
                Id = model.Id,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Comment = model.Comment,
                Item1Id = model.Item1Id,
                Item1 = model.Item1,
                Item2Id = model.Item2Id,
                Item2 = model.Item2
            };

            return appointmentCommon;
        }

        /// <summary>
        /// Override ToString method to represent state of the appointment.
        /// </summary>
        /// <returns>Returns string.</returns>
        public override string ToString()
        {
            return this.item2;
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Item1Id;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as AppointmentModel);
        }

        /// <summary>
        /// Map appointment information.
        /// </summary>
        /// <param name="copy">Target appointment.</param>
        /// <returns>Returns a new appointment.</returns>
        public AppointmentModel Map(AppointmentModel copy = null)
        {
            if (copy == null)
            {
                copy = new AppointmentModel();
            }

            copy.Id = this.Id;
            copy.StartTime = this.StartTime;
            copy.EndTime = this.EndTime;
            copy.Item1Id = this.Item1Id;
            copy.Item1 = this.Item1;
            copy.Item2Id = this.Item2Id;
            copy.Item2 = this.Item2;
            copy.Comment = this.Comment;

            base.Map(copy);

            return copy;
        }

        private static bool CompareObjects(AppointmentModel a, AppointmentModel b)
        {
            return (a.Id == b.Id) &&
                   (a.StartTime.TrimMilliseconds() == b.StartTime.TrimMilliseconds()) &&
                   (a.EndTime.TrimMilliseconds() == b.EndTime.TrimMilliseconds()) &&
                   (a.Item1Id == b.Item1Id) &&
                   (a.Item1 == b.Item1) &&
                   (a.Item2Id == b.Item2Id) &&
                   (a.Item2 == b.Item2) &&
                   (a.Comment == b.Comment) &&
                   (a.Created.TrimMilliseconds() == b.Created.TrimMilliseconds()) &&
                   (a.CreatedBy == b.CreatedBy) &&
                   (a.Changed.TrimMilliseconds() == b.Changed.TrimMilliseconds()) &&
                   (a.ChangedBy == b.ChangedBy);
        }
    }
}
