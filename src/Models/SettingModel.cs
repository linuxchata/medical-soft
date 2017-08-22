using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Represents model for setting.
    /// </summary>
    [Table("Setting")]
    public sealed class SettingModel : ModelBase<SettingModel>
    {
        #region Private Fields

        /// <summary>
        /// Key of the setting.
        /// </summary>
        private string key;

        /// <summary>
        /// String value of the setting.
        /// </summary>
        private string stringValue;

        /// <summary>
        /// Integer value of the setting.
        /// </summary>
        private int? intValue;

        /// <summary>
        /// Boolean value of the setting.
        /// </summary>
        private bool? bitValue;

        #endregion

        /// <summary>
        /// Gets or sets key of the setting.
        /// </summary>
        [Key]
        [Required]
        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string NvKey
        {
            get
            {
                return this.key;
            }

            set
            {
                this.key = value;
                this.OnPropertyChanged(() => this.NvKey);
            }
        }

        /// <summary>
        /// Gets or sets string value of the setting.
        /// </summary>
        [Validatable]
        [MaxLength(100)]
        public string NvValue
        {
            get
            {
                return this.stringValue;
            }

            set
            {
                this.stringValue = value;
                this.OnPropertyChanged(() => this.NvValue);
            }
        }

        /// <summary>
        /// Gets or sets integer value of the setting.
        /// </summary>
        public int? IntValue
        {
            get
            {
                return this.intValue;
            }

            set
            {
                this.intValue = value;
                this.OnPropertyChanged(() => this.IntValue);
            }
        }

        /// <summary>
        /// Gets or sets boolean value of the setting.
        /// </summary>
        public bool? BitValue
        {
            get
            {
                return this.bitValue;
            }

            set
            {
                this.bitValue = value;
                this.OnPropertyChanged(() => this.BitValue);
            }
        }

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        public static bool operator ==(SettingModel a, SettingModel b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return ModelBase<SettingModel>.BaseEquals(a, b) && CompareObjects(a, b);
        }

        /// <summary>
        /// Override != operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects aren't equal, otherwise, false.</returns>
        public static bool operator !=(SettingModel a, SettingModel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.NvKey.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.ObjectEquals(obj) && CompareObjects(this, obj as SettingModel);
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
                case "NvValue":
                    if (this.stringValue.IsNullOrEmpty())
                    {
                        return "nvValue is required";
                    }

                    break;

                case "IntValue":
                    if (this.intValue == null)
                    {
                        return "intValue is required";
                    }

                    break;

                case "BitValue":
                    if (this.bitValue == null)
                    {
                        return "bitValue is required";
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
        private static bool CompareObjects(SettingModel a, SettingModel b)
        {
            return a.key == b.key;
        }
    }
}
