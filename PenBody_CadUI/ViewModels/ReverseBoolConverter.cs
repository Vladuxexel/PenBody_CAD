using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PenBody_CadUI
{
    public class ReverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Метод конвертации.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// Метод обратной конвертации.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
