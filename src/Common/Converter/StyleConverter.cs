using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converter
{
    /// <summary>
    /// Style converter.
    /// </summary>
    public class StyleConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convert method.
        /// </summary>
        /// <param name="values">The value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var targetElement = values[0] as FrameworkElement;
            var styleName = values[1] as string;

            if (styleName == null)
            {
                return null;
            }

            var style = (Style)targetElement.TryFindResource(styleName);

            if (style == null)
            {
                throw new ArgumentNullException("values", string.Format("Style {0} was not found by StyleConverter", styleName));
            }

            return style;
        }

        /// <summary>
        /// Convert back method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetTypes">Target types.</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Return converted object.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
