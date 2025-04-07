using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToolBox_Pro.Converters
{
    public class TimeSpanToReadableTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan ts)
            {
                var totalMinutes = (int)Math.Round(ts.TotalMinutes);
                var hours = totalMinutes / 60;
                var minutes = Math.Abs(totalMinutes % 60);
                return $"{(ts.TotalMinutes < 0 ? "-" : "")}{Math.Abs(hours):D2}:{minutes:D2}";
            }

            return "00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
