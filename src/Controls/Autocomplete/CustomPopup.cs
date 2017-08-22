using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Autocomplete
{
    /// <summary>
    /// Represents custom popup.
    /// </summary>
    public class CustomPopup : Popup
    {
        /// <summary>
        /// Initializes static members of the CustomPopup class.
        /// </summary>
        static CustomPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomPopup), new FrameworkPropertyMetadata(typeof(CustomPopup)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPopup"/> class.
        /// </summary>
        public CustomPopup()
        {
            var baseType = this.GetType().BaseType;
            var popupSecHelper = this.GetHiddenField(this, baseType, "_secHelper");
            this.SetHiddenField(popupSecHelper, "_isChildPopupInitialized", true);
            this.SetHiddenField(popupSecHelper, "_isChildPopup", true);
        }

        /// <summary>
        /// Get hidden field.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>Returns hidden field.</returns>
        protected dynamic GetHiddenField(object container, string fieldName)
        {
            return this.GetHiddenField(container, container.GetType(), fieldName);
        }

        /// <summary>
        /// Get hidden field.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerType">The container type.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>Returns hidden field.</returns>
        protected dynamic GetHiddenField(object container, Type containerType, string fieldName)
        {
            dynamic retVal = null;
            var fieldInfo = containerType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                retVal = fieldInfo.GetValue(container);
            }

            return retVal;
        }

        /// <summary>
        /// Set hidden field.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="value">The value.</param>
        protected void SetHiddenField(object container, string fieldName, object value)
        {
            this.SetHiddenField(container, container.GetType(), fieldName, value);
        }

        /// <summary>
        /// Set hidden field.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerType">The container type.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="value">The value.</param>
        protected void SetHiddenField(object container, Type containerType, string fieldName, object value)
        {
            var fieldInfo = containerType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(container, value);
            }
        }
    }
}
