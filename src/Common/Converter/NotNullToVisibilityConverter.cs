using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converter
{
    /// <summary>
    /// Not null to visibility converter.
    /// </summary>
    public class NotNullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Convert back method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
