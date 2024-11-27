using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Converters
{
    public class ZeroToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Wenn der Wert 0 oder null ist, wird Collapsed zurückgegeben
            if (value is double doubleValue && doubleValue == 0)
                return Visibility.Collapsed;

            if (value is int intValue && intValue == 0)
                return Visibility.Collapsed;

            if (value == null || (value is System.Collections.IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext()))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
