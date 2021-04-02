using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PenBody_CadUI
{
    /// <summary>
    /// Конвертер логического значения на противоположное.
    /// </summary>
    public class ReverseBoolConverter : IValueConverter
    {
        //TODO: CalcBinding
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
