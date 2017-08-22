using System;
using System.Windows.Data;
using Common.Enumeration;

namespace Common.Converter
{
    /// <summary>
    /// Status to boolean converter.
    /// </summary>
    public class StatusToBooleanConverter : IValueConverter
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
            LoadingStatus enumValue;
            if (Enum.TryParse(value.ToString(), out enumValue))
            {
                switch (enumValue)
                {
                    case LoadingStatus.Canceled:
                    case LoadingStatus.Failed:
                    case LoadingStatus.Loaded:
                    case LoadingStatus.Unknown:
                        return true;
                    case LoadingStatus.Added:
                    case LoadingStatus.Loading:
                    case LoadingStatus.Updated:
                        return false;
                }
            }

            return true;
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
