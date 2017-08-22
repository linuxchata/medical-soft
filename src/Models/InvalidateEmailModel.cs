using System;
using System.Linq;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model class for invalidate email.
    /// </summary>
    public sealed class InvalidateEmailModel : ModelBase<InvalidateEmailModel>
    {
        #region Private Fields

        /// <summary>
        /// List of all e-mail to invalidate.
        /// </summary>
        private string emails;

        /// <summary>
        /// Amount of e-mails to invalidate.
        /// </summary>
        private int amount;

        #endregion

        /// <summary>
        /// Gets or sets list of all e-mail to invalidate.
        /// </summary>
        [Validatable]
        public string Emails
        {
            get
            {
                return this.emails;
            }

            set
            {
                this.emails = value;

                this.Amount = this.Emails.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Count;
                this.OnPropertyChanged(() => this.Emails);
            }
        }

        /// <summary>
        /// Gets or sets amount of e-mails to invalidate.
        /// </summary>
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
                this.OnPropertyChanged(() => this.Amount);
            }
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
                case "Emails":
                    if (this.Emails.IsNullOrEmpty())
                    {
                        return "Emails are required";
                    }

                    break;

                default:
                    throw new ArgumentException("Unexpected property being validated " + columnName);
            }

            return null;
        }
    }
}
