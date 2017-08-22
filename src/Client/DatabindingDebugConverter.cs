using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Client
{
    /// <summary>
    /// This converter does nothing except breaking the debugger into the convert method.
    /// </summary>
    public class DatabindingDebugConverter : IValueConverter
    {
        /// <summary>
        /// Convert method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }

        /// <summary>
        /// Convert back method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }
    }
}
