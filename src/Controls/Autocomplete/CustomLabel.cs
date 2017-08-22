using System.Windows;
using System.Windows.Controls;

namespace Autocomplete
{
    /// <summary>
    /// Represents Label with internal item.
    /// </summary>
    internal class CustomLabel : Label
    {
        /// <summary>
        /// Is mouse over.
        /// Original IsMouseOver can't be used, since it doesn't contain setter.
        /// </summary>
        public static readonly DependencyProperty IsMouseOver2Property =
            DependencyProperty.Register(
            "IsMouseOver2",
            typeof(bool),
            typeof(CustomLabel));

        /// <summary>
        /// Gets or sets internal item.
        /// </summary>
        public InternalItem Item { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is over.
        /// </summary>
        public bool IsMouseOver2
        {
            get
            {
                return (bool)this.GetValue(IsMouseOver2Property);
            }

            set
            {
                this.SetValue(IsMouseOver2Property, value);
            }
        }
    }
}
