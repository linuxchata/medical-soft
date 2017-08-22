using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Models.Attribute;

namespace Models
{
    /// <summary>
    /// Base class for model that implement INotifyPropertyChanged and IDataErrorInfo interface.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public abstract class ModelBase<TModel> : INotifyPropertyChanged, IDataErrorInfo
        where TModel : class
    {
        /// <summary>
        /// List of the validation properties.
        /// </summary>
        private IList<PropertyInfo> validatableProperties;

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets error.
        /// </summary>
        public string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object hasn't validation errors.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return this.ValidatableProperties.All(property => this.GetValidationError(property.Name) == null);
            }
        }

        /// <summary>
        /// Gets list of the validation properties.
        /// </summary>
        protected IList<PropertyInfo> ValidatableProperties
        {
            get
            {
                if (this.validatableProperties == null)
                {
                    this.validatableProperties = this.GetType().GetProperties()
                        .Where(p => p.IsDefined(typeof(ValidatableAttribute), true)).ToList();
                }

                return this.validatableProperties;
            }
        }

        /// <summary>
        /// The indexer.
        /// </summary>
        /// <param name="columnName">Validated property.</param>
        /// <returns>Returns error string.</returns>
        public string this[string columnName]
        {
            get
            {
                return this.GetValidationError(columnName);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Method to raise PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var memberExpression = (MemberExpression)propertyName.Body;
                this.PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }

        #endregion

        /// <summary>
        /// Override == operator.
        /// </summary>
        /// <param name="a">The object A.</param>
        /// <param name="b">The object B.</param>
        /// <returns>Return true if objects are equal, otherwise, false.</returns>
        protected static bool BaseEquals(TModel a, TModel b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (a == null || b == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get validated error for current model.
        /// </summary>
        /// <param name="columnName">Validated property.</param>
        /// <returns>Returns validation error if any, otherwise, null.</returns>
        protected virtual string GetValidationError(string columnName)
        {
            return null;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object otherwise, false.</returns>
        protected bool ObjectEquals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast return false.
            var casted = obj as TModel;
            if (casted == null)
            {
                return false;
            }

            return true;
        }
    }
}
